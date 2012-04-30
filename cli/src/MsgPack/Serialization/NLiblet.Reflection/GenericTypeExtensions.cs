#region -- License Terms --
//
// NLiblet
//
// Copyright (C) 2011 FUJIWARA, Yusuke
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
using System.Diagnostics.Contracts;
using System.Linq;

namespace NLiblet.Reflection
{
	/// <summary>
	///		Define utility extension method for generic type.
	/// </summary>
	internal static class GenericTypeExtensions
	{
		/// <summary>
		///		Determine whether the source type inherits directly or indirectly from specified generic type or its built type.
		/// </summary>
		/// <param name="source">Target type.</param>
		/// <param name="genericType">Generic type.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="source"/>, directly or indirectly, inherits from <paramref name="genericType"/>,
		///		or built closed generic type;
		///		otherwise <c>false</c>.
		/// </returns>
		[Pure]
		public static bool Inherits( this Type source, Type genericType )
		{
			Contract.Assert( source != null );
			Contract.Assert( genericType != null );
			Contract.Assert( !genericType.IsInterface );

			return FindGenericClass( source, genericType, false ) != null;
		}

		/// <summary>
		///		Determine whether the element type of the source array type inherits directly or indirectly from specified generic type or its built type.
		/// </summary>
		/// <param name="source">Target type.</param>
		/// <param name="genericType">Generic type.</param>
		/// <returns>
		///		<c>true</c> if the elements of <paramref name="source"/>, directly or indirectly, inherits from <paramref name="genericType"/>,
		///		or built closed generic type;
		///		otherwise <c>false</c>.
		/// </returns>
		[Pure]
		public static bool ElementInherits( this Type source, Type genericType )
		{
			Contract.Assert( source != null );
			Contract.Assert( source.IsArray );
			Contract.Assert( genericType != null );
			Contract.Assert( !genericType.IsInterface );

			return FindGenericClass( source.GetElementType(), genericType, false ) != null;
		}

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
		[Pure]
		public static bool Implements( this Type source, Type genericType )
		{
			Contract.Assert( source != null );
			Contract.Assert( genericType != null );
			Contract.Assert( genericType.IsInterface );

			return EnumerateGenericIntefaces( source, genericType, false ).Any();
		}

		/// <summary>
		///		Determine whether the element type of the source array type implements specified generic type or its built type.
		/// </summary>
		/// <param name="source">Target type.</param>
		/// <param name="genericType">Generic interface type.</param>
		/// <returns>
		///		<c>true</c> if the elements of <paramref name="source"/> implements <paramref name="genericType"/>,
		///		or built closed generic interface type;
		///		otherwise <c>false</c>.
		/// </returns>
		[Pure]
		public static bool ElementImplements( this Type source, Type genericType )
		{
			Contract.Assert( source != null );
			Contract.Assert( source.IsArray );
			Contract.Assert( genericType != null );
			Contract.Assert( genericType.IsInterface );

			return EnumerateGenericIntefaces( source.GetElementType(), genericType, false ).Any();
		}

		/// <summary>
		///		Gets the generic types which is specified generic type or its built type, which are implemented or inherited by source type.
		/// </summary>
		/// <param name="source">Target type.</param>
		/// <param name="genericType">Generic type.</param>
		/// <returns>
		///		If <paramref name="source"/> has closed built type of, inhertis, or implements <paramref name="genericType"/>or its closed type,
		///		returns these types.
		///		otherwise empty array.
		/// </returns>
		/// <remarks>
		///		If <paramref name="source"/> is generic type definition, then return types are generic type definitions.
		///		Else return types are not generic type definitions, but might not be closed generic type when in the generic method.
		/// </remarks>
		[Pure]
		public static Type[] FindGenericTypes( this Type source, Type genericType )
		{
			Contract.Assert( source != null );
			Contract.Assert( genericType != null );
			Contract.Assert( genericType.IsGenericType );
			Contract.Ensures( Contract.Result<Type[]>() != null );

			if ( genericType.IsInterface )
			{
				return EnumerateGenericIntefaces( source, genericType, true ).ToArray();
			}
			else
			{
				var result = FindGenericClass( source, genericType, true );
				return result == null ? Type.EmptyTypes : new Type[] { result };
			}
		}

