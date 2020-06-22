// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace MsgPack
{
	/// <summary>
	///		Defines helper method for items of tuple type.
	/// </summary>
	internal static class TupleItems
	{
		/// <summary>
		///		Creates type list for nested tuples.
		/// </summary>
		/// <param name="rootTupleType">The type of base tuple.</param>
		/// <returns>
		///		The type list for nested tuples.
		///		The order is from outer to inner.
		/// </returns>
		public static List<Type> CreateTupleTypeList(Type rootTupleType)
		{
			if (!rootTupleType.GetIsGenericType())
			{
				// arity 0 value tuple
				return new List<Type>(1) { rootTupleType };
			}

			var assembly = rootTupleType.GetAssembly();
			var baseName = rootTupleType.FullName!.Remove(rootTupleType.FullName.IndexOf('`') + 1);
			var result = new List<Type>();
			var tupleType = rootTupleType;
			while (true)
			{
				result.Add(tupleType);
				if (!tupleType.GetIsGenericType())
				{
					// arity 0
					break;
				}

				var itemTypes = tupleType.GetGenericArguments();
				if (itemTypes.Length < 8)
				{
					// leaf tuple
					break;
				}

				tupleType = itemTypes.Last();
			}

			return result;
		}

		public static IReadOnlyList<MemberInfo> GetTupleItemMembers(Type tupleType)
		{
			Debug.Assert(IsTuple(tupleType), $"IsTuple({tupleType.AssemblyQualifiedName})");
			var members = new List<MemberInfo>(tupleType.GetGenericArguments().Length);
			GetTupleItemMembers(tupleType, members);
			return members;
		}

		private static void GetTupleItemMembers(Type tupleType, IList<MemberInfo> result)
		{
			var properties = tupleType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			var count = properties.Length == 8 ? 7 : properties.Length;
			for (var i = 0; i < count; i++)
			{
				result.Add(properties[i]);
			}

			if (properties.Length == 8)
			{
				var rest = properties[7];
				Debug.Assert(IsTuple(rest.PropertyType), $"IsTuple({rest.PropertyType.AssemblyQualifiedName})");
				// Put nested tuple's item types recursively.
				GetTupleItemMembers(rest.PropertyType, result);
			}
		}

		public static IReadOnlyList<Type> GetTupleItemTypes(Type tupleType)
		{
			Debug.Assert(IsTuple(tupleType), $"IsTuple({tupleType.AssemblyQualifiedName})");
			var arguments = tupleType.GetGenericArguments();
			var itemTypes = new List<Type>(tupleType.GetGenericArguments().Length);
			GetTupleItemTypes(arguments, itemTypes);
			return itemTypes;
		}

		private static void GetTupleItemTypes(IList<Type> itemTypes, IList<Type> result)
		{
			var count = itemTypes.Count == 8 ? 7 : itemTypes.Count;
			for (var i = 0; i < count; i++)
			{
				result.Add(itemTypes[i]);
			}

			if (itemTypes.Count == 8)
			{
				var trest = itemTypes[7];
				Debug.Assert(IsTuple(trest), $"IsTuple({trest.AssemblyQualifiedName})");
				// Put nested tuple's item types recursively.
				GetTupleItemTypes(trest.GetGenericArguments(), result);
			}
		}

		public static bool IsTuple(Type type)
		{
			return
				type.GetIsPublic()
				&& ((type.FullName!.StartsWith("System.ValueTuple`", StringComparison.Ordinal) && type.GetIsValueType())
					|| (type.FullName.StartsWith("System.Tuple`", StringComparison.Ordinal) && !type.GetIsValueType())
					|| (type.FullName == "System.ValueTuple" && type.GetIsValueType())
				);
		}
	}
}
