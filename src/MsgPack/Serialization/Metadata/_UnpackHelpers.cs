#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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

// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.Reflection;

namespace MsgPack.Serialization.Metadata
{
	internal static partial class _UnpackHelpers
	{
		public static readonly MethodInfo GetItemsCount =
			FromExpression.ToMethod( ( Unpacker unpacker ) => UnpackHelpers.GetItemsCount( unpacker ) );

		/// <summary>
		///		<see cref="UnpackHelpers.GetEqualityComparer{T}()"/> generic method.
		/// </summary>
		public static readonly MethodInfo GetEqualityComparer_1Method =
			typeof( UnpackHelpers ).GetMethod( "GetEqualityComparer" );

		/// <summary>
		///		<see cref="UnpackHelpers.UnpackFromArray{TContext,TResult}(Unpacker,TContext,Func{TContext,TResult},IList{string},IList{System.Action{MsgPack.Unpacker,TContext,int}})"/> generic method.
		/// </summary>
		public static readonly MethodInfo UnpackFromArray_2Method =
			typeof( UnpackHelpers ).GetMethod( "UnpackFromArray" );

		/// <summary>
		///		<see cref="UnpackHelpers.UnpackFromMap{TContext,TResult}(Unpacker,TContext,Func{TContext,TResult},IDictionary{String,Action{Unpacker,TContext,int}})"/> generic method.
		/// </summary>
		public static readonly MethodInfo UnpackFromMap_2Method =
			typeof( UnpackHelpers ).GetMethod( "UnpackFromMap" );

		/// <summary>
		///		<see cref="UnpackHelpers.UnpackCollection{T}(Unpacker,Int32,T,Action{MsgPack.Unpacker,T,Int32},Action{MsgPack.Unpacker,T,Int32})"/> generic method.
		/// </summary>
		public static readonly MethodInfo UnpackCollection_1Method =
			typeof( UnpackHelpers ).GetMethod( "UnpackCollection" );

		/// <summary>
		///		<see cref="UnpackHelpers.GetIdentity{T}()"/> generic method.
		/// </summary>
		public static readonly MethodInfo GetIdentity_1Method =
			typeof( UnpackHelpers ).GetMethod( "GetIdentity" );

		/// <summary>
		///		<see cref="UnpackHelpers.Unbox{T}()"/> generic method.
		/// </summary>
		public static readonly MethodInfo Unbox_1Method =
			typeof( UnpackHelpers ).GetMethod( "Unbox" );
	}
}
