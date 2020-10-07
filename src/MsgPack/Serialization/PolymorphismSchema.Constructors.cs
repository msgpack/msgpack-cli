// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MsgPack.Serialization
{
	public partial class PolymorphismSchema
	{
		private static readonly Dictionary<string, Type> EmptyMap = new Dictionary<string, Type>(0);
		private static readonly PolymorphismSchema[] EmptyChildren = new PolymorphismSchema[0];

		// Internal only, for Default
		private PolymorphismSchema()
		{
			this.TargetType = null;
			this.PolymorphismType = PolymorphismType.None;
			this.ChildrenType = PolymorphismSchemaChildrenType.None;
#if !UNITY
			this._codeTypeMapping = new ReadOnlyDictionary<string, Type>(EmptyMap);
			this._children = new ReadOnlyCollection<PolymorphismSchema>(EmptyChildren);
#else
			// Work-around Unity type intiialization issue.
			this._codeTypeMapping = new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>(0));
			this._children = new ReadOnlyCollection<PolymorphismSchema>(new PolymorphismSchema[0]);
#endif
		}

		// Aggregate
		private PolymorphismSchema(
			Type targetType,
			PolymorphismType polymorphismType,
			Func<PolymorphicTypeVerificationContext, bool>? typeVerifier,
			PolymorphismSchemaChildrenType childrenType,
			params PolymorphismSchema[] childItemSchemaList
		)
			: this(
				targetType,
				polymorphismType,
				new ReadOnlyDictionary<string, Type>(EmptyMap),
				typeVerifier,
				childrenType,
				new ReadOnlyCollection<PolymorphismSchema>(
					(childItemSchemaList ?? EmptyChildren).Select(x => x ?? Default).ToArray()
				)
			)
		{ }

		private PolymorphismSchema(
			Type targetType,
			PolymorphismType polymorphismType,
			IDictionary<string, Type> codeTypeMapping,
			Func<PolymorphicTypeVerificationContext, bool>? typeVerifier,
			PolymorphismSchemaChildrenType childrenType,
			params PolymorphismSchema[] childItemSchemaList
		)
			: this(
				targetType,
				polymorphismType,
				new ReadOnlyDictionary<string, Type>(codeTypeMapping),
				typeVerifier,
				childrenType,
				new ReadOnlyCollection<PolymorphismSchema>(
					(childItemSchemaList ?? EmptyChildren).Select(x => x ?? Default).ToArray()
				)
			)
		{ }

		private PolymorphismSchema(
			Type targetType,
			PolymorphismType polymorphismType,
			ReadOnlyDictionary<string, Type> codeTypeMapping,
			Func<PolymorphicTypeVerificationContext, bool>? typeVerifier,
			PolymorphismSchemaChildrenType childrenType,
			ReadOnlyCollection<PolymorphismSchema> childItemSchemaList
		)
		{
			this.TargetType = Ensure.NotNull(targetType);
			this.PolymorphismType = polymorphismType;
			this._codeTypeMapping = codeTypeMapping;
			this.ChildrenType = childrenType;
			this._children = childItemSchemaList;
			this.TypeVerifier = typeVerifier ?? DefaultTypeVerfiier;
		}

		// Plane

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for non-collection object which uses type embedding based polymorphism.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for non-collection object which uses type embedding based polymorphism.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		public static PolymorphismSchema ForPolymorphicObject(Type targetType)
			=> new PolymorphismSchema(targetType, PolymorphismType.RuntimeType, DefaultTypeVerfiier, PolymorphismSchemaChildrenType.None);

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for non-collection object which uses type embedding based polymorphism.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="typeVerifier">The delegate which verifies loading type in runtime type polymorphism.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for non-collection object which uses type embedding based polymorphism.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		public static PolymorphismSchema ForPolymorphicObject(Type targetType, Func<PolymorphicTypeVerificationContext, bool> typeVerifier)
			=> new PolymorphismSchema(targetType, PolymorphismType.RuntimeType, typeVerifier, PolymorphismSchemaChildrenType.None);

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for non-collection object which uses ext-type code mapping based polymorphism.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="codeTypeMapping">The code-type mapping which maps between ext-type codes and .NET <see cref="Type"/>s.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for non-collection object which uses ext-type code mapping based polymorphism.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		public static PolymorphismSchema ForPolymorphicObject(Type targetType, IDictionary<string, Type> codeTypeMapping)
			=> new PolymorphismSchema(
				targetType,
				PolymorphismType.KnownTypes,
				codeTypeMapping,
				DefaultTypeVerfiier,
				PolymorphismSchemaChildrenType.None
			);

		// Collection items

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for collection object which uses declared type or context specified concrete type.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="itemSchema">The schema for collection items of the serialization target collection.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for collection object which uses declared type or context specified concrete type.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		public static PolymorphismSchema ForContextSpecifiedCollection(Type targetType, PolymorphismSchema itemSchema)
			=> new PolymorphismSchema(
				targetType,
				PolymorphismType.None,
				DefaultTypeVerfiier,
				PolymorphismSchemaChildrenType.CollectionItems,
				itemSchema
			);

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for collection object which uses type embedding based polymorphism.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="itemSchema">The schema for collection items of the serialization target collection.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for collection object which uses type embedding based polymorphism.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		public static PolymorphismSchema ForPolymorphicCollection(Type targetType, PolymorphismSchema itemSchema)
			=> new PolymorphismSchema(
				targetType,
				PolymorphismType.RuntimeType,
				DefaultTypeVerfiier,
				PolymorphismSchemaChildrenType.CollectionItems,
				itemSchema
			);

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for collection object which uses type embedding based polymorphism.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="itemSchema">The schema for collection items of the serialization target collection.</param>
		/// <param name="typeVerifier">The delegate which verifies loading type in runtime type polymorphism.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for collection object which uses type embedding based polymorphism.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		public static PolymorphismSchema ForPolymorphicCollection(Type targetType, PolymorphismSchema itemSchema, Func<PolymorphicTypeVerificationContext, bool> typeVerifier)
			=> new PolymorphismSchema(
				targetType,
				PolymorphismType.RuntimeType,
				typeVerifier,
				PolymorphismSchemaChildrenType.CollectionItems,
				itemSchema
			);

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for collection object which uses ext-type code mapping based polymorphism.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="codeTypeMapping">The code type mapping which maps between ext-type codes and .NET <see cref="Type"/>s.</param>
		/// <param name="itemSchema">The schema for collection items of the serialization target collection.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for collection object which uses ext-type code mapping based polymorphism.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		public static PolymorphismSchema ForPolymorphicCollection(
			Type targetType,
			IDictionary<string, Type> codeTypeMapping,
			PolymorphismSchema itemSchema
		) => new PolymorphismSchema(
				targetType,
				PolymorphismType.KnownTypes,
				codeTypeMapping,
				DefaultTypeVerfiier,
				PolymorphismSchemaChildrenType.CollectionItems,
				itemSchema
			);

		// Dictionary key/values

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for dictionary object which uses declared type or context specified concrete type.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="keySchema">The schema for dictionary keys of the serialization target dictionary.</param>
		/// <param name="valueSchema">The schema for dictionary values of the serialization target dictionary.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for dictionary object which uses declared type or context specified concrete type.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		public static PolymorphismSchema ForContextSpecifiedDictionary(
			Type targetType,
			PolymorphismSchema keySchema,
			PolymorphismSchema valueSchema
		) => new PolymorphismSchema(
				targetType,
				PolymorphismType.None,
				DefaultTypeVerfiier,
				PolymorphismSchemaChildrenType.DictionaryKeyValues,
				keySchema,
				valueSchema
			);

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for dictionary object which uses type embedding based polymorphism.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="keySchema">The schema for dictionary keys of the serialization target dictionary.</param>
		/// <param name="valueSchema">The schema for dictionary values of the serialization target dictionary.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for dictionary object which uses type embedding based polymorphism.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		public static PolymorphismSchema ForPolymorphicDictionary(
			Type targetType,
			PolymorphismSchema keySchema,
			PolymorphismSchema valueSchema
		) => new PolymorphismSchema(
				targetType,
				PolymorphismType.RuntimeType,
				DefaultTypeVerfiier,
				PolymorphismSchemaChildrenType.DictionaryKeyValues,
				keySchema,
				valueSchema
			);

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for dictionary object which uses type embedding based polymorphism.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="keySchema">The schema for dictionary keys of the serialization target dictionary.</param>
		/// <param name="valueSchema">The schema for dictionary values of the serialization target dictionary.</param>
		/// <param name="typeVerifier">The delegate which verifies loading type in runtime type polymorphism.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for dictionary object which uses type embedding based polymorphism.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		public static PolymorphismSchema ForPolymorphicDictionary(
			Type targetType,
			PolymorphismSchema keySchema,
			PolymorphismSchema valueSchema,
			Func<PolymorphicTypeVerificationContext, bool> typeVerifier
		) => new PolymorphismSchema(
				targetType,
				PolymorphismType.RuntimeType,
				typeVerifier,
				PolymorphismSchemaChildrenType.DictionaryKeyValues,
				keySchema,
				valueSchema
			);

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for dictionary object which uses ext-type code mapping based polymorphism.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="codeTypeMapping">The code type mapping which maps between ext-type codes and .NET <see cref="Type"/>s.</param>
		/// <param name="keySchema">The schema for dictionary keys of the serialization target dictionary.</param>
		/// <param name="valueSchema">The schema for dictionary values of the serialization target dictionary.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for dictionary object which uses ext-type code mapping based polymorphism.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		public static PolymorphismSchema ForPolymorphicDictionary(
			Type targetType,
			IDictionary<string, Type> codeTypeMapping,
			PolymorphismSchema keySchema,
			PolymorphismSchema valueSchema
		) => new PolymorphismSchema(
				targetType,
				PolymorphismType.KnownTypes,
				codeTypeMapping,
				DefaultTypeVerfiier,
				PolymorphismSchemaChildrenType.DictionaryKeyValues,
				keySchema,
				valueSchema
			);

