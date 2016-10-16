#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke and contributors
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.CodeGenerators
{
	internal abstract class SerializerGenerationLogic<TConfig>
		where TConfig : class, ISerializerGeneratorConfiguration
	{
		protected abstract EmitterFlavor EmitterFlavor { get; }

		public IEnumerable<SerializerCodeGenerationResult> Generate( IEnumerable<Type> targetTypes, TConfig configuration )
		{
			if ( targetTypes == null )
			{
				throw new ArgumentNullException( "targetTypes" );
			}

			if ( configuration == null )
			{
				throw new ArgumentNullException( "configuration" );
			}

			configuration.Validate();
			var context =
				new SerializationContext
				{
#if !NETSTANDARD1_3
					GeneratorOption = SerializationMethodGeneratorOption.CanDump,
#endif // !NETSTANDARD1_3
					EnumSerializationMethod = configuration.EnumSerializationMethod,
					SerializationMethod = configuration.SerializationMethod
				};
			context.SerializerOptions.EmitterFlavor = this.EmitterFlavor;
			context.CompatibilityOptions.AllowNonCollectionEnumerableTypes = configuration.CompatibilityOptions.AllowNonCollectionEnumerableTypes;
			context.CompatibilityOptions.IgnorePackabilityForCollection = configuration.CompatibilityOptions.IgnorePackabilityForCollection;
			context.CompatibilityOptions.OneBoundDataMemberOrder = configuration.CompatibilityOptions.OneBoundDataMemberOrder;
			context.CompatibilityOptions.PackerCompatibilityOptions = configuration.CompatibilityOptions.PackerCompatibilityOptions;
#if FEATURE_TAP
			context.SerializerOptions.WithAsync = configuration.WithAsync;
#endif // FEATURE_TAP

			IEnumerable<Type> realTargetTypes;
			if ( configuration.IsRecursive )
			{
				realTargetTypes =
					targetTypes
						.SelectMany( t => ExtractElementTypes( context, configuration, t ) );
			}
			else
			{
				realTargetTypes =
					targetTypes
						.Where( t => !SerializationTarget.BuiltInSerializerExists( configuration, t, t.GetCollectionTraits( CollectionTraitOptions.None, context.CompatibilityOptions.AllowNonCollectionEnumerableTypes ) ) );
			}

			var generationContext = this.CreateGenerationContext( context, configuration );
			var generatorFactory = this.CreateGeneratorFactory( context );

			foreach ( var targetType in realTargetTypes.Distinct() )
			{
				var generator = generatorFactory( targetType );

				var concreteType = default( Type );
				if ( targetType.GetIsInterface() || targetType.GetIsAbstract() )
				{
					concreteType = context.DefaultCollectionTypes.GetConcreteType( targetType );
				}

				generator.BuildSerializerCode( generationContext, concreteType, null );
			}

			Directory.CreateDirectory( configuration.OutputDirectory );

			return generationContext.Generate();
		}

		private static IEnumerable<Type> ExtractElementTypes( SerializationContext context, ISerializerGeneratorConfiguration configuration, Type type )
		{
			if ( !SerializationTarget.BuiltInSerializerExists( configuration, type, type.GetCollectionTraits( CollectionTraitOptions.None, context.CompatibilityOptions.AllowNonCollectionEnumerableTypes ) ) )
			{
				yield return type;

				// Search dependents recursively if the type is NOT enum.
				if ( !type.GetIsEnum() )
				{
					foreach (
						var dependentType in
						SerializationTarget.Prepare( context, type )
							.Members.Where( m => m.Member != null ).SelectMany( m => ExtractElementTypes( context, configuration, m.Member.GetMemberValueType() ) )
					)
					{
						yield return dependentType;
					}
				}
			}

			if ( type.IsArray )
			{
				var elementType = type.GetElementType();
				if ( !SerializationTarget.BuiltInSerializerExists( configuration, elementType, elementType.GetCollectionTraits( CollectionTraitOptions.None, allowNonCollectionEnumerableTypes: false ) ) )
				{
					foreach ( var descendant in ExtractElementTypes( context, configuration, elementType ) )
					{
						yield return descendant;
					}

					yield return elementType;
				}

				yield break;
			}

			if ( type.GetIsGenericType() )
			{
				// Search generic arguments recursively.
				foreach ( var elementType in type.GetGenericArguments().SelectMany( g => ExtractElementTypes( context, configuration, g ) ) )
				{
					yield return elementType;
				}
			}

			if ( configuration.WithNullableSerializers && type.GetIsValueType() && Nullable.GetUnderlyingType( type ) == null )
			{
				// Retrun nullable companion even if they always have built-in serializers.
				yield return typeof( Nullable<> ).MakeGenericType( type );
			}
		}

		protected abstract ISerializerCodeGenerationContext CreateGenerationContext( SerializationContext context, TConfig configuration );

		protected abstract Func<Type, ISerializerCodeGenerator> CreateGeneratorFactory( SerializationContext context );
	}
}