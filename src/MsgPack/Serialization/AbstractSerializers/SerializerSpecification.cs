#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015 FUJIWARA, Yusuke
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
#if DEBUG
using System.Diagnostics.Contracts;
#endif // DEBUG

namespace MsgPack.Serialization.AbstractSerializers
{
	internal sealed class SerializerSpecification : IEquatable<SerializerSpecification>
	{
		public Type TargetType { get; private set; }
		public CollectionTraits TargetCollectionTraits { get; private set; }
		public string SerializerTypeFullName { get; private set; }
		public string SerializerTypeName { get; private set; }
		public string SerializerTypeNamespace { get; private set; }

		public SerializerSpecification( Type targetType, CollectionTraits targetCollectionTraits, string serializerTypeName, string serializerTypeNamespace )
		{
#if DEBUG
			Contract.Assert( targetType != null, "targetType != null" );
			Contract.Assert( targetCollectionTraits != null, "targetCollectionTraits != null" );
			Contract.Assert( serializerTypeName != null, "serializerTypeName != null" );
			Contract.Assert( serializerTypeNamespace != null, "serializerTypeNamespace != null" );
#endif // DEBUG 
			this.TargetType = targetType;
			this.TargetCollectionTraits = targetCollectionTraits;
			this.SerializerTypeName = serializerTypeName;
			this.SerializerTypeNamespace = serializerTypeNamespace;
			this.SerializerTypeFullName = 
				String.IsNullOrEmpty( serializerTypeNamespace )
				? serializerTypeName
				: serializerTypeNamespace + "." + serializerTypeName;
		}

		internal static SerializerSpecification CreateAnonymous( Type targetType, CollectionTraits targetCollectionTraits )
		{
			return new SerializerSpecification( targetType, targetCollectionTraits, "Anonymous", "AnonymousGeneratedSerializers" );
		}

		public override bool Equals( object obj )
		{
			return this.Equals( obj as SerializerSpecification );
		}

		public bool Equals( SerializerSpecification other )
		{
			return Equals( this, other );
		}

		public override int GetHashCode()
		{
			// TargetCollectionTraits is always derived from TargetType
			return this.TargetType.GetHashCode() ^ this.SerializerTypeName.GetHashCode() ^ this.SerializerTypeNamespace.GetHashCode();
		}

		public static bool Equals( SerializerSpecification left, SerializerSpecification right )
		{
			if ( Object.ReferenceEquals( left, right ) )
			{
				return true;
			}

			if ( Object.ReferenceEquals( left, null ) )
			{
				return Object.ReferenceEquals( right, null );
			}
			
			if ( Object.ReferenceEquals( right, null ) )
			{
				return false;
			}

			// TargetCollectionTraits is always derived from TargetType
			return
				left.TargetType == right.TargetType
				&& left.SerializerTypeName == right.SerializerTypeName
				&& left.SerializerTypeNamespace == right.SerializerTypeNamespace;
		}

		public static bool operator == (SerializerSpecification left, SerializerSpecification right)
		{
			return Equals( left, right );
		}

		public static bool operator !=( SerializerSpecification left, SerializerSpecification right )
		{
			return !Equals( left, right );
		}
	}
}
