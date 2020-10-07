// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MsgPack.Internal;

namespace MsgPack.Serialization
{
	/// <summary>
	///		A builder object for <see cref="SerializationOptions"/> which defines options for each serialization operations.
	/// </summary>
	/// <seealso cref="SerializationOptions"/>
	public sealed class SerializationOptionsBuilder
	{
		/// <summary>
		///		Gets the maximum depth of the serialized object tree.
		/// </summary>
		/// <value>
		///		The maximum depth of the serialized object tree. Default is <c>100</c>.
		/// </value>
		public int MaxDepth { get; private set; } = OptionsDefaults.MaxDepth;

		/// <summary>
		///		Gets the default string encoding for string members or map keys which are not specified explicitly.
		/// </summary>
		/// <value>
		///		The default string encoding for string members or map keys which are not specified explicitly.
		///		Default value is <c>null</c>, which means using default encoding of underlying codec.
		/// </value>
		public Encoding? DefaultStringEncoding { get; private set; }

		/// <summary>
		///		Gets the preferred <see cref="SerializationMethod"/> which will be used members which serialization methods are not derived from their attributes nor member types.  
		/// </summary>
		/// <value>
		///		The preferred <see cref="SerializationMethod"/> which will be used members which serialization methods are not derived from their attributes nor member types.  
		///		Default is <c>null</c>, which means using default method of underlying codec.
		/// </value>
		public SerializationMethod? PreferredSerializationMethod { get; private set; } = OptionsDefaults.PreferredSerializationMethod;

#warning TODO:
		public ExtensionTypeMapping ExtensionTypeMapping { get; } = new ExtensionTypeMapping();

		/// <summary>
		///		Initializes a new instance of <see cref="SerializationOptionsBuilder"/> object.
		/// </summary>
		public SerializationOptionsBuilder() { }

		/// <summary>
		///		Creates a new instance of <see cref="SerializationOptions"/> object from current state of this instance.
		/// </summary>
		/// <returns>A new instance of <see cref="SerializationOptions"/> object.</returns>
		public SerializationOptions Create()
			=> new SerializationOptions(this);

		/// <summary>
		///		Indicates the maximum depth of the serialized object tree to the default value.
		/// </summary>
		/// <returns>This <see cref="SerializationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="MaxDepth"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public SerializationOptionsBuilder UseDefaultMaxDepth()
			=> this.UseMaxDepth(OptionsDefaults.MaxDepth);

		/// <summary>
		///		Sets the maximum depth of the serialized object tree.
		/// </summary>
		/// <param name="value">A value to be set.</param>
		/// <returns>This <see cref="SerializationOptionsBuilder"/> instance.</returns>
		/// <exception cref="System.ArgumentNullException">
		///		<paramref name="value"/> is less than or equal to <c>0</c>, or exceeds <c>0x7FEFFFFF</c>.
		/// </exception>
		public SerializationOptionsBuilder UseMaxDepth(int value)
		{
			this.MaxDepth = Ensure.IsBetween(value, 1, OptionsDefaults.MaxMultiByteCollectionLength); ;
			return this;
		}

		/// <summary>
		///		Indicates the default string encoding for string members or map keys to use default encoding of the codec.
		/// </summary>
		/// <returns>This <see cref="SerializationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="DefaultStringEncoding"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public SerializationOptionsBuilder UseCodecDefaultStringEncoding()
		{
			this.DefaultStringEncoding = null;
			return this;
		}

		/// <summary>
		///		Sets the default string encoding for string members or map keys which are not specified explicitly.
		/// </summary>
		/// <param name="value">An <see cref="Encoding"/> to use encode/decode string value.</param>
		/// <returns>This <see cref="SerializationOptionsBuilder"/> instance.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
		/// <remarks>
		///		This method should be called only when you needs to interoperability with system which uses non-default encoding for the codec.
		/// </remarks>
		public SerializationOptionsBuilder UseCustomDefaultStringEncoding(Encoding value)
		{
			this.DefaultStringEncoding = Ensure.NotNull(value);
			return this;
		}

		/// <summary>
		///		Indicates the preferred <see cref="SerializationMethod"/> to use codec's default.
		/// </summary>
		/// <returns>This <see cref="SerializationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="PreferredSerializationMethod"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public SerializationOptionsBuilder UseCodecDefaultPreferredSerializationMethod()
		{
			this.PreferredSerializationMethod = null;
			return this;
		}

		/// <summary>
		///		Sets the preferred <see cref="SerializationMethod"/> which will be used members which serialization methods are not derived from their attributes nor member types.  
		/// </summary>
		/// <returns>This <see cref="SerializationOptionsBuilder"/> instance.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException"><paramref name="value"/> is not known <see cref="SerializationMethod"/> value.</exception>
		/// <remarks>
		///		Note that preferred <see cref="SerializationMethod"/> may cause error when the codec does not support the value.
		/// </remarks>
		public SerializationOptionsBuilder UseCustomPreferredSerializationMethod(SerializationMethod value)
		{
			switch (value)
			{
				case SerializationMethod.Array:
				case SerializationMethod.Map:
				{
					break;
				}
				default:
				{
					Throw.UndefinedEnumMember(value);
					break; // never
				}
			}
			this.PreferredSerializationMethod = value;
			return this;
		}
	}

