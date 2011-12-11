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
		/// <summary>
		///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that value type cannot be <c>null</c> on deserialization.
		/// </summary>
		/// <param name="name">The name of the member.</param>
		/// <param name="memberType">The type of the member.</param>
		/// <param name="declaringType">The type that declares the member.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewValueTypeCannotBeNull( string name, Type memberType, Type declaringType )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Member '{0}' of type '{1}' cannot be null because it is value type('{2}').", name, declaringType, memberType ) );
		}

		/// <summary>
		///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that value type cannot be <c>null</c> on deserialization.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewValueTypeCannotBeNull( Type type )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot be null '{0}' type value.", type ) );
		}

		/// <summary>
		///		<see cref="MethodInfo"/> of <see cref="NewTypeCannotSerialize"/> method.
		/// </summary>
		internal static readonly MethodInfo NewTypeCannotSerializeMethod = FromExpression.ToMethod( ( Type type ) => NewTypeCannotSerialize( type ) );

		/// <summary>
		///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that value type cannot serialize.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewTypeCannotSerialize( Type type )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot serialize '{0}' type.", type ) );
		}

		/// <summary>
		///		<see cref="MethodInfo"/> of <see cref="NewTypeCannotDeserialize"/> method.
		/// </summary>
		internal static readonly MethodInfo NewTypeCannotDeserializeMethod = FromExpression.ToMethod( ( Type type ) => NewTypeCannotDeserialize( type ) );

		/// <summary>
		///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that value type cannot deserialize.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewTypeCannotDeserialize( Type type )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot deserialize '{0}' type.", type ) );
		}

		/// <summary>
		///		<see cref="MethodInfo"/> of <see cref="NewMissingItem"/> method.
		/// </summary>
		internal static readonly MethodInfo NewMissingItemMethod = FromExpression.ToMethod( ( int index ) => NewMissingItem( index ) );

		/// <summary>
		///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that item is not found on the unpacking stream.
		/// </summary>
		/// <param name="index">The index to be unpacking.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewMissingItem( int index )
		{
			return new InvalidMessagePackStreamException( String.Format( CultureInfo.CurrentCulture, "Items at index '{0}' is missing.", index ) );
		}

		/// <summary>
		///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that target type is not serializable because it does not have public default constructor.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		internal static Exception NewTargetDoesNotHavePublicDefaultConstructor( Type type )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Type '{0}' does not have default (parameterless) public constructor.", type ) );
		}

		/// <summary>
		///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that target type is not serializable because it does not have both of public default constructor and public constructor with an Int32 parameter.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		internal static Exception NewTargetDoesNotHavePublicDefaultConstructorNorInitialCapacity( Type type )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Type '{0}' does not have both of default (parameterless) public constructor and  public constructor with an Int32 parameter.", type ) );
		}

		/// <summary>
		///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that there are no serializable fields and properties on the target type.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		internal static Exception NewNoSerializableFieldsException( Type type )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot serialize type '{0}' because it does not have any serializable fields nor properties.", type ) );
		}

		/// <summary>
		///		<see cref="MethodInfo"/> of <see cref="NewMissingProperty"/> method.
		/// </summary>
		internal static readonly MethodInfo NewMissingPropertyMethod = FromExpression.ToMethod( ( string name ) => NewMissingProperty( name ) );

		/// <summary>
		///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that required field is not found on the unpacking stream.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewMissingProperty( string name )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Property '{0}' is missing.", name ) );
		}

		/// <summary>
		///		<see cref="MethodInfo"/> of <see cref="NewUnexpectedEndOfStream"/> method.
		/// </summary>
		internal static readonly MethodInfo NewUnexpectedEndOfStreamMethod = FromExpression.ToMethod( () => NewUnexpectedEndOfStream() );

		/// <summary>
		///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that unpacking stream ends on unexpectedly position.
		/// </summary>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewUnexpectedEndOfStream()
		{
			return new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
		}

		/// <summary>
		///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that target collection type does not declare appropriate Add(T) method.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		internal static Exception NewMissingAddMethod( Type type )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Type '{0}' does not have appropriate Add method.", type ) );
		}

		internal static readonly MethodInfo NewIsNotArrayHeaderMethod = FromExpression.ToMethod( () => SerializationExceptions.NewIsNotArrayHeader() );

		/// <summary>
		///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that unpacker is not in the array header, that is the state is invalid.
		/// </summary>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewIsNotArrayHeader()
		{
			return new InvalidOperationException( "Unpacker is not in the array header" );
		}

		internal static readonly MethodInfo NewIsNotMapHeaderMethod = FromExpression.ToMethod( () => SerializationExceptions.NewIsNotMapHeader() );

		/// <summary>
		///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that unpacker is not in the array header, that is the state is invalid.
		/// </summary>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewIsNotMapHeader()
		{
			return new InvalidOperationException( "Unpacker is not in the map header" );
		}

		internal static readonly MethodInfo NewNotSupportedBecauseCannotInstanciateAbstractTypeMethod = FromExpression.ToMethod( ( Type type ) => SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractType( type ) );

		/// <summary>
		///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that operation is not supported because <paramref name="type"/> cannot be instanciated.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewNotSupportedBecauseCannotInstanciateAbstractType( Type type )
		{
			return new NotSupportedException( String.Format( CultureInfo.CurrentCulture, "This operation is not supported because '{0}' cannot be instanciated.", type ) );
		}

		internal static readonly MethodInfo NewTupleCardinarityIsNotMatchMethod = FromExpression.ToMethod( ( int expected, int actual ) => SerializationExceptions.NewTupleCardinarityIsNotMatch( expected, actual ) );

		/// <summary>
		///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that the array length does not match to expected tuple cardinality.
		/// </summary>
		/// <param name="expectedTupleCardinality">The expected cardinality of the tuple.</param>
		/// <param name="actualArrayLength">The actual serialized array length.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewTupleCardinarityIsNotMatch( int expectedTupleCardinality, int actualArrayLength )
		{
			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "The length of array ({0}) does not match to tuple cardinality ({1}).", actualArrayLength, expectedTupleCardinality ) );
		}
	}
}
