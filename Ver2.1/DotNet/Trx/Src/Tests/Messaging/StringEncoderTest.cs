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
using System.Text;

using Trx.Messaging;
using Trx.Utilities;
using NUnit.Framework;

namespace Trx.Tests.Messaging {

	/// <summary>
	/// Test fixture for StringEncoder.
	/// </summary>
	[TestFixture( Description="String encoder tests.")]
	public class StringEncoderTest {

		private StringEncoder _encoder;
		private string _data;
		private byte[] _binaryData;

		#region Constructors
		/// <summary>
		/// It builds and initializes a new instance of the class
		/// <see cref="StringEncoderTest"/>.
		/// </summary>
		public StringEncoderTest() {

		}
		#endregion

		#region Methods
		/// <summary>
		/// This method will be called by NUnit for test setup.
		/// </summary>
		[SetUp]
		public void SetUp() {

			_encoder = StringEncoder.GetInstance();
			Assert.IsNotNull( _encoder);
			_data  = "Sample data";
			_binaryData = Encoding.UTF7.GetBytes( _data);
		}

		/// <summary>
		/// Test GetInstance method.
		/// </summary>
		[Test( Description="Test GetInstance method.")]
		public void GetInstance() {

			Assert.IsTrue( _encoder == StringEncoder.GetInstance());
		}

		/// <summary>
		/// Test GetEncodedLength method.
		/// </summary>
		[Test( Description="Test GetEncodedLength method.")]
		public void GetEncodedLength() {

			Assert.IsTrue( _encoder.GetEncodedLength( 0) == 0);
			Assert.IsTrue( _encoder.GetEncodedLength( 5) == 5);
			Assert.IsFalse( _encoder.GetEncodedLength( 3) == 7);
		}

		/// <summary>
		/// Test Encode method.
		/// </summary>
		[Test( Description="Test Encode method.")]
		public void Encode() {

			FormatterContext formatterContext =
				new FormatterContext( FormatterContext.DefaultBufferSize);

			_encoder.Encode( _data, ref formatterContext);

			Assert.IsTrue( formatterContext.DataLength == _data.Length);

			byte[] encodedData = formatterContext.GetData();

			for ( int i = _binaryData.Length - 1; i >= 0; i--) {
				Assert.IsTrue( _binaryData[i] == encodedData[i]);
			}
		}
		
		/// <summary>
		/// Test Decode method.
		/// </summary>
		[Test( Description="Test Decode method.")]
		public void Decode() {

			ParserContext parserContext =
				new ParserContext( ParserContext.DefaultBufferSize);

			parserContext.Write( _binaryData);

			string decodedData = _encoder.Decode( ref parserContext,
				parserContext.DataLength);

			Assert.IsTrue( !StringUtilities.IsNullOrEmpty( decodedData));
			Assert.IsTrue( _data.Equals( decodedData));
		}
		#endregion
	}
}
