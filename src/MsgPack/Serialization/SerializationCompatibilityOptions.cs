#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke and contributors
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
// Contributors:
//    Samuel Cragg
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
	///		Represents compatibility options of serialization runtime.
	/// </summary>
	public sealed class SerializationCompatibilityOptions
	{
#if NETFX_35 || UNITY || SILVERLIGHT
		private volatile bool _oneBoundDataMemberOrder;
#else
		private bool _oneBoundDataMemberOrder;
#endif // NETFX_35 || UNITY || SILVERLIGHT
		/// <summary>
		///		Gets or sets a value indicating whether <c>System.Runtime.Serialization.DataMemberAttribute.Order</c> should be started with 1 instead of 0.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if <c>System.Runtime.Serialization.DataMemberAttribute.Order</c> should be started with 1 instead of 0; otherwise, <c>false</c>.
		/// 	Default is <c>false</c>.
		/// </value>
		/// <remarks>
		///		Using this value, you can switch between MessagePack for CLI and ProtoBuf.NET seamlessly.
		/// </remarks>
		public bool OneBoundDataMemberOrder
		{
			get
			{
#if NETFX_35 || UNITY || SILVERLIGHT
				return this._oneBoundDataMemberOrder;
#else
				return Volatile.Read( ref this._oneBoundDataMemberOrder );
#endif // NETFX_35 || UNITY || SILVERLIGHT
			}
			set
			{
#if NETFX_35 || UNITY || SILVERLIGHT
				this._oneBoundDataMemberOrder = value;
#else
				Volatile.Write( ref this._oneBoundDataMemberOrder, value );
#endif // NETFX_35 || UNITY || SILVERLIGHT
			}
		}

		private int _packerCompatibilityOptions;

		/// <summary>
		///		Gets or sets the <see cref="PackerCompatibilityOptions"/>.
		/// </summary>
		/// <value>
		///		The <see cref="PackerCompatibilityOptions"/>. The default is <see cref="F:PackerCompatibilityOptions.Classic"/>.
		/// </value>
		/// <remarks>
		///		<note>
		///			Changing this property value does not affect already built serializers -- especially built-in (default) serializers.
		///			You must specify <see cref="T:PackerCompatibilityOptions"/> enumeration to the constructor of <see cref="SerializationContext"/> to
		///			change built-in serializers' behavior.
		///		</note>
		/// </remarks>
		public PackerCompatibilityOptions PackerCompatibilityOptions
		{
			get { return ( PackerCompatibilityOptions )Volatile.Read( ref this._packerCompatibilityOptions ); }
			set { Volatile.Write( ref this._packerCompatibilityOptions, ( int )value ); }
		}

#if NETFX_35 || UNITY || SILVERLIGHT
		private volatile bool _ignorePackabilityForCollection;
#else
		private bool _ignorePackabilityForCollection;
#endif // NETFX_35 || UNITY || SILVERLIGHT

		/// <summary>
		///		Gets or sets a value indicating whether serializer generator ignores packability interfaces for collections or not.
		/// </summary>
		/// <value>
		///		<c>true</c> if serializer generator ignores packability interfaces for collections; otherwise, <c>false</c>. The default is <c>false</c>.
		/// </value>
		/// <remarks>
		///		Historically, MessagePack for CLI ignored packability interfaces (<see cref="IPackable"/>, <see cref="IUnpackable"/>, 
		///		<see cref="IAsyncPackable"/> and <see cref="IAsyncUnpackable"/>) for collection which implements <see cref="IEquatable{T}"/> (except <see cref="String"/> and its kinds).
		///		As of 0.7, the generator respects such interfaces even if the target type is collection.
		///		Although this behavior is desirable and correct, setting this property <c>true</c> turn out the new behavior for backward compatibility.
		/// </remarks>
		public bool IgnorePackabilityForCollection
		{
			get
			{
#if NETFX_35 || UNITY || SILVERLIGHT
				return this._ignorePackabilityForCollection;
#else
				return Volatile.Read( ref this._ignorePackabilityForCollection );
#endif // NETFX_35 || UNITY || SILVERLIGHT
			}
			set
			{
#if NETFX_35 || UNITY || SILVERLIGHT
				this._ignorePackabilityForCollection = value;
#else
				Volatile.Write( ref this._ignorePackabilityForCollection, value );
#endif // NETFX_35 || UNITY || SILVERLIGHT
			}
		}

#if NETFX_35 || UNITY || SILVERLIGHT
		private volatile bool _allowNonCollectionEnumerableTypes;
#else
		private bool _allowNonCollectionEnumerableTypes;
#endif // NETFX_35 || UNITY || SILVERLIGHT

		/// <summary>
		///		Gets or sets a value indicating whether the serializer generator should serialize types that implement IEnumerable but do not have an Add method.
		/// </summary>
		/// <value>
		///		<c>true</c> if serializer generator should serialize a type implementing IEnumerable as a normal type if a public Add method is not found; otherwise, <c>false</c>. The default is <c>true</c>.
		/// </value>
		/// <remarks>
		///		Historically, MessagePack for CLI always tried to serialize any type that implemented IEnumerable as a collection, throwing an exception
		///		if an Add method could not be found. However, for types that implement IEnumerable but don't have an Add method the generator will now
		///		serialize the type as a non-collection type. To restore the old behavior for backwards compatibility, set this option to <c>false</c>.
		/// </remarks>
		public bool AllowNonCollectionEnumerableTypes
		{
			get
			{
#if NETFX_35 || UNITY || SILVERLIGHT
				return this._allowNonCollectionEnumerableTypes;
#else
				return Volatile.Read(ref this._allowNonCollectionEnumerableTypes);
#endif // NETFX_35 || UNITY || SILVERLIGHT
			}
			set
			{
#if NETFX_35 || UNITY || SILVERLIGHT
				this._allowNonCollectionEnumerableTypes = value;
#else
				Volatile.Write(ref this._allowNonCollectionEnumerableTypes, value);
#endif // NETFX_35 || UNITY || SILVERLIGHT
			}
		}

		// TODO: CheckNilImplicationInConstructorDeserialization

		internal SerializationCompatibilityOptions()
		{
			this.PackerCompatibilityOptions = PackerCompatibilityOptions.None;
			this.IgnorePackabilityForCollection = false;
			this.AllowNonCollectionEnumerableTypes = true;
		}
	}
}
