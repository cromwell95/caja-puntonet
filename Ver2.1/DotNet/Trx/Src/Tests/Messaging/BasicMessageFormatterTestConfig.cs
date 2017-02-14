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
using Trx.Messaging;

namespace Trx.Tests.Messaging {

	/// <summary>
	/// Define los datos necesarios para efectuar el test de un
	/// formateador de mensajes.
	/// </summary>
	public struct MessageFormatterTestConfig {

		/// <summary>
		/// Son los campos a emplear en el test.
		/// </summary>
		public int[] Fields;

		/// <summary>
		/// Es el formateador del header.
		/// </summary>
		public StringMessageHeaderFormatter HeaderFormatter;

		/// <summary>
		/// Es el header a emplear.
		/// </summary>
		public MessageHeader Header;

		/// <summary>
		/// Es el resultado esperado del formateo.
		/// </summary>
		public string ExpectedFormattedData;

		#region Constructors
		/// <summary>
		/// Construye una nueva instancia de <see cref="MessageFormatterTestConfig"/>.
		/// </summary>
		/// <param name="fields">
		/// Son los campos a emplear en el test.
		/// </param>
		/// <param name="headerFormatter">
		/// Es el formateador del header.
		/// </param>
		/// <param name="header">
		/// Es el header a emplear.
		/// </param>
		/// <param name="expectedFormattedData">
		/// Es el resultado esperado del formateo.
		/// </param>
		public MessageFormatterTestConfig( int[] fields,
			StringMessageHeaderFormatter headerFormatter,
			MessageHeader header, string expectedFormattedData) {

			Fields = fields;
			HeaderFormatter = headerFormatter;
			Header = header;
			ExpectedFormattedData = expectedFormattedData;
		}
		#endregion
	}
}