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
	/// Test fixture for BinaryEncoder.
	/// </summary>
	[TestFixture( Description="Binary encoder tests.")]
	public class BinaryEncoderTest {

		private BinaryEncoder _encoder;
		private byte[] _data;

		#region Constructors
		/// <summary>
		/// It builds and initializes a new instance of the class
		/// <see cref="BinaryEncoderTest"/>.
		/// </summary>
		public BinaryEncoderTest() {

		}
		#endregion

		#region Methods
		/// <summary>
		/// This method will be called by NUnit for test setup.
		/// </summary>
		[SetUp]
		public void SetUp() {

			_encoder = BinaryEncoder.GetInstance();
			_data = Encoding.UTF7.GetBytes( "Sample data");
			Assert.IsNotNull( _encoder);
		}

		/// <summary>
		/// Test GetInstance method.
		/// </summary>
		[Test( Description="Test GetInstance method.")]
		public void GetInstance() {

			Assert.IsTrue( _encoder == BinaryEncoder.GetInstance());
		}

		/// <summary>
		/// Test GetEncodedLength method.
		/// </summary>
		[Test( Description="Test GetEncodedLength method.")]
		public void GetEncodedLength() {

			Assert.IsTrue( _encoder.GetEncodedLength( 0) == 0);
			Assert.IsTrue( _encoder.GetEncodedLength( 10) == 10);
			Assert.IsFalse( _encoder.GetEncodedLength( 8) == 4);
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

			for ( int i = _data.Length - 1; i >= 0; i--) {
				Assert.IsTrue( _data[i] == encodedData[i]);
			}
		}

		/// <summary>
		/// Test Decode method.
		/// </summary>
		[Test( Description="Test Decode method.")]
		public void Decode() {

			ParserContext parserContext =
				new ParserContext( ParserContext.DefaultBufferSize);

			parserContext.Write( _data);

			byte[] decodedData = _encoder.Decode( ref parserContext,
				parserContext.DataLength);

			Assert.IsNotNull( decodedData);
			Assert.IsTrue( decodedData.Length == _data.Length);
			for ( int i = _data.Length - 1; i >= 0; i--) {
				Assert.IsTrue( decodedData[i] == _data[i]);
			}
		}
		#endregion
	}
}
