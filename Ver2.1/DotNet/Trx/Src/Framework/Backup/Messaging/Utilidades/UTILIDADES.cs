using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Security.Cryptography;
using System.Reflection;
using Trx.Messaging.Iso8583;



namespace Trx.Messaging.Utilidades
{
    public class UTILIDADES
    {
        public  static char SEPARADOR01 = '^';
        public  static char SEPARADOR02 = '<';
        public  static char SEPARADOR03 = '>';
        public  static char SEPARADOR04 = '=';
        public  static char SEPARADOR05 = '[';
        internal static string merror = string.Empty;
        public static void mensaje(String sMensaje, String sNombreArchivo, String sExtencion)
        {
            char cLF , cCR;
            cLF = Convert.ToChar(0X0A);
            cCR = Convert.ToChar(0X0D);

            String sFecha = DateTime.Now.ToString("yyyyMMddHH:mm:ss");
            String sRuta = AppPath(true) + "Log\\" ;
            Directory.CreateDirectory(sRuta);
            UTILIDADES.almacenarArchivo(sRuta, sNombreArchivo, true, sMensaje);   
            //new GrabaArchivo(sRuta + sNombreArchivo + sFecha.Substring(0, 8) + "." + sExtencion, sFecha.Substring(8) + "\t" + sMensaje, true);
        }


#region Utilidades de Cadenas
        public static byte[] Hex2Byte(String formatterContext)
        {
            byte[] bBuffer;
            int ilength = formatterContext.Length;
            int ilengthByte=ilength/2;

            bBuffer = new byte[ilengthByte];
            for (int i = 1; i <= ilengthByte; i++)
            {
                bBuffer[ilengthByte - i] = Convert.ToByte(Convert.ToInt32(formatterContext.Substring(ilength - (i * 2), 2), 16));
            }
            return bBuffer;
        }
        public static String Byte2Hex(byte[] formatterContext) 
        {
            int iTamana = formatterContext.Length;
            String sHexOutPut = "";
            foreach (byte b in formatterContext) 
            {
                sHexOutPut = sHexOutPut+String.Format("{0:X}", Convert.ToInt32(b)).PadLeft(2,'0');
            }
            return sHexOutPut;
        }
        
        private static string ConvertToHex(string asciiString)
        {
            string hex = "";
            foreach (char c in asciiString)
            {
                int tmp = c;
                hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }                 

        private static string HexAsciiConvert(string hex)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hex.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hex.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }
        public static String padMultiplo(String data,int mult,String c)
        {   String temp=data;
            while(temp.Length % mult!=0)
            {
                temp=temp+c;
            }
            return temp;
        }
        public static String unPad(String sCadenaPad, String Padeo) 
        {
            String sCadenaNoPad = sCadenaPad;
            int iLargoPadeo = Padeo.Length;
            int iLargoCadena = sCadenaNoPad.Length;
            while (iLargoCadena != 0) 
            {
                if (sCadenaNoPad.Substring(iLargoCadena - iLargoPadeo, iLargoPadeo).CompareTo(Padeo) == 0)
                {
                    sCadenaNoPad = sCadenaNoPad.Substring(0, iLargoCadena - iLargoPadeo);
                    iLargoCadena = sCadenaNoPad.Length;
                }
                else 
                {
                    break;
                }
            }
            return sCadenaNoPad;
        }
        public static String padleft(String s, int len, char c)
        {
            s = s.Trim();
            if (s.Length > len)
                throw new Exception("Tamaño invalido en padeo--> cadena contiene: " +s.Length + "caracteres y la solicitud del padeo es de:" +len+" caracteres");
            String d = "";
            int fill = len - s.Length;
            while (fill-- > 0)
                d = d + c;
            d = d + s;
            return d;
        }
        public static String zeropad(String s, int len)  {
            return padleft(s, len, '0');
        }
        public static byte[] StringToBytes(String cadena)
        {
            System.Text.ASCIIEncoding codificador = new System.Text.ASCIIEncoding();
            return codificador.GetBytes(cadena);
        }
        public static String getString(byte[] text)
        {
            System.Text.ASCIIEncoding codificador = new System.Text.ASCIIEncoding();
            return codificador.GetString(text);
        }

