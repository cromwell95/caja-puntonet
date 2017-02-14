using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.IO;
using Trx.Messaging.Iso8583;

namespace Trx.Messaging.Utilidades
{
    /**
     
     */

    public class Pos
    {
        private String _ip;
        private int _puerto;
        private int _timeoutPos;
       // private int _talla;
       // private int _Bitmap;
       // private String _MapaBits;
        private String _trama;
       // private String _respuesta;
      //  private String _error;
        private String _sTipoTransaccion="";
        private String _sSecuencial="";
        private String _sHora="";
        private String _sFecha="";
        private String _sTerminalId="";
       // private String _Merchand="";
        private String _sNumeroTarjeta;
        private String _sNombreTarjeta;
        private String _sMerchandId = "";
        private String _sMontoFijo;
        private Iso8583Message _ISOMsg;
        private Iso8583MessageFormatter _formato;
        private int _iGrabaLog;
        private int _iGrabaMsg;
        private  String _sLlaveKEKIzquierda="1A237F5E6B98D0C4";
        private  String _sLlaveKEKDerecha="5B6E3F1A9C8D2478";
        private static int _lengthTrama = 304;
        
        

        public Pos(String ip,int puerto,int timeout,String trama,int iGrabaLog,int iGrabaMsg) 
        {
            this._ip=ip;
            this._puerto=puerto;
            this._timeoutPos=timeout;
            //this._talla=1;
            //this._Bitmap=2;
            this._trama=trama;
            this._iGrabaLog=iGrabaLog;
            this._iGrabaMsg=iGrabaMsg;
            this._formato=new Iso8583Bin1987MessageFormatter();
            if (trama.Length == _lengthTrama)
                _ISOMsg = camposTramaIso(trama, _formato);
            else
                throw new Exception("Longitud de trama Plana de requerimiento no es valida");
        }
        private String camposTramaPlano(Iso8583Message isoMsg) 
        {
            String sCodigoRespuesta="",sMensajeRespuesta="",
                   sNumeroAutorizacion="",sMontoInteres="",sMensajesPremiosPublicidad="",sComisionCuotaFacil="",sGrupoTarjeta="",
                   sAdquirente="",sMidAdquirente="",sTidAdquirente="";
            CamposAdicionales camposAd;
            if(isoMsg.Fields.Contains(116))
            {
                sMensajeRespuesta = ((Field)isoMsg.Fields[116]).Value.ToString(); 
            }
            if (isoMsg.Fields.Contains(39))
            {
                sCodigoRespuesta = ((Field)isoMsg.Fields[39]).Value.ToString();
                if ((isoMsg.MessageTypeIdentifier==400) && (sCodigoRespuesta.CompareTo("91") == 0))
                {
                    throw new Exception("Respondio BCO FUERA DE LINEA en reversa");
                }
            }
            else
            {
                throw new Exception("No existe campo 39 en respuesta ISO");
            }
            if (isoMsg.Fields.Contains(38))
            {
                sNumeroAutorizacion = ((Field)isoMsg.Fields[38]).Value.ToString();
            }
            else
            {
                sNumeroAutorizacion="      ";
            }
            if(isoMsg.Fields.Contains(114))
            {
                try
                {
                    camposAd = new CamposAdicionales(1, ((Field)isoMsg.Fields[114]).Value.ToString());
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al convertir campos Adicionales : " + ex.Message);
                }
                sMontoInteres=camposAd.getIntereses();
                if(sMontoInteres==null)
                {
                    sMontoInteres="";
                }
                while(sMontoInteres.Length<12)
                {
                    sMontoInteres= sMontoInteres + " ";
                }
                while(sComisionCuotaFacil.Length<12)
                {
                    sComisionCuotaFacil= sComisionCuotaFacil + " ";
                }
                sComisionCuotaFacil=camposAd.getMontoFijo();
                if(sComisionCuotaFacil==null)
                {
                    sComisionCuotaFacil="";
                }
                while(sMontoInteres.Length<12)
                {
                    sMontoInteres= sMontoInteres + " ";
                }
                while(sComisionCuotaFacil.Length<12)
                {
                    sComisionCuotaFacil= sComisionCuotaFacil + " ";
                }
                
            }
            else
            {
                while(sMontoInteres.Length<12)
                {
                    sMontoInteres= sMontoInteres + " ";
                }
                while(sComisionCuotaFacil.Length<12)
                {
                    sComisionCuotaFacil= sComisionCuotaFacil + " " ;
                }
            }
            while (_sMontoFijo.Length < 12)
            {
                _sMontoFijo = _sMontoFijo + " ";
            }
            if(isoMsg.Fields.Contains(115))
            {
                sMensajesPremiosPublicidad = ((Field)isoMsg.Fields[115]).Value.ToString();
            }
            if(sMensajesPremiosPublicidad.Length<80)
            {
                while(sMensajesPremiosPublicidad.Length<80)
                {
                    sMensajesPremiosPublicidad= sMensajesPremiosPublicidad + (" ");
                }
            }
            if (isoMsg.Fields.Contains(105))
            {
                sGrupoTarjeta = ((Field)isoMsg.Fields[105]).Value==null ? "":((Field)isoMsg.Fields[105]).Value.ToString();
            }
            if (sGrupoTarjeta.Length < 60)
            {
                while (sGrupoTarjeta.Length < 60)
                {
                    sGrupoTarjeta = sGrupoTarjeta + (" ");
                }
            }
            if (sGrupoTarjeta.Length > 60) 
            {
                sGrupoTarjeta = sGrupoTarjeta.Substring(0, 60);
            }
            if (isoMsg.Fields.Contains(106))
            {
                sAdquirente = ((Field)isoMsg.Fields[106]).Value == null ? "" : ((Field)isoMsg.Fields[106]).Value.ToString();
            }
            if (sAdquirente.Length < 60)
            {
                while (sAdquirente.Length < 60)
                {
                    sAdquirente = sAdquirente + (" ");
                }
            }
            if (sAdquirente.Length > 60)
            {
                sAdquirente = sAdquirente.Substring(0, 60);
            }
            if (isoMsg.Fields.Contains(107))
            {
                sMidAdquirente = ((Field)isoMsg.Fields[107]).Value == null ? "" : ((Field)isoMsg.Fields[107]).Value.ToString();
            }
            if (sMidAdquirente.Length < 15)
            {
                while (sMidAdquirente.Length < 15)
                {
                    sMidAdquirente = sMidAdquirente + (" ");
                }
            }
            if (sMidAdquirente.Length > 15)
            {
                sMidAdquirente = sMidAdquirente.Substring(0, 15);
            }
            if (isoMsg.Fields.Contains(108))
            {
                sTidAdquirente = ((Field)isoMsg.Fields[108]).Value == null ? "" : ((Field)isoMsg.Fields[108]).Value.ToString();
            }
            if (sTidAdquirente.Length < 8)
            {
                while (sTidAdquirente.Length < 8)
                {
                    sTidAdquirente = sTidAdquirente + (" ");
                }
            }
            if (sTidAdquirente.Length > 8)
            {
                sTidAdquirente = sTidAdquirente.Substring(0, 8);
            }
           
            if(sMensajeRespuesta.Length>20)
            {
                sMensajeRespuesta=sMensajeRespuesta.Substring(0, 20);
            }
            else if((sMensajeRespuesta.Length<20))
            {
                while(sMensajeRespuesta.Length<20)
                {
                    sMensajeRespuesta= sMensajeRespuesta + " ";
                }
            }
            if (_sNumeroTarjeta.Length > 20)
            {
                _sNumeroTarjeta = _sNumeroTarjeta.Substring(0, 20);
            }
            else if ((_sNumeroTarjeta.Length < 20))
            {
                while (_sNumeroTarjeta.Length < 20)
                {
                    _sNumeroTarjeta = _sNumeroTarjeta + " ";
                }
            }
            if (_sNombreTarjeta.Length > 40)
            {
                _sNombreTarjeta = _sNombreTarjeta.Substring(0, 40);
            }
            else if ((_sNombreTarjeta.Length < 40))
            {
                while (_sNombreTarjeta.Length < 40)
                {
                    _sNombreTarjeta = _sNombreTarjeta + " ";
                }
            }
            if(isoMsg.Fields.Contains(41))
            {
                if (((Field)isoMsg.Fields[41]).Value.ToString().CompareTo(_sTerminalId) != 0)
                {
                    throw new Exception("Terminal ID no concuerda con el terminal enviado");
                }
            }
            else
            {
                    throw new Exception("Terminal ID no se encuentra en la respuesta");
            }
            return _sTipoTransaccion + sCodigoRespuesta + sMensajeRespuesta + _sSecuencial + _sHora + _sFecha +
                 sNumeroAutorizacion + _sTerminalId + sMontoInteres + sComisionCuotaFacil + sMensajesPremiosPublicidad + 
                 sGrupoTarjeta + sAdquirente + sMidAdquirente + sTidAdquirente + _sNumeroTarjeta + _sMontoFijo + _sNombreTarjeta;
        }
        private Iso8583Message camposTramaIso(String trama, Iso8583MessageFormatter formato) 
        {
            Iso8583Message _ISOMsg;
            String sInfoTrack2;
            String sMontoServicio;
            String sMontoPropina;
            String sMontoCashOver;
            String sInfoTrack1;
            String sMesesGracia;
            String sMontoBaseGravaIVA="";
            String sMontoBaseNoGravaIVA="";
            String sTipoTarjeta;
            String sTipoPlazoCredito;
            String sModoEntrada;
            String sCVV2;
            String sMonto;
            String sMontoIva;
            String sNumeroAutorizacion;
            String sNumeroLote;
            String sMTI = "";
            String sTipoTransaccion = "";
            CamposAdicionales camposAdicionales = new CamposAdicionales();
            _sTipoTransaccion = trama.Substring(0, 2);
            sTipoTarjeta = trama.Substring(2, 2);
            sTipoPlazoCredito = trama.Substring(4, 4);
            sMesesGracia = trama.Substring(8, 2);
            sInfoTrack2 = trama.Substring(10, 37).Trim();
            sModoEntrada = trama.Substring(47, 3);
            sCVV2 = trama.Substring(50, 4).Trim();
            sMonto = trama.Substring(54, 12);
            sMontoBaseGravaIVA = trama.Substring(66, 12).Trim();
            sMontoBaseNoGravaIVA = trama.Substring(78, 12).Trim();
            sMontoIva = trama.Substring(90, 12).Trim();
            sMontoServicio = trama.Substring(102, 12).Trim();
            sMontoPropina = trama.Substring(114, 12).Trim();
            sMontoCashOver = trama.Substring(126, 12).Trim();
            _sSecuencial = trama.Substring(138, 6);
            _sHora = trama.Substring(144, 6);
            _sFecha = trama.Substring(150, 8);
            sNumeroAutorizacion = trama.Substring(158, 6).Trim();
            _sTerminalId = trama.Substring(164, 8);
            _sMerchandId = trama.Substring(172, 15);
            sNumeroLote = trama.Substring(187, 6);
            sInfoTrack1 = trama.Substring(193, 99);
            _sMontoFijo = trama.Substring(292, 12).Trim();
            _sNombreTarjeta = "";
            sInfoTrack1 = sInfoTrack1.Trim();
            if (sMontoIva != null) 
            {
                if (sMontoIva.Length > 0) 
                {
                    camposAdicionales.setIVA(sMontoIva);    
                }
            }
            if (sMontoServicio != null)
            {
                if (sMontoServicio.Length > 0)
                {
                    camposAdicionales.setServicios(sMontoServicio);
                }
            }
            if (sMontoPropina != null)
            {
                if (sMontoPropina.Length > 0)
                {
                    camposAdicionales.setPropina(sMontoPropina);
                }
            }
            if (_sMontoFijo != null)
            {
                if (_sMontoFijo.Length > 0)
                {
                    camposAdicionales.setMontoFijo(_sMontoFijo);
                }
            }
            if (sMontoCashOver != null)
            {
                if (sMontoCashOver.Length > 0)
                {
                    camposAdicionales.setCashOver(sMontoCashOver);
                }
            }
            if ((_sTipoTransaccion.CompareTo("01") == 0) || (_sTipoTransaccion.CompareTo("02") == 0)) {
                sMTI = "0200";
                sTipoTransaccion = "00";
            } 
            else if (_sTipoTransaccion.CompareTo("03") == 0)
            {
                sMTI = "0200";
                sTipoTransaccion = "20";
            } 
            else if (_sTipoTransaccion.CompareTo("04") == 0)
            {
                sMTI = "0400";
                sTipoTransaccion = "00";
            }
            _ISOMsg = new Iso8583Message();
            _ISOMsg.Formatter = formato;
            _ISOMsg.MessageTypeIdentifier = Convert.ToInt32(sMTI);
            _ISOMsg.Fields.Add(3, sTipoTransaccion + sTipoTarjeta + "00");
            _ISOMsg.Fields.Add(113, sTipoPlazoCredito+ sMesesGracia);
            sInfoTrack2 = sInfoTrack2.Trim();
            int posicion = sInfoTrack2.IndexOf("=");
            _sNumeroTarjeta = sInfoTrack2.Substring(0, posicion);
            _sNumeroTarjeta = protegeTarjeta(_sNumeroTarjeta);
            if (sModoEntrada.CompareTo("012") == 0)
            {
                _ISOMsg.Fields.Add(2, sInfoTrack2.Substring(0, posicion));
                _ISOMsg.Fields.Add(14, sInfoTrack2.Substring(posicion + 1));
            }
            else if (sModoEntrada.CompareTo("022") == 0)
            {
                _ISOMsg.Fields.Add(35, sInfoTrack2);
            }
            _ISOMsg.Fields.Add(22, sModoEntrada);
            if (sCVV2.Length > 0)
            {
                _ISOMsg.Fields.Add(48, sCVV2);
            }
            _ISOMsg.Fields.Add(4, sMonto);
            _ISOMsg.Fields.Add(114, camposAdicionales.toISOHiCC());
            _ISOMsg.Fields.Add(11, _sSecuencial);
            _ISOMsg.Fields.Add(12, _sHora);
            _ISOMsg.Fields.Add(13, _sFecha.Substring(4));
            sMontoBaseGravaIVA=sMontoBaseGravaIVA.Trim();
            sMontoBaseNoGravaIVA = sMontoBaseNoGravaIVA.Trim();
            _ISOMsg.Fields.Add(119,UTILIDADES.zeropad(sMontoBaseGravaIVA, 12) + UTILIDADES.zeropad(sMontoBaseNoGravaIVA, 12));
            sNumeroAutorizacion = sNumeroAutorizacion.Trim();
            if (sNumeroAutorizacion.Length == 6)
            {
                _ISOMsg.Fields.Add(38, sNumeroAutorizacion);
            }
            _ISOMsg.Fields.Add(41, _sTerminalId);
            _ISOMsg.Fields.Add(42, _sMerchandId);
            if (sInfoTrack1.Length > 0)
            {
                _sNombreTarjeta = extraeTarjetaHabiente(sInfoTrack1);
                _ISOMsg.Fields.Add(45, sInfoTrack1);
            }
            _ISOMsg.Fields.Add(112, sNumeroLote);
            if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Datos:" + _sSecuencial + "|" + _sTerminalId + "|" + _sMerchandId, "LogEvent", "log");
            return _ISOMsg;
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
        private byte[] TransformaISOByte(Iso8583Message isoMSG, String sTPDU)
        {
            byte[] bIsoByte = null;
            byte[] bBuffer = isoMSG.GetBytes();
            byte[] bHeader = UTILIDADES.Hex2Byte(sTPDU);
            int iTalla = bBuffer.Length + bHeader.Length;
            byte[] bTalla = UTILIDADES.Hex2Byte(Convert.ToString(iTalla, 16).PadLeft(4, '0'));
            bIsoByte = new byte[iTalla + 2];
            Buffer.BlockCopy(bTalla , 0, bIsoByte, 0, bTalla.Length);
            Buffer.BlockCopy(bHeader, 0, bIsoByte, bTalla.Length, bHeader.Length);
            Buffer.BlockCopy(bBuffer, 0, bIsoByte, bTalla.Length + bHeader.Length, bBuffer.Length);
            return bIsoByte;

        }
        private Socket InicializaSocket()
        {
            Socket _socket;
            AddressFamily _family;
            _family = AddressFamily.InterNetwork;
            _socket = new Socket(_family, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(this._ip, this._puerto);
            return _socket;
        }
        public String EnvioRequerimiento()
        {
            String _respuesta="";
            String sError="";
            String sPosteo;
            String sFileName;
            String sFecha;
            ParserContext parserContext;
            Hashtable ISO;
            Iso8583Message isoMsgResp;
            Socket socket;

            byte[] bBody;
            byte[] bBufferRecive;
            byte[] bBufferEnvio;
            int iLargoBody;
            if(this._trama.Length!=_lengthTrama)
            {
                if (this._iGrabaMsg == 1) UTILIDADES.mensaje("POS   : " + "Longitud de trama Plana de requerimiento no es valida", "LogEvent", "log");
                return "";
            }
            if (this._iGrabaLog == 1) UTILIDADES.mensaje("INPUT METODO : " + UTILIDADES.ProtegeTramaPlana(_trama), "LogTracer", "log");
            try
            {
                if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Previo Socket", "LogEvent", "log");
                socket = InicializaSocket();
                if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Despues Socket", "LogEvent", "log");
            }
            catch (System.Net.Sockets.SocketException e)
            {
                sError += e.Message;
                if (this._iGrabaMsg == 1) UTILIDADES.mensaje("POS   : " + e.Message, "LogEvent", "log");
                throw new Exception(sError);
            }
            try 
            {
                if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Antes de ISO to BYTE", "LogEvent", "log"); 
                bBufferEnvio = TransformaISOByte(_ISOMsg, "6000081000");
                if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Despues de ISO to BYTE", "LogEvent", "log");
            }
            catch (Exception e)
            {
                sError+=e.Message;
                if (this._iGrabaMsg == 1) UTILIDADES.mensaje("POS   : " + e.Message, "LogEvent", "log");
                throw new Exception(sError);
            }
            try
            {
                 if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Antes de Envio", "LogEvent", "log"); 
                 socket.Send(bBufferEnvio);
                 if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Despues de Envio", "LogEvent", "log");
                 if (this._iGrabaLog == 1) UTILIDADES.mensaje("OUTPUT DLL   : " + _ISOMsg.ToString(), "LogTracer", "log");
                 if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "TimeOut Definido", "LogEvent", "log"); 
                 socket.ReceiveTimeout = Convert.ToInt32(_timeoutPos);
                 if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "TimeOut Definido", "LogEvent", "log"); 
                 bBufferRecive = new byte[socket.ReceiveBufferSize];
                 socket.Receive(bBufferRecive);
                 if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Buffer recibido", "LogEvent", "log"); 
                 ISO = TransaformaIso(bBufferRecive);
                 if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Transforma buffer ISO", "LogEvent", "log"); 
                 bBody = (byte[])ISO["Body"];
                 iLargoBody = bBody.Length;
                 parserContext = new ParserContext(iLargoBody);
                 parserContext.Initialize();
                 parserContext.ResizeBuffer(iLargoBody);
                 parserContext.Write(bBody);
                 isoMsgResp = (Iso8583Message)_ISOMsg.Formatter.Parse(ref parserContext);
                 if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Finaliza buffer ISO", "LogEvent", "log"); 
                 if (this._iGrabaLog == 1) UTILIDADES.mensaje("INPUT DLL    : " + isoMsgResp.ToString(), "LogTracer", "log");
            }
            catch (Exception e)
            {
                sError += e.Message;
                if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Excepcion :" + sError, "LogEvent", "log"); 
                if (((Field)_ISOMsg.Fields[3]).Value.ToString().CompareTo("003000") == 0)
                {
                    
                    try
                    {
                        if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Inicia Posteo"  , "LogEvent", "log"); 
                        sPosteo = UTILIDADES.F3DESEncriptarBloque(UTILIDADES.padMultiplo(UTILIDADES.Byte2Hex(_ISOMsg.GetBytes()), 16, "1C"), _sLlaveKEKIzquierda, _sLlaveKEKDerecha);
                        if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Finaliza cifrado", "LogEvent", "log"); 
                        sFecha = DateTime.Now.ToString("HHmmss");
                        String sRuta = UTILIDADES.AppPath(true) + "Saf\\";
                        if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Valida Ruta de Posteo", "LogEvent", "log"); 
                        Directory.CreateDirectory(sRuta);
                        sFileName = Convert.ToString(_ISOMsg.MessageTypeIdentifier) + ((Field)_ISOMsg.Fields[11]).Value.ToString() + ((Field)_ISOMsg.Fields[41]).Value.ToString() + ((Field)_ISOMsg.Fields[112]).Value.ToString() + sFecha + ".saf";
                        //new GrabaArchivo(sFileName, sPosteo, false);
                        if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Guarda Archivo de Posteo", "LogEvent", "log"); 
                        UTILIDADES.almacenarArchivo(sRuta, sFileName, false, sPosteo);
                        if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Ejecuta Posteo", "LogEvent", "log"); 
                        new Posteo(_ip, _puerto, _timeoutPos, _iGrabaLog, _iGrabaMsg, _sLlaveKEKIzquierda, _sLlaveKEKDerecha, _formato);
                        if (this._iGrabaMsg == 1) UTILIDADES.mensaje("DEBUG : " + "Finaliza Posteo", "LogEvent", "log"); 
                        if (socket != null) socket.Close();
                    }
                    catch (Exception ex)
                    {
                        sError += "\t" + ex.Message;
                        if (this._iGrabaMsg == 1) UTILIDADES.mensaje("POS   : " + ex.Message, "LogEvent", "log");
                    }
                }
                if (this._iGrabaMsg == 1) UTILIDADES.mensaje("POS   : " + e.Message, "LogEvent", "log");
                throw new Exception(sError);
            }
            try
            {
                socket.Close();
            }
            catch (Exception e)
            {
                sError += e.Message;
                if (this._iGrabaMsg == 1) UTILIDADES.mensaje("POS   : " + e.Message, "LogEvent", "log");
            }
            try
            {
                new Posteo(_ip, _puerto, _timeoutPos, _iGrabaLog, _iGrabaMsg, _sLlaveKEKIzquierda, _sLlaveKEKDerecha, _formato);
            }
            catch (Exception e) 
            {
                sError += e.Message;
                if (this._iGrabaMsg == 1) UTILIDADES.mensaje("POS   : " + e.Message, "LogEvent", "log");
            }
            try
            {
                _respuesta = camposTramaPlano(isoMsgResp);
            }
            catch (Exception e)
            {
                sError += e.Message;
                if (this._iGrabaMsg == 1) UTILIDADES.mensaje("POS   : " + e.Message, "LogEvent", "log");
            }
            if (this._iGrabaLog == 1) UTILIDADES.mensaje("OUTPUT METODO: " + _respuesta, "LogTracer", "log");
            if (_respuesta.Length > 0)
            {
                return _respuesta;
            }
            return "";
        }
        public static String protegeTarjeta(String sNumeroTarjeta)
        {
            String sNumeroTarjetaEnmascarada;
            int iTamanoRelleno = sNumeroTarjeta.Length;
            if (iTamanoRelleno > 12)
            {
                sNumeroTarjetaEnmascarada = sNumeroTarjeta.Substring(0, 6) + UTILIDADES.padleft("", iTamanoRelleno - 9, 'X') + sNumeroTarjeta.Substring(iTamanoRelleno - 3);
            }
            else
            {
                sNumeroTarjetaEnmascarada = UTILIDADES.padleft("", iTamanoRelleno, 'X');
            }
            return sNumeroTarjetaEnmascarada;
        }

