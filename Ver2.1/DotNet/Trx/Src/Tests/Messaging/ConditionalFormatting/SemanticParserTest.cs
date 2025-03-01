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
using System.IO;
using Trx.Messaging;
using Trx.Messaging.Iso8583;
using Trx.Messaging.ConditionalFormatting;
using NUnit.Framework;

namespace Trx.Tests.Messaging.ConditionalFormatting {

	/// <summary>
	/// Test fixture for ConditionalFormatingSemanticParser.
	/// </summary>
	[TestFixture( Description="ConditionalFormatingSemanticParser functionality tests.")]
	public class SemanticParserTest {

        public static string SimpleExpression1Value = "3 = '000000'";
        public static string SimpleExpression2Value = "4 = 303132333435H";
        public static string SimpleExpression3Value = "mti = 800";
        public static string SimpleExpression4Value = "3[0,2] = '00'";
        public static string SimpleExpression5Value = "4[10,2] = 3030H";
        public static string SubFieldExpression1Value = "48.5 = '123'";
        public static string SubFieldExpression2Value = "63.4.7 = 2020h";
        public static string SubFieldExpression3Value = "48.mti = 800";
        public static string SubFieldExpression4Value = "48.5[1,2] = '23'";
        public static string SubFieldExpression5Value = "63.4.7[5,3] = 202020h";
        public static string ParentExpression1Value = "Parent.4 = '000000072000'";
        public static string ParentExpression2Value = "parent.Parent.49 = 383538H";
        public static string ParentExpression3Value = "Parent.MTI = 200";
        public static string ParentExpression4Value = "Parent.4[10,2] = '50'";
        public static string ParentExpression5Value = "parent.Parent.49[0,1] = 38H";
        public static string ParentSubFieldExpression1Value = "Parent.28.5 = 'ABC'";
        public static string ParentSubFieldExpression2Value = "Parent.parent.62.3.1 = ' '";
        public static string ParentSubFieldExpression3Value = "Parent.parent.4.mti = 800";
        public static string SimpleEmptyStringExpressionValue = "6   = ''";
        public static string IsSetExpression1Value = "isset( 3 )";
        public static string IsSetExpression2Value = "isset(1.3)";
        public static string IsSetExpression3Value = "isSet(parent.3 )";
        public static string IsSetExpression4Value = "ISSET( parent.parent.4.5)";

		#region Class constructors
		/// <summary>
		/// Default <see cref="ConditionalFormatingSemanticParserTest"/> constructor.
		/// </summary>
		public SemanticParserTest() {

		}
		#endregion

		#region Class methods
        /// <summary>
        /// Parsing simple expression 1.
        /// </summary>
        [Test( Description = "Parsing simple expression 1" )]
        public void ParsingSimpleExpression1() {

            SemanticParserTest.CheckSimpleExpression1( CompileExpression( SimpleExpression1Value ) as
                FieldValueEqualsStringOperator );
        }

        /// <summary>
        /// Parsing simple expression 2.
        /// </summary>
        [Test( Description = "Parsing simple expression 2" )]
        public void ParsingSimpleExpression2() {

            CheckSimpleExpression2( CompileExpression( SimpleExpression2Value ) as
                FieldValueEqualsBinaryOperator );
        }

        /// <summary>
        /// Parsing simple expression 3.
        /// </summary>
        [Test( Description = "Parsing simple expression 3" )]
        public void ParsingSimpleExpression3() {

            CheckSimpleExpression3( CompileExpression( SimpleExpression3Value ) as MtiEqualsExpression );
        }

        /// <summary>
        /// Parsing simple expression 4.
        /// </summary>
        [Test( Description = "Parsing simple expression 4" )]
        public void ParsingSimpleExpression4() {

            CheckSimpleExpression4( CompileExpression( SimpleExpression4Value ) as
                MidEqualsStringOperator );
        }

        /// <summary>
        /// Parsing simple expression 5.
        /// </summary>
        [Test( Description = "Parsing simple expression 5" )]
        public void ParsingSimpleExpression5() {

            CheckSimpleExpression5( CompileExpression( SimpleExpression5Value ) as
                MidEqualsBinaryOperator );
        }

        /// <summary>
        /// Parsing and simple expression.
        /// </summary>
        [Test( Description = "Parsing and simple expressions" )]
        public void ParsingSimpleAndExpression1() {

            object result = CompileExpression( string.Format( "{0} and {1}",
                SimpleExpression1Value, SimpleExpression2Value ) );
            ConditionalAndOperator op = result as ConditionalAndOperator;
            Assert.IsNotNull( op );
            Assert.IsNotNull( op.LeftExpression );
            SemanticParserTest.CheckSimpleExpression1( op.LeftExpression as FieldValueEqualsStringOperator );
            Assert.IsNotNull( op.RightExpression );
            CheckSimpleExpression2( op.RightExpression as FieldValueEqualsBinaryOperator );
        }

        /// <summary>
        /// Parsing and simple expression with parenthesis.
        /// </summary>
        [Test( Description = "Parsing and simple expression with parenthesis" )]
        public void ParsingSimpleAndExpression2() {

            object result = CompileExpression( string.Format( "( ( ( {0} ) ) and ( {1} ) )",
                SimpleExpression2Value, SimpleExpression1Value ) );
            ConditionalAndOperator op = result as ConditionalAndOperator;
            Assert.IsNotNull( op );
            Assert.IsNotNull( op.LeftExpression );
            CheckSimpleExpression2( op.LeftExpression as FieldValueEqualsBinaryOperator );
            Assert.IsNotNull( op.RightExpression );
            SemanticParserTest.CheckSimpleExpression1( op.RightExpression as FieldValueEqualsStringOperator );
        }

        /// <summary>
        /// Parsing or simple expression.
        /// </summary>
        [Test( Description = "Parsing or simple expression." )]
        public void ParsingSimpleOrExpression() {

            object result = CompileExpression( string.Format( "{0} or {1}",
                SimpleExpression3Value, SimpleExpression1Value ) );
            ConditionalOrOperator op = result as ConditionalOrOperator;
            Assert.IsNotNull( op );
            Assert.IsNotNull( op.LeftExpression );
            CheckSimpleExpression3( op.LeftExpression as MtiEqualsExpression );
            Assert.IsNotNull( op.RightExpression );
            SemanticParserTest.CheckSimpleExpression1( op.RightExpression as FieldValueEqualsStringOperator );
        }

