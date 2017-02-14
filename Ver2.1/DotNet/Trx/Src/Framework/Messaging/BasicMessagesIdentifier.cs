

using System;
using System.Text;

// TODO: Translate spanish -> english.

namespace Trx.Messaging {

	/// <summary>
	/// Implementa un identificador de mensaje básico que para
	/// realizar su función concatena los campos indicados en el
	/// momento de instanciación.
	/// </summary>
	public class BasicMessagesIdentifier : IMessagesIdentifier {

		private int[] _fields;

		#region Constructors
		/// <summary>
		/// Inicializa una nueva instancia de la clase
		/// <see cref="BasicMessagesIdentifier"/>.
		/// </summary>
		/// <param name="fields">
		/// Son los campos a concatenar para obtener el identificador
		/// del mensaje.
		/// </param>
		public BasicMessagesIdentifier( int[] fields) {

			_fields = fields;
		}

		/// <summary>
		/// Inicializa una nueva instancia de la clase
		/// <see cref="BasicMessagesIdentifier"/>.
		/// </summary>
		/// <param name="firstFieldNumber">
		/// Es el primer campo a concatenar para obtener el identificador
		/// del mensaje.
		/// </param>
		/// <param name="secondFieldNumber">
		/// Es el segundo campo a concatenar para obtener el identificador
		/// del mensaje.
		/// </param>
		public BasicMessagesIdentifier( int firstFieldNumber,
			int secondFieldNumber) {

			_fields = new int[] { firstFieldNumber, secondFieldNumber};
		}

		/// <summary>
		/// Inicializa una nueva instancia de la clase
		/// <see cref="BasicMessagesIdentifier"/>.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el campo que contiene el identificador del mensaje.
		/// </param>
		public BasicMessagesIdentifier( int fieldNumber) {

			_fields = new int[] { fieldNumber};
		}
		#endregion

		#region Methods
		/// <summary>
		/// Calcula el identificador del mensaje dado.
		/// </summary>
		/// <param name="message">
		/// Es el mensaje del que se quiere saber su identificador.
		/// </param>
		/// <returns>
		/// El identificador del mensaje.
		/// </returns>
		public object ComputeIdentifier( Message message) {

			if ( !message.Fields.Contains( _fields)) {
				return null;
			}

			if ( _fields.Length > 1) {
				StringBuilder identifier = new StringBuilder();

				for ( int i = 0; i < _fields.Length; i++) {
					identifier.Append( message.Fields[_fields[i]].ToString());
				}

				return identifier.ToString();
			} else {
				return message.Fields[_fields[0]].ToString();
			}
		}
		#endregion
	}
}
