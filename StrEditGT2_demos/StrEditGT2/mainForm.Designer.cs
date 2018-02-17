namespace StrEdit
{
    partial class mainForm
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
            this.editGrid = new System.Windows.Forms.DataGridView();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.btnSaveFile = new System.Windows.Forms.Button();
            this.txFilename = new System.Windows.Forms.TextBox();
            this.btnExportCSV = new System.Windows.Forms.Button();
            this.btnImportCSV = new System.Windows.Forms.Button();
            this.rbIngame = new System.Windows.Forms.RadioButton();
            this.rbMenu = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.editGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // editGrid
            // 
            this.editGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.editGrid.Location = new System.Drawing.Point(12, 42);
            this.editGrid.Name = "editGrid";
            this.editGrid.Size = new System.Drawing.Size(735, 432);
            this.editGrid.TabIndex = 0;
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(13, 13);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(75, 23);
            this.btnLoadFile.TabIndex = 1;
            this.btnLoadFile.Text = "Load File";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // btnSaveFile
            // 
            this.btnSaveFile.Location = new System.Drawing.Point(95, 13);
            this.btnSaveFile.Name = "btnSaveFile";
            this.btnSaveFile.Size = new System.Drawing.Size(75, 23);
            this.btnSaveFile.TabIndex = 2;
            this.btnSaveFile.Text = "Save File";
            this.btnSaveFile.UseVisualStyleBackColor = true;
            this.btnSaveFile.Click += new System.EventHandler(this.btnSaveFile_Click);
            // 
            // txFilename
            // 
            this.txFilename.Location = new System.Drawing.Point(177, 13);
            this.txFilename.Name = "txFilename";
            this.txFilename.Size = new System.Drawing.Size(141, 20);
            this.txFilename.TabIndex = 3;
            this.txFilename.Text = ".carinfoe";
            // 
            // btnExportCSV
            // 
            this.btnExportCSV.Location = new System.Drawing.Point(592, 13);
            this.btnExportCSV.Name = "btnExportCSV";
            this.btnExportCSV.Size = new System.Drawing.Size(75, 23);
            this.btnExportCSV.TabIndex = 4;
            this.btnExportCSV.Text = "Export CSV";
            this.btnExportCSV.UseVisualStyleBackColor = true;
            this.btnExportCSV.Click += new System.EventHandler(this.btnExportCSV_Click);
            // 
            // btnImportCSV
            // 
            this.btnImportCSV.Location = new System.Drawing.Point(673, 13);
            this.btnImportCSV.Name = "btnImportCSV";
            this.btnImportCSV.Size = new System.Drawing.Size(75, 23);
            this.btnImportCSV.TabIndex = 5;
            this.btnImportCSV.Text = "Import CSV";
            this.btnImportCSV.UseVisualStyleBackColor = true;
            this.btnImportCSV.Click += new System.EventHandler(this.btnImportCSV_Click);
            // 
            // rbIngame
            // 
            this.rbIngame.AutoSize = true;
            this.rbIngame.Checked = true;
            this.rbIngame.Location = new System.Drawing.Point(325, 15);
            this.rbIngame.Name = "rbIngame";
            this.rbIngame.Size = new System.Drawing.Size(60, 17);
            this.rbIngame.TabIndex = 6;
            this.rbIngame.TabStop = true;
            this.rbIngame.Text = "Ingame";
            this.rbIngame.UseVisualStyleBackColor = true;
            // 
            // rbMenu
            // 
            this.rbMenu.AutoSize = true;
            this.rbMenu.Location = new System.Drawing.Point(392, 15);
            this.rbMenu.Name = "rbMenu";
            this.rbMenu.Size = new System.Drawing.Size(52, 17);
            this.rbMenu.TabIndex = 7;
            this.rbMenu.Text = "Menu";
            this.rbMenu.UseVisualStyleBackColor = true;
            this.rbMenu.CheckedChanged += new System.EventHandler(this.rbMenu_CheckedChanged);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 486);
            this.Controls.Add(this.rbMenu);
            this.Controls.Add(this.rbIngame);
            this.Controls.Add(this.btnImportCSV);
            this.Controls.Add(this.btnExportCSV);
            this.Controls.Add(this.txFilename);
            this.Controls.Add(this.btnSaveFile);
            this.Controls.Add(this.btnLoadFile);
            this.Controls.Add(this.editGrid);
            this.Name = "mainForm";
            this.Text = "StrEdit v0.1";
            this.Load += new System.EventHandler(this.mainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.editGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView editGrid;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.Button btnSaveFile;
        private System.Windows.Forms.TextBox txFilename;
        private System.Windows.Forms.Button btnExportCSV;
        private System.Windows.Forms.Button btnImportCSV;
        private System.Windows.Forms.RadioButton rbIngame;
        private System.Windows.Forms.RadioButton rbMenu;
    }
}