        /// <summary>
        /// Or precedence 1.
        /// </summary>
        [Test( Description = "Or precedence 1" )]
        public void OrPrecedence1() {

            object result = CompileExpression( string.Format( "{0} or {1} and {2}",
                SimpleExpression1Value, SimpleExpression2Value, SimpleExpression3Value ) );
            ConditionalOrOperator op = result as ConditionalOrOperator;
            Assert.IsNotNull( op );
            Assert.IsNotNull( op.LeftExpression );
            SemanticParserTest.CheckSimpleExpression1( op.LeftExpression as FieldValueEqualsStringOperator );
            Assert.IsNotNull( op.RightExpression );
            ConditionalAndOperator andOp = op.RightExpression as ConditionalAndOperator;
            Assert.IsNotNull( andOp );
            CheckSimpleExpression2( andOp.LeftExpression as FieldValueEqualsBinaryOperator );
            CheckSimpleExpression3( andOp.RightExpression as MtiEqualsExpression );
        }

        /// <summary>
        /// Or precedence 2.
        /// </summary>
        [Test( Description = "Or precedence 2" )]
        public void OrPrecedence2() {

            object result = CompileExpression( string.Format( "{0} and {1} or {2}",
                SimpleExpression1Value, SimpleExpression2Value, SimpleExpression3Value ) );
            ConditionalOrOperator op = result as ConditionalOrOperator;
            Assert.IsNotNull( op );
            Assert.IsNotNull( op.LeftExpression );
            ConditionalAndOperator andOp = op.LeftExpression as ConditionalAndOperator;
            Assert.IsNotNull( andOp );
            SemanticParserTest.CheckSimpleExpression1( andOp.LeftExpression as FieldValueEqualsStringOperator );
            CheckSimpleExpression2( andOp.RightExpression as FieldValueEqualsBinaryOperator );
            Assert.IsNotNull( op.RightExpression );
            CheckSimpleExpression3( op.RightExpression as MtiEqualsExpression );
        }

        /// <summary>
        /// Or precedence override 1.
        /// </summary>
        [Test( Description = "Or precedence override 1" )]
        public void OrPrecedenceOverride1() {

            object result = CompileExpression( string.Format( "( {0} or {1} ) and {2}",
                SimpleExpression1Value, SimpleExpression2Value, SimpleExpression3Value ) );
            ConditionalAndOperator op = result as ConditionalAndOperator;
            Assert.IsNotNull( op );
            Assert.IsNotNull( op.LeftExpression );
            ConditionalOrOperator orOp = op.LeftExpression as ConditionalOrOperator;
            Assert.IsNotNull( orOp );
            SemanticParserTest.CheckSimpleExpression1( orOp.LeftExpression as FieldValueEqualsStringOperator );
            CheckSimpleExpression2( orOp.RightExpression as FieldValueEqualsBinaryOperator );
            Assert.IsNotNull( op.RightExpression );
            CheckSimpleExpression3( op.RightExpression as MtiEqualsExpression );
        }

        /// <summary>
        /// Or precedence override 2.
        /// </summary>
        [Test( Description = "Or precedence override 2" )]
        public void OrPrecedenceOverride2() {

            object result = CompileExpression( string.Format( "( ( {0} ) and ( {1} or {2} ) )",
                SimpleExpression1Value, SimpleExpression2Value, SimpleExpression3Value ) );
            ConditionalAndOperator op = result as ConditionalAndOperator;
            Assert.IsNotNull( op );
            Assert.IsNotNull( op.LeftExpression );
            SemanticParserTest.CheckSimpleExpression1( op.LeftExpression as FieldValueEqualsStringOperator );
            Assert.IsNotNull( op.RightExpression );
            ConditionalOrOperator orOp = op.RightExpression as ConditionalOrOperator;
            Assert.IsNotNull( orOp );
            CheckSimpleExpression2( orOp.LeftExpression as FieldValueEqualsBinaryOperator );
            CheckSimpleExpression3( orOp.RightExpression as MtiEqualsExpression );
        }

        /// <summary>
        /// Parsing large expression.
        /// </summary>
        [Test( Description = "Parsing large expression" )]
        public void ParsingLargeExpression() {

            object result = CompileExpression( string.Format(
                "( ( {0} or {1} ) and {2} ) or ( ( {3} ) and ( {4} or {5} ) )",
                new object[] { SimpleExpression1Value, SimpleExpression2Value, SimpleExpression3Value,
                    SimpleExpression1Value, SimpleExpression2Value, SimpleExpression3Value } ) );

            ConditionalOrOperator op = result as ConditionalOrOperator;
            Assert.IsNotNull( op );

            ConditionalAndOperator opLeft = op.LeftExpression as ConditionalAndOperator;
            Assert.IsNotNull( opLeft );

            Assert.IsNotNull( opLeft.LeftExpression );
            ConditionalOrOperator opLeftLeft = opLeft.LeftExpression as ConditionalOrOperator;
            Assert.IsNotNull( opLeftLeft );
            SemanticParserTest.CheckSimpleExpression1( opLeftLeft.LeftExpression as FieldValueEqualsStringOperator );
            CheckSimpleExpression2( opLeftLeft.RightExpression as FieldValueEqualsBinaryOperator );
            Assert.IsNotNull( opLeft.RightExpression );
            CheckSimpleExpression3( opLeft.RightExpression as MtiEqualsExpression );

            ConditionalAndOperator opRight = op.RightExpression as ConditionalAndOperator;
            Assert.IsNotNull( opRight );

            Assert.IsNotNull( opRight.LeftExpression );
            SemanticParserTest.CheckSimpleExpression1( opRight.LeftExpression as FieldValueEqualsStringOperator );
            Assert.IsNotNull( opRight.RightExpression );
            ConditionalOrOperator opRightRight = opRight.RightExpression as ConditionalOrOperator;
            Assert.IsNotNull( opRightRight );
            CheckSimpleExpression2( opRightRight.LeftExpression as FieldValueEqualsBinaryOperator );
            CheckSimpleExpression3( opRightRight.RightExpression as MtiEqualsExpression );
        }

