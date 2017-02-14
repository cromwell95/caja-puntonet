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

using Trx.Utilities;
using NUnit.Framework;

namespace Trx.Tests.Utilities {

	/// <summary>
	/// Test fixture for StringUtilities.
	/// </summary>
	[TestFixture( Description="StringUtilities functionality tests.")]
	public class StringUtilitiesTest {

		#region Class constructors
		/// <summary>
		/// Default <see cref="StringUtilitiesTest"/> constructor.
		/// </summary>
		public StringUtilitiesTest() {

		}
		#endregion

		#region Class methods
		/// <summary>
		/// Test IsNullOrEmpty method.
		/// </summary>
		[Test( Description="Test IsNullOrEmpty method.")]
		public void IsNullOrEmpty() {

			Assert.IsTrue( StringUtilities.IsNullOrEmpty( "  "));
			Assert.IsTrue( StringUtilities.IsNullOrEmpty( string.Empty));
			Assert.IsTrue( StringUtilities.IsNullOrEmpty( null));
			Assert.IsFalse( StringUtilities.IsNullOrEmpty( "Should be false"));
		}

		/// <summary>
		/// Test ConvertEmptyToNull method.
		/// </summary>
		[Test( Description="Test ConvertEmptyToNull method.")]
		public void ConvertEmptyToNull() {

			Assert.IsNull( StringUtilities.ConvertEmptyToNull( "  "));
			Assert.IsNull( StringUtilities.ConvertEmptyToNull( string.Empty));
			Assert.IsNull( StringUtilities.ConvertEmptyToNull( null));
			Assert.IsNotNull( StringUtilities.ConvertEmptyToNull( "Should be not null"));
		}
		#endregion
	}
}
