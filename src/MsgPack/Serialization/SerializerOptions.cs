#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2012-2015 FUJIWARA, Yusuke
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
using System.Threading;
#if !UNITY
#if CORE_CLR
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif
#endif // !UNITY

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines options for serializer generation.
	/// </summary>
	public sealed class SerializerOptions
	{
#if XAMIOS || XAMDROID || UNITY_IPHONE || UNITY_ANDROID
		private int _emitterFlavor = ( int )EmitterFlavor.ReflectionBased;
#elif !NETFX_CORE
		private int _emitterFlavor = ( int )EmitterFlavor.FieldBased;
#else
		private int _emitterFlavor = ( int )EmitterFlavor.ExpressionBased;
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

#if !XAMIOS && !XAMDROID && !UNITY

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
					case SerializationMethodGeneratorOption.CanDump:
#endif
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

#if NETFX_35 || UNITY || SILVERLIGHT
		private volatile bool _isRuntimeGenerationDisabled;
#else
		private bool _isRuntimeGenerationDisabled;
#endif // NETFX_35 || UNITY || SILVERLIGHT

		/// <summary>
		///		Gets or sets a value indicating whether runtime generation is disabled or not.
		/// </summary>
		/// <value>
		///		<c>true</c> if runtime generation is disabled; otherwise, <c>false</c>.
		/// </value>
		internal bool IsRuntimeGenerationDisabled
		{
			get
			{
#if NETFX_35 || UNITY || SILVERLIGHT
				return this._isRuntimeGenerationDisabled;
#else
				return Volatile.Read( ref this._isRuntimeGenerationDisabled );
#endif // NETFX_35 || UNITY || SILVERLIGHT
			}
			set
			{
#if NETFX_35 || UNITY || SILVERLIGHT
				this._isRuntimeGenerationDisabled = value;
#else
				Volatile.Write( ref this._isRuntimeGenerationDisabled, value );
#endif // NETFX_35 || UNITY || SILVERLIGHT
			}
		}
#endif // !XAMIOS && !XAMDROID && !UNITY

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
#warning TODO: true
			this.WithAsync = false;
#endif // FEATURE_TAP
#if !XAMIOS && !XAMDROID && !UNITY
			this.GeneratorOption = SerializationMethodGeneratorOption.Fast;
#endif // !XAMIOS && !XAMDROID && !UNITY
		}
	}
}
