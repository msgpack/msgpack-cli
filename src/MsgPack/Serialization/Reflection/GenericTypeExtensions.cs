#region -- License Terms --
//
// NLiblet
//
// Copyright (C) 2011-2015 FUJIWARA, Yusuke
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
using PureAttribute = System.Diagnostics.Contracts.PureAttribute;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Linq;
#if NETFX_CORE
using System.Reflection;
#endif

namespace MsgPack.Serialization.Reflection
{
	/// <summary>
	///		Define utility extension method for generic type.
	/// </summary>
	internal static class GenericTypeExtensions
	{
		/// <summary>
		///		Determine whether the source type implements specified generic type or its built type.
		/// </summary>
		/// <param name="source">Target type.</param>
		/// <param name="genericType">Generic interface type.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="source"/> implements <paramref name="genericType"/>,
		///		or built closed generic interface type;
		///		otherwise <c>false</c>.
		/// </returns>
#if !UNITY
		[Pure]
#endif // !UNITY
		public static bool Implements( this Type source, Type genericType )
		{
#if !UNITY
			Contract.Assert( source != null, "source != null" );
			Contract.Assert( genericType != null, "genericType != null" );
			Contract.Assert( genericType.GetIsInterface(), "genericType.GetIsInterface()" );
#endif // !UNITY

			return EnumerateGenericIntefaces( source, genericType, false ).Any();
		}

		private static IEnumerable<Type> EnumerateGenericIntefaces( Type source, Type genericType, bool includesOwn )
		{
			return
				( includesOwn ? new[] { source }.Concat( source.GetInterfaces() ) : source.GetInterfaces() )
				.Where( @interface =>
					@interface.GetIsGenericType()
					&& ( genericType.GetIsGenericTypeDefinition()
						? @interface.GetGenericTypeDefinition() == genericType
						: @interface == genericType
					)
				).Select( @interface => // If source is GenericTypeDefinition, type def is only valid type (i.e. has name)
					source.GetIsGenericTypeDefinition() ? @interface.GetGenericTypeDefinition() : @interface
				);
		}

		/// <summary>
		///		Get name of type without namespace and assembly name of itself and its generic arguments.
		/// </summary>
		/// <param name="source">Target type.</param>
		/// <returns>Simple name of type.</returns>
#if !UNITY
		[Pure]
#endif // !UNITY
		public static string GetName( this Type source )
		{
#if !UNITY
			Contract.Assert( source != null, "source != null" );
#endif // !UNITY
			if ( !source.GetIsGenericType() )
			{
				return source.Name;
			}

			return
				String.Concat(
					source.Name,
					'[',
#if !NETFX_35 && !UNITY
					String.Join( ", ", source.GetGenericArguments().Select( t => t.GetName() ) ),
#else
					String.Join( ", ", source.GetGenericArguments().Select( t => t.GetName() ).ToArray() ),
#endif // !NETFX_35 && !UNITY
					']'
				);
		}

		/// <summary>
		///		Get full name of type including namespace and excluding assembly name of itself and its generic arguments.
		/// </summary>
		/// <param name="source">Target type.</param>
		/// <returns>Full name of type.</returns>
#if !UNITY
		[Pure]
#endif // !UNITY
		public static string GetFullName( this Type source )
		{
#if !UNITY
			Contract.Assert( source != null, "source != null" );
#endif // !UNITY

			if ( source.IsArray )
			{
				var elementType = source.GetElementType();
				if ( !elementType.GetIsGenericType() )
				{
					return source.FullName;
				}

				if ( 1 < source.GetArrayRank() )
				{
					return elementType.GetFullName() + "[*]";
				}
				else
				{
					return elementType.GetFullName() + "[]";
				}
			}

			if ( !source.GetIsGenericType() )
			{
				return source.FullName;
			}

			return
				String.Concat(
					source.Namespace,
					ReflectionAbstractions.TypeDelimiter,
					source.Name,
					'[',
#if !NETFX_35 && !UNITY
					String.Join( ", ", source.GetGenericArguments().Select( t => t.GetFullName() ) ),
#else
					String.Join( ", ", source.GetGenericArguments().Select( t => t.GetFullName() ).ToArray() ),
#endif // !NETFX_35 && !UNITY
					']'
				);
		}
	}
}
