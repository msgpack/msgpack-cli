// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
#warning TODO: Write description that this option only affect object serialization!
	/// <summary>
	///		Builder object for dictionary (map) serialization options.
	/// </summary>
	public sealed class DictionarySerializationOptionsBuilder
	{
		/// <summary>
		///		Gets a value indicating whether omit key-value entry itself when the value is <c>null</c>.
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
		public bool OmitsNullEntries { get; private set; }

		/// <summary>
		///		Gets a key name handler which enables dictionary key name customization.
		/// </summary>
		/// <value>
		///		A key name handler which enables dictionary key name customization.
		///		The default value is <c>null</c>, which indicates that key name is not transformed.
		/// </value>
		/// <see cref="DictionaryKeyTransformers"/>
		public Func<string, string>? KeyTransformer { get; private set; }

		/// <summary>
		///		Indicates that emitting key-value entry itself even when the value is <c>null</c>.
		/// </summary>
		/// <returns>This <see cref="DictionarySerializationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="OmitsNullEntries"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public DictionarySerializationOptionsBuilder EmitNullEntries()
		{
			this.OmitsNullEntries = false;
			return this;
		}

		/// <summary>
		///		Indicates that omitting key-value entry itself when the value is <c>null</c>.
		/// </summary>
		/// <returns>This <see cref="DictionarySerializationOptionsBuilder"/> instance.</returns>
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
		public DictionarySerializationOptionsBuilder OmitNullEntries()
		{
			this.OmitsNullEntries = true;
			return this;
		}


		/// <summary>
		///		Sets <see cref="KeyTransformer"/> property to default (no name transformation).
		/// </summary>
		/// <returns>This <see cref="DictionarySerializationOptionsBuilder"/> instance.</returns>
		public DictionarySerializationOptionsBuilder UseDefaultKeyTransformer()
		{
			this.KeyTransformer = null;
			return this;
		}

		/// <summary>
		///		Sets <see cref="KeyTransformer"/> property with specified transformer delegate.
		/// </summary>
		/// <param name="value">
		///		The delegate which accept original key and then returns serialized key.
		/// </param>
		/// <returns>This <see cref="DictionarySerializationOptionsBuilder"/> instance.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="value"/> is <c>null</c>.
		/// </exception>
		public DictionarySerializationOptionsBuilder UseCustomKeyTransformer(Func<string, string> value)
		{
			this.KeyTransformer = Ensure.NotNull(value);
			return this;
		}

		/// <summary>
		///		Initializes a new instance of <see cref="DictionarySerializationOptionsBuilder" /> object.
		/// </summary>
		public DictionarySerializationOptionsBuilder() { }
	}
}
