using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Wenster.Entity;
using Wenster.DataProvider;
using Wenster.Properties;

namespace Wenster
{
    public partial class frmWenster : Form
    {
        private delegate void CrossThreadOperate();

        //Alpha,Beta,Gamma取值范围 * 100

        private const int FltMin = 1;
        private const int FltMax = 99;

        //预测的来源数据
        private List<Production> _initDataList = new List<Production>();

        //所要预测的实际数据
        private List<Production> _realDataList = new List<Production>();

        //存放实时运算的最优结果集
        private readonly Result _minResult = new Result();

        //周期的天数
        private int _intLetterL = 0;

        //要预测的周期
        private int _intDataCount = 0;

        //计算中用到的S、F、B
        private List<Variable> _collectionS = new List<Variable>();
        private List<Variable> _collectionB = new List<Variable>();
        private List<Variable> _collectionF = new List<Variable>();

        //已经循环计算的次数
        private long _lngCalcNumber = 0;

        public frmWenster()
        {
            InitializeComponent();
        }

        private void FrmWensterLoad(object sender, EventArgs e)
        {
            txtLetterL.Clear();
            txtInitDataCount.Clear();
            txtInitDataCount.Width = txtLetterL.Width;
            txtLetterL.Focus();
            tssProcessValue.Text = Resources.CALC_READY;
        }

