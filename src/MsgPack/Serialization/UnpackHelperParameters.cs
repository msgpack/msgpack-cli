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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if !UNITY || MSGPACK_UNITY_FULL
using System.ComponentModel;
#endif //!UNITY || MSGPACK_UNITY_FULL
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization
{
	// This file was generated from UnpackHelperParameters.tt
	// DO NET modify this file directly.


	/// <summary>
	///		Represents parameters of <see cref="UnpackHelpers.UnpackValueTypeValue{TContext, TValue}(ref UnpackValueTypeValueParameters{TContext, TValue})"/> method.
	/// </summary>
	/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "This struct is not intended for value." )]
	public struct UnpackValueTypeValueParameters<TContext, TValue>
	{
		/// <summary>
		///		The unpacker.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Unpacker Unpacker;

		/// <summary>
		///		The context which will store deserialized value.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public TContext UnpackingContext;

		/// <summary>
		///		The serializer to deserialize current item.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public MessagePackSerializer<TValue> Serializer;

		/// <summary>
		///		The items count to be unpacked.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int ItemsCount;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int Unpacked;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public string MemberName;

		/// <summary>
		///		The delegate which takes <see cref="UnpackingContext" /> and unpacked value, and then set the value to the context.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Action<TContext, TValue> Setter;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Type TargetObjectType;

		/// <summary>
		///		The delegate which refers direct reading. This field should be <c>null</c> when <see cref="Serializer" /> is specified.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Func<Unpacker, Type, string, TValue> DirectRead;
	}

#if FEATURE_TAP

	/// <summary>
	///		Represents parameters of <see cref="UnpackHelpers.UnpackValueTypeValueAsync{TContext, TValue}(ref UnpackValueTypeValueAsyncParameters{TContext, TValue})"/> method.
	/// </summary>
	/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "This struct is not intended for value." )]
	public struct UnpackValueTypeValueAsyncParameters<TContext, TValue>
	{
		/// <summary>
		///		The unpacker.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Unpacker Unpacker;

		/// <summary>
		///		The context which will store deserialized value.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public TContext UnpackingContext;

		/// <summary>
		///		The serializer to deserialize current item.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public MessagePackSerializer<TValue> Serializer;

		/// <summary>
		///		The items count to be unpacked.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int ItemsCount;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int Unpacked;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public string MemberName;

		/// <summary>
		///		The delegate which takes <see cref="UnpackingContext" /> and unpacked value, and then set the value to the context.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Action<TContext, TValue> Setter;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Type TargetObjectType;

		/// <summary>
		///		The delegate which refers direct reading. This field should be <c>null</c> when <see cref="Serializer" /> is specified.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Func<Unpacker, Type, string, CancellationToken, Task<TValue>> DirectRead;

		/// <summary>
		///		The token to monitor for cancellation requests. The default value is <see cref="P:CancellationToken.None" />.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public CancellationToken CancellationToken;
	}

#endif // FEATURE_TAP

	/// <summary>
	///		Represents parameters of <see cref="UnpackHelpers.UnpackReferenceTypeValue{TContext, TValue}(ref UnpackReferenceTypeValueParameters{TContext, TValue})"/> method.
	/// </summary>
	/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "This struct is not intended for value." )]
	public struct UnpackReferenceTypeValueParameters<TContext, TValue>
	{
		/// <summary>
		///		The unpacker.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Unpacker Unpacker;

		/// <summary>
		///		The context which will store deserialized value.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public TContext UnpackingContext;

		/// <summary>
		///		The serializer to deserialize current item.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public MessagePackSerializer<TValue> Serializer;

		/// <summary>
		///		The items count to be unpacked.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int ItemsCount;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int Unpacked;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public string MemberName;

		/// <summary>
		///		The delegate which takes <see cref="UnpackingContext" /> and unpacked value, and then set the value to the context.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Action<TContext, TValue> Setter;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Type TargetObjectType;

		/// <summary>
		///		The delegate which refers direct reading. This field should be <c>null</c> when <see cref="Serializer" /> is specified.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Func<Unpacker, Type, string, TValue> DirectRead;

		/// <summary>
		///		The nil implication of current item.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public NilImplication NilImplication;
	}

