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
using System.Collections;

// TODO: Translate to english.

namespace Trx.Messaging.FlowControl {

	/// <summary>
	/// Implementa una colecci�n de elementos de tipo <see cref="Peer"/>.
	/// </summary>
	/// <remarks>
	/// El nombre del punto de conexi�n es empleado como clave dentro de
	/// la colecci�n.
	/// </remarks>
	public class PeerCollection : IEnumerable, ICollection {

		private Hashtable _peers;

		#region Constructors
		/// <summary>
		/// Crea una nueva instancia de la colecci�n de puntos de conexi�n.
		/// </summary>
		public PeerCollection() {

			_peers = new Hashtable( 8);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Retorna o asigna un punto de conexi�n de la colecci�n de puntos
		/// de conexi�n.
		/// </summary>
		/// <remarks>
		/// Si el punto de conexi�n no existe en la colecci�n, un valor nulo
		/// es retornado.
		///
		/// Si existe se est� agregando y ya existe es reemplazado.
		/// </remarks>
		public Peer this[string name] {

			get {

				return ( Peer)_peers[name];
			}

			set {

				if ( value == null) {
					return;
				}

				_peers[name] = value;
			}
		}

		/// <summary>
		/// Retorna la cantidad de puntos de conexi�n incluidos en la colecci�n.
		/// </summary>
		public int Count {

			get {

				return _peers.Count;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Agrega un punto de conexi�n a la colecci�n.
		/// </summary>
		/// <param name="peer">
		/// Es el punto de conexi�n a agregar a la colecci�n.
		/// </param>
		/// <remarks>
		/// Si existe es reemplazado.
		/// </remarks>
		public void Add( Peer peer) {

			if ( peer == null) {
				return;
			}

			this[peer.Name] = peer;
		}

		/// <summary>
		/// Elimina el punto de conexi�n cuyo nombre coincida con el
		/// especificado.
		/// </summary>
		/// <param name="name">
		/// Es el nombre del punto de conexi�n que se desea eliminar
		/// de la colecci�n.
		/// </param>
		public void Remove( string name) {

			_peers.Remove( name);
		}
		/// <summary>
		/// Elimina todos los puntos de conexi�n de la colecci�n.
		/// </summary>
		public void Clear() {

			if ( _peers.Count == 0) {
				return;
			}

			_peers.Clear();
		}

		/// <summary>
		/// Indica si la colecci�n contiene un punto de conexi�n con el nombre
		/// indicado.
		/// </summary>
		/// <param name="name">
		/// Es el nombre del punto de conexi�n que se desea conocer si existe en
		/// la colecci�n.
		/// </param>
		/// <returns>
		/// <see langref="true"/> si el punto de conexi�n est� contenido en la
		/// colecci�n, <see langref="false"/> en caso contrario.
		/// </returns>
		public bool Contains( string name) {

			return _peers.Contains( name);
		}
		#endregion

		#region Implementation of IEnumerable
		/// <summary>
		/// Devuelve un enumerador de la colecci�n.
		/// </summary>
		/// <returns>
		/// El enumerador sobre la colecci�n.
		/// </returns>
		public IEnumerator GetEnumerator() {

			return new PeersEnumerator( _peers);
		}

		/// <summary>
		/// Implementa el enumerador de la colecci�n.
		/// </summary>
		private class PeersEnumerator : IEnumerator {

			private IEnumerator _peersEnumerator;

			#region Constructors
			/// <summary>
			/// Crea una nueva instancia de la clase <see cref="PeersEnumerator"/>.
			/// </summary>
			/// <param name="peers">
			/// Es la tabla de hash que contiene los campos.
			/// </param>
			public PeersEnumerator( Hashtable peers) {

				_peersEnumerator = peers.GetEnumerator();
			}
			#endregion

			#region Implementation of IEnumerator
			/// <summary>
			/// Reinicia la enumeraci�n.
			/// </summary>
			public void Reset() {
			
				_peersEnumerator.Reset();
			}

			/// <summary>
			/// Se mueve al siguiente elemento en la enumeraci�n.
			/// </summary>
			/// <returns>
			/// Un valor verdadero si logr� posicionarse en el siguiente elemento de
			/// la enumeraci�n, un valor igual a falso cuando no existen mas elementos
			/// a enumerar.
			/// </returns>
			public bool MoveNext() {

				return _peersEnumerator.MoveNext();
			}

			/// <summary>
			/// Retorna el elemento actual de la enumeraci�n.
			/// </summary>
			public object Current {

				get {

					return ( ( DictionaryEntry)_peersEnumerator.Current).Value;
				}
			}
			#endregion
		}
		#endregion

		#region ICollection Members
		/// <summary>
		/// Informa si la colecci�n est� sincronizada.
		/// </summary>
		public bool IsSynchronized {

			get {

				return false;
			}
		}

		/// <summary>
		/// Copia los elementos de la colecci�n en el array indicado.
		/// </summary>
		/// <param name="array">
		/// Es el array destino donde se copian los elementos.
		/// </param>
		/// <param name="index">
		/// Es el �ndice en el array desde donde comienza la copia de los
		/// elementos.
		/// </param>
		public void CopyTo( Array array, int index) {

			_peers.CopyTo( array, index);
		}

		/// <summary>
		/// Retorna un objeto que encapsula a la colecci�n sincronizada.
		/// </summary>
		public object SyncRoot {

			get {

				return this;
			}
		}
		#endregion
	}
}