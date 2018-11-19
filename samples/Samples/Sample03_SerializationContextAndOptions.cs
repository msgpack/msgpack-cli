#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
using System.Collections.ObjectModel;

using MsgPack;
using MsgPack.Serialization;
using NUnit.Framework; // For running checking

namespace Samples
{
	/// <summary>
	///	A sample code to describe SerializationContext usage.
	/// </summary>
	[TestFixture]
	public class SerializationContextAndOptionsSample
	{
		[Test]
		public void CustomizeSerializeBehavior()
		{
			// 1. To take advantage of SerializationContext, you should create own context to isolate others.
			//    Note that SerializationContext is thread safe.
			//    As you imagine, you can change 'default' settings by modifying properties of SerializationContext.Default.
			var context = new SerializationContext();

			// 2. Set options.

			// 2-1. If you prefer classic (before 0.6) behavor, use utility method as following
			// var context = SerializationContext.CreateClassicContext();

			//      Or, you can configure Default context classic as following
			// SerializationContext.ConfigureClassic();

			// 2-2. SerializationMethod: it changes comple type serialization method as array or map.
			//           Array(default): Space and time efficient, but depends on member declaration order so less version torrelant.
			//           Map : Less effitient, but more version torrelant (and easy to traverse as MesasgePackObject).
			context.SerializationMethod = SerializationMethod.Map;

			// 2-2-1. You can customize map key handling when you use SerializationMethod.Map to improve interoperability.
			//        You can use preconfigured transformation in DictionaryKeyTransformers class.
			context.DictionarySerializationOptions.KeyTransformer = DictionaryKeyTransformers.LowerCamel;
			// 2-2-2. You can omit entry when map value is null.
			context.DictionarySerializationOptions.OmitNullEntry = true;

			// 2-3. EnumSerializationMethod: it changes enum serialization as their name or underlying value.
			//          ByName(default): More version torrelant and interoperable, and backward compatible prior to 0.5 of MsgPack for CLI.
			//          ByUnderlyingValue: More efficient, but you should manage their underlying value and specify precise data contract between counterpart systems.
			context.EnumSerializationOptions.SerializationMethod = EnumSerializationMethod.ByName;

			// 2-3-1. You can customize enum naming casing for EnumSerializationMethod.ByName.
			//        You can use preconfigured transformation in EnumNameTransformers class.
			context.EnumSerializationOptions.NameTransformer = EnumNameTransformers.UpperSnake;

			// 2-4. There are many compatibility switches:

			// 2-4-1. If CompatibilityOptions.OneBoundDataMemberOrder is set, the origin DataMemberAttribute.Order becomes 1.
			//        It is compatibility options 1 base library like Proto-buf.NET.
			context.CompatibilityOptions.OneBoundDataMemberOrder = true;

			// 2-4-2. The CompatibilityOptions.PackerCompatibilityOptions control packer compatibility level.
			//        If you want to communicate with the library which only supports legacy message pack format spec, use PackerCompatibilityOptions.Classic flag set (default).
			//        If you want to utilize full feature including tiny string type, binary type, extended type, specify PackerCompatibilityOptions.None explicitly.
			context.CompatibilityOptions.PackerCompatibilityOptions = PackerCompatibilityOptions.None;

			// 2-4-3. As of 0.7, MsgPack for CLI accepts IEnumerable type which does not have Add method as non-collection object.
			//        You can disable this new behavior with setting AllowNonCollectionEnumerableTypes to false.
			context.CompatibilityOptions.AllowNonCollectionEnumerableTypes = false;

			// 2-4-4. As of 0.7, MsgPack for CLI respects IPackable/IUnpackable for collection types.
			//        You can disable this new behavior with setting IgnorePackabilityForCollection to true.
			context.CompatibilityOptions.IgnorePackabilityForCollection = true;

			// 2-4-5. As of 0.9, MsgPack for CLI allows types which cannot be deserialized.
			//        It generates asymmetric serializer which can only serialize object.
			//        You can disable this new behavior with setting AllowAsymmetricSerializer to false.
			context.CompatibilityOptions.AllowAsymmetricSerializer = false;
			// Note: You can check capability of the generated serializer with MessagePackSerializer.Capabilities property.

			// 2-5. You can tweak default concrete collection types for collection interfaces including IEnumerable<T>, IList, etc.
			context.DefaultCollectionTypes.Unregister( typeof( IList<> ) );
			context.DefaultCollectionTypes.Register( typeof( IList<> ), typeof( Collection<> ) );

			// 2-6. You can change default DateTime serialization method.
			//          Native: Use DateTime.ToBinary() based, it is precise but not interoperable
			//          UnixEpoc: Use milliseconds Unix epoc from 1970-01-01 for interoperability.
			context.DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native;

			// 2-7. You can tweak behaviors in code generation for serializer. These are advanced option.

			// 2-7-1. You can suppress asynchronous method generation. This is useful to generate serializer code for .NET 3.5/Unity.
			context.SerializerOptions.WithAsync = false;

			// 2-7-2. You can tweak runtime serializer code(IL) generation behavior. This is very advanced feature.
			context.SerializerOptions.GeneratorOption = SerializationMethodGeneratorOption.CanCollect;

			// 2-8. You can use SerializationContext as repository of msgpack ext type mappings.
			//      You must implement custom serializer to handle ext types correctly. See sample 04 to implement custom serializers.
			context.ExtTypeCodeMapping.Add( "EventTime", 1 );

			// 3. Get a serializer instance with customized settings.
			var serializer = MessagePackSerializer.Get<PhotoEntry>( context );

			// Following instructions are omitted... see sample 01.
		}
	}
}
