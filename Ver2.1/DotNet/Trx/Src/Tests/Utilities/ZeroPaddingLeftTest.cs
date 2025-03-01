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

using Trx.Utilities;
using NUnit.Framework;

namespace Trx.Tests.Utilities {

	/// <summary>
	/// Test fixture for ZeroPaddingLeft.
	/// </summary>
	[TestFixture( Description="Zero padding right tests.")]
	public class ZeroPaddingLeftTest {

		private ZeroPaddingLeft _paddingWithTruncate = null;
		private ZeroPaddingLeft _paddingWithoutTruncate = null;

		#region Constructors
		/// <summary>
		/// It builds and initializes a new instance of the class
		/// <see cref="ZeroPaddingLeftTest"/>.
		/// </summary>
		public ZeroPaddingLeftTest() {

		}
		#endregion

		#region Methods
		/// <summary>
		/// This method will be called by NUnit for test setup.
		/// </summary>
		[SetUp]
		public void SetUp() {

			_paddingWithTruncate = ZeroPaddingLeft.GetInstance( true, true);
			Assert.IsNotNull( _paddingWithTruncate);

			_paddingWithoutTruncate = ZeroPaddingLeft.GetInstance( false, true);
			Assert.IsNotNull( _paddingWithoutTruncate);
		}

		/// <summary>
		/// Test GetInstance method.
		/// </summary>
		[Test( Description="Test GetInstance method.")]
		public void GetInstance() {

			Assert.IsTrue( _paddingWithTruncate == ZeroPaddingLeft.GetInstance( true, true));
			Assert.IsTrue( _paddingWithoutTruncate == ZeroPaddingLeft.GetInstance( false, true));
		}

		/// <summary>
		/// Test Truncate property.
		/// </summary>
		[Test( Description="Test Truncate property.")]
		public void Truncate() {

			Assert.IsTrue( _paddingWithTruncate.Truncate == true);
			Assert.IsTrue( _paddingWithoutTruncate.Truncate == false);
		}


		/// <summary>
		/// Test Pad method.
		/// </summary>
		[Test( Description="Test Pad method.")]
		public void Pad() {

			string data = "Test data";
			string paddedData = "00000000000Test data";
			string result;

			// Test padding with truncate.
			result = _paddingWithTruncate.Pad( data, 20);
			Assert.IsTrue( result.Length == 20);
			Assert.IsTrue( paddedData.Equals( result));

			// Test padding with truncate.
			result = _paddingWithTruncate.Pad( data, data.Length);
			Assert.IsTrue( result.Length == data.Length);
			Assert.IsTrue( data.Equals( result));

			// Test padding with truncate.
			result = _paddingWithTruncate.Pad( null, 5);
			Assert.IsTrue( result.Length == 5);
			Assert.IsTrue( result.Equals( "00000"));

			// Test padding with truncate.
			result = _paddingWithTruncate.Pad( string.Empty, 3);
			Assert.IsTrue( result.Length == 3);
			Assert.IsTrue( result.Equals( "000"));

			// Test truncate.
			result = _paddingWithTruncate.Pad( data, 4);
			Assert.IsTrue( result.Length == 4);
			Assert.IsTrue( data.Substring( 0, 4).Equals( result));

			// Test width.
			try {
				result = _paddingWithTruncate.Pad( data, 0);
				Assert.Fail();
			} catch ( ArgumentException e) {
				Assert.IsTrue( e.ParamName == "totalWidth");
			}

			// Test padding without truncate.
			result = _paddingWithoutTruncate.Pad( data, 20);
			Assert.IsTrue( result.Length == 20);
			Assert.IsTrue( paddedData.Equals( result));

			// Test padding without truncate.
			result = _paddingWithoutTruncate.Pad( data, data.Length);
			Assert.IsTrue( result.Length == data.Length);
			Assert.IsTrue( data.Equals( result));

			// Test padding without truncate.
			result = _paddingWithoutTruncate.Pad( null, 5);
			Assert.IsTrue( result.Length == 5);
			Assert.IsTrue( result.Equals( "00000"));

			// Test padding without truncate.
			result = _paddingWithoutTruncate.Pad( string.Empty, 3);
			Assert.IsTrue( result.Length == 3);
			Assert.IsTrue( result.Equals( "000"));

			// Test truncate (must fail).
			try {
				result = _paddingWithoutTruncate.Pad( data, 4);
				Assert.Fail();
			} catch ( ArgumentException e) {
				Assert.IsTrue( e.ParamName == "data");
			}

			// Test width.
			try {
				result = _paddingWithoutTruncate.Pad( data, 0);
				Assert.Fail();
			} catch ( ArgumentException e) {
				Assert.IsTrue( e.ParamName == "totalWidth");
			}
		}


		/// <summary>
		/// Test RemovePad method.
		/// </summary>
		[Test( Description="Test RemovePad method.")]
		public void RemovePad() {

			string data = "My test data";
			string paddedData = "00000000My test data";
			string result;

			result = _paddingWithTruncate.RemovePad( paddedData);
			Assert.IsTrue( data.Equals( result));

			result = _paddingWithTruncate.RemovePad( data);
			Assert.IsTrue( data.Equals( result));

			result = _paddingWithTruncate.RemovePad( null);
			Assert.IsNull( result);

			result = _paddingWithTruncate.RemovePad( string.Empty);
			Assert.IsTrue( result.Equals( "0"));

			result = _paddingWithoutTruncate.RemovePad( paddedData);
			Assert.IsTrue( data.Equals( result));

			result = _paddingWithoutTruncate.RemovePad( data);
			Assert.IsTrue( data.Equals( result));

			result = _paddingWithoutTruncate.RemovePad( null);
			Assert.IsNull( result);

			result = _paddingWithoutTruncate.RemovePad( string.Empty);
			Assert.IsTrue( result.Equals( "0"));
		}
		#endregion
	}
}
