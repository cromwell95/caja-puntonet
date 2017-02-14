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
	/// Test fixture for FieldAttribute.
	/// </summary>
	[TestFixture( Description="FieldAttribute functionality tests.")]
	public class FieldAttributeTest {

		#region Class constructors
		/// <summary>
		/// Default <see cref="FieldAttributeTest"/> constructor.
		/// </summary>
		public FieldAttributeTest() {

		}
		#endregion

		#region Class methods
		/// <summary>
		/// This method will be called by NUnit for test setup.
		/// </summary>
		[SetUp]
		public void SetUp() {

		}

		/// <summary>
		/// Test instantiation and properties.
		/// </summary>
		[Test( Description="Test instantiation and properties.")]
		public void Instantiation() {

			FieldAttribute attr = new FieldAttribute( 10);

			Assert.IsTrue( attr.FieldNumber == 10);
		}
		#endregion
	}
}