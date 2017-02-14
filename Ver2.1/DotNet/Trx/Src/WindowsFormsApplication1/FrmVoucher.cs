using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AppDemo
{
    public partial class FrmVoucher : Form
    {
        

        public FrmVoucher()
        {
            InitializeComponent();
            
        }

        public FrmVoucher(AppDemo.DtsReport.compraDataTable dt)
        {
            DataTable dtcompra;
            dtcompra = dt;
            InitializeComponent();
            AppDemo.Reportes.rptcompra rptautorizacion = new AppDemo.Reportes.rptcompra();
            rptautorizacion.SetDataSource(dtcompra);
            visorcompra.ReportSource = rptautorizacion;
        }

    }
}