        public static String protect(String s)
        {
            StringBuilder sb = new StringBuilder();
            int len = s.Length;
            int clear = 0;
            int lastFourIndex = -1;
            int track1 = -1;
            if (len >= 7)
            {
                lastFourIndex = s.IndexOf('=') - 2;
                if (lastFourIndex < 0)
                {
                    lastFourIndex = s.IndexOf('^') - 2;
                    track1 = s.IndexOf('^', lastFourIndex + 5);
                    if (lastFourIndex < 0)
                    {
                        if (len > 13)
                        {
                            if (s.IndexOf('D')>-1)
                            {
                                clear = 0;
                            }
                            else
                            {
                                lastFourIndex = len - 3;
                                clear = 6;
                            }

                        }
                        else
                        {
                            clear = 0;
                        }

                    }
                    else
                    {
                        clear = 6;

                    }
                }
                else
                {
                    clear = 6;
                }
            }
            for (int i = 0; i < len; i++)
            {
                if (s[i] == '=')
                {
                    clear = 1;  // use clear=5 to keep the expiration date
                }
                else
                {
                    if (s[i] == '^')
                    {
                        lastFourIndex = 0;
                        //clear = len - i;
                        clear = 1;
                        if (track1 == i)
                        {
                            clear = 1;
                        }
                    }
                    else
                    {
                        if (i == lastFourIndex)
                        {
                            clear = len - lastFourIndex;
                        }
                    }
                }
                sb.Append(clear-- > 0 ? s[i] : 'X');
            }
            return sb.ToString();
        }
        public static Iso8583Message checkProtectedClone(Iso8583Message m)
        {
            Iso8583Message pm = (Iso8583Message) m.Clone();
            pm.Formatter = m.Formatter;
            int[] protectFields  = {2,14,35,45,52,48,126};
            for (int i=0; i<protectFields.Length; i++) {
                int f = protectFields[i];
                if (pm.Fields.Contains(f))
                    pm.Fields.Add(f, protect(((Field)m.Fields[f]).Value.ToString()));
            }
            return pm;
        }
        public static String ProtegeTramaPlana(String sOrigen)
        {
            String sParte01 = sOrigen.Substring(0, 8);
            String sNumeroTarjeta = sOrigen.Substring(8, 37);
            String sParte02 = sOrigen.Substring(45, 3);
            String sCVV2CVC2 = sOrigen.Substring(48, 3);
            String sParte03 = sOrigen.Substring(51, 79);
            String sTrack1 = sOrigen.Substring(130);
            return sParte01 + protect(sNumeroTarjeta) + sParte02 + protect(sCVV2CVC2) + sParte03 + protect(sTrack1);
        }
#endregion
#region Encripcion
        //Método para encriptar datos con el algoritmo 3Des (La data debe estar en Hexadeximal)
        public static string F3DESEncriptar(string Data, string Key_izquirda, string key_derecha)
        {
            String regreso = "";
            byte[] respuesta = Hex2Byte(Key_izquirda + key_derecha);
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.KeySize = 128;
            des.Key = respuesta;
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.None;
            ICryptoTransform ic = des.CreateEncryptor();
            byte[] datares = Hex2Byte(Data);
            byte[] enc = ic.TransformFinalBlock(datares, 0, 8);
            regreso = Byte2Hex(enc);
            regreso = regreso.Replace("\0", "").Trim();
            return regreso;
        }

