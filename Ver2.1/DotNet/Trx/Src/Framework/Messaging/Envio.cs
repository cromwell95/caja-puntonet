using System;
using System.Collections.Generic;
using System.Text;
using Trx.Messaging.Utilidades;

namespace Trx.Messaging
{
    public class Envio
    {
        public static readonly string ErrorCnxPinpad = "ER03ERR. CONEXION PINPAD";
        string TipoTrx;


        public  String Envio_requerimiento(String IP,int Puerto,int timeout, string trama,int iGrabaLog,int iGrabaMsg) 
        {
            try
            {
                if (String.IsNullOrEmpty(IP))
                {
                    return  Pinpad.SendPinpad(trama, Puerto, timeout,  iGrabaLog, iGrabaMsg);
                }
                else
                {
                    Pos p = new Pos(IP, Puerto, timeout, trama, iGrabaLog, iGrabaMsg);
                    return p.EnvioRequerimiento();
                }
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
                
        public String Envio_requerimientoPinpad(String IP, int Puerto, int timeout, String trama, String RutaBines,int iGrabaLog)
        {
            string [] arrip;
            string ip=string.Empty;
            string puerto=string.Empty;

            try
            {
                TipoTrx = trama.Substring(0, 2);
            }
            catch (Exception ex)
            {
                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Envio_requerimiento Error al obtener el tipo de transaccion " + ex.Message, "LogTracerCOM", "log");
                return TipoTrx + ErrorCnxPinpad;
            }


            try
            {
                if (TipoTrx.Equals("RA") || TipoTrx.Equals("BB") || TipoTrx.Equals("PP") || TipoTrx.Equals("LT"))
                {

                    if (String.IsNullOrEmpty(IP))
                    {
                        return Pinpad.SendPinpad(trama, Puerto, timeout, RutaBines, iGrabaLog);
                    }
                    else
                    {
                        if (IP.Length > 0)
                        {
                            arrip = IP.Split(':');
                            ip = arrip[0].PadRight(15, ' ');
                            puerto = arrip[1].PadRight(5, ' ');
                        }

                        if (ip.Length == 0)
                        {
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Envio_requerimiento IP no enviada ip= " + ip, "LogTramas", "log");
                            return TipoTrx + ErrorCnxPinpad;                              
                        }

                        if (puerto.Length == 0)
                        {
                            if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Envio_requerimiento PUERTO no enviado Puerto= " + puerto, "LogTramas", "log");
                            return TipoTrx + ErrorCnxPinpad;
                        }

                        trama = trama + ip + puerto;
                        return Pinpad.SendPinpad(trama, Puerto, timeout, RutaBines, iGrabaLog);

                    }
                }
                else
                {
                    return Pinpad.SendPinpad(trama, Puerto, timeout, RutaBines, iGrabaLog);
                }
               
            }
            catch (Exception e)
            {
                if (iGrabaLog == 1) UTILIDADES.mensaje("DEBUG : " + "Envio_requerimiento Excepcion= " + e.Message, "LogTramas", "log");
                return TipoTrx + ErrorCnxPinpad;
                

            }
        }



    }

   
}
