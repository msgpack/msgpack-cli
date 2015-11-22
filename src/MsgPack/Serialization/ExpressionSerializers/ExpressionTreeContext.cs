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

using System;
using System.Collections.Generic;
#if CORE_CLR
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR
using System.Linq;
using System.Linq.Expressions;
#if !NETFX_CORE && !SILVERLIGHT
using System.Reflection;
using System.Reflection.Emit;
#endif // !NETFX_CORE && !SILVERLIGHT

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		Implements <see cref="SerializerGenerationContext{TConstruct}"/> for <see cref="ExpressionTreeSerializerBuilder{TObject}"/>.
	/// </summary>
	internal sealed class ExpressionTreeContext : SerializerGenerationContext<ExpressionConstruct>
	{
		private readonly ParameterExpression[] _commonPrivateInstanceMethodParameters;

		/// <summary>
		///		Gets a common <see cref="ParameterExpression"/> collection for instance method of current serializer.
		/// </summary>
		/// <param name="name">The name of method.</param>
		/// <returns>A common <see cref="ParameterExpression"/> collection for instance method of current serializer. This value will not be <c>null</c>.</returns>
		public IEnumerable<ParameterExpression> GetCommonThisMethodArguments( string name )
		{
			var method = this.GetDeclaredMethod( name );
			foreach ( var parameterType in method.ParameterTypes )
			{
				if ( parameterType.TryGetRuntimeType() == this._serializerClass )
				{
					yield return ( ParameterExpression )this.This.Expression;
				}

				if ( parameterType.TryGetRuntimeType() == this._context.ContextType.TryGetRuntimeType() )
				{
					yield return ( ParameterExpression ) this.Context.Expression;
				}
			}
		}

#if !NETFX_CORE && !SILVERLIGHT && !CORE_CLR
		private readonly TypeBuilder _typeBuilder;
#endif // !NETFX_CORE && !SILVERLIGHT && !CORE_CLR
		private readonly ExpressionConstruct _context;

		/// <summary>
		///		Gets the code construct which represents 'context' parameter of generated methods.
		/// </summary>
		/// <value>
		///		The code construct which represents 'context' parameter of generated methods.
		///		Its type is <see cref="SerializationContext"/>, and it holds dependent serializers.
		///		This value will not be <c>null</c>.
		/// </value>
		public override ExpressionConstruct Context
		{
			get { return this._context; }
		}

		/// <summary>
		///		Gets the code construct which represents 'this' reference.
		/// </summary>
		/// <value>
		///		The code construct which represents 'this' reference.
		/// </value>
		public ExpressionConstruct This { get; private set; }

		private readonly Dictionary<string, Delegate> _delegates;
		private readonly Dictionary<string, LambdaExpression> _methodLambdas;

		// For lamdas which must be expose to base class or helper API, e.g. CreateObjectFromContext, PackToCore, etc.
		public Delegate GetDelegate( string name )
		{
			Delegate result;
			this._delegates.TryGetValue( name, out result );
			return result;
		}

		// For lamdas which have not to be expose to base class or helper API, e.g. PackValueOf..., etc.
		public LambdaExpression GetMethodLambda( string name )
		{
			LambdaExpression result;
			this._methodLambdas.TryGetValue( name, out result );
			return result;
		}

		public IDictionary<string, Delegate> GetDelegates()
		{
			return this._delegates;
		}

		private Type _targetType;
		private Type _serializerClass;
		private CollectionTraits _targetCollectionTraits;

		private readonly Stack<MethodContext> _methodContextStack;

		private bool IsEnum
		{
			get { return this._targetType.GetIsEnum(); }
		}

		private bool IsDictionary
		{
			get { return this.KeyToAdd != null; }
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="ExpressionTreeContext"/> class.
		/// </summary>
		/// <param name="context">The serialization context.</param>
		/// <param name="targetType">Type of the serialization target.</param>
		/// <param name="baseSerializerType">Type of the base type of the generating serializer.</param>
		public ExpressionTreeContext( SerializationContext context, Type targetType, Type baseSerializerType )
			: base( context )
		{
			var contextParameter = Expression.Parameter( typeof( SerializationContext ), "context" );
			this._context = contextParameter;
			this._commonPrivateInstanceMethodParameters = new ParameterExpression[ 1 ];
			this._commonPrivateInstanceMethodParameters[ 0 ] = contextParameter;
			this._context = contextParameter;
			this._delegates = new Dictionary<string, Delegate>();
			this._methodLambdas = new Dictionary<string, LambdaExpression>();
			this._methodContextStack = new Stack<MethodContext>();
			this.Reset( targetType, baseSerializerType );
#if !NETFX_CORE && !SILVERLIGHT && !CORE_CLR
			if ( SerializerDebugging.DumpEnabled )
			{
				SerializerDebugging.PrepareDump();
				this._typeBuilder = SerializerDebugging.NewTypeBuilder( targetType );
			}
#endif // !NETFX_CORE && !SILVERLIGHT && !CORE_CLR
		}

		protected override void ResetCore( Type targetType, Type baseClass )
		{
			this._delegates.Clear();
			this._methodLambdas.Clear();
			this._methodContextStack.Clear();
			this.Packer = Expression.Parameter( typeof( Packer ), "packer" );
			this.PackToTarget = Expression.Parameter( targetType, "objectTree" );
			this.Unpacker = Expression.Parameter( typeof( Unpacker ), "unpacker" );
			this.IndexOfItem = Expression.Parameter( typeof( int ), "indexOfItem" );
			this.UnpackToTarget = Expression.Parameter( targetType, "collection" );
			this._targetType = targetType;
			this._targetCollectionTraits = targetType.GetCollectionTraits();
			this._serializerClass =
				ExpressionTreeSerializerBuilderHelpers.GetSerializerClass( targetType, this._targetCollectionTraits );
			var @this = Expression.Parameter( this._serializerClass, "this" );
			this.This = @this;
			if ( this._targetCollectionTraits.ElementType != null )
			{
				this.CollectionToBeAdded = Expression.Parameter( targetType, "collection" );
				this.ItemToAdd = Expression.Parameter( this._targetCollectionTraits.ElementType, "item" );
				if ( this._targetCollectionTraits.DetailedCollectionType == CollectionDetailedKind.GenericDictionary
#if !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
					|| this._targetCollectionTraits.DetailedCollectionType == CollectionDetailedKind.GenericReadOnlyDictionary
#endif // !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
 )
				{
					this.KeyToAdd = Expression.Parameter( this._targetCollectionTraits.ElementType.GetGenericArguments()[ 0 ], "key" );
					this.ValueToAdd = Expression.Parameter( this._targetCollectionTraits.ElementType.GetGenericArguments()[ 1 ], "value" );
				}
				else
				{
					this.KeyToAdd = null;
					this.ValueToAdd = null;
				}
				this.InitialCapacity = Expression.Parameter( typeof( int ), "initialCapacity" );
			}

			// ExpressionTreeSerializerBuilder does not support Async API now.
		}

		public IList<ParameterExpression> GetCurrentParameters()
		{
			return this._methodContextStack.Peek().Parameters;
		}

		public override void BeginMethodOverride( string name )
		{
			this._methodContextStack.Push( 
				new MethodContext( 
					name, 
					null,
					this.GetParameters( name )
				)
			);
		}

		private IEnumerable<ParameterExpression> GetParameters( string method )
		{
			yield return this.This.Expression as ParameterExpression;

			if ( !this.IsEnum )
			{
				yield return this._context.Expression as ParameterExpression;
			}

			switch ( method )
			{
				case MethodName.PackToCore:
				{
					yield return this.Packer.Expression as ParameterExpression;
					yield return this.PackToTarget.Expression as ParameterExpression;
					break;
				}
				case MethodName.UnpackFromCore:
				{
					yield return this.Unpacker.Expression as ParameterExpression;
					break;
				}
				case MethodName.UnpackToCore:
				{
					yield return this.Unpacker.Expression as ParameterExpression;
					yield return this.UnpackToTarget.Expression as ParameterExpression;
					break;
				}
				case MethodName.PackUnderlyingValueTo:
				{
					yield return this.Packer.Expression as ParameterExpression;
					yield return Expression.Parameter( this._targetType, "enumValue" );
					break;
				}
				case MethodName.UnpackFromUnderlyingValue:
				{
					yield return Expression.Parameter( typeof( MessagePackObject ), "messagePackObject" );
					break;
				}
				case MethodName.AddItem:
				{
					yield return this.CollectionToBeAdded.Expression as ParameterExpression;
					if ( this.IsDictionary )
					{
						yield return this.KeyToAdd.Expression as ParameterExpression;
						yield return this.ValueToAdd.Expression as ParameterExpression;
					}
					else
					{
						yield return this.ItemToAdd.Expression as ParameterExpression;
					}
					break;
				}
				case MethodName.CreateInstance:
				{
					yield return this.InitialCapacity.Expression as ParameterExpression;
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "method", method );
				}
			}
		}

		protected override MethodDefinition EndMethodOverrideCore( string name, ExpressionConstruct body )
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "----{0}----", name );
				body.ToString( SerializerDebugging.ILTraceWriter );
				SerializerDebugging.FlushTraceData();
			}

			var context = this._methodContextStack.Pop();
#if DEBUG
			Contract.Assert( context.Name == name, context.Name + "==" + name );
			Contract.Assert( body != null, "No lamda target for " + name );
#endif // DEBUG

			var lambda =
				Expression.Lambda(
					this.CreateDelegateType( name ),
					body.Expression,
					name,
					false,
					context.Parameters
				);

#if !NETFX_CORE && !SILVERLIGHT && !CORE_CLR
			if ( SerializerDebugging.DumpEnabled )
			{
				var mb =
					this._typeBuilder.DefineMethod(
						name,
						MethodAttributes.Public | MethodAttributes.Static,
						lambda.Type,
						lambda.Parameters.Select( e => e.Type ).ToArray()
						);
				lambda.CompileToMethod( mb );
			}
#endif // !NETFX_CORE && !SILVERLIGHT && !CORE_CLR
			this._methodLambdas[ name ] = lambda;
			this._delegates[ name ] = lambda.Compile();
			return
				new MethodDefinition(
					name,
					null,
					null,
					context.ReturnType,
					context.Parameters.Select( p => TypeDefinition.Object( p.Type ) ).ToArray()
				);
		}

		private Type CreateDelegateType( string method )
		{
			switch ( method )
			{
				case MethodName.PackToCore:
				{
					return
						typeof( Action<,,,> ).MakeGenericType(
							this._serializerClass,
							typeof( SerializationContext ),
							typeof( Packer ),
							this._targetType
						);
				}
				case MethodName.UnpackFromCore:
				{
					return
						typeof( Func<,,,> ).MakeGenericType(
							this._serializerClass,
							typeof( SerializationContext ),
							typeof( Unpacker ),
							this._targetType
						);
				}
				case MethodName.UnpackToCore:
				{
					return
						typeof( Action<,,,> ).MakeGenericType(
							this._serializerClass,
							typeof( SerializationContext ),
							typeof( Unpacker ),
							this._targetType
						);
				}
				case MethodName.PackUnderlyingValueTo:
				{
					return
						typeof( Action<,,> )
						.MakeGenericType(
							typeof( ExpressionCallbackEnumMessagePackSerializer<> ).MakeGenericType( this._targetType ),
							typeof( Packer ),
							this._targetType
						);
				}
				case MethodName.UnpackFromUnderlyingValue:
				{
					return
						typeof( Func<,,> )
						.MakeGenericType(
							typeof( ExpressionCallbackEnumMessagePackSerializer<> ).MakeGenericType( this._targetType ),
							typeof( MessagePackObject ),
							this._targetType
						);
				}
				case MethodName.AddItem:
				{
					if ( this.IsDictionary )
					{
						return
							typeof( Action<,,,,> ).MakeGenericType(
								this._serializerClass,
								typeof( SerializationContext ),
								this._targetType,
								this._targetCollectionTraits.ElementType.GetGenericArguments()[ 0 ],
								this._targetCollectionTraits.ElementType.GetGenericArguments()[ 1 ]
							);
					}
					else
					{
						return
							typeof( Action<,,,> ).MakeGenericType(
								this._serializerClass,
								typeof( SerializationContext ),
								this._targetType,
								this._targetCollectionTraits.ElementType
							);
					}
				}
				case MethodName.CreateInstance:
				{
					return
						typeof( Func<,,,> ).MakeGenericType(
							this._serializerClass,
							typeof( SerializationContext ),
							typeof( int ),
							this._targetType
						);
				}
				case MethodName.RestoreSchema:
				{
					return
						typeof( Func<> ).MakeGenericType(
							typeof( PolymorphismSchema )
						);
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "method", method );
				}
			}
		}

		public override void BeginPrivateMethod( string name, bool isStatic, TypeDefinition returnType, params ExpressionConstruct[] parameters )
		{
			// Extract operand of ConvertExpression(UnaryExpression) to take care of UnpackingContext parameter.
			var parameterExpressions = 
				parameters.Select( p => 
					( p.Expression as ParameterExpression ) ?? (ParameterExpression)( (UnaryExpression )p.Expression ).Operand
				);
			this._methodContextStack.Push(
				new MethodContext(
					name,
					returnType,
					isStatic 
					? parameterExpressions
					: this._commonPrivateInstanceMethodParameters.Concat(
						parameterExpressions
					)
				)
			);
		}

		protected override MethodDefinition EndPrivateMethodCore( string name, ExpressionConstruct body )
		{
			var context = this._methodContextStack.Pop();
#if DEBUG
			Contract.Assert( context.Name == name, context.Name + "==" + name );
			Contract.Assert( body != null, "No lamda target for " + name );
#endif // DEBUG

			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "----{0}----", name );
				body.ToString( SerializerDebugging.ILTraceWriter );
				SerializerDebugging.FlushTraceData();
			}

			var parameterTypes = context.Parameters.Select( p => TypeDefinition.Object( p.Type ) ).ToArray();
			var delegateType =
				SerializerBuilderHelper.GetDelegateType(
					context.ReturnType.ResolveRuntimeType(),
					parameterTypes
				);

			var lambda =
				Expression.Lambda(
					delegateType,
					body.Expression,
					name,
					false,
					context.Parameters
				);

#if !NETFX_CORE && !SILVERLIGHT && !CORE_CLR
			if ( SerializerDebugging.DumpEnabled )
			{
				var mb =
					this._typeBuilder.DefineMethod(
						name,
						MethodAttributes.Public | MethodAttributes.Static,
						lambda.Type,
						lambda.Parameters.Select( e => e.Type ).ToArray()
					);
				lambda.CompileToMethod( mb );
			}
#endif // !NETFX_CORE && !SILVERLIGHT && !CORE_CLR
			this._methodLambdas[ name ] = lambda;
			this._delegates[ name ] = lambda.Compile();

			return
				new MethodDefinition(
					name,
					null,
					null,
					context.ReturnType,
					parameterTypes
				);
		}

		protected override FieldDefinition DeclarePrivateFieldCore( string name, TypeDefinition type )
		{
			throw new NotSupportedException();
		}

		protected override void DefineUnpackingContextCore(
			IList<KeyValuePair<string, TypeDefinition>> fields,
			out TypeDefinition type,
			out ConstructorDefinition constructor,
			out ExpressionConstruct parameterInUnpackValueMethods,
			out ExpressionConstruct parameterInCreateObjectFromContext
		)
		{
			// Use Object because base Proeprty type must be determined too early in current design,
			// so it cannot have DynamicUnpackingContext nor TObject as generic argument.
			type = typeof( object );
			constructor = DynamicUnpackingContext.Constructor;
			parameterInUnpackValueMethods =
				Expression.Convert(
					Expression.Parameter( typeof( object ), "unpackingContext" ),
					typeof( DynamicUnpackingContext )
				);
			parameterInCreateObjectFromContext =
				Expression.Convert(
					Expression.Parameter( typeof( object ), "unpackingContext" ),
					typeof( DynamicUnpackingContext )
				);
		}

		protected override void DefineUnpackingContextWithResultObjectCore(
			out TypeDefinition type,
			out ExpressionConstruct parameterInUnpackValueMethods,
			out ExpressionConstruct parameterInCreateObjectFromContext
		)
		{
			// Use Object because base Proeprty type must be determined too early in current design,
			// so it cannot have DynamicUnpackingContext nor TObject as generic argument.
			type = typeof( object );
			parameterInUnpackValueMethods =
				Expression.Convert(
					Expression.Parameter( typeof( object ), "unpackingContext" ),
					 this._targetType
				);
			parameterInCreateObjectFromContext =
				Expression.Convert(
					Expression.Parameter( typeof( object ), "unpackingContext" ),
					 this._targetType
				);
		}

		public void Finish()
		{
#if !NETFX_CORE && !SILVERLIGHT && !CORE_CLR
			if ( SerializerDebugging.DumpEnabled )
			{
				this._typeBuilder.CreateType();
			}
#endif // !NETFX_CORE && !SILVERLIGHT && !CORE_CLR
		}

		private sealed class MethodContext
		{
			public readonly string Name;
			public readonly TypeDefinition ReturnType;
			public readonly ParameterExpression[] Parameters;

			public MethodContext( string name, TypeDefinition returnType, IEnumerable<ParameterExpression> parameters )
			{
				this.Name = name;
				this.ReturnType = returnType;
				this.Parameters = parameters.ToArray();
			}
		}
	}
}