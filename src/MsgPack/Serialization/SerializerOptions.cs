// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.ComponentModel;
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
#if NETSTANDARD1_3 || NETSTANDARD2_0
using System.Reflection;
using System.Reflection.Emit;
#endif // NETSTANDARD1_3 || NETSTANDARD2_0
using System.Runtime.CompilerServices;
using System.Threading;
#if (NETSTANDARD1_3 || NETSTANDARD2_0) && !WINDOWS_UWP
using MsgPack.Serialization.EmittingSerializers;
#endif // ( NETSTANDARD1_3 || NETSTANDARD2_0 ) && !WINDOWS_UWP

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines options for serializer generation.
	/// </summary>
	public sealed class SerializerOptions
	{
#if !UNITY

		private int _generatorOption;

		/// <summary>
		///		Gets or sets the <see cref="SerializationMethodGeneratorOption"/> to control code generation.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">The setting value is invalid as <see cref="SerializationMethod"/> enum.</exception>
		/// <value>
		///		The <see cref="SerializationMethodGeneratorOption"/>.
		/// </value>
		public SerializationMethodGeneratorOption GeneratorOption
		{
			get
			{
				Contract.Ensures(Enum.IsDefined(typeof(SerializationMethod), Contract.Result<SerializationMethodGeneratorOption>()));

				return (SerializationMethodGeneratorOption)Volatile.Read(ref this._generatorOption);
			}
			set
			{
				switch (value)
				{
					case SerializationMethodGeneratorOption.Fast:
#if !SILVERLIGHT
					case SerializationMethodGeneratorOption.CanCollect:
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
					case SerializationMethodGeneratorOption.CanDump:
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
#endif // !SILVERLIGHT
					{
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException("value");
					}
				}

				Contract.EndContractBlock();

				Volatile.Write(ref this._generatorOption, (int)value);
			}
		}

#if !FEATURE_CONCURRENT
		private volatile bool _isRuntimeGenerationDisabled;
#else
		private bool _isRuntimeGenerationDisabled;
#endif // !FEATURE_CONCURRENT

		/// <summary>
		///		Gets or sets a value indicating whether runtime generation should be disabled or not.
		/// </summary>
		/// <value>
		///		<c>true</c> if runtime generation is disabled; otherwise, <c>false</c>. Defaults to <c>false</c>.
		/// </value>
		public bool DisableRuntimeCodeGeneration
		{
			get
			{
#if !FEATURE_CONCURRENT
				return this._isRuntimeGenerationDisabled;
#else
				return Volatile.Read( ref this._isRuntimeGenerationDisabled );
#endif // !FEATURE_CONCURRENT
			}
			set
			{
#if !FEATURE_CONCURRENT
				this._isRuntimeGenerationDisabled = value;
#else
				Volatile.Write( ref this._isRuntimeGenerationDisabled, value );
#endif // !FEATURE_CONCURRENT
			}
		}

		internal static readonly bool CanEmit = DetermineCanEmit();

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static bool DetermineCanEmit()
		{
#if (NETSTANDARD1_3 || NETSTANDARD2_0) && !WINDOWS_UWP
			try
			{
				return DetermineCanEmitCore();
			}
			catch
			{
				return false;
			}
#elif NETFX_CORE || UNITY
			return false;
#else
			// Desktop etc.
			return true;
#endif
		}

#if (NETSTANDARD1_3 || NETSTANDARD2_0) && !WINDOWS_UWP

		[MethodImpl( MethodImplOptions.NoInlining )]
		private static bool DetermineCanEmitCore()
		{
			return SerializationMethodGeneratorManager.Fast != null;
		}

#endif // ( NETSTANDARD1_3 || NETSTANDARD2_0 ) && !WINDOWS_UWP

		internal bool CanRuntimeCodeGeneration
		{
			get { return CanEmit && !this.DisableRuntimeCodeGeneration; }
		}

#endif // !UNITY

#if !FEATURE_CONCURRENT
		private volatile bool _isNonPublicAccessDisabled;
#else
		private bool _isNonPublicAccessDisabled;
#endif // !FEATURE_CONCURRENT

		/// <summary>
		///		Gets or sets a value indicating whether generated and/or reflection serializers should not access non public members via privileged reflection.
		/// </summary>
		/// <value>
		///		<c>true</c> if privileged reflection access is disabled; otherwise, <c>false</c>. Defaults to <c>false</c>.
		/// </value>
		/// <remarks>
		///		The privileged reflection means:
		///		<list type="bullet">
		///			<item>Access for non-public fields or property accessors via reflection. This operation requires <c>ReflectionPermission</c> of <c>MemberAccess</c> or <c>RestrictedMemberAccess</c>.</item>
		///			<item>Writing values for init only fields via reflection. This operation requires <c>SecurityPermission</c> of <c>SerializationFormatter</c>.</item>
		///		</list>
		///		If the program run on non-privileged Silverlight environment or restricted desktop CLR,
		///		serialization and deserialization should fail with <c>SecurityException</c>.
		/// </remarks>
		public bool DisablePrivilegedAccess
		{
			get
			{
#if !FEATURE_CONCURRENT
				return this._isNonPublicAccessDisabled;
#else
				return Volatile.Read( ref this._isNonPublicAccessDisabled );
#endif // !FEATURE_CONCURRENT
			}
			set
			{
#if !FEATURE_CONCURRENT
				this._isNonPublicAccessDisabled = value;
#else
				Volatile.Write( ref this._isNonPublicAccessDisabled, value );
#endif // !FEATURE_CONCURRENT
			}
		}

#if FEATURE_TAP

		private bool _withAsync;

		/// <summary>
		///		Gets or sets a value indicating whether generated serializers will override async methods or not.
		/// </summary>
		/// <value>
		///		<c>true</c> if generated serializers will override async methods; otherwise, <c>false</c>.
		///		Default is <c>true</c>.
		/// </value>
		public bool WithAsync
		{
			get { return Volatile.Read( ref this._withAsync ); }
			set { Volatile.Write( ref this._withAsync, value ); }
		}

#endif // FEATURE_TAP

		// TODO: SkipPackingNullMemberForMap

		internal SerializerOptions()
		{
#if FEATURE_TAP
			this.WithAsync = true;
#endif // FEATURE_TAP
#if !UNITY
			this.GeneratorOption = SerializationMethodGeneratorOption.Fast;
#endif // !UNITY
		}
	}
}