        /// <summary>
        /// Parsing simple subfield expression 1.
        /// </summary>
        [Test( Description = "Parsing simple subfield expression 1" )]
        public void ParsingSimpleSubFieldExpression1() {

            CheckSimpleSubFieldExpression1( CompileExpression( SubFieldExpression1Value ) as
                FieldValueEqualsStringOperator );
        }

        /// <summary>
        /// Parsing simple subfield expression 2.
        /// </summary>
        [Test( Description = "Parsing simple subfield expression 2" )]
        public void ParsingSimpleSubFieldExpression2() {

            CheckSimpleSubFieldExpression2( CompileExpression( SubFieldExpression2Value ) as
                FieldValueEqualsBinaryOperator );
        }

        /// <summary>
        /// Parsing simple subfield expression 3.
        /// </summary>
        [Test( Description = "Parsing simple subfield expression 3" )]
        public void ParsingSimpleSubFieldExpression3() {

            CheckSimpleSubFieldExpression3( CompileExpression( SubFieldExpression3Value ) as
                MtiEqualsExpression );
        }

        /// <summary>
        /// Parsing simple subfield expression 4.
        /// </summary>
        [Test( Description = "Parsing simple subfield expression 4" )]
        public void ParsingSimpleSubFieldExpression4() {

            CheckSimpleSubFieldExpression4( CompileExpression( SubFieldExpression4Value ) as
                MidEqualsStringOperator );
        }

        /// <summary>
        /// Parsing simple subfield expression 5.
        /// </summary>
        [Test( Description = "Parsing simple subfield expression 5" )]
        public void ParsingSimpleSubFieldExpression5() {

            CheckSimpleSubFieldExpression5( CompileExpression( SubFieldExpression5Value ) as
                MidEqualsBinaryOperator );
        }

        /// <summary>
        /// Parsing simple parent expression 1.
        /// </summary>
        [Test( Description = "Parsing simple parent expression 1" )]
        public void ParsingSimpleParentExpression1() {

            CheckSimpleParentExpression1( CompileExpression( ParentExpression1Value ) as
                FieldValueEqualsStringOperator );
        }

        /// <summary>
        /// Parsing simple parent expression 2.
        /// </summary>
        [Test( Description = "Parsing simple parent expression 2" )]
        public void ParsingSimpleParentExpression2() {

            CheckSimpleParentExpression2( CompileExpression( ParentExpression2Value ) as
                FieldValueEqualsBinaryOperator );
        }

        /// <summary>
        /// Parsing simple parent expression 3.
        /// </summary>
        [Test( Description = "Parsing simple parent expression 3" )]
        public void ParsingSimpleParentExpression3() {

            CheckSimpleParentExpression3( CompileExpression( ParentExpression3Value ) as
                MtiEqualsExpression );
        }

        /// <summary>
        /// Parsing simple parent expression 4.
        /// </summary>
        [Test( Description = "Parsing simple parent expression 4" )]
        public void ParsingSimpleParentExpression4() {

            CheckSimpleParentExpression4( CompileExpression( ParentExpression4Value ) as
                MidEqualsStringOperator );
        }

        /// <summary>
        /// Parsing simple parent expression 5.
        /// </summary>
        [Test( Description = "Parsing simple parent expression 5" )]
        public void ParsingSimpleParentExpression5() {

            CheckSimpleParentExpression5( CompileExpression( ParentExpression5Value ) as
                MidEqualsBinaryOperator );
        }

        /// <summary>
        /// Parsing simple parent and subfield expression 1.
        /// </summary>
        [Test( Description = "Parsing simple parent and subfield expression 1" )]
        public void ParsingSimpleParentSubFieldExpression1() {

            CheckSimpleParentSubFieldExpression1( CompileExpression( ParentSubFieldExpression1Value ) as
                FieldValueEqualsStringOperator );
        }

        /// <summary>
        /// Parsing simple parent and subfield expression 2.
        /// </summary>
        [Test( Description = "Parsing simple parent and subfield expression 2" )]
        public void ParsingSimpleParentSubFieldExpression2() {

            CheckSimpleParentSubFieldExpression2( CompileExpression( ParentSubFieldExpression2Value ) as
                FieldValueEqualsStringOperator );
        }

        /// <summary>
        /// Parsing simple parent and subfield expression 3.
        /// </summary>
        [Test( Description = "Parsing simple parent and subfield expression 3" )]
        public void ParsingSimpleParentSubFieldExpression3() {

            CheckSimpleParentSubFieldExpression3( CompileExpression( ParentSubFieldExpression3Value ) as
                MtiEqualsExpression );
        }

        /// <summary>
        /// Parsing field equals empty string expression.
        /// </summary>
        [Test( Description = "Parsing field equals empty string expression" )]
        public void ParsingSimpleEmptyStringExpression() {

            CheckSimpleEmptyStringExpression( CompileExpression( SimpleEmptyStringExpressionValue ) as
                FieldValueEqualsStringOperator );
        }

        /// <summary>
        /// Parsing IsSet expression 1.
        /// </summary>
        [Test( Description = "Parsing IsSet expression 1" )]
        public void ParsingIsSetExpression1() {

            CheckIsSetExpression1( CompileExpression( IsSetExpression1Value ) as IsSetExpression );
        }

        /// <summary>
        /// Parsing IsSet expression 2.
        /// </summary>
        [Test( Description = "Parsing IsSet expression 2" )]
        public void ParsingIsSetExpression2() {

            CheckIsSetExpression2( CompileExpression( IsSetExpression2Value ) as IsSetExpression );
        }

        /// <summary>
        /// Parsing IsSet expression 3.
        /// </summary>
        [Test( Description = "Parsing IsSet expression 3" )]
        public void ParsingIsSetExpression3() {

            CheckIsSetExpression3( CompileExpression( IsSetExpression3Value ) as IsSetExpression );
        }

        /// <summary>
        /// Parsing IsSet expression 4.
        /// </summary>
        [Test( Description = "Parsing IsSet expression 4" )]
        public void ParsingIsSetExpression4() {

            CheckIsSetExpression4( CompileExpression( IsSetExpression4Value ) as IsSetExpression );
        }

