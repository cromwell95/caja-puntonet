using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace Trx.Messaging.Utilidades
{
   public  class GrabaArchivo
    {
        private String _NombreArchivo;
        private String _sMensaje;
        private bool _boAcumulativo;

        public void grabacion() 
        {
            String sRuta = "";
            UTILIDADES.almacenarArchivo(sRuta,_NombreArchivo,_boAcumulativo,_sMensaje);         
        }
        public GrabaArchivo(String _NombreArchivo, String _sMensaje, bool _boAcumulativo)
        {
            this._NombreArchivo = _NombreArchivo;
            this._sMensaje = _sMensaje;
            this._boAcumulativo = _boAcumulativo;

            ThreadStart theadStartGuardar = new ThreadStart(grabacion);
            Thread theadGuardar = new Thread(theadStartGuardar);
            //theadGuardar.Priority = ThreadPriority.Lowest;
            theadGuardar.Start();
        }

    }
}
