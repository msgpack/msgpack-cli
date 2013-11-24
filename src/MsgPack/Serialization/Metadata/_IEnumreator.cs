#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MsgPack.Serialization.Metadata
{
	internal static class _IEnumerator
	{
		private static readonly Type[] EmptyTypes = new Type[ 0 ];
		public static readonly MethodInfo MoveNext = FromExpression.ToMethod( ( IEnumerator enumerator ) => enumerator.MoveNext() );
		public static readonly PropertyInfo Current = FromExpression.ToProperty( ( IEnumerator enumerator ) => enumerator.Current );

		public static PropertyInfo FindEnumeratorCurrentProperty( Type enumeratorType, CollectionTraits traits )
		{
			PropertyInfo currentProperty = traits.GetEnumeratorMethod.ReturnType.GetProperty( "Current" );

			if ( currentProperty == null )
			{
				if ( enumeratorType == typeof( IDictionaryEnumerator ) )
				{
					currentProperty = Metadata._IDictionaryEnumerator.Entry;
				}
				else if ( enumeratorType.GetIsInterface() )
				{
					if ( enumeratorType.GetIsGenericType() && enumeratorType.GetGenericTypeDefinition() == typeof( IEnumerator<> ) )
					{
						currentProperty = typeof( IEnumerator<> ).MakeGenericType( traits.ElementType ).GetProperty( "Current" );
					}
					else
					{
						currentProperty = Metadata._IEnumerator.Current;
					}
				}
			}
			return currentProperty;
		}

		public static MethodInfo FindEnumeratorMoveNextMethod( Type enumeratorType )
		{
			MethodInfo moveNextMethod = enumeratorType.GetMethod( "MoveNext", EmptyTypes );

			if ( moveNextMethod == null )
			{
				moveNextMethod = Metadata._IEnumerator.MoveNext;
			}
			return moveNextMethod;
		}
	}
}
