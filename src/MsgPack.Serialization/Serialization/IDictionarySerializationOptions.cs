// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Provides dictionary (map) serialization options.
	/// </summary>
	/// <remarks>
	///		These options affect both of object serialization with <see cref="SerializationMethod.Map"/> and dictionary types serialization.
	/// </remarks>
	internal interface IDictionarySerializationOptions 
	{
		/// <summary>
		///		Gets the value indicating whether omit key-value entry itself when the value is <c>null</c>.
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
		bool OmitsNullEntries { get; }

		/// <summary>
		///		Gets the key name handler which enables dictionary key name customization.
		/// </summary>
		/// <value>
		///		The key name handler which enables dictionary key name customization.
		/// </value>
		/// <see cref="DictionaryKeyTransformers"/>
		Func<string, string> KeyTransformer { get; }
	}
}
