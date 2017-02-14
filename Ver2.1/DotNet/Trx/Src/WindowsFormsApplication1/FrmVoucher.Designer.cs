namespace AppDemo
{
    partial class FrmVoucher
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
            this.visorcompra = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // visorcompra
            // 
            this.visorcompra.ActiveViewIndex = -1;
            this.visorcompra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.visorcompra.DisplayGroupTree = false;
            this.visorcompra.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visorcompra.Location = new System.Drawing.Point(0, 0);
            this.visorcompra.Name = "visorcompra";
            this.visorcompra.SelectionFormula = "";
            this.visorcompra.Size = new System.Drawing.Size(391, 487);
            this.visorcompra.TabIndex = 0;
            this.visorcompra.ViewTimeSelectionFormula = "";
            // 
            // FrmVoucher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 487);
            this.Controls.Add(this.visorcompra);
            this.Name = "FrmVoucher";
            this.Text = "Voucher";
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer visorcompra;
    }
}