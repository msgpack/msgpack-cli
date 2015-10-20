#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
using System.Collections;
using System.Collections.Generic;
#if !NETFX_35 && !SILVERLIGHT && !NETFX_40
using System.Collections.ObjectModel;
#endif
using System.Globalization;
using System.Linq;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Repository of known concrete collection type for abstract collection type.
	/// </summary>
	public sealed class DefaultConcreteTypeRepository
	{
		private readonly TypeKeyRepository _defaultCollectionTypes;

		internal DefaultConcreteTypeRepository()
		{
			this._defaultCollectionTypes = new TypeKeyRepository(
				new Dictionary<RuntimeTypeHandle, object>(
#if NETFX_35 || ( SILVERLIGHT && !WINDOWS_PHONE )
					8
#elif NETFX_40
					9
#else
					12
#endif
				)
				{
					{ typeof( IEnumerable<> ).TypeHandle, typeof( List<> ) },
					{ typeof( ICollection<> ).TypeHandle, typeof( List<> ) },
					{ typeof( IList<> ).TypeHandle, typeof( List<> ) },
					{ typeof( IDictionary<,> ).TypeHandle, typeof( Dictionary<,> ) },
					{ typeof( IEnumerable ).TypeHandle, typeof( List<MessagePackObject> ) },
					{ typeof( ICollection ).TypeHandle, typeof( List<MessagePackObject> ) },
					{ typeof( IList ).TypeHandle, typeof( List<MessagePackObject> ) },
					{ typeof( IDictionary ).TypeHandle, typeof( MessagePackObjectDictionary ) },
#if !NETFX_35 && !UNITY
					{ typeof( ISet<> ).TypeHandle, typeof( HashSet<> ) },
#if !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
					{ typeof( IReadOnlyCollection<> ).TypeHandle, typeof( List<> ) },
					{ typeof( IReadOnlyList<> ).TypeHandle, typeof( List<> ) },
					{ typeof( IReadOnlyDictionary<,> ).TypeHandle, typeof( Dictionary<,> ) },
#endif // !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
#endif // !NETFX_35 && !UNITY
				}
			);
		}

		/// <summary>
		///		Gets the default type for the collection.
		/// </summary>
		/// <param name="abstractCollectionType">Type of the abstract collection.</param>
		/// <returns>
		///		Type of default concrete collection.
		///		If concrete collection type of <paramref name="abstractCollectionType"/>, then returns <c>null</c>.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		///		<paramref name="abstractCollectionType"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		By default, following types are registered:
		///		<list type="table">
		///			<listheader>
		///				<term>Abstract Collection Type</term>
		///				<description>Concrete Default Collection Type</description>
		///			</listheader>
		///			<item>
		///				<term><see cref="IEnumerable{T}"/></term>
		///				<description><see cref="List{T}"/></description>
		///			</item>
		///			<item>
		///				<term><see cref="ICollection{T}"/></term>
		///				<description><see cref="List{T}"/></description>
		///			</item>
		///			<item>
		///				<term><c>ISet{T}</c> (.NET 4 or lator)</term>
		///				<description><see cref="HashSet{T}"/></description>
		///			</item>
		///			<item>
		///				<term><see cref="IList{T}"/></term>
		///				<description><see cref="List{T}"/></description>
		///			</item>
		///			<item>
		///				<term><see cref="IDictionary{TKey,TValue}"/></term>
		///				<description><see cref="Dictionary{TKey,TValue}"/></description>
		///			</item>
		///			<item>
		///				<term><see cref="IEnumerable"/></term>
		///				<description><see cref="List{T}"/> of <see cref="MessagePackObject"/>.</description>
		///			</item>
		///			<item>
		///				<term><see cref="ICollection"/></term>
		///				<description><see cref="List{T}"/> of <see cref="MessagePackObject"/>.</description>
		///			</item>
		///			<item>
		///				<term><see cref="IList"/></term>
		///				<description><see cref="List{T}"/> of <see cref="MessagePackObject"/>.</description>
		///			</item>
		///			<item>
		///				<term><see cref="IDictionary"/></term>
		///				<description><see cref="MessagePackObjectDictionary"/></description>
		///			</item>
		///		</list>
		/// </remarks>
		public Type Get( Type abstractCollectionType )
		{
			if ( abstractCollectionType == null )
			{
				throw new ArgumentNullException( "abstractCollectionType" );
			}

			object concrete, genericDefinition;
			this._defaultCollectionTypes.Get( abstractCollectionType, out concrete, out genericDefinition );

			return concrete as Type ?? genericDefinition as Type;
		}

		internal Type GetConcreteType( Type abstractCollectionType )
		{
			var typeOrDefinition = this.Get( abstractCollectionType );
			if ( typeOrDefinition == null || !typeOrDefinition.GetIsGenericTypeDefinition() || !abstractCollectionType.GetIsGenericType() )
			{
				return typeOrDefinition;
			}

			// Assume type repository has only concrete generic type definition which has same arity for abstract type.
			return typeOrDefinition.MakeGenericType( abstractCollectionType.GetGenericArguments() );
		}

		/// <summary>
		///		Registers the default type of the collection.
		/// </summary>
		/// <param name="abstractCollectionType">Type of the abstract collection.</param>
		/// <param name="defaultCollectionType">Default concrete type of the <paramref name="abstractCollectionType"/>.</param>
		/// <exception cref="System.ArgumentNullException">
		///		<paramref name="abstractCollectionType"/> is <c>null</c>.
		///		Or <paramref name="defaultCollectionType"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		///		<paramref name="abstractCollectionType"/> is not collection type.
		///		Or <paramref name="defaultCollectionType"/> is abstract class or interface.
		///		Or <paramref name="defaultCollectionType"/> is open generic type but <paramref name="abstractCollectionType"/> is closed generic type.
		///		Or <paramref name="defaultCollectionType"/> is closed generic type but <paramref name="abstractCollectionType"/> is open generic type.
		///		Or <paramref name="defaultCollectionType"/> does not have same arity for <paramref name="abstractCollectionType"/>.
		///		Or <paramref name="defaultCollectionType"/> is not assignable to <paramref name="abstractCollectionType"/>
		///		or the constructed type from <paramref name="defaultCollectionType"/> will not be assignable to the constructed type from <paramref name="abstractCollectionType"/>.
		/// </exception>
		/// <remarks>
		///		If you want to overwrite default type for collection interfaces, you can use this method.
		///		Note that this method only supports collection interface, that is subtype of the <see cref="IEnumerable"/> interface.
		///		<note>
		///			If you register invalid type for <paramref name="defaultCollectionType"/>, then runtime exception will be occurred.
		///			For example, you register <see cref="IEnumerable{T}"/> of <see cref="Char"/> and <see cref="String"/> pair, but it will cause runtime error.
		///		</note>
		/// </remarks>
		/// <seealso cref="Get"/>
		public void Register( Type abstractCollectionType, Type defaultCollectionType )
		{

			if ( abstractCollectionType == null )
			{
				throw new ArgumentNullException( "abstractCollectionType" );
			}

			if ( defaultCollectionType == null )
			{
				throw new ArgumentNullException( "defaultCollectionType" );
			}

			// Use GetIntegerfaces() in addition to IsAssignableFrom because of open generic type support.
			if ( !abstractCollectionType.GetInterfaces().Contains( typeof( IEnumerable ) ) )
			{
				throw new ArgumentException(
					String.Format( CultureInfo.CurrentCulture, "The type '{0}' is not collection.", abstractCollectionType ),
					"abstractCollectionType" );
			}

			if ( abstractCollectionType.GetIsGenericTypeDefinition() )
			{
				if ( !defaultCollectionType.GetIsGenericTypeDefinition() )
				{
					throw new ArgumentException(
						String.Format(
							CultureInfo.CurrentCulture,
							"The closed generic type '{1}' cannot be assigned to open generic type '{0}'.",
							abstractCollectionType,
							defaultCollectionType ),
						"defaultCollectionType" );
				}

				if ( abstractCollectionType.GetGenericTypeParameters().Length !=
					defaultCollectionType.GetGenericTypeParameters().Length )
				{
					throw new ArgumentException(
						String.Format(
							CultureInfo.CurrentCulture,
							"The default generic collection type '{1}' does not have same arity for abstract generic collection type '{0}'.",
							abstractCollectionType,
							defaultCollectionType ),
						"defaultCollectionType" );
				}
			}
			else if ( defaultCollectionType.GetIsGenericTypeDefinition() )
			{
				throw new ArgumentException(
					String.Format(
						CultureInfo.CurrentCulture,
						"The open generic type '{1}' cannot be assigned to closed generic type '{0}'.",
						abstractCollectionType,
						defaultCollectionType ),
					"defaultCollectionType" );
			}

			if ( defaultCollectionType.GetIsAbstract() || defaultCollectionType.GetIsInterface() )
			{
				throw new ArgumentException(
					String.Format( CultureInfo.CurrentCulture, "The defaultCollectionType cannot be abstract class nor interface. The type '{0}' is abstract type.", defaultCollectionType ),
					"defaultCollectionType" );
			}

			// Use GetIntegerfaces() and BaseType instead of IsAssignableFrom because of open generic type support.
			if ( !abstractCollectionType.IsAssignableFrom( defaultCollectionType )
				 && abstractCollectionType.GetIsGenericTypeDefinition()
				 && !defaultCollectionType
						 .GetInterfaces()
						 .Select( t => ( t.GetIsGenericType() && !t.GetIsGenericTypeDefinition() ) ? t.GetGenericTypeDefinition() : t )
						 .Contains( abstractCollectionType )
				 && !IsAnscestorType( abstractCollectionType, defaultCollectionType ) )
			{
				throw new ArgumentException(
					String.Format(
						CultureInfo.CurrentCulture,
						"The type '{1}' is not assignable to '{0}'.",
						abstractCollectionType,
						defaultCollectionType ),
					"defaultCollectionType" );
			}

			this._defaultCollectionTypes.Register( abstractCollectionType, defaultCollectionType, null, null, SerializerRegistrationOptions.AllowOverride );
		}

		private static bool IsAnscestorType( Type mayBeAncestor, Type mayBeDescendant )
		{
			for ( var type = mayBeDescendant; type != null; type = type.GetBaseType() )
			{
				if ( type == mayBeAncestor )
				{
					return true;
				}

				if ( type.GetIsGenericType() && type.GetGenericTypeDefinition() == mayBeAncestor )
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		///		Unregisters the default type of the collection.
		/// </summary>
		/// <param name="abstractCollectionType">Type of the abstract collection.</param>
		/// <returns>
		///		<c>true</c> if default collection type is removed successfully;
		///		otherwise, <c>false</c>.
		/// </returns>
		public bool Unregister( Type abstractCollectionType )
		{
			if ( abstractCollectionType == null )
			{
				return false;
			}

			return this._defaultCollectionTypes.Unregister( abstractCollectionType );
		}
	}
}