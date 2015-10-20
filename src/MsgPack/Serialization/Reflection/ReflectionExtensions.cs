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
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY

namespace MsgPack.Serialization.Reflection
{
	/// <summary>
	///		Defines utility extension method for reflection API.
	/// </summary>
	internal static class ReflectionExtensions
	{
		/// <summary>
		///		Determines whether specified <see cref="Type"/> can be assigned to source <see cref="Type"/>.
		/// </summary>
		/// <param name="source">The source type.</param>
		/// <param name="target">The type to compare with the source type.</param>
		/// <returns>
		///   <c>true</c> if <paramref name="source"/> and <paramref name="target"/> represent the same type, 
		///   or if <paramref name="target"/> is in the inheritance hierarchy of <paramref name="source"/>, 
		///   or if <paramref name="target"/> is an interface that <paramref name="source"/> implements, 
		///   or if <paramref name="source"/> is a generic type parameter and <paramref name="target"/> represents one of the constraints of <paramref name="source"/>. 
		///   <c>false</c> if none of these conditions are <c>true</c>, or if <paramref name="target"/> is <c>false</c>. 
		/// </returns>
		public static bool IsAssignableTo( this Type source, Type target )
		{
#if !UNITY && DEBUG
			Contract.Assert( source != null, "source != null" );
#endif // !UNITY && DEBUG

			if ( target == null )
			{
				return false;
			}

			return target.IsAssignableFrom( source );
		}
	}
}
