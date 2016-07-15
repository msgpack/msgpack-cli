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

#if NETFX_35 || UNITY || SILVERLIGHT
#define NO_THREADING_VOLATILE
#endif // NETFX_35 || UNITY || SILVERLIGHT

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
	/// </remarks>
	public sealed class DictionarySerlaizationOptions
	{
#if NO_THREADING_VOLATILE
		private volatile Func<string, string> _keyNameHandler;
#else
		private Func<string, string> _keyNameHandler;
#endif


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
#if NO_THREADING_VOLATILE
				return this._keyNameHandler;
#else
				return Volatile.Read( ref this._keyNameHandler );
#endif
			}
			set
			{
#if NO_THREADING_VOLATILE
				this._keyNameHandler = value;
#else
				Volatile.Write( ref this._keyNameHandler, value );
#endif
			}
		}

		internal Func<string, string> SafeKeyTransformer
		{
			get { return this.KeyTransformer ?? DictionaryKeyTransformers.AsIs; }
		} 

		/// <summary>
		///		Initializes a new instance of the <see cref="DictionarySerlaizationOptions"/> class.
		/// </summary>
		public DictionarySerlaizationOptions() { }
	}
}
