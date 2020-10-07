// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	[Obsolete("Use IObjectSerializerProvider")]
	public sealed class ObjectSerializationContext
	{
		public SerializationOptions Options { get; }

		public ObjectSerializer<T> GetSerializer<T>(object? providerParameter)
		{
			throw new NotImplementedException();
		}

		public ObjectSerializer GetSerializer(Type targetType, object? providerParameter)
		{
			throw new NotImplementedException();
		}
	}

	internal static class SerializationMetadataFactory
	{
		// Facade for metadata retrieval.

#warning TODO: Move factory logic from SerializationTarget

		public static NameMapper GetEnumMetadata(Type enumType, IEnumSerializationOptions options)
		{
			Debug.Assert(enumType.IsEnum);
			return NameMapper.FromEnum(enumType, options.NameTransformer, options.IgnoresCaseOnDeserialization);
		}

		public static SerializationTarget GetObjectMetadata(Type targetType, ISerializerGenerationOptions options)
			=> SerializationTarget.Prepare(options, targetType);

		public static SerializationTarget GetTupleMetadata(Type tupleType)
			=> SerializationTarget.CreateForTuple(tupleType);

		public static SerializationTarget GetCollectionMetadata(Type collectionInstanceType, CollectionTraits traits, ISerializerGenerationOptions options)
		{
			bool canDeserialize;
			var collectionConstructor = GetCollectionConstructor(collectionInstanceType);
			if (collectionConstructor == null)
			{
				if (!options.AllowsAsymmetricSerializer)
				{
					Throw.TargetDoesNotHavePublicDefaultConstructorNorInitialCapacity(collectionInstanceType);
				}

				// Serialize only.
				canDeserialize = false;
			}
			else
			{
				canDeserialize = true;
			}
			
			return SerializationTarget.CreateForCollection(collectionInstanceType, traits, collectionConstructor, canDeserialize);
		}

		private static ConstructorInfo? GetCollectionConstructor(Type collectionInstanceType)
		{
			static bool IsIEqualityComparer(Type type)
				=> type.GetIsGenericType() && type.GetGenericTypeDefinition() == typeof(IEqualityComparer<>);

			const int noParameters = 0;
			const int withCapacity = 10;
			const int withComparer = 11;
			const int withComparerAndCapacity = 20;
			const int withCapacityAndComparer = 21;

			ConstructorInfo? constructor = null;
			var currentScore = -1;

			foreach (var candidate in collectionInstanceType.GetConstructors())
			{
				var parameters = candidate.GetParameters();
				switch (parameters.Length)
				{
					case 0:
					{
						if (currentScore < noParameters)
						{
							constructor = candidate;
							currentScore = noParameters;
						}

						break;
					}
					case 1:
					{
						if (currentScore < withCapacity && parameters[0].ParameterType == typeof(int))
						{
							constructor = candidate;
							currentScore = noParameters;
						}
						else if (currentScore < withComparer && IsIEqualityComparer(parameters[0].ParameterType))
						{
							constructor = candidate;
							currentScore = noParameters;
						}
						break;
					}
					case 2:
					{
						if (currentScore < withCapacityAndComparer && parameters[0].ParameterType == typeof(int) && IsIEqualityComparer(parameters[1].ParameterType))
						{
							constructor = candidate;
							currentScore = withCapacityAndComparer;
						}
						else if (currentScore < withComparerAndCapacity && parameters[1].ParameterType == typeof(int) && IsIEqualityComparer(parameters[0].ParameterType))
						{
							constructor = candidate;
							currentScore = withComparerAndCapacity;
						}

						break;
					}
				}
			}

			return constructor;
		}
	}

	/// <summary>
	///		Implementing name mapping logic.
	/// </summary>
	internal sealed class NameMapper
	{
		private readonly Dictionary<string, string> _originalToSerializedMapping;
		private readonly Dictionary<string, string> _serializedToOriginalMapping;

		private NameMapper(bool isCaseInsensitive)
		{
			var comparer = isCaseInsensitive ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
			this._originalToSerializedMapping = new Dictionary<string, string>(comparer);
			this._serializedToOriginalMapping = new Dictionary<string, string>(comparer);
		}

		public string SerializeName(string originalName)
		{
			if (!this._originalToSerializedMapping.TryGetValue(originalName, out var result))
			{
				Throw.UnknownOriginalName(originalName);
			}

			return result!;
		}

		public string DeserializeName(string serializedName)
		{
			if (!this._serializedToOriginalMapping.TryGetValue(serializedName, out var result))
			{
				Throw.UnknownSerializedName(serializedName);
			}

			return result!;
		}

		public static NameMapper FromEnum(Type enumType, Func<string, string> nameTransformer, bool isCaseInsensitive = false)
		{
			Debug.Assert(enumType != null);
			Debug.Assert(enumType.IsEnum);

			return
				Initialize(
					enumType.GetFields(BindingFlags.Public | BindingFlags.Static)
					.Select(f =>
						{
							var enumMemberAttribute = f.GetCustomAttribute<EnumMemberAttribute>();
							if (enumMemberAttribute == null
								|| !enumMemberAttribute.IsValueSetExplicitly
								|| String.IsNullOrEmpty(enumMemberAttribute.Value))
							{
								return (MemberName: f.Name, ExplicitName: default(string?));
							}

							return (MemberName: f.Name, ExplicitName: enumMemberAttribute.Value);
						}
					),
					nameTransformer,
					isCaseInsensitive
				);
		}

		public static NameMapper FromObjectMembers(IEnumerable<SerializingMember> members, Func<string, string> nameTransformer, bool isCaseInsensitive = false)
			=> Initialize(
				members.Select(m => (MemberName: m.Member.Name, ExplicitName: m.MemberName != m.Member.Name ? m.MemberName : null)),
				nameTransformer,
				isCaseInsensitive
			);


		private static NameMapper Initialize(IEnumerable<(string MemberName, string? ExplicitName)> names, Func<string, string> nameTransformer, bool isCaseSensitive)
		{
			var mapper = new NameMapper(isCaseSensitive);

			foreach (var name in names)
			{
				mapper.Register(name.MemberName, name.ExplicitName ?? nameTransformer(name.MemberName));
			}

			return mapper;
		}

		private void Register(string originalName, string serializedName)
		{
			if (!this._originalToSerializedMapping.TryAdd(originalName, serializedName))
			{
				Throw.DuplicatedOriginalName(originalName);
			}

			if (!this._serializedToOriginalMapping.TryAdd(serializedName, originalName))
			{
				// rollback
				this._originalToSerializedMapping.Remove(originalName);
				Throw.DuplicatedSerializedName(serializedName);
			}
		}
	}
	// TODO: SerializationContext -> SerializerProvider
	// Sergen
	public abstract class SerializerProviderBuilder
	{
		internal SerializerGenerationOptionsBuilder? OptionsBuilder { get; private set; }

		protected SerializerProvider BuildCore() => new SerializerProvider(this.GetInitializedBuilder());

		protected virtual void ConfigureDefault(SerializerGenerationOptionsBuilder builder)
		{
			// nop
		}

		private SerializerGenerationOptionsBuilder GetInitializedBuilder()
		{
			var result = this.OptionsBuilder;
			if (result == null)
			{
				result = this.OptionsBuilder = new SerializerGenerationOptionsBuilder();
				this.ConfigureDefault(result);
			}

			return result;
		}

		public void Configure(Action<SerializerGenerationOptionsBuilder> configure)
			=> Ensure.NotNull(configure).Invoke(this.GetInitializedBuilder());
	}

	internal sealed class GenericSerializerProviderBuilder : SerializerProviderBuilder
	{
		public SerializerProvider Build() => this.BuildCore();
	}

}