        /// <summary>
        /// Parsing invalid expressions.
        /// </summary>
        [Test( Description = "Parsing invalid expressions" )]
        public void ParsingInvalidExpressions() {

            Assert.IsTrue( CompileInvalidExpression( "1" ) == 1 );
            Assert.IsTrue( CompileInvalidExpression( "1. " ) == 3 );
            Assert.IsTrue( CompileInvalidExpression( "1 =" ) == 3 );
            Assert.IsTrue( CompileInvalidExpression( "1 = 1" ) == 5 );
            Assert.IsTrue( CompileInvalidExpression( "1[10, = 1" ) == 7 );
            Assert.IsTrue( CompileInvalidExpression( "1[10,2 = 1" ) == 8 );
            Assert.IsTrue( CompileInvalidExpression( "1[10] = 1" ) == 5 );
            Assert.IsTrue( CompileInvalidExpression( "1 and 1" ) == 3 );
            Assert.IsTrue( CompileInvalidExpression( "1 = 'Test' and 1" ) == 16 );
            Assert.IsTrue( CompileInvalidExpression( "(1 = 'Test')) or 1" ) == 13 );
            Assert.IsTrue( CompileInvalidExpression( "1 = H" ) == 5 );
            Assert.IsTrue( CompileInvalidExpression( "1 = ' " ) == 6 );
            Assert.IsTrue( CompileInvalidExpression( "parent.1.parent = 1" ) == 10 );
            Assert.IsTrue( CompileInvalidExpression( "1. = 1" ) == 4 );
            Assert.IsTrue( CompileInvalidExpression( "parent. = 1" ) == 9 );
            Assert.IsTrue( CompileInvalidExpression( "parent.1. = 1" ) == 11 );
            Assert.IsTrue( CompileInvalidExpression( "1 = 'Test' and 1 = ' " ) == 21 );
            Assert.IsTrue( CompileInvalidExpression( ".1 = 1" ) == 1 );
            Assert.IsTrue( CompileInvalidExpression( "parent.1..1 = 'Test'" ) == 10 );
            Assert.IsTrue( CompileInvalidExpression( "(1) = 'Test'" ) == 3 );
            Assert.IsTrue( CompileInvalidExpression( "isset()" ) == 7 );
            Assert.IsTrue( CompileInvalidExpression( "isset ()" ) == 8 );
            Assert.IsTrue( CompileInvalidExpression( "isset ( 6" ) == 9 );
            Assert.IsTrue( CompileInvalidExpression( "isset 3.3)" ) == 7 );
            Assert.IsTrue( CompileInvalidExpression( string.Empty ) == 1 );
        }

        /// <summary>
        /// Evaluating compiled expressions.
        /// </summary>
        [Test( Description = "Evaluating compiled expressions" )]
        public void EvaluateCompiledExpression() {

            Iso8583Message msg = MessagesProvider.GetIso8583Message();
            ParserContext pc = new ParserContext( ParserContext.DefaultBufferSize );
            FormatterContext fc = new FormatterContext( FormatterContext.DefaultBufferSize );
            fc.CurrentMessage = msg;
            pc.CurrentMessage = msg;

            IBooleanExpression ce = CompileExpression( "mti = 200" ) as IBooleanExpression;
            Assert.IsNotNull( ce );
            Assert.IsTrue( ce.EvaluateParse( ref pc ) );
            Assert.IsTrue( ce.EvaluateFormat( new StringField( 3 ), ref fc ) );
            msg.MessageTypeIdentifier = 210;
            Assert.IsFalse( ce.EvaluateParse( ref pc ) );
            Assert.IsFalse( ce.EvaluateFormat( new StringField( 3 ), ref fc ) );

            ce = CompileExpression( "( mti = 210 ) and ( 3 = '999999' )" ) as IBooleanExpression;
            Assert.IsNotNull( ce );
            Assert.IsTrue( ce.EvaluateParse( ref pc ) );
            Assert.IsTrue( ce.EvaluateFormat( new StringField( 3 ), ref fc ) );
            msg[3].Value = "000000";
            Assert.IsFalse( ce.EvaluateParse( ref pc ) );
            Assert.IsFalse( ce.EvaluateFormat( new StringField( 3 ), ref fc ) );

            ce = CompileExpression( "( mti = 210 ) and ( 3 = '999999' ) or 11 = '000001'" )
                as IBooleanExpression;
            Assert.IsNotNull( ce );
            Assert.IsTrue( ce.EvaluateParse( ref pc ) );
            Assert.IsTrue( ce.EvaluateFormat( new StringField( 3 ), ref fc ) );
            msg[11].Value = "000002";
            Assert.IsFalse( ce.EvaluateParse( ref pc ) );
            Assert.IsFalse( ce.EvaluateFormat( new StringField( 3 ), ref fc ) );
            msg[3].Value = "999999";
            Assert.IsTrue( ce.EvaluateParse( ref pc ) );
            Assert.IsTrue( ce.EvaluateFormat( new StringField( 3 ), ref fc ) );

            ce = CompileExpression( "(mti = 210) and 61.mti = 800" ) as IBooleanExpression;
            Assert.IsNotNull( ce );
            Assert.IsTrue( ce.EvaluateParse( ref pc ) );
            Assert.IsTrue( ce.EvaluateFormat( new StringField( 3 ), ref fc ) );
            ( msg[61].Value as Iso8583Message ).MessageTypeIdentifier = 810;
            Assert.IsFalse( ce.EvaluateParse( ref pc ) );
            Assert.IsFalse( ce.EvaluateFormat( new StringField( 3 ), ref fc ) );
            ( msg[61].Value as Iso8583Message ).MessageTypeIdentifier = 800;
            Assert.IsTrue( ce.EvaluateParse( ref pc ) );
            Assert.IsTrue( ce.EvaluateFormat( new StringField( 3 ), ref fc ) );
            msg.MessageTypeIdentifier = 200;
            Assert.IsFalse( ce.EvaluateParse( ref pc ) );
            Assert.IsFalse( ce.EvaluateFormat( new StringField( 3 ), ref fc ) );
        }

        private object CompileExpression( string expression ) {

            object result = null;

            using ( StringReader sr = new StringReader( expression ) ) {

                SemanticParser sp = new SemanticParser();
                Tokenizer tokenizer = new Tokenizer( sr );

                try {
                    result = sp.yyparse( tokenizer );
                }
                catch ( Exception ex ) {
                    Assert.Fail( ex.Message );
                }

                Assert.IsNotNull( result );
            }

            return result;
        }

