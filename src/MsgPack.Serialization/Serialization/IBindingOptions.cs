// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines internal interface of binding options.
	/// </summary>
	internal interface IBindingOptions
	{
		/// <summary>
		///		Gets a value which indicates whether non-public member access from serializer should be 
		/// </summary>
		/// <value>
		///		<c>true</c> if privileged access for non public types or members is disabled; <c>false</c>, otherwise.
		/// </value>
		bool IsPrivilegedAccessDisabled { get; }

		/// <summary>
		///		Gets the value which indicates whether non-public member access can be assumed to be enabled relying on <see cref="System.Runtime.CompilerServices.InternalsVisibleToAttribute"/>.
		/// </summary>
		/// <value>
		///		<c>true</c> if non public types or members is non-public member access can be assumed to be enabled relying on <see cref="System.Runtime.CompilerServices.InternalsVisibleToAttribute"/>; <c>false</c>, otherwise.
		/// </value>
		/// <remarks>
		///		If this property is <c>true</c>, genenrators emit code with normal access even though the member or type is not public.
		///		Note that the generated serializer will belong to the assembly which has <see cref="RuntimeCodeGenerationAssemblyName"/> in runtime code generation,
		///		so the assembly which declares target type must be marked with <see cref="System.Runtime.CompilerServices.InternalsVisibleToAttribute"/> for the <see cref="RuntimeCodeGenerationAssemblyName"/> without any public key.
		///		Also, for source code generation, the assembly which declares target type must be marked with <see cref="System.Runtime.CompilerServices.InternalsVisibleToAttribute"/> for the assembly name of compiled serializer without any public key.
		///		In this case, the assembly name will be specified via compiler option (or specified with appropriate settings in <c>.csproj</c> or to be its file name).
		/// </remarks>
		bool AssumesInternalsVisibleTo { get; }

		/// <summary>
		///		Get the collection which contains ignoring member names of the target type.
		/// </summary>
		/// <param name="targetType">The target type.</param>
		/// <returns>The collection which contains ignoring member names of the target type. Empty for no members should be excluded.</returns>
		IEnumerable<string> GetIgnoringMembers(Type targetType);
	}

}
