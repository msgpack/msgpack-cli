#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	/// <summary>
	///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		Defines common exception factory methods.
	/// </summary>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static class SerializationExceptions
	{
		internal static readonly MethodInfo NewValueTypeCannotBeNullMethod = FromExpression.ToMethod( ( string name, Type memberType, Type declaringType ) => NewValueTypeCannotBeNull( name, memberType, declaringType ) );

		public static SerializationException NewValueTypeCannotBeNull( string name, Type memberType, Type declaringType )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Member '{0}' of type '{1}' cannot be null because it is value type('{2}').", name, declaringType, memberType ) );
		}

		internal static readonly MethodInfo NewValueCannotMarshalMethod = FromExpression.ToMethod( ( Type type ) => NewValueCannotMarshal( type ) );

		public static SerializationException NewValueCannotMarshal( Type type )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot marhsal '{0}' type.", type ) );
		}

		internal static readonly MethodInfo NewValueCannotUnmarshalMethod = FromExpression.ToMethod( ( Type type ) => NewValueCannotUnmarshal( type ) );

		public static Exception NewValueCannotUnmarshal( Type type )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot unmarhsal '{0}' type.", type ) );
		}

		internal static readonly MethodInfo NewMissingItemMethod = FromExpression.ToMethod( ( int index ) => NewMissingItem( index ) );

		public static Exception NewMissingItem( int index )
		{
			return new InvalidMessagePackStreamException( String.Format( CultureInfo.CurrentCulture, "Items at index '{0}' is missing.", index ) );
		}

		internal static Exception NewTypeIsNotSerializable( Type type )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Type '{0}' is not serializable.", type ) );
		}

		internal static Exception NewTargetDoesNotHavePublicDefaultConstructor( Type type )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Type '{0}' does not have default (parameterless) public constructor.", type ) );
		}

		internal static Exception NewTypeIsNotSerializableBecauesNotDataContract( Type type )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot serialize type '{0}' because it is not qualified with DataContractAttribute.", type ) );
		}

		internal static Exception NewNoSerializableFieldsException( Type type )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot serialize type '{0}' because it does not have any serializable fields nor properties.", type ) );
		}

		internal static Exception NewMissingField( string name )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Field '{0}' is missing.", name ) );
		}

		internal static Exception NewMissingProperty( string name )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Property '{0}' is missing.", name ) );
		}

		internal static readonly MethodInfo NewUnexpectedEndOfStreamMethod = FromExpression.ToMethod( () => NewUnexpectedEndOfStream() );

		public static Exception NewUnexpectedEndOfStream()
		{
			return new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
		}

		internal static Exception NewMissingAddMethod( Type type )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Type '{0}' does not have appropriate Add method.", type ) );
		}

		internal static readonly MethodInfo NewCannotReadCollectionHeaderMethod = FromExpression.ToMethod( () => NewCannotReadCollectionHeader() );

		public static Exception NewCannotReadCollectionHeader()
		{
			return new InvalidMessagePackStreamException( "Stream ends unexpectedly." );
		}

		internal static readonly MethodInfo NewValiueTypeCannotBeNullMethod = FromExpression.ToMethod( ( Type type ) => NewValiueTypeCannotBeNull( type ) );

		public static Exception NewValiueTypeCannotBeNull( Type type )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot be null '{0}' type value.", type ) );
		}
	}
}
