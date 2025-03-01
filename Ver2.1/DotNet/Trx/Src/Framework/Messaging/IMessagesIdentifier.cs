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

// TODO: Translate spanish -> english.

namespace Trx.Messaging {

	/// <summary>
	/// Esta interfaz define qu� debe implementar una clase para calcular
	/// el identificador de los mensajes.
	/// </summary>
	public interface IMessagesIdentifier {

		/// <summary>
		/// Calcula el identificador del mensaje dado.
		/// </summary>
		/// <param name="message">
		/// Es el mensaje del que se quiere saber su identificador.
		/// </param>
		/// <returns>
		/// El identificador del mensaje.
		/// </returns>
		object ComputeIdentifier( Message message);
	}
}