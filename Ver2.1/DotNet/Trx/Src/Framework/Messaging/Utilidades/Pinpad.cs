using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Timers;
using System.Threading;
using System.Security.Cryptography;
using System.Data;



using System.Collections;

namespace Trx.Messaging.Utilidades
{
    public class Pinpad
    {

        public static readonly string ArchivoNoExiste = "0403ARCHIVO DE BINES NO EXISTE";
        public static readonly string FormatoIncorrecto = "0503FORMATO DE ARCHIVO DE BINES INCORRECTO";
        public static readonly string ErrorCnxPinpad = "ER03ERR. CONEXION PINPAD"; //posible error - verificar
        public static readonly string TimeOutIN = "TO03TIMEOUT";
        public static readonly string TimeOutID = "TO03TIMEOUT";
        public static readonly string TimeOutAut = "TOERTOTIMEOUT             ";
        public static string ErrorCPPinpad = "ERERR. CONEXION PINPAD"; //Mensaje agregado para CP raaf
        public static string TimeOutCP = "TOTIMEOUT             ";     //Mensaje agregado para CP raaf
        public static string ErrorCTPinpad = "ER                                     ERR. CONEXION PINPAD"; //Mensaje agregado para CP raaf
        public static string TimeOutCT = "TO                                     TIMEOUT             ";                  //Mensaje agregado para CP raaf
        public static string ErrorLTPinpad = "ER                           ERR. CONEXION PINPAD"; //Mensaje agregado para LT raaf
        public static string TimeOutLT = "TO                           TIMEOUT             ";                 //Mensaje agregado para LT raaf
        public static string ErrorPPPinpad = "ER    ERR. CONEXION PINPAD"; //Mensaje agregado para PP raaf
        public static string TimeOutPP = "TO    TIMEOUT             ";     //Mensaje agregado para PP raaf
        public static string ErrorPCPinpad = "ER  ERR. CONEXION PINPAD";   //Mensaje agregado para PP raaf
        public static string TimeOutPC = "TO  TIMEOUT             ";       //Mensaje agregado para PP raaf
        public static string ErrorPAPinpad = "ER  ERR. CONEXION PINPAD";   //Mensaje agregado para PP raaf
        public static string TimeOutPA = "TO  TIMEOUT             ";       //Mensaje agregado para PP raaf

            //Errores de conexion plan D - LABA 08/10/2015
        public static string ErrorCPPD   = "ERERR. CONEXION PINPAD"; //Mensaje agregado para CP
        public static string ErrorCTPD   = "ER                                                  ERR. CONEXION PINPAD"; //Mensaje agregado para CT
        public static string ErrorLTPD   = "ER00                                                                     ERR. CONEXION PINPAD"; //Mensaje agregado para LT
        public static string ErrorPPPD   = "ER    ERR. CONEXION PINPAD                                                                                                                                                                                                                                                                                                                                                                                                                                                               "; //Mensaje agregado para PP
        public static string ErrorPCPD   = "ER  ERR. CONEXION PINPAD"; //Mensaje agregado para PC
        public static string ErrorPAPD   = "ER  ERR. CONEXION PINPAD"; //Mensaje agregado para PA
        
        //Errores de timeout plan D - LABA 08/10/2015
        public static string TimeOutCPPD = "TOTIME OUT            "; //Mensaje agregado para CP
        public static string TimeOutCTPD = "TO                                                  TIME OUT            "; //Mensaje agregado para CT
        public static string TimeOutLTPD = "TO00                                                                     TIME OUT            "; //Mensaje agregado para LT
        public static string TimeOutPPPD = "TO    TIME OUT                                                                                                                                                                                                                                                                                                                                                                                                                                                                           "; //Mensaje agregado para PP
        public static string TimeOutPCPD = "TO  TIME OUT            "; //Mensaje agregado para PC
        public static string TimeOutPAPD = "TO  TIME OUT            "; //Mensaje agregado para PA

        public static String ErrorBBPD = "ER  ERR. CONEXION PINPAD                                                                                                                                                                                                                                                                                      ";
        public static String TimeOutBBPd = "TO  TIMEOUT                                                                                                                                                                                                                                                                                                   ";
    

        public static void XTA()
        {
            ErrorCPPinpad = ErrorCPPD;
            ErrorCTPinpad = ErrorCTPD;
            ErrorLTPinpad = ErrorLTPD;
            ErrorPPPinpad = ErrorPPPD;
            ErrorPCPinpad = ErrorPCPD;
            ErrorPAPinpad = ErrorPAPD;

            TimeOutCP = TimeOutCPPD;
            TimeOutCT = TimeOutCTPD;
            TimeOutLT = TimeOutLTPD;
            TimeOutPP = TimeOutPPPD;
            TimeOutPC = TimeOutPCPD;
            TimeOutPA = TimeOutPAPD;
        }


        // Funcion para calcular el LRC de un mensaje enviado sin tomar en cuenta el STX
        private static char ComputeCheckSum(string istr)
        {
            char ETX = (char)0x03;

            if (istr.IndexOf(ETX) == -1)
            {
                istr = istr + ETX.ToString();
            }


            int pCheckSum = 0;
            for (int i = 1; i < istr.Length; i++)
            {
                pCheckSum = pCheckSum ^ ((int)istr[i]);
            }
            return (char)pCheckSum;
        }  
    
        private static String GenerateHash(String ram)
        {
            string sResult = GetSHA1(ram.ToLower()).ToUpper();        
            return sResult.Substring(0,16);
        }
        private static string GetSHA1(string str)
        {
            SHA1 sha1 = SHA1Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha1.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        //Funcion que envia el mensaje por el puerto COM al pinpad que se envia por parametro a la DLL 
        public static string SendPinpad(string trama, int puerto, int timeout, int iGrabaLog, int iGrabaMsg)
        {         

            char STX = (char)0x02;
            char ETX = (char)0x03;
            char ACK = (char)0x06;
            char NAK = (char)0x15;

            char LRC;
            char LRCSeguridad;
            char LRCSeguridadError;
            string iPuerto;
            Int32 iBaudios;
            int iBits;           
            int I;
            
            Single FinSeg;
           


            int ET;
            int ST;
            int AC;
            int C = 0; // Reintentos cuando envio trama y el pinpad responde nack
            int J = 0; // Reintentos cuando la trama de respuesta no es correcta
            int Timer=0;
            string Dato;
            string LlaveIzq = "FAC7BC932ABCEC2B";
            string LlaveDer = "C8B2463ABEF901FE";
            string Criptograma= string.Empty;
            string tramaseguridad = string.Empty;
            string tramaseguridaderror = string.Empty;
            string Respuesta = String.Empty;
            SerialPort Port = new SerialPort();
            string TipoTrx;
        
               

            //Dato randomico para el proceso de encripcion
            Random r = new Random(DateTime.Now.Millisecond);
            Int64 aleatorio = r.Next(999999);         
            Dato = GenerateHash(aleatorio.ToString());
            

         
            //Constuyo la trama de venta para enviar al pinpad
            trama = STX + trama + ETX;
            //Constuyo la trama de seguridad para enviar al pinpad
            tramaseguridad = STX + "SH" +  Dato + ETX;
            //Constuyo la trama de seguridad por error para enviar al pinpad
            tramaseguridaderror = STX + "EOT" + ETX; 


            //Calculo LRC
            LRC = ComputeCheckSum(trama);

            //Calculo LRC
            LRCSeguridad = ComputeCheckSum(tramaseguridad);

             //Calculo LRC
            LRCSeguridadError = ComputeCheckSum(tramaseguridaderror);

            //Calcula el criptograma del dato enviado
            Criptograma = UTILIDADES.F3DESEncriptarBloque(UTILIDADES.padMultiplo(Dato, 16, "1C"), LlaveIzq, LlaveDer);

            try
            {
                int indextp = trama.IndexOf(STX);
                TipoTrx = trama.Substring(indextp +1 , 2);
            }
            catch (Exception ex)
            {
                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Error al obtener el tipo de transaccion " + ex.Message, "LogTracerCOM", "log");
                return  "ER ERR.CONEXION PINPAD";  
            }
           
            
            try
            {
                // Leemos los parametros del archivo de configuracion antes de abrir el puerto y enviar el mensaje
                Init ini = new Init(Environment.CurrentDirectory + "\\DriverVerifone32.ini");
                         
                iPuerto = "COM" + puerto.ToString();
               
                iBaudios = Convert.ToInt32(ini.IniReadValue("Config", "iBaudios"));
                iBits = Convert.ToInt32(ini.IniReadValue("Config", "iBits"));
                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Cargamos la configuracion para la libreria DriverVerifone32 " + " Puerto=" + puerto + " iBaudios=" + iBaudios + " iBits=" + iBits + " TimeOut=" + timeout , "LogTracerCOM", "log");

                Port = new SerialPort(iPuerto, iBaudios, Parity.None, iBits, StopBits.One);
               
                FinSeg = timeout / 1000;

                Port.Open();

                if (Port.IsOpen)
                {
                    byte[] mBufferSeguridad = Encoding.ASCII.GetBytes(tramaseguridad + LRCSeguridad);
                    Port.Write(mBufferSeguridad, 0, mBufferSeguridad.Length);

                    if (iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Se envia trama de seguridad al puerto " + iPuerto + " TramaSeguridad=" + tramaseguridad + LRCSeguridad, "LogEventCOM", "log");
                    

                    while (true)
                    {

                        Thread.Sleep(1000);
                        Timer += 1;

                        byte[] bufferseguridad = new byte[Port.BytesToRead];
                        Port.Read(bufferseguridad, 0, bufferseguridad.Length);
                        Respuesta = System.Text.ASCIIEncoding.ASCII.GetString(bufferseguridad);

                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Trama de Seguridad Leo la respuesta enviada por el pinpad Resp.=" + Respuesta, "LogTracerCOM", "log");

                        if (Respuesta.Length > 0)
                        {
                            int index = Respuesta.IndexOf("H");
                            if (Criptograma == Respuesta.Substring(index + 1, 16))
                            {
                                Port.Write(ACK.ToString());
                                break;
                            }
                            else
                            {
                                byte[] mBufferSeg = Encoding.ASCII.GetBytes(tramaseguridaderror + LRCSeguridadError);
                                Port.Write(mBufferSeg, 0, mBufferSeg.Length);
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "ERROR DE SEGURIDAD EN EL PINPID", "LogTracerCOM", "log");
                                Respuesta = TipoTrx + "ER ERR.CONEXION PINPAD"; 
                                Port.Close();
                                return Respuesta;
                            }
                        }

                        //Validacion del timeout
                        if (Timer > FinSeg)
                        {
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Salgo por timeout= " + FinSeg + " seg", "LogTracerCOM", "log");
                            Respuesta =  TipoTrx + "ER ERR.CONEXION PINPAD";
                            Port.Close();
                            return Respuesta;
                        }
                    }                    
                }
                
            }
                 catch (Exception ex)
            {              
                if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR : Exception: " + ex.Message.ToString(), "LogTracerCOM", "log");
                Port.Close();
                return TipoTrx + "ER ERR.CONEXION PINPAD";
            }

            try
            {
                Respuesta = string.Empty;
                FinSeg = timeout / 1000;

                if (Port.IsOpen)
                {
                   byte[] mBuffer = Encoding.ASCII.GetBytes(trama + LRC);
                   Port.Write(mBuffer, 0, mBuffer.Length);

                    if (iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Se envia trama al puerto " + iPuerto + " Trama=" + trama, "LogEventCOM", "log");
                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Se envia trama al puerto " + iPuerto + " Trama=" + trama, "LogTracerCOM", "log");

                    while (true)
                    {

                        Thread.Sleep(1000);
                        Timer += 1;

                        byte[] buffer = new byte[Port.BytesToRead];
                        Port.Read(buffer, 0, buffer.Length);
                        Respuesta = System.Text.ASCIIEncoding.ASCII.GetString(buffer);

                        //Validacion del timeout
                        if (Timer > FinSeg)
                        {
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Salgo por timeout= " + FinSeg + " seg", "LogTracerCOM", "log");
                            Respuesta = TipoTrx + "ER ERR.CONEXION PINPAD"; 
                            break;
                        }

                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Leo la respuesta enviada por el pinpad Resp.=" + Respuesta, "LogTracerCOM", "log");

                        I = Respuesta.IndexOf(NAK);

                        if (I == 0)
                        {
                            if (C < 3)
                            {
                                //Vuelvo a enviar la trama 

                                byte[] newBuffer = Encoding.ASCII.GetBytes(trama + LRC);
                                Port.Write(newBuffer, 0, newBuffer.Length);
                                C += 1;
                                Respuesta = string.Empty;
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Se envia nuevamente la trama intento No." + C + " Trama enviada--> " + trama, "LogTracerCOM", "log");

                            }
                            else
                            {
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Salgo del proceso que lee el puerto  intento No. " + C + " Respuesta=" + Respuesta, "LogTracerCOM", "log");
                                break;
                            }
                        }


                        //Verifico si la trama de respuesta tiene inicio y fin del mensaje
                        ST = Respuesta.IndexOf(STX);
                        ET = Respuesta.IndexOf(ETX);
                        AC = Respuesta.IndexOf(ACK);


                        if ((ST == 0 && ET > 0) || (ST > 0 && ET > 0 && AC == 0))  
                        {
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Evaluo si la trama de respuesta es correcta calculo el LRC=" + Respuesta.Substring(ET + 1, 1), "LogTracerCOM", "log");

                            //Evaluo si la trama es correcta 
                            if (ComputeCheckSum(Respuesta.Substring(ST, ET + 1)) == Convert.ToChar(Respuesta.Substring(ET + 1, 1)))
                            {
                                // Envio ACK                                    
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "LRC de respuesta es correcto envio ACK y salgo del ciclo respuesta=" + Respuesta, "LogTracerCOM", "log");
                                Port.Write(ACK.ToString());
                                break;
                            }
                            else
                            {
                                if (J < 3)
                                {
                                    Respuesta = string.Empty;
                                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "La trama de respuesta esta erronea envio NACK trama recibida intento numero " + J, "LogTracerCOM", "log");

                                    Port.Write(NAK.ToString());
                                    J += 1;

                                    if (J == 2)
                                    {
                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Despues de evaluar 3 veces el LRC de respuesta salgo y se envia respuesta a la caja, dejo de leer el Puerto", "LogTracerCOM", "log");
                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "LRC ENVIADO POR EL PINPAD NO ENTENDIDO", "LogTracerCOM", "log");
                                        Respuesta = TipoTrx + "ER ERR.CONEXION PINPAD"; 
                                        break;
                                    }
                                }

                            }
                        }
                    }
                }

                // Cerramos el puerto
                Port.Close();
                ST = Respuesta.IndexOf(STX);
                ET = Respuesta.IndexOf(ETX);

                if (ET > 0)
                {

                    if (Respuesta.Length > ET)
                    {
                        Respuesta = Respuesta.Remove(ET, 1);
                        Respuesta = Respuesta.Remove(ST, 1);
                        Respuesta = Respuesta.Remove(Respuesta.Length - 1, 1);
                    }

                }
                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Se envia respuesta a la caja respuesta=" + Respuesta, "LogTracerCOM", "log");                
                return Respuesta;
            }

            catch (Exception ex)
            {
                if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR : Exception: " + ex.Message.ToString(), "LogTracerCOM", "log");
                Port.Close();
                return TipoTrx + "ER ERR.CONEXION PINPAD"; 
            } 
                   
        }

        //Funcion que envia un archivo de bines al pos - Funcion DotNet para el envio de una trx
        public static string SendPinpad(string trama, int puerto, int timeout, string RutaBines, int iGrabaLog)
        {
            XTA();
            char STX = (char)0x02;
            char ETX = (char)0x03;
            char ACK = (char)0x06;
            char NAK = (char)0x15;

            char LRC;
            char LRCSeguridad;
            char LRCSeguridadError;
            string iPuerto;
            Int32 iBaudios;
            int iBits;
            string iRutaBinesOut;
            int I;

            Single FinSeg;

            int ET;
            int ST;
            int C = 0; // Reintentos cuando envio trama y el pinpad responde nack
            int J = 0; // Reintentos cuando la trama de respuesta no es correcta
            int Timer = 0;
            string Dato;
            string LlaveIzq = "FAC7BC932ABCEC2B";
            string LlaveDer = "C8B2463ABEF901FE";
            string Criptograma = string.Empty;
            string tramaseguridad = string.Empty;
            string tramaseguridaderror = string.Empty;
            string Respuesta = String.Empty;
            SerialPort Port = new SerialPort();
            string TipoTrx = String.Empty;
            bool Error = false;
            //string space = null;

            //Datos para posible Error de Conexion con PinPad raaf
            /*if (trama.Substring(0, 2).Equals("PP"))
            {
                space = "";
                for (int i = 0; i < 352; i++)
                {
                    space = space + " ";
                }
                ErrorPPPinpad = ErrorPPPinpad + space;
                TimeOutPP = TimeOutPP + space;
                
            }*/
            
            //Dato randomico para el proceso de encripcion
            Random r = new Random(DateTime.Now.Millisecond);
            Int64 aleatorio = r.Next(999999);
            Dato = GenerateHash(aleatorio.ToString());



            //Constuyo la trama de venta para enviar al pinpad
            trama = STX + trama + ETX;
            //Constuyo la trama de seguridad para enviar al pinpad
            tramaseguridad = STX + "SH" + Dato + ETX;
            //Constuyo la trama de seguridad por error para enviar al pinpad
            tramaseguridaderror = STX + "EOT" + ETX;


            //Calculo LRC
            LRC = ComputeCheckSum(trama);

            //Calculo LRC
            LRCSeguridad = ComputeCheckSum(tramaseguridad);

            //Calculo LRC
            LRCSeguridadError = ComputeCheckSum(tramaseguridaderror);

            //Calcula el criptograma del dato enviado
            Criptograma = UTILIDADES.F3DESEncriptarBloque(UTILIDADES.padMultiplo(Dato, 16, "1C"), LlaveIzq, LlaveDer);

            try
            {
                int indextp = trama.IndexOf(STX);
                TipoTrx = trama.Substring(indextp + 1, 2);
            }
            catch (Exception ex)
            {
                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendPinpad Error al obtener el tipo de transaccion " + ex.Message, "LogTracerCOM", "log");
                return  TipoTrx + ErrorCnxPinpad;
            }


            #region ValidoArchivo


            if (TipoTrx.Equals("IN") || TipoTrx.Equals("ID"))
            {
                try
                {
                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Verificacion de archivo de bines ruta = " + RutaBines, "LogTracerCOM", "log");
                    if (validaArchivoBines(RutaBines) != true)
                    {
                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG :Verificacion de archivo Error al validar archivo de bines", "LogTracerCOM", "log");
                        return   Respuesta = TipoTrx + FormatoIncorrecto;
                    }
                }
                catch (Exception ex)
                {
                    if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR :Verificacion de archivo Exception al validar archivo de bines " + ex.Message, "LogTracerCOM", "log");
                        return  Respuesta = TipoTrx + ErrorCnxPinpad;
                }
            }
            #endregion

            #region SeguridadPinpad
            try
            {
                // Leemos los parametros del archivo de configuracion antes de abrir el puerto y enviar el mensaje
            Init ini = new Init(Environment.CurrentDirectory + "\\DriverVerifone32.ini");
            iPuerto = "COM" + puerto.ToString();
            iBaudios = Convert.ToInt32(ini.IniReadValue("Config", "iBaudios"));
            iBits = Convert.ToInt32(ini.IniReadValue("Config", "iBits"));
            iRutaBinesOut = System.AppDomain.CurrentDomain.BaseDirectory + "BINESOUT.TXT";

            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendPinpad cargamos la configuracion para la libreria DriverVerifone32 " + " Puerto=" + puerto + " iBaudios=" + iBaudios + " iBits=" + iBits + " TimeOut=" + timeout, "LogTracerCOM", "log");

                Port = new SerialPort(iPuerto, iBaudios, Parity.None, iBits, StopBits.One);
                FinSeg = timeout / 1000;
                Port.Open();

                if (Port.IsOpen)
                {
                    byte[] mBufferSeguridad = Encoding.ASCII.GetBytes(tramaseguridad + LRCSeguridad);
                    Port.Write(mBufferSeguridad, 0, mBufferSeguridad.Length);

                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendPinpad Se envia trama de seguridad al puerto " + iPuerto + " TramaSeguridad=" + tramaseguridad + LRCSeguridad, "LogTracerCOM", "log");

                    while (true)
                    {

                        Thread.Sleep(1000);
                        Timer += 1;

                        byte[] bufferseguridad = new byte[Port.BytesToRead];
                        Port.Read(bufferseguridad, 0, bufferseguridad.Length);
                       
                        Respuesta = System.Text.ASCIIEncoding.ASCII.GetString(bufferseguridad);

                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendPinpad Trama de Seguridad Leo la respuesta enviada por el pinpad Resp.=" + Respuesta, "LogTracerCOM", "log");

                        if (Respuesta.Length > 0)
                        {
                            int index = Respuesta.IndexOf("H");
                            if (Criptograma == Respuesta.Substring(index + 1, 16))
                            {
                                Port.Write(ACK.ToString());
                                Port.Close();
                                break;
                            }
                            else
                            {
                                byte[] mBufferSeg = Encoding.ASCII.GetBytes(tramaseguridaderror + LRCSeguridadError);
                                Port.Write(mBufferSeg, 0, mBufferSeg.Length);
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendPinpad ERROR DE SEGURIDAD EN EL PINPID", "LogTracerCOM", "log");
                               
                                if (TipoTrx == "IN" || TipoTrx == "ID")
                                    Respuesta = TipoTrx + TimeOutIN;
                                else
                                if (TipoTrx.Equals("CP"))
                                {
                                    Respuesta = TipoTrx + TimeOutCP; //Agregado para CP raaf
                                }
                                else
                                if (TipoTrx.Equals("CT"))
                                {
                                    Respuesta = TipoTrx + TimeOutCT; //Agregado para CT raaf
                                }
                                else
                                if (TipoTrx.Equals("LT"))
                                {
                                    Respuesta = TipoTrx + TimeOutLT; //Agregado para LT raaf
                                }
                                else
                                if (TipoTrx.Equals("PP"))
                                {
                                    Respuesta = TipoTrx + TimeOutPP; //Agregado para PP raaf
                                }
                                else
                                if (TipoTrx.Equals("PC"))
                                {
                                    Respuesta = TipoTrx + TimeOutPC; //Agregado para LT raaf
                                }
                                else
                                if (TipoTrx.Equals("PA"))
                                {
                                    Respuesta = TipoTrx + TimeOutPA; //Agregado para PP raaf
                                }
                                else
                                if (TipoTrx.Equals("BB"))
                                {
                                    Respuesta = TipoTrx + TimeOutBBPd; //Agregado para BB laba
                                }
                                else
                                {
                                    Respuesta = TipoTrx + TimeOutAut;
                                } 
                                Port.Close();
                                return Respuesta;
                            }
                        }

                        //Validacion del timeout
                        if (Timer > FinSeg)
                        {
                            Port.Close();// Subi el port close
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendPinpad Salgo por timeout= " + FinSeg + " seg", "LogTracerCOM", "log");
                            if (TipoTrx == "IN" || TipoTrx == "ID")
                                Respuesta = TipoTrx + TimeOutIN;
                            else
                                if (TipoTrx.Equals("CP"))
                                {
                                    return Respuesta = TipoTrx + TimeOutCP; //Agregado para CP raaf
                                }
                                else
                                if (TipoTrx.Equals("CT"))
                                {
                                    return Respuesta = TipoTrx + TimeOutCT; //Agregado para CT raaf
                                }
                                else
                                if (TipoTrx.Equals("LT"))
                                {
                                    return Respuesta = TipoTrx + TimeOutLT; //Agregado para LT raaf
                                }
                                else
                                if (TipoTrx.Equals("PP"))
                                {
                                    return Respuesta = TipoTrx + TimeOutPP; //Agregado para PP raaf
                                }
                                else
                                if (TipoTrx.Equals("PC"))
                                {
                                    return Respuesta = TipoTrx + TimeOutPC; //Agregado para LT raaf
                                }
                                else
                                if (TipoTrx.Equals("PA"))
                                {
                                    return Respuesta = TipoTrx + TimeOutPA; //Agregado para PP raaf
                                }
                                else
                                if (TipoTrx.Equals("BB"))
                                {
                                    return Respuesta = TipoTrx + TimeOutBBPd; //Agregado para BB laba
                                }
                                else
                                {
                                    return Respuesta = TipoTrx + TimeOutAut;
                                } 
                            return Respuesta;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR : Exception: " + ex.Message.ToString(), "LogTracerCOM", "log");
                Port.Close();
                if (TipoTrx.Equals("CP"))
                {
                    return Respuesta = TipoTrx + ErrorCPPinpad; //Agregado para CP raaf
                }
                else
                if (TipoTrx.Equals("CT"))
                {
                    return Respuesta = TipoTrx + ErrorCTPinpad; //Agregado para CT raaf
                }
                else
                if (TipoTrx.Equals("LT"))
                {
                    return Respuesta = TipoTrx + ErrorLTPinpad; //Agregado para LT raaf
                }
                else
                if (TipoTrx.Equals("PP"))
                {
                    return Respuesta = TipoTrx + ErrorPPPinpad; //Agregado para PP raaf
                }
                else
                if (TipoTrx.Equals("PC"))
                {
                    return Respuesta = TipoTrx + ErrorPCPinpad; //Agregado para LT raaf
                }
                else
                if (TipoTrx.Equals("PA"))
                {
                    return Respuesta = TipoTrx + ErrorPAPinpad; //Agregado para PP raaf
                }
                else
                if (TipoTrx.Equals("BB"))
                {
                    return Respuesta = TipoTrx + ErrorBBPD; //Agregado para BB laba
                }
                else
                {
                    return Respuesta = TipoTrx + ErrorCnxPinpad;
                }
            }
            #endregion

            if (TipoTrx.Equals("IN"))
            {
                Respuesta = string.Empty;

                if (String.IsNullOrEmpty(RutaBines))
                {
                    Respuesta = SendCantPaq_00(TipoTrx,trama,iPuerto, iBaudios, timeout, iBits, iGrabaLog);
                    return Respuesta;
                }

                if (SendFilePinpad(TipoTrx, trama,iPuerto, iBaudios, timeout, iBits, iRutaBinesOut, RutaBines, iGrabaLog, ref Respuesta) != true)
                {
                    return Respuesta;
                }
                return Respuesta;
            }

            if (TipoTrx.Equals("ID"))
            {
                Respuesta = string.Empty;
                if (String.IsNullOrEmpty(RutaBines))
                {
                    Respuesta = SendCantPaq_00(TipoTrx,trama, iPuerto, iBaudios, timeout, iBits, iGrabaLog);
                    return Respuesta;
                }


                if (InicioDia(TipoTrx, trama, iPuerto, iBaudios,  iBits,timeout,  RutaBines, iGrabaLog, ref Respuesta ,ref Error))
                {
                    return Respuesta;
                }
                else 
                {
                    if (Error == false)
                    {
                        if (SendFilePinpadDia(TipoTrx, trama, iPuerto, iBaudios, timeout, iBits, iRutaBinesOut, RutaBines, iGrabaLog, ref Respuesta) != true)
                        {
                            return Respuesta;
                        }
                    }
                }

                return Respuesta;
            }            

            //transaccion de compra



            try
            {
                Respuesta = string.Empty;
                Port = new SerialPort(iPuerto, iBaudios, Parity.None, iBits, StopBits.One);
                FinSeg = timeout / 1000;
                Port.Open();

                if (Port.IsOpen)
                {
                    byte[] mBuffer = Encoding.ASCII.GetBytes(trama + LRC);
                    Port.Write(mBuffer, 0, mBuffer.Length);

                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendPinpad Autorizacion se envia trama al puerto " + iPuerto + " Trama=" + trama, "LogTracerCOM", "log");
                    while (true)
                    {

                        Thread.Sleep(1000);
                        Timer += 1;

                        byte[] buffer = new byte[Port.BytesToRead];
                        Port.Read(buffer, 0, buffer.Length);
                        Respuesta = System.Text.ASCIIEncoding.ASCII.GetString(buffer);

                        //Validacion del timeout
                        if (Timer > FinSeg)
                        {
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendPinpad Autorizacion salgo por timeout= " + FinSeg + " seg", "LogTracerCOM", "log");
                            if (TipoTrx.Equals("CP"))
                            {
                                Respuesta = TipoTrx + TimeOutCP; //Agregado para CP raaf
                            }
                            else
                            if (TipoTrx.Equals("CT"))
                            {
                                Respuesta = TipoTrx + TimeOutCT; //Agregado para CT raaf
                            }
                            else
                            if (TipoTrx.Equals("LT"))
                            {
                                Respuesta = TipoTrx + TimeOutLT; //Agregado para LT raaf
                            }
                            else
                            if (TipoTrx.Equals("PP"))
                            {
                                Respuesta = TipoTrx + TimeOutPP; //Agregado para PP raaf
                            }
                            else
                            if (TipoTrx.Equals("PC"))
                            {
                                Respuesta = TipoTrx + TimeOutPC; //Agregado para LT raaf
                            }
                            else
                            if (TipoTrx.Equals("PA"))
                            {
                                Respuesta = TipoTrx + TimeOutPA; //Agregado para PP raaf
                            }
                            else
                            if (TipoTrx.Equals("BB"))
                            {
                                Respuesta = TipoTrx + TimeOutBBPd; //Agregado para BB laba
                            }
                            else
                            {
                                Respuesta = TipoTrx + TimeOutAut;
                            }
                            break;
                        }

                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendPinpad Autorizacion leo la respuesta enviada por el pinpad Resp.=" + Respuesta, "LogTracerCOM", "log");

                        I = Respuesta.IndexOf(NAK);

                        if (I == 0)
                        {
                            if (C < 3)
                            {
                                //Vuelvo a enviar la trama 

                                byte[] newBuffer = Encoding.ASCII.GetBytes(trama + LRC);
                                Port.Write(newBuffer, 0, newBuffer.Length);
                                C += 1;
                                Respuesta = string.Empty;
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendPinpad Autorizacion se envia nuevamente la trama intento No." + C + " Trama enviada--> " + trama, "LogTracerCOM", "log");

                            }
                            else
                            {
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendPinpad Autorizacion salgo del proceso que lee el puerto  intento No. " + C + " Respuesta=" + Respuesta, "LogTracerCOM", "log");
                                break;
                            }
                        }


                        //Verifico si la trama de respuesta tiene inicio y fin del mensaje
                        ST = Respuesta.IndexOf(STX);
                        ET = Respuesta.IndexOf(ETX);


                        if (ST == 0 && ET > 0)
                        {
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendPinpad Autorizacion evaluo si la trama de respuesta es correcta calculo el LRC=" + Respuesta.Substring(ET + 1, 1), "LogTracerCOM", "log");

                            //Evaluo si la trama es correcta 
                            if (ComputeCheckSum(Respuesta.Substring(0, ET + 1)) == Convert.ToChar(Respuesta.Substring(ET + 1, 1)))
                            {
                                // Envio ACK                                    
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendPinpad Autorizacion LRC de respuesta es correcto envio ACK y salgo del ciclo respuesta=" + Respuesta, "LogTracerCOM", "log");
                                Port.Write(ACK.ToString());
                                break;
                            }
                            else
                            {
                                if (J < 3)
                                {
                                    Respuesta = string.Empty;
                                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendPinpad Autorizacion la trama de respuesta esta erronea envio NACK trama recibida intento numero " + J, "LogTracerCOM", "log");

                                    Port.Write(NAK.ToString());
                                    J += 1;

                                    if (J == 2)
                                    {
                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendPinpad Autorizacion despues de evaluar 3 veces el LRC de respuesta salgo y se envia respuesta a la caja, dejo de leer el Puerto", "LogTracerCOM", "log");
                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendPinpad Autorizacion LRC ENVIADO POR EL PINPAD NO ENTENDIDO", "LogTracerCOM", "log");
                                        Respuesta = TipoTrx + ErrorCnxPinpad;
                                        break;
                                    }
                                }

                            }
                        }
                    }
                }

                // Cerramos el puerto
                Port.Close();
                ST = Respuesta.IndexOf(STX);
                ET = Respuesta.IndexOf(ETX);

                if (ET > 0)
                {

                    if (Respuesta.Length > ET)
                    {
                        Respuesta = Respuesta.Remove(ET, 1);
                        Respuesta = Respuesta.Remove(ST, 1);
                        Respuesta = Respuesta.Remove(Respuesta.Length - 1, 1);
                    }

                }
                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendPinpad Autorizacion se envia respuesta a la caja respuesta=" + Respuesta, "LogTracerCOM", "log");


                return Respuesta;
            }

            catch (Exception ex)
            {
                if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR :SendPinpad Autorizacion Exception: " + ex.Message.ToString(), "LogTracerCOM", "log");
                Port.Close();
                return TipoTrx + ErrorCnxPinpad;
            }

        }


       
        //Funcion que envia archivo para inicio de dia
        public static bool InicioDia(string TipoTrx, string tramadia, string iPuerto, int iBaudios,int iBits, int timeout, string RutaBines, int iGrabaLog, ref string Respuesta, ref bool Error )
        {
            char STX = (char)0x02;
            char ETX = (char)0x03;
            char ACK = (char)0x06;
            char NAK = (char)0x15;
           
            char LRCDIA;
            char LRCDIARESP;
            Single FinSeg;
            int ET;
            int ST;
            int AC;
            int ET1;
            int ST1;
            int AC1;
            int Timer = 0;
            
            string tramaini = string.Empty;
            string binesfechahora = string.Empty;
            string tramanumpaq = string.Empty;
            SerialPort Port = new SerialPort();
            string tramaresp = string.Empty;
            bool flag = true;

            
            //string RutaBinesOut = string.Empty;
            ASCIIEncoding encoding = new ASCIIEncoding();

            LRCDIA = ComputeCheckSum(tramadia);

            tramaresp = STX + TipoTrx + "00" + ETX ;

            LRCDIARESP = ComputeCheckSum(tramaresp);

            try
            {

                Port = new SerialPort(iPuerto, iBaudios, Parity.None, iBits, StopBits.One);
                FinSeg = timeout / 1000;
                Port.Open();
                if (Port.IsOpen)
                {
                    ArrayList Arrbytes;
                    Arrbytes = leerarchivo(RutaBines);

                    binesfechahora = Arrbytes[0].ToString().Trim().Replace("\r\n", "");

                    byte[] mBufferDia = Encoding.ASCII.GetBytes(tramadia + LRCDIA);
                    Port.Write(mBufferDia, 0, mBufferDia.Length);

                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "InicioDia se envia trama de inicio de dia al puerto " + iPuerto + " Trama inicio de dia=" + tramadia + LRCDIA, "LogTracerCOM", "log");

                    //Validacion de la fecha de creacion del archivo de bines
                    while (true)
                    {

                        Thread.Sleep(1000);
                        Timer += 1;

                        //Validacion del timeout
                        if (Timer > FinSeg)
                        {
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "InicioDia salgo por timeout= " + FinSeg + " seg", "LogTracerCOM", "log");
                            Respuesta = TipoTrx + ErrorCnxPinpad;
                            Port.Close();
                            return false;
                        }

                        byte[] bufferdia = new byte[Port.BytesToRead];
                        Port.Read(bufferdia, 0, bufferdia.Length);
                        Respuesta = System.Text.ASCIIEncoding.ASCII.GetString(bufferdia);
                       
                        if (Respuesta.Length > 0)
                        {
                            //Verifico si la trama de respuesta tiene inicio y fin del mensaje
                            ST = Respuesta.IndexOf(STX);
                            ET = Respuesta.IndexOf(ETX);
                            AC = Respuesta.IndexOf(ACK);

                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "1 ST=" + ST, "LogTracerCOM", "log");
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "2 ET=" + ET, "LogTracerCOM", "log");
                           
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "InicioDia trama inicio de dia leo la respuesta del pinpad=" + Respuesta, "LogTracerCOM", "log");

                            if ((ST == 0 && ET > 0) || (ST > 0 && ET > 0 && AC == 0))
                          
                            {                                
                               if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "3 InicioDia leo trama =" + Respuesta.Substring(ST, ET), "LogTracerCOM", "log");
                               if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "4 InicioDia leo trama inicio de dia lectura evaluo si la trama de respuesta es correcta trama=" + Respuesta.Substring(ST, ET) + " calculo el LRC=" + Respuesta.Substring(ET + 1, 1), "LogTracerCOM", "log");                                                           

                                //Evaluo si la trama es correcta 
                               //Ariel 2013-03-20 se quita ET+1 por ET
                                if (ComputeCheckSum(Respuesta.Substring(ST, ET)) == Convert.ToChar(Respuesta.Substring(ET + 1, 1)))
                                {
                                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "InicioDia comparo el 1er registro archivo=" + binesfechahora + "pos=" + Respuesta.ToString().Trim().Substring(ST + 3, ET - 4), "LogTracerCOM", "log");
                                    
                                    if (flag == true)
                                    {

                                            if (binesfechahora == Respuesta.ToString().Substring(ST + 3, ET - 4).Trim())
                                            {
                                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "InicioDia se envia ACK =" + ACK.ToString(), "LogTracerCOM", "log");
                                              
                                                Port.Write(ACK.ToString());

                                                Thread.Sleep(2000);

                                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "InicioDia 1er registro correcto envio trama " + tramaresp + LRCDIARESP, "LogTracerCOM", "log");
                                               
                                                flag = false;
                                                Port.Write(tramaresp + LRCDIARESP);

                                                // volvemos a inicializar las variables 
                                                Respuesta = string.Empty;
                                                ST = -1;
                                                ET = -1;
                                                AC = -1;

                                                //Validacion de la fecha de creacion del archivo de bines
                                                while (true)
                                                {
                                                    Thread.Sleep(1000);
                                                    Timer += 1;

                                                    //Validacion del timeout
                                                    if (Timer > FinSeg)
                                                    {
                                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "InicioDia salgo por timeout= " + FinSeg + " seg", "LogTracerCOM", "log");
                                                        Respuesta = TipoTrx + TimeOutID;
                                                        Port.Close();
                                                        Error = true;
                                                        return false;                                                       
                                                    }

                                                    byte[] bufferdiaresp = new byte[Port.BytesToRead];
                                                    Port.Read(bufferdiaresp, 0, bufferdiaresp.Length);
                                                 

                                                    Respuesta = System.Text.ASCIIEncoding.ASCII.GetString(bufferdiaresp);
                                              


                                                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "InicioDia trama inicio leo respuesta de trama ID00=" + Respuesta, "LogTracerCOM", "log");

                                                    if (Respuesta.Length > 0)
                                                    {
                                                        //Verifico si la trama de respuesta tiene inicio y fin del mensaje
                                                        ST1 = Respuesta.IndexOf(STX);
                                                        ET1 = Respuesta.IndexOf(ETX);
                                                        AC1 = Respuesta.IndexOf(ACK);

                                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "5 ST1=" + ST1, "LogTracerCOM", "log");
                                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "6 ET1=" + ET1, "LogTracerCOM", "log");
                                                      