        private int CompileInvalidExpression( string expression ) {

            using ( StringReader sr = new StringReader( expression ) ) {

                SemanticParser sp = new SemanticParser();
                Tokenizer tokenizer = new Tokenizer( sr );

                try {
                    sp.yyparse( tokenizer );
                }
                catch ( ExpressionCompileException ecex ) {
                    return ecex.LastParsedTokenIndex;
                }
                catch ( Exception ex ) {
                    Assert.Fail( ex.Message );
                }
            }

            Assert.Fail();

            return -1;
        }

        internal static void CheckSimpleExpression1( FieldValueEqualsStringOperator op ) {

            Assert.IsNotNull( op );
            Assert.IsNotNull( op.MessageExpression );
            Assert.IsNotNull( op.ValueExpression );

            if ( op.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }

            MessageExpression me = op.MessageExpression as MessageExpression;

            Assert.IsTrue( me.FieldNumber == 3 );

            if ( op.ValueExpression.GetType() != typeof( StringConstantExpression ) ) {
                Assert.Fail( "StringConstantExpression was expected." );
            }

            StringConstantExpression sce = op.ValueExpression as StringConstantExpression;

            Assert.IsTrue( sce.Constant == "000000" );
        }

        private void CheckSimpleExpression2( FieldValueEqualsBinaryOperator op ) {

            Assert.IsNotNull( op );
            Assert.IsNotNull( op.MessageExpression );
            Assert.IsNotNull( op.ValueExpression );

            if ( op.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }

            MessageExpression me = op.MessageExpression as MessageExpression;

            Assert.IsTrue( me.FieldNumber == 4 );

            if ( op.ValueExpression.GetType() != typeof( BinaryConstantExpression ) ) {
                Assert.Fail( "BinaryConstantExpression was expected." );
            }

            BinaryConstantExpression bce = op.ValueExpression as BinaryConstantExpression;

            Assert.IsTrue( bce.Constant == "303132333435" );

            byte[] data = new byte[] { 0x30, 0x31, 0x32, 0x33, 0x34, 0x35 };
            byte[] constVal = bce.GetValue();

            Assert.IsTrue( data.Length == constVal.Length );
            for ( int i = data.Length - 1; i >= 0; i-- ) {
                Assert.IsTrue( data[i] == constVal[i] );
            }
        }

        private void CheckSimpleExpression3( MtiEqualsExpression op ) {

            Assert.IsNotNull( op );
            Assert.IsTrue( op.Mti == 800 );
            Assert.IsNotNull( op.MessageExpression as MessageExpression );
        }

        internal void CheckSimpleExpression4( MidEqualsStringOperator op ) {

            Assert.IsNotNull( op );
            Assert.IsNotNull( op.MessageExpression );
            Assert.IsNotNull( op.ValueExpression );
            Assert.IsNotNull( op.StartIndex == 0 );
            Assert.IsNotNull( op.Length == 2 );

            if ( op.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }

            MessageExpression me = op.MessageExpression as MessageExpression;

            Assert.IsTrue( me.FieldNumber == 3 );

            if ( op.ValueExpression.GetType() != typeof( StringConstantExpression ) ) {
                Assert.Fail( "StringConstantExpression was expected." );
            }

            StringConstantExpression sce = op.ValueExpression as StringConstantExpression;

            Assert.IsTrue( sce.Constant == "00" );
        }

        private void CheckSimpleExpression5( MidEqualsBinaryOperator op ) {

            Assert.IsNotNull( op );
            Assert.IsNotNull( op.MessageExpression );
            Assert.IsNotNull( op.ValueExpression );
            Assert.IsNotNull( op.StartIndex == 10 );
            Assert.IsNotNull( op.Length == 2 );

            if ( op.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }

            MessageExpression me = op.MessageExpression as MessageExpression;

            Assert.IsTrue( me.FieldNumber == 4 );

            if ( op.ValueExpression.GetType() != typeof( BinaryConstantExpression ) ) {
                Assert.Fail( "BinaryConstantExpression was expected." );
            }

            BinaryConstantExpression bce = op.ValueExpression as BinaryConstantExpression;

            Assert.IsTrue( bce.Constant == "3030" );

            byte[] data = new byte[] { 0x30, 0x30 };
            byte[] constVal = bce.GetValue();

            Assert.IsTrue( data.Length == constVal.Length );
            for ( int i = data.Length - 1; i >= 0; i-- ) {
                Assert.IsTrue( data[i] == constVal[i] );
            }
        }

        private void CheckSimpleSubFieldExpression1( FieldValueEqualsStringOperator op ) {

            Assert.IsNotNull( op );
            Assert.IsNotNull( op.MessageExpression );
            Assert.IsNotNull( op.ValueExpression );

            if ( op.MessageExpression.GetType() != typeof( SubMessageExpression ) ) {
                Assert.Fail( "SubMessageExpression was expected." );
            }

            SubMessageExpression sme = op.MessageExpression as SubMessageExpression;

            Assert.IsNotNull( sme.MessageExpression );
            Assert.IsTrue( sme.FieldNumber == 48 );

            if ( sme.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }

            MessageExpression me = sme.MessageExpression as MessageExpression;

            Assert.IsTrue( me.FieldNumber == 5 );

            if ( op.ValueExpression.GetType() != typeof( StringConstantExpression ) ) {
                Assert.Fail( "StringConstantExpression was expected." );
            }

            StringConstantExpression sce = op.ValueExpression as StringConstantExpression;

            Assert.IsTrue( sce.Constant == "123" );
        }

        private void CheckSimpleSubFieldExpression2( FieldValueEqualsBinaryOperator op ) {

            Assert.IsNotNull( op );
            Assert.IsNotNull( op.MessageExpression );
            Assert.IsNotNull( op.ValueExpression );

            if ( op.MessageExpression.GetType() != typeof( SubMessageExpression ) ) {
                Assert.Fail( "SubMessageExpression was expected." );
            }

            SubMessageExpression sme1 = op.MessageExpression as SubMessageExpression;

            Assert.IsNotNull( sme1.MessageExpression );
            Assert.IsTrue( sme1.FieldNumber == 63 );

            if ( sme1.MessageExpression.GetType() != typeof( SubMessageExpression ) ) {
                Assert.Fail( "SubMessageExpression was expected." );
            }

            SubMessageExpression sme2 = sme1.MessageExpression as SubMessageExpression;

            Assert.IsNotNull( sme2.MessageExpression );
            Assert.IsTrue( sme2.FieldNumber == 4 );

            if ( sme2.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }

            MessageExpression me = sme2.MessageExpression as MessageExpression;

            Assert.IsTrue( me.FieldNumber == 7 );

            if ( op.ValueExpression.GetType() != typeof( BinaryConstantExpression ) ) {
                Assert.Fail( "BinaryConstantExpression was expected." );
            }

            BinaryConstantExpression bce = op.ValueExpression as BinaryConstantExpression;

            Assert.IsTrue( bce.Constant == "2020" );

            byte[] data = new byte[] { 0x20, 0x20 };
            byte[] constVal = bce.GetValue();

            Assert.IsTrue( data.Length == constVal.Length );
            for ( int i = data.Length - 1; i >= 0; i-- ) {
                Assert.IsTrue( data[i] == constVal[i] );
            }
        }

