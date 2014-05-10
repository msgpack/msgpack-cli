#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

using MsgPack.Serialization.DefaultSerializers;

namespace MsgPack.Serialization.AbstractSerializers
{
	/// <summary>
	///		Defines common features for serializer builder.
	/// </summary>
	/// <typeparam name="TContext">The type of the context which holds global information for generating serializer.</typeparam>
	/// <typeparam name="TConstruct">The type of the construct which abstracts code constructs.</typeparam>
	/// <typeparam name="TObject">The type of the object which will be target of the generating serializer.</typeparam>
	[ContractClass( typeof( SerializerBuilderContract<,,> ) )]
	internal abstract partial class SerializerBuilder<TContext, TConstruct, TObject> :
#if !NETFX_CORE && !SILVERLIGHT
		ISerializerCodeGenerator,
#endif
		ISerializerBuilder<TObject>
		where TContext : SerializerGenerationContext<TConstruct>
		where TConstruct : class, ICodeConstruct
	{
		/// <summary>
		///		<see cref="PropertyInfo"/> of <see cref="MessagePackSerializer{TObject}.OwnerContext"/>.
		/// </summary>
		protected static readonly PropertyInfo OwnerContextProperty =
			FromExpression.ToProperty(
				( MessagePackSerializer<TObject> serializer ) => serializer.OwnerContext
			);

		/// <summary>
		///		Initializes a new instance of the <see cref="SerializerBuilder{TContext, TConstruct, TObject}"/> class.
		/// </summary>
		protected SerializerBuilder() { }

		/// <summary>
		///		Builds the serializer and returns its new instance.
		/// </summary>
		/// <param name="context">The context information.</param>
		/// <returns>
		///		Newly created serializer object.
		///		This value will not be <c>null</c>.
		/// </returns>
		public MessagePackSerializer<TObject> BuildSerializerInstance( SerializationContext context )
		{
			var genericSerializer = GenericSerializer.Create<TObject>( context );
			if ( genericSerializer != null )
			{
				return genericSerializer;
			}

			var codeGenerationContext = this.CreateCodeGenerationContextForSerializerCreation( context );
			if ( typeof( TObject ).GetIsEnum() )
			{
				this.BuildEnumSerializer( codeGenerationContext );
				Func<SerializationContext, MessagePackSerializer<TObject>> constructor =
					this.CreateEnumSerializerConstructor( codeGenerationContext );

				if ( constructor != null )
				{
					var serializer = constructor( context );
#if DEBUG
					Contract.Assert( serializer != null );
#endif
					if ( !context.Serializers.Register( serializer ) )
					{
						serializer = context.Serializers.Get<TObject>( context );
#if DEBUG
						Contract.Assert( serializer != null );
#endif
					}

					return serializer;
				}
			}
			else
			{
				this.BuildSerializer( codeGenerationContext );
				Func<SerializationContext, MessagePackSerializer<TObject>> constructor =
					this.CreateSerializerConstructor( codeGenerationContext );

				if ( constructor != null )
				{
					var serializer = constructor( context );
#if DEBUG
					Contract.Assert( serializer != null );
#endif
					if ( !context.Serializers.Register( serializer ) )
					{
						serializer = context.Serializers.Get<TObject>( context );
#if DEBUG
						Contract.Assert( serializer != null );
#endif
					}

					return serializer;
				}
			}

			throw SerializationExceptions.NewTypeCannotSerialize( typeof( TObject ) );
		}

		/// <summary>
		///		Creates the code generation context for serializer instance creation.
		/// </summary>
		/// <param name="context">The serialization context.</param>
		/// <returns>
		///		The code generation context for serializer instance creation.
		///		This value will not be <c>null</c>.
		/// </returns>
		protected abstract TContext CreateCodeGenerationContextForSerializerCreation( SerializationContext context );

		/// <summary>
		///		Builds the serializer and returns its new instance.
		/// </summary>
		/// <param name="context">The context information. This value will not be <c>null</c>.</param>
		/// <returns>
		///		Newly created serializer object.
		///		This value will not be <c>null</c>.
		/// </returns>
		protected void BuildSerializer( TContext context )
		{
#if DEBUG
			Contract.Assert( !typeof( TObject ).IsArray );
#endif

			var traits = typeof( TObject ).GetCollectionTraits();
			switch ( traits.CollectionType )
			{
				case CollectionKind.Array:
				{
					this.BuildArraySerializer( context, traits );
					break;
				}
				case CollectionKind.Map:
				{
					this.BuildMapSerializer( context, traits );
					break;
				}
				case CollectionKind.NotCollection:
				{
					if ( typeof( TObject ).GetIsEnum() )
					{
						this.BuildEnumSerializer( context );
					}
#if !WINDOWS_PHONE && !NETFX_35
					else if ( ( typeof( TObject ).GetAssembly().Equals( typeof( object ).GetAssembly() ) ||
						  typeof( TObject ).GetAssembly().Equals( typeof( Enumerable ).GetAssembly() ) )
						&& typeof( TObject ).GetIsPublic() && typeof( TObject ).Name.StartsWith( "Tuple`", StringComparison.Ordinal ) )
					{
						this.BuildTupleSerializer( context );
					}
#endif
					else
					{
						this.BuildObjectSerializer( context );
					}
					break;
				}
			}
		}

		/// <summary>
		///		Creates the serializer type and returns its constructor.
		/// </summary>
		/// <param name="codeGenerationContext">The code generation context.</param>
		/// <returns>
		///		<see cref="Func{T, TResult}"/> which refers newly created constructor.
		///		This value will not be <c>null</c>.
		/// </returns>
		protected abstract Func<SerializationContext, MessagePackSerializer<TObject>> CreateSerializerConstructor(
			TContext codeGenerationContext
		);

		/// <summary>
		///		Creates the enum serializer type and returns its constructor.
		/// </summary>
		/// <param name="codeGenerationContext">The code generation context.</param>
		/// <returns>
		///		<see cref="Func{T, TResult}"/> which refers newly created constructor.
		///		This value will not be <c>null</c>.
		/// </returns>
		protected abstract Func<SerializationContext, MessagePackSerializer<TObject>> CreateEnumSerializerConstructor(
			TContext codeGenerationContext
		);


#if !NETFX_CORE && !SILVERLIGHT
		/// <summary>
		///		Builds the serializer code using specified code generation context.
		/// </summary>
		/// <param name="context">
		///		The <see cref="ISerializerCodeGenerationContext"/> which holds configuration and stores generated code constructs.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		This class does not support code generation.
		/// </exception>
		/// <remarks>
		///		This method will not do anything when <see cref="ISerializerCodeGenerationContext.BuiltInSerializerExists"/> returns <c>true</c> for <typeparamref name="TObject"/>.
		/// </remarks>
		public void BuildSerializerCode( ISerializerCodeGenerationContext context )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			if ( context.BuiltInSerializerExists( typeof( TObject ) ) )
			{
				// nothing to do.
				return;
			}

			this.BuildSerializerCodeCore( context );
		}

		/// <summary>
		///		In derived class, builds the serializer code using specified code generation context.
		/// </summary>
		/// <param name="context">
		///		The <see cref="ISerializerCodeGenerationContext"/> which holds configuration and stores generated code constructs.
		///		This value will not be <c>null</c>.
		/// </param>
		/// <exception cref="NotSupportedException">
		///		This class does not support code generation.
		/// </exception>
		protected virtual void BuildSerializerCodeCore( ISerializerCodeGenerationContext context )
		{
			throw new NotSupportedException();
		}
#endif
	}
}