#if FEATURE_TAP

	/// <summary>
	///		Represents parameters of <see cref="UnpackHelpers.UnpackReferenceTypeValueAsync{TContext, TValue}(ref UnpackReferenceTypeValueAsyncParameters{TContext, TValue})"/> method.
	/// </summary>
	/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "This struct is not intended for value." )]
	public struct UnpackReferenceTypeValueAsyncParameters<TContext, TValue>
	{
		/// <summary>
		///		The unpacker.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Unpacker Unpacker;

		/// <summary>
		///		The context which will store deserialized value.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public TContext UnpackingContext;

		/// <summary>
		///		The serializer to deserialize current item.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public MessagePackSerializer<TValue> Serializer;

		/// <summary>
		///		The items count to be unpacked.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int ItemsCount;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int Unpacked;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public string MemberName;

		/// <summary>
		///		The delegate which takes <see cref="UnpackingContext" /> and unpacked value, and then set the value to the context.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Action<TContext, TValue> Setter;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Type TargetObjectType;

		/// <summary>
		///		The delegate which refers direct reading. This field should be <c>null</c> when <see cref="Serializer" /> is specified.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Func<Unpacker, Type, string, CancellationToken, Task<TValue>> DirectRead;

		/// <summary>
		///		The nil implication of current item.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public NilImplication NilImplication;

		/// <summary>
		///		The token to monitor for cancellation requests. The default value is <see cref="P:CancellationToken.None" />.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public CancellationToken CancellationToken;
	}

#endif // FEATURE_TAP

	/// <summary>
	///		Represents parameters of <see cref="UnpackHelpers.UnpackNullableTypeValue{TContext, TValue}(ref UnpackNullableTypeValueParameters{TContext, TValue})"/> method.
	/// </summary>
	/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "This struct is not intended for value." )]
	public struct UnpackNullableTypeValueParameters<TContext, TValue>
		where TValue : struct
	{
		/// <summary>
		///		The unpacker.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Unpacker Unpacker;

		/// <summary>
		///		The context which will store deserialized value.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public TContext UnpackingContext;

		/// <summary>
		///		The serializer to deserialize current item.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public MessagePackSerializer<TValue?> Serializer;

		/// <summary>
		///		The items count to be unpacked.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int ItemsCount;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int Unpacked;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public string MemberName;

		/// <summary>
		///		The delegate which takes <see cref="UnpackingContext" /> and unpacked value, and then set the value to the context.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Action<TContext, TValue?> Setter;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Type TargetObjectType;

		/// <summary>
		///		The delegate which refers direct reading. This field should be <c>null</c> when <see cref="Serializer" /> is specified.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Func<Unpacker, Type, string, TValue?> DirectRead;

		/// <summary>
		///		The nil implication of current item.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public NilImplication NilImplication;
	}

#if FEATURE_TAP

	/// <summary>
	///		Represents parameters of <see cref="UnpackHelpers.UnpackNullableTypeValueAsync{TContext, TValue}(ref UnpackNullableTypeValueAsyncParameters{TContext, TValue})"/> method.
	/// </summary>
	/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "This struct is not intended for value." )]
	public struct UnpackNullableTypeValueAsyncParameters<TContext, TValue>
		where TValue : struct
	{
		/// <summary>
		///		The unpacker.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Unpacker Unpacker;

		/// <summary>
		///		The context which will store deserialized value.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public TContext UnpackingContext;

		/// <summary>
		///		The serializer to deserialize current item.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public MessagePackSerializer<TValue?> Serializer;

		/// <summary>
		///		The items count to be unpacked.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int ItemsCount;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int Unpacked;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public string MemberName;

		/// <summary>
		///		The delegate which takes <see cref="UnpackingContext" /> and unpacked value, and then set the value to the context.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Action<TContext, TValue?> Setter;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Type TargetObjectType;

		/// <summary>
		///		The delegate which refers direct reading. This field should be <c>null</c> when <see cref="Serializer" /> is specified.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Func<Unpacker, Type, string, CancellationToken, Task<TValue?>> DirectRead;

		/// <summary>
		///		The nil implication of current item.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public NilImplication NilImplication;

		/// <summary>
		///		The token to monitor for cancellation requests. The default value is <see cref="P:CancellationToken.None" />.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public CancellationToken CancellationToken;
	}

#endif // FEATURE_TAP

	/// <summary>
	///		Represents parameters of <see cref="UnpackHelpers.UnpackMessagePackObjectValue{TContext}(ref UnpackMessagePackObjectValueParameters{TContext})"/> method.
	/// </summary>
	/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "This struct is not intended for value." )]
	public struct UnpackMessagePackObjectValueParameters<TContext>
	{
		/// <summary>
		///		The unpacker.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Unpacker Unpacker;

		/// <summary>
		///		The context which will store deserialized value.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public TContext UnpackingContext;

		/// <summary>
		///		The serializer to deserialize current item.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public MessagePackSerializer<MessagePackObject> Serializer;

		/// <summary>
		///		The items count to be unpacked.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int ItemsCount;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int Unpacked;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public string MemberName;

		/// <summary>
		///		The delegate which takes <see cref="UnpackingContext" /> and unpacked value, and then set the value to the context.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Action<TContext, MessagePackObject> Setter;

		/// <summary>
		///		The nil implication of current item.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public NilImplication NilImplication;
	}