		/// <summary>
		///		Gets the generic types which is specified generic type or its built type, which are implemented or inherited by the element type of source array type.
		/// </summary>
		/// <param name="source">Target type.</param>
		/// <param name="genericType">Generic type.</param>
		/// <returns>
		///		If <paramref name="source"/> has closed built type of, inhertis, or implements <paramref name="genericType"/>or its closed type,
		///		returns these types.
		///		otherwise empty array.
		/// </returns>
		/// <remarks>
		///		Return types are not generic type definitions, but might not be closed generic type when in the generic method.
		/// </remarks>
		[Pure]
		public static Type[] FindElementGenericTypes( this Type source, Type genericType )
		{
			Contract.Assert( source != null );
			Contract.Assert( source.IsArray );
			Contract.Assert( genericType != null );
			Contract.Assert( genericType.IsGenericType );
			Contract.Ensures( Contract.Result<Type[]>() != null );

			if ( genericType.IsInterface )
			{
				return EnumerateGenericIntefaces( source.GetElementType(), genericType, true ).ToArray();
			}
			else
			{
				var result = FindGenericClass( source.GetElementType(), genericType, true );
				return result == null ? Type.EmptyTypes : new Type[] { result };
			}
		}

		private static Type FindGenericClass( Type source, Type genericType, bool includesOwn )
		{
			for ( Type current = includesOwn ? source : source.BaseType; current != null; current = current.BaseType )
			{
				if ( current.IsGenericType )
				{
					if ( genericType.IsGenericTypeDefinition )
					{
						var definition = current.IsGenericTypeDefinition ? current : current.GetGenericTypeDefinition();

						if ( definition == genericType )
						{
							// If source is GenericTypeDefinition, type def is only valid type (i.e. has name)
							return source.IsGenericTypeDefinition ? current.GetGenericTypeDefinition() : current;
						}
					}
					else
					{
						if ( current == genericType )
						{
							return current;
						}
					}
				}
			}

			return null;
		}

		private static IEnumerable<Type> EnumerateGenericIntefaces( Type source, Type genericType, bool includesOwn )
		{
			return
				( includesOwn ? new[] { source }.Concat( source.GetInterfaces() ) : source.GetInterfaces() )
				.Where( @interface =>
					@interface.IsGenericType
					&& ( genericType.IsGenericTypeDefinition
						? @interface.GetGenericTypeDefinition() == genericType
						: @interface == genericType
					)
				).Select( @interface => // If source is GenericTypeDefinition, type def is only valid type (i.e. has name)
					source.IsGenericTypeDefinition ? @interface.GetGenericTypeDefinition() : @interface
				);
		}

		/// <summary>
		///		Determine whether source type is closed generic type for specified open generic type (a.k.a. generic type definition).
		/// </summary>
		/// <param name="source">Target type.</param>
		/// <param name="genericTypeDefinition">Generic type definition.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="source"/> is closed type built from <paramref name="genericTypeDefinition"/>,
		///		otherwise <c>false</c>.
		///		When <paramref name="source"/> is not generic type or is generic type definition then <c>false</c>.
		/// </returns>
		[Pure]
		public static bool IsClosedTypeOf( this Type source, Type genericTypeDefinition )
		{
			Contract.Assert( source != null );
			Contract.Assert( genericTypeDefinition != null );
			Contract.Assert( genericTypeDefinition.IsGenericTypeDefinition );

			if ( !source.IsGenericType || source.IsGenericTypeDefinition )
			{
				return false;
			}
			else
			{
				return genericTypeDefinition == source.GetGenericTypeDefinition();
			}
		}

		/// <summary>
		///		Get name of type without namespace and assembly name of itself and its generic arguments.
		/// </summary>
		/// <param name="source">Target type.</param>
		/// <returns>Simple name of type.</returns>
		[Pure]
		public static string GetName( this Type source )
		{
			Contract.Assert( source != null );
			if ( !source.IsGenericType )
			{
				return source.Name;
			}

			return
				String.Join(
					String.Empty,
					source.Name,
					'[',
					String.Join( ", ", source.GetGenericArguments().Select( t => t.GetName() ) ),
					']'
				);
		}

		/// <summary>
		///		Get full name of type including namespace and excluding assembly name of itself and its generic arguments.
		/// </summary>
		/// <param name="source">Target type.</param>
		/// <returns>Full name of type.</returns>
		[Pure]
		public static string GetFullName( this Type source )
		{
			Contract.Assert( source != null );

			if ( source.IsArray )
			{
				var elementType = source.GetElementType();
				if ( !elementType.IsGenericType )
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

			if ( !source.IsGenericType )
			{
				return source.FullName;
			}

			return
				String.Join(
					String.Empty,
					source.Namespace,
					Type.Delimiter,
					source.Name,
					'[',
					String.Join( ", ", source.GetGenericArguments().Select( t => t.GetFullName() ) ),
					']'
				);
		}
	}
}
