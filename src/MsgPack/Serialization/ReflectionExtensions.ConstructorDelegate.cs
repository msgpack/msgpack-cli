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
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization
{
	partial class ReflectionExtensions
	{
		public static Func<T> CreateConstructorDelegate<T>()
		{
			return CreateConstructorDelegate<T>( typeof( T ) );
		}

		public static Func<T> CreateConstructorDelegate<T>( this Type source )
		{
			ValidateInstanceType( source, typeof( T ) );

			ConstructorInfo defaultConstructor = null;

			if ( !typeof( T ).GetIsValueType() )
			{
				defaultConstructor = GetConstructor( typeof( T ), ReflectionAbstractions.EmptyTypes );

				if ( defaultConstructor == null )
				{
					throw new InvalidOperationException(
							String.Format(
								CultureInfo.CurrentCulture,
								"There are no default constructors in type '{0}'.",
								typeof( T )
							)
						);
				}
			}

			return ( Func<T> ) CreateDelegate( typeof( Func<T> ), typeof( T ), defaultConstructor, ReflectionAbstractions.EmptyTypes );
		}

		public static Func<TArg, T> CreateConstructorDelegate<T, TArg>()
		{
			return CreateConstructorDelegate<T, TArg>( typeof( T ) );
		}

		public static Func<TArg, T> CreateConstructorDelegate<T, TArg>( this Type source )
		{
			ValidateInstanceType( source, typeof( T ) );
			var parameterTypes = new[] { typeof( TArg ) };
			return ( Func<TArg, T> ) CreateDelegate( typeof( Func<TArg, T> ), typeof( T ), GetConstructor( typeof( T ), parameterTypes ), parameterTypes );
		}

		private static void ValidateInstanceType( Type source, Type returnType )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( !returnType.IsAssignableFrom( source ) )
			{
				throw new InvalidOperationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"The instance type '{0}' is not assignable to return type '{1}'.",
							source,
							returnType
						)
					);
			}

		}

		private static ConstructorInfo GetConstructor( Type source, Type[] parameterTypes )
		{
			var constructor = source.GetRuntimeConstructor( parameterTypes );
			if ( constructor != null )
			{
				return constructor;
			}

			throw new InvalidOperationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"There are no constructors in type '{0}' which matches parameter types '{1}'.",
						source,
						String.Join( ", ", parameterTypes.Select( t => t.GetFullName() ).ToArray() )
					)
				);
		}

		private static Delegate CreateDelegate( Type delegateType, Type targetType, ConstructorInfo constructor, Type[] parameterTypes )
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