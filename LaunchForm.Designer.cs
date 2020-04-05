﻿namespace ProvinceMapper
{
	partial class LaunchForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.tbSourceMapFolder = new System.Windows.Forms.TextBox();
            this.tbDestMapFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbSourceTag = new System.Windows.Forms.TextBox();
            this.tbDestTag = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnBegin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tbMappingsFile = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cbScale = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbNamesFrom = new System.Windows.Forms.ComboBox();
            this.ckInvertSource = new System.Windows.Forms.CheckBox();
            this.ckInvertDest = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source Map Folder";
            // 
            // tbSourceMapFolder
            // 
            this.tbSourceMapFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSourceMapFolder.Location = new System.Drawing.Point(116, 9);
            this.tbSourceMapFolder.Name = "tbSourceMapFolder";
            this.tbSourceMapFolder.Size = new System.Drawing.Size(534, 20);
            this.tbSourceMapFolder.TabIndex = 1;
            this.tbSourceMapFolder.Text = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Crusader Kings II\\map";
            // 
            // tbDestMapFolder
            // 
            this.tbDestMapFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDestMapFolder.Location = new System.Drawing.Point(116, 35);
            this.tbDestMapFolder.Name = "tbDestMapFolder";
            this.tbDestMapFolder.Size = new System.Drawing.Size(534, 20);
            this.tbDestMapFolder.TabIndex = 3;
            this.tbDestMapFolder.Text = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Europa Universalis IV\\map";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Target Map Folder";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Source Tag";
            // 
            // tbSourceTag
            // 
            this.tbSourceTag.Location = new System.Drawing.Point(116, 62);
            this.tbSourceTag.Name = "tbSourceTag";
            this.tbSourceTag.Size = new System.Drawing.Size(45, 20);
            this.tbSourceTag.TabIndex = 5;
            this.tbSourceTag.Text = "ck2";
            // 
            // tbDestTag
            // 
            this.tbDestTag.Location = new System.Drawing.Point(116, 88);
            this.tbDestTag.Name = "tbDestTag";
            this.tbDestTag.Size = new System.Drawing.Size(45, 20);
            this.tbDestTag.TabIndex = 7;
            this.tbDestTag.Text = "eu4";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Target Tag";
            // 
            // btnBegin
            // 
            this.btnBegin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBegin.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnBegin.Location = new System.Drawing.Point(643, 298);
            this.btnBegin.Name = "btnBegin";
            this.btnBegin.Size = new System.Drawing.Size(61, 23);
            this.btnBegin.TabIndex = 8;
            this.btnBegin.Text = "Begin";
            this.btnBegin.UseVisualStyleBackColor = true;
            this.btnBegin.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(12, 298);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(61, 23);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Existing Mappings";
            // 
            // tbMappingsFile
            // 
            this.tbMappingsFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMappingsFile.Location = new System.Drawing.Point(116, 114);
            this.tbMappingsFile.Name = "tbMappingsFile";
            this.tbMappingsFile.Size = new System.Drawing.Size(534, 20);
            this.tbMappingsFile.TabIndex = 11;
            this.tbMappingsFile.Text = "C:\\MyProjects\\paradoxGameConvertersForks\\CK2ToEU4\\CK2ToEU4\\Data_Files\\configurabl" +
    "es\\provinces.txt";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 269);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(692, 23);
            this.progressBar1.TabIndex = 12;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(79, 303);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(47, 13);
            this.lblStatus.TabIndex = 13;
            this.lblStatus.Text = "Ready...";
            // 
            // cbScale
            // 
            this.cbScale.AutoSize = true;
            this.cbScale.Checked = true;
            this.cbScale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbScale.Location = new System.Drawing.Point(12, 166);
            this.cbScale.Name = "cbScale";
            this.cbScale.Size = new System.Drawing.Size(116, 17);
            this.cbScale.TabIndex = 14;
            this.cbScale.Text = "Resize smaller map";
            this.cbScale.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 143);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Use Names From:";
            // 
            // cbNamesFrom
            // 
            this.cbNamesFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNamesFrom.FormattingEnabled = true;
            this.cbNamesFrom.Items.AddRange(new object[] {
            "Localization",
            "Map data"});
            this.cbNamesFrom.Location = new System.Drawing.Point(116, 140);
            this.cbNamesFrom.Name = "cbNamesFrom";
            this.cbNamesFrom.Size = new System.Drawing.Size(121, 21);
            this.cbNamesFrom.TabIndex = 16;
            // 
            // ckInvertSource
            // 
            this.ckInvertSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ckInvertSource.AutoSize = true;
            this.ckInvertSource.Checked = true;
            this.ckInvertSource.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckInvertSource.Location = new System.Drawing.Point(656, 11);
            this.ckInvertSource.Name = "ckInvertSource";
            this.ckInvertSource.Size = new System.Drawing.Size(53, 17);
            this.ckInvertSource.TabIndex = 17;
            this.ckInvertSource.Text = "Invert";
            this.ckInvertSource.UseVisualStyleBackColor = true;
            // 
            // ckInvertDest
            // 
            this.ckInvertDest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ckInvertDest.AutoSize = true;
            this.ckInvertDest.Checked = true;
            this.ckInvertDest.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckInvertDest.Location = new System.Drawing.Point(656, 37);
            this.ckInvertDest.Name = "ckInvertDest";
            this.ckInvertDest.Size = new System.Drawing.Size(53, 17);
            this.ckInvertDest.TabIndex = 18;
            this.ckInvertDest.Text = "Invert";
            this.ckInvertDest.UseVisualStyleBackColor = true;
            // 
            // LaunchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 334);
            this.Controls.Add(this.ckInvertDest);
            this.Controls.Add(this.ckInvertSource);
            this.Controls.Add(this.cbNamesFrom);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbScale);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.tbMappingsFile);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnBegin);
            this.Controls.Add(this.tbDestTag);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbSourceTag);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbDestMapFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbSourceMapFolder);
            this.Controls.Add(this.label1);
            this.Name = "LaunchForm";
            this.Text = "Province Mapper Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbSourceMapFolder;
		private System.Windows.Forms.TextBox tbDestMapFolder;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbSourceTag;
		private System.Windows.Forms.TextBox tbDestTag;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnBegin;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tbMappingsFile;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.CheckBox cbScale;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox cbNamesFrom;
		private System.Windows.Forms.CheckBox ckInvertSource;
		private System.Windows.Forms.CheckBox ckInvertDest;
	}
}

