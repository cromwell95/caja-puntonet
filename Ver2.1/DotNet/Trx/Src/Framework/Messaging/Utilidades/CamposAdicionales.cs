using System;
using System.Collections.Generic;
using System.Text;
using Trx.Messaging.Utilidades;

namespace Trx.Messaging.Utilidades
{
    public class CamposAdicionales
    {
        #region Atributos
        private String _IVA;
        private String _CashOver;
        private String _Servicios;
        private String _Propina;
        private String _Intereses;
        private String _MontoFijo;
        private String _CargoAdicional;
        private String _NumeroAutorizacion;
        private String _CampoLibre2;
        private String _CampoLibre3;
        private String _CampoLibre4;
        private byte[] _LongitugLarga={0x12};
        private byte[] _LongitugCorta={0x06};
        #endregion
        #region Metodos de Atributos
        public String getCampoLibre2()
        {
            return _CampoLibre2;
        }
        public void setCampoLibre2(String _CampoLibre2)
        {
            this._CampoLibre2 = _CampoLibre2;
        }
        public String getCampoLibre3()
        {
            return _CampoLibre3;
        }
        public void setCampoLibre3(String _CampoLibre3)
        {
            this._CampoLibre3 = _CampoLibre3;
        }
        public String getCampoLibre4()
        {
            return _CampoLibre4;
        }
        public void setCampoLibre4(String _CampoLibre4)
        {
            this._CampoLibre4 = _CampoLibre4;
        }
        public String getCargoAdicional()
        {
            return _CargoAdicional;
        }
        public void setCargoAdicional(String _CargoAdicional)
        {
            this._CargoAdicional = _CargoAdicional;
        }
        public String getIVA()
        {
            return _IVA;
        }
        public void setIVA(String _IVA)
        {
            this._IVA = _IVA;
        }
        public String getIntereses()
        {
            return _Intereses;
        }
        public void setIntereses(String _Intereses)
        {
            this._Intereses = _Intereses;
        }
        public String getMontoFijo()
        {
            return _MontoFijo;
        }
        public void setMontoFijo(String _MontoFijo)
        {
            this._MontoFijo = _MontoFijo;
        }
        public String getNumeroAutorizacion()
        {
            return _NumeroAutorizacion;
        }
        public void setNumeroAutorizacion(String _NumeroAutorizacion)
        {
            this._NumeroAutorizacion = _NumeroAutorizacion;
        }
        public String getPropina()
        {
            return _Propina;
        }
        public void setPropina(String _Propina)
        {
            this._Propina = _Propina;
        }
        public String getServicios()
        {
            return _Servicios;
        }
        public void setServicios(String _Servicios)
        {
            this._Servicios = _Servicios;
        }
        public String getCashOver()
        {
            return _CashOver;
        }
        public void setCashOver(String _CashOver)
        {
            this._CashOver = _CashOver;
        }
        #endregion

        
        