        private void CheckSimpleSubFieldExpression3( MtiEqualsExpression ee ) {

            Assert.IsNotNull( ee );
            Assert.IsNotNull( ee.MessageExpression );
            Assert.IsTrue( ee.Mti == 800 );

            if ( ee.MessageExpression.GetType() != typeof( SubMessageExpression ) ) {
                Assert.Fail( "SubMessageExpression was expected." );
            }

            SubMessageExpression sme = ee.MessageExpression as SubMessageExpression;

            Assert.IsNotNull( sme.MessageExpression );
            Assert.IsTrue( sme.FieldNumber == 48 );

            if ( sme.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }
        }

        private void CheckSimpleSubFieldExpression4( MidEqualsStringOperator op ) {

            Assert.IsNotNull( op );
            Assert.IsNotNull( op.MessageExpression );
            Assert.IsNotNull( op.ValueExpression );
            Assert.IsNotNull( op.StartIndex == 1 );
            Assert.IsNotNull( op.StartIndex == 2 );

            if ( op.MessageExpression.GetType() != typeof( SubMessageExpression ) ) {
                Assert.Fail( "SubMessageExpression was expected." );
            }

            SubMessageExpression sme = op.MessageExpression as SubMessageExpression;

            Assert.IsNotNull( sme.MessageExpression );
            Assert.IsTrue( sme.FieldNumber == 48 );

            if ( sme.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }

            MessageExpression me = sme.MessageExpression as MessageExpression;

            Assert.IsTrue( me.FieldNumber == 5 );

            if ( op.ValueExpression.GetType() != typeof( StringConstantExpression ) ) {
                Assert.Fail( "StringConstantExpression was expected." );
            }

            StringConstantExpression sce = op.ValueExpression as StringConstantExpression;

            Assert.IsTrue( sce.Constant == "23" );
        }

        private void CheckSimpleSubFieldExpression5( MidEqualsBinaryOperator op ) {

            Assert.IsNotNull( op );
            Assert.IsNotNull( op.MessageExpression );
            Assert.IsNotNull( op.ValueExpression );
            Assert.IsNotNull( op.StartIndex == 5 );
            Assert.IsNotNull( op.StartIndex == 3 );

            if ( op.MessageExpression.GetType() != typeof( SubMessageExpression ) ) {
                Assert.Fail( "SubMessageExpression was expected." );
            }

            SubMessageExpression sme1 = op.MessageExpression as SubMessageExpression;

            Assert.IsNotNull( sme1.MessageExpression );
            Assert.IsTrue( sme1.FieldNumber == 63 );

            if ( sme1.MessageExpression.GetType() != typeof( SubMessageExpression ) ) {
                Assert.Fail( "SubMessageExpression was expected." );
            }

            SubMessageExpression sme2 = sme1.MessageExpression as SubMessageExpression;

            Assert.IsNotNull( sme2.MessageExpression );
            Assert.IsTrue( sme2.FieldNumber == 4 );

            if ( sme2.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }

            MessageExpression me = sme2.MessageExpression as MessageExpression;

            Assert.IsTrue( me.FieldNumber == 7 );

            if ( op.ValueExpression.GetType() != typeof( BinaryConstantExpression ) ) {
                Assert.Fail( "BinaryConstantExpression was expected." );
            }

            BinaryConstantExpression bce = op.ValueExpression as BinaryConstantExpression;

            Assert.IsTrue( bce.Constant == "202020" );

            byte[] data = new byte[] { 0x20, 0x20, 0x20 };
            byte[] constVal = bce.GetValue();

            Assert.IsTrue( data.Length == constVal.Length );
            for ( int i = data.Length - 1; i >= 0; i-- ) {
                Assert.IsTrue( data[i] == constVal[i] );
            }
        }

        private void CheckSimpleParentExpression1( FieldValueEqualsStringOperator op ) {

            Assert.IsNotNull( op );
            Assert.IsNotNull( op.MessageExpression );
            Assert.IsNotNull( op.ValueExpression );

            if ( op.MessageExpression.GetType() != typeof( ParentMessageExpression ) ) {
                Assert.Fail( "ParentMessageExpression was expected." );
            }

            ParentMessageExpression pme = op.MessageExpression as ParentMessageExpression;

            Assert.IsNotNull( pme.MessageExpression );

            if ( pme.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }

            MessageExpression me = pme.MessageExpression as MessageExpression;

            Assert.IsTrue( me.FieldNumber == 4 );

            if ( op.ValueExpression.GetType() != typeof( StringConstantExpression ) ) {
                Assert.Fail( "StringConstantExpression was expected." );
            }

            StringConstantExpression sce = op.ValueExpression as StringConstantExpression;

            Assert.IsTrue( sce.Constant == "000000072000" );
        }

