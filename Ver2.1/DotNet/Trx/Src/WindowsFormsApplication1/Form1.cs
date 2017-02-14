using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using Trx.Messaging;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                textResultado.Text = "Enviando...";
                Envio oEnvio = new Envio();
                String sResultado = oEnvio.Envio_requerimiento(textIP.Text, Convert.ToInt32(textPuerto.Text), 10000, textRequerimiento.Text, 1, 1);                
                //String sResultado = Envio.Envio_requerimiento(textIP.Text, Convert.ToInt32(textPuerto.Text), 10000, textRequerimiento.Text, 1, 1);                
                textResultado.Text = "Esperando...";
                if (sResultado.Length > 0)
                {
                    textResultado.Text = sResultado;
                }
                else 
                {
                    textResultado.Text = "<ERROR>";
                }
            }
            catch (Exception ex)
            {
                textResultado.Text = "<ERROR>" + ex.Message;
            }
        }

       
        
    }
}
