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
using System.Linq;
using System.Reflection;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

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
		///		<see cref="UnpackHelpers.UnpackFromArray{TContext,TResult}(Unpacker,TContext,Func{TContext,TResult},IList{string},IList{System.Action{MsgPack.Unpacker,TContext,int,int}})"/> generic method.
		/// </summary>
		public static readonly MethodInfo UnpackFromArray_2Method =
			typeof( UnpackHelpers ).GetMethod( "UnpackFromArray" );

		/// <summary>
		///		<see cref="UnpackHelpers.UnpackFromMap{TContext,TResult}(Unpacker,TContext,Func{TContext,TResult},IDictionary{String,Action{Unpacker,TContext,int,int}})"/> generic method.
		/// </summary>
		public static readonly MethodInfo UnpackFromMap_2Method =
			typeof( UnpackHelpers ).GetMethod( "UnpackFromMap" );

		/// <summary>
		///		<see cref="UnpackHelpers.UnpackCollection{T}(Unpacker,Int32,T,Action{MsgPack.Unpacker,T,int},Action{MsgPack.Unpacker,T,int,int})"/> generic method.
		/// </summary>
		public static readonly MethodInfo UnpackCollection_1Method =
			typeof( UnpackHelpers ).GetMethod( "UnpackCollection" );

		/// <summary>
		///		<see cref="UnpackHelpers.UnpackComplexObject{T}(Unpacker,MessagePackSerializer{T},int)"/> generic method.
		/// </summary>
		public static readonly MethodInfo UnpackComplexObject_1MethodForArray =
			typeof( UnpackHelpers ).GetMethods().Single( m => m.Name == "UnpackComplexObject" && m.GetParameters().Length == 3 );

		/// <summary>
		///		<see cref="UnpackHelpers.UnpackComplexObject{T}(Unpacker,MessagePackSerializer{T},int,string)"/> generic method.
		/// </summary>
		public static readonly MethodInfo UnpackComplexObject_1MethodForMap =
			typeof( UnpackHelpers ).GetMethods().Single( m => m.Name == "UnpackComplexObject" && m.GetParameters().Length == 4 );

#if FEATURE_TAP

		/// <summary>
		///		<see cref="UnpackHelpers.UnpackFromArrayAsync{TContext,TResult}(Unpacker,TContext,Func{TContext,TResult},IList{string},IList{Func{Unpacker,TContext,int,int,CancellationToken,Task}},CancellationToken)"/> generic method.
		/// </summary>
		public static readonly MethodInfo UnpackFromArrayAsync_2Method =
			typeof( UnpackHelpers ).GetMethod( "UnpackFromArrayAsync" );

		/// <summary>
		///		<see cref="UnpackHelpers.UnpackFromMapAsync{TContext,TResult}(Unpacker,TContext,Func{TContext,TResult},IDictionary{string,Func{Unpacker,TContext,int,int,CancellationToken,Task}},CancellationToken)"/> generic method.
		/// </summary>
		public static readonly MethodInfo UnpackFromMapAsync_2Method =
			typeof( UnpackHelpers ).GetMethod( "UnpackFromMapAsync" );

		/// <summary>
		///		<see cref="UnpackHelpers.UnpackCollectionAsync{T}(Unpacker,int,T,System.Func{Unpacker,T,int,CancellationToken,Task},Func{Unpacker,T,int,int,CancellationToken,Task},CancellationToken)"/> generic method.
		/// </summary>
		public static readonly MethodInfo UnpackCollectionAsync_1Method =
			typeof( UnpackHelpers ).GetMethod( "UnpackCollectionAsync" );

		/// <summary>
		///		<see cref="UnpackHelpers.UnpackComplexObjectAsync{T}(Unpacker,MessagePackSerializer{T},int,CancellationToken)"/> generic method.
		/// </summary>
		public static readonly MethodInfo UnpackComplexObjectAsymc_1MethodForArray =
			typeof( UnpackHelpers ).GetMethods().Single( m => m.Name == "UnpackComplexObjectAsync" && m.GetParameters().Length == 4 );

		/// <summary>
		///		<see cref="UnpackHelpers.UnpackComplexObjectAsync{T}(Unpacker,MessagePackSerializer{T},int,string,CancellationToken)"/> generic method.
		/// </summary>
		public static readonly MethodInfo UnpackComplexObjectAsymc_1MethodForMap =
			typeof( UnpackHelpers ).GetMethods().Single( m => m.Name == "UnpackComplexObjectAsync" && m.GetParameters().Length == 5 );

		/// <summary>
		///		<see cref="UnpackHelpers.ToNullable{T}"/> generic method.
		/// </summary>
		public static readonly MethodInfo ToNullable1Method = typeof( UnpackHelpers ).GetMethod( "ToNullable" );

#endif // FEATURE_MAP

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
