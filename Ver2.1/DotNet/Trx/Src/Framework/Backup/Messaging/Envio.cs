using System;
using System.Collections.Generic;
using System.Text;
using Trx.Messaging.Utilidades;

namespace Trx.Messaging
{
    public class Envio
    {
        public static String Envio_requerimiento(String IP,int Puerto,int timeout, String trama,int iGrabaLog,int iGrabaMsg) 
        {
            try
            {
                Pos p = new Pos(IP, Puerto, timeout, trama, iGrabaLog, iGrabaMsg);
                return p.EnvioRequerimiento();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
    }
}
