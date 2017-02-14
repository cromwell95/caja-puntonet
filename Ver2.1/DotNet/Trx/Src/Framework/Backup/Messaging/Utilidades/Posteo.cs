using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Trx.Messaging.Iso8583;

namespace Trx.Messaging.Utilidades
{
    class Posteo
    {
        //private String _sRuta;
        private String _sIp;
        private String _sLlaveIzq;
        private String _sLlaveDer;
        private int _iPuerto;
        private int _iTimeOut;
        private int _iGrabaLog;
        private int _iGrabaMsg;
        Iso8583MessageFormatter _formato;

        private void ejecucionPosteo() 
        {
            try
            {
                int iLargoArchivo, iLargoBody;
                byte[] bArchivo, bBufferEnvio, bBufferRecive, bBody;
                ParserContext parserContext;
                if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUGP: " + "Valida Ruta de  Posteo", "LogEvent", "log"); 
                String _sRuta = UTILIDADES.AppPath(true) + "Saf\\";
                String []files=Directory.GetFiles(_sRuta, "*.saf");
                if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUGP: " + "Optiene Archivos de Posteo", "LogEvent", "log"); 
                Iso8583Message isoMsgReq, isoMsgResp;
                Socket socket;
                Hashtable ISO;
                String resultado;
                foreach (String sFile in files) 
                {
                    try
                    {
                        resultado="";
                        isoMsgReq=new Iso8583Message();
                        isoMsgReq.Formatter = _formato;
                        bArchivo =UTILIDADES.Hex2Byte(UTILIDADES.F3DESDencriptarBloque(UTILIDADES.leerArchivo("", sFile), _sLlaveIzq, _sLlaveDer));
                        if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUGP: " + "Leer y decifra archivo:" + sFile, "LogEvent", "log"); 
                        iLargoArchivo = bArchivo.Length;
                        parserContext = new ParserContext(iLargoArchivo);
                        parserContext.Initialize();
                        parserContext.ResizeBuffer(iLargoArchivo);
                        parserContext.Write(bArchivo);
                        if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUGP: " + "Convierte ISO" , "LogEvent", "log"); 
                        isoMsgReq = (Iso8583Message)isoMsgReq.Formatter.Parse(ref parserContext);
                        isoMsgReq.MessageTypeIdentifier=400;
                        bBufferEnvio = TransformaISOByte(isoMsgReq, "6000081000");
                        if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUGP: " + "Inicia Socket" , "LogEvent", "log"); 
                        socket = InicializaSocket();
                        if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUGP: " + "Inicia Socket Terminado" , "LogEvent", "log"); 
                        socket.Send(bBufferEnvio);
                        if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUGP: " + "Envio de Buffer" , "LogEvent", "log"); 
                        if (this._iGrabaLog == 1) UTILIDADES.mensaje("OUTPUT DLL  P: " + isoMsgReq.ToString(), "LogTracer", "log");
                        socket.ReceiveTimeout = 10000;
                        if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUGP: " + "Set TimeOut: 10000" , "LogEvent", "log"); 
                        bBufferRecive = new byte[socket.ReceiveBufferSize];
                        socket.Receive(bBufferRecive);
                        if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUGP: " + "Recibe Buffer", "LogEvent", "log"); 
                        ISO = TransaformaIso(bBufferRecive);
                        bBody = (byte[])ISO["Body"];
                        iLargoBody = bBody.Length;
                        parserContext = new ParserContext(iLargoBody);
                        parserContext.Initialize();
                        parserContext.ResizeBuffer(iLargoBody);
                        parserContext.Write(bBody);
                        if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUGP: " + "Convierte Buffer en ISO" , "LogEvent", "log"); 
                        isoMsgResp = (Iso8583Message)isoMsgReq.Formatter.Parse(ref parserContext);
                        if (this._iGrabaLog == 1) UTILIDADES.mensaje("INPUT DLL   P: " + isoMsgResp.ToString(), "LogTracer", "log");
                        resultado = ((Field)isoMsgResp.Fields[39]).Value.ToString();
                        if((resultado.CompareTo("91")!=0) && ((resultado.CompareTo("27")!=0)))
                        {
                            if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUGP: " + "Valida Respuesta " , "LogEvent", "log"); 
                            File.Delete(sFile);
                            if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUGP: " + "Elimina: " + sFile, "LogEvent", "log"); 
                        }
                        try
                        {
                            socket.Close();
                            if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUGP: " + "Cierra Socket ", "LogEvent", "log"); 
                        }
                        catch (Exception e)
                        {
                            if(this._iGrabaMsg==1)UTILIDADES.mensaje("POSTEO: " + e.Message, "LogEvent", "log");
                        }
                    }
                    catch (Exception e)
                    {
                        if(this._iGrabaMsg==1)UTILIDADES.mensaje("POSTEO: " + e.Message, "LogEvent", "log");
                    }
                }
            }
            catch (Exception e)
            {
                if (this._iGrabaMsg == 1) UTILIDADES.mensaje("POSTEO: " + e.Message, "LogEvent", "log");
            }

        }
        private Hashtable TransaformaIso(byte[] formatterContext)
        {
            Hashtable hashMensaje = new Hashtable();
            String sHexOutPut = "";
            int iTalla;
            byte[] bHeader = new byte[5];
            byte[] bBody;
            //byte b;
            for (int i = 0; i < 2; i++)
            {
                sHexOutPut = sHexOutPut + String.Format("{0:X}", Convert.ToInt32(formatterContext[i])).PadLeft(2, '0');
            }
            for (int i = 2; i < 7; i++)
            {
                bHeader[i - 2] = formatterContext[i];
            }
            iTalla = Convert.ToInt32(sHexOutPut, 16);
            bBody = new byte[iTalla - 5];
            for (int i = 7; i < iTalla + 2; i++)
            {
                bBody[i - 7] = formatterContext[i];
            }
            hashMensaje.Add("Talla", iTalla);
            hashMensaje.Add("Header", bHeader);
            hashMensaje.Add("Body", bBody);
            return hashMensaje;
        }
        private Socket InicializaSocket()
        {
            Socket _socket;
            AddressFamily _family;
            _family = AddressFamily.InterNetwork;
            _socket = new Socket(_family, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(this._sIp, this._iPuerto);
            return _socket;
        }
        private byte[] TransformaISOByte(Iso8583Message isoMSG, String sTPDU)
        {
            byte[] bIsoByte = null;
            byte[] bBuffer = isoMSG.GetBytes();
            byte[] bHeader = UTILIDADES.Hex2Byte(sTPDU);
            int iTalla = bBuffer.Length + bHeader.Length;
            byte[] bTalla = UTILIDADES.Hex2Byte(Convert.ToString(iTalla, 16).PadLeft(4, '0'));
            bIsoByte = new byte[iTalla + 2];
            Buffer.BlockCopy(bTalla, 0, bIsoByte, 0, bTalla.Length);
            Buffer.BlockCopy(bHeader, 0, bIsoByte, bTalla.Length, bHeader.Length);
            Buffer.BlockCopy(bBuffer, 0, bIsoByte, bTalla.Length + bHeader.Length, bBuffer.Length);
            return bIsoByte;

        }
        public Posteo(String sIp, int iPuerto, int iTimeOut, int iGrabaLog, int iGrabaMsg, String sLlaveIzq, String sLlaveDer, Iso8583MessageFormatter formato)
        {
            this._sIp = sIp;
            this._sLlaveIzq = sLlaveIzq;
            this._sLlaveDer = sLlaveDer;
            this._iPuerto = iPuerto;
            this._iTimeOut = iTimeOut;
            this._iGrabaLog = iGrabaLog;
            this._iGrabaMsg = iGrabaMsg;
            this._formato = formato;
            ThreadStart theadStartPosteo = new ThreadStart(ejecucionPosteo);
            Thread theadPosteo = new Thread(theadStartPosteo);
            //theadPosteo.Priority = ThreadPriority.Highest;
            theadPosteo.Start();
        }

    }
}
