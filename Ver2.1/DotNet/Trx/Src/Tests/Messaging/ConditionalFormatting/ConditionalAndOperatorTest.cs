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
    /// Test fixture for ConditionalAndOperator.
    /// </summary>
    [TestFixture( Description = "ConditionalAndOperator functionality tests." )]
    public class ConditionalAndOperatorTest {

        #region Class constructors
        /// <summary>
        /// Default <see cref="ConditionalAndOperatorTest"/> constructor.
        /// </summary>
        public ConditionalAndOperatorTest() {

        }
        #endregion

        #region Class methods
        /// <summary>
        /// Instantiation test.
        /// </summary>
        [Test( Description = "Instantiation and properties test" )]
        public void InstantiationAndProperties() {

            ConditionalAndOperator op = new ConditionalAndOperator();

            Assert.IsNull( op.LeftExpression );
            Assert.IsNull( op.RightExpression );

            MockBooleanExpression left = new MockBooleanExpression( true );
            MockBooleanExpression right = new MockBooleanExpression( true );

            op = new ConditionalAndOperator( left, right );

            Assert.IsTrue( op.LeftExpression == left );
            Assert.IsTrue( op.RightExpression == right );

            op.LeftExpression = null;
            op.RightExpression = null;

            Assert.IsNull( op.LeftExpression );
            Assert.IsNull( op.RightExpression );
        }

        /// <summary>
        /// Evaluation test.
        /// </summary>
        [Test( Description = "Evaluation test" )]
        public void Evaluate() {

            ParserContext pc = new ParserContext( ParserContext.DefaultBufferSize );
            FormatterContext fc = new FormatterContext( FormatterContext.DefaultBufferSize );

            ConditionalAndOperator op = new ConditionalAndOperator(
                new MockBooleanExpression( true ), new MockBooleanExpression( true ) );

            Assert.IsTrue( op.EvaluateParse( ref pc ) );
            Assert.IsTrue( op.EvaluateFormat( new StringField( 3, "000000" ), ref fc ) );

            op = new ConditionalAndOperator(
                new MockBooleanExpression( false ), new MockBooleanExpression( true ) );

            Assert.IsFalse( op.EvaluateParse( ref pc ) );
            Assert.IsFalse( op.EvaluateFormat( new StringField( 3, "000000" ), ref fc ) );

            op = new ConditionalAndOperator(
                new MockBooleanExpression( true ), new MockBooleanExpression( false ) );

            Assert.IsFalse( op.EvaluateParse( ref pc ) );
            Assert.IsFalse( op.EvaluateFormat( new StringField( 3, "000000" ), ref fc ) );

            op = new ConditionalAndOperator(
                new MockBooleanExpression( false ), new MockBooleanExpression( false ) );

            Assert.IsFalse( op.EvaluateParse( ref pc ) );
            Assert.IsFalse( op.EvaluateFormat( new StringField( 3, "000000" ), ref fc ) );
        }
        #endregion
    }
}
