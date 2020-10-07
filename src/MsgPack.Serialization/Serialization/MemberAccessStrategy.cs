// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines member access strategy for object serialization.
	/// </summary>
	internal enum MemberAccessStrategy
	{
		/// <summary>
		///		Skip the member in serialization.
		/// </summary>
		Skip = 0,

		/// <summary>
		///		Use direct access because it is public.
		/// </summary>
		Direct,

		/// <summary>
		///		Use Core CLR's <c>System.Runtime.CompilerServices.IgnoreAccessChecksToAttribute</c> to skip access check verification for non-public member.
		/// </summary>
		DirectWithIgnoreAccessChecksToAttribute,

		/// <summary>
		///		Use delegate which wraps <see cref="MethodInfo"/> to access non-public member assuming that current thread has enough code access security permission.
		/// </summary>
		ViaDelegate,

		/// <summary>
		///		Use reflection serializer instead of any code generation.
		/// </summary>
		WithReflection,

		/// <summary>
		///		Use constructor for deserialization because no setters exist.
		/// </summary>
		ViaConstrutor,

		/// <summary>
		///		There are no setters and constructors, so only asymmetric serializer can be generated if allowed.
		/// </summary>
		WillBeAsymmetric,

		/// <summary>
		///		Use addition to existing collection instead of setting collection itself because the member is read only and the type is mutable collection.
		/// </summary>
		CollectionAdd
	}

}
