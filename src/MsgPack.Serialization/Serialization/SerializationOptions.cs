// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Text;
using MsgPack.Codecs;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines each serialization operations level options. This object is immutable.
	/// </summary>
	/// <seealso cref="SerializationOptionsBuilder"/>
	public sealed class SerializationOptions
	{
		/// <summary>
		///		Gets the default instance.
		/// </summary>
		/// <value>The default instance.</value>
		public static SerializationOptions Default { get; } = new SerializationOptionsBuilder().Create();

		/// <summary>
		///		Gets the maximum depth of serialized object tree.
		/// </summary>
		/// <value>
		///		The maximum depth of serialized object tree. Default is <c>100</c>.
		/// </value>
		public int MaxDepth { get; }

		/// <summary>
		///		Gets the default string encoding for string members or map keys which are not specified explicitly.
		/// </summary>
		/// <value>
		///		The default string encoding for string members or map keys which are not specified explicitly.
		///		Default value is <c>null</c>, which means using default encoding of underlying codec.
		/// </value>
		public Encoding? DefaultStringEncoding { get; }

		// (Specified by Attribute [future]) > Type Default > This > FormatDefault
		/// <summary>
		///		Gets the preferred <see cref="SerializationMethod"/> which will be used members which serialization methods are not derived from their attributes nor member types.  
		/// </summary>
		/// <value>
		///		The preferred <see cref="SerializationMethod"/> which will be used members which serialization methods are not derived from their attributes nor member types.  
		///		Default is <c>null</c>, which means using default method of underlying codec.
		/// </value>
		public SerializationMethod? PreferredSerializationMethod { get; }

		/// <summary>
		///		Gets a default value of object serialization method for specified codec.
		/// </summary>
		/// <param name="codecFeatures">A feature of the codec.</param>
		/// <returns>
		///		<see cref="SerializationMethod"/> for specified codec features.
		///		If an any value is set via builder object, the value will be returned,
		///		else returns <see cref="CodecFeatures.PreferredObjectSerializationMethod"/>.
		/// </returns>
		public SerializationMethod GetDefaultObjectSerializationMethod(CodecFeatures codecFeatures)
		{
			Ensure.NotNull(codecFeatures);
			var result = this.PreferredSerializationMethod ?? codecFeatures.PreferredObjectSerializationMethod;
			var isAvailable =
				result switch
				{
					SerializationMethod.Array => (codecFeatures.AvailableSerializationMethods & AvailableSerializationMethods.Array) != 0,
					_ /* Map */ => (codecFeatures.AvailableSerializationMethods & AvailableSerializationMethods.Map) != 0,
				};
			if (!isAvailable)
			{
				Throw.UnavailableMethod(codecFeatures.Name, result);
			}

			return result;
		}



		internal SerializationOptions(
			SerializationOptionsBuilder builder
		)
		{
			this.MaxDepth = builder.MaxDepth;
			this.DefaultStringEncoding = builder.DefaultStringEncoding;
			this.PreferredSerializationMethod = builder.PreferredSerializationMethod;
		}
	}
}
