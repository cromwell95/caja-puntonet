#region CopyrightExpression (C) 2004-2006 Diego Zabaleta, Leonardo Zabaleta
//
// CopyrightExpression © 2004-2006 Diego Zabaleta, Leonardo Zabaleta
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

    internal class MockFieldFormatter : FieldFormatter {

        private bool _formatWasCalled = false;
        private bool _parseWasCalled = false;

        #region Constructors
        /// <summary>
        /// It initializes a new instance of the class.
        /// </summary>
        public MockFieldFormatter() : base( -1 ) {

        }
        #endregion

        #region Properties
        /// <summary>
        /// It returns or sets the flag indicating if <see cref="Format"/> method
        /// was called.
        /// </summary>
        public bool FormatWasCalled {

            get {

                return _formatWasCalled;
            }

            set {

                _formatWasCalled = value;
            }
        }

        /// <summary>
        /// It returns or sets the flag indicating if <see cref="Parse"/> function
        /// was called.
        /// </summary>
        public bool ParseWasCalled {

            get {

                return _parseWasCalled;
            }

            set {

                _parseWasCalled = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// It resets the flags which indicates if <see cref="Format"/> or
        /// <see cref="Parse"/> were called.
        /// </summary>
        public void ResetFlags() {

            _formatWasCalled = false;
            _parseWasCalled = false;
        }

        /// <summary>
        /// Formats the specified field.
        /// </summary>
        /// <param name="field">
        /// It's the field to format.
        /// </param>
        /// <param name="formatterContext">
        /// It's the context of formatting to be used by the method.
        /// </param>
        public override void Format( Field field, ref FormatterContext formatterContext ) {

            _formatWasCalled = true;
        }

        /// <summary>
        /// It parses the information in the parser context and builds the field.
        /// </summary>
        /// <param name="parserContext">
        /// It's the parser context.
        /// </param>
        /// <returns>
        /// The new field built with the information found in the parser context.
        /// </returns>
        public override Field Parse( ref ParserContext parserContext ) {

            _parseWasCalled = true;
            return null;
        }
        #endregion
    }
}
