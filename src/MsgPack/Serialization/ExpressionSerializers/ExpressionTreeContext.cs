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
using System.Collections.Generic;
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
		public ExpressionConstruct Context
		{
			get { return this._context; }
		}

		private readonly ExpressionConstruct _this;

		/// <summary>
		///		Gets the code construct which represents 'this' reference.
		/// </summary>
		/// <value>
		///		The code construct which represents 'this' reference.
		/// </value>
		public ExpressionConstruct This
		{
			get { return this._this; }
		}

		private Delegate _packToCore;
		private Delegate _unpackFromCore;
		private Delegate _unpackToCore;

		/// <summary>
		///		Initializes a new instance of the <see cref="ExpressionTreeContext"/> class.
		/// </summary>
		/// <param name="serializationContext">The serialization context.</param>
		/// <param name="packer">
		///		The code construct which represents the argument for the packer.
		///	</param>
		/// <param name="packingTarget">
		///		The code construct which represents the argument for the packing target object tree root.
		/// </param>
		/// <param name="unpacker">
		///		The code construct which represents the argument for the unpacker.
		/// </param>
		/// <param name="unpackToTarget">
		///		The code construct which represents the argument for the collection which will hold unpacked items.
		/// </param>
		public ExpressionTreeContext( SerializationContext serializationContext, ExpressionConstruct packer, ExpressionConstruct packingTarget, ExpressionConstruct unpacker, ExpressionConstruct unpackToTarget )
			: base( serializationContext, packer, packingTarget, unpacker, unpackToTarget )
		{
			this._context = Expression.Parameter( typeof( SerializationContext ), "context" );
			this._this =
				Expression.Parameter(
					typeof( ExpressionCallbackMessagePackSerializer<> ).MakeGenericType( packingTarget.ContextType ), "this"
					);
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
		public static Type CreateDelegateType<TObject>( SerializerMethod method )
		{
			switch ( method )
			{
				case SerializerMethod.PackToCore:
				{
					return typeof( Action<ExpressionCallbackMessagePackSerializer<TObject>, SerializationContext, Packer, TObject> );
				}
				case SerializerMethod.UnpackFromCore:
				{
					return typeof( Func<ExpressionCallbackMessagePackSerializer<TObject>, SerializationContext, Unpacker, TObject> );
				}
				case SerializerMethod.UnpackToCore:
				{
					return typeof( Action<ExpressionCallbackMessagePackSerializer<TObject>, SerializationContext, Unpacker, TObject> );
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
		public IEnumerable<ParameterExpression> GetParameters( SerializerMethod method )
		{
			yield return this._this.Expression as ParameterExpression;
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
		///		Gets the delegate which refers created <c>PackToCore(SerializationContext,Packer,T)</c> instance method for <see cref="ExpressionCallbackMessagePackSerializer{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of serialization target.</typeparam>
		/// <returns>
		///		The delegate which was set for <c>PackToCore</c> method.
		/// </returns>
		public Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T> GetPackToCore<T>()
		{
			return this._packToCore as Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T>;
		}

		/// <summary>
		///		Gets the delegate which refers created <c>UnpackFromCore(SerializationContext,Unpacker)</c> instance method for <see cref="ExpressionCallbackMessagePackSerializer{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of deserialization target.</typeparam>
		/// <returns>
		///		The delegate which was set for <c>UnpackFromCore</c> method.
		/// </returns>
		public Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> GetUnpackFromCore<T>()
		{
			return this._unpackFromCore as Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T>;
		}

		/// <summary>
		///		Gets the delegate which refers created <c>UnpackToCore(SerializationContext,Unpacker,T)</c> instance method for <see cref="ExpressionCallbackMessagePackSerializer{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of deserialization target.</typeparam>
		/// <returns>
		///		The delegate which was set for <c>UnpackToCore</c> method.
		/// </returns>
		public Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> GetUnpackToCore<T>()
		{
			return this._unpackToCore as Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T>;
		}
	}
}