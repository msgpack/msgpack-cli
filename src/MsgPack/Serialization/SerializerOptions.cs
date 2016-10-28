#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2012-2016 FUJIWARA, Yusuke
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
#define AOT
#endif

using System;
using System.Threading;
#if NETFX_CORE || UNITY || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // NETFX_CORE || UNITY || NETSTANDARD1_1

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines options for serializer generation.
	/// </summary>
	public sealed class SerializerOptions
	{
#if AOT || SILVERLIGHT
		private int _emitterFlavor = ( int )EmitterFlavor.ReflectionBased;
#else
		private int _emitterFlavor = ( int )EmitterFlavor.FieldBased;
#endif

		/// <summary>
		///		Gets or sets the <see cref="EmitterFlavor"/>.
		/// </summary>
		/// <value>
		///		The <see cref="EmitterFlavor"/>
		/// </value>
		/// <remarks>
		///		For testing purposes.
		/// </remarks>
		internal EmitterFlavor EmitterFlavor
		{
			get { return ( EmitterFlavor )Volatile.Read( ref this._emitterFlavor ); }
			set { Volatile.Write( ref this._emitterFlavor, ( int )value ); }
		}

#if !AOT

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
				Contract.Ensures( Enum.IsDefined( typeof( SerializationMethod ), Contract.Result<SerializationMethodGeneratorOption>() ) );

				return ( SerializationMethodGeneratorOption )Volatile.Read( ref this._generatorOption );
			}
			set
			{
				switch ( value )
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
						throw new ArgumentOutOfRangeException( "value" );
					}
				}

				Contract.EndContractBlock();

				Volatile.Write( ref this._generatorOption, ( int )value );
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
#endif // !AOT

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
#if !AOT
			this.GeneratorOption = SerializationMethodGeneratorOption.Fast;
#endif // !AOT
		}
	}
}
