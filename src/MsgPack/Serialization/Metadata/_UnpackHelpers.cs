#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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
		///		<see cref="UnpackHelpers.UnpackCollection{T}(Unpacker,Int32,T,Action{MsgPack.Unpacker,T,int},Action{MsgPack.Unpacker,T,int,int})"/> generic method.
		/// </summary>
		public static readonly MethodInfo UnpackCollection_1Method =
			typeof( UnpackHelpers ).GetMethod( "UnpackCollection" );

#if FEATURE_TAP

		/// <summary>
		///		<see cref="UnpackHelpers.UnpackCollectionAsync{T}(Unpacker,int,T,System.Func{Unpacker,T,int,CancellationToken,Task},Func{Unpacker,T,int,int,CancellationToken,Task},CancellationToken)"/> generic method.
		/// </summary>
		public static readonly MethodInfo UnpackCollectionAsync_1Method =
			typeof( UnpackHelpers ).GetMethod( "UnpackCollectionAsync" );

		/// <summary>
		///		<see cref="UnpackHelpers.ToNullable{T}"/> generic method.
		/// </summary>
		public static readonly MethodInfo ToNullable1Method = typeof( UnpackHelpers ).GetMethod( "ToNullable" );

		/// <summary>
		///		<see cref="UnpackHelpers.UnpackFromMessageAsync{T}(T,Unpacker,CancellationToken)"/> generic method.
		/// </summary>
		public static readonly MethodInfo UnpackFromMessageAsyncMethod = typeof( UnpackHelpers ).GetMethod( "UnpackFromMessageAsync" );

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