                                                        if ((ST1 == 0 && ET1 > 0) || (ST1 > 0 && ET1 > 0 && AC1 == 0))
                                                        {

                                                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "7 InicioDia leo trama =" + Respuesta.Substring(ST1, ET1), "LogTracerCOM", "log");
                                                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "8 InicioDia leo trama inicio de dia lectura evaluo si la trama de respuesta es correcta trama=" + Respuesta.Substring(ST1, ET1+1) + " calculo el LRC=" + Respuesta.Substring(ET1 + 1, 1), "LogTracerCOM", "log");
                                                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "9 InicioDia pura resp.=" + Respuesta.Substring(ST1, ET1), "LogTracerCOM", "log");
                                                            //Evaluo si la trama es correcta 
                                                            //Ariel 2013-03-20 se quita ET1+1 por ET1
                                                            if (ComputeCheckSum(Respuesta.Substring(ST1, ET1)) == Convert.ToChar(Respuesta.Substring(ET1 + 1, 1)))
                                                            {
                                                                Port.Write(ACK.ToString());
                                                                Error = true;
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                Port.Write(NAK.ToString());
                                                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "InicioDia LRC no corresponde a la trama ID00 de Resp.=" + Respuesta, "LogTracerCOM", "log");
                                                                Port.Close();
                                                                Error = true;
                                                                return false;

                                                            }
                                                        }
                                                    }
                                                }                                               
                                            }
                                            else
                                            {
                                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "InicioDia la fecha del archivo del pos es diferente Resp.=" + Respuesta, "LogTracerCOM", "log");
                                                Port.Write(ACK.ToString());
                                                Port.Close();
                                                Error = false;
                                                return false;
                                            }

                                    } // fin de flag

                                    break;
                                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "InicioDia sale del flag  Resp.=" + Respuesta, "LogTracerCOM", "log");
                                   
                                }
                                else
                                {
                                    Port.Write(NAK.ToString());
                                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "InicioDia LRC no corresponde a la trama de Resp.=" + Respuesta, "LogTracerCOM", "log");
                                    Port.Close();
                                    Error = true;
                                    return false;                                     
                                   
                                }
                            }
                        }
                    }


                    ST = Respuesta.IndexOf(STX);
                    ET = Respuesta.IndexOf(ETX);
                    AC = Respuesta.IndexOf(ACK);

                    if (ET > 0)
                    {

                        if (Respuesta.Length > ET)
                        {
                            Respuesta = Respuesta.Remove(ET, 1);
                            Respuesta = Respuesta.Remove(ST, 1);

                            if(AC >=0)
                            {
                                Respuesta = Respuesta.Remove(AC, 1);                            
                            }
                            Respuesta = Respuesta.Remove(Respuesta.Length - 1, 1);
                        }

                    }

                    Port.Close();
                    return true;
                }
                Port.Close();
                
                return false;
            }
            catch (Exception ex)
            {
                if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR :InicioDia Exception: " + ex.Message.ToString(), "LogTracerCOM", "log");
                Port.Close();
                Respuesta = TipoTrx + ErrorCnxPinpad;
                Error = true;
                return false;
            }

        }
        //Funcion que transmite el archivo de bines ordenado
        public static bool SendFilePinpad(string TipoTrx, string trama,string iPuerto, int iBaudios, int timeout, int iBits, string RutaBinesOut, string RutaBines, int iGrabaLog, ref string Respuesta)
        {
            char STX = (char)0x02;
            char ETX = (char)0x03;
            char ACK = (char)0x06;
            char NAK = (char)0x15;

            int ST ;
            int ET;
            int AC;
            char LRCINI;
            char LRC;
            char LRCPaquete;
            Single FinSeg;
            
            int C = 0; // Reintentos cuando envio trama y el pinpad responde nack
            int J = 0; // Reintentos cuando la trama de respuesta no es correcta
            int I;
            int Timer = 0;            
            string tramaini = string.Empty;
            int NumPaq;

            SerialPort Port = new SerialPort();

            int offset = 0;
            int length = 0;
            int dif = 0;
            
            ASCIIEncoding encoding = new ASCIIEncoding();
            DataTable TablaBines = new DataTable();



            try
            {
                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad enviaTramaInicializacion valido el archivo de bines ruta = " + RutaBines, "LogTracerCOM", "log");
                if (validaArchivoBines(RutaBines, ref TablaBines) != true)
                {
                    if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR :SendFilePinpad enviaTramaInicializacion Error al validar archivo de bines", "LogTracerCOM", "log");
                    Respuesta = TipoTrx + FormatoIncorrecto;
                    return false;
                }


                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad enviaTramaInicializacion creamos el archivo de bines Ruta=" + RutaBinesOut, "LogTracerCOM", "log");
                if (CrearArchivoBines(TablaBines, RutaBinesOut) != true)
                {
                    if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR : SendFilePinpad enviaTramaInicializacion error al crear archivo de bines", "LogTracerCOM", "log");
                    Respuesta = TipoTrx + FormatoIncorrecto;
                    return false;
                }
            }
            catch (Exception ex)
            {

                if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR :SendFilePinpad enviaTramaInicializacion exception al crear archivo de bines " + ex.Message, "LogTracerCOM", "log");
                Respuesta = TipoTrx + ErrorCnxPinpad;
                return false; ;
            }
            

            try
            {
                NumPaq = numpaq(RutaBines);

                if (NumPaq == 0)
                {
                    Respuesta = TipoTrx + ArchivoNoExiste;
                    return false;
                }

            }
            catch (Exception ex)
            {
                if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR :SendFilePinpad al obtener numero de paquetes a transmitir " + ex.Message, "LogTracerCOM", "log");
                Respuesta = TipoTrx + ErrorCnxPinpad;
                return false; ; 
            }
            
            
   

            //Constuyo la trama de cantidad de paquetes a transmitir
            tramaini = STX + TipoTrx + NumPaq.ToString().PadLeft(2,'0') + ETX;

            //Calculo LRC
            LRCINI = ComputeCheckSum(tramaini);
            LRC = ComputeCheckSum(trama);
        
          
            #region EnviaTramaInicializacion

            try
                {
                    FinSeg = timeout / 1000;
                    Port = new SerialPort(iPuerto, iBaudios, Parity.None, iBits, StopBits.One);
                    Port.Open();

                    if (Port.IsOpen)
                    {
                        byte[] mBuffer = Encoding.ASCII.GetBytes(trama + LRC);
                        Port.Write(mBuffer, 0, mBuffer.Length);


                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad enviaTramaInicializacion se envia trama " + iPuerto + " Trama=" + trama + LRC, "LogTracerCOM", "log");

                        while (true)
                        {

                            Thread.Sleep(1000);
                            Timer += 1;
                            //Validacion del timeout
                            if (Timer > FinSeg)
                            {
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad enviaTramaInicializacion salgo por timeout = " + FinSeg + " seg", "LogTracerCOM", "log");
                                Respuesta = TipoTrx + TimeOutIN;
                                Port.Close();
                                return false;
                            }

                            byte[] bufferout = new byte[Port.BytesToRead];
                            Port.Read(bufferout, 0, bufferout.Length);
                            Respuesta = System.Text.ASCIIEncoding.ASCII.GetString(bufferout);


                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad enviaTramaInicializacion trama resp enviada por el pinpad Resp.=" + Respuesta, "LogTracerCOM", "log");

                            //Verifico si la trama de respuesta tiene inicio y fin del mensaje
                            ST = Respuesta.IndexOf(STX);
                            ET = Respuesta.IndexOf(ETX);
                            AC = Respuesta.IndexOf(ACK);

                            if ((ST == 0 && ET > 0 ) || (ST > 0 && ET > 0 && AC == 0 ) )                            
                            {
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad enviaTramaInicializacion evaluo si la trama de respuesta es correcta calculo el LRC=" + Respuesta.Substring(ET + 1, 1), "LogTracerCOM", "log");


                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad enviaTramaInicializacion evaluo si la trama de respuesta es correcta calculo el LRC=" + Respuesta.Substring(ET + 1, 1) + "trama=" + Respuesta.Substring(ST, ET ) , "LogTracerCOM", "log");
                                //Evaluo si la trama es correcta 
                                if (ComputeCheckSum(Respuesta.Substring(ST, ET + 1 )) == Convert.ToChar(Respuesta.Substring(ET + 1, 1)))
                                {
                                    Port.Write(ACK.ToString());
                                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad enviaTramaInicializacion LRC de respuesta es correcto salgo ciclo respuesta y respondo ACK =" + Respuesta, "LogTracerCOM", "log");
                                    break;
                                }
                                else
                                {
                                    if (J < 3)
                                    {
                                        Respuesta = string.Empty;
                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad enviaTramaInicializacion la trama de respuesta esta erronea envio NACK trama recibida intento numero " + J, "LogTracerCOM", "log");

                                        Port.Write(NAK.ToString());
                                        J += 1;

                                        if (J == 2)
                                        {
                                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad enviaTramaInicializacion despues de evaluar 3 veces el LRC de respuesta salgo y se envia respuesta a la caja, dejo de leer el Puerto", "LogTracerCOM", "log");
                                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad enviaTramaInicializacion LRC ENVIADO POR EL PINPAD NO ENTENDIDO", "LogTracerCOM", "log");
                                            Respuesta = TipoTrx + ErrorCnxPinpad;
                                            Port.Close();
                                            return false;
                                        }
                                    }

                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR :SendFilePinpad enviaTramaInicializacion Exception: " + ex.Message.ToString(), "LogTracerCOM", "log");
                    Port.Close();
                    Respuesta = TipoTrx + ErrorCnxPinpad;
                    return false;
                }
           

            #endregion

            #region EnviaTramaCantPaq


            try
            {
                FinSeg = timeout / 1000;
               
                if (Port.IsOpen)
                {
                    Thread.Sleep(2000);

                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad Incio el proceso para enviar la cant. de paquetes que vamos a transmitir trama=" + tramaini + LRCINI, "LogTracerCOM", "log");
               
                    byte[] mBufferCantPaq = Encoding.ASCII.GetBytes(tramaini + LRCINI);
                    Port.Write(mBufferCantPaq, 0, mBufferCantPaq.Length);

                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad se envia trama de cantidad de paquetes " + iPuerto + " TramaCantPaq=" + tramaini + LRCINI, "LogTracerCOM", "log");

                    while (true)
                    {
                        Thread.Sleep(1000);
                        Timer += 1;
                        //Validacion del timeout
                        if (Timer > FinSeg)
                        {
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad trama cant. paquetes salgo por timeout= " + FinSeg + " seg", "LogTracerCOM", "log");
                            Respuesta = TipoTrx + TimeOutIN;
                            Port.Close();
                            return false;
                        }

                        byte[] bufferoutcantpaq = new byte[Port.BytesToRead];
                        Port.Read(bufferoutcantpaq, 0, bufferoutcantpaq.Length);
                        Respuesta = System.Text.ASCIIEncoding.ASCII.GetString(bufferoutcantpaq);

                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad trama de cant. paquetes leo resp enviada por el pinpad Resp.=" + Respuesta, "LogTracerCOM", "log");

                        if (Respuesta.Length > 0)
                        {
                            int index = Respuesta.IndexOf(ACK);
                            if (index == 0)
                            {
                                break;
                            }
                           /* else
                            {
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad trama de cant. paquetes se vuelve a enviar la trama=" + tramaini + LRCINI, "LogTracerCOM", "log");
                                byte[] mBuffercantpaq = Encoding.ASCII.GetBytes(tramaini + LRCINI);
                                Port.Write(mBuffercantpaq, 0, mBuffercantpaq.Length);

                            }*/
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR :SendFilePinpad cant paquetes Exception: " + ex.Message.ToString(), "LogTracerCOM", "log");
                Port.Close();
                Respuesta = TipoTrx + ErrorCnxPinpad;
                return false;
            }
            #endregion

            #region transmitepaquetes

            try
            {
                FinSeg = timeout / 1000;


                if (Port.IsOpen)
                {
                    
                    //Open the stream and read it back.
                    using (FileStream fs = File.OpenRead(RutaBinesOut))
                    {
                        // Read the source file into a byte array.
                        byte[] b = new byte[fs.Length];

                        UTF8Encoding temp = new UTF8Encoding(true);

                        if (b.Length <= 400)
                        {
                            length = b.Length;
                        }
                        else
                        {
                            length = 400;
                            dif = b.Length;
                        }

                        while (fs.Read(b, offset, length) > 0)
                        {
                            string Paquete = temp.GetString(b, offset, length);                            
                            Paquete = STX + TipoTrx + Paquete.ToString().PadRight(400,' ') + ETX;
                            LRCPaquete = ComputeCheckSum(Paquete);

                            dif = dif - length;

                            if (dif > 0 || dif == 0)
                            {
                                if (dif > 400)
                                {
                                    offset += 400;
                                }
                                else
                                {
                                    offset += 400;
                                    length = dif;

                                    if (offset > b.Length)
                                    {
                                        offset = b.Length;
                                    }
                                }

                            }
                            
                            

                            byte[] mBufferFile = Encoding.ASCII.GetBytes(Paquete + LRCPaquete);
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad transmite paquetes se escribe el paquete=" + Paquete + LRCPaquete, "LogTracerCOM", "log");
                            Port.Write(mBufferFile, 0, mBufferFile.Length);
                          

                            while (true)
                            {
                                Thread.Sleep(1000);
                                Timer += 1;
                                //Validacion del timeout
                                if (Timer > FinSeg)
                                {
                                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad transmite paquetes salgo por timeout paquetes = " + FinSeg + " seg", "LogTracerCOM", "log");
                                    Respuesta = TipoTrx + TimeOutIN;
                                    Port.Close();
                                    return false;
                                }
                                byte[] bufferinicio = new byte[Port.BytesToRead];
                                Port.Read(bufferinicio, 0, bufferinicio.Length);
                                Respuesta = System.Text.ASCIIEncoding.ASCII.GetString(bufferinicio);

                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad transmite paquetes trama de Inicializacion Leo la respuesta enviada por el pinpad Resp.=" + Respuesta, "LogTracerCOM", "log");
                              

                                I = Respuesta.IndexOf(ACK);

                                if (I == 0)
                                {
                                    //if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Se envia el siguiente paquete" + Paquete + LRCPaquete, "LogTracerCOM", "log");
                                    break;
                                }
                                else
                                {
                                    if (C < 3)
                                    {
                                        //Vuelvo a enviar la trama 
                                        byte[] newBuffer = Encoding.ASCII.GetBytes(Paquete + LRCPaquete);
                                        Port.Write(newBuffer, 0, newBuffer.Length);
                                        C += 1;
                                        Respuesta = string.Empty;
                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad transmite paquetes se envia nuevamente la trama intento No." + C + " Paquete enviada--> " + Paquete, "LogTracerCOM", "log");

                                    }
                                    else
                                    {
                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad transmite paquetes si la respuesta no es ACK salgo del proceso que lee el puerto  intento No. " + C + " Respuesta=" + Respuesta, "LogTracerCOM", "log");
                                        Port.Close();
                                        Respuesta = TipoTrx + ErrorCnxPinpad;
                                        return false;
                                    }

                                }

                            }
                        }

#endregion

            #region ultimaleidaPinpad

                        //Capturamos la ultima respuesta del pinpad
                        Respuesta = string.Empty;
                        ST = -1;
                        ET = -1;
                        AC = -1;
                            
                        while (true)
                        {

                            Thread.Sleep(1000);
                            Timer += 1;
                            //Validacion del timeout
                            if (Timer > FinSeg)
                            {
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad ultima lectura salgo por timeout esperando mensaje de inicialización tiempo = " + FinSeg + " seg", "LogTracerCOM", "log");
                                Respuesta = TipoTrx + TimeOutIN;
                                Port.Close();
                                return false;
                            }

                            byte[] buffermsg = new byte[Port.BytesToRead];
                            Port.Read(buffermsg, 0, buffermsg.Length);
                            Respuesta = System.Text.ASCIIEncoding.ASCII.GetString(buffermsg);
                            

                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad ultima lectura Trama resp enviada por el pinpad Resp.=" + Respuesta, "LogTracerCOM", "log");

                            //Verifico si la trama de respuesta tiene inicio y fin del mensaje
                            ST = Respuesta.IndexOf(STX);
                            ET = Respuesta.IndexOf(ETX);
                            AC = Respuesta.IndexOf(ACK);

                            if ((ST == 0 && ET > 0) || (ST > 0 && ET > 0 && AC == 0))                           
           
                            {
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad ultima lectura Evaluo si la trama de respuesta es correcta calculo el LRC=" + Respuesta.Substring(ET + 1, 1), "LogTracerCOM", "log");
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad ultima lectura Evaluo ST=" + ST + "ET="+ ET + "si la trama=" + Respuesta.Substring(ST, ET )+ " de respuesta es correcta calculo el LRC=" + Respuesta.Substring(ET + 1, 1), "LogTracerCOM", "log");
                                
                                //Evaluo si la trama es correcta 
                                if (ComputeCheckSum(Respuesta.Substring(ST, ET + 1 )) == Convert.ToChar(Respuesta.Substring(ET + 1, 1)))
                                {
                                    Port.Write(ACK.ToString());
                                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad ultima lectura LRC de respuesta es correcto se envia ACK y sale del ciclo respuesta=" + Respuesta, "LogTracerCOM", "log");
                                    break;
                                }
                                else
                                {
                                    if (J < 3)
                                    {
                                        Respuesta = string.Empty;
                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad ultima lectura la trama de respuesta esta erronea envio NACK trama recibida intento numero " + J, "LogTracerCOM", "log");

                                        Port.Write(NAK.ToString());
                                        J += 1;

                                        if (J == 2)
                                        {
                                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad ultima lectura Despues de evaluar 3 veces el LRC de respuesta salgo y se envia respuesta a la caja, dejo de leer el Puerto", "LogTracerCOM", "log");
                                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpad LRC ENVIADO POR EL PINPAD NO ENTENDIDO INICIALIZACION FINAL", "LogTracerCOM", "log");
                                            Respuesta = TipoTrx + ErrorCnxPinpad;
                                            Port.Close();
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Cerramos el puerto
                Port.Close();
                ST = Respuesta.IndexOf(STX);
                ET = Respuesta.IndexOf(ETX);
                AC = Respuesta.IndexOf(ACK);

                if (ET > 0)
                {

                    if (Respuesta.Length > ET)
                    {
                        Respuesta = Respuesta.Remove(ET, 1);
                        Respuesta = Respuesta.Remove(ST, 1);
                        if (AC >= 0)
                        {
                            Respuesta = Respuesta.Remove(AC, 1);
                        }
                        
                        Respuesta = Respuesta.Remove(Respuesta.Length - 1, 1);
                    }

                }
                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Se envia respuesta a la caja respuesta=" + Respuesta, "LogTracerCOM", "log");
                return true;                
            }

            catch (Exception ex)
            {
                if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR : Exception: " + ex.Message.ToString(), "LogTracerCOM", "log");
                Port.Close();
                Respuesta = TipoTrx + ErrorCnxPinpad;
                return false;
            }
#endregion

        }
        static DataTable GetTable()
        {            
            DataTable table = new DataTable();
       
            table.Columns.Add("bininicio", typeof(string));
            table.Columns.Add("binfin", typeof(string));
            table.Columns.Add("adquiriente", typeof(string));
            table.Columns.Add("resultado", typeof(int));
            table.Columns.Add("cadena", typeof(string));
            return table;
        }
        public MemoryStream MemoryFile(DataTable dt)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            MemoryStream memStream = new MemoryStream();
            foreach (DataRow item in dt.Rows)
	        {
        	        byte[] firstString = encoding.GetBytes((string)item["cadena"]);
                    using(memStream)
                    {
                    // Write the first string to the stream.
                       memStream.Write(firstString, 0 , firstString.Length);
                    }
	        }

            return memStream;
        }     
        public static MemoryStream MemoryFile(string rutabines)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            MemoryStream memStream = new MemoryStream();

            try
            {
                MemoryStream ms = new MemoryStream();
                FileStream file = new FileStream(rutabines, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                file.Close();
                ms.Close();

             
                return ms;
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        public static ArrayList leerarchivo(string ruta)
        {
        StreamReader objReader = new StreamReader(ruta);
			string sLine="";
			ArrayList arrText = new ArrayList();

			while (sLine != null)
			{
				sLine = objReader.ReadLine();
				if (sLine != null)
					arrText.Add(sLine + "\r\n");
			}
                        objReader.Close();
                        return arrText;
        }

        public static bool validaArchivoBines(string Ruta, ref DataTable tablabines)
        {
            
            string BinInicio = string.Empty;
            string BinFin = string.Empty;
            string Adquiriente = string.Empty;
            string Cadena = string.Empty;
            Int64 result;

            bool flag = false;

            DataTable dt;
            dt = GetTable();

            try
            {
                StreamReader objReader = new StreamReader(Ruta);
                string sLine = "";
                ArrayList arrText = new ArrayList();

                while (sLine != null)
                {
                    sLine = objReader.ReadLine();

                    if (sLine != null)
                    {
                        if (flag == true)
                        {
                            BinInicio = sLine.Substring(0, 6);
                            BinFin = sLine.Substring(6, 6);
                            Adquiriente = sLine.Substring(12, 4);
                            result = int.Parse(BinFin) - int.Parse(BinInicio);
                            if (result < 0)
                            {
                                tablabines = null;
                                return false;
                            
                            }

                            dt.Rows.Add(BinInicio, BinFin, Adquiriente, result, sLine);
                        }
                        else
                        {
                            dt.Rows.Add(null, null, null, 0, sLine);
                            flag = true;
                        }       
                    }                                                
                }

                objReader.Close();

                DataView dv = dt.DefaultView;
                dv.Sort = "bininicio,resultado asc";
                DataTable sortedDT = dv.ToTable();

                //Clono la tabla de rango de bines
                DataTable cdt;
                cdt = dt.Clone();

                DataTable cdtbines;
                cdtbines = dt.Clone();
                string rag;

                sortedDT.Rows.Add(" ",null,null, 0,"");

                cdtbines.Rows.Add("", "", "", 0, sortedDT.Rows[0]["cadena"].ToString());
                           
                       int j = 0;
                        for (int c1 = 1; c1 < sortedDT.Rows.Count -1; c1++)
                        {                          
                            rag = sortedDT.Rows[j + 1]["bininicio"].ToString().Substring(0, 1);
                            if (rag == sortedDT.Rows[c1 + 1]["bininicio"].ToString().Substring(0, 1))
                            {
                                cdt.Rows.Add(sortedDT.Rows[j + 1]["bininicio"].ToString(), sortedDT.Rows[j + 1]["binfin"].ToString(), sortedDT.Rows[j + 1]["adquiriente"].ToString(), sortedDT.Rows[j + 1]["resultado"].ToString(), sortedDT.Rows[j + 1]["cadena"].ToString());
                            }
                            else
                            {
                                cdt.Rows.Add(sortedDT.Rows[j + 1]["bininicio"].ToString(), sortedDT.Rows[j + 1]["binfin"].ToString(), sortedDT.Rows[j + 1]["adquiriente"].ToString(), sortedDT.Rows[j + 1]["resultado"].ToString(), sortedDT.Rows[j + 1]["cadena"].ToString());
                                DataView dv1 = cdt.DefaultView;
                                dv1.Sort = " resultado asc";
                                DataTable sortedDT1 = dv1.ToTable();
                                foreach (DataRow fila in sortedDT1.Rows)
                                {
                                    cdtbines.Rows.Add(fila["bininicio"].ToString(), fila["binfin"].ToString(), fila["adquiriente"].ToString(), fila["resultado"].ToString(), fila["cadena"].ToString());

                                }
                                cdt = new DataTable();
                                cdt = dt.Clone();
                            }
                            j += 1;                                         
                    }
                        tablabines = cdtbines;
                        return true;
            }
            catch (Exception ex)
            {
                tablabines = null;
                return false;               
            }
        
        }
        public static bool CrearArchivoBines(DataTable dt, string RutaDestino)
        {

                  try
                    {
                        StreamWriter myStreamWriter = new StreamWriter(RutaDestino, false);
                           foreach (DataRow item in dt.Rows)
                            {
                                 myStreamWriter.WriteLine(item["cadena"].ToString());
                            }

                              myStreamWriter.Close();
                              return true;    
                        }
                    
                        catch (Exception ex)
                        {
                            return false;
                        }
                    
            }

        public static int numpaq(string RutaBinesOut)        
        { 
            int length;
            int dif=0;
            int offset=0;
            int totpaq=0;


            try
            {

                //Open the stream and read it back.
                using (FileStream fs = File.OpenRead(RutaBinesOut))
                {
                    // Read the source file into a byte array.
                    byte[] b = new byte[fs.Length];
                    UTF8Encoding temp = new UTF8Encoding(true);

                    if (b.Length <= 400)
                    {
                        length = b.Length;
                    }
                    else
                    {
                        length = 400;
                        dif = b.Length;
                    }

                    while (fs.Read(b, offset, length) > 0)
                    {
                        dif = dif - length;
                        if (dif > 0)
                        {
                            if (dif > 400)
                            {
                                offset += 400;
                            }
                            else
                            {
                                offset += 400;
                                length = dif;
                                if (offset > b.Length)
                                {
                                    offset = b.Length;
                                }
                            }
                        }
                        totpaq += 1;
                    }
                }

                return totpaq;

            }
            catch (Exception ex)
            {
                return 0;
                
            }
        }


        //funcion que envia el archivo cuando es inicio de dia         
        public static bool SendFilePinpadDia(string TipoTrx, string trama, string iPuerto, int iBaudios, int timeout, int iBits, string RutaBinesOut, string RutaBines, int iGrabaLog, ref string Respuesta)
        {
            char STX = (char)0x02;
            char ETX = (char)0x03;
            char ACK = (char)0x06;
            char NAK = (char)0x15;

            int ST;
            int ET;
            int AC;
            char LRCINI;
            char LRC;
            char LRCPaquete;



            Single FinSeg;

            int C = 0; // Reintentos cuando envio trama y el pinpad responde nack
            int J = 0; // Reintentos cuando la trama de respuesta no es correcta
            int I;
            int Timer = 0;
            string tramaini = string.Empty;
            int NumPaq;

            SerialPort Port = new SerialPort();

            int offset = 0;
            int length = 0;
            int dif = 0;

            ASCIIEncoding encoding = new ASCIIEncoding();
            DataTable TablaBines = new DataTable();
            


            try
            {
                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia valido el archivo de bines ruta = " + RutaBines, "LogTracerCOM", "log");
                if (validaArchivoBines(RutaBines, ref TablaBines) != true)
                {
                    if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR :SendFilePinpadDia Error al validar archivo de bines", "LogTracerCOM", "log");
                    Respuesta = TipoTrx + FormatoIncorrecto;
                    return false;
                }


                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia creamos el archivo de bines Ruta=" + RutaBinesOut, "LogTracerCOM", "log");
                if (CrearArchivoBines(TablaBines, RutaBinesOut) != true)
                {
                    if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR :SendFilePinpadDia Error al crear archivo de bines", "LogTracerCOM", "log");
                    Respuesta = TipoTrx + FormatoIncorrecto;
                    return false;
                }
            }
            catch (Exception ex)
            {

                if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR :SendFilePinpadDia Exception al crear archivo de bines " + ex.Message, "LogTracerCOM", "log");
                Respuesta = TipoTrx + ErrorCnxPinpad;
                return false; ;
            }



            try
            {
                NumPaq = numpaq(RutaBines);
            }
            catch (Exception ex )
            {
                Respuesta = TipoTrx + FormatoIncorrecto;
                if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR :SendFilePinpadDia Exception al obtener numero de paquetes del archivo de bines " + ex.Message, "LogTracerCOM", "log");
                return false;
            }
                
            

            if (NumPaq == 0)
            {
                Respuesta = TipoTrx + FormatoIncorrecto;
                if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR :SendFilePinpadDia numero de paquetes igual a cero", "LogTracerCOM", "log");
                return false;
            }

            
            //Constuyo la trama de cantidad de paquetes a transmitir
            tramaini = STX + TipoTrx + NumPaq.ToString().PadLeft(2, '0') + ETX;

            //Calculo LRC
            LRCINI = ComputeCheckSum(tramaini);

           


            #region EnviaTramaCantPaq


            try
            {
                FinSeg = timeout / 1000;

                Port = new SerialPort(iPuerto, iBaudios, Parity.None, iBits, StopBits.One);
                Port.Open();

                if (Port.IsOpen)
                {
                    Thread.Sleep(2000);

                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia incio el proceso para enviar la cant. de paquetes que vamos a transmitir trama=" + tramaini + LRCINI, "LogTracerCOM", "log");

                    byte[] mBufferCantPaq = Encoding.ASCII.GetBytes(tramaini + LRCINI);
                    Port.Write(mBufferCantPaq, 0, mBufferCantPaq.Length);

                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia se envia trama de cantidad de paquetes " + iPuerto + " TramaCantPaq=" + tramaini + LRCINI, "LogTracerCOM", "log");

                    while (true)
                    {

                        Thread.Sleep(1000);
                        Timer += 1;
                        //Validacion del timeout
                        if (Timer > FinSeg)
                        {
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia salgo por timeout= " + FinSeg + " seg", "LogTracerCOM", "log");
                            Respuesta = TipoTrx + TimeOutID;
                            Port.Close();
                            return false;
                        }

                        byte[] bufferoutcantpaq = new byte[Port.BytesToRead];
                        Port.Read(bufferoutcantpaq, 0, bufferoutcantpaq.Length);
                        Respuesta = System.Text.ASCIIEncoding.ASCII.GetString(bufferoutcantpaq);

                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia trama de cant. paquetes leo resp enviada por el pinpad Resp.=" + Respuesta, "LogTracerCOM", "log");

                        if (Respuesta.Length > 0)
                        {
                            int index = Respuesta.IndexOf(ACK);
                            if (index == 0)
                            {
                                break;
                            }
                            else
                            {
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia trama de cant. paquetes se vuelve a enviar la trama=" + tramaini + LRCINI, "LogTracerCOM", "log");
                                byte[] mBuffercantpaq = Encoding.ASCII.GetBytes(tramaini + LRCINI);
                                Port.Write(mBuffercantpaq, 0, mBuffercantpaq.Length);

                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR :SendFilePinpadDiaCant paquetes Exception: " + ex.Message.ToString(), "LogTracerCOM", "log");
                Port.Close();
                Respuesta = TipoTrx + ErrorCnxPinpad;
                return false;
            }
            #endregion

            #region transmitepaquetes

            try
            {
                FinSeg = timeout / 1000;


                if (Port.IsOpen)
                {

                    //Open the stream and read it back.
                    using (FileStream fs = File.OpenRead(RutaBinesOut))
                    {
                        // Read the source file into a byte array.
                        byte[] b = new byte[fs.Length];

                        UTF8Encoding temp = new UTF8Encoding(true);

                        if (b.Length <= 400)
                        {
                            length = b.Length;
                        }
                        else
                        {
                            length = 400;
                            dif = b.Length;
                        }

                        while (fs.Read(b, offset, length) > 0)
                        {
                            string Paquete = temp.GetString(b, offset, length);
                            Paquete = STX + TipoTrx + Paquete.ToString().PadRight(400, ' ') + ETX;
                            LRCPaquete = ComputeCheckSum(Paquete);

                            dif = dif - length;

                            if (dif > 0 || dif == 0)
                            {
                                if (dif > 400)
                                {
                                    offset += 400;
                                }
                                else
                                {
                                    offset += 400;
                                    length = dif;

                                    if (offset > b.Length)
                                    {
                                        offset = b.Length;
                                    }
                                }
                            }


                            byte[] mBufferFile = Encoding.ASCII.GetBytes(Paquete + LRCPaquete);
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia se escribe el paquete=" + Paquete + LRCPaquete, "LogTracerCOM", "log");
                            Port.Write(mBufferFile, 0, mBufferFile.Length);


                            while (true)
                            {
                                Thread.Sleep(1000);
                                Timer += 1;
                                //Validacion del timeout
                                if (Timer > FinSeg)
                                {
                                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia salgo por timeout paquetes = " + FinSeg + " seg", "LogTracerCOM", "log");
                                    Respuesta = TipoTrx + TimeOutID;
                                    Port.Close();
                                    return false;
                                }
                                byte[] bufferinicio = new byte[Port.BytesToRead];
                                Port.Read(bufferinicio, 0, bufferinicio.Length);
                                Respuesta = System.Text.ASCIIEncoding.ASCII.GetString(bufferinicio);

                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia trama de Inicializacion Leo la respuesta enviada por el pinpad Resp.=" + Respuesta, "LogTracerCOM", "log");
                                
                                I = Respuesta.IndexOf(ACK);

                                if (I == 0)
                                {
                                    //if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Se envia el siguiente paquete" + Paquete + LRCPaquete, "LogTracerCOM", "log");
                                    break;
                                }
                                else
                                {
                                    if (C < 3)
                                    {
                                        //Vuelvo a enviar la trama 
                                        byte[] newBuffer = Encoding.ASCII.GetBytes(Paquete + LRCPaquete);
                                        Port.Write(newBuffer, 0, newBuffer.Length);
                                        C += 1;
                                        Respuesta = string.Empty;
                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia se envia nuevamente la trama intento No." + C + " Paquete enviada--> " + Paquete, "LogTracerCOM", "log");

                                    }
                                    else
                                    {
                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia si la respuesta no es ACK salgo del proceso que lee el puerto  intento No. " + C + " Respuesta=" + Respuesta, "LogTracerCOM", "log");
                                        Port.Close();
                                        Respuesta = TipoTrx + ErrorCnxPinpad;
                                        return false;
                                    }

                                }

                            }
                        }

            #endregion

                        #region ultimaleidaPinpad



                        //Capturamos la ultima respuesta del pinpad
                        Respuesta = string.Empty;
                        ST = -1;
                        ET = -1;
                        AC = -1;


                        while (true)
                        {

                            Thread.Sleep(1000);
                            Timer += 1;
                            //Validacion del timeout
                            if (Timer > FinSeg)
                            {
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia ultima lectura salgo por timeout esperando mensaje de inicialización tiempo = " + FinSeg + " seg", "LogTracerCOM", "log");
                                Respuesta = TipoTrx + TimeOutID;
                                Port.Close();
                                return false;
                            }

                            byte[] buffermsg = new byte[Port.BytesToRead];
                            Port.Read(buffermsg, 0, buffermsg.Length);
                            
                            Respuesta = System.Text.ASCIIEncoding.ASCII.GetString(buffermsg);


                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia ultima lectura Trama resp enviada por el pinpad Resp.=" + Respuesta, "LogTracerCOM", "log");

                            //Verifico si la trama de respuesta tiene inicio y fin del mensaje
                            ST = Respuesta.IndexOf(STX);
                            ET = Respuesta.IndexOf(ETX);
                            AC = Respuesta.IndexOf(ACK);

                            if ((ST == 0 && ET > 0) || (ST > 0 && ET > 0 && AC == 0))
                            
                            {
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia ultima lectura Evaluo si la trama de respuesta es correcta calculo el LRC=" + Respuesta.Substring(ET + 1, 1), "LogTracerCOM", "log");

                                //Evaluo si la trama es correcta 
                                if (ComputeCheckSum(Respuesta.Substring(ST, ET + 1)) == Convert.ToChar(Respuesta.Substring(ET + 1, 1)))
                                {
                                    Port.Write(ACK.ToString());
                                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia ultima lectura LRC de respuesta es correcto se envia ACK y sale del ciclo respuesta=" + Respuesta, "LogTracerCOM", "log");
                                    break;
                                }
                                else
                                {
                                    if (J < 3)
                                    {
                                        Respuesta = string.Empty;
                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia ultima lectura la trama de respuesta esta erronea envio NACK trama recibida intento numero " + J, "LogTracerCOM", "log");

                                        Port.Write(NAK.ToString());
                                        J += 1;

                                        if (J == 2)
                                        {
                                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia ultima lectura Despues de evaluar 3 veces el LRC de respuesta salgo y se envia respuesta a la caja, dejo de leer el Puerto", "LogTracerCOM", "log");
                                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia LRC ENVIADO POR EL PINPAD NO ENTENDIDO INICIALIZACION FINAL", "LogTracerCOM", "log");
                                            Respuesta = TipoTrx + ErrorCnxPinpad;
                                            Port.Close();
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Cerramos el puerto
                Port.Close();
                ST = Respuesta.IndexOf(STX);
                ET = Respuesta.IndexOf(ETX);
                AC = Respuesta.IndexOf(ACK);

                if (ET > 0)
                {

                    if (Respuesta.Length > ET)
                    {
                        Respuesta = Respuesta.Remove(ET, 1);
                        Respuesta = Respuesta.Remove(ST, 1);
                        if(AC >=0)
                        {
                            Respuesta = Respuesta.Remove(AC, 1);
                        }
                        
                        Respuesta = Respuesta.Remove(Respuesta.Length - 1, 1);
                    }

                }
                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendFilePinpadDia Se envia respuesta a la caja respuesta=" + Respuesta, "LogTracerCOM", "log");
                return true;
            }

            catch (Exception ex)
            {
                if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR :SendFilePinpadDia Exception: " + ex.Message.ToString(), "LogTracerCOM", "log");
                Port.Close();
                Respuesta = TipoTrx + ErrorCnxPinpad;
                return false;
            }
        }
#endregion
       
                   
        
        
        #region EnviaTramaCantPaq_00

        //funcion que envia el archivo cuando es inicio de dia         
        public static string SendCantPaq_00(string TipoTrx, string trama, string iPuerto, int iBaudios, int timeout,int iBits, int iGrabaLog)
        {

            char STX = (char)0x02;
            char ETX = (char)0x03;
            char ACK = (char)0x06;
            char NAK = (char)0x15;
            char LRCPaquete;
            char LRC;
            int I;

            Single FinSeg;

            int ET;
            int ST;
            int AC;
            int C = 0; // Reintentos cuando envio trama y el pinpad responde nack
            int J = 0; // Reintentos cuando la trama de respuesta no es correcta
            int Timer = 0;
            
            string tramapaq= string.Empty;
            
            int NumPaq;

            SerialPort Port = new SerialPort();
            
            ASCIIEncoding encoding = new ASCIIEncoding();
           
            string Respuesta;

            NumPaq = 00;

          


            //Constuyo la trama de cantidad de paquetes a transmitir
            tramapaq = STX + TipoTrx + NumPaq.ToString().PadLeft(2, '0') + ETX;

            //Calculo LRC
            LRC = ComputeCheckSum(trama);
                        

            //Calculo LRCPaquete
            LRCPaquete = ComputeCheckSum(tramapaq);
            try
            {
                
                Port = new SerialPort(iPuerto, iBaudios, Parity.None, iBits, StopBits.One);
                Port.Open();
                FinSeg = timeout / 1000;
                if (Port.IsOpen)
                {
                    Thread.Sleep(2000);
                    byte[] mBuffer = Encoding.ASCII.GetBytes(trama + LRC);
                    Port.Write(mBuffer, 0, mBuffer.Length);
                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendCantPaq_trama  Se envia trama de cantidad de paquetes " + iPuerto + " TramaCantPaq=" + trama + LRC, "LogTracerCOM", "log");
                    while (true)
                    {
                        Thread.Sleep(1000);
                        Timer += 1;
                        //Validacion del timeout
                        if (Timer > FinSeg)
                        {
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendCantPaq_trama Salgo por timeout= " + FinSeg + " seg", "LogTracerCOM", "log");
                            Respuesta = TipoTrx == "IN" ? TimeOutIN.ToString() : TimeOutID.ToString();
                            Port.Close();
                            return Respuesta;
                        }


                        byte[] bufferout = new byte[Port.BytesToRead];
                        Port.Read(bufferout, 0, bufferout.Length);
                        Respuesta = System.Text.ASCIIEncoding.ASCII.GetString(bufferout);

                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Lectura SendCantPaq_trama trama enviada por el pinpad Resp.=" + Respuesta, "LogTracerCOM", "log");

                        //Verifico si la trama de respuesta tiene inicio y fin del mensaje
                        ST = Respuesta.IndexOf(STX);
                        ET = Respuesta.IndexOf(ETX);
                        AC = Respuesta.IndexOf(ACK);

                        if ((ST == 0 && ET > 0) || (ST > 0 && ET > 0 && AC == 0))
                        {
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Lectura SendCantPaq_trama evaluo si la trama de respuesta es correcta calculo el LRC=" + Respuesta.Substring(ET + 1, 1), "LogTracerCOM", "log");

                            //Evaluo si la trama es correcta 
                            if (ComputeCheckSum(Respuesta.Substring(ST, ET + 1)) == Convert.ToChar(Respuesta.Substring(ET + 1, 1)))
                            {
                                Port.Write(ACK.ToString());
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Escribe SendCantPaq_trama LRC de respuesta es correcto se envia ACK " + ACK.ToString(), "LogTracerCOM", "log");
                                break;
                            }
                            else
                            {
                                if (J < 3)
                                {
                                    Respuesta = string.Empty;
                                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendCantPaq_trama lectura la trama de respuesta esta erronea envio NACK trama recibida intento numero " + J + " NACK=" + NAK.ToString(), "LogTracerCOM", "log");

                                    Port.Write(NAK.ToString());
                                    J += 1;

                                    if (J == 2)
                                    {
                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendCantPaq_trama lectura Despues de evaluar 3 veces el LRC de respuesta salgo y se envia respuesta a la caja, dejo de leer el Puerto", "LogTracerCOM", "log");
                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendCantPaq_trama LRC ENVIADO POR EL PINPAD NO ENTENDIDO INICIALIZACION FINAL", "LogTracerCOM", "log");
                                        Respuesta = TipoTrx + ErrorCnxPinpad;
                                        Port.Close();
                                        return Respuesta;
                                    }
                                }
                            }
                        }
                    }
                }

                FinSeg = timeout / 1000;
                if (Port.IsOpen)
                {
                    Thread.Sleep(2000);
                    byte[] mBufferCantPaq = Encoding.ASCII.GetBytes(tramapaq + LRCPaquete);
                    Port.Write(mBufferCantPaq, 0, mBufferCantPaq.Length);
                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendCantPaq_00  Se envia trama de cantidad de paquetes " + iPuerto + " TramaCantPaq=" + tramapaq + LRCPaquete, "LogTracerCOM", "log");
                    while (true)
                    {
                        Thread.Sleep(1000);
                        Timer += 1;
                        //Validacion del timeout
                        if (Timer > FinSeg)
                        {
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendCantPaq_00 Salgo por timeout= " + FinSeg + " seg", "LogTracerCOM", "log");
                            Respuesta = TipoTrx == "IN" ? TimeOutIN.ToString() :TimeOutID.ToString();
                            Port.Close();
                            return Respuesta;
                        }

                        byte[] bufferoutcantpaq = new byte[Port.BytesToRead];
                        Port.Read(bufferoutcantpaq, 0, bufferoutcantpaq.Length);
                        Respuesta = System.Text.ASCIIEncoding.ASCII.GetString(bufferoutcantpaq);

                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Lectura SendCantPaq_00 trama enviada por el pinpad Resp.=" + Respuesta, "LogTracerCOM", "log");

                        //Verifico si la trama de respuesta tiene inicio y fin del mensaje
                        ST = Respuesta.IndexOf(STX);
                        ET = Respuesta.IndexOf(ETX);
                        AC = Respuesta.IndexOf(ACK);

                        if ((ST == 0 && ET > 0) || (ST > 0 && ET > 0 && AC == 0))
                        {
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Lectura SendCantPaq_00 evaluo si la trama de respuesta es correcta calculo el LRC=" + Respuesta.Substring(ET + 1, 1), "LogTracerCOM", "log");

                            //Evaluo si la trama es correcta 
                            if (ComputeCheckSum(Respuesta.Substring(ST, ET + 1)) == Convert.ToChar(Respuesta.Substring(ET + 1, 1)))
                            {
                                Port.Write(ACK.ToString());
                                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Escribe SendCantPaq_00 LRC de respuesta es correcto se envia ACK " + ACK.ToString(), "LogTracerCOM", "log");
                                break;
                            }
                            else
                            {
                                if (J < 3)
                                {
                                    Respuesta = string.Empty;
                                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendCantPaq_00 lectura la trama de respuesta esta erronea envio NACK trama recibida intento numero " + J + " NACK=" +  NAK.ToString(), "LogTracerCOM", "log");

                                    Port.Write(NAK.ToString());
                                    J += 1;

                                    if (J == 2)
                                    {
                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendCantPaq_00 lectura Despues de evaluar 3 veces el LRC de respuesta salgo y se envia respuesta a la caja, dejo de leer el Puerto", "LogTracerCOM", "log");
                                        if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendCantPaq_00 LRC ENVIADO POR EL PINPAD NO ENTENDIDO INICIALIZACION FINAL", "LogTracerCOM", "log");
                                        Respuesta = TipoTrx + ErrorCnxPinpad;
                                        Port.Close();
                                        return Respuesta;
                                    }
                                }
                            }
                        }
                    }


                    // Cerramos el puerto
                    Port.Close();
                    ST = Respuesta.IndexOf(STX);
                    ET = Respuesta.IndexOf(ETX);
                    AC = Respuesta.IndexOf(ACK);

                    if (ET > 0)
                    {
                        if (Respuesta.Length > ET)
                        {
                            Respuesta = Respuesta.Remove(ET, 1);
                            Respuesta = Respuesta.Remove(ST, 1);
                            if(AC >= 0)
                            {
                                Respuesta = Respuesta.Remove(AC, 1);
                            }
                            
                            Respuesta = Respuesta.Remove(Respuesta.Length - 1, 1);
                        }

                    }

                    if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "SendCantPaq_99 Se envia respuesta a la caja respuesta=" + Respuesta, "LogTracerCOM", "log");
                    return Respuesta;
                }               
            }
               
            catch (Exception ex)
            {
                if (iGrabaLog == 1) UTILIDADES.mensaje("ERROR :SendCantPaq_99 Exception: " + ex.Message.ToString(), "LogTracerCOM", "log");
                Port.Close();
                Respuesta = TipoTrx + ErrorCnxPinpad;
                return Respuesta;
            }
            return TipoTrx + " ERR.CONEXION PINPAD"; 
        }
            #endregion




        public static bool validaArchivoBines(string Ruta)
        {
            string BinInicio = string.Empty;
            string BinFin = string.Empty;
            string Adquiriente = string.Empty;
            string Cadena = string.Empty;
            Int64 result;

            bool flag = false;

            DataTable dt;
            dt = GetTable();

            try
            {
                StreamReader objReader = new StreamReader(Ruta);
                string sLine = "";
                ArrayList arrText = new ArrayList();

                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                    {
                        if (flag == true)
                        {
                            BinInicio = sLine.Substring(0, 6);
                            BinFin = sLine.Substring(6, 6);
                            Adquiriente = sLine.Substring(12, 4);
                            result = int.Parse(BinFin) - int.Parse(BinInicio);
                            if (result < 0)
                            {                              
                                return false;
                            }
                            dt.Rows.Add(BinInicio, BinFin, Adquiriente, result, sLine);
                        }
                        else
                        {
                            dt.Rows.Add(null, null, null, 0, sLine);
                            flag = true;
                        }
                    }
                }

                objReader.Close();

                if (dt.Rows.Count > 1)
                {
                    return true;
                }

                return false;
                
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }

}





