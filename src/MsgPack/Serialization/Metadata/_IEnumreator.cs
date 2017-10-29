#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke
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
#if DEBUG
#if NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // NETSTANDARD1_1
#endif // DEBUG
using System.Reflection;

namespace MsgPack.Serialization.Metadata
{
	internal static class _IEnumerator
	{
		public static readonly MethodInfo MoveNext = typeof( IEnumerator ).GetMethod( nameof( IEnumerator.MoveNext ), ReflectionAbstractions.EmptyTypes );
		public static readonly PropertyInfo Current = typeof( IEnumerator ).GetProperty( nameof( IEnumerator.Current ) );

		public static PropertyInfo FindEnumeratorCurrentProperty( Type enumeratorType, CollectionTraits traits )
		{
#if DEBUG
			Contract.Assert( traits.GetEnumeratorMethod != null );
#endif // DEBUG
			var currentProperty = traits.GetEnumeratorMethod.ReturnType.GetProperty( nameof( IEnumerator<object>.Current ) );

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
						currentProperty = typeof( IEnumerator<> ).MakeGenericType( traits.ElementType ).GetProperty( nameof( IEnumerator<object>.Current ) );
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
			var moveNextMethod = enumeratorType.GetMethod( nameof( IEnumerator<object>.MoveNext ), ReflectionAbstractions.EmptyTypes );

			if ( moveNextMethod == null )
			{
				moveNextMethod = Metadata._IEnumerator.MoveNext;
			}
			return moveNextMethod;
		}
	}
}