        protected void setISOAST(String Campo) 
        {
            int i=0,j=0,numCampos = 0, lenCampos=0, lenInfo=12, numCampo;
            byte[] infoAdi, camposAdi = UTILIDADES.StringToBytes(Campo);

            try
            {
                numCampos = camposAdi[lenCampos] - 48;
                lenCampos++;
                for(i=0;i<numCampos;i++)
                {
                    numCampo=camposAdi[lenCampos]-48;
                    lenCampos++;
                    infoAdi = new byte[lenInfo];
                    for(j=0;j<lenInfo;j++) infoAdi[j] = camposAdi[lenCampos++];
                    switch(numCampo)
                    {
                        case 1: _IVA = UTILIDADES.getString(infoAdi); break;
                        case 2: _Propina = UTILIDADES.getString(infoAdi); break;
                        case 3: _Servicios = UTILIDADES.getString(infoAdi); break;
                        case 4: _Intereses = UTILIDADES.getString(infoAdi); break;
                        case 5: _MontoFijo = UTILIDADES.getString(infoAdi); break;

                    }
                }
            }
            catch(Exception e)
            {
                throw new Exception("Error al desempaquetar los campos adicionales de ISO-AST:" + e.Message);
            }

         }
        protected void setISOMN(String Campo) 
        {
            int i=0,j=0,numCampos = 0, lenCampos=0, lenInfo=12, numCampo;
            byte[] infoAdi, camposAdi = UTILIDADES.StringToBytes(Campo);
            try
            {
                numCampos = camposAdi[lenCampos] - 48;
                lenCampos++;
                for(i=0;i<numCampos;i++)
                {
                    numCampo=camposAdi[lenCampos]-48;
                    lenCampos++;
                    infoAdi = new byte[lenInfo];
                    for(j=0;j<lenInfo;j++) infoAdi[j] = camposAdi[lenCampos++];
                    switch(numCampo)
                    {
                        case 1: _IVA = UTILIDADES.getString(infoAdi); break;
                        case 2: _Servicios = UTILIDADES.getString(infoAdi); break;
                        case 3: _Propina = UTILIDADES.getString(infoAdi); break;
                        case 4: _Intereses = UTILIDADES.getString(infoAdi); break;
                        case 5: _MontoFijo = UTILIDADES.getString(infoAdi); break;
                        case 6: _CargoAdicional = UTILIDADES.getString(infoAdi); break;
                        case 7: _CampoLibre2 = UTILIDADES.getString(infoAdi); break;
                        case 8: _CampoLibre3 = UTILIDADES.getString(infoAdi); break;
                        case 9: _CampoLibre4 = UTILIDADES.getString(infoAdi); break;
                    }
                }
            }
            catch(Exception e)
            {
                throw new Exception("Error al desempaquetar los campos adicionales de ISO-MN:" + e.Message);
            }         

         }
        protected void setISOHiCC(String Campo)
        {
            int i=0,j=0,numCampos = 0, lenCampos=0, lenInfo=12, numCampo;
            byte[] infoAdi, camposAdi = UTILIDADES.StringToBytes(Campo);

            try
            {
                numCampos = camposAdi[lenCampos] - 48;
                lenCampos++;
                for(i=0;i<numCampos;i++)
                {
                    numCampo=camposAdi[lenCampos]-48;
                    lenCampos++;
                    infoAdi = new byte[lenInfo];
                    for(j=0;j<lenInfo;j++) infoAdi[j] = camposAdi[lenCampos++];
                    switch(numCampo)
                    {
                        case 1: _IVA = UTILIDADES.getString(infoAdi); break;
                        case 2: _Servicios = UTILIDADES.getString(infoAdi); break;
                        case 3: _Propina = UTILIDADES.getString(infoAdi); break;
                        case 4: _Intereses = UTILIDADES.getString(infoAdi); break;
                        case 5: _MontoFijo = UTILIDADES.getString(infoAdi); break;
                        case 6: _CargoAdicional = UTILIDADES.getString(infoAdi); break;
                        case 7: _CashOver = UTILIDADES.getString(infoAdi); break;
                        case 8: _CampoLibre3 = UTILIDADES.getString(infoAdi); break;
                        case 9: _CampoLibre4 = UTILIDADES.getString(infoAdi); break;
                    }
                }
            }
            catch(Exception e)
            {
                throw new Exception("Error al desempaquetar los campos adicionales de ISO-HiCC:" + e.Message);
            }
         }
        protected void setISODF(String Campo)
        {
            int i=0,j=0,numCampos = 0, lenCampos=0, lenInfo, numCampo;
            byte[] infoAdi, camposAdi = UTILIDADES.StringToBytes(Campo);
            try
            {
                numCampos = camposAdi[lenCampos] - 48;
                lenCampos++;
                for(i=0;i<numCampos;i++)
                {
                    numCampo=camposAdi[lenCampos]-48;
                    lenCampos++;
                    lenInfo=camposAdi[lenCampos];
                    
                    lenCampos++;
                    infoAdi = new byte[lenInfo];
                    for(j=0;j<lenInfo;j++) infoAdi[j] = camposAdi[lenCampos++];
                    switch(numCampo)
                    {
                        case 1: _IVA = UTILIDADES.getString(infoAdi); break;
                        case 2: _Servicios = UTILIDADES.getString(infoAdi); break;
                        case 3: _Propina = UTILIDADES.getString(infoAdi); break;
                        case 4: _Intereses = UTILIDADES.getString(infoAdi); break;
                        case 5: _NumeroAutorizacion = UTILIDADES.getString(infoAdi); break;
                        case 6: _MontoFijo = UTILIDADES.getString(infoAdi); break;
                        case 7: _CashOver = UTILIDADES.getString(infoAdi); break;
                    }
                }
            }
            catch(Exception e)
            {
                throw new Exception("Error al desempaquetar los campos adicionales de ISO-DF:" + e.Message);
            }

         }
        protected void setISOUB(String Campo)
        {
            int i=0,j=0,numCampos = 0, lenCampos=0, lenInfo=12, numCampo;
            byte[] infoAdi, camposAdi = UTILIDADES.StringToBytes(Campo);
            try
            {
                numCampos = camposAdi[lenCampos] - 48;
                lenCampos++;
                for(i=0;i<numCampos;i++)
                {
                    numCampo=camposAdi[lenCampos]-48;
                    lenCampos++;
                    infoAdi = new byte[lenInfo];
                    for(j=0;j<lenInfo;j++) infoAdi[j] = camposAdi[lenCampos++];
                    switch(numCampo)
                    {
                        case 1: _IVA = UTILIDADES.getString(infoAdi); break;
                        case 2: _Servicios = UTILIDADES.getString(infoAdi); break;
                        case 3: _Propina = UTILIDADES.getString(infoAdi); break;
                        case 4: _Intereses = UTILIDADES.getString(infoAdi); break;
                        case 5: _MontoFijo = UTILIDADES.getString(infoAdi); break;
                        case 6: _CargoAdicional = UTILIDADES.getString(infoAdi); break;
                        case 7: _CampoLibre2 = UTILIDADES.getString(infoAdi); break;
                        case 8: _CampoLibre3 = UTILIDADES.getString(infoAdi); break;
                        case 9: _CampoLibre4 = UTILIDADES.getString(infoAdi); break;
                    }
                }
            }
            catch(Exception e)
            {
                throw new Exception("Error al desempaquetar los campos adicionales de ISO-UB:" + e.Message);
            }
         }
        protected void setISOCC(String Campo) 
        {
            int i=0,j=0,numCampos = 0, lenCampos=0, lenInfo, numCampo;
            byte[] infoAdi, camposAdi = UTILIDADES.StringToBytes(Campo);
            try
            {
                numCampos = camposAdi[lenCampos] - 48;
                lenCampos++;
                for(i=0;i<numCampos;i++)
                {
                    numCampo=camposAdi[lenCampos]-48;
                    lenCampos++;
                    lenInfo = camposAdi[lenCampos];
                    lenCampos++;
                    infoAdi = new byte[lenInfo];
                    for(j=0;j<lenInfo;j++) infoAdi[j] = camposAdi[lenCampos++];
                    switch(numCampo)
                    {
                        case 1: _IVA = UTILIDADES.getString(infoAdi); break;
                        case 2: _Servicios = UTILIDADES.getString(infoAdi); break;
                        case 3: _Propina = UTILIDADES.getString(infoAdi); break;
                        case 4: _Intereses = UTILIDADES.getString(infoAdi); break;
                        case 5: _NumeroAutorizacion = UTILIDADES.getString(infoAdi); break;
                        case 6: _MontoFijo = UTILIDADES.getString(infoAdi); break;
                    }
                }
            }
            catch(Exception e)
            {
                throw new Exception("Error al desempaquetar los campos adicionales de ISO-CC:" + e.Message);
            }

         }
        public CamposAdicionales(int trama,String Campo)
        {
            switch(trama)
            {
                case 1:setISOHiCC(Campo);break;
                case 2:setISOMN(Campo);break;
                case 3:setISOAST(Campo);break;
                case 4:setISODF(Campo);break;
                case 5:setISOUB(Campo);break;
                case 6:setISOCC(Campo);break;
                case 7:throw new Exception("Campos adicionales de trama 7 no soportados");
                case 8:throw new Exception("Campos adicionales de trama 8 no soportados");
                case 9:throw new Exception("Campos adicionales de trama 9 no soportados");
                default: throw new Exception("Campos adicionales de trama "+trama+" no soportados");
            }
        }
        public CamposAdicionales() 
        {
        }

        public String toISOHiCC() 
        {
            String Campo="";
            int cantidad=0;
            if(_IVA!=null)
            {
                Campo += "1" + UTILIDADES.zeropad(_IVA, 12);
                cantidad++;
            }
            if(_Servicios!=null)
            {
                Campo += "2" + UTILIDADES.zeropad(_Servicios, 12);
                cantidad++;
            }
            if(_Propina!=null)
            {
                Campo += "3" + UTILIDADES.zeropad(_Propina, 12);
                cantidad++;
            }
            if(_Intereses!=null)
            {
                Campo += "4" + UTILIDADES.zeropad(_Intereses, 12);
                cantidad++;
            }
            if(_MontoFijo!=null)
            {
                Campo += "5" + UTILIDADES.zeropad(_MontoFijo, 12);
                cantidad++;
            }
            if (_CashOver != null)
            {
                Campo += "7" + UTILIDADES.zeropad(_CashOver, 12);
                cantidad++;
            }
            return Convert.ToString(cantidad)+Campo;
        }
        public String toISOMN() 
        {
            String Campo="";
            int cantidad=0;
            if(_IVA!=null)
            {
                Campo += "1" + UTILIDADES.zeropad(_IVA, 12);
                cantidad++;
            }
            if(_Servicios!=null)
            {
                Campo += "2" + UTILIDADES.zeropad(_Servicios, 12);
                cantidad++;
            }
            if(_Propina!=null)
            {
                Campo += "3" + UTILIDADES.zeropad(_Propina, 12);
                cantidad++;
            }
            if(_Intereses!=null)
            {
                Campo += "4" + UTILIDADES.zeropad(_Intereses, 12);
                cantidad++;
            }
            if(_MontoFijo!=null)
            {
                Campo += "5" + UTILIDADES.zeropad(_MontoFijo, 12);
                cantidad++;
            }
            if(_CargoAdicional!=null)
            {
                Campo += "6" + UTILIDADES.zeropad(_CargoAdicional, 12);
                cantidad++;
            }
            if(_CampoLibre2!=null)
            {
                Campo += "7" + UTILIDADES.zeropad(_CampoLibre2, 12);
                cantidad++;
            }
            if(_CampoLibre3!=null)
            {
                Campo += "8" + UTILIDADES.zeropad(_CampoLibre3, 12);
                cantidad++;
            }
            if(_CampoLibre4!=null)
            {
                Campo += "9" + UTILIDADES.zeropad(_CampoLibre4, 12);
                cantidad++;
            }
            return Convert.ToString(cantidad) + Campo;

        }
        public String toISOAST() 
        {
            String Campo="";
            int cantidad=0;
            if(_IVA!=null)
            {
                Campo += "1" + UTILIDADES.zeropad(_IVA, 12);
                cantidad++;
            }
            if(_Propina!=null)
            {
                Campo += "2" + UTILIDADES.zeropad(_Propina, 12);
                cantidad++;
            }
            if(_Servicios!=null)
            {
                Campo += "3" + UTILIDADES.zeropad(_Servicios, 12);
                cantidad++;
            }
            if(_Intereses!=null)
            {
                Campo += "4" + UTILIDADES.zeropad(_Intereses, 12);
                cantidad++;
            }
            if(_MontoFijo!=null)
            {
                Campo += "5" + UTILIDADES.zeropad(_MontoFijo, 12);
                cantidad++;
            }
            return Convert.ToString(cantidad) + Campo;
        }
        public String toISODF() 
        {
            String Campo="";
            int cantidad=0;
            if(_IVA!=null)
            {
                Campo += "1" + UTILIDADES.getString(_LongitugLarga) + UTILIDADES.zeropad(_IVA, 12);
                cantidad++;
            }
            if(_Servicios!=null)
            {
                Campo += "2" + UTILIDADES.getString(_LongitugLarga) + UTILIDADES.zeropad(_Servicios, 12);
                cantidad++;
            }
            if(_Propina!=null)
            {
                Campo += "3" + UTILIDADES.getString(_LongitugLarga) + UTILIDADES.zeropad(_Propina, 12);
                cantidad++;
            }
            if(_Intereses!=null)
            {
                Campo += "4" + UTILIDADES.getString(_LongitugLarga) + UTILIDADES.zeropad(_Intereses, 12);
                cantidad++;
            }
            if(_NumeroAutorizacion!=null)
            {
                Campo += "5" + UTILIDADES.getString(_LongitugCorta) + UTILIDADES.zeropad(_NumeroAutorizacion, 6);
                cantidad++;
            }
            if(_MontoFijo!=null)
            {
                Campo += "6" + UTILIDADES.getString(_LongitugLarga) + UTILIDADES.zeropad(_MontoFijo, 12);
                cantidad++;
            }
            if (_CashOver != null)
            {
                Campo += "7" + UTILIDADES.getString(_LongitugLarga) + UTILIDADES.zeropad(_CashOver, 12);
                cantidad++;
            }
            return Convert.ToString(cantidad) + Campo;
        }
        public String toISOUB() 
        {
            String Campo="";
            int cantidad=0;
            if(_IVA!=null)
            {
                Campo += "1" + UTILIDADES.zeropad(_IVA, 12);
                cantidad++;
            }
            if(_Servicios!=null)
            {
                Campo += "2" + UTILIDADES.zeropad(_Servicios, 12);
                cantidad++;
            }
            if(_Propina!=null)
            {
                Campo += "3" + UTILIDADES.zeropad(_Propina, 12);
                cantidad++;
            }
            if(_Intereses!=null)
            {
                Campo += "4" + UTILIDADES.zeropad(_Intereses, 12);
                cantidad++;
            }
            if(_MontoFijo!=null)
            {
                Campo += "5" + UTILIDADES.zeropad(_MontoFijo, 12);
                cantidad++;
            }
            if(_CargoAdicional!=null)
            {
                Campo += "6" + UTILIDADES.zeropad(_CargoAdicional, 12);
                cantidad++;
            }
            if(_CampoLibre2!=null)
            {
                Campo += "7" + UTILIDADES.zeropad(_CampoLibre2, 12);
                cantidad++;
            }
            if(_CampoLibre3!=null)
            {
                Campo += "8" + UTILIDADES.zeropad(_CampoLibre3, 12);
                cantidad++;
            }
            if(_CampoLibre4!=null)
            {
                Campo += "9" + UTILIDADES.zeropad(_CampoLibre4, 12);
                cantidad++;
            }
            return Convert.ToString(cantidad) + Campo;
        }
        public String toISOCC() 
        {
            String Campo="";
            int cantidad=0;
            if(_IVA!=null)
            {
                Campo += "1" + UTILIDADES.getString(_LongitugLarga) + UTILIDADES.zeropad(_IVA, 12);
                cantidad++;
            }
            if(_Servicios!=null)
            {
                Campo += "2" + UTILIDADES.getString(_LongitugLarga) + UTILIDADES.zeropad(_Servicios, 12);
                cantidad++;
            }
            if(_Propina!=null)
            {
                Campo += "3" + UTILIDADES.getString(_LongitugLarga) + UTILIDADES.zeropad(_Propina, 12);
                cantidad++;
            }
            if(_Intereses!=null)
            {
                Campo += "4" + UTILIDADES.getString(_LongitugLarga) + UTILIDADES.zeropad(_Intereses, 12);
                cantidad++;
            }
            if(_NumeroAutorizacion!=null)
            {
                Campo += "5" + UTILIDADES.getString(_LongitugCorta) + UTILIDADES.zeropad(_NumeroAutorizacion, 6);
                cantidad++;
            }
            if(_MontoFijo!=null)
            {
                Campo += "6" + UTILIDADES.getString(_LongitugLarga) + UTILIDADES.zeropad(_MontoFijo, 12);
                cantidad++;
            }
            return Convert.ToString(cantidad) + Campo;
        }

    }
}
