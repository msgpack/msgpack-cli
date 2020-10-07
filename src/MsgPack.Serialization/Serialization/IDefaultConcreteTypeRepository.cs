// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines internal interface for the repository of known concrete collections type for abstract collection types on deserialization. 
	/// </summary>
	internal interface IDefaultConcreteTypeRepository
	{
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
		Type? Get(Type abstractCollectionType);

		IEnumerable<KeyValuePair<RuntimeTypeHandle, object>> AsEnumerable();
	}
}
