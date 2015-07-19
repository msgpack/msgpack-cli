#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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

using MsgPack.Serialization.AbstractSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		Implements <see cref="SerializerGenerationContext{TConstruct}"/> for <see cref="ILEmittingSerializerBuilder{TContext,TObject}"/>.
	/// </summary>
	internal class ILEmittingContext : SerializerGenerationContext<ILConstruct>
	{
		/// <summary>
		///		Gets or sets the <see cref="TracingILGenerator"/> to emit IL for current method.
		/// </summary>
		/// <value>
		///		The <see cref="TracingILGenerator"/> to emit IL for current method.
		/// </value>
		internal TracingILGenerator IL { get; set; }

		private readonly Func<SerializerEmitter> _emitterFactory;
		private readonly Func<EnumSerializerEmitter> _enumEmitterFactory;

		private SerializerEmitter _emitter;
		private EnumSerializerEmitter _enumEmitter;

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
		///		Gets the <see cref="EnumSerializerEmitter"/>.
		/// </summary>
		/// <value>
		///		The <see cref="EnumSerializerEmitter"/>.
		/// </value>
		internal EnumSerializerEmitter EnumEmitter
		{
			get
			{
				if ( this._enumEmitter == null )
				{
					this._enumEmitter = this._enumEmitterFactory();
				}

				return this._enumEmitter;
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

		/// <summary>
		/// Initializes a new instance of the <see cref="ILEmittingContext"/> class.
		/// </summary>
		/// <param name="context">The serialization context.</param>
		/// <param name="emitterFactory">
		///		The factory for <see cref="SerializerEmitter"/> to be used.
		/// </param>
		/// <param name="enumEmitterFactory">
		///		The factory for <see cref="EnumSerializerEmitter"/> to be used.
		/// </param>
		public ILEmittingContext(
			SerializationContext context,
			Func<SerializerEmitter> emitterFactory,
			Func<EnumSerializerEmitter> enumEmitterFactory
		)
			: base( context )
		{
			this._emitterFactory = emitterFactory;
			this._enumEmitterFactory = enumEmitterFactory;
		}

		protected sealed override void ResetCore( Type targetType, Type baseClass )
		{
			// Note: baseClass is always null this class hiearchy.
			this.Packer = ILConstruct.Argument( 1, typeof( Packer ), "packer" );
			this.PackToTarget = ILConstruct.Argument( 2, targetType, "objectTree" );
			this.Unpacker = ILConstruct.Argument( 1, typeof( Unpacker ), "unpacker" );
			this.UnpackToTarget = ILConstruct.Argument( 2, targetType, "collection" );
			var traits = targetType.GetCollectionTraits();
			if ( traits.ElementType != null )
			{
				this.CollectionToBeAdded = ILConstruct.Argument( 1, targetType, "collection" );
				this.ItemToAdd = ILConstruct.Argument( 2, traits.ElementType, "item" );
				if ( traits.DetailedCollectionType == CollectionDetailedKind.GenericDictionary
#if !NETFX_35 && !UNITY && !NETFX_40 && !SILVERLIGHT
					|| traits.DetailedCollectionType == CollectionDetailedKind.GenericReadOnlyDictionary
#endif // !NETFX_35 && !UNITY && !NETFX_40 && !SILVERLIGHT
 )
				{
					this.KeyToAdd = ILConstruct.Argument( 2, traits.ElementType.GetGenericArguments()[ 0 ], "key" );
					this.ValueToAdd = ILConstruct.Argument( 3, traits.ElementType.GetGenericArguments()[ 1 ], "value" );
				}
				this.InitialCapacity = ILConstruct.Argument( 1, typeof( int ), "initialCapacity" );
			}
			this._emitter = null;
			this._enumEmitter = null;
		}
	}
}