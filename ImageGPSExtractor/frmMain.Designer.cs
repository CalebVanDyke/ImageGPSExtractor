namespace ImageGPSExtractor
{
    partial class frmMain
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
            this.btnGetGpsMetaData = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button7 = new System.Windows.Forms.Button();
            this.btnResizeImages = new System.Windows.Forms.Button();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chkBoxDuplicates = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnGetGpsMetaData
            // 
            this.btnGetGpsMetaData.Location = new System.Drawing.Point(396, 12);
            this.btnGetGpsMetaData.Name = "btnGetGpsMetaData";
            this.btnGetGpsMetaData.Size = new System.Drawing.Size(108, 23);
            this.btnGetGpsMetaData.TabIndex = 0;
            this.btnGetGpsMetaData.Text = "Extract GPS Meta";
            this.btnGetGpsMetaData.UseVisualStyleBackColor = true;
            this.btnGetGpsMetaData.Click += new System.EventHandler(this.btnGetGpsMetaData_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(108, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Select Images";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(13, 42);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(847, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.Location = new System.Drawing.Point(12, 71);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(847, 631);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 50;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(741, 13);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(118, 23);
            this.button7.TabIndex = 11;
            this.button7.Text = "Clear Images";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // btnResizeImages
            // 
            this.btnResizeImages.Location = new System.Drawing.Point(510, 12);
            this.btnResizeImages.Name = "btnResizeImages";
            this.btnResizeImages.Size = new System.Drawing.Size(106, 23);
            this.btnResizeImages.TabIndex = 12;
            this.btnResizeImages.Text = "Resize Images";
            this.btnResizeImages.UseVisualStyleBackColor = true;
            this.btnResizeImages.Click += new System.EventHandler(this.btnResizeImages_Click);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "File Name";
            this.columnHeader2.Width = 500;
            // 
            // chkBoxDuplicates
            // 
            this.chkBoxDuplicates.AutoSize = true;
            this.chkBoxDuplicates.Checked = true;
            this.chkBoxDuplicates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxDuplicates.Location = new System.Drawing.Point(127, 16);
            this.chkBoxDuplicates.Name = "chkBoxDuplicates";
            this.chkBoxDuplicates.Size = new System.Drawing.Size(125, 17);
            this.chkBoxDuplicates.TabIndex = 13;
            this.chkBoxDuplicates.Text = "Remove Duplicates?";
            this.chkBoxDuplicates.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 737);
            this.Controls.Add(this.chkBoxDuplicates);
            this.Controls.Add(this.btnResizeImages);
            this.Controls.Add(this.btnGetGpsMetaData);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button2);
            this.Name = "frmMain";
            this.Text = "Extract GPS From Images";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetGpsMetaData;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button btnResizeImages;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.CheckBox chkBoxDuplicates;
    }
}

