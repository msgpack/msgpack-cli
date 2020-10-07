// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Builder object to configure default concrete types for abstract collection types on deserialization.
	/// </summary>
	public sealed class DefaultConcreteTypeRepositoryBuilder
	{
		internal TypeKeyRepository DefaultCollectionTypes { get; }

		/// <summary>
		///		Initializes a new instance of <see cref="DefaultConcreteTypeRepositoryBuilder"/> object.
		/// </summary>
		/// <remarks>
		///		This constructor internally registers default type mapping for known collection types.
		/// </remarks>
		public DefaultConcreteTypeRepositoryBuilder()
		{
			this.DefaultCollectionTypes =
				new TypeKeyRepository(
					new Dictionary<RuntimeTypeHandle, object>(
#if !FEATURE_ISET && !FEATURE_READ_ONLY_COLLECTION
						8
#elif !FEATURE_READ_ONLY_COLLECTION
						9
#else
						12
#endif
					)
					{
						{ typeof(IEnumerable<>).TypeHandle, typeof(List<>) },
						{ typeof(ICollection<>).TypeHandle, typeof(List<>) },
						{ typeof(IList<>).TypeHandle, typeof(List<>) },
						{ typeof(IDictionary<,>).TypeHandle, typeof(Dictionary<,>) },
						{ typeof(IEnumerable).TypeHandle, typeof(List<MessagePackObject>) },
						{ typeof(ICollection).TypeHandle, typeof(List<MessagePackObject>) },
						{ typeof(IList).TypeHandle, typeof(List<MessagePackObject>) },
						{ typeof(IDictionary).TypeHandle, typeof(MessagePackObjectDictionary) },
#if FEATURE_ISET
						{ typeof(ISet<>).TypeHandle, typeof(HashSet<>) },
#if FEATURE_READ_ONLY_COLLECTION
						{ typeof(IReadOnlyCollection<> ).TypeHandle, typeof(List<>) },
						{ typeof(IReadOnlyList<>).TypeHandle, typeof(List<>) },
						{ typeof(IReadOnlyDictionary<,>).TypeHandle, typeof(Dictionary<,>) },
#endif // FEATURE_READ_ONLY_COLLECTION
#endif // FEATURE_ISET
					}
				);
		}

		internal DefaultConcreteTypeRepositoryBuilder(TypeKeyRepository existing)
		{
			this.DefaultCollectionTypes = new TypeKeyRepository(existing);
		}

		internal ImmutableDefaultConcreteTypeRepository Build()
			=> new ImmutableDefaultConcreteTypeRepository(this.DefaultCollectionTypes.GetClonedTable());

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
		public void Register(Type abstractCollectionType, Type defaultCollectionType)
		{

			if (abstractCollectionType == null)
			{
				throw new ArgumentNullException("abstractCollectionType");
			}

			if (defaultCollectionType == null)
			{
				throw new ArgumentNullException("defaultCollectionType");
			}

			// Use GetIntegerfaces() in addition to IsAssignableFrom because of open generic type support.
			if (!abstractCollectionType.GetInterfaces().Contains(typeof(IEnumerable)))
			{
				throw new ArgumentException(
					String.Format(CultureInfo.CurrentCulture, "The type '{0}' is not collection.", abstractCollectionType),
					"abstractCollectionType");
			}

			if (abstractCollectionType.GetIsGenericTypeDefinition())
			{
				if (!defaultCollectionType.GetIsGenericTypeDefinition())
				{
					throw new ArgumentException(
						String.Format(
							CultureInfo.CurrentCulture,
							"The closed generic type '{1}' cannot be assigned to open generic type '{0}'.",
							abstractCollectionType,
							defaultCollectionType),
						"defaultCollectionType");
				}

				if (abstractCollectionType.GetGenericTypeParameters().Length !=
					defaultCollectionType.GetGenericTypeParameters().Length)
				{
					throw new ArgumentException(
						String.Format(
							CultureInfo.CurrentCulture,
							"The default generic collection type '{1}' does not have same arity for abstract generic collection type '{0}'.",
							abstractCollectionType,
							defaultCollectionType),
						"defaultCollectionType");
				}
			}
			else if (defaultCollectionType.GetIsGenericTypeDefinition())
			{
				throw new ArgumentException(
					String.Format(
						CultureInfo.CurrentCulture,
						"The open generic type '{1}' cannot be assigned to closed generic type '{0}'.",
						abstractCollectionType,
						defaultCollectionType),
					"defaultCollectionType");
			}

			if (defaultCollectionType.GetIsAbstract() || defaultCollectionType.GetIsInterface())
			{
				throw new ArgumentException(
					String.Format(CultureInfo.CurrentCulture, "The defaultCollectionType cannot be abstract class nor interface. The type '{0}' is abstract type.", defaultCollectionType),
					"defaultCollectionType");
			}

			// Use GetIntegerfaces() and BaseType instead of IsAssignableFrom because of open generic type support.
			if (!abstractCollectionType.IsAssignableFrom(defaultCollectionType)
				 && abstractCollectionType.GetIsGenericTypeDefinition()
				 && !defaultCollectionType
						 .GetInterfaces()
						 .Select(t => (t.GetIsGenericType() && !t.GetIsGenericTypeDefinition()) ? t.GetGenericTypeDefinition() : t)
						 .Contains(abstractCollectionType)
				 && !IsAnscestorType(abstractCollectionType, defaultCollectionType))
			{
				throw new ArgumentException(
					String.Format(
						CultureInfo.CurrentCulture,
						"The type '{1}' is not assignable to '{0}'.",
						abstractCollectionType,
						defaultCollectionType),
					"defaultCollectionType");
			}

			this.DefaultCollectionTypes.Register(abstractCollectionType, defaultCollectionType, null, null, SerializerRegistrationOptions.AllowOverride);
		}

		private static bool IsAnscestorType(Type mayBeAncestor, Type mayBeDescendant)
		{
			for (var type = mayBeDescendant; type != null; type = type.GetBaseType())
			{
				if (type == mayBeAncestor)
				{
					return true;
				}

				if (type.GetIsGenericType() && type.GetGenericTypeDefinition() == mayBeAncestor)
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
		public bool Unregister(Type abstractCollectionType)
		{
			if (abstractCollectionType == null)
			{
				return false;
			}

			return this.DefaultCollectionTypes.Unregister(abstractCollectionType);
		}
	}
}
