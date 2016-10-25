#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2016 FUJIWARA, Yusuke
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
using System.IO;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines convinient extension methods for interfaces.
	/// </summary>
	public static class MessagePackSerializerExtensions
	{
		/// <summary>
		///		Serializes specified object to the <see cref="Stream"/> with default <see cref="PackerCompatibilityOptions"/>.
		/// </summary>
		/// <param name="source"><see cref="MessagePackSerializer"/> object.</param>
		/// <param name="stream">Destination <see cref="Stream"/>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="stream"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize <paramref name="objectTree"/>.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = "'objectTree' does not mean System.Object." )]
		public static void Pack( this MessagePackSerializer source, Stream stream, object objectTree )
		{
			Pack(source, stream, objectTree, SerializationContext.Default.CompatibilityOptions.PackerCompatibilityOptions  );
		}

		/// <summary>
		///		Serializes specified object to the <see cref="Stream"/>.
		/// </summary>
		/// <param name="source"><see cref="MessagePackSerializer"/> object.</param>
		/// <param name="stream">Destination <see cref="Stream"/>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <param name="packerCompatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="stream"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize <paramref name="objectTree"/>.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = "'objectTree' does not mean System.Object." )]
		public static void Pack( this MessagePackSerializer source, Stream stream, object objectTree, PackerCompatibilityOptions packerCompatibilityOptions )
		{
			if ( source == null )
			{
				throw new ArgumentNullException("source");
			}

			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			// Packer does not have finalizer, so just avoiding packer disposing prevents stream closing.
			source.PackTo( Packer.Create( stream, packerCompatibilityOptions ), objectTree );
		}

		/// <summary>
		///		Deserializes object from the <see cref="Stream"/>.
		/// </summary>
		/// <param name="source"><see cref="MessagePackSerializer"/> object.</param>
		/// <param name="stream">Source <see cref="Stream"/>.</param>
		/// <returns>Deserialized object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="stream"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to deserialize from <paramref name="stream"/>.
		/// </exception>
		public static object Unpack( this MessagePackSerializer source, Stream stream )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			// Unpacker does not have finalizer, so just avoiding unpacker disposing prevents stream closing.
			var unpacker = Unpacker.Create( stream );
			if ( !unpacker.Read() )
			{
				SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
			}

			return source.UnpackFrom( unpacker );
		}

		/// <summary>
		///		Serializes object as a single <see cref="MessagePackObject"/>.
		/// </summary>
		/// <param name="source"><see cref="MessagePackSerializer"/> object.</param>
		/// <param name="obj">The object to be serialized.</param>
		/// <returns><see cref="MessagePackObject"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "obj", Justification = "obj is appropriate for this context." )]
		public static MessagePackObject ToMessagePackObject( this MessagePackSerializer source, object obj )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			using ( var buffer = new MemoryStream() )
			{
				source.Pack( buffer, obj );
				buffer.Position = 0;
				return Unpacking.UnpackObject( buffer );
			}
		}

		/// <summary>
		///		Serializes object as a single <see cref="MessagePackObject"/>.
		/// </summary>
		/// <typeparam name="T">The type of the object to be serialized.</typeparam>
		/// <param name="source"><see cref="MessagePackSerializer{T}"/> object.</param>
		/// <param name="obj">The object to be serialized.</param>
		/// <returns><see cref="MessagePackObject"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize.
		/// </exception>
		public static MessagePackObject ToMessagePackObject<T>( this MessagePackSerializer<T> source, T obj )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			using ( var buffer = new MemoryStream() )
			{
				source.Pack( buffer, obj );
				buffer.Position = 0;
				return Unpacking.UnpackObject( buffer );
			}
		}

		/// <summary>
		///		Deserializes object from a single <see cref="MessagePackObject"/>.
		/// </summary>
		/// <param name="source"><see cref="MessagePackSerializer"/> object.</param>
		/// <param name="mpo">The <see cref="MessagePackObject"/> which represents deserializing object structructure.</param>
		/// <returns>A deserialized object. This value can be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to deserialize.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes", Justification = "ownsStream: false" )]
		public static object FromMessagePackObject( this MessagePackSerializer source, MessagePackObject mpo )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			// This idea is borrowed from @TSnake41
			using ( var buffer = new MemoryStream() )
			{
				using ( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None, ownsStream: false ) )
				{
					mpo.PackToMessage( packer, null );
				}

				buffer.Position = 0;

				return source.Unpack( buffer );
			}
		}

		/// <summary>
		///		Deserializes object from a single <see cref="MessagePackObject"/>.
		/// </summary>
		/// <typeparam name="T">The type of the object to be deserialized.</typeparam>
		/// <param name="source"><see cref="MessagePackSerializer{T}"/> object.</param>
		/// <param name="mpo">The <see cref="MessagePackObject"/> which represents deserializing object structructure.</param>
		/// <returns>A deserialized object. This value can be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to deserialize.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes", Justification = "ownsStream: false" )]
		public static T FromMessagePackObject<T>( this MessagePackSerializer<T> source, MessagePackObject mpo )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			// This idea is borrowed from @TSnake41
			using ( var buffer = new MemoryStream() )
			{
				using ( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None, ownsStream: false ) )
				{
					mpo.PackToMessage( packer, null );
				}

				buffer.Position = 0;

				return source.Unpack( buffer );
			}
		}
	}
}

