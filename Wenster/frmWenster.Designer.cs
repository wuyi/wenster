namespace Wenster
{
    partial class frmWenster
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gbSetting = new System.Windows.Forms.GroupBox();
            this.txtInitDataCount = new System.Windows.Forms.TextBox();
            this.lblInitDataCount = new System.Windows.Forms.Label();
            this.btnCalc = new System.Windows.Forms.Button();
            this.txtLetterL = new System.Windows.Forms.TextBox();
            this.lblLetterL = new System.Windows.Forms.Label();
            this.gbResult = new System.Windows.Forms.GroupBox();
            this.lblAlphaValue = new System.Windows.Forms.Label();
            this.lblCurrentAlpha = new System.Windows.Forms.Label();
            this.lvwResult = new System.Windows.Forms.ListView();
            this.colId = new System.Windows.Forms.ColumnHeader();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colValue = new System.Windows.Forms.ColumnHeader();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.tssAppName = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssProcessValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.tspProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tspProgressValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.bwProcessor = new System.ComponentModel.BackgroundWorker();
            this.gbSetting.SuspendLayout();
            this.gbResult.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSetting
            // 
            this.gbSetting.Controls.Add(this.txtInitDataCount);
            this.gbSetting.Controls.Add(this.lblInitDataCount);
            this.gbSetting.Controls.Add(this.btnCalc);
            this.gbSetting.Controls.Add(this.txtLetterL);
            this.gbSetting.Controls.Add(this.lblLetterL);
            this.gbSetting.Location = new System.Drawing.Point(13, 12);
            this.gbSetting.Name = "gbSetting";
            this.gbSetting.Size = new System.Drawing.Size(336, 78);
            this.gbSetting.TabIndex = 0;
            this.gbSetting.TabStop = false;
            this.gbSetting.Text = "参数设置";
            // 
            // txtInitDataCount
            // 
            this.txtInitDataCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInitDataCount.Location = new System.Drawing.Point(90, 49);
            this.txtInitDataCount.Name = "txtInitDataCount";
            this.txtInitDataCount.Size = new System.Drawing.Size(100, 21);
            this.txtInitDataCount.TabIndex = 4;
            // 
            // lblInitDataCount
            // 
            this.lblInitDataCount.AutoSize = true;
            this.lblInitDataCount.Location = new System.Drawing.Point(7, 51);
            this.lblInitDataCount.Name = "lblInitDataCount";
            this.lblInitDataCount.Size = new System.Drawing.Size(77, 12);
            this.lblInitDataCount.TabIndex = 3;
            this.lblInitDataCount.Text = "计算的周期：";
            // 
            // btnCalc
            // 
            this.btnCalc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalc.Location = new System.Drawing.Point(249, 46);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(75, 21);
            this.btnCalc.TabIndex = 2;
            this.btnCalc.Text = "开始计算";
            this.btnCalc.UseVisualStyleBackColor = true;
            this.btnCalc.Click += new System.EventHandler(this.BtnCalcClick);
            // 
            // txtLetterL
            // 
            this.txtLetterL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLetterL.Location = new System.Drawing.Point(90, 16);
            this.txtLetterL.Name = "txtLetterL";
            this.txtLetterL.Size = new System.Drawing.Size(101, 21);
            this.txtLetterL.TabIndex = 1;
            // 
            // lblLetterL
            // 
            this.lblLetterL.AutoSize = true;
            this.lblLetterL.Location = new System.Drawing.Point(7, 18);
            this.lblLetterL.Name = "lblLetterL";
            this.lblLetterL.Size = new System.Drawing.Size(83, 12);
            this.lblLetterL.TabIndex = 0;
            this.lblLetterL.Text = "季节性周期L：";
            // 
            // gbResult
            // 
            this.gbResult.Controls.Add(this.lblAlphaValue);
            this.gbResult.Controls.Add(this.lblCurrentAlpha);
            this.gbResult.Controls.Add(this.lvwResult);
            this.gbResult.Location = new System.Drawing.Point(8, 96);
            this.gbResult.Name = "gbResult";
            this.gbResult.Size = new System.Drawing.Size(341, 217);
            this.gbResult.TabIndex = 1;
            this.gbResult.TabStop = false;
            this.gbResult.Text = "计算结果";
            // 
            // lblAlphaValue
            // 
            this.lblAlphaValue.AutoSize = true;
            this.lblAlphaValue.Location = new System.Drawing.Point(80, 198);
            this.lblAlphaValue.Name = "lblAlphaValue";
            this.lblAlphaValue.Size = new System.Drawing.Size(0, 12);
            this.lblAlphaValue.TabIndex = 2;
            // 
            // lblCurrentAlpha
            // 
            this.lblCurrentAlpha.AutoSize = true;
            this.lblCurrentAlpha.Location = new System.Drawing.Point(6, 198);
            this.lblCurrentAlpha.Name = "lblCurrentAlpha";
            this.lblCurrentAlpha.Size = new System.Drawing.Size(53, 12);
            this.lblCurrentAlpha.TabIndex = 1;
            this.lblCurrentAlpha.Text = "当前值：";
            // 
            // lvwResult
            // 
            this.lvwResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colId,
            this.colName,
            this.colValue});
            this.lvwResult.FullRowSelect = true;
            this.lvwResult.GridLines = true;
            this.lvwResult.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwResult.Location = new System.Drawing.Point(10, 18);
            this.lvwResult.Name = "lvwResult";
            this.lvwResult.Size = new System.Drawing.Size(319, 173);
            this.lvwResult.TabIndex = 0;
            this.lvwResult.UseCompatibleStateImageBehavior = false;
            this.lvwResult.View = System.Windows.Forms.View.Details;
            // 
            // colId
            // 
            this.colId.Text = "ID";
            this.colId.Width = 31;
            // 
            // colName
            // 
            this.colName.Text = "项目名";
            this.colName.Width = 76;
            // 
            // colValue
            // 
            this.colValue.Text = "项目值";
            this.colValue.Width = 146;
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssAppName,
            this.tssProcessValue,
            this.tspProgress,
            this.tspProgressValue});
            this.statusBar.Location = new System.Drawing.Point(0, 318);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(359, 23);
            this.statusBar.TabIndex = 2;
            this.statusBar.Text = "statusStrip1";
            // 
            // tssAppName
            // 
            this.tssAppName.Name = "tssAppName";
            this.tssAppName.Size = new System.Drawing.Size(53, 18);
            this.tssAppName.Text = "(C) 2011";
            // 
            // tssProcessValue
            // 
            this.tssProcessValue.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.tssProcessValue.Name = "tssProcessValue";
            this.tssProcessValue.Size = new System.Drawing.Size(57, 18);
            this.tssProcessValue.Text = "尚未计算";
            // 
            // tspProgress
            // 
            this.tspProgress.Name = "tspProgress";
            this.tspProgress.Size = new System.Drawing.Size(100, 17);
            // 
            // tspProgressValue
            // 
            this.tspProgressValue.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.tspProgressValue.Name = "tspProgressValue";
            this.tspProgressValue.Size = new System.Drawing.Size(75, 18);
            this.tspProgressValue.Text = "0 / 1000000";
            // 
            // bwProcessor
            // 
            this.bwProcessor.WorkerReportsProgress = true;
            this.bwProcessor.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BwProcessorDoWork);
            this.bwProcessor.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BwProcessorProgressChanged);
            // 
            // frmWenster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 341);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.gbResult);
            this.Controls.Add(this.gbSetting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmWenster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wenster";
            this.Load += new System.EventHandler(this.FrmWensterLoad);
            this.gbSetting.ResumeLayout(false);
            this.gbSetting.PerformLayout();
            this.gbResult.ResumeLayout(false);
            this.gbResult.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSetting;
        private System.Windows.Forms.Label lblLetterL;
        private System.Windows.Forms.TextBox txtLetterL;
        private System.Windows.Forms.TextBox txtInitDataCount;
        private System.Windows.Forms.Label lblInitDataCount;
        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.GroupBox gbResult;
        private System.Windows.Forms.ListView lvwResult;
        private System.Windows.Forms.ColumnHeader colId;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colValue;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel tssAppName;
        private System.Windows.Forms.ToolStripStatusLabel tssProcessValue;
        private System.ComponentModel.BackgroundWorker bwProcessor;
        private System.Windows.Forms.ToolStripProgressBar tspProgress;
        private System.Windows.Forms.ToolStripStatusLabel tspProgressValue;
        private System.Windows.Forms.Label lblAlphaValue;
        private System.Windows.Forms.Label lblCurrentAlpha;
    }
}

