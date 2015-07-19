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
using System.Linq;
using System.Linq.Expressions;

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		Implements <see cref="SerializerGenerationContext{TConstruct}"/> for <see cref="ExpressionTreeSerializerBuilder{TObject}"/>.
	/// </summary>
	internal sealed class ExpressionTreeContext : SerializerGenerationContext<ExpressionConstruct>
	{
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

		private ParameterExpression[] _currentParamters;

		private Delegate _packToCore;
		private Delegate _unpackFromCore;
		private Delegate _unpackToCore;
		private Delegate _packUnderyingValueTo;
		private Delegate _unpackFromUnderlyingValue;
		private Delegate _addItem;
		private Delegate _createInstance;

		/// <summary>
		///		Initializes a new instance of the <see cref="ExpressionTreeContext"/> class.
		/// </summary>
		/// <param name="context">The serialization context.</param>
		public ExpressionTreeContext( SerializationContext context )
			: base( context )
		{
			this._context = Expression.Parameter( typeof( SerializationContext ), "context" );
		}

		protected override void ResetCore( Type targetType, Type baseClass )
		{
			this.This = Expression.Parameter( baseClass, "this" );
			this.Packer = Expression.Parameter( typeof( Packer ), "packer" );
			this.PackToTarget = Expression.Parameter( targetType, "objectTree" );
			this.Unpacker = Expression.Parameter( typeof( Unpacker ), "unpacker" );
			this.UnpackToTarget = Expression.Parameter( targetType, "collection" );
			var traits = targetType.GetCollectionTraits();
			if ( traits.ElementType != null )
			{
				this.CollectionToBeAdded = Expression.Parameter( targetType, "collection" );
				this.ItemToAdd = Expression.Parameter( traits.ElementType, "item" );
				if ( traits.DetailedCollectionType == CollectionDetailedKind.GenericDictionary
#if !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
					|| traits.DetailedCollectionType == CollectionDetailedKind.GenericReadOnlyDictionary
#endif // !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				)
				{
					this.KeyToAdd = Expression.Parameter( traits.ElementType.GetGenericArguments()[ 0 ], "key" );
					this.ValueToAdd = Expression.Parameter( traits.ElementType.GetGenericArguments()[ 1 ], "value" );
				}
				this.InitialCapacity = Expression.Parameter( typeof( int ), "initialCapacity" );
			}
		}

		public IList<ParameterExpression> GetCurrentParameters()
		{
			return this._currentParamters;
		}

		public void SetCurrentMethod( SerializerMethod method )
		{
			this._currentParamters = this.GetParameters( method ).ToArray();
		}

		public void SetCurrentMethod( Type targetType, EnumSerializerMethod method )
		{
			this._currentParamters = this.GetParameters( targetType, method ).ToArray();
		}

		public void SetCurrentMethod( CollectionSerializerMethod method, CollectionTraits traits )
		{
			this._currentParamters = this.GetParameters( method, traits ).ToArray();
		}

		/// <summary>
		///		Creates the type of the delegate.
		/// </summary>
		/// <typeparam name="TObject">The type of serialization target.</typeparam>
		/// <param name="method">The method to be created.</param>
		/// <param name="serializerType">The type of callback serializer.</param>
		/// <returns>
		///		The <see cref="Type"/> of delegate which can refer to the generating method.
		///		This value will not be <c>null</c>.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		<paramref name="method"/> is unknown.
		/// </exception>
		public static Type CreateDelegateType<TObject>( SerializerMethod method, Type serializerType )
		{
			switch ( method )
			{
				case SerializerMethod.PackToCore:
				{
					return
						typeof( Action<,,,> ).MakeGenericType(
							serializerType,
							typeof( SerializationContext ),
							typeof( Packer ),
							typeof( TObject )
						);
				}
				case SerializerMethod.UnpackFromCore:
				{
					return
						typeof( Func<,,,> ).MakeGenericType(
							serializerType,
							typeof( SerializationContext ),
							typeof( Unpacker ),
							typeof( TObject )
						);
				}
				case SerializerMethod.UnpackToCore:
				{
					return
						typeof( Action<,,,> ).MakeGenericType(
							serializerType,
							typeof( SerializationContext ),
							typeof( Unpacker ),
							typeof( TObject )
						);
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "method", method.ToString() );
				}
			}
		}

		/// <summary>
		///		Creates the type of the delegate.
		/// </summary>
		/// <typeparam name="TObject">The type of serialization target.</typeparam>
		/// <param name="method">The method to be created.</param>
		/// <returns>
		///		The <see cref="Type"/> of delegate which can refer to the generating method.
		///		This value will not be <c>null</c>.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		<paramref name="method"/> is unknown.
		/// </exception>
		public static Type CreateDelegateType<TObject>( EnumSerializerMethod method )
		{
			switch ( method )
			{
				case EnumSerializerMethod.PackUnderlyingValueTo:
				{
					return
						typeof( Action<,,> )
						.MakeGenericType(
							typeof( ExpressionCallbackEnumMessagePackSerializer<> ).MakeGenericType( typeof( TObject ) ),
							typeof( Packer ),
							typeof( TObject )
						);
				}
				case EnumSerializerMethod.UnpackFromUnderlyingValue:
				{
					return
						typeof( Func<,,> )
						.MakeGenericType(
							typeof( ExpressionCallbackEnumMessagePackSerializer<> ).MakeGenericType( typeof( TObject ) ),
							typeof( MessagePackObject ),
							typeof( TObject )
						);
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "method", method.ToString() );
				}
			}
		}

		/// <summary>
		///		Creates the type of the delegate.
		/// </summary>
		/// <typeparam name="TObject">The type of serialization target.</typeparam>
		/// <param name="method">The method to be created.</param>
		/// <param name="serializerType">The type of callback serializer.</param>
		/// <param name="traits">The collection traits of the type.</param>
		/// <returns>
		///		The <see cref="Type"/> of delegate which can refer to the generating method.
		///		This value will not be <c>null</c>.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		<paramref name="method"/> is unknown.
		/// </exception>
		public static Type CreateDelegateType<TObject>( CollectionSerializerMethod method, Type serializerType, CollectionTraits traits )
		{
			switch ( method )
			{
				case CollectionSerializerMethod.AddItem:
				{
					if ( traits.DetailedCollectionType == CollectionDetailedKind.GenericDictionary 
#if !NETFX_40  && !( SILVERLIGHT && !WINDOWS_PHONE )
						|| traits.DetailedCollectionType == CollectionDetailedKind.GenericReadOnlyDictionary
#endif // !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
					)
					{
						return
							typeof( Action<,,,,> ).MakeGenericType(
								serializerType,
								typeof( SerializationContext ),
								typeof( TObject ),
								traits.ElementType.GetGenericArguments()[ 0 ],
								traits.ElementType.GetGenericArguments()[ 1 ]
							);
					}
					else
					{
						return
							typeof( Action<,,,> ).MakeGenericType(
								serializerType,
								typeof( SerializationContext ),
								typeof( TObject ),
								traits.ElementType
							);
					}
				}
				case CollectionSerializerMethod.CreateInstance:
				{
					return
						typeof( Func<,,,> ).MakeGenericType(
							serializerType,
							typeof( SerializationContext ),
							typeof( int ),
							typeof( TObject )
						);
				}
				case CollectionSerializerMethod.RestoreSchema:
				{
					return
						typeof( Func<> ).MakeGenericType(
							typeof(PolymorphismSchema)
						);
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "method", method.ToString() );
				}
			}
		}
		/// <summary>
		///		Gets the <see cref="ParameterExpression"/>s for specified method.
		/// </summary>
		/// <param name="method">The method to be created.</param>
		/// <returns>
		///		The <see cref="ParameterExpression"/>s for specified method.
		///		This value will not be <c>null</c>.
		/// </returns>
		private IEnumerable<ParameterExpression> GetParameters( SerializerMethod method )
		{
			yield return this.This.Expression as ParameterExpression;
			yield return this._context.Expression as ParameterExpression;

			switch ( method )
			{
				case SerializerMethod.PackToCore:
				{
					yield return this.Packer.Expression as ParameterExpression;
					yield return this.PackToTarget.Expression as ParameterExpression;
					break;
				}
				case SerializerMethod.UnpackFromCore:
				{
					yield return this.Unpacker.Expression as ParameterExpression;
					break;
				}
				case SerializerMethod.UnpackToCore:
				{
					yield return this.Unpacker.Expression as ParameterExpression;
					yield return this.UnpackToTarget.Expression as ParameterExpression;
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "method", method.ToString() );
				}
			}
		}

		/// <summary>
		///		Gets the <see cref="ParameterExpression"/>s for specified method.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="method">The method to be created.</param>
		/// <returns>
		///		The <see cref="ParameterExpression"/>s for specified method.
		///		This value will not be <c>null</c>.
		/// </returns>
		private IEnumerable<ParameterExpression> GetParameters( Type targetType, EnumSerializerMethod method )
		{
			yield return this.This.Expression as ParameterExpression;

			switch ( method )
			{
				case EnumSerializerMethod.PackUnderlyingValueTo:
				{
					yield return this.Packer.Expression as ParameterExpression;
					yield return Expression.Parameter( targetType, "enumValue" );
					break;
				}
				case EnumSerializerMethod.UnpackFromUnderlyingValue:
				{
					yield return Expression.Parameter( typeof( MessagePackObject ), "messagePackObject" );
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "method", method.ToString() );
				}
			}
		}


		/// <summary>
		///		Gets the <see cref="ParameterExpression"/>s for specified method.
		/// </summary>
		/// <param name="method">The method to be created.</param>
		/// <param name="traits">The traits of the collection.</param>
		/// <returns>
		///		The <see cref="ParameterExpression"/>s for specified method.
		///		This value will not be <c>null</c>.
		/// </returns>
		private IEnumerable<ParameterExpression> GetParameters( CollectionSerializerMethod method, CollectionTraits traits )
		{
			switch ( method )
			{
				case CollectionSerializerMethod.AddItem:
				{
					yield return this.This.Expression as ParameterExpression;
					yield return this._context.Expression as ParameterExpression;
					yield return this.CollectionToBeAdded.Expression as ParameterExpression;
					if ( traits.DetailedCollectionType == CollectionDetailedKind.GenericDictionary
#if !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
						|| traits.DetailedCollectionType == CollectionDetailedKind.GenericReadOnlyDictionary
#endif // !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
					)
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
				case CollectionSerializerMethod.CreateInstance:
				{
					yield return this.This.Expression as ParameterExpression;
					yield return this._context.Expression as ParameterExpression;
					yield return this.InitialCapacity.Expression as ParameterExpression;
					break;
				}
				case CollectionSerializerMethod.RestoreSchema:
				{
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "method", method.ToString() );
				}
			}
		}

		/// <summary>
		///		Sets the specified delegate object for specified method.
		/// </summary>
		/// <param name="method">The method to be created.</param>
		/// <param name="delegate">The delegate which refers the generated method.</param>
		public void SetDelegate( SerializerMethod method, Delegate @delegate )
		{
			switch ( method )
			{
				case SerializerMethod.PackToCore:
				{
					this._packToCore = @delegate;
					break;
				}
				case SerializerMethod.UnpackFromCore:
				{
					this._unpackFromCore = @delegate;
					break;
				}
				case SerializerMethod.UnpackToCore:
				{
					this._unpackToCore = @delegate;
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "method", method.ToString() );
				}
			}
		}

		/// <summary>
		///		Sets the specified delegate object for specified method.
		/// </summary>
		/// <param name="method">The method to be created.</param>
		/// <param name="delegate">The delegate which refers the generated method.</param>
		public void SetDelegate( EnumSerializerMethod method, Delegate @delegate )
		{
			switch ( method )
			{
				case EnumSerializerMethod.PackUnderlyingValueTo:
				{
					this._packUnderyingValueTo = @delegate;
					break;
				}
				case EnumSerializerMethod.UnpackFromUnderlyingValue:
				{
					this._unpackFromUnderlyingValue = @delegate;
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "method", method.ToString() );
				}
			}
		}

		/// <summary>
		///		Sets the specified delegate object for specified method.
		/// </summary>
		/// <param name="method">The method to be created.</param>
		/// <param name="delegate">The delegate which refers the generated method.</param>
		public void SetDelegate( CollectionSerializerMethod method, Delegate @delegate )
		{
			switch ( method )
			{
				case CollectionSerializerMethod.AddItem:
				{
					this._addItem = @delegate;
					break;
				}
				case CollectionSerializerMethod.CreateInstance:
				{
					this._createInstance = @delegate;
					break;
				}
				case CollectionSerializerMethod.RestoreSchema:
				{
					// Note: RestoreSchema is not useless in Expression serializer, so nothing to do here.
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "method", method.ToString() );
				}
			}
		}

		/// <summary>
		///		Gets the delegate which refers created <c>PackToCore(SerializationContext,Packer,T)</c> instance method for <see cref="ExpressionCallbackMessagePackSerializer{T}"/>.
		/// </summary>
		/// <returns>
		///		The delegate which was set for <c>PackToCore</c> method.
		/// </returns>
		public Delegate GetPackToCore()
		{
			return this._packToCore;
		}

		/// <summary>
		///		Gets the delegate which refers created <c>UnpackFromCore(SerializationContext,Unpacker)</c> instance method for <see cref="ExpressionCallbackMessagePackSerializer{T}"/>.
		/// </summary>
		/// <returns>
		///		The delegate which was set for <c>UnpackFromCore</c> method.
		/// </returns>
		public Delegate GetUnpackFromCore()
		{
			return this._unpackFromCore;
		}

		/// <summary>
		///		Gets the delegate which refers created <c>UnpackToCore(SerializationContext,Unpacker,T)</c> instance method for <see cref="ExpressionCallbackMessagePackSerializer{T}"/>.
		/// </summary>
		/// <returns>
		///		The delegate which was set for <c>UnpackToCore</c> method.
		/// </returns>
		public Delegate GetUnpackToCore()
		{
			return this._unpackToCore;
		}

		/// <summary>
		///		Gets the delegate which refers created <c>PackUnderlyingValueTo(Packer,T)</c> instance method for <see cref="ExpressionCallbackEnumMessagePackSerializer{T}"/>.
		/// </summary>
		/// <returns>
		///		The delegate which was set for <c>UnpackToCore</c> method.
		/// </returns>
		public Delegate GetPackUnderyingValueTo()
		{
			return this._packUnderyingValueTo;
		}

		/// <summary>
		///		Gets the delegate which refers created <c>UnpackFromUnderlyingValue(MessagePackObject)</c> instance method for <see cref="ExpressionCallbackEnumMessagePackSerializer{T}"/>.
		/// </summary>
		/// <returns>
		///		The delegate which was set for <c>UnpackFromCore</c> method.
		/// </returns>
		public Delegate GetUnpackFromUnderlyingValue()
		{
			return this._unpackFromUnderlyingValue;
		}

		/// <summary>
		///		Gets the delegate which refers created <c>UnpackToCore(SerializationContext,Unpacker,T)</c> instance method for <see cref="ExpressionCallbackMessagePackSerializer{T}"/>.
		/// </summary>
		/// <returns>
		///		The delegate which was set for <c>UnpackToCore</c> method.
		/// </returns>
		public Delegate GetCreateInstance()
		{
			return this._createInstance;
		}

		/// <summary>
		///		Gets the delegate which refers created <c>UnpackToCore(SerializationContext,Unpacker,T)</c> instance method for <see cref="ExpressionCallbackMessagePackSerializer{T}"/>.
		/// </summary>
		/// <returns>
		///		The delegate which was set for <c>UnpackToCore</c> method.
		/// </returns>
		public Delegate GetAddItem()
		{
			return this._addItem;
		}
	}
}