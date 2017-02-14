#region Copyright (C) 2004-2006 Diego Zabaleta, Leonardo Zabaleta
//
// Copyright © 2004-2006 Diego Zabaleta, Leonardo Zabaleta
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
#endregion

using System;
using System.Threading;

using Trx.Messaging;
using System.Net.Sockets;
using Trx.Messaging.Iso8583;
using Trx.Messaging.Channels;
using Trx.Messaging.FlowControl;

using log4net;
using System.Collections;

// Configure logging for this assembly using the 'Merchant.exe.log4net' file
[assembly: log4net.Config.XmlConfigurator( ConfigFileExtension = "log4net", Watch = true )]

namespace TrxExamples.Messaging {

	/// <summary>
	/// This class tries to connect to the port 8583 and when the connection
	/// is established, it periodically send echo test messages.
	/// </summary>
	/// <remarks>
	/// This example must be used in conjunction with the Acquirer example.
	/// </remarks>
	public class Merchant {

		private const int Field3ProcCode = 3;
		private const int Field7TransDateTime = 7;
		private const int Field11Trace = 11;
		private const int Field24Nii = 24;
		private const int Field41TerminalCode = 41;
		private const int Field42MerchantCode = 42;

        //private ClientPeer _clientPeer;
        //private Timer _timer;
		private VolatileStanSequencer _sequencer;
        private Socket _socket;
        private AddressFamily _family;
        //private IMessageFormatter _formatter;

        //private int _requestsCnt = 0;
        //private int _expiredRequests = 0;

		/// <summary>
		/// Initializes a new instance of this class.
		/// </summary>
        /// <param name="hostname">
        /// The host name of the computer where the Acquirer.exe program is running.
        /// </param>
        //public Merchant( string hostname) {

        //    // Create a client peer to connect to remote system. The messages
        //    // will be matched using fields 41 and 11.
        //    //_clientPeer = new ClientPeer( "Merchant", new TcpChannel(
        //    //    new Iso8583Bin1987MessageFormatter(), "192.168.123.39", 9004 ),
        //    //    new BasicMessagesIdentifier( Field41TerminalCode, Field11Trace ) );

			

        //    //_timer = new Timer( new TimerCallback( OnTimer ), null, 1000, 2000 );

        //    //_clientPeer.Connect();
        //}

        ///// <summary>
        ///// Returns the number of requests made.
        ///// </summary>
        //public int RequestsCount {

        //    get {

        //        return _requestsCnt;
        //    }
        //}

        ///// <summary>
        ///// Returns the number of expired requests (not responded by the remote peer).
        ///// </summary>
        //public int ExpiredRequests {

        //    get {

        //        return _expiredRequests;
        //    }
        //}

		/// <summary>
		/// Called when the timer ticks.
		/// </summary>
		/// <param name="state">
		/// Null.
		/// </param>
        //private void OnTimer( object state ) {

        //    lock ( this ) {
        //        if ( _clientPeer.IsConnected ) {

        //            // Build echo test message.
        //            Iso8583Message echoMsg = new Iso8583Message( 800 );
        //            echoMsg.Fields.Add( Field3ProcCode, "990000" );
        //            DateTime transmissionDate = DateTime.Now;
        //            echoMsg.Fields.Add( Field7TransDateTime, string.Format( "{0}{1}",
        //                string.Format( "{0:00}{1:00}", transmissionDate.Month, transmissionDate.Day),
        //                string.Format( "{0:00}{1:00}{2:00}", transmissionDate.Hour,
        //                transmissionDate.Minute, transmissionDate.Second)));
        //            echoMsg.Fields.Add( Field11Trace, _sequencer.Increment().ToString() );
        //            echoMsg.Fields.Add( Field24Nii, "101" );
        //            echoMsg.Fields.Add( Field41TerminalCode, "12131415" );
        //            echoMsg.Fields.Add( Field42MerchantCode, "000000852963  " );
        //            IMessageFormatter _formatter = new Iso8583Bin1987MessageFormatter();
        //            FormatterContext formatterContext = new FormatterContext(FormatterContext.DefaultBufferSize);
        //            echoMsg.Formatter = _formatter;
        //            echoMsg.Formatter.Format(echoMsg, ref formatterContext);
                    
        //            byte[] bBuffer = formatterContext.GetData();
        //            byte[] bHeader = Hex2Byte("6000018000");
        //            int iTalla = bBuffer.GetLength() + bHeader.GetLength() ;
        //            byte[] bTalla=Hex2Byte(Convert.ToString(iTalla, 16).PadLeft(4,'0'));
        //            _family = AddressFamily.InterNetwork;
        //            _socket = new Socket(_family, SocketType.Stream, ProtocolType.Tcp);
        //            _socket.Connect("192.168.123.39", 9004);
        //            _socket.Send(bTalla);
        //            _socket.Send(bHeader);
        //            _socket.Send(bBuffer);
        //            _socket.Close();
        //            PeerRequest request = new PeerRequest(_clientPeer, echoMsg);
        //            request.Send();
        //            request.WaitResponse( 1000 );

        //            if ( request.Expired ) {
        //                _expiredRequests++;
        //            }
        //            else {
        //                _requestsCnt++;
        //            }
        //        }
        //    }
        //}

		/// <summary>
		/// Stop merchant activity.
		/// </summary>
        //public void Stop() {

        //    lock ( this ) {
        //        _timer.Change( Timeout.Infinite, Timeout.Infinite );
        //        _clientPeer.Close();
        //    }
        //}
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
        public static Hashtable TransaformaIso(byte[] formatterContext)
        {
            
            Hashtable hashMensaje = new Hashtable();
            String sHexOutPut = "";
            int iTalla;
            byte[] bHeader = new byte[5];
            byte[] bBody;
            byte b;
            for (int i = 0; i < 2;i++ )
            {
                sHexOutPut = sHexOutPut + String.Format("{0:X}", Convert.ToInt32(formatterContext[i])).PadLeft(2, '0');
            }
            for (int i = 2; i < 7; i++)
            {
                bHeader[i-2] = formatterContext[i];
            }
            iTalla = Convert.ToInt32(sHexOutPut, 16);
            bBody = new byte[iTalla - 5];
            for (int i = 7; i < iTalla+2 ; i++)
            {
                bBody[i - 7] = formatterContext[i];
            }
            hashMensaje.Add("Talla", iTalla);
            hashMensaje.Add("Header", bHeader);
            hashMensaje.Add("Body", bBody);
            return hashMensaje;
        }
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
        //[STAThread]
		static void Main( string[] args ) 
        {
            Console.WriteLine("Is running.");
            VolatileStanSequencer _sequencer;
            Socket _socket;
            AddressFamily _family;
            _sequencer = new VolatileStanSequencer();
            Iso8583Message echoMsg = new Iso8583Message(800);
            echoMsg.Fields.Add(Field3ProcCode, "990000");
            DateTime transmissionDate = DateTime.Now;
            echoMsg.Fields.Add(Field7TransDateTime, string.Format("{0}{1}",string.Format("{0:00}{1:00}", transmissionDate.Month, transmissionDate.Day), string.Format("{0:00}{1:00}{2:00}", transmissionDate.Hour,transmissionDate.Minute, transmissionDate.Second)));
            echoMsg.Fields.Add(Field11Trace, _sequencer.Increment().ToString());
            echoMsg.Fields.Add(Field24Nii, "101");
            echoMsg.Fields.Add(Field41TerminalCode, "12131415");
            echoMsg.Fields.Add(Field42MerchantCode, "000000852963  ");
            IMessageFormatter _formatter = new Iso8583Bin1987MessageFormatter();
            
            //formatterContext.GetData();
            echoMsg.Formatter = _formatter;
            //echoMsg.Formatter.Format(echoMsg, ref formatterContext);
            
            byte[] bBufferRecive;
            byte[] bBuffer = echoMsg.GetBytes();
            byte[] bHeader = Hex2Byte("6000018000");
            int iTalla = bBuffer.Length + bHeader.Length;
            byte[] bTalla = Hex2Byte(Convert.ToString(iTalla, 16).PadLeft(4, '0'));
            _family = AddressFamily.InterNetwork;
            _socket = new Socket(_family, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect("192.168.123.39", 9004);
            _socket.Send(bTalla);
            _socket.Send(bHeader);
            _socket.Send(bBuffer);
            Console.WriteLine(Byte2Hex(bTalla) + Byte2Hex(bHeader) + Byte2Hex(bBuffer));
            _socket.ReceiveTimeout = 10000;
            bBufferRecive = new byte[_socket.ReceiveBufferSize];
            _socket.Receive(bBufferRecive);
            Hashtable ISO = TransaformaIso(bBufferRecive);
            byte[] bBody = (byte[])ISO["Body"];
            //FormatterContext formatterContext = new FormatterContext(FormatterContext.DefaultBufferSize);
            //formatterContext.Write(bBody);
            ParserContext _parserContext = new ParserContext(bBody.Length);
            _parserContext.Initialize();
            _parserContext.ResizeBuffer(bBody.Length);
            _parserContext.Write(bBody);

            Iso8583Message echoMsgResp = (Iso8583Message)echoMsg.Formatter.Parse(ref _parserContext);
            //echoMsg.Formatter.Format(echoMsg, ref formatterContext);
            Console.WriteLine(Byte2Hex(bBody));
            Console.WriteLine(echoMsgResp.ToString());
            _socket.Close();
			Console.ReadLine();
		}
	}
}