        private void BtnCalcClick(object sender, EventArgs e)
        {
            //进行计算前的输入值检查
            var wi = ValidInputInformation();
            if (!wi.ProcessResult)
            {
                MessageBox.Show(wi.ProcessInfomation, Resources.MSG_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //获取数据量
            _initDataList = new Provider().GetInitializeData(_intLetterL).OrderBy(idl => idl.ID).ToList();
            _realDataList = new Provider().GetRealData(_intLetterL).OrderBy(rdl=>rdl.ID).ToList();

            //计算
            tssProcessValue.Text = Resources.CALC_ING;
            gbSetting.Enabled = false;
            bwProcessor.RunWorkerAsync();

        }

        /// <summary>
        /// 检查初始化的数据
        /// </summary>
        /// <returns>WensterInformation对象</returns>
        private WensterInformation ValidInputInformation()
        {
            
            var wi = new WensterInformation();

            //检查季节性周期L 
            if (string.IsNullOrEmpty(txtLetterL.Text) 
                || !int.TryParse(txtLetterL.Text,out _intLetterL) 
                || _intLetterL == 0)
            {
                wi.ProcessResult = false;
                wi.ProcessInfomation = "季节性周期的值为空，或者不是数字！";
                return wi;
            }

            //检查初始数据量
            if (string.IsNullOrEmpty(txtInitDataCount.Text) 
                || !int.TryParse(txtInitDataCount.Text, out _intDataCount) 
                || _intDataCount == 0)
            {
                wi.ProcessResult = false;
                wi.ProcessInfomation = "初始数据量为空，或者不是数字，或者比季节性周期L要小！";
                return wi;
            }

            wi.ProcessResult = true;
            return wi;
        }

        private void BwProcessorDoWork(object sender, DoWorkEventArgs e)
        {
            _minResult.Prediction = new List<Variable>();
            const long lngTotalNum = FltMax * FltMax * FltMax;
            //计算S(t)的初始值
            var vS = new Variable {Pos = _initDataList[_intLetterL].ID, Number = _initDataList[_intLetterL].Number};
            _collectionS.Add(vS);

            //先计算平均值Y
            var dblAverY = 0.00;
            for (var index = 0; index <= _intLetterL; index++)
            {
                dblAverY += _initDataList[index].Number;
            }
            dblAverY = dblAverY / (_intLetterL + 1);

            //然后求出F(t)
            for (var index = 0; index <= _intLetterL; index++)
            {
                var vF = new Variable {Pos = _initDataList[index].ID, Number = _initDataList[index].Number/dblAverY};
                _collectionF.Add(vF);
            }

            //然后求出B(t)
            var vB = new Variable {Pos = _initDataList[_intLetterL].ID, Number = 0.00};
            for (var index = 0; index < _intLetterL -1; index++)
            {
                vB.Number += _initDataList[_intLetterL + index].Number - _initDataList[index].Number;
            }
            vB.Number = vB.Number / ((_intLetterL -1) * _intLetterL);
            _collectionB.Add(vB);

            //遍历取Alpha,Beta,Gamma，以Alpha作为最外层元素

            for (double alpha = FltMin; alpha <= FltMax; alpha++)
            {
                for (double beta = FltMin; beta <= FltMax; beta++)
                {
                    CleanCollection(9, 28);

                    for (double gamma = FltMin; gamma <= FltMax; gamma++)
                    {
                        var realAlpha = alpha / 100;
                        var realBeta = beta / 100;
                        var realGamma = gamma / 100;

                        _lngCalcNumber++;

                        var blnFlush = false;

                        //求S9~S28
                        for (var index = _intLetterL + 1; index <= (_intDataCount - 1) * _intLetterL - 1; index++)
                        {
                            //求S(index)
                            var loopS = new Variable
                                            {
                                                Pos = index + 1,
                                                Number =
                                                    realAlpha*
                                                    (_initDataList[index].Number/
                                                     GetCollectionRecordValue(_collectionF, index - _intLetterL + 1))
                                                    +
                                                    (1 - realAlpha)*
                                                    (GetCollectionRecordValue(_collectionS, index) +
                                                     GetCollectionRecordValue(_collectionB, index))
                                            };
                            _collectionS.Add(loopS);

                            //求B(index)
                            var loopB = new Variable
                                            {
                                                Pos = index + 1,
                                                Number =
                                                    realBeta*
                                                    (GetCollectionRecordValue(_collectionS, index + 1) -
                                                     GetCollectionRecordValue(_collectionS, index))
                                                    + (1 - realBeta)*GetCollectionRecordValue(_collectionB, index)
                                            };
                            _collectionB.Add(loopB);

                            //求F(index)
                            var loopF = new Variable
                                            {
                                                Pos = index + 1,
                                                Number =
                                                    realGamma*
                                                    (_initDataList[index].Number/
                                                     GetCollectionRecordValue(_collectionS, index + 1))
                                                    +
                                                    (1 - realGamma)*
                                                    GetCollectionRecordValue(_collectionF, index - _intLetterL + 1)
                                            };
                            _collectionF.Add(loopF);
                        }

                        //求指定周期的预测数据
                        var preNumber = new List<Variable>();
                        for (var index = (_intDataCount - 1) * _intLetterL + 1; index <= _intDataCount * _intLetterL; index++)
                        {
                            var preY = new Variable
                                           {
                                               Pos = index,
                                               Number =
                                                   (GetCollectionRecordValue(_collectionS,
                                                                             (_intDataCount - 1)*_intLetterL)
                                                    +
                                                    GetCollectionRecordValue(_collectionB,
                                                                             (_intDataCount - 1)*_intLetterL)*
                                                    (index - (_intDataCount - 1)*_intLetterL))
                                                   *GetCollectionRecordValue(_collectionF, index - _intLetterL)
                                           };
                            preNumber.Add(preY);
                        }

                        //计算误差值，存储当前的Alpha、Beta、Gamma
                        var deviation = 0.00;
                        preNumber = preNumber.OrderBy(var => var.Pos).ToList();
                        foreach (var item in preNumber)
                        {
                            var itemDeviation = 0.00;
                            itemDeviation = item.Number - _realDataList[item.Pos - (_intDataCount - 1) * _intLetterL - 1].Number;
                            itemDeviation = Math.Abs(itemDeviation) * 100;
                            deviation += itemDeviation / _realDataList[item.Pos - (_intDataCount - 1) * _intLetterL - 1].Number;
                        }
                        deviation = deviation / _intLetterL;

                        if (_minResult.Prediction.Count == 0 || deviation < _minResult.Average)
                        {
                            _minResult.Alpha = realAlpha;
                            _minResult.Beta = realBeta;
                            _minResult.Gamma = realGamma;
                            _minResult.Average = deviation;
                            _minResult.Prediction = preNumber;

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
                        this.tspProgressValue.Text = _lngCalcNumber + " / " + lngTotalNum;
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
                item.SubItems.AddRange(new string[] { "Alpha值", _minResult.Alpha + "" });
                this.lvwResult.Items.Add(item);

                item = new ListViewItem();
                item.Text = this.lvwResult.Items.Count + 1 + "";
                item.SubItems.AddRange(new string[] { "Beta值", _minResult.Beta + "" });
                this.lvwResult.Items.Add(item);

                item = new ListViewItem();
                item.Text = this.lvwResult.Items.Count + 1 + "";
                item.SubItems.AddRange(new string[] { "Gamma值", _minResult.Gamma + "" });
                this.lvwResult.Items.Add(item);

                item = new ListViewItem {Text = lvwResult.Items.Count + 1 + ""};
                item.SubItems.AddRange(new[] { "平均误差值", _minResult.Average + "" });
                lvwResult.Items.Add(item);

                foreach (var var in _minResult.Prediction)
                {
                    item = new ListViewItem {Text = lvwResult.Items.Count + 1 + ""};
                    item.SubItems.AddRange(new[] { "第" + _intDataCount + "周第" + (var.Pos - (_intDataCount - 1) * _intLetterL) + "天", var.Number + "" });
                    lvwResult.Items.Add(item);
                }
            };
            //利用invoke转发委托
            Invoke(cto);
        }

        //清除集合中的数据
        private void CleanCollection(int minPos,int maxPos)
        {
            var tmpCollectionB = _collectionB.Where(item => item.Pos < minPos || item.Pos > maxPos).ToList();
            _collectionB = new List<Variable>();
            _collectionB = tmpCollectionB;

            var tmpCollectionF = _collectionF.Where(item => item.Pos < minPos || item.Pos > maxPos).ToList();
            _collectionF = new List<Variable>();
            _collectionF = tmpCollectionF;

            var tmpCollectionS = _collectionS.Where(item => item.Pos < minPos || item.Pos > maxPos).ToList();
            _collectionS = new List<Variable>();
            _collectionS = tmpCollectionS;
        }
        

       //根据序号获取集合中数据值
        private double GetCollectionRecordValue(IEnumerable<Variable> items, int posNumber)
        {
            return (from item in items where item.Pos == posNumber select item.Number).FirstOrDefault();
        }

        private void BwProcessorRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tssProcessValue.Text = Resources.CALC_COMPLETED;
            gbSetting.Enabled = true;
        }

        private void BwProcessorProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            tspProgress.Value = e.ProgressPercentage;
        }
    }
}