	/// <summary>
	///		Implements mapping table between known ext type codes and names.
	/// </summary>
	/// <remarks>
	///		Well-known (pre-defined) ext type names are defined in <c>KnownExtensionTypeNames</c>, and their default mapped codes are found in <c>KnownExtensionTypeCodes</c>.
	///		They should be defined in codec assemblies if available.
	/// </remarks>
	/// <threadsafety instance="true" static="true" />
	public sealed class ExtensionTypeCodeMapping : IEnumerable<KeyValuePair<string, ExtensionType>>
	{
		private readonly object _syncRoot;
		private readonly Dictionary<string, ExtensionType> _index;
		private readonly Dictionary<ExtensionType, string> _types;

		/// <summary>
		///		Gets a mapped byte to the specified ext type name.
		/// </summary>
		/// <param name="name">The name of the ext type.</param>
		/// <returns>
		///		The <see cref="ExtensionType"/> for specified ext type in the current context.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
		/// <exception cref="KeyNotFoundException"><paramref name="name"/> is not registered as known ext type name.</exception>
		public ExtensionType this[string name]
		{
			get
			{
				ValidateName(name);

				lock (this._syncRoot)
				{
					if (!this._index.TryGetValue(name, out var code))
					{
						Throw.ExtensionTypeKeyNotFound(name);
						throw new KeyNotFoundException(
							String.Format(CultureInfo.CurrentCulture, "Ext type '{0}' is not found.", name)
						);
					}

					return code;
				}
			}
		}

		internal ExtensionTypeCodeMapping()
		{
			this._syncRoot = new object();
			this._index = new Dictionary<string, ExtensionType>();
			this._types = new Dictionary<ExtensionType, string>();
		}

		/// <summary>
		///		Adds the known ext type mapping.
		/// </summary>
		/// <param name="name">The name of the ext type.</param>
		/// <param name="typeCode">The ext type code to be mapped.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="name"/> AND <paramref name="typeCode"/> were not registered and then newly registered; <c>false</c>, otherwise.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="typeCode"/> is greater than 0x7F.</exception>
		public bool Add(string name, byte typeCode)
		{
			ValidateName(name);
			ValidateTypeCode(typeCode);
			return this.AddInternal(name, typeCode);
		}

		private bool AddInternal(string name, byte typeCode)
		{
			lock (this._syncRoot)
			{
				try
				{
					this._types.Add(typeCode, name);
				}
				catch (ArgumentException)
				{
					return false;
				}

				this._index[name] = typeCode;
				return true;
			}
		}

		/// <summary>
		///		Removes the mapping with specified name.
		/// </summary>
		/// <param name="name">The name of the ext type.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="name"/> was registered and has been removed successfully; <c>false</c>, otherwise.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
		public bool Remove(string name)
		{
			ValidateName(name);

			lock (this._syncRoot)
			{
				byte typeCode;
				if (!this._index.TryGetValue(name, out typeCode))
				{
					return false;
				}

				this.RemoveCore(name, typeCode);
				return true;
			}
		}

		/// <summary>
		///		Removes the mapping with specified code.
		/// </summary>
		/// <param name="typeCode">The type code of the ext type.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="typeCode"/> was registered and has been removed successfully; <c>false</c>, otherwise.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="typeCode"/> is greater than 0x7F.</exception>
		public bool Remove(byte typeCode)
		{
			ValidateTypeCode(typeCode);

			lock (this._syncRoot)
			{
				string name;
				if (!this._types.TryGetValue(typeCode, out name))
				{
					return false;
				}

				this.RemoveCore(name, typeCode);
				return true;
			}
		}

		private void RemoveCore(string name, byte typeCode)
		{
#if DEBUG && NET45
			Contract.Assert( Monitor.IsEntered( this._syncRoot ) );
#endif // DEBUG && NET45
			var shouldBeTrue = this._types.Remove(typeCode);
			Contract.Assert(shouldBeTrue);
			shouldBeTrue = this._index.Remove(name);
			Contract.Assert(shouldBeTrue);
		}

		/// <summary>
		///		Clears all mappings.
		/// </summary>
		public void Clear()
		{
			lock (this._syncRoot)
			{
				this._types.Clear();
				this._index.Clear();
			}
		}

		/// <summary>
		///		Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		///		A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
		/// </returns>
		/// <remarks>
		///		This method causes internal collection copying, so this makes O(n) time.
		/// </remarks>
		public IEnumerator<KeyValuePair<string, byte>> GetEnumerator()
		{
			List<KeyValuePair<string, byte>> list;
			lock (this._syncRoot)
			{
				list = this._index.ToList();
			}

			return list.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		private static void ValidateName(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}

			if (name.Length == 0)
			{
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "A parameter cannot be empty."), "name");
			}
		}

		private static void ValidateTypeCode(byte typeCode)
		{
			if (typeCode > 0x7F)
			{
				throw new ArgumentOutOfRangeException("typeCode", "Ext type code must be between 0 and 0x7F.");
			}
		}
	}
}