        private void CheckSimpleParentExpression2( FieldValueEqualsBinaryOperator op ) {

            Assert.IsNotNull( op );
            Assert.IsNotNull( op.MessageExpression );
            Assert.IsNotNull( op.ValueExpression );

            if ( op.MessageExpression.GetType() != typeof( ParentMessageExpression ) ) {
                Assert.Fail( "ParentMessageExpression was expected." );
            }

            ParentMessageExpression pme1 = op.MessageExpression as ParentMessageExpression;

            Assert.IsNotNull( pme1.MessageExpression );

            if ( pme1.MessageExpression.GetType() != typeof( ParentMessageExpression ) ) {
                Assert.Fail( "ParentMessageExpression was expected." );
            }

            ParentMessageExpression pme2 = pme1.MessageExpression as ParentMessageExpression;

            Assert.IsNotNull( pme2.MessageExpression );

            if ( pme2.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }

            MessageExpression me = pme2.MessageExpression as MessageExpression;

            Assert.IsTrue( me.FieldNumber == 49 );

            if ( op.ValueExpression.GetType() != typeof( BinaryConstantExpression ) ) {
                Assert.Fail( "BinaryConstantExpression was expected." );
            }

            BinaryConstantExpression bce = op.ValueExpression as BinaryConstantExpression;

            Assert.IsTrue( bce.Constant == "383538" );

            byte[] data = new byte[] { 0x38, 0x35, 0x38 };
            byte[] constVal = bce.GetValue();

            Assert.IsTrue( data.Length == constVal.Length );
            for ( int i = data.Length - 1; i >= 0; i-- ) {
                Assert.IsTrue( data[i] == constVal[i] );
            }
        }

        private void CheckSimpleParentExpression3( MtiEqualsExpression ee ) {

            Assert.IsNotNull( ee );
            Assert.IsNotNull( ee.MessageExpression );
            Assert.IsTrue( ee.Mti == 200 );

            if ( ee.MessageExpression.GetType() != typeof( ParentMessageExpression ) ) {
                Assert.Fail( "ParentMessageExpression was expected." );
            }

            ParentMessageExpression pme = ee.MessageExpression as ParentMessageExpression;

            Assert.IsNotNull( pme.MessageExpression );

            if ( pme.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }
        }

        private void CheckSimpleParentExpression4( MidEqualsStringOperator op ) {

            Assert.IsNotNull( op );
            Assert.IsNotNull( op.MessageExpression );
            Assert.IsNotNull( op.ValueExpression );
            Assert.IsNotNull( op.StartIndex == 10 );
            Assert.IsNotNull( op.Length == 2 );

            if ( op.MessageExpression.GetType() != typeof( ParentMessageExpression ) ) {
                Assert.Fail( "ParentMessageExpression was expected." );
            }

            ParentMessageExpression pme = op.MessageExpression as ParentMessageExpression;

            Assert.IsNotNull( pme.MessageExpression );

            if ( pme.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }

            MessageExpression me = pme.MessageExpression as MessageExpression;

            Assert.IsTrue( me.FieldNumber == 4 );

            if ( op.ValueExpression.GetType() != typeof( StringConstantExpression ) ) {
                Assert.Fail( "StringConstantExpression was expected." );
            }

            StringConstantExpression sce = op.ValueExpression as StringConstantExpression;

            Assert.IsTrue( sce.Constant == "50" );
        }

        private void CheckSimpleParentExpression5( MidEqualsBinaryOperator op ) {

            Assert.IsNotNull( op );
            Assert.IsNotNull( op.MessageExpression );
            Assert.IsNotNull( op.ValueExpression );
            Assert.IsNotNull( op.StartIndex == 0 );
            Assert.IsNotNull( op.Length == 1 );

            if ( op.MessageExpression.GetType() != typeof( ParentMessageExpression ) ) {
                Assert.Fail( "ParentMessageExpression was expected." );
            }

            ParentMessageExpression pme1 = op.MessageExpression as ParentMessageExpression;

            Assert.IsNotNull( pme1.MessageExpression );

            if ( pme1.MessageExpression.GetType() != typeof( ParentMessageExpression ) ) {
                Assert.Fail( "ParentMessageExpression was expected." );
            }

            ParentMessageExpression pme2 = pme1.MessageExpression as ParentMessageExpression;

            Assert.IsNotNull( pme2.MessageExpression );

            if ( pme2.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }

            MessageExpression me = pme2.MessageExpression as MessageExpression;

            Assert.IsTrue( me.FieldNumber == 49 );

            if ( op.ValueExpression.GetType() != typeof( BinaryConstantExpression ) ) {
                Assert.Fail( "BinaryConstantExpression was expected." );
            }

            BinaryConstantExpression bce = op.ValueExpression as BinaryConstantExpression;

            Assert.IsTrue( bce.Constant == "38" );

            byte[] data = new byte[] { 0x38 };
            byte[] constVal = bce.GetValue();

            Assert.IsTrue( data.Length == constVal.Length );
            for ( int i = data.Length - 1; i >= 0; i-- ) {
                Assert.IsTrue( data[i] == constVal[i] );
            }
        }

        private void CheckSimpleParentSubFieldExpression1( FieldValueEqualsStringOperator op ) {

            Assert.IsNotNull( op );
            Assert.IsNotNull( op.MessageExpression );
            Assert.IsNotNull( op.ValueExpression );

            if ( op.MessageExpression.GetType() != typeof( ParentMessageExpression ) ) {
                Assert.Fail( "ParentMessageExpression was expected." );
            }

            ParentMessageExpression pme = op.MessageExpression as ParentMessageExpression;

            Assert.IsNotNull( pme.MessageExpression );

            SubMessageExpression sme = pme.MessageExpression as SubMessageExpression;

            Assert.IsNotNull( sme.MessageExpression );
            Assert.IsTrue( sme.FieldNumber == 28 );

            if ( sme.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }

            if ( sme.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }

            MessageExpression me = sme.MessageExpression as MessageExpression;

            Assert.IsTrue( me.FieldNumber == 5 );

            if ( op.ValueExpression.GetType() != typeof( StringConstantExpression ) ) {
                Assert.Fail( "StringConstantExpression was expected." );
            }

            StringConstantExpression sce = op.ValueExpression as StringConstantExpression;

            Assert.IsTrue( sce.Constant == "ABC" );
        }

