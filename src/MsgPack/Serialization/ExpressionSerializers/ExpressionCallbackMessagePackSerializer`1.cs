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

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		A helper <see cref="MessagePackSerializer{T}"/> for <see cref="ExpressionTreeSerializerBuilder{TObject}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the serialization target.</typeparam>
	internal class ExpressionCallbackMessagePackSerializer<T> : MessagePackSerializer<T>
	{
		private readonly Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T> _packToCore;
		private readonly Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> _unpackFromCore;
		private readonly Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> _unpackToCore;
		public Func<object, T> CreateInstanceFromContext { get; private set; }
		public IList<Action<Packer, T>> PackOperationList { get; private set; }
		public IDictionary<string, Action<Packer, T>> PackOperationTable { get; private set; }
		public IList<Action<Unpacker, object, int, int>> UnpackOperationList { get; private set; }
		public IDictionary<string, Action<Unpacker, object, int, int>> UnpackOperationTable { get; private set; }
		public IDictionary<string, Delegate> Delegates { get; private set; }
		public IList<string> MemberNames { get; private set; }

		/// <summary>
		///		Initializes a new instance of the <see cref="ExpressionCallbackMessagePackSerializer{T}"/> class.
		/// </summary>
		/// <param name="ownerContext">The serialization context.</param>
		/// <param name="packToCore">The delegate to <c>PackToCore</c> method body. This value must not be <c>null</c>.</param>
		/// <param name="unpackFromCore">The delegate to <c>UnpackFromCore</c> method body. This value must not be <c>null</c>.</param>
		/// <param name="unpackToCore">The delegate to <c>UnpackToCore</c> method body. This value can be <c>null</c>.</param>
		/// <param name="packOperationList">The list of <see cref="PackToCore"/> actions for array.</param>
		/// <param name="packOperationTable">The dictionary of <see cref="PackToCore"/> actions for map.</param>
		/// <param name="unpackOperationList">The list of <see cref="UnpackFromCore"/> actions for array.</param>
		/// <param name="unpackOperationTable">The dictionary of <see cref="UnpackFromCore"/> actions for map.</param>
		/// <param name="delegates">The lamda expression to &quot;private methods&quot; with their names.</param>
		/// <param name="createInstanceFromContext">The delegate to <c>CreateInstanceFromContext</c> method body.</param>
		/// <param name="memberNames">The list of member names.</param>
		public ExpressionCallbackMessagePackSerializer(
			SerializationContext ownerContext,
			Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T> packToCore,
			Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> unpackFromCore,
			Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> unpackToCore,
			IList<Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T>> packOperationList,
			IDictionary<string, Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T>> packOperationTable,
			IList<Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, object, int, int>> unpackOperationList,
			IDictionary<string, Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, object, int, int>> unpackOperationTable,
			IDictionary<string, LambdaExpression> delegates,
			Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, object, T> createInstanceFromContext,
			IList<string> memberNames
		)
			: base( ownerContext )
		{
#if DEBUG
			Contract.Assert( packToCore != null );
			Contract.Assert( unpackFromCore != null );
			Contract.Assert( packOperationList != null );
			Contract.Assert( unpackOperationList != null );
			Contract.Assert( unpackOperationTable != null );
			Contract.Assert( delegates != null );
			Contract.Assert( memberNames != null );
#endif // DEBUG

			this._packToCore = packToCore;
			this._unpackFromCore = unpackFromCore;
			this._unpackToCore = unpackToCore;
			this.PackOperationList =
				packOperationList.Select(
					a => new Action<Packer, T>( ( packer, objectTree ) => a( this, this.OwnerContext, packer, objectTree ) )
				).ToArray();
			this.PackOperationTable =
				packOperationTable.ToDictionary(
					kv => kv.Key,
					kv => new Action<Packer, T>( ( packer, objectTree ) => kv.Value( this, this.OwnerContext, packer, objectTree ) )
				);
			this.UnpackOperationList =
				unpackOperationList.Select(
					a =>
						new Action<Unpacker, object, int, int>(
							( unpacker, unpackingContext, itemIndex, itemsCount ) => a( this, this.OwnerContext, unpacker, unpackingContext, itemIndex, itemsCount )
						)
				).ToArray();
			this.UnpackOperationTable =
				unpackOperationTable.ToDictionary(
					kv => kv.Key,
					kv =>
						new Action<Unpacker, object, int, int>(
							( unpacker, unpackingContext, itemIndex, itemsCount ) => kv.Value( this, this.OwnerContext, unpacker, unpackingContext, itemIndex, itemsCount )
						)
				);
			this.Delegates = ExpressionTreeSerializerBuilderHelpers.SupplyPrivateMethodCommonArguments( this, delegates );
			this.CreateInstanceFromContext =
				unpackingContext =>
					createInstanceFromContext( this, this.OwnerContext, unpackingContext );
			this.MemberNames = memberNames;
		}

		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			this._packToCore( this, this.OwnerContext, packer, objectTree );
		}

		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			return this._unpackFromCore( this, this.OwnerContext, unpacker );
		}

		protected internal override void UnpackToCore( Unpacker unpacker, T collection )
		{
			if ( this._unpackToCore != null )
			{
				this._unpackToCore( this, this.OwnerContext, unpacker, collection );
			}
			else
			{
				base.UnpackToCore( unpacker, collection );
			}
		}
	}
}