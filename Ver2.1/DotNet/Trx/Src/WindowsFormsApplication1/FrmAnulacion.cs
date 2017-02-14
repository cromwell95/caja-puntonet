using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AppDemo
{
    public partial class FrmAnulacion : Form
    {
        public FrmAnulacion()
        {
            InitializeComponent();
        }


        public FrmAnulacion(AppDemo.DtsReport.anulacionDataTable dt)
        {
            try
            {
                DataTable dtanulacion;
                dtanulacion = dt;
                InitializeComponent();

                AppDemo.Reportes.rptanulacion rptanulacion1 = new AppDemo.Reportes.rptanulacion();
                rptanulacion1.SetDataSource(dtanulacion);
                visoranulacion.ReportSource = rptanulacion1;



            }
            catch (Exception ex)
            {
                
                throw;
            }

           
        }
    }
}
