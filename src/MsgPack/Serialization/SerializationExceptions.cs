#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if !UNITY || MSGPACK_UNITY_FULL
using System.ComponentModel;
#endif // !UNITY || MSGPACK_UNITY_FULL
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

using MsgPack.Serialization.CollectionSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		Defines common exception factory methods.
	/// </summary>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	public static class SerializationExceptions
	{
#if !XAMIOS && !XAMDROID && !UNITY
		internal static readonly MethodInfo NewValueTypeCannotBeNull3Method = FromExpression.ToMethod( ( string name, Type memberType, Type declaringType ) => NewValueTypeCannotBeNull( name, memberType, declaringType ) );
#endif // !XAMIOS && !XAMDROID && !UNITY

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that value type cannot be <c>null</c> on deserialization.
		/// </summary>
		/// <param name="name">The name of the member.</param>
		/// <param name="memberType">The type of the member.</param>
		/// <param name="declaringType">The type that declares the member.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewValueTypeCannotBeNull( string name, Type memberType, Type declaringType )
		{
#if !UNITY
			Contract.Requires( !String.IsNullOrEmpty( name ) );
			Contract.Requires( memberType != null );
			Contract.Requires( declaringType != null );
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Member '{0}' of type '{1}' cannot be null because it is value type('{2}').", name, declaringType, memberType ) );
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that value type cannot be <c>null</c> on deserialization.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewValueTypeCannotBeNull( Type type )
		{
#if !UNITY
			Contract.Requires( type != null );
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot be null '{0}' type value.", type ) );
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that value type cannot serialize.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewTypeCannotSerialize( Type type )
		{
#if !UNITY
			Contract.Requires( type != null );
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot serialize '{0}' type.", type ) );
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that value type cannot deserialize.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewTypeCannotDeserialize( Type type )
		{
#if !UNITY
			Contract.Requires( type != null );
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot deserialize '{0}' type.", type ) );
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that value type cannot deserialize.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <param name="memberName">The name of deserializing member.</param>
		/// <param name="inner">The inner exception.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewTypeCannotDeserialize( Type type, string memberName, Exception inner )
		{
#if !UNITY
			Contract.Requires( type != null );
			Contract.Requires( !String.IsNullOrEmpty( memberName ) );
			Contract.Requires( inner != null );
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot deserialize member '{1}' of type '{0}'.", type, memberName ), inner );
		}

#if !XAMIOS && !XAMDROID && !UNITY
		/// <summary>
		///		<see cref="MethodInfo"/> of <see cref="NewMissingItem"/> method.
		/// </summary>
		internal static readonly MethodInfo NewMissingItemMethod = FromExpression.ToMethod( ( int index ) => NewMissingItem( index ) );
#endif // !XAMIOS && !XAMDROID && !UNITY

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that item is not found on the unpacking stream.
		/// </summary>
		/// <param name="index">The index to be unpacking.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewMissingItem( int index )
		{
#if !UNITY
			Contract.Requires( index >= 0 );
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new InvalidMessagePackStreamException( String.Format( CultureInfo.CurrentCulture, "Items at index '{0}' is missing.", index ) );
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that target type is not serializable because it does not have public default constructor.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		internal static Exception NewTargetDoesNotHavePublicDefaultConstructor( Type type )
		{
#if !UNITY
			Contract.Requires( type != null );
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Type '{0}' does not have default (parameterless) public constructor.", type ) );
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that target type is not serializable because it does not have both of public default constructor and public constructor with an Int32 parameter.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		internal static Exception NewTargetDoesNotHavePublicDefaultConstructorNorInitialCapacity( Type type )
		{
#if !UNITY
			Contract.Requires( type != null );
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Type '{0}' does not have both of default (parameterless) public constructor and  public constructor with an Int32 parameter.", type ) );
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that required field is not found on the unpacking stream.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewMissingProperty( string name )
		{
#if !UNITY
			Contract.Requires( !String.IsNullOrEmpty( name ) );
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Property '{0}' is missing.", name ) );
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that unpacking stream ends on unexpectedly position.
		/// </summary>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewUnexpectedEndOfStream()
		{
#if !UNITY
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( "Stream unexpectedly ends." );
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that target collection type does not declare appropriate Add(T) method.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewMissingAddMethod( Type type )
		{
#if !UNITY
			Contract.Requires( type != null );
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Type '{0}' does not have appropriate Add method.", type ) );
		}

#if !XAMIOS && !XAMDROID && !UNITY
		internal static readonly MethodInfo NewIsNotArrayHeaderMethod = FromExpression.ToMethod( () => NewIsNotArrayHeader() );
#endif // !XAMIOS && !XAMDROID && !UNITY

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that unpacker is not in the array header, that is the state is invalid.
		/// </summary>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewIsNotArrayHeader()
		{
			return new SerializationException( "Unpacker is not in the array header. The stream may not be array." );
		}

#if !XAMIOS && !XAMDROID && !UNITY
		internal static readonly MethodInfo NewIsNotMapHeaderMethod = FromExpression.ToMethod( () => NewIsNotMapHeader() );
#endif // !XAMIOS && !XAMDROID && !UNITY

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that unpacker is not in the array header, that is the state is invalid.
		/// </summary>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewIsNotMapHeader()
		{
#if !UNITY
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( "Unpacker is not in the map header. The stream may not be map." );
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that operation is not supported because <paramref name="type"/> cannot be instanciated.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewNotSupportedBecauseCannotInstanciateAbstractType( Type type )
		{
#if !UNITY
			Contract.Requires( type != null );
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new NotSupportedException( String.Format( CultureInfo.CurrentCulture, "This operation is not supported because '{0}' cannot be instanciated.", type ) );
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that the array length does not match to expected tuple cardinality.
		/// </summary>
		/// <param name="expectedTupleCardinality">The expected cardinality of the tuple.</param>
		/// <param name="actualArrayLength">The actual serialized array length.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewTupleCardinarityIsNotMatch( int expectedTupleCardinality, int actualArrayLength )
		{
#if !UNITY
			Contract.Requires( expectedTupleCardinality > 0 );
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "The length of array ({0}) does not match to tuple cardinality ({1}).", actualArrayLength, expectedTupleCardinality ) );
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that the underlying stream is not correct semantically because failed to unpack items count of array/map.
		/// </summary>
		/// <param name="innerException">The inner exception for the debug. The value is implementation specific.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewIsIncorrectStream( Exception innerException )
		{
#if !UNITY
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( "Failed to unpack items count of the collection.", innerException );
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that the unpacking collection is too large to represents in the current runtime environment.
		/// </summary>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewIsTooLargeCollection()
		{
#if !UNITY
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new MessageNotSupportedException( "The collection which has more than Int32.MaxValue items is not supported." );
		}

#if !XAMIOS && !XAMDROID && !UNITY
		internal static readonly MethodInfo NewNullIsProhibitedMethod = FromExpression.ToMethod( ( string memberName ) => NewNullIsProhibited( memberName ) );
#endif // !XAMIOS && !XAMDROID && !UNITY

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that the member cannot be <c>null</c> or the unpacking value cannot be nil because nil value is explicitly prohibitted.
		/// </summary>
		/// <param name="memberName">The name of the member.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewNullIsProhibited( string memberName )
		{
#if !UNITY
			Contract.Requires( !String.IsNullOrEmpty( memberName ) );
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "The member '{0}' cannot be nil.", memberName ) );
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that the unpacking value cannot be nil because the target member is read only and its type is collection.
		/// </summary>
		/// <param name="memberName">The name of the member.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewReadOnlyMemberItemsMustNotBeNull( string memberName )
		{
#if !UNITY
			Contract.Requires( !String.IsNullOrEmpty( memberName ) );
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "The member '{0}' cannot be nil because it is read only member.", memberName ) );
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that the unpacking collection value is not a collection.
		/// </summary>
		/// <param name="memberName">The name of the member.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewStreamDoesNotContainCollectionForMember( string memberName )
		{
#if !UNITY
			Contract.Requires( !String.IsNullOrEmpty( memberName ) );
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot deserialize member '{0}' because the underlying stream does not contain collection.", memberName ) );
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that the unpacking array size is not expected length.
		/// </summary>
		/// <param name="expectedLength">Expected, required for deserialization array length.</param>
		/// <param name="actualLength">Actual array length.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewUnexpectedArrayLength( int expectedLength, int actualLength )
		{
#if !UNITY
			Contract.Requires( expectedLength >= 0 );
			Contract.Requires( actualLength >= 0 );
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "The MessagePack stream is invalid. Expected array length is {0}, but actual is {1}.", expectedLength, actualLength ) );
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that it is failed to deserialize member.
		/// </summary>
		/// <param name="targetType">Deserializing type.</param>
		/// <param name="memberName">The name of the deserializing member.</param>
		/// <param name="inner">The exception which caused current error.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewFailedToDeserializeMember( Type targetType, string memberName, Exception inner )
		{
#if !UNITY
			Contract.Requires( targetType != null );
			Contract.Requires( !String.IsNullOrEmpty( memberName ) );
			Contract.Requires( inner != null );
			Contract.Ensures( Contract.Result<Exception>() != null );
#endif // !UNITY

			return new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot deserialize member '{0}' of type '{1}'.", memberName, targetType ), inner );
		}

		internal static Exception NewUnpackToIsNotSupported( Type type, Exception inner )
		{
#if !UNITY
			Contract.Requires( type != null );
#endif // !UNITY
			return new NotSupportedException( String.Format( CultureInfo.CurrentCulture, "This operation is not supported for '{0}' because it does not have accesible Add(T) method.", type ), inner );
		}

		internal static Exception NewValueTypeCannotBePolymorphic( Type type )
		{
			return
				new SerializationException(
					String.Format( CultureInfo.CurrentCulture, "Value type '{0}' cannot be polymorphic.", type )
				);
		}

		internal static Exception NewUnknownTypeEmbedding()
		{
			return new SerializationException( "Cannot deserialize with type-embedding based serializer. Root object must be 3 element array." );
		}

		internal static Exception NewIncompatibleCollectionSerializer( Type targetType, Type incompatibleType, Type exampleClass )
		{
			return 
				new SerializationException(
					String.Format( 
						CultureInfo.CurrentCulture,
						"Cannot serialize type '{0}' because registered or generated serializer '{1}' does not implement '{2}', which is implemented by '{3}', for example.",
						targetType.GetFullName(),
						incompatibleType.GetFullName(),
						typeof( ICollectionInstanceFactory ),
						exampleClass.GetFullName()
					)
				);
		}
	}
}
