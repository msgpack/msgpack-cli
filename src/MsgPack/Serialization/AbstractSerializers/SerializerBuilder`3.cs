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
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using MsgPack.Serialization.DefaultSerializers;

namespace MsgPack.Serialization.AbstractSerializers
{
	internal interface ISerializerCodeGenerator
	{
		void BuildSerializerCode( ISerializerCodeGenerationContext context );
		ISerializerCodeGenerationContext CreateGenerationContextForCodeGeneration( SerializationContext context );
	}

	internal interface ISerializerInstanceGenerator<TObject>
	{
		MessagePackSerializer<TObject> BuildSerializerInstance( SerializationContext context );
	}

	internal interface ICodeConstruct
	{
		Type ContextType { get; }
	}

	[ContractClass( typeof( SerializerBuilderContract<,,> ) )]
	internal abstract partial class SerializerBuilder<TContext, TConstruct, TObject> : ISerializerCodeGenerator, ISerializerInstanceGenerator<TObject>
		where TContext : SerializerGenerationContext<TConstruct>
		where TConstruct : class, ICodeConstruct
	{
		private readonly string _assemblyName;

		protected string AssemblyName
		{
			get { return this._assemblyName; }
		}

		private readonly Version _version;

		protected Version Version
		{
			get { return this._version; }
		}

		protected SerializerBuilder( string assemblyName, Version version )
		{
			this._assemblyName = assemblyName;
			this._version = version;
		}

		public MessagePackSerializer<TObject> BuildSerializerInstance( SerializationContext context )
		{
			if ( typeof( TObject ).IsArray )
			{
				return
					Activator.CreateInstance(
						typeof( ArraySerializer<> ).MakeGenericType( typeof( TObject ).GetElementType() ), context
					) as MessagePackSerializer<TObject>;
			}

			var codeGenerationContext = this.CreateGenerationContextForSerializerCreation( context );
			this.BuildSerializer( codeGenerationContext );
			Func<SerializationContext, MessagePackSerializer<TObject>> constructor = this.CreateSerializerConstructor( codeGenerationContext );

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

			throw SerializationExceptions.NewTypeCannotSerialize( typeof( TObject ) );
		}

		protected abstract Func<SerializationContext, MessagePackSerializer<TObject>> CreateSerializerConstructor(
			TContext codeGenerationContext
		);

		protected abstract TContext CreateGenerationContextForSerializerCreation( SerializationContext context );


		public ISerializerCodeGenerationContext CreateGenerationContextForCodeGeneration( SerializationContext context )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			return this.CreateGenerationContextForCodeGenerationCore( context );
		}

		protected virtual ISerializerCodeGenerationContext CreateGenerationContextForCodeGenerationCore( SerializationContext context )
		{
			throw new NotSupportedException();
		}

		public void BuildSerializerCode( ISerializerCodeGenerationContext context )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			this.BuildSerializerCodeCore( context );
		}

		protected virtual void BuildSerializerCodeCore( ISerializerCodeGenerationContext context )
		{
			throw new NotSupportedException();
		}

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
#if !WINDOWS_PHONE && !NETFX_35
					if ( ( typeof( TObject ).GetAssembly().Equals( typeof( object ).GetAssembly() ) ||
						  typeof( TObject ).GetAssembly().Equals( typeof( Enumerable ).GetAssembly() ) )
						&& typeof( TObject ).GetIsPublic() && typeof( TObject ).Name.StartsWith( "Tuple`", StringComparison.Ordinal ) )
					{
						this.BuildTupleSerializer( context );
					}
					else
					{
#endif
						this.BuildObjectSerializer( context );
#if !WINDOWS_PHONE && !NETFX_35
					}
#endif
					break;
				}
			}
		}
	}
}
