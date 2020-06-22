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

using System;
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Threading;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines enum serialization options.
	/// </summary>
	public sealed class EnumSerializationOptions
	{
		private int _serializationMethod;

		/// <summary>
		///		Gets or sets the <see cref="EnumSerializationMethod"/> to determine default serialization strategy of enum types.
		/// </summary>
		/// <value>
		///		The <see cref="EnumSerializationMethod"/> to determine default serialization strategy of enum types.
		/// </value>
		/// <exception cref="ArgumentOutOfRangeException">The setting value is invalid as <see cref="EnumSerializationMethod"/> enum.</exception>
		/// <remarks>
		///		A serialization strategy for specific <strong>member</strong> is determined as following:
		///		<list type="numeric">
		///			<item>If the member is marked with <see cref="MessagePackEnumMemberAttribute"/> and its value is not <see cref="EnumMemberSerializationMethod.Default"/>, then it will be used.</item>
		///			<item>Otherwise, if the enum type itself is marked with <see cref="MessagePackEnumAttribute"/>, then it will be used.</item>
		///			<item>Otherwise, the value of this property will be used.</item>
		/// 	</list>
		///		Note that the default value of this property is <see cref="T:EnumSerializationMethod.ByName"/>, it is not size efficient but tolerant to unexpected enum definition change.
		/// </remarks>
		public EnumSerializationMethod SerializationMethod
		{
			get
			{
#if DEBUG
				Contract.Ensures( Enum.IsDefined( typeof( EnumSerializationMethod ), Contract.Result<EnumSerializationMethod>() ) );
#endif // DEBUG

				return (EnumSerializationMethod)Volatile.Read(ref this._serializationMethod);
			}
			set
			{
				switch (value)
				{
					case EnumSerializationMethod.ByName:
					case EnumSerializationMethod.ByUnderlyingValue:
					{
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException("value");
					}
				}

				Contract.EndContractBlock();

				Volatile.Write(ref this._serializationMethod, (int)value);
			}
		}


#if !FEATURE_CONCURRENT
		private volatile Func<string, string> _nameTransformer;
#else
		private Func<string, string> _nameTransformer;
#endif // !FEATURE_CONCURRENT

		/// <summary>
		///		Gets or sets the key name handler which enables dictionary key name customization.
		/// </summary>
		/// <value>
		///		The key name handler which enables dictionary key name customization.
		///		The default value is <c>null</c>, which indicates that key name is not transformed.
		/// </value>
		/// <see cref="EnumNameTransformers"/>
		public Func<string, string> NameTransformer
		{
			get
			{
#if !FEATURE_CONCURRENT
				return this._nameTransformer;
#else
				return Volatile.Read( ref this._nameTransformer );
#endif // !FEATURE_CONCURRENT
			}
			set
			{
#if !FEATURE_CONCURRENT
				this._nameTransformer = value;
#else
				Volatile.Write( ref this._nameTransformer, value );
#endif // !FEATURE_CONCURRENT
			}
		}

		internal Func<string, string> SafeNameTransformer
		{
			get { return this.NameTransformer ?? KeyNameTransformers.AsIs; }
		}
	}
}
