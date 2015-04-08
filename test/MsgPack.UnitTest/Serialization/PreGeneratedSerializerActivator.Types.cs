
 
 
#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MsgPack.Serialization
{
	internal static partial class PreGeneratedSerializerActivator
	{
		private static IList<Type> InitializeKnownTypes()
		{
			var result = new List<Type>();
			result.Add( typeof( AddOnlyCollection<System.Object> ) ); 
			result.Add( typeof( AddOnlyCollection<System.Object[]> ) ); 
			result.Add( typeof( SimpleCollection<System.Object> ) ); 
			result.Add( typeof( SimpleCollection<System.Object[]> ) ); 
			result.Add( typeof( Collection<System.Object> ) ); 
			result.Add( typeof( Collection<System.Object[]> ) ); 
			result.Add( typeof( List<System.Object> ) ); 
			result.Add( typeof( List<System.Object[]> ) ); 
			result.Add( typeof( HashSet<System.Object> ) ); 
			result.Add( typeof( HashSet<System.Object[]> ) ); 
			result.Add( typeof( ObservableCollection<System.Object> ) ); 
			result.Add( typeof( ObservableCollection<System.Object[]> ) ); 
			result.Add( typeof( StringKeyedCollection<System.Object> ) ); 
			result.Add( typeof( StringKeyedCollection<System.Object[]> ) ); 
			result.Add( typeof( AddOnlyCollection<System.DateTime> ) ); 
			result.Add( typeof( AddOnlyCollection<System.DateTime[]> ) ); 
			result.Add( typeof( SimpleCollection<System.DateTime> ) ); 
			result.Add( typeof( SimpleCollection<System.DateTime[]> ) ); 
			result.Add( typeof( Collection<System.DateTime> ) ); 
			result.Add( typeof( Collection<System.DateTime[]> ) ); 
			result.Add( typeof( List<System.DateTime> ) ); 
			result.Add( typeof( List<System.DateTime[]> ) ); 
			result.Add( typeof( HashSet<System.DateTime> ) ); 
			result.Add( typeof( HashSet<System.DateTime[]> ) ); 
			result.Add( typeof( ObservableCollection<System.DateTime> ) ); 
			result.Add( typeof( ObservableCollection<System.DateTime[]> ) ); 
			result.Add( typeof( StringKeyedCollection<System.DateTime> ) ); 
			result.Add( typeof( StringKeyedCollection<System.DateTime[]> ) ); 
			result.Add( typeof( AddOnlyCollection<MessagePackObject> ) ); 
			result.Add( typeof( AddOnlyCollection<MessagePackObject[]> ) ); 
			result.Add( typeof( SimpleCollection<MessagePackObject> ) ); 
			result.Add( typeof( SimpleCollection<MessagePackObject[]> ) ); 
			result.Add( typeof( Collection<MessagePackObject> ) ); 
			result.Add( typeof( Collection<MessagePackObject[]> ) ); 
			result.Add( typeof( List<MessagePackObject[]> ) ); 
			result.Add( typeof( HashSet<MessagePackObject> ) ); 
			result.Add( typeof( HashSet<MessagePackObject[]> ) ); 
			result.Add( typeof( ObservableCollection<MessagePackObject> ) ); 
			result.Add( typeof( ObservableCollection<MessagePackObject[]> ) ); 
			result.Add( typeof( StringKeyedCollection<MessagePackObject> ) ); 
			result.Add( typeof( StringKeyedCollection<MessagePackObject[]> ) ); 
			result.Add( typeof( AddOnlyCollection<System.Int32> ) ); 
			result.Add( typeof( AddOnlyCollection<System.Int32[]> ) ); 
			result.Add( typeof( SimpleCollection<System.Int32> ) ); 
			result.Add( typeof( SimpleCollection<System.Int32[]> ) ); 
			result.Add( typeof( Collection<System.Int32> ) ); 
			result.Add( typeof( Collection<System.Int32[]> ) ); 
			result.Add( typeof( List<System.Int32> ) ); 
			result.Add( typeof( List<System.Int32[]> ) ); 
			result.Add( typeof( HashSet<System.Int32> ) ); 
			result.Add( typeof( HashSet<System.Int32[]> ) ); 
			result.Add( typeof( ObservableCollection<System.Int32> ) ); 
			result.Add( typeof( ObservableCollection<System.Int32[]> ) ); 
			result.Add( typeof( StringKeyedCollection<System.Int32> ) ); 
			result.Add( typeof( StringKeyedCollection<System.Int32[]> ) ); 
			result.Add( typeof( Dictionary<MessagePackObject, MessagePackObject> ) );
			result.Add( typeof( Dictionary<object, object> ) );
			result.Add( typeof( Dictionary<String, DateTime> ) );
			result.Add( typeof( Dictionary<String, int> ) );
			result.Add( typeof( ComplexType ) ); 
			result.Add( typeof( ComplexTypeGenerated ) ); 
			result.Add( typeof( ComplexTypeGeneratedEnclosure ) ); 
			result.Add( typeof( ComplexTypeWithDataContract ) ); 
			result.Add( typeof( ComplexTypeWithDataContractWithOrder ) ); 
			result.Add( typeof( ComplexTypeWithOneBaseOrder ) ); 
			result.Add( typeof( ComplexTypeWithNonSerialized ) ); 
			result.Add( typeof( ComplexTypeWithTwoMember ) ); 
			result.Add( typeof( ComplexTypeWithoutAnyAttribute ) ); 
			result.Add( typeof( DataMemberAttributeNamedPropertyTestTarget ) ); 
			result.Add( typeof( Image ) ); 
			result.Add( typeof( TestValueType ) ); 
			result.Add( typeof( DayOfWeek ) ); 
			result.Add( typeof( VersioningTestTarget ) ); 
			result.Add( typeof( EnumDefault ) ); 
			result.Add( typeof( EnumByName ) ); 
			result.Add( typeof( EnumByUnderlyingValue ) ); 
			result.Add( typeof( EnumMemberObject ) ); 
			result.Add( typeof( EnumByte ) ); 
			result.Add( typeof( EnumSByte ) ); 
			result.Add( typeof( EnumInt16 ) ); 
			result.Add( typeof( EnumUInt16 ) ); 
			result.Add( typeof( EnumInt32 ) ); 
			result.Add( typeof( EnumUInt32 ) ); 
			result.Add( typeof( EnumInt64 ) ); 
			result.Add( typeof( EnumUInt64 ) ); 
			result.Add( typeof( EnumByteFlags ) ); 
			result.Add( typeof( EnumSByteFlags ) ); 
			result.Add( typeof( EnumInt16Flags ) ); 
			result.Add( typeof( EnumUInt16Flags ) ); 
			result.Add( typeof( EnumInt32Flags ) ); 
			result.Add( typeof( EnumUInt32Flags ) ); 
			result.Add( typeof( EnumInt64Flags ) ); 
			result.Add( typeof( EnumUInt64Flags ) ); 
			result.Add( typeof( ListValueType<int> ) );
			result.Add( typeof( DictionaryValueType<int, int> ) );
			result.Add( typeof( WithAbstractInt32Collection ) );
			result.Add( typeof( WithAbstractStringCollection ) );
			result.Add( typeof( WithAbstractNonCollection ) );
			result.Add( typeof( HasEnumerable ) );
			result.Add( typeof( Outer ) );
			result.Add( typeof( Inner ) );
			result.Add( typeof( PlainClass ) );
			result.Add( typeof( AnnotatedClass ) );
			result.Add( typeof( DataMamberClass ) );
		#region -- Polymorphism --
		#region ---- KnownType ----

		#region ------ KnownType.NormalTypes ------
		#endregion ------ KnownType.NormalTypes ------

		#region ------ KnownType.CollectionTypes ------
		#endregion ------ KnownType.CollectionTypes ------

		#region ------ KnownType.DictionaryTypes ------
		#endregion ------ KnownType.DictionaryTypes ------

		#region ------ KnownType.TupleTypes ------
		#endregion ------ KnownType.TupleTypes ------

		#endregion ---- KnownType ----
		#region ---- RuntimeType ----

		#region ------ RuntimeType.NormalTypes ------
		#endregion ------ RuntimeType.NormalTypes ------

		#region ------ RuntimeType.CollectionTypes ------
		#endregion ------ RuntimeType.CollectionTypes ------

		#region ------ RuntimeType.DictionaryTypes ------
		#endregion ------ RuntimeType.DictionaryTypes ------

		#region ------ RuntimeType.TupleTypes ------
		#endregion ------ RuntimeType.TupleTypes ------

		#endregion ---- RuntimeType ----

		#endregion -- Polymorphism --
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_ReferenceReadWritePropertyAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_ReferenceReadWriteFieldAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_ReferenceGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_ReferenceGetOnlyPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_ReferencePrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_ReferencePrivateSetterPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_ReferenceReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_ReferenceReadOnlyFieldAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_ValueReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_ValueReadWritePropertyAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_ValueReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_ValueReadWriteFieldAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_ValueGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_ValueGetOnlyPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_ValuePrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_ValuePrivateSetterPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_ValueReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_ValueReadOnlyFieldAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_PrimitiveReadWritePropertyAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_PrimitiveReadWriteFieldAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_PrimitiveGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_PrimitiveGetOnlyPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_PrimitivePrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_PrimitivePrivateSetterPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_PrimitiveReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_PrimitiveReadOnlyFieldAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_StringReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_StringReadWritePropertyAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_StringReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_StringReadWriteFieldAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_StringGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_StringGetOnlyPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_StringPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_StringPrivateSetterPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_StringReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_StringReadOnlyFieldAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_PolymorphicReadWritePropertyAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_PolymorphicReadWriteFieldAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_PolymorphicGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_PolymorphicGetOnlyPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_PolymorphicPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_PolymorphicPrivateSetterPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Normal_PolymorphicReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_PolymorphicReadOnlyFieldAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListStaticItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListStaticItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListObjectItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListObjectItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListObjectItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListObjectItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListObjectItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListObjectItselfReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListObjectItselfReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListObjectItselfGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListObjectItselfPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Collection_ListObjectItselfReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryObjectKeyAndStaticItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryObjectKeyAndStaticItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryObjectKeyAndStaticItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryObjectKeyAndStaticItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryObjectKeyAndStaticItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndObjectItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndObjectItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndObjectItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndObjectItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndObjectItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryObjectKeyAndItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryObjectKeyAndItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryObjectKeyAndItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryObjectKeyAndItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryObjectKeyAndItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryObjectItselfReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryObjectItselfReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryObjectItselfGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryObjectItselfPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Dictionary_DictionaryObjectItselfReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_ReferenceReadWritePropertyAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_ReferenceReadWriteFieldAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_ReferenceGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_ReferenceGetOnlyPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_ReferencePrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_ReferencePrivateSetterPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_ReferenceReadOnlyFieldAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_ValueReadWritePropertyAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_ValueReadWriteFieldAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_ValueGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_ValueGetOnlyPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_ValuePrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_ValuePrivateSetterPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_ValueReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_ValueReadOnlyFieldAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_PrimitiveReadWritePropertyAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_PrimitiveReadWriteFieldAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_PrimitiveGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_PrimitiveGetOnlyPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_PrimitivePrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_PrimitivePrivateSetterPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_PrimitiveReadOnlyFieldAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_StringReadWritePropertyAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_StringReadWriteFieldAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_StringGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_StringGetOnlyPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_StringPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_StringPrivateSetterPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_StringReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_StringReadOnlyFieldAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_PolymorphicReadWritePropertyAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_PolymorphicReadWriteFieldAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_PolymorphicGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_PolymorphicGetOnlyPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_PolymorphicPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_PolymorphicPrivateSetterPropertyAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_PolymorphicReadOnlyFieldAndConstructorAsObject ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListObjectItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListObjectItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListObjectItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListObjectItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListObjectItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListObjectItselfReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListObjectItselfReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListObjectItselfGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListObjectItselfPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Collection_ListObjectItselfReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryObjectKeyAndStaticItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryObjectKeyAndStaticItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryObjectKeyAndStaticItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryObjectKeyAndStaticItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryObjectKeyAndStaticItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndObjectItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndObjectItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndObjectItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndObjectItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndObjectItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryObjectKeyAndItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryObjectKeyAndItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryObjectKeyAndItemGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryObjectKeyAndItemPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryObjectKeyAndItemReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryObjectItselfReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryObjectItselfReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryObjectItselfGetOnlyCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryObjectItselfPrivateSetterCollectionProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryObjectItselfReadOnlyCollectionField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteProperty ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteField ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor ) ); 
			result.Add( typeof( PolymorphicMemberTypeMixed ) ); 
			result.Add( typeof( AbstractClassMemberNoAttribute ) ); 
			result.Add( typeof( AbstractClassMemberKnownType ) ); 
			result.Add( typeof( AbstractClassMemberRuntimeType ) ); 
			result.Add( typeof( AbstractClassCollectionItemNoAttribute ) ); 
			result.Add( typeof( AbstractClassCollectionItemKnownType ) ); 
			result.Add( typeof( AbstractClassCollectionItemRuntimeType ) ); 
			result.Add( typeof( AbstractClassDictionaryKeyNoAttribute ) ); 
			result.Add( typeof( AbstractClassDictionaryKeyKnownType ) ); 
			result.Add( typeof( AbstractClassDictionaryKeyRuntimeType ) ); 
			result.Add( typeof( InterfaceMemberNoAttribute ) ); 
			result.Add( typeof( InterfaceMemberKnownType ) ); 
			result.Add( typeof( InterfaceMemberRuntimeType ) ); 
			result.Add( typeof( InterfaceCollectionItemNoAttribute ) ); 
			result.Add( typeof( InterfaceCollectionItemKnownType ) ); 
			result.Add( typeof( InterfaceCollectionItemRuntimeType ) ); 
			result.Add( typeof( InterfaceDictionaryKeyNoAttribute ) ); 
			result.Add( typeof( InterfaceDictionaryKeyKnownType ) ); 
			result.Add( typeof( InterfaceDictionaryKeyRuntimeType ) ); 
			result.Add( typeof( EchoKeyedCollection<> ) ); 
			result.Add( typeof( TupleAbstractType ) ); 
			result.Add( typeof( DuplicatedKnownMember ) ); 
			result.Add( typeof( DuplicatedKnownCollectionItem ) ); 
			result.Add( typeof( DuplicatedKnownDictionaryKey ) ); 
			result.Add( typeof( DuplicatedKnownTupleItem ) ); 
			result.Add( typeof( KnownAndRuntimeMember ) ); 
			result.Add( typeof( KnownAndRuntimeCollectionItem ) ); 
			result.Add( typeof( KnownAndRuntimeDictionaryKey ) ); 
			result.Add( typeof( KnownAndRuntimeTupleItem ) ); 
			return result;
		}
	}
}
