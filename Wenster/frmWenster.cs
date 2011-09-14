using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wenster.Entity;
using Wenster.DataProvider;
using System.Collections.ObjectModel;
using System.Threading;

namespace Wenster
{
    public partial class frmWenster : Form
    {
        private delegate void CrossThreadOperate();

        //Alpha,Beta,Gamma取值范围 * 100

        private const int fltMin = 1;
        private const int fltMax = 99;

        //预测的来源数据
        private List<Production> initDataList = new List<Production>();

        //所要预测的实际数据
        private List<Production> realDataList = new List<Production>();

        //存放实时运算的最优结果集
        private Result minResult = new Result();

        //周期的天数
        private int intLetterL = 0;

        //要预测的周期
        private int intDataCount = 0;

        //计算中用到的S、F、B
        private List<Variable> collectionS = new List<Variable>();
        private List<Variable> collectionB = new List<Variable>();
        private List<Variable> collectionF = new List<Variable>();

        //已经循环计算的次数
        private long lngCalcNumber = 0;

        public frmWenster()
        {
            InitializeComponent();
        }

        private void frmWenster_Load(object sender, EventArgs e)
        {
            this.txtLetterL.Clear();
            this.txtInitDataCount.Clear();
            this.txtInitDataCount.Width = this.txtLetterL.Width;
            this.txtLetterL.Focus();
            this.tssProcessValue.Text = "已准备就绪";
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            //进行计算前的输入值检查
            WensterInformation wi = new WensterInformation();
            wi = ValidInputInformation();
            if (!wi.ProcessResult)
            {
                MessageBox.Show(wi.ProcessInfomation, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //获取数据量
            initDataList = new Provider().GetInitializeData(intLetterL).OrderBy<Production, int>(idl => idl.ID).ToList<Production>();
            realDataList = new Provider().GetRealData(intLetterL).OrderBy<Production,int>(rdl=>rdl.ID).ToList<Production>();

            //计算
            tssProcessValue.Text = "正进行计算";
            gbSetting.Enabled = false;
            bwProcessor.RunWorkerAsync();

        }

        /// <summary>
        /// 检查初始化的数据
        /// </summary>
        /// <returns>WensterInformation对象</returns>
        private WensterInformation ValidInputInformation()
        {
            
            WensterInformation wi = new WensterInformation();

            //检查季节性周期L 
            if (string.IsNullOrEmpty(txtLetterL.Text) 
                || !int.TryParse(txtLetterL.Text,out intLetterL) 
                || intLetterL == 0)
            {
                wi.ProcessResult = false;
                wi.ProcessInfomation = "季节性周期的值为空，或者不是数字！";
                return wi;
            }

            //检查初始数据量
            if (string.IsNullOrEmpty(txtInitDataCount.Text) 
                || !int.TryParse(txtInitDataCount.Text, out intDataCount) 
                || intDataCount == 0)
            {
                wi.ProcessResult = false;
                wi.ProcessInfomation = "初始数据量为空，或者不是数字，或者比季节性周期L要小！";
                return wi;
            }

            wi.ProcessResult = true;
            return wi;
        }

        private void bwProcessor_DoWork(object sender, DoWorkEventArgs e)
        {
            minResult.Prediction = new List<Variable>();
            long lngTotalNum = fltMax * fltMax * fltMax;
            //计算S(t)的初始值
            Variable vS = new Variable();
            vS.Pos = initDataList[intLetterL].ID;
            vS.Number = initDataList[intLetterL].Number;
            collectionS.Add(vS);

            //先计算平均值Y
            double dblAverY = 0.00;
            for (int index = 0; index <= intLetterL; index++)
            {
                dblAverY += initDataList[index].Number;
            }
            dblAverY = dblAverY / (intLetterL + 1);

            //然后求出F(t)
            for (int index = 0; index <= intLetterL; index++)
            {
                Variable vF = new Variable();
                vF.Pos = initDataList[index].ID;
                vF.Number = initDataList[index].Number / dblAverY;
                collectionF.Add(vF);
            }

            //然后求出B(t)
            Variable vB = new Variable();
            vB.Pos = initDataList[intLetterL].ID;
            vB.Number = 0.00;
            for (int index = 0; index < intLetterL -1; index++)
            {
                vB.Number += initDataList[intLetterL + index].Number - initDataList[index].Number;
            }
            vB.Number = vB.Number / ((intLetterL -1) * intLetterL);
            collectionB.Add(vB);

            double realAlpha = 0.00;
            double realBeta = 0.00;
            double realGamma = 0.00;
            
            //遍历取Alpha,Beta,Gamma，以Alpha作为最外层元素
            bool blnFlush = false;

            for (double alpha = fltMin; alpha <= fltMax; alpha++)
            {
                for (double beta = fltMin; beta <= fltMax; beta++)
                {
                    CleanCollection(9, 28);

                    for (double gamma = fltMin; gamma <= fltMax; gamma++)
                    {
                        realAlpha = alpha / 100;
                        realBeta = beta / 100;
                        realGamma = gamma / 100;

                        lngCalcNumber++;

                        blnFlush = false;

                        //求S9~S28
                        for (int index = intLetterL + 1; index <= (intDataCount - 1) * intLetterL - 1; index++)
                        {
                            //求S(index)
                            Variable loopS = new Variable();
                            loopS.Pos = index + 1;
                            loopS.Number = realAlpha * (initDataList[index].Number / GetCollectionRecordValue(collectionF, index - intLetterL + 1))
                                + (1 - realAlpha) * (GetCollectionRecordValue(collectionS, index) + GetCollectionRecordValue(collectionB, index));
                            collectionS.Add(loopS);

                            //求B(index)
                            Variable loopB = new Variable();
                            loopB.Pos = index + 1;
                            loopB.Number = realBeta * (GetCollectionRecordValue(collectionS, index + 1) - GetCollectionRecordValue(collectionS, index))
                                + (1 - realBeta) * GetCollectionRecordValue(collectionB, index);
                            collectionB.Add(loopB);

                            //求F(index)
                            Variable loopF = new Variable();
                            loopF.Pos = index + 1;
                            loopF.Number = realGamma * (initDataList[index].Number / GetCollectionRecordValue(collectionS, index + 1))
                                + (1 - realGamma) * GetCollectionRecordValue(collectionF, index - intLetterL + 1);
                            collectionF.Add(loopF);
                        }

                        //求指定周期的预测数据
                        List<Variable> preNumber = new List<Variable>();
                        for (int index = (intDataCount - 1) * intLetterL + 1; index <= intDataCount * intLetterL; index++)
                        {
                            Variable preY = new Variable();
                            preY.Pos = index;
                            preY.Number = (GetCollectionRecordValue(collectionS,(intDataCount - 1) * intLetterL) 
                                +GetCollectionRecordValue(collectionB,(intDataCount - 1) * intLetterL) * (index - (intDataCount - 1) * intLetterL))
                                * GetCollectionRecordValue(collectionF, index - intLetterL);
                            preNumber.Add(preY);
                        }

                        //计算误差值，存储当前的Alpha、Beta、Gamma
                        double deviation = 0.00;
                        preNumber = preNumber.OrderBy<Variable, int>(var => var.Pos).ToList();
                        foreach (Variable item in preNumber)
                        {
                            double itemDeviation = 0.00;
                            itemDeviation = item.Number - realDataList[item.Pos - (intDataCount - 1) * intLetterL - 1].Number;
                            itemDeviation = Math.Abs(itemDeviation) * 100;
                            deviation += itemDeviation / realDataList[item.Pos - (intDataCount - 1) * intLetterL - 1].Number;
                        }
                        deviation = deviation / intLetterL;

                        if (minResult.Prediction.Count == 0 || deviation < minResult.Average)
                        {
                            minResult.Alpha = realAlpha;
                            minResult.Beta = realBeta;
                            minResult.Gamma = realGamma;
                            minResult.Average = deviation;
                            minResult.Prediction = preNumber;

                            blnFlush = true;
                        }

                        //当前最优解
                        if (blnFlush)
                        {
                            AddResultToListView();

                        }
                        
                        //显示进度
                        tssProcessValue.Text = "正在计算中";
                        bwProcessor.ReportProgress(Convert.ToInt32(alpha) + 1);
                        this.tspProgressValue.Text = lngCalcNumber + " / " + lngTotalNum;
                        this.lblAlphaValue.Text = "Alpha=" + realAlpha + " | 平均数=" + deviation;
                         
                    }
                }
            }
        }

        //将结果显示到ListView控件
        private void AddResultToListView()
        {
            //声明委托
            CrossThreadOperate cto = delegate()
            {
                this.lvwResult.Items.Clear();
                ListViewItem item = new ListViewItem();
                item.Text = this.lvwResult.Items.Count + 1 + "";
                item.SubItems.AddRange(new string[] { "Alpha值", minResult.Alpha + "" });
                this.lvwResult.Items.Add(item);

                item = new ListViewItem();
                item.Text = this.lvwResult.Items.Count + 1 + "";
                item.SubItems.AddRange(new string[] { "Beta值", minResult.Beta + "" });
                this.lvwResult.Items.Add(item);

                item = new ListViewItem();
                item.Text = this.lvwResult.Items.Count + 1 + "";
                item.SubItems.AddRange(new string[] { "Gamma值", minResult.Gamma + "" });
                this.lvwResult.Items.Add(item);

                item = new ListViewItem();
                item.Text = this.lvwResult.Items.Count + 1 + "";
                item.SubItems.AddRange(new string[] { "平均误差值", minResult.Average + "" });
                this.lvwResult.Items.Add(item);

                foreach (Variable var in minResult.Prediction)
                {
                    item = new ListViewItem();
                    item.Text = this.lvwResult.Items.Count + 1 + "";
                    item.SubItems.AddRange(new string[] { "第" + intDataCount + "周第" + (var.Pos - (intDataCount - 1) * intLetterL) + "天", var.Number + "" });
                    this.lvwResult.Items.Add(item);
                }
            };
            //利用invoke转发委托
            this.Invoke(cto);
        }

        //清除集合中的数据
        private void CleanCollection(int minPos,int maxPos)
        {
            List<Variable> tmpCollectionB = new List<Variable>();
            foreach (Variable item in collectionB)
            {
                if (item.Pos >= minPos && item.Pos <= maxPos)
                {
                    continue;
                }
                else
                {
                    tmpCollectionB.Add(item);
                }
            }
            collectionB = new List<Variable>();
            collectionB = tmpCollectionB;

            List<Variable> tmpCollectionF = new List<Variable>();
            foreach (Variable item in collectionF)
            {
                if (item.Pos >= minPos && item.Pos <= maxPos)
                {
                    continue;
                }
                else
                {
                    tmpCollectionF.Add(item);
                }
            }
            collectionF = new List<Variable>();
            collectionF = tmpCollectionF;

            List<Variable> tmpCollectionS = new List<Variable>();
            foreach (Variable item in collectionS)
            {
                if (item.Pos >= minPos && item.Pos <= maxPos)
                {
                    continue;
                }
                else
                {
                    tmpCollectionS.Add(item);
                }
            }
            collectionS = new List<Variable>();
            collectionS = tmpCollectionS;
        }
        

       //根据序号获取集合中数据值
        private double GetCollectionRecordValue(List<Variable> items, int posNumber)
        {
            foreach (Variable item in items)
            {
                if (item.Pos == posNumber)
                {
                    return item.Number;
                }
            }
            return 0.00;
        }

        private void bwProcessor_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tssProcessValue.Text = "计算已完成";
            gbSetting.Enabled = true;
        }

        private void bwProcessor_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.tspProgress.Value = e.ProgressPercentage;
        }

        private void statusBar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void lblLetterL_Click(object sender, EventArgs e)
        {

        }

        private void txtLetterL_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblInitDataCount_Click(object sender, EventArgs e)
        {

        }

        private void txtInitDataCount_TextChanged(object sender, EventArgs e)
        {

        }

        private void lvwResult_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tspProgress_Click(object sender, EventArgs e)
        {

        }

        private void tssAppName_Click(object sender, EventArgs e)
        {

        }
    }
}
