namespace WindowsFormsApplication1
{
    partial class Form2
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtreq = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txttimeout = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkgrabalog = new System.Windows.Forms.CheckBox();
            this.chkgrabamsg = new System.Windows.Forms.CheckBox();
            this.txtresp = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.txtruta = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtip = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(118, 9);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Puertos Disponibles";
            // 
            // txtreq
            // 
            this.txtreq.Location = new System.Drawing.Point(12, 87);
            this.txtreq.Multiline = true;
            this.txtreq.Name = "txtreq";
            this.txtreq.Size = new System.Drawing.Size(535, 94);
            this.txtreq.TabIndex = 2;
            this.txtreq.Text = "RA0300000000008621000000006599000000000500000000000792000000000710000000000020   " +
                "         00102712490220140104028112000000328465   000000328464   121314151213141" +
                "5";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Requerimiento";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 187);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Respuesta";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(217, 348);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Enviar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txttimeout
            // 
            this.txttimeout.Location = new System.Drawing.Point(298, 10);
            this.txttimeout.Name = "txttimeout";
            this.txttimeout.Size = new System.Drawing.Size(67, 20);
            this.txttimeout.TabIndex = 10;
            this.txttimeout.Text = "30000";
            this.txttimeout.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txttimeout_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(242, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Time Out";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(367, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(192, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Valor en milisegundos (30000 = 30 seg)";
            // 
            // chkgrabalog
            // 
            this.chkgrabalog.AutoSize = true;
            this.chkgrabalog.Checked = true;
            this.chkgrabalog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkgrabalog.Location = new System.Drawing.Point(16, 36);
            this.chkgrabalog.Name = "chkgrabalog";
            this.chkgrabalog.Size = new System.Drawing.Size(76, 17);
            this.chkgrabalog.TabIndex = 17;
            this.chkgrabalog.Text = "Graba Log";
            this.chkgrabalog.UseVisualStyleBackColor = true;
            // 
            // chkgrabamsg
            // 
            this.chkgrabamsg.AutoSize = true;
            this.chkgrabamsg.Checked = true;
            this.chkgrabamsg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkgrabamsg.Location = new System.Drawing.Point(118, 36);
            this.chkgrabamsg.Name = "chkgrabamsg";
            this.chkgrabamsg.Size = new System.Drawing.Size(78, 17);
            this.chkgrabamsg.TabIndex = 18;
            this.chkgrabamsg.Text = "Graba Msg";
            this.chkgrabamsg.UseVisualStyleBackColor = true;
            // 
            // txtresp
            // 
            this.txtresp.Location = new System.Drawing.Point(12, 203);
            this.txtresp.Multiline = true;
            this.txtresp.Name = "txtresp";
            this.txtresp.Size = new System.Drawing.Size(531, 139);
            this.txtresp.TabIndex = 19;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(495, 58);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(52, 23);
            this.button2.TabIndex = 20;
            this.button2.Text = "Archivo";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtruta
            // 
            this.txtruta.Location = new System.Drawing.Point(217, 59);
            this.txtruta.Name = "txtruta";
            this.txtruta.Size = new System.Drawing.Size(272, 20);
            this.txtruta.TabIndex = 21;
            this.txtruta.Text = "C:\\Users\\farmijos\\Desktop\\Bines.txt";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(242, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "IP";
            // 
            // txtip
            // 
            this.txtip.Location = new System.Drawing.Point(298, 33);
            this.txtip.Name = "txtip";
            this.txtip.Size = new System.Drawing.Size(191, 20);
            this.txtip.TabIndex = 22;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 383);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtip);
            this.Controls.Add(this.txtruta);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtresp);
            this.Controls.Add(this.chkgrabamsg);
            this.Controls.Add(this.chkgrabalog);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txttimeout);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtreq);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Name = "Form2";
            this.Text = "BinesVoucher v2.1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtreq;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txttimeout;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkgrabalog;
        private System.Windows.Forms.CheckBox chkgrabamsg;
        private System.Windows.Forms.TextBox txtresp;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtruta;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtip;
    }
}