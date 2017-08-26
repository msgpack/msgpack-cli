#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke and contributors
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
// Contributors:
//    Samuel Cragg
//
#endregion -- License Terms --

using System;
using System.Collections.Generic;
#if NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // NETSTANDARD1_1
using System.Linq;
using System.Reflection;

using MsgPack.Serialization.AbstractSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		A code generation context for <see cref="AssemblyBuilderSerializerBuilder"/>.
	/// </summary>
	internal class AssemblyBuilderEmittingContext : SerializerGenerationContext<ILConstruct>
	{
		private readonly Type _targetType;

		/// <summary>
		///		Gets or sets the <see cref="TracingILGenerator"/> to emit IL for current method.
		/// </summary>
		/// <value>
		///		The <see cref="TracingILGenerator"/> to emit IL for current method.
		/// </value>
		internal TracingILGenerator IL { get { return this._ilGeneratorStack.Peek().ILGenerator; } }

		private readonly Func<SerializerEmitter> _emitterFactory;
		private readonly Stack<ILMethodConctext> _ilGeneratorStack;

		private SerializerEmitter _emitter;

		/// <summary>
		///		Gets the <see cref="SerializerEmitter"/>.
		/// </summary>
		/// <value>
		///		The <see cref="SerializerEmitter"/>.
		/// </value>
		internal SerializerEmitter Emitter
		{
			get
			{
				if ( this._emitter == null )
				{
					this._emitter = this._emitterFactory();
				}

				return this._emitter;
			}
		}

		/// <summary>
		///		Gets the type of the serializer.
		/// </summary>
		/// <param name="targetType">Type of the serialization target.</param>
		/// <returns>Type of the serializer.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "By design" )]
		public Type GetSerializerType( Type targetType )
		{
			return typeof( MessagePackSerializer<> ).MakeGenericType( targetType );
		}

		///  <summary>
		///  Initializes a new instance of the <see cref="AssemblyBuilderEmittingContext"/> class.
		///  </summary>
		/// <param name="context">The serialization context.</param>
		/// <param name="targetType">Type of the serialization target.</param>
		/// <param name="emitterFactory">
		///		The factory for <see cref="SerializerEmitter"/> to be used.
		/// </param>
		public AssemblyBuilderEmittingContext( SerializationContext context, Type targetType, Func<SerializerEmitter> emitterFactory )
			: base( context )
		{
			this._targetType = targetType;
			this._emitterFactory = emitterFactory;
			this._ilGeneratorStack = new Stack<ILMethodConctext>();
			this.Reset( targetType, null );
		}

		protected sealed override void ResetCore( Type targetType, Type baseClass )
		{
			// Note: baseClass is always null this class hiearchy.
			var targetTypeDefinition = TypeDefinition.Object( targetType );
			this.Packer = ILConstruct.Argument( 1, TypeDefinition.PackerType, "packer" );
			this.PackToTarget = ILConstruct.Argument( 2, targetTypeDefinition, "objectTree" );
			this.NullCheckTarget = ILConstruct.Argument( 1, targetTypeDefinition, "objectTree" );
			this.Unpacker = ILConstruct.Argument( 1, TypeDefinition.UnpackerType, "unpacker" );
			this.IndexOfItem = ILConstruct.Argument( 3, TypeDefinition.Int32Type, "indexOfItem" );
			this.ItemsCount = ILConstruct.Argument( 4, TypeDefinition.Int32Type, "itemsCount" );
			this.UnpackToTarget = ILConstruct.Argument( 2, targetTypeDefinition, "collection" );
			var traits = targetType.GetCollectionTraits( CollectionTraitOptions.Full, this.SerializationContext.CompatibilityOptions.AllowNonCollectionEnumerableTypes );
			if ( traits.ElementType != null )
			{
				this.CollectionToBeAdded = ILConstruct.Argument( 1, targetTypeDefinition, "collection" );
				this.ItemToAdd = ILConstruct.Argument( 2, traits.ElementType, "item" );
				if ( traits.DetailedCollectionType == CollectionDetailedKind.GenericDictionary
#if !NET35 && !UNITY && !NET40 && !SILVERLIGHT
					|| traits.DetailedCollectionType == CollectionDetailedKind.GenericReadOnlyDictionary
#endif // !NET35 && !UNITY && !NET40 && !SILVERLIGHT
 )
				{
					this.KeyToAdd = ILConstruct.Argument( 2, traits.ElementType.GetGenericArguments()[ 0 ], "key" );
					this.ValueToAdd = ILConstruct.Argument( 3, traits.ElementType.GetGenericArguments()[ 1 ], "value" );
				}
				this.InitialCapacity = ILConstruct.Argument( 1, TypeDefinition.Int32Type, "initialCapacity" );
			}

			this._emitter = null;
			this._ilGeneratorStack.Clear();
		}

		public override void BeginMethodOverride( string name )
		{
			this._ilGeneratorStack.Push( this.Emitter.DefineOverrideMethod( name ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Validated internally" )]
		public override void BeginPrivateMethod( string name, bool isStatic, TypeDefinition returnType, params ILConstruct[] parameters )
		{
			Contract.Assert( returnType != null );

			this._ilGeneratorStack.Push(
				this.Emitter.DefinePrivateMethod(
					name,
					isStatic,
					returnType.ResolveRuntimeType(),
					parameters.Select( p => p.ContextType.ResolveRuntimeType() ).ToArray()
				)
			);
		}

		protected override MethodDefinition EndPrivateMethodCore( string name, ILConstruct body )
		{
			return this.EndMethod( body );
		}

		protected override MethodDefinition EndMethodOverrideCore( string name, ILConstruct body )
		{
			return this.EndMethod( body );
		}

		private MethodDefinition EndMethod( ILConstruct body )
		{
			ILMethodConctext lastMethod;
			try
			{
				if ( body != null )
				{
					body.Evaluate( this.IL );
				}

				if ( body == null || !body.IsTerminating )
				{
					this.IL.EmitRet();
				}
			}
			finally
			{
				this.IL.FlushTrace();
#if DEBUG
				SerializerDebugging.FlushTraceData();
#endif // DEBUG
				lastMethod = this._ilGeneratorStack.Pop();
			}

			return new MethodDefinition( lastMethod.Method, null, lastMethod.ParameterTypes );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected override FieldDefinition DeclarePrivateFieldCore( string name, TypeDefinition type )
		{
			Contract.Assert( type != null );

			return this.Emitter.RegisterField( name, type.ResolveRuntimeType() );
		}

		protected override void DefineUnpackingContextCore(
			IList<KeyValuePair<string, TypeDefinition>> fields,
			out TypeDefinition type,
			out ConstructorDefinition constructor,
			out ILConstruct parameterInUnpackValueMethods,
			out ILConstruct parameterInSetValueMethods,
			out ILConstruct parameterInCreateObjectFromContext
		)
		{
			Type runtimeType;
			ConstructorInfo runtimeConstructor;
			this.Emitter.DefineUnpackingContext(
				SerializerBuilderHelper.UnpackingContextTypeName,
				fields.Select( kv => new KeyValuePair<string, Type>( kv.Key, kv.Value.ResolveRuntimeType() ) ).ToArray(),
				out runtimeType,
				out runtimeConstructor
			);
			type = runtimeType;
			constructor =
				new ConstructorDefinition(
					runtimeConstructor,
					fields.Select( kv => TypeDefinition.Object( kv.Value.ResolveRuntimeType() ) )
				);
			DefineUnpackValueMethodArguments( type, out parameterInUnpackValueMethods, out parameterInSetValueMethods, out parameterInCreateObjectFromContext );
		}

		protected override void DefineUnpackingContextWithResultObjectCore(
			out TypeDefinition type,
			out ILConstruct parameterInUnpackValueMethods,
			out ILConstruct parameterInSetValueMethods,
			out ILConstruct parameterInCreateObjectFromContext
		)
		{
			type = this._targetType;
			DefineUnpackValueMethodArguments( type, out parameterInUnpackValueMethods, out parameterInSetValueMethods, out parameterInCreateObjectFromContext );
		}

		private static void DefineUnpackValueMethodArguments( TypeDefinition type, out ILConstruct parameterInUnpackValueMethods, out ILConstruct parameterInSetValueMethods, out ILConstruct parameterInCreateObjectFromContext )
		{
			parameterInUnpackValueMethods = ILConstruct.Argument( 2, type, "unpackingContext" );
			parameterInSetValueMethods = ILConstruct.Argument( 1, type, "unpackingContext" );
			parameterInCreateObjectFromContext = ILConstruct.Argument( 1, type, "unpackingContext" );
		}

		public override ILConstruct DefineUnpackedItemParameterInSetValueMethods( TypeDefinition itemType )
		{
			return ILConstruct.Argument( 2, itemType, "unpackedValue" );
		}
	}
}