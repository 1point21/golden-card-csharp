
namespace Guldkortet
{
    partial class Form1
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.lbxSyslog = new System.Windows.Forms.ListBox();
            this.lblIPAdress = new System.Windows.Forms.Label();
            this.lblportNr = new System.Windows.Forms.Label();
            this.lblLogg = new System.Windows.Forms.Label();
            this.btnAvsluta = new System.Windows.Forms.Button();
            this.btnRensa = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(107, 76);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "CONNECT";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lbxSyslog
            // 
            this.lbxSyslog.FormattingEnabled = true;
            this.lbxSyslog.Location = new System.Drawing.Point(12, 133);
            this.lbxSyslog.Name = "lbxSyslog";
            this.lbxSyslog.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lbxSyslog.Size = new System.Drawing.Size(395, 251);
            this.lbxSyslog.TabIndex = 2;
            // 
            // lblIPAdress
            // 
            this.lblIPAdress.AutoSize = true;
            this.lblIPAdress.Location = new System.Drawing.Point(125, 22);
            this.lblIPAdress.Name = "lblIPAdress";
            this.lblIPAdress.Size = new System.Drawing.Size(102, 13);
            this.lblIPAdress.TabIndex = 3;
            this.lblIPAdress.Text = "IP-adress: 127.0.0.1";
            this.lblIPAdress.Visible = false;
            // 
            // lblportNr
            // 
            this.lblportNr.AutoSize = true;
            this.lblportNr.Location = new System.Drawing.Point(125, 44);
            this.lblportNr.Name = "lblportNr";
            this.lblportNr.Size = new System.Drawing.Size(62, 13);
            this.lblportNr.TabIndex = 4;
            this.lblportNr.Text = "Port: 12345";
            this.lblportNr.Visible = false;
            // 
            // lblLogg
            // 
            this.lblLogg.AutoSize = true;
            this.lblLogg.Location = new System.Drawing.Point(13, 114);
            this.lblLogg.Name = "lblLogg";
            this.lblLogg.Size = new System.Drawing.Size(61, 13);
            this.lblLogg.TabIndex = 5;
            this.lblLogg.Text = "Systemlogg";
            // 
            // btnAvsluta
            // 
            this.btnAvsluta.Enabled = false;
            this.btnAvsluta.Location = new System.Drawing.Point(300, 396);
            this.btnAvsluta.Name = "btnAvsluta";
            this.btnAvsluta.Size = new System.Drawing.Size(107, 40);
            this.btnAvsluta.TabIndex = 6;
            this.btnAvsluta.Text = "AVSLUTAR OCH KOPPLA NED";
            this.btnAvsluta.UseVisualStyleBackColor = true;
            this.btnAvsluta.Click += new System.EventHandler(this.btnAvsluta_Click);
            // 
            // btnRensa
            // 
            this.btnRensa.Location = new System.Drawing.Point(12, 396);
            this.btnRensa.Name = "btnRensa";
            this.btnRensa.Size = new System.Drawing.Size(75, 40);
            this.btnRensa.TabIndex = 7;
            this.btnRensa.Text = "Rensa Logg";
            this.btnRensa.UseVisualStyleBackColor = true;
            this.btnRensa.Click += new System.EventHandler(this.btnRensa_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(93, 396);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 40);
            this.btnExport.TabIndex = 8;
            this.btnExport.Text = "Exportera";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Guldkortet.Properties.Resources.nos;
            this.pictureBox1.Location = new System.Drawing.Point(282, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(125, 120);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 450);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnRensa);
            this.Controls.Add(this.btnAvsluta);
            this.Controls.Add(this.lblLogg);
            this.Controls.Add(this.lblportNr);
            this.Controls.Add(this.lblIPAdress);
            this.Controls.Add(this.lbxSyslog);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GULDKORT";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ListBox lbxSyslog;
        private System.Windows.Forms.Label lblIPAdress;
        private System.Windows.Forms.Label lblportNr;
        private System.Windows.Forms.Label lblLogg;
        private System.Windows.Forms.Button btnAvsluta;
        private System.Windows.Forms.Button btnRensa;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

