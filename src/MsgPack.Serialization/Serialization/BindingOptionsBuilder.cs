// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace MsgPack.Serialization
{
	/// <summary>
	///		A builder object to configure binding options of target members.
	/// </summary>
	public sealed class BindingOptionsBuilder
	{
		internal static BindingOptionsBuilder Default { get; } = new BindingOptionsBuilder();

		internal IDictionary<Type, IEnumerable<string>> IgnoringMembers { get; } = new Dictionary<Type, IEnumerable<string>>();

		/// <summary>
		///		Gets the value which indicates whether non-public member access can be assumed to be enabled relying on <see cref="System.Runtime.CompilerServices.InternalsVisibleToAttribute"/>.
		/// </summary>
		/// <value>
		///		<c>true</c> if non public types or members is non-public member access can be assumed to be enabled relying on <see cref="System.Runtime.CompilerServices.InternalsVisibleToAttribute"/>; <c>false</c>, otherwise.
		/// </value>
		/// <remarks>
		///		If this property is <c>true</c>, genenrators emit code with normal access even though the member or type is not public.
		///		Note that the generated serializer will belong to the assembly which has <see cref="SerializerGenerationOptionsBuilder.RuntimeCodeGenerationAssemblyName"/> in runtime code generation,
		///		so the assembly which declares target type must be marked with <see cref="System.Runtime.CompilerServices.InternalsVisibleToAttribute"/> for the <see cref="RuntimeCodeGenerationAssemblyName"/> without any public key.
		///		Also, for source code generation, the assembly which declares target type must be marked with <see cref="System.Runtime.CompilerServices.InternalsVisibleToAttribute"/> for the assembly name of compiled serializer without any public key.
		///		In this case, the assembly name will be specified via compiler option (or specified with appropriate settings in <c>.csproj</c> or to be its file name).
		/// </remarks>
		/// <seealso cref="SerializerGenerationOptionsBuilder.RuntimeCodeGenerationAssemblyName"/>
		public bool AssumesInternalsVisibleTo { get; private set; }

		/// <summary>
		///		Gets a value which indicates whether non-public member access from serializer should be 
		/// </summary>
		/// <value>
		///		<c>true</c> if privileged access for non public types or members is disabled; <c>false</c>, otherwise.
		/// </value>
		public bool IsPrivilegedAccessDisabled { get; private set; } = true;

		/// <summary>
		///		Initializes a new instance of <see cref="BindingOptionsBuilder"/> object.
		/// </summary>
		public BindingOptionsBuilder() { }

		/// <summary>
		///		Sets ignoring specified type's members.
		/// </summary>
		/// <param name="type">Target type.</param>
		/// <param name="memberNames">Names of members to be ignored. The values are case sensitive. <c>null</c> or empty items will be ignored.</param>
		/// <returns>This <see cref="BindingOptionsBuilder"/> instance.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
		public BindingOptionsBuilder IgnoreMembers(Type type, params string[] memberNames)
			=> this.IgnoreMembers(type, memberNames as IEnumerable<string>);

		/// <summary>
		///		Sets ignoring specified type's members.
		/// </summary>
		/// <param name="type">Target type.</param>
		/// <param name="memberNames">Names of members to be ignored. The values are case sensitive. <c>null</c> or empty items will be ignored.</param>
		/// <returns>This <see cref="BindingOptionsBuilder"/> instance.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
		public BindingOptionsBuilder IgnoreMembers(Type type, IEnumerable<string> memberNames)
		{
			this.IgnoringMembers[Ensure.NotNull(type)] = memberNames?.Where(m => !String.IsNullOrEmpty(m)).ToArray() ?? Enumerable.Empty<string>();
			return this;
		}

		/// <summary>
		///		Indicates which indicates whether non-public member access from serializer should be 
		/// </summary>
		/// <returns>This <see cref="BindingOptionsBuilder"/> instance.</returns>
		public BindingOptionsBuilder EnablePrivilegedAccess()
		{
			this.IsPrivilegedAccessDisabled = true;
			return this;
		}

		public BindingOptionsBuilder DisablePrivilegedAccess()
		{
			this.IsPrivilegedAccessDisabled = false;
			return this;
		}

		public BindingOptionsBuilder WithoutAssumingInternalsVisibleTo()
		{
			this.AssumesInternalsVisibleTo = false;
			return this;
		}

		public BindingOptionsBuilder WithAssumingInternalsVisibleTo()
		{
			this.AssumesInternalsVisibleTo = true;
			return this;
		}
	}
}