#if FEATURE_TUPLE
		// Tuple items

		/// <summary>
		///		Creates a new instance of the <see cref="PolymorphismSchema"/> class for <see cref="Tuple"/> object.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="itemSchemaList">The schema for collection items of the serialization target tuple. <c>null</c> or empty indicates all items do not have any polymorphism.</param>
		/// <returns>A new instance of the <see cref="PolymorphismSchema"/> class for <see cref="Tuple"/> object.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		/// <exception cref="System.ArgumentException">A count of <paramref name="itemSchemaList"/> does not match for an arity of the tuple type specified as <paramref name="targetType"/>.</exception>
		public static PolymorphismSchema ForPolymorphicTuple(Type targetType, PolymorphismSchema[] itemSchemaList)
		{
			VerifyArity(targetType, itemSchemaList);
			return
				new PolymorphismSchema(
					targetType,
					PolymorphismType.None,
					DefaultTypeVerfiier,
					PolymorphismSchemaChildrenType.TupleItems,
					itemSchemaList
				);
		}

		private static void VerifyArity(Type tupleType, ICollection<PolymorphismSchema> itemSchemaList)
		{
			if (itemSchemaList == null || itemSchemaList.Count == 0)
			{
				// OK
				return;
			}

			if (TupleItems.GetTupleItemTypes(tupleType).Count != itemSchemaList.Count)
			{
				throw new ArgumentException("An arity of itemSchemaList does not match for an arity of the tuple.", "itemSchemaList");
			}
		}
#endif // !NET35 && !UNITY

		internal PolymorphismSchema FilterSelf()
		{
			if (this == Default)
			{
				return this;
			}

			return new PolymorphismSchema(this.TargetType, PolymorphismType.None, this._codeTypeMapping, this.TypeVerifier, this.ChildrenType, this._children);
		}
	}
}