        //Método para desencriptar datos con el algoritno 3Des (La data debe estar en Hexadecimal)
        public static string F3DESDencriptar(string Data, string Key_izquierda, string key_derecha)
        {
            String regreso = "";
            byte[] respuesta = Hex2Byte(Key_izquierda + key_derecha);
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.KeySize = 128;
            des.Key = respuesta;
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.None;
            ICryptoTransform ic1 = des.CreateDecryptor();
            byte[] datares = Hex2Byte(Data);
            byte[] enc = ic1.TransformFinalBlock(datares, 0, 8);
            regreso = Byte2Hex(enc);
            regreso = regreso.Replace("\0", "").Trim();
            return regreso;
        }
        public static string F3DESEncriptarBloque(String sBloqueNoEncriptado, String keyIzquierda,String keyDerecha)
        {
            String sBloqueEncriptado="";
            int iLargo=sBloqueNoEncriptado.Length;
            int interacciones=0;
            if ((iLargo % 16) != 0)
            {
                interacciones = (iLargo / 16) + 1;
                sBloqueNoEncriptado = padMultiplo(sBloqueNoEncriptado, 16, "1C");
            }
            else 
            {
                interacciones = (iLargo / 16);
            }
            for(int i=0;i<interacciones;i++)
            {
                sBloqueEncriptado=sBloqueEncriptado + F3DESEncriptar(sBloqueNoEncriptado.Substring(i*16,16),keyIzquierda,keyDerecha);
            };
            return sBloqueEncriptado;
        }
        public static string F3DESDencriptarBloque(String sBloqueEncriptado , String keyIzquierda, String keyDerecha)
        {
            String sBloqueNoEncriptado = "";
            int iLargo = sBloqueEncriptado.Length;
            int interacciones = (iLargo / 16);
            for (int i = 0; i < interacciones; i++)
            {
                sBloqueNoEncriptado = sBloqueNoEncriptado + F3DESDencriptar(sBloqueEncriptado.Substring(i * 16, 16), keyIzquierda, keyDerecha);
            }
            return unPad(sBloqueNoEncriptado, "1C");
        }
       
#endregion
#region Funciones para saber el path y nombre del ejecutable (y esta DLL)
        //
        public static String AppPath()
        {
            return Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
        }
        public static String AppPath(bool backSlash)
        {
            String s = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            // si hay que añadirle el backslash
            if (backSlash)
                s += "\\";
            return s;
        }
        //
        public static String AppExeName()
        {
            String s = Assembly.GetCallingAssembly().Location;
            FileInfo fi = new FileInfo(s);
            s = fi.Name;
            return s;
        }
        public static String AppExeName(bool fullPath)
        {
            String s = Assembly.GetCallingAssembly().Location;
            FileInfo fi = new FileInfo(s);
            if (fullPath)
                s = fi.FullName;
            else
                s = fi.Name;
            return s;
        }
        //
        public static String DLLPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        public static String DLLPath(bool backSlash)
        {
            String s = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // si hay que añadirle el backslash
            if (backSlash)
                s += "\\";
            return s;
        }
        //
        public static String DLLName()
        {
            String s = Assembly.GetExecutingAssembly().Location;
            FileInfo fi = new FileInfo(s);
            return fi.Name;
        }
        public static String DLLName(bool fullPath)
        {
            String s = Assembly.GetExecutingAssembly().Location;
            FileInfo fi = new FileInfo(s);
            if (fullPath)
                s = fi.FullName;
            else
                s = fi.Name;
            return s;
        }
#endregion
#region Metodos para Almacenar en disco
        public static void almacenarArchivo(String sRuta,String sNombre,Boolean boTipo, String sMensaje)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(sRuta + sNombre, boTipo);
            sw.WriteLine(sMensaje);
            sw.Close();
        }
        public static String leerArchivo(String sRuta, String sNombre) 
        {
            String sMensaje;
            System.IO.StreamReader sr = new System.IO.StreamReader(sRuta + sNombre);
            sMensaje = sr.ReadToEnd();
            sr.Close();
            return sMensaje;
        }
#endregion



    }
}
