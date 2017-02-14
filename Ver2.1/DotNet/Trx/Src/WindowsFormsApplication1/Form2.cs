using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using Trx.Messaging;
using Trx.Messaging.Utilidades;


namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            foreach (var item in SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(item);

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string resp;
            string []arrip;
            //string ip=string.Empty;
            //string puerto=string.Empty;
            //string trama=string.Empty;
            //int puertoCOM;
            try
                
            {
               button1.Enabled = false;
               Envio oEnvio = new Envio();

               //if (String.IsNullOrEmpty(txtip.Text) == false)
               //{

               //    arrip = this.txtip.Text.Split(':');
               //    ip = arrip[0].PadRight(15, ' ');
               //    puerto = arrip[1].PadRight(6, ' ');
               //    trama = txtreq.Text;
               //}
               
                //if (String.IsNullOrEmpty(txtruta.Text))
                //{
                //    trama = trama + ip + puerto;

                //}
              
               //resp = oEnvio.Envio_requerimiento(null, 9, 30000, "RA000000000000001000000000000000000000001000000000000000 00000114241520130603 12131415000000000328464030613 ", "C:\\Users\\farmijos\\Desktop\\PinPad\\NRLibreriaserial\\Fuentes\\Net\\BINES.txt", 1);
               //  resp = oEnvio.Envio_requerimientoPinpad(this.txtip.Text, Convert.ToInt16(comboBox1.SelectedItem.ToString().Length == 4 ? comboBox1.SelectedItem.ToString().Substring(3, 1) : comboBox1.SelectedItem.ToString().Substring(3, 2)), Convert.ToInt32(this.txttimeout.Text), this.txtreq.Text, txtruta.Text, this.chkgrabalog.Checked == true ? 1 : 0);
               //puertoCOM = Convert.ToInt32(comboBox1.SelectedItem.ToString().Length == 4 ? comboBox1.SelectedItem.ToString().Substring(3, 1) : comboBox1.SelectedItem.ToString().Substring(3, 2));
               //resp = oEnvio.Envio_requerimientoPinpad(this.txtip.Text, 1, Convert.ToInt32(this.txttimeout.Text), this.txtreq.Text, txtruta.Text, this.chkgrabalog.Checked == true ? 1 : 0);
                       // oEnvio.Envio_requerimientoPinpad("192.168.1.75", 3030, Convert.ToInt32("30000"), "LT", "1", 1);
                        //oEnvio.Envio_requerimientoPinpad("192.168.1.75:3030", 3, 30000, "LT", "1", "1");

               object puertoCom = comboBox1.SelectedItem;
               string puertoCom1 = puertoCom.ToString();
               puertoCom1 = puertoCom1.Substring(puertoCom1.Length-1, 1);
                int puertofinal = Convert.ToInt32(puertoCom1);
                resp = oEnvio.Envio_requerimientoPinpad(txtip.Text, puertofinal, Convert.ToInt32(this.txttimeout.Text), this.txtreq.Text, null, this.chkgrabalog.Checked == true ? 1 : 0); //FUNCIONANDO
               // resp = oEnvio.Envio_requerimientoPinpad(null, puertofinal, 30000, "PP01200     000000002000000000000000000000001000000000000100000000000500000000000300                  12324420140701      00000000000000200000001", null, 1);
              //Autorizacion
                //resp = "RA000200AUTORIZACION OK.    0010220000251249022014010403943812131415000000328464   0000                                                                                                                                                 476173XXXXXX010     VISA/ELECTRON            03VISA ACQUIRER TEST CARD 18                          VISA CREDIT                                                                                  ";
               //Anulacion
               //  resp = "RA000200AUTORIZACION OK.    0010270000251249022014010402811212131415000000328464   0000                                                                                                                                                                     VISA/ELECTRON            03VISA ACQUIRER TEST CARD 18                          VISA CREDIT                                                                                  ";
                 txtresp.Text = resp;
                 // txtresp.AppendText(resp + "\r" + "\n");
                 /*if (resp.Length > 10)
                 {

                     if ((txtreq.Text.Substring(2, 2) == "01" || txtreq.Text.Substring(2, 2) == "02" || txtreq.Text.Substring(2, 2) == "05") && (resp.Substring(2, 2) == "00"))
                     {                    
                         imprimir(resp);
                     }

                     if (txtreq.Text.Substring(2, 2) == "03" && (resp.Substring(2, 2) == "00"))
                     {
                         imprimir(resp);
                     }
                 }*/

                button1.Enabled = true;
                //txtresp.AppendText(resp + "\r" + "\n");
         
            }
            catch (Exception ex)
            {
                this.txtresp.AppendText(ex.Message);
                throw;
            }
           
        }

        private void txttimeout_KeyPress(object sender, KeyPressEventArgs e )
        {
            //Para obligar a que sólo se introduzcan números
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {

                if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso
                {
                    e.Handled = false;
                }
                else
                {
                    //el resto de teclas pulsadas se desactivan
                    e.Handled = true;
                } 
             }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    txtruta.Text = openFileDialog1.FileName;
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }




        private void imprimir(string resp)
        {
            string comercio;
            string lote;
            string tid;
            string trx;
            string lectura;
            string autorizacion;
            decimal subtot;
            decimal iva;
            decimal total;
            string tarjeta;
            string aplabel;
            string numtarjeta;
            string nombre;
            decimal grabaiva;
            decimal nograbaiva;
            DateTime fecha;
            DateTime hora;

            try
            {
                UTILIDADES.mensaje("DEBUG : " + txtreq.Text.Substring(2, 2).Trim() + " == " + resp.Substring(4, 2), "LogTracerCOM", "log");
                if (txtreq.Text.Substring(2, 2).Trim() == "01" || txtreq.Text.Substring(2, 2).Trim() == "02" || txtreq.Text.Substring(2, 2).Trim() == "05")
                {
                  

                    comercio = this.txtresp.Text.Substring(68, 15).PadLeft(15, ' ');
                    UTILIDADES.mensaje("DEBUG : " + "comercio == " + comercio, "LogTracerCOM", "log");
                    lote = this.txtresp.Text.Substring(34, 6);
                    UTILIDADES.mensaje("DEBUG : " + "lote == " + lote, "LogTracerCOM", "log");
                    tid = this.txtresp.Text.Substring(60, 8).PadLeft(8, ' ');
                    UTILIDADES.mensaje("DEBUG : " + "tid == " + tid, "LogTracerCOM", "log");
                    numtarjeta = this.txtresp.Text.Substring(232, 20);
                    UTILIDADES.mensaje("DEBUG : " + "numtarjeta == " + numtarjeta, "LogTracerCOM", "log");
                    trx = this.txtresp.Text.Substring(28, 6);
                    UTILIDADES.mensaje("DEBUG : " + "trx == " + trx, "LogTracerCOM", "log");
                    autorizacion = this.txtresp.Text.Substring(54, 6);
                    UTILIDADES.mensaje("DEBUG : " + "autorizacion == " + autorizacion, "LogTracerCOM", "log");
                    total = Convert.ToDecimal(this.txtreq.Text.Substring(6, 12)) / 100;
                    UTILIDADES.mensaje("DEBUG : " + "total == " + total, "LogTracerCOM", "log");
                    grabaiva = Convert.ToDecimal(this.txtreq.Text.Substring(18, 12)) / 100;
                    UTILIDADES.mensaje("DEBUG : " + "grabaiva == " + grabaiva, "LogTracerCOM", "log");
                    nograbaiva = Convert.ToDecimal(this.txtreq.Text.Substring(30, 12)) / 100;
                    UTILIDADES.mensaje("DEBUG : " + "nograbaiva == " + nograbaiva, "LogTracerCOM", "log");
                    iva = Convert.ToDecimal(this.txtreq.Text.Substring(42, 12)) / 100;
                    UTILIDADES.mensaje("DEBUG : " + "iva == " + iva, "LogTracerCOM", "log");
                    lectura = this.txtresp.Text.Substring(277, 2).Trim();
                    UTILIDADES.mensaje("DEBUG : " + "lectura == " + lectura, "LogTracerCOM", "log");
                    aplabel = this.txtresp.Text.Substring(331, 20).Trim();
                    UTILIDADES.mensaje("DEBUG : " + "aplabel == " + aplabel, "LogTracerCOM", "log");
                    tarjeta = this.txtresp.Text.Substring(252, 4).Trim();

                    UTILIDADES.mensaje("DEBUG : " + "tarjeta == " + tarjeta, "LogTracerCOM", "log");
                    nombre = this.txtresp.Text.Substring(279, 40).Trim();
                    UTILIDADES.mensaje("DEBUG : " + "nombre == " + nombre, "LogTracerCOM", "log");

                    subtot = grabaiva + nograbaiva;

                    
                    hora = Convert.ToDateTime(this.txtresp.Text.Substring(40, 2) + ":" + this.txtresp.Text.Substring(42, 2) + ":" + this.txtresp.Text.Substring(44, 2));
                    UTILIDADES.mensaje("DEBUG : " + "hora == " + hora, "LogTracerCOM", "log");

                    fecha = Convert.ToDateTime(this.txtresp.Text.Substring(46, 4) + "-" + this.txtresp.Text.Substring(50, 2) + "-" + this.txtresp.Text.Substring(52, 2));
                    UTILIDADES.mensaje("DEBUG : " + "fecha == " + fecha, "LogTracerCOM", "log");

                    if (lectura == "02")
                        lectura = "BANDA";
                    if (lectura == "03")
                        lectura = "CHIP";
                    if (lectura == "04" || lectura == "05")
                        lectura = "CHIP BANDA";

                    UTILIDADES.mensaje("DEBUG : " + "lectura == " + lectura, "LogTracerCOM", "log");

                    AppDemo.DtsReport ds = new AppDemo.DtsReport();
                    AppDemo.DtsReport.compraDataTable dtcompra = new AppDemo.DtsReport.compraDataTable();
                    dtcompra.AddcompraRow(comercio, lote, tid, tarjeta, numtarjeta, lectura, trx, autorizacion, grabaiva, nograbaiva, subtot, iva, total, aplabel, nombre, fecha, hora);
                    AppDemo.FrmVoucher frm = new AppDemo.FrmVoucher(dtcompra);
                    frm.Show();
                }


                if (this.txtreq.Text.Substring(2, 2) == "03")
                {
                    comercio = this.txtresp.Text.Substring(68, 15).PadLeft(15, ' ');
                    UTILIDADES.mensaje("DEBUG : " + "comercio == " + comercio, "LogTracerCOM", "log");

                    lote = this.txtresp.Text.Substring(34, 6);
                    UTILIDADES.mensaje("DEBUG : " + "lote == " + lote, "LogTracerCOM", "log");
                    tid = this.txtresp.Text.Substring(60, 8).PadLeft(8, ' ');
                    UTILIDADES.mensaje("DEBUG : " + "tid == " + tid, "LogTracerCOM", "log");
                    numtarjeta = this.txtresp.Text.Substring(232, 20);
                    UTILIDADES.mensaje("DEBUG : " + "numtarjeta == " + numtarjeta, "LogTracerCOM", "log");
                    tarjeta = this.txtresp.Text.Substring(252, 4).Trim();
                    UTILIDADES.mensaje("DEBUG : " + "tarjeta == " + tarjeta, "LogTracerCOM", "log");
                    trx = this.txtresp.Text.Substring(28, 6);
                    UTILIDADES.mensaje("DEBUG : " + "trx == " + trx, "LogTracerCOM", "log");
                    autorizacion = this.txtresp.Text.Substring(54, 6);
                    UTILIDADES.mensaje("DEBUG : " + "autorizacion == " + autorizacion, "LogTracerCOM", "log");
                    total = Convert.ToDecimal(this.txtreq.Text.Substring(6, 12)) / 100;
                    UTILIDADES.mensaje("DEBUG : " + "total == " + total, "LogTracerCOM", "log");
                    lectura = this.txtresp.Text.Substring(277, 2).Trim();
                    UTILIDADES.mensaje("DEBUG : " + "lectura == " + lectura, "LogTracerCOM", "log");
                    hora = Convert.ToDateTime(this.txtresp.Text.Substring(40, 2) + ":" + this.txtresp.Text.Substring(42, 2) + ":" + this.txtresp.Text.Substring(44, 2));
                    UTILIDADES.mensaje("DEBUG : " + "hora == " + hora, "LogTracerCOM", "log");
                    fecha = Convert.ToDateTime(this.txtresp.Text.Substring(46, 4) + "-" + this.txtresp.Text.Substring(50, 2) + "-" + this.txtresp.Text.Substring(52, 2));
                    UTILIDADES.mensaje("DEBUG : " + "fecha == " + fecha, "LogTracerCOM", "log");

                    if (lectura == "02")
                        lectura = "BANDA";
                    if (lectura == "03")
                        lectura = "CHIP";
                    if (lectura == "04" || lectura == "05")
                        lectura = "CHIP BANDA";

                    AppDemo.DtsReport ds = new AppDemo.DtsReport();
                    AppDemo.DtsReport.anulacionDataTable dtanulacion = new AppDemo.DtsReport.anulacionDataTable();
                    dtanulacion.AddanulacionRow(comercio, lote, tid, tarjeta, numtarjeta, lectura, trx, autorizacion, trx, total, fecha, hora);
                    AppDemo.FrmAnulacion frm = new AppDemo.FrmAnulacion(dtanulacion);
                    frm.Show();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Opción no disponible " +  ex.Message, "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }



        }

       
    }
}


