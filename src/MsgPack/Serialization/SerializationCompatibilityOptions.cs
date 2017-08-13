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
#if !FEATURE_CONCURRENT
		private volatile bool _oneBoundDataMemberOrder;
#else
		private bool _oneBoundDataMemberOrder;
#endif // !FEATURE_CONCURRENT

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
#if !FEATURE_CONCURRENT
				return this._oneBoundDataMemberOrder;
#else
				return Volatile.Read( ref this._oneBoundDataMemberOrder );
#endif // !FEATURE_CONCURRENT
			}
			set
			{
#if !FEATURE_CONCURRENT
				this._oneBoundDataMemberOrder = value;
#else
				Volatile.Write( ref this._oneBoundDataMemberOrder, value );
#endif // !FEATURE_CONCURRENT
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

#if !FEATURE_CONCURRENT
		private volatile bool _ignorePackabilityForCollection;
#else
		private bool _ignorePackabilityForCollection;
#endif // !FEATURE_CONCURRENT

		/// <summary>
		///		Gets or sets a value indicating whether serializer generator ignores packability interfaces for collections or not.
		/// </summary>
		/// <value>
		///		<c>true</c> if serializer generator ignores packability interfaces for collections; otherwise, <c>false</c>. The default is <c>false</c>.
		/// </value>
		/// <remarks>
		///		Historically, MessagePack for CLI ignored packability interfaces (<see cref="IPackable"/>, <see cref="IUnpackable"/>, 
		///		<c>IAsyncPackable"</c> and <c>IAsyncUnpackable</c>) for collection which implements <see cref="IEquatable{T}"/> (except <see cref="String"/> and its kinds).
		///		As of 0.7, the generator respects such interfaces even if the target type is collection.
		///		Although this behavior is desirable and correct, setting this property <c>true</c> turn out the new behavior for backward compatibility.
		/// </remarks>
		public bool IgnorePackabilityForCollection
		{
			get
			{
#if !FEATURE_CONCURRENT
				return this._ignorePackabilityForCollection;
#else
				return Volatile.Read( ref this._ignorePackabilityForCollection );
#endif // !FEATURE_CONCURRENT
			}
			set
			{
#if !FEATURE_CONCURRENT
				this._ignorePackabilityForCollection = value;
#else
				Volatile.Write( ref this._ignorePackabilityForCollection, value );
#endif // !FEATURE_CONCURRENT
			}
		}

#if !FEATURE_CONCURRENT
		private volatile bool _allowNonCollectionEnumerableTypes;
#else
		private bool _allowNonCollectionEnumerableTypes;
#endif // !FEATURE_CONCURRENT

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
#if !FEATURE_CONCURRENT
				return this._allowNonCollectionEnumerableTypes;
#else
				return Volatile.Read( ref this._allowNonCollectionEnumerableTypes );
#endif // !FEATURE_CONCURRENT
			}
			set
			{
#if !FEATURE_CONCURRENT
				this._allowNonCollectionEnumerableTypes = value;
#else
				Volatile.Write( ref this._allowNonCollectionEnumerableTypes, value );
#endif // !FEATURE_CONCURRENT
			}
		}


#if !FEATURE_CONCURRENT
		private volatile bool _allowAsymmetricSerializer;
#else
		private bool _allowAsymmetricSerializer;
#endif // !FEATURE_CONCURRENT

		/// <summary>
		///		Gets or sets a value indicating whether the serializer generator generates serializer types even when the generator determines that feature complete serializer cannot be generated due to lack of some requirement.
		/// </summary>
		/// <value>
		///		<c>true</c> if the serializer generator generates serializer types even when the generator determines that feature complete serializer cannot be generated due to lack of some requirement; otherwise, <c>false</c>. The default is <c>false</c>.
		/// </value>
		/// <remarks>
		///		Currently, the lack of constructor (default or parameterized) or lack of settable members are considerd as "cannot generate feature complete serializer".
		///		Therefore, you can get serialization only serializer if this property is set to <c>true</c>.
		///		This is useful for logging, telemetry injestion, or so.
		///		You can investigate serializer capability via <see cref="MessagePackSerializer.Capabilities"/> property.
		/// </remarks>
		public bool AllowAsymmetricSerializer
		{
			get
			{
#if !FEATURE_CONCURRENT
				return this._allowAsymmetricSerializer;
#else
				return Volatile.Read( ref this._allowAsymmetricSerializer );
#endif // !FEATURE_CONCURRENT
			}
			set
			{
#if !FEATURE_CONCURRENT
				this._allowAsymmetricSerializer = value;
#else
				Volatile.Write( ref this._allowAsymmetricSerializer, value );
#endif // !FEATURE_CONCURRENT
			}
		}

		// TODO: CheckNilImplicationInConstructorDeserialization

		internal SerializationCompatibilityOptions()
		{
			this.PackerCompatibilityOptions = PackerCompatibilityOptions.None;
			this.IgnorePackabilityForCollection = false;
			this.AllowNonCollectionEnumerableTypes = true;
			this.AllowAsymmetricSerializer = false;
		}
	}
}
