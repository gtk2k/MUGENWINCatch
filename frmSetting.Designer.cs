namespace MUGENWINCatch
{
    partial class frmSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtLoiLoPath = new System.Windows.Forms.TextBox();
            this.lblLoiLoPath = new System.Windows.Forms.Label();
            this.btnLoiLoReference = new System.Windows.Forms.Button();
            this.btnReg = new System.Windows.Forms.Button();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.btnMUGENReference = new System.Windows.Forms.Button();
            this.lblMUGENPath = new System.Windows.Forms.Label();
            this.txtMUGENPath = new System.Windows.Forms.TextBox();
            this.btnFFmpegReference = new System.Windows.Forms.Button();
            this.lblFFmpegPath = new System.Windows.Forms.Label();
            this.txtFFmpegPath = new System.Windows.Forms.TextBox();
            this.btnLoiLoAVIOutputFolderReference = new System.Windows.Forms.Button();
            this.lblLoiLoAVIFoloderPath = new System.Windows.Forms.Label();
            this.txtLoiLoAVIOutputFolder = new System.Windows.Forms.TextBox();
            this.btnMP4OutputFolderReference = new System.Windows.Forms.Button();
            this.lblMP4OutputFolderPath = new System.Windows.Forms.Label();
            this.txtMP4OutputFolder = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtLoiLoPath
            // 
            this.txtLoiLoPath.Location = new System.Drawing.Point(6, 29);
            this.txtLoiLoPath.Name = "txtLoiLoPath";
            this.txtLoiLoPath.ReadOnly = true;
            this.txtLoiLoPath.Size = new System.Drawing.Size(661, 20);
            this.txtLoiLoPath.TabIndex = 0;
            this.txtLoiLoPath.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // lblLoiLoPath
            // 
            this.lblLoiLoPath.AutoSize = true;
            this.lblLoiLoPath.Location = new System.Drawing.Point(3, 10);
            this.lblLoiLoPath.Name = "lblLoiLoPath";
            this.lblLoiLoPath.Size = new System.Drawing.Size(230, 13);
            this.lblLoiLoPath.TabIndex = 1;
            this.lblLoiLoPath.Text = "LoiLo Game Recorderの実行ファイル場所";
            // 
            // btnLoiLoReference
            // 
            this.btnLoiLoReference.Location = new System.Drawing.Point(674, 26);
            this.btnLoiLoReference.Name = "btnLoiLoReference";
            this.btnLoiLoReference.Size = new System.Drawing.Size(87, 25);
            this.btnLoiLoReference.TabIndex = 2;
            this.btnLoiLoReference.Text = "参照";
            this.btnLoiLoReference.UseVisualStyleBackColor = true;
            this.btnLoiLoReference.Click += new System.EventHandler(this.btnLoiLoReference_Click);
            // 
            // btnReg
            // 
            this.btnReg.Enabled = false;
            this.btnReg.Location = new System.Drawing.Point(674, 278);
            this.btnReg.Name = "btnReg";
            this.btnReg.Size = new System.Drawing.Size(87, 25);
            this.btnReg.TabIndex = 3;
            this.btnReg.Text = "設定";
            this.btnReg.UseVisualStyleBackColor = true;
            this.btnReg.Click += new System.EventHandler(this.btnReg_Click);
            // 
            // ofd
            // 
            this.ofd.FileName = "mugen.exe";
            this.ofd.Filter = "exeファイル(*.exe)|*.exe";
            // 
            // btnMUGENReference
            // 
            this.btnMUGENReference.Location = new System.Drawing.Point(674, 117);
            this.btnMUGENReference.Name = "btnMUGENReference";
            this.btnMUGENReference.Size = new System.Drawing.Size(87, 25);
            this.btnMUGENReference.TabIndex = 6;
            this.btnMUGENReference.Text = "参照";
            this.btnMUGENReference.UseVisualStyleBackColor = true;
            this.btnMUGENReference.Click += new System.EventHandler(this.btnMUGENReference_Click);
            // 
            // lblMUGENPath
            // 
            this.lblMUGENPath.AutoSize = true;
            this.lblMUGENPath.Location = new System.Drawing.Point(3, 100);
            this.lblMUGENPath.Name = "lblMUGENPath";
            this.lblMUGENPath.Size = new System.Drawing.Size(161, 13);
            this.lblMUGENPath.TabIndex = 5;
            this.lblMUGENPath.Text = "M.U.G.E.Nの実行ファイル場所";
            // 
            // txtMUGENPath
            // 
            this.txtMUGENPath.Location = new System.Drawing.Point(6, 120);
            this.txtMUGENPath.Name = "txtMUGENPath";
            this.txtMUGENPath.ReadOnly = true;
            this.txtMUGENPath.Size = new System.Drawing.Size(661, 20);
            this.txtMUGENPath.TabIndex = 4;
            this.txtMUGENPath.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // btnFFmpegReference
            // 
            this.btnFFmpegReference.Location = new System.Drawing.Point(674, 163);
            this.btnFFmpegReference.Name = "btnFFmpegReference";
            this.btnFFmpegReference.Size = new System.Drawing.Size(87, 25);
            this.btnFFmpegReference.TabIndex = 9;
            this.btnFFmpegReference.Text = "参照";
            this.btnFFmpegReference.UseVisualStyleBackColor = true;
            this.btnFFmpegReference.Click += new System.EventHandler(this.btnFFmpegReference_Click);
            // 
            // lblFFmpegPath
            // 
            this.lblFFmpegPath.AutoSize = true;
            this.lblFFmpegPath.Location = new System.Drawing.Point(3, 147);
            this.lblFFmpegPath.Name = "lblFFmpegPath";
            this.lblFFmpegPath.Size = new System.Drawing.Size(150, 13);
            this.lblFFmpegPath.TabIndex = 8;
            this.lblFFmpegPath.Text = "FFmpegの実行ファイル場所";
            // 
            // txtFFmpegPath
            // 
            this.txtFFmpegPath.Location = new System.Drawing.Point(6, 166);
            this.txtFFmpegPath.Name = "txtFFmpegPath";
            this.txtFFmpegPath.ReadOnly = true;
            this.txtFFmpegPath.Size = new System.Drawing.Size(661, 20);
            this.txtFFmpegPath.TabIndex = 7;
            this.txtFFmpegPath.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // btnLoiLoAVIOutputFolderReference
            // 
            this.btnLoiLoAVIOutputFolderReference.Location = new System.Drawing.Point(674, 71);
            this.btnLoiLoAVIOutputFolderReference.Name = "btnLoiLoAVIOutputFolderReference";
            this.btnLoiLoAVIOutputFolderReference.Size = new System.Drawing.Size(87, 25);
            this.btnLoiLoAVIOutputFolderReference.TabIndex = 12;
            this.btnLoiLoAVIOutputFolderReference.Text = "参照";
            this.btnLoiLoAVIOutputFolderReference.UseVisualStyleBackColor = true;
            this.btnLoiLoAVIOutputFolderReference.Click += new System.EventHandler(this.btnLoiLoAVIOutputFolderReference_Click);
            // 
            // lblLoiLoAVIFoloderPath
            // 
            this.lblLoiLoAVIFoloderPath.AutoSize = true;
            this.lblLoiLoAVIFoloderPath.Location = new System.Drawing.Point(3, 55);
            this.lblLoiLoAVIFoloderPath.Name = "lblLoiLoAVIFoloderPath";
            this.lblLoiLoAVIFoloderPath.Size = new System.Drawing.Size(218, 13);
            this.lblLoiLoAVIFoloderPath.TabIndex = 11;
            this.lblLoiLoAVIFoloderPath.Text = "LoiLo Game Recorderの保存先フォルダ";
            // 
            // txtLoiLoAVIOutputFolder
            // 
            this.txtLoiLoAVIOutputFolder.Location = new System.Drawing.Point(6, 74);
            this.txtLoiLoAVIOutputFolder.Name = "txtLoiLoAVIOutputFolder";
            this.txtLoiLoAVIOutputFolder.ReadOnly = true;
            this.txtLoiLoAVIOutputFolder.Size = new System.Drawing.Size(661, 20);
            this.txtLoiLoAVIOutputFolder.TabIndex = 10;
            // 
            // btnMP4OutputFolderReference
            // 
            this.btnMP4OutputFolderReference.Location = new System.Drawing.Point(674, 209);
            this.btnMP4OutputFolderReference.Name = "btnMP4OutputFolderReference";
            this.btnMP4OutputFolderReference.Size = new System.Drawing.Size(87, 25);
            this.btnMP4OutputFolderReference.TabIndex = 15;
            this.btnMP4OutputFolderReference.Text = "参照";
            this.btnMP4OutputFolderReference.UseVisualStyleBackColor = true;
            this.btnMP4OutputFolderReference.Click += new System.EventHandler(this.btnMP4OutputFolderReference_Click);
            // 
            // lblMP4OutputFolderPath
            // 
            this.lblMP4OutputFolderPath.AutoSize = true;
            this.lblMP4OutputFolderPath.Location = new System.Drawing.Point(3, 193);
            this.lblMP4OutputFolderPath.Name = "lblMP4OutputFolderPath";
            this.lblMP4OutputFolderPath.Size = new System.Drawing.Size(159, 13);
            this.lblMP4OutputFolderPath.TabIndex = 14;
            this.lblMP4OutputFolderPath.Text = "変換後のMP4保存先フォルダ";
            // 
            // txtMP4OutputFolder
            // 
            this.txtMP4OutputFolder.Location = new System.Drawing.Point(6, 212);
            this.txtMP4OutputFolder.Name = "txtMP4OutputFolder";
            this.txtMP4OutputFolder.ReadOnly = true;
            this.txtMP4OutputFolder.Size = new System.Drawing.Size(661, 20);
            this.txtMP4OutputFolder.TabIndex = 13;
            this.txtMP4OutputFolder.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // frmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 337);
            this.Controls.Add(this.btnMP4OutputFolderReference);
            this.Controls.Add(this.lblMP4OutputFolderPath);
            this.Controls.Add(this.txtMP4OutputFolder);
            this.Controls.Add(this.btnLoiLoAVIOutputFolderReference);
            this.Controls.Add(this.lblLoiLoAVIFoloderPath);
            this.Controls.Add(this.txtLoiLoAVIOutputFolder);
            this.Controls.Add(this.btnFFmpegReference);
            this.Controls.Add(this.lblFFmpegPath);
            this.Controls.Add(this.txtFFmpegPath);
            this.Controls.Add(this.btnMUGENReference);
            this.Controls.Add(this.lblMUGENPath);
            this.Controls.Add(this.txtMUGENPath);
            this.Controls.Add(this.btnReg);
            this.Controls.Add(this.btnLoiLoReference);
            this.Controls.Add(this.lblLoiLoPath);
            this.Controls.Add(this.txtLoiLoPath);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetting";
            this.Text = "初回設定";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Button btnLoiLoReference;
        private System.Windows.Forms.Button btnReg;
        private System.Windows.Forms.OpenFileDialog ofd;

        #endregion
        internal System.Windows.Forms.Label lblLoiLoPath;
        private System.Windows.Forms.Button btnMUGENReference;
        internal System.Windows.Forms.Label lblMUGENPath;
        private System.Windows.Forms.Button btnFFmpegReference;
        internal System.Windows.Forms.Label lblFFmpegPath;
        internal System.Windows.Forms.TextBox txtLoiLoPath;
        internal System.Windows.Forms.TextBox txtMUGENPath;
        internal System.Windows.Forms.TextBox txtFFmpegPath;
        private System.Windows.Forms.Button btnLoiLoAVIOutputFolderReference;
        internal System.Windows.Forms.Label lblLoiLoAVIFoloderPath;
        internal System.Windows.Forms.TextBox txtLoiLoAVIOutputFolder;
        private System.Windows.Forms.Button btnMP4OutputFolderReference;
        internal System.Windows.Forms.Label lblMP4OutputFolderPath;
        internal System.Windows.Forms.TextBox txtMP4OutputFolder;
    }
}