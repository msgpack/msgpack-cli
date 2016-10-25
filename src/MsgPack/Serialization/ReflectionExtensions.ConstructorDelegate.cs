#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke
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
using System.Reflection;
using System.Reflection.Emit;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization
{
	partial class ReflectionExtensions
	{
		public static TDelegate CreateConstructorDelegate<TDelegate>( this ConstructorInfo constructor )
		{
			return ( TDelegate )CreateDelegate( typeof( TDelegate ), constructor.DeclaringType, constructor, constructor.GetParameterTypes() );
		}

		private static object CreateDelegate( Type delegateType, Type targetType, ConstructorInfo constructor, Type[] parameterTypes )
		{
			var dynamicMethod =
#if !SILVERLIGHT
				new DynamicMethod( "Create" + targetType.Name, targetType, parameterTypes, restrictedSkipVisibility: true );
#else
				new DynamicMethod( "Create" + targetType.Name, targetType, parameterTypes );
#endif // !SILVERLIGHT
			var il = new TracingILGenerator( dynamicMethod, NullTextWriter.Instance, isDebuggable: false );
			if ( constructor == null )
			{
				// Value type's init.
				il.DeclareLocal( targetType );
				il.EmitAnyLdloca( 0 );
				il.EmitInitobj( targetType );
				il.EmitAnyLdloc( 0 );
			}
			else
			{
				for ( var i = 0; i < parameterTypes.Length; i++ )
				{
					il.EmitAnyLdarg( i );
				}

				il.EmitNewobj( constructor );
			}

			il.EmitRet();
			return dynamicMethod.CreateDelegate( delegateType );
		}
	}
}