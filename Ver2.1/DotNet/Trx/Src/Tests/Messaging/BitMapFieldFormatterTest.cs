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
using NUnit.Framework;

namespace Trx.Tests.Messaging {

	/// <summary>
	/// Test fixture for BitMapFieldFormatter.
	/// </summary>
	[TestFixture( Description="BitMap field formatter tests.")]
	public class BitMapFieldFormatterTest {

		#region Constructors
		/// <summary>
		/// It builds and initializes a new instance of the class
		/// <see cref="BitMapFieldFormatterTest"/>.
		/// </summary>
		public BitMapFieldFormatterTest() {

		}
		#endregion

		#region Methods
		/// <summary>
		/// This method will be called by NUnit for test setup.
		/// </summary>
		[SetUp]
		public void SetUp() {

		}

		/// <summary>
		/// Test constructors.
		/// </summary>
		[Test( Description="Test constructors.")]
		public void Constructors() {

			BitMapFieldFormatter bitmapFormatter = new BitMapFieldFormatter(
				0, 1, 64, BinaryEncoder.GetInstance());

			Assert.IsTrue( bitmapFormatter.FieldNumber == 0);
			Assert.IsTrue( bitmapFormatter.LowerFieldNumber == 1);
			Assert.IsTrue( bitmapFormatter.UpperFieldNumber == 64);
			Assert.IsTrue( bitmapFormatter.Encoder == BinaryEncoder.GetInstance());
			Assert.IsTrue( bitmapFormatter.Description == string.Empty);

			bitmapFormatter = new BitMapFieldFormatter(
				1, 65, 128, BinaryEncoder.GetInstance(), "Second bitmap");

			Assert.IsTrue( bitmapFormatter.FieldNumber == 1);
			Assert.IsTrue( bitmapFormatter.LowerFieldNumber == 65);
			Assert.IsTrue( bitmapFormatter.UpperFieldNumber == 128);
			Assert.IsTrue( bitmapFormatter.Encoder == BinaryEncoder.GetInstance());
			Assert.IsTrue( bitmapFormatter.Description.Equals( "Second bitmap"));

			try {
				bitmapFormatter = new BitMapFieldFormatter( 0, 1, 64, null);
				Assert.Fail();
			} catch ( ArgumentNullException e) {
				Assert.IsTrue( e.ParamName.Equals( "encoder"));
			}

			try {
				bitmapFormatter = new BitMapFieldFormatter( 0, -1, 64,
					BinaryEncoder.GetInstance());
				Assert.Fail();
			} catch ( ArgumentOutOfRangeException e) {
				Assert.IsTrue( e.ParamName.Equals( "lowerFieldNumber"));
			}

			try {
				bitmapFormatter = new BitMapFieldFormatter( 0, 65, 64,
					BinaryEncoder.GetInstance());
				Assert.Fail();
			} catch ( ArgumentOutOfRangeException e) {
				Assert.IsTrue( e.ParamName.Equals( "lowerFieldNumber"));
			}
		}

		/// <summary>
		/// Test Format method.
		/// </summary>
		[Test( Description="Test Format method.")]
		public void Format() {

			FormatterContext formatterContext = new FormatterContext(
				FormatterContext.DefaultBufferSize);
			BitMapField bitmap = new BitMapField( 0, 1, 64);
			BitMapFieldFormatter bitmapFormatter = new BitMapFieldFormatter(
				0, 1, 64, HexadecimalBinaryEncoder.GetInstance());

			bitmapFormatter.Format( bitmap, ref formatterContext);
			Assert.IsTrue( formatterContext.GetDataAsString().Equals(
				"0000000000000000"));
			formatterContext.Clear();

			int[] fields = { 4, 7, 13, 19, 27, 36, 41, 42, 45, 47, 52, 56, 57, 63, 64};
			for ( int i = 0; i < fields.Length; i++) {
				bitmap.Set( fields[i], true);
			}
			bitmapFormatter.Format( bitmap, ref formatterContext);
			Assert.IsTrue( formatterContext.GetDataAsString().Equals(
				"1208202010CA1183"));
		}

		/// <summary>
		/// Test Parse method.
		/// </summary>
		[Test( Description="Test Parse method.")]
		public void Parse() {

			ParserContext parserContext = new ParserContext(
				ParserContext.DefaultBufferSize);
			BitMapFieldFormatter bitmapFormatter = new BitMapFieldFormatter(
				0, 1, 64, HexadecimalBinaryEncoder.GetInstance());

			BitMapField bitmap = ( BitMapField)bitmapFormatter.Parse(
				ref parserContext);
			Assert.IsNull( bitmap);
			parserContext.Write( "12082020");
			bitmap = ( BitMapField)bitmapFormatter.Parse( ref parserContext);
			Assert.IsTrue( parserContext.DataLength == 8);
			Assert.IsNull( bitmap);
			parserContext.Write( "10CA118");
			bitmap = ( BitMapField)bitmapFormatter.Parse( ref parserContext);
			Assert.IsTrue( parserContext.DataLength == 15);
			Assert.IsNull( bitmap);
			parserContext.Write( "3");
			bitmap = ( BitMapField)bitmapFormatter.Parse( ref parserContext);
			Assert.IsTrue( parserContext.DataLength == 0);
			Assert.IsNotNull( bitmap);

			byte[] referenceBitmap = { 0x12, 0x08, 0x20, 0x20, 0x10, 0xCA, 0x11, 0x83};
			byte[] bitmapValue = bitmap.GetBytes();

			for ( int i = 0; i < bitmapValue.Length; i++) {
				Assert.IsTrue( bitmapValue[i] == referenceBitmap[i]);
			}
		}
		#endregion
	}
}
