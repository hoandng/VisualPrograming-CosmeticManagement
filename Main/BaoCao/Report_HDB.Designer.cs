namespace Main.BaoCao
{
    partial class Report_HDB
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
            this.rv_HoaDonBan = new Microsoft.Reporting.WinForms.ReportViewer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtp_Den = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtp_Tu = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rv_HoaDonBan
            // 
            this.rv_HoaDonBan.AutoScroll = true;
            this.rv_HoaDonBan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rv_HoaDonBan.Location = new System.Drawing.Point(0, 73);
            this.rv_HoaDonBan.Name = "rv_HoaDonBan";
            this.rv_HoaDonBan.ServerReport.BearerToken = null;
            this.rv_HoaDonBan.Size = new System.Drawing.Size(907, 785);
            this.rv_HoaDonBan.TabIndex = 0;
            this.rv_HoaDonBan.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth;
            this.rv_HoaDonBan.ZoomPercent = 75;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtp_Den);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dtp_Tu);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(907, 73);
            this.panel1.TabIndex = 1;
            // 
            // dtp_Den
            // 
            this.dtp_Den.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtp_Den.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_Den.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_Den.Location = new System.Drawing.Point(551, 23);
            this.dtp_Den.Name = "dtp_Den";
            this.dtp_Den.Size = new System.Drawing.Size(200, 28);
            this.dtp_Den.TabIndex = 1;
            this.dtp_Den.ValueChanged += new System.EventHandler(this.dtp_Den_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(499, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Đến";
            // 
            // dtp_Tu
            // 
            this.dtp_Tu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtp_Tu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_Tu.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_Tu.Location = new System.Drawing.Point(206, 24);
            this.dtp_Tu.Name = "dtp_Tu";
            this.dtp_Tu.Size = new System.Drawing.Size(200, 28);
            this.dtp_Tu.TabIndex = 1;
            this.dtp_Tu.ValueChanged += new System.EventHandler(this.dtp_Tu_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(154, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Từ:";
            // 
            // Report_HDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 858);
            this.Controls.Add(this.rv_HoaDonBan);
            this.Controls.Add(this.panel1);
            this.Name = "Report_HDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report_HDB";
            this.Load += new System.EventHandler(this.Report_HDB_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rv_HoaDonBan;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtp_Tu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtp_Den;
        private System.Windows.Forms.Label label2;
    }
}