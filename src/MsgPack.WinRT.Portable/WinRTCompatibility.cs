#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
#endregion -- License Terms --

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MsgPack
{
	internal static class WinRTCompatibility
	{
		private static readonly Dictionary<Type, TypeCode> _typeCodeTable =
			new Dictionary<Type, TypeCode>()
			{
				{ typeof( Boolean ), TypeCode.Boolean },
				{ typeof( Char ), TypeCode.Char },
				{ typeof( Byte ), TypeCode.Byte },
				{ typeof( Int16 ), TypeCode.Int16 },
				{ typeof( Int32 ), TypeCode.Int32 },
				{ typeof( Int64 ), TypeCode.Int64 },
				{ typeof( SByte ), TypeCode.SByte },
				{ typeof( UInt16 ), TypeCode.UInt16 },
				{ typeof( UInt32 ), TypeCode.UInt32 },
				{ typeof( UInt64 ), TypeCode.UInt64 },
				{ typeof( Single ), TypeCode.Single },
				{ typeof( Double ), TypeCode.Double },
				{ typeof( DateTime ), TypeCode.DateTime },
				{ typeof( Decimal ), TypeCode.Decimal },
				{ typeof( String ), TypeCode.String },
			};

		public static TypeCode GetTypeCode( Type type )
		{
			if ( type == null )
			{
				return TypeCode.Empty;
			}

			TypeCode result;
			if ( !_typeCodeTable.TryGetValue( type, out result ) )
			{
				result = TypeCode.Object;
			}

			return result;
		}
	}

	internal enum TypeCode
	{
		Byte,
		Int16,
		Int32,
		Int64,
		SByte,
		UInt16,
		UInt32,
		UInt64,
		Single,
		Double,
		Char,
		Boolean,
		String,
		DateTime,
		Decimal,
		Empty,
		DBNull, // Never used
		Object
	}
}