        public static String[] extraeDatos(String sCadena, char sSeparador)
        {
            String[] sListas = null;
            int iPosicion, iPosicionUltima, iIndice = 0, iPosicionAnterior = 0;
            Hashtable hashtable = new Hashtable();
            iPosicion = sCadena.IndexOf(sSeparador);
            iPosicionUltima = sCadena.LastIndexOf(sSeparador);
            if (iPosicion > -1)
            {
                if (iPosicion < iPosicionUltima)
                {
                    iPosicion = 0;
                    while (iPosicion <= iPosicionUltima)
                    {
                        iPosicion = sCadena.IndexOf(sSeparador, iPosicion + 1);
                        if (iPosicion == -1)
                        {
                            iPosicion = sCadena.Length;
                        }
                        hashtable.Add(Convert.ToString(iIndice), sCadena.Substring(iPosicionAnterior, iPosicion - iPosicionAnterior));
                        iPosicionAnterior = iPosicion + 1;
                        iIndice++;
                    }
                    sListas = new String[iIndice];
                    for (int i = 0; i < iIndice; i++)
                    {
                        sListas[i] = (String)hashtable[Convert.ToString(i)];
                    }
                }
            }
            return sListas;
        }

        public static String extraeTarjetaHabiente(String sInfoTrack1)
        {
            String sTarjetaHabiente = "";
            String[] sListas = null;
            sListas = extraeDatos(sInfoTrack1, UTILIDADES.SEPARADOR01);
            if (sListas == null)
            {
                sListas = extraeDatos(sInfoTrack1, UTILIDADES.SEPARADOR02);
                if (sListas == null)
                {
                    sListas = extraeDatos(sInfoTrack1, UTILIDADES.SEPARADOR03);
                    if (sListas == null)
                    {
                        sListas = extraeDatos(sInfoTrack1, UTILIDADES.SEPARADOR04);
                        if (sListas == null)
                        {
                            sListas = extraeDatos(sInfoTrack1, UTILIDADES.SEPARADOR05);
                        }
                    }
                }
            }
            if (sListas != null)
            {
                if (sListas.Length > 1)
                {
                    if ((sListas.Length % 2) != 0)
                    {
                        sTarjetaHabiente = sListas[(sListas.Length / 2)];
                    }
                }
            }
            return sTarjetaHabiente;
        }

    }
}
