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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace MsgPack
{
	partial class Packer
	{
		private static Type ExtractCollectionType( Type targetType )
		{
			Contract.Assert( targetType != null );

			if ( targetType == typeof( byte[] ) )
			{
				return targetType;
			}

			if ( targetType == typeof( string ) )
			{
				return targetType;
			}

			if ( targetType.IsArray )
			{
				return typeof( IList<> ).MakeGenericType( targetType.GetElementType() );
			}

			foreach ( var type in Enumerable.Repeat( targetType, 1 ).Concat( targetType.GetInterfaces() ) )
			{
				if ( type == typeof( IList ) )
				{
					return targetType;
				}

				if ( type == typeof( IDictionary ) )
				{
					return targetType;
				}

				if ( type.IsGenericType )
				{
					var genericTypeDefinition = type.GetGenericTypeDefinition();
					if ( genericTypeDefinition == typeof( IList<> ) )
					{
						return type;
					}
					if ( genericTypeDefinition == typeof( IDictionary<,> ) )
					{
						return type;
					}
				}
			}

			return null;
		}
	}
}
