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
using System.Threading;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines dictionary (map) based serialization options.
	/// </summary>
	/// <remarks>
	///		These options do NOT affect serialization of <see cref="System.Collections.IDictionary"/> 
	///		and <see cref="System.Collections.Generic.IDictionary{TKey, TValue}"/>.
	///		The option only affect dictionary (map) based serialization which can be enabled via <see cref="SerializationContext.SerializationMethod"/>.
	/// </remarks>
	public sealed class DictionarySerializationOptions
	{
#if !FEATURE_CONCURRENT
		private volatile bool _omitNullEntry;
#else
		private bool _omitNullEntry;
#endif // !FEATURE_CONCURRENT

		/// <summary>
		///		Gets or sets a value indicating whether omit key-value entry itself when the value is <c>null</c>.
		/// </summary>
		/// <value>
		///		<c>true</c> if key-value entry itself when the value is <c>null</c>; otherwise, <c>false</c>.
		///		The default is <c>false</c>.
		/// </value>
		/// <remarks>
		///		When the value is <c>false</c>, null value entry is emitted as following (using JSON syntax for easy visualization):
		///		<code><pre>
		///		{ "Foo": null }
		///		</pre></code>
		///		else, the value is <c>true</c>, null value entry is ommitted as following:
		///		<code><pre>
		///		{}
		///		</pre></code>
		/// </remarks>
		public bool OmitNullEntry
		{
			get
			{
#if !FEATURE_CONCURRENT
				return this._omitNullEntry;
#else
				return Volatile.Read( ref this._omitNullEntry );
#endif // !FEATURE_CONCURRENT
			}
			set
			{
#if !FEATURE_CONCURRENT
				this._omitNullEntry = value;
#else
				Volatile.Write( ref this._omitNullEntry, value );
#endif // !FEATURE_CONCURRENT
			}
		}

#if !FEATURE_CONCURRENT
		private volatile Func<string, string> _keyNameHandler;
#else
		private Func<string, string> _keyTransformer;
#endif // !FEATURE_CONCURRENT


		/// <summary>
		///		Gets or sets the key name handler which enables dictionary key name customization.
		/// </summary>
		/// <value>
		///		The key name handler which enables dictionary key name customization.
		///		The default value is <c>null</c>, which indicates that key name is not transformed.
		/// </value>
		/// <see cref="DictionaryKeyTransformers"/>
		public Func<string, string> KeyTransformer
		{
			get
			{
#if !FEATURE_CONCURRENT
				return this._keyNameHandler;
#else
				return Volatile.Read( ref this._keyTransformer );
#endif // !FEATURE_CONCURRENT
			}
			set
			{
#if !FEATURE_CONCURRENT
				this._keyNameHandler = value;
#else
				Volatile.Write( ref this._keyTransformer, value );
#endif // !FEATURE_CONCURRENT
			}
		}

		internal Func<string, string> SafeKeyTransformer
		{
			get { return this.KeyTransformer ?? KeyNameTransformers.AsIs; }
		} 

		/// <summary>
		///		Initializes a new instance of the <see cref="DictionarySerializationOptions"/> class.
		/// </summary>
		public DictionarySerializationOptions() { }
	}
}
