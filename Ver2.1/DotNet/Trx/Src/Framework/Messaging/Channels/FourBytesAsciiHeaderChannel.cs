#region Copyright (C) 2004-2006 Diego Zabaleta, Leonardo Zabaleta
//
// Copyright � 2004-2006 Diego Zabaleta, Leonardo Zabaleta
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

namespace Trx.Messaging.Channels {

	/// <summary>
	/// This class implements a channel capable of interchanging messages
	/// with another system, using the TCP/IP communication protocol.
	/// It uses a 4 bytes header in NBO format to indicate the length of
	/// data.
	/// </summary>
	public class FourBytesAsciiHeaderChannel : TcpChannel {

		#region Constructors
		/// <summary>
		/// It initializes a new instance of <see cref="FourBytesAsciiHeaderChannel"/> class.
		/// </summary>
		/// <param name="formatter">
		/// It's the messages formatter to use.
		/// </param>
		public FourBytesAsciiHeaderChannel( IMessageFormatter formatter) : base( formatter) {

		}

		/// <summary>
		/// It initializes a new instance of <see cref="FourBytesAsciiHeaderChannel"/> class,
		/// and tries to connect to the indicated host and port.
		/// </summary>
		/// <param name="formatter">
		/// It's the messages formatter to use.
		/// </param>
		/// <param name="hostName">
		/// It's the host's name to which it tries to connect.
		/// </param>
		/// <param name="port">
		/// It's the port number in the host to which it tries to connect.
		/// </param>
		public FourBytesAsciiHeaderChannel( IMessageFormatter formatter, string hostName,
			int port) : base( formatter, hostName, port) {

		}
		#endregion

		#region Methods
		/// <summary>
		/// It informs the length of the packet length indicator.
		/// </summary>
		/// <param name="formatting">
		/// true if we're formatting, false if we're receiving a message.
		/// </param>
		/// <returns>
		/// The length of the packet length indicator.
		/// </returns>
		protected override int DataLengthHeaderLength( bool formatting) {

			return 4;
		}

		/// <summary>
		/// Updates the data length to be sent to the remote system.
		/// </summary>
		/// <param name="formatterContext">
		/// It's the message formatter context.
		/// </param>
		protected override void UpdateDataLengthHeader( ref FormatterContext formatterContext) {

			byte[] buffer = formatterContext.GetBuffer();
			int length = formatterContext.DataLength - 4;

			for ( int i = 3; i >= 0; i--) {
				buffer[i] = Convert.ToByte( (length % 10) + 0x30);
				length /= 10;
			}
		}

		/// <summary>
		/// Analyse from the parser context the length of the data packet
		/// to be processed.
		/// </summary>
		/// <param name="parserContext">
		/// It's the parser context which holds the information of the
		/// packet length.
		/// </param>
		/// <param name="lengthConsumed">
		/// Used to indicate data length was consumed.
		/// </param>
		/// <returns>
		/// It's the data packet length which can be turned into a
		/// message.
		/// </returns>
		/// <remarks>
		/// This method consumes data from the parser context.
		/// </remarks>
		protected override int GetDataLength( ref ParserContext parserContext,
			ref bool lengthConsumed) {

			return Convert.ToInt32( parserContext.GetDataAsString( true, 4));
		}

		/// <summary>
		/// Clones the channel.
		/// </summary>
		/// <returns>
		/// The clone.
		/// </returns>
		public override object Clone() {

			FourBytesAsciiHeaderChannel clone = new FourBytesAsciiHeaderChannel( Formatter);
			SetCommonProperties( clone);

			return clone;
		}
		#endregion
	}
}