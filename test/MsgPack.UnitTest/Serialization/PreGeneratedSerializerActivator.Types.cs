 
 
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
			return result;
		}
	}
}
