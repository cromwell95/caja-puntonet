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
using System.Globalization;
using System.Security.Permissions;
using System.Threading;


using Trx.Messaging;



namespace TrxExamples.Messaging {


	public class TramaEnvio 
    {
        
        
		static void Main( string[] args ) 
        {
            try
            {
                Console.WriteLine("CurrentCulture is {0}.", CultureInfo.CurrentCulture.Name);
               // Console.WriteLine(Envio.Envio_requerimiento("192.168.123.39", 5000, 10000, "013000005482440202834002=09065012138688600000022   00000000100000000000012000001410351320101008      11111111900000328464   000007B5482440202834002^PABLO MOREIRA             ^0906501213860000000000886000000                       ", 1, 1));
                Console.ReadLine();
            }
            catch(Exception e) 
            {
                
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
		}
	}
}
