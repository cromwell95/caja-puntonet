namespace AppDemo
{
    partial class FrmAnulacion
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
            this.visoranulacion = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // visoranulacion
            // 
            this.visoranulacion.ActiveViewIndex = -1;
            this.visoranulacion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.visoranulacion.DisplayGroupTree = false;
            this.visoranulacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visoranulacion.Location = new System.Drawing.Point(0, 0);
            this.visoranulacion.Name = "visoranulacion";
            this.visoranulacion.SelectionFormula = "";
            this.visoranulacion.Size = new System.Drawing.Size(314, 450);
            this.visoranulacion.TabIndex = 0;
            this.visoranulacion.ViewTimeSelectionFormula = "";
            // 
            // FrmAnulacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 450);
            this.Controls.Add(this.visoranulacion);
            this.Name = "FrmAnulacion";
            this.Text = "Anulación";
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crvanulacion;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer visoranulacion;
    }
}