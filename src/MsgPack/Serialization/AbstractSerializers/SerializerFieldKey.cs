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

namespace MsgPack.Serialization.AbstractSerializers
{
	/// <summary>
	///		Represents dictionary key to remember fields which store dependent serializer instance.
	/// </summary>
	internal struct SerializerFieldKey : IEquatable<SerializerFieldKey>
	{
		/// <summary>
		///		Type of serializing/deserializing type. 
		/// </summary>
		public readonly RuntimeTypeHandle TypeHandle;

		/// <summary>
		///		Enum serialization method for specific member.
		/// </summary>
		public readonly EnumMemberSerializationMethod EnumSerializationMethod;

		public SerializerFieldKey( Type targetType, EnumMemberSerializationMethod enumMemberSerializationMethod )
		{
			this.TypeHandle = targetType.TypeHandle;
			this.EnumSerializationMethod = enumMemberSerializationMethod;
		}

		public bool Equals( SerializerFieldKey other )
		{
			// ReSharper disable once ImpureMethodCallOnReadonlyValueField
			return
				this.TypeHandle.Equals( other.TypeHandle )
				&& this.EnumSerializationMethod == other.EnumSerializationMethod;
		}

		public override bool Equals( object obj )
		{
			if (!( obj is SerializerFieldKey ) )
			{
				return false;
			}

			return this.Equals( ( SerializerFieldKey )obj );
		}

		public override int GetHashCode()
		{
			return this.TypeHandle.GetHashCode() ^ this.EnumSerializationMethod.GetHashCode();
		}
	}
}