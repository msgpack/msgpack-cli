#region -- License Terms --
//
// NLiblet
//
// Copyright (C) 2015 FUJIWARA, Yusuke
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
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization.Reflection
{
	/// <summary>
	///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		Defines serialization helper APIs.
	/// </summary>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static class ReflectionHelpers
	{
		internal static readonly MethodInfo GetRuntimeMethodMethod =
			FromExpression.ToMethod(
				( Type source, string name, Type[] parameterTypes ) => GetRuntimeMethod( source, name, parameterTypes ) 
			);

		/// <summary>
		///		Gets a specific method even if the candidate is not public.
		/// </summary>
		/// <param name="source">The target type.</param>
		/// <param name="name">The name of the method.</param>
		/// <param name="parameterTypes">The types of the parameter.</param>
		/// <returns>A <see cref="MethodInfo"/> if found; otherwise, <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static MethodInfo GetRuntimeMethod( this Type source, string name, params Type[] parameterTypes )
		{
			if ( name == null )
			{
				throw new ArgumentNullException( "name" );
			}

			if ( name.Length == 0 )
			{
				throw new ArgumentException( "'name' cannot be empty.", "name" );
			}

			var safeParameterTypes = parameterTypes ?? ReflectionAbstractions.EmptyTypes;

			return
				source.GetRuntimeMethods()
					.SingleOrDefault(
						m => m.Name == name && m.GetParameters().Select( p => p.ParameterType ).SequenceEqual( safeParameterTypes ) 
					);
		}
		internal static readonly MethodInfo GetRuntimeFieldMethod =
			FromExpression.ToMethod(
				( Type source, string name ) => GetRuntimeField( source, name )
			);

		/// <summary>
		///		Gets a specific field even if the candidate is not public.
		/// </summary>
		/// <param name="source">The target type.</param>
		/// <param name="name">The name of the method.</param>
		/// <returns>A <see cref="FieldInfo"/> if found; otherwise, <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static FieldInfo GetRuntimeField( this Type source, string name )
		{
			if ( name == null )
			{
				throw new ArgumentNullException( "name" );
			}

			if ( name.Length == 0 )
			{
				throw new ArgumentException( "'name' cannot be empty.", "name" );
			}

			return
				source.GetRuntimeFields()
					.SingleOrDefault(
						m => m.Name == name 
					);
		}
	}
}
