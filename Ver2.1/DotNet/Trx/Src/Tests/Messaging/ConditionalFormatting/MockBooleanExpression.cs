#region CopyrightExpression (C) 2004-2006 Diego Zabaleta, Leonardo Zabaleta
//
// CopyrightExpression � 2004-2006 Diego Zabaleta, Leonardo Zabaleta
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
using Trx.Messaging.ConditionalFormatting;

namespace Trx.Tests.Messaging.ConditionalFormatting {

    internal class MockBooleanExpression : IBooleanExpression {

        private bool _resultValue;

        #region Constructors
        /// <summary>
        /// It initializes a new instance of the class.
        /// </summary>
        public MockBooleanExpression( bool resultValue ) {

            _resultValue = resultValue;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Evaluates the expression when parsing a message.
        /// </summary>
        /// <param name="parserContext">
        /// It's the parser context.
        /// </param>
        /// <returns>
        /// A boolean value.
        /// </returns>
        public bool EvaluateParse( ref ParserContext parserContext ) {

            return _resultValue;
        }

        /// <summary>
        /// Evaluates the expression when formating a message.
        /// </summary>
        /// <param name="field">
        /// It's the field to format.
        /// </param>
        /// <param name="formatterContext">
        /// It's the context of formatting to be used by the method.
        /// </param>
        /// <returns>
        /// A boolean value.
        /// </returns>
        public bool EvaluateFormat( Trx.Messaging.Field field,
            ref FormatterContext formatterContext ) {

            return _resultValue;
        }
        #endregion
    }
}
