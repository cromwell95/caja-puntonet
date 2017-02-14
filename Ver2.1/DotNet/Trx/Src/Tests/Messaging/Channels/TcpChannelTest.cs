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
using Trx.Messaging.Channels;
using NUnit.Framework;

namespace Trx.Tests.Messaging.Channels {

	/// <summary>
	/// Test fixture for TcpChannel.
	/// </summary>
	[TestFixture( Description="Tcp channel tests.")]
	public class TcpChannelTest {

		#region Constructors
		/// <summary>
		/// Construye e inicializa una nueva instancia de la clase
		/// <see cref="TcpChannelTest"/>.
		/// </summary>
		public TcpChannelTest() {

		}
		#endregion

		#region Methods
		/// <summary>
		/// This method will be called by NUnit for test setup.
		/// </summary>
		[SetUp]
		public void SetUp() {

		}

		/// <summary>
		/// Simple.
		/// </summary>
		[Test( Description="Simple.")]
		public void Simple() {

			/*

			TcpChannel channel = new TcpChannel( new BasicMessageFormatter());

			channel.Connected += new ChannelConnectedEventHandler( channelConnected);
			channel.Disconnected += new ChannelDisconnectedEventHandler( channelDisconnected);

			channel.Connect( "localhost", 7);

			lock ( this) {
				Monitor.Wait( this, 15000);
			}

			Console.WriteLine( channel.IsConnected);

			Thread.Sleep( 120000);

			channel.Close();
		*/
		}

		private void channelConnected( object sender, EventArgs e) {

			lock ( this) {
				Monitor.Pulse( this);
			}
		}

		private void channelDisconnected( object sender, EventArgs e) {

			lock ( this) {
				Monitor.Pulse( this);
			}
		}
		#endregion
	}
}
