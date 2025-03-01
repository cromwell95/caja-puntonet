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
using Trx.Messaging.ConditionalFormatting;
using NUnit.Framework;

namespace Trx.Tests.Messaging.ConditionalFormatting {

    /// <summary>
    /// Test fixture for BinaryConstantExpression.
    /// </summary>
    [TestFixture( Description = "BinaryConstantExpression functionality tests." )]
    public class BinaryConstantExpressionTest {

        #region Class constructors
        /// <summary>
        /// Default <see cref="BinaryConstantExpressionTest"/> constructor.
        /// </summary>
        public BinaryConstantExpressionTest() {

        }
        #endregion

        #region Class methods
        /// <summary>
        /// Instantiation and properties test.
        /// </summary>
        [Test( Description = "Instantiation and properties test" )]
        public void InstantiationAndProperties() {

            BinaryConstantExpression bce = new BinaryConstantExpression();

            Assert.IsTrue( bce.Constant == null );
            Assert.IsTrue( bce.GetValue() == null );

            bce = new BinaryConstantExpression( "303020" );
            Assert.IsTrue( bce.Constant == "303020" );
            Assert.IsTrue( MessagesProvider.CompareByteArrays( bce.GetValue(),
                new byte[] { 0x30, 0x30, 0x20 } ) );

            bce.Constant = null;
            Assert.IsTrue( bce.Constant == null );
            Assert.IsTrue( bce.GetValue() == null );

            bce.Constant = "203020";
            Assert.IsTrue( bce.Constant == "203020" );
            Assert.IsTrue( MessagesProvider.CompareByteArrays( bce.GetValue(),
                new byte[] { 0x20, 0x30, 0x20 } ) );

            bce.Constant = "2030A";
            Assert.IsTrue( bce.Constant == "2030A" );
            Assert.IsTrue( MessagesProvider.CompareByteArrays( bce.GetValue(),
                new byte[] { 0x20, 0x30, 0xA0 } ) );
        }
        #endregion
    }
}
