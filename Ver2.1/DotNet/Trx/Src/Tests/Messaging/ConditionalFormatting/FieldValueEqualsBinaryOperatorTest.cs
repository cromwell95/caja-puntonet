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
using Trx.Messaging;
using Trx.Messaging.ConditionalFormatting;
using NUnit.Framework;

namespace Trx.Tests.Messaging.ConditionalFormatting {

    /// <summary>
    /// Test fixture for FieldValueEqualsBinaryOperator.
    /// </summary>
    [TestFixture( Description = "FieldValueEqualsBinaryOperator functionality tests." )]
    public class FieldValueEqualsBinaryOperatorTest {

        #region Class constructors
        /// <summary>
        /// Default <see cref="FieldValueEqualsBinaryOperatorTest"/> constructor.
        /// </summary>
        public FieldValueEqualsBinaryOperatorTest() {

        }
        #endregion

        #region Class methods
        /// <summary>
        /// Instantiation test.
        /// </summary>
        [Test( Description = "Instantiation and properties test" )]
        public void InstantiationAndProperties() {

            FieldValueEqualsBinaryOperator ee = new FieldValueEqualsBinaryOperator();

            Assert.IsNull( ee.ValueExpression );
            Assert.IsNull( ee.MessageExpression );

            BinaryConstantExpression bce = new BinaryConstantExpression( "202030302020" );
            MessageExpression me = new MessageExpression( 3 );
            ee = new FieldValueEqualsBinaryOperator( me, bce );

            Assert.IsTrue( ee.ValueExpression == bce );
            Assert.IsTrue( ee.MessageExpression == me );

            ee.ValueExpression = null;
            ee.MessageExpression = null;

            Assert.IsNull( ee.ValueExpression );
            Assert.IsNull( ee.MessageExpression );

            try {
                ee.ValueExpression = new StringConstantExpression( "Test" );
                Assert.Fail();
            }
            catch ( ArgumentException ) {
            }
        }

        /// <summary>
        /// Evaluation test.
        /// </summary>
        [Test( Description = "Evaluation test" )]
        public void Evaluate() {

            ParserContext pc = new ParserContext( ParserContext.DefaultBufferSize );
            FormatterContext fc = new FormatterContext( FormatterContext.DefaultBufferSize );

            FieldValueEqualsBinaryOperator ee = new FieldValueEqualsBinaryOperator(
                new MessageExpression( 1 ), new BinaryConstantExpression( null ) );

            Message msg = new Message();
            msg.Fields.Add( new BinaryField( 1, null ) );

            // Both values are null.
            pc.CurrentMessage = msg;
            Assert.IsTrue( ee.EvaluateParse( ref pc ) );
            fc.CurrentMessage = msg;
            Assert.IsTrue( ee.EvaluateFormat( new StringField( 3, "000000" ), ref fc ) );

            ee = new FieldValueEqualsBinaryOperator(
                new MessageExpression( 1 ), new BinaryConstantExpression( "1520253035404550" ) );
            // Field value is null.
            Assert.IsFalse( ee.EvaluateParse( ref pc ) );
            Assert.IsFalse( ee.EvaluateFormat( new StringField( 3, "000000" ), ref fc ) );

            ee = new FieldValueEqualsBinaryOperator(
                new MessageExpression( 52 ), new BinaryConstantExpression( null ) );
            msg = MessagesProvider.GetMessage();
            // Constant is null.
            pc.CurrentMessage = msg;
            Assert.IsFalse( ee.EvaluateParse( ref pc ) );
            fc.CurrentMessage = msg;
            Assert.IsFalse( ee.EvaluateFormat( new StringField( 3, "000000" ), ref fc ) );

            ee = new FieldValueEqualsBinaryOperator(
                new MessageExpression( 52 ), new BinaryConstantExpression( "152025303540" ) );
            // Different lengths.
            Assert.IsFalse( ee.EvaluateParse( ref pc ) );
            Assert.IsFalse( ee.EvaluateFormat( new StringField( 3, "000000" ), ref fc ) );

            ee = new FieldValueEqualsBinaryOperator(
                new MessageExpression( 52 ), new BinaryConstantExpression( "1520253035404551" ) );
            // Different data.
            Assert.IsFalse( ee.EvaluateParse( ref pc ) );
            Assert.IsFalse( ee.EvaluateFormat( new StringField( 3, "000000" ), ref fc ) );

            ee = new FieldValueEqualsBinaryOperator(
                new MessageExpression( 52 ), new BinaryConstantExpression( "1520253035404550" ) );
            // Equals.
            Assert.IsTrue( ee.EvaluateParse( ref pc ) );
            Assert.IsTrue( ee.EvaluateFormat( new StringField( 3, "000000" ), ref fc ) );
        }
        #endregion
    }
}
