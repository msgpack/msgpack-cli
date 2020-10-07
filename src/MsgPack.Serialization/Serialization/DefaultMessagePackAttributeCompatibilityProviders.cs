// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines default provider logic for <see cref="MessagePackMemberAttribute"/> compatibility.
	/// </summary>
	public static class DefaultMessagePackAttributeCompatibilityProviders
	{
		/// <summary>
		///		Gets a delegate for the logic which provides <see cref="MessagePackMemberAttributeData"/> or <c>null</c> from specified attributes.
		/// </summary>
		/// <value>
		///		A delegate for the logic which provides <see cref="MessagePackMemberAttributeData"/> or <c>null</c> from specified attributes.
		/// </value>
		/// <remarks>
		///		The logic provides compatiblity for <c>System.Runtime.Serialization.DataMemberAttribute</c>
		///		only if the type is qualified with <c>System.Runtime.Serialization.DataContractAttribute</c>.
		/// </remarks>
		public static Func<IEnumerable<CustomAttributeData>, IEnumerable<CustomAttributeData>, MessagePackMemberAttributeData?> DefaultMessagePackMemberAttributeCompatibilityProvider { get; } = ProvideDefaultMessagePackMemberAttributeCompatibility;

		/// <summary>
		///		Gets a delegate for the logic which provides <see cref="MessagePackIgnoreAttributeData"/> or <c>null</c> from specified attributes.
		/// </summary>
		/// <value>
		///		A delegate for the logic which provides <see cref="MessagePackIgnoreAttributeData"/> or <c>null</c> from specified attributes.
		/// </value>
		/// <remarks>
		///		The logic always returns <c>null</c>.
		/// </remarks>
		public static Func<IEnumerable<CustomAttributeData>, IEnumerable<CustomAttributeData>, MessagePackIgnoreAttributeData?> DefaultMessagePackIgnoreAttributeCompatibilityProvider { get; } = ProvideDefaultMessagePackIgnoreAttributeCompatibility;

		/// <summary>
		///		Provides <see cref="MessagePackMemberAttributeData"/> from specified attributes.
		/// </summary>
		/// <param name="typeAttributes">Attributes which qualify a type which declares current target member.</param>
		/// <param name="memberAttributes">Attribute which qualify current target member.</param>
		/// <returns>
		///		Compatible <see cref="MessagePackMemberAttributeData"/>. <c>null</c> if no compatible attributes were found.
		/// </returns>
		/// <remarks>
		///		This logic provides compatiblity for <c>System.Runtime.Serialization.DataMemberAttribute</c>
		///		only if the type is qualified with <c>System.Runtime.Serialization.DataContractAttribute</c>.
		/// </remarks>
		private static MessagePackMemberAttributeData? ProvideDefaultMessagePackMemberAttributeCompatibility(
			IEnumerable<CustomAttributeData> typeAttributes,
			IEnumerable<CustomAttributeData> memberAttributes
		)
		{
			if (!typeAttributes.Any(a => a.GetAttributeType().FullName == "System.Runtime.Serialization.DataContractAttribute"))
			{
				return null;
			}

			var dataMemberAttribute =
				memberAttributes
				.FirstOrDefault(a =>
					a.GetAttributeType()
					.FullName == "System.Runtime.Serialization.DataMemberAttribute"
				);
			if (dataMemberAttribute == null)
			{
				return null;
			}

			var name = default(string?);
			var order = default(int?);
			foreach (var namedArgument in dataMemberAttribute.GetNamedArguments())
			{
				var memberName = namedArgument.GetMemberName();
				if (memberName == "Name")
				{
					name = (string?)namedArgument.GetTypedValue().Value;
				}
				else if (memberName == "Order")
				{
					order = (int?)namedArgument.GetTypedValue().Value;
				}
			}

			return new MessagePackMemberAttributeData(order < 0 ? null : order, name);
		}

		/// <summary>
		///		Provides <see cref="MessagePackIgnoreAttributeData"/> from specified attributes.
		/// </summary>
		/// <param name="typeAttributes">Attributes which qualify a type which declares current target member.</param>
		/// <param name="memberAttributes">Attribute which qualify current target member.</param>
		/// <returns>
		///		Always <c>null</c>.
		/// </returns>
		/// <remarks>
		///		This logic provides compatiblity for <c>System.Runtime.Serialization.DataMemberAttribute</c>
		///		only if the type is qualified with <c>System.Runtime.Serialization.DataContractAttribute</c>.
		/// </remarks>
		private static MessagePackIgnoreAttributeData? ProvideDefaultMessagePackIgnoreAttributeCompatibility(
			IEnumerable<CustomAttributeData> typeAttributes,
			IEnumerable<CustomAttributeData> memberAttributes
		) => memberAttributes.Any(
				a => a.AttributeType.FullName == "System.Runtime.Serialization.IgnoreDataMemberAttribute") ?
				default(MessagePackIgnoreAttributeData) : 
				default(MessagePackIgnoreAttributeData?);
	}
}