#if FEATURE_TAP

	/// <summary>
	///		Represents parameters of <see cref="UnpackHelpers.UnpackMessagePackObjectValueAsync{TContext}(ref UnpackMessagePackObjectValueAsyncParameters{TContext})"/> method.
	/// </summary>
	/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "This struct is not intended for value." )]
	public struct UnpackMessagePackObjectValueAsyncParameters<TContext>
	{
		/// <summary>
		///		The unpacker.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Unpacker Unpacker;

		/// <summary>
		///		The context which will store deserialized value.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public TContext UnpackingContext;

		/// <summary>
		///		The serializer to deserialize current item.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public MessagePackSerializer<MessagePackObject> Serializer;

		/// <summary>
		///		The items count to be unpacked.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int ItemsCount;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int Unpacked;

		/// <summary>
		///		The current unpacked count for debugging.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public string MemberName;

		/// <summary>
		///		The delegate which takes <see cref="UnpackingContext" /> and unpacked value, and then set the value to the context.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Action<TContext, MessagePackObject> Setter;

		/// <summary>
		///		The nil implication of current item.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public NilImplication NilImplication;

		/// <summary>
		///		The token to monitor for cancellation requests. The default value is <see cref="P:CancellationToken.None" />.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public CancellationToken CancellationToken;
	}

#endif // FEATURE_TAP

	/// <summary>
	///		Represents parameters of <see cref="UnpackHelpers.UnpackFromArray{TContext, TResult}(ref UnpackFromArrayParameters{TContext, TResult})"/> method.
	/// </summary>
	/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
	/// <typeparam name="TResult">The type of the unpacked object.</typeparam>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "This struct is not intended for value." )]
	public struct UnpackFromArrayParameters<TContext, TResult>
	{
		/// <summary>
		///		The unpacker.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Unpacker Unpacker;

		/// <summary>
		///		The context which will store deserialized value.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public TContext UnpackingContext;

		/// <summary>
		///		A delegate to the factory method which creates the result from the context.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Func<TContext, TResult> Factory;
		/// <summary>
		///		The names of the members for pretty exception message.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public IList<string> ItemNames;

		/// <summary>
		///		Delegates each ones unpack single member in order.
		///		The 1st argument will be <see cref="Unpacker"/>, 2nd argument will be <see cref="UnpackingContext"/>,
		///		3rd argument is index of current item, and 4th argument is total items count in the array or map stream.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public IList<Action<Unpacker, TContext, int, int>> Operations;
	}

#if FEATURE_TAP

	/// <summary>
	///		Represents parameters of <see cref="UnpackHelpers.UnpackFromArrayAsync{TContext, TResult}(ref UnpackFromArrayAsyncParameters{TContext, TResult})"/> method.
	/// </summary>
	/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
	/// <typeparam name="TResult">The type of the unpacked object.</typeparam>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "This struct is not intended for value." )]
	public struct UnpackFromArrayAsyncParameters<TContext, TResult>
	{
		/// <summary>
		///		The unpacker.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Unpacker Unpacker;

		/// <summary>
		///		The context which will store deserialized value.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public TContext UnpackingContext;

		/// <summary>
		///		A delegate to the factory method which creates the result from the context.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Func<TContext, TResult> Factory;
		/// <summary>
		///		The names of the members for pretty exception message.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public IList<string> ItemNames;

		/// <summary>
		///		Delegates each ones unpack single member in order.
		///		The 1st argument will be <see cref="Unpacker"/>, 2nd argument will be <see cref="UnpackingContext"/>,
		///		3rd argument is index of current item, and 4th argument is total items count in the array or map stream.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public IList<Func<Unpacker, TContext, int, int, CancellationToken, Task>> Operations;

		/// <summary>
		///		The token to monitor for cancellation requests. The default value is <see cref="P:CancellationToken.None" />.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public CancellationToken CancellationToken;
	}

#endif // FEATURE_TAP

	/// <summary>
	///		Represents parameters of <see cref="UnpackHelpers.UnpackFromMap{TContext, TResult}(ref UnpackFromMapParameters{TContext, TResult})"/> method.
	/// </summary>
	/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
	/// <typeparam name="TResult">The type of the unpacked object.</typeparam>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "This struct is not intended for value." )]
	public struct UnpackFromMapParameters<TContext, TResult>
	{
		/// <summary>
		///		The unpacker.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Unpacker Unpacker;

		/// <summary>
		///		The context which will store deserialized value.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public TContext UnpackingContext;

		/// <summary>
		///		A delegate to the factory method which creates the result from the context.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Func<TContext, TResult> Factory;

		/// <summary>
		///		Delegates each ones unpack single member in order.
		///		The key of this dictionary must be member name.
		///		The 1st argument will be <see cref="Unpacker"/>, 2nd argument will be <see cref="UnpackingContext"/>,
		///		3rd argument is index of current item, and 4th argument is total items count in the array or map stream.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public IDictionary<string, Action<Unpacker, TContext, int, int>> Operations;
	}

#if FEATURE_TAP

	/// <summary>
	///		Represents parameters of <see cref="UnpackHelpers.UnpackFromMapAsync{TContext, TResult}(ref UnpackFromMapAsyncParameters{TContext, TResult})"/> method.
	/// </summary>
	/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
	/// <typeparam name="TResult">The type of the unpacked object.</typeparam>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "This struct is not intended for value." )]
	public struct UnpackFromMapAsyncParameters<TContext, TResult>
	{
		/// <summary>
		///		The unpacker.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Unpacker Unpacker;

		/// <summary>
		///		The context which will store deserialized value.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public TContext UnpackingContext;

		/// <summary>
		///		A delegate to the factory method which creates the result from the context.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Func<TContext, TResult> Factory;

		/// <summary>
		///		Delegates each ones unpack single member in order.
		///		The key of this dictionary must be member name.
		///		The 1st argument will be <see cref="Unpacker"/>, 2nd argument will be <see cref="UnpackingContext"/>,
		///		3rd argument is index of current item, and 4th argument is total items count in the array or map stream.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public IDictionary<string, Func<Unpacker, TContext, int, int, CancellationToken, Task>> Operations;

		/// <summary>
		///		The token to monitor for cancellation requests. The default value is <see cref="P:CancellationToken.None" />.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public CancellationToken CancellationToken;
	}

#endif // FEATURE_TAP


	/// <summary>
	///		Represents parameters of <see cref="UnpackHelpers.UnpackCollection{T}(ref UnpackCollectionParameters{T})"/> method.
	/// </summary>
	/// <typeparam name="T">The type of the collection to be unpacked.</typeparam>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "This struct is not intended for value." )]
	public struct UnpackCollectionParameters<T>
	{
		/// <summary>
		///		The unpacker.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Unpacker Unpacker;

		/// <summary>
		///		The items count to be unpacked.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int ItemsCount;

		/// <summary>
		///		The collection instance to be added unpacked items.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public T Collection;

		/// <summary>
		///		A delegate to the bulk operation (typically UnpackToCore call). 
		///		The 1st argument will be <see cref="Unpacker"/>, 2nd argument will be <see cref="Collection"/>,
		///		and 3rd argument will be <see cref="ItemsCount"/>.
		///		If this field is <c>null</c>, <see cref="EachOperation"/> will be used.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Action<Unpacker, T, int> BulkOperation;

		/// <summary>
		///		A delegate to the operation for each items, which typically unpack value and append it to the <see cref="Collection"/>.
		///		The 1st argument will be <see cref="Unpacker"/>, 2nd argument will be <see cref="Collection"/>,
		///		and 3rd argument will be index of the current item.
		///		If <see cref="BulkOperation"/> field is not <c>null</c>, this field will be ignored.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Action<Unpacker, T, int, int> EachOperation;
	}

#if FEATURE_TAP

	/// <summary>
	///		Represents parameters of <see cref="UnpackHelpers.UnpackCollectionAsync{T}(ref UnpackCollectionAsyncParameters{T})"/> method.
	/// </summary>
	/// <typeparam name="T">The type of the collection to be unpacked.</typeparam>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "This struct is not intended for value." )]
	public struct UnpackCollectionAsyncParameters<T>
	{
		/// <summary>
		///		The unpacker.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Unpacker Unpacker;

		/// <summary>
		///		The items count to be unpacked.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public int ItemsCount;

		/// <summary>
		///		The collection instance to be added unpacked items.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public T Collection;

		/// <summary>
		///		A delegate to the bulk operation (typically UnpackToCore call). 
		///		The 1st argument will be <see cref="Unpacker"/>, 2nd argument will be <see cref="Collection"/>,
		///		and 3rd argument will be <see cref="ItemsCount"/>.
		///		If this field is <c>null</c>, <see cref="EachOperation"/> will be used.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Func<Unpacker, T, int, CancellationToken, Task> BulkOperation;

		/// <summary>
		///		A delegate to the operation for each items, which typically unpack value and append it to the <see cref="Collection"/>.
		///		The 1st argument will be <see cref="Unpacker"/>, 2nd argument will be <see cref="Collection"/>,
		///		and 3rd argument will be index of the current item.
		///		If <see cref="BulkOperation"/> field is not <c>null</c>, this field will be ignored.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By Design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public Func<Unpacker, T, int, int, CancellationToken, Task> EachOperation;

		/// <summary>
		///		The token to monitor for cancellation requests. The default value is <see cref="P:CancellationToken.None" />.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Performance critical effectively internal structure for ref access.")]
		public CancellationToken CancellationToken;
	}

#endif // FEATURE_TAP
}