        private void CheckSimpleParentSubFieldExpression2( FieldValueEqualsStringOperator op ) {

            Assert.IsNotNull( op );
            Assert.IsNotNull( op.MessageExpression );
            Assert.IsNotNull( op.ValueExpression );

            if ( op.MessageExpression.GetType() != typeof( ParentMessageExpression ) ) {
                Assert.Fail( "ParentMessageExpression was expected." );
            }

            ParentMessageExpression pme1 = op.MessageExpression as ParentMessageExpression;

            Assert.IsNotNull( pme1.MessageExpression );

            if ( pme1.MessageExpression.GetType() != typeof( ParentMessageExpression ) ) {
                Assert.Fail( "ParentMessageExpression was expected." );
            }

            ParentMessageExpression pme2 = pme1.MessageExpression as ParentMessageExpression;

            Assert.IsNotNull( pme2.MessageExpression );

            if ( pme2.MessageExpression.GetType() != typeof( SubMessageExpression ) ) {
                Assert.Fail( "SubMessageExpression was expected." );
            }

            if ( pme2.MessageExpression.GetType() != typeof( SubMessageExpression ) ) {
                Assert.Fail( "SubMessageExpression was expected." );
            }

            SubMessageExpression sme1 = pme2.MessageExpression as SubMessageExpression;

            Assert.IsNotNull( sme1.MessageExpression );
            Assert.IsTrue( sme1.FieldNumber == 62 );

            if ( sme1.MessageExpression.GetType() != typeof( SubMessageExpression ) ) {
                Assert.Fail( "SubMessageExpression was expected." );
            }

            SubMessageExpression sme2 = sme1.MessageExpression as SubMessageExpression;

            Assert.IsNotNull( sme2.MessageExpression );
            Assert.IsTrue( sme2.FieldNumber == 3 );

            if ( sme2.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }

            MessageExpression me = sme2.MessageExpression as MessageExpression;

            Assert.IsTrue( me.FieldNumber == 1 );

            if ( op.ValueExpression.GetType() != typeof( StringConstantExpression ) ) {
                Assert.Fail( "StringConstantExpression was expected." );
            }

            StringConstantExpression sce = op.ValueExpression as StringConstantExpression;

            Assert.IsTrue( sce.Constant == " " );
        }

        private void CheckSimpleParentSubFieldExpression3( MtiEqualsExpression ee ) {

            Assert.IsNotNull( ee );
            Assert.IsNotNull( ee.MessageExpression );
            Assert.IsTrue( ee.Mti == 800 );

            if ( ee.MessageExpression.GetType() != typeof( ParentMessageExpression ) ) {
                Assert.Fail( "ParentMessageExpression was expected." );
            }

            ParentMessageExpression pme = ee.MessageExpression as ParentMessageExpression;

            Assert.IsNotNull( pme.MessageExpression );

            if ( pme.MessageExpression.GetType() != typeof( ParentMessageExpression ) ) {
                Assert.Fail( "ParentMessageExpression was expected." );
            }

            pme = pme.MessageExpression as ParentMessageExpression;

            if ( pme.MessageExpression.GetType() != typeof( SubMessageExpression ) ) {
                Assert.Fail( "SubMessageExpression was expected." );
            }

            SubMessageExpression sme = pme.MessageExpression as SubMessageExpression;

            Assert.IsNotNull( sme.MessageExpression );
            Assert.IsTrue( sme.FieldNumber == 4 );

            if ( sme.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }
        }

        private void CheckSimpleEmptyStringExpression( FieldValueEqualsStringOperator op ) {

            Assert.IsNotNull( op );
            Assert.IsNotNull( op.MessageExpression );
            Assert.IsNotNull( op.ValueExpression );

            if ( op.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }

            MessageExpression me = op.MessageExpression as MessageExpression;

            Assert.IsTrue( me.FieldNumber == 6 );

            if ( op.ValueExpression.GetType() != typeof( StringConstantExpression ) ) {
                Assert.Fail( "StringConstantExpression was expected." );
            }

            StringConstantExpression sce = op.ValueExpression as StringConstantExpression;

            Assert.IsTrue( sce.Constant == string.Empty );
        }

        private void CheckIsSetExpression1( IsSetExpression ise ) {

            Assert.IsNotNull( ise );
            Assert.IsTrue( ise.MessageExpression.GetLeafFieldNumber() == 3 );
            Assert.IsNotNull( ise.MessageExpression as MessageExpression );
        }

        private void CheckIsSetExpression2( IsSetExpression ise ) {

            Assert.IsNotNull( ise );
            Assert.IsNotNull( ise.MessageExpression );
            Assert.IsTrue( ise.MessageExpression.GetLeafFieldNumber() == 3 );

            if ( ise.MessageExpression.GetType() != typeof( SubMessageExpression ) ) {
                Assert.Fail( "SubMessageExpression was expected." );
            }

            SubMessageExpression sme = ise.MessageExpression as SubMessageExpression;

            Assert.IsNotNull( sme.MessageExpression );
            Assert.IsTrue( sme.FieldNumber == 1 );

            if ( sme.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }
        }

        private void CheckIsSetExpression3( IsSetExpression ise ) {

            Assert.IsNotNull( ise );
            Assert.IsNotNull( ise.MessageExpression );
            Assert.IsTrue( ise.MessageExpression.GetLeafFieldNumber() == 3 );

            if ( ise.MessageExpression.GetType() != typeof( ParentMessageExpression ) ) {
                Assert.Fail( "ParentMessageExpression was expected." );
            }

            ParentMessageExpression pme = ise.MessageExpression as ParentMessageExpression;

            Assert.IsNotNull( pme.MessageExpression );

            if ( pme.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }
        }

        private void CheckIsSetExpression4( IsSetExpression ise ) {

            Assert.IsNotNull( ise );
            Assert.IsNotNull( ise.MessageExpression );
            Assert.IsTrue( ise.MessageExpression.GetLeafFieldNumber() == 5 );

            if ( ise.MessageExpression.GetType() != typeof( ParentMessageExpression ) ) {
                Assert.Fail( "ParentMessageExpression was expected." );
            }

            ParentMessageExpression pme = ise.MessageExpression as ParentMessageExpression;

            Assert.IsNotNull( pme.MessageExpression );

            if ( pme.MessageExpression.GetType() != typeof( ParentMessageExpression ) ) {
                Assert.Fail( "ParentMessageExpression was expected." );
            }

            pme = pme.MessageExpression as ParentMessageExpression;

            if ( pme.MessageExpression.GetType() != typeof( SubMessageExpression ) ) {
                Assert.Fail( "SubMessageExpression was expected." );
            }

            SubMessageExpression sme = pme.MessageExpression as SubMessageExpression;

            Assert.IsNotNull( sme.MessageExpression );
            Assert.IsTrue( sme.FieldNumber == 4 );

            if ( sme.MessageExpression.GetType() != typeof( MessageExpression ) ) {
                Assert.Fail( "MessageExpression was expected." );
            }
        }
        #endregion
	}
}