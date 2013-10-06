#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
#if NETFX_CORE
using System.Linq;
#endif
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using MsgPack.Serialization.Metadata;

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		<see cref="MessagePackSerializer{T}"/> for a sequential collection using expression tree.
	/// </summary>
	/// <typeparam name="T">The type of element.</typeparam>
	internal abstract class SequenceExpressionMessagePackSerializer<T> : MessagePackSerializer<T>
#if !SILVERLIGHT
, IExpressionMessagePackSerializer
#endif
	{
		private readonly Func<T, int> _getCount;
		private readonly CollectionTraits _traits;

		protected CollectionTraits Traits
		{
			get { return this._traits; }
		}

		private readonly IMessagePackSerializer _elementSerializer;

		private readonly Action<Packer, T, IMessagePackSerializer> _packToCore;
		private readonly Action<Unpacker, T, IMessagePackSerializer> _unpackToCore;
#if !SILVERLIGHT
		private readonly Expression _packToCoreExpression;
		private readonly Expression _unpackToCoreExpression;
#endif

		protected SequenceExpressionMessagePackSerializer( SerializationContext context, CollectionTraits traits )
			: base( ( context ?? SerializationContext.Default ).CompatibilityOptions.PackerCompatibilityOptions )
		{
			Contract.Assert( typeof( T ).IsArray || typeof( IEnumerable ).IsAssignableFrom( typeof( T ) ), typeof( T ) + " is not array nor IEnumerable" );
			this._traits = traits;
			this._elementSerializer = context.GetSerializer( traits.ElementType );
			this._getCount = ExpressionSerializerLogics.CreateGetCount<T>( traits );

			var packerParameter = Expression.Parameter( typeof( Packer ), "packer" );
			var objectTreeParameter = Expression.Parameter( typeof( T ), "objectTree" );
			var elementSerializerParameter = Expression.Parameter( typeof( IMessagePackSerializer ), "elementSerializer" );
			var elementSerializerType = typeof( MessagePackSerializer<> ).MakeGenericType( traits.ElementType );

			/*
			 *	packer.PackArrayHeader( objectTree.Count() );
			 *	foreach( var item in objectTree )
			 *	{
			 *		elementSerializer.PackTo( packer, item );
			 *	}
			 */
			var packToCore =
				Expression.Lambda<Action<Packer, T, IMessagePackSerializer>>(
					Expression.Block(
						Expression.Call(
							packerParameter,
							Metadata._Packer.PackArrayHeader,
							ExpressionSerializerLogics.CreateGetCountExpression<T>( traits, objectTreeParameter )
						),
						traits.AddMethod == null
						? ExpressionSerializerLogics.For(
							Expression.ArrayLength( objectTreeParameter ),
							indexVariable =>
								Expression.Call(
									Expression.TypeAs( elementSerializerParameter, elementSerializerType ),
									typeof( MessagePackSerializer<> ).MakeGenericType( traits.ElementType ).GetMethod( "PackTo" ),
									packerParameter,
									Expression.ArrayIndex( objectTreeParameter, indexVariable )
								)
						)
						: ExpressionSerializerLogics.ForEach(
							objectTreeParameter,
							traits,
							elementVariable =>
								Expression.Call(
									Expression.TypeAs( elementSerializerParameter, elementSerializerType ),
									typeof( MessagePackSerializer<> ).MakeGenericType( traits.ElementType ).GetMethod( "PackTo" ),
									packerParameter,
									elementVariable
								)
						)
					),
					packerParameter, objectTreeParameter, elementSerializerParameter
				);

#if !SILVERLIGHT
			if ( context.GeneratorOption == SerializationMethodGeneratorOption.CanDump )
			{
				this._packToCoreExpression = packToCore;
			}
#endif

			this._packToCore = packToCore.Compile();

			var unpackerParameter = Expression.Parameter( typeof( Unpacker ), "unpacker" );
			var instanceParameter = Expression.Parameter( typeof( T ), "instance" );
			var countParamter = Expression.Parameter( typeof( int ), "count" );

			/*
			 *	for ( int i = 0; i < count; i++ )
			 *	{
			 *		if ( !unpacker.Read() )
			 *		{
			 *			throw SerializationExceptions.NewMissingItem( i );
			 *		}
			 *	
			 *		T item;
			 *		if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
			 *		{
			 *			item = this.ElementSerializer.UnpackFrom( unpacker );
			 *		}
			 *		else
			 *		{
			 *			using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
			 *			{
			 *				item = this.ElementSerializer.UnpackFrom( subtreeUnpacker );
			 *			}
			 *		}
			 *
			 *		instance[ i ] = item; -- OR -- instance.Add( item );
			 *	}
			 */

			// FIXME: use UnpackHelper
			Expression<Action<Unpacker, T, IMessagePackSerializer>> unpackToCore;

			if ( typeof( T ).IsArray )
			{
				unpackToCore =
					Expression.Lambda<Action<Unpacker, T, IMessagePackSerializer>>(
						Expression.Call(
							null,
							_UnpackHelpers.UnpackArrayTo_1.MakeGenericMethod( traits.ElementType ),
							unpackerParameter,
							Expression.TypeAs( elementSerializerParameter, typeof( MessagePackSerializer<> ).MakeGenericType( traits.ElementType ) ),
							instanceParameter
						),
						unpackerParameter, instanceParameter, elementSerializerParameter
					);
			}
			else if ( typeof( T ).GetIsGenericType() )
			{
				MethodInfo unpackCollectionToMethod;
				Type delegateType;
				if ( traits.AddMethod.ReturnType == null || traits.AddMethod.ReturnType == typeof( void ) )
				{
					unpackCollectionToMethod = _UnpackHelpers.UnpackCollectionTo_1.MakeGenericMethod( traits.ElementType );
					delegateType = typeof( Action<> ).MakeGenericType( traits.ElementType );
				}
				else
				{
					unpackCollectionToMethod = _UnpackHelpers.UnpackCollectionTo_2.MakeGenericMethod( traits.ElementType, traits.AddMethod.ReturnType );
					delegateType = typeof( Func<,> ).MakeGenericType( traits.ElementType, traits.AddMethod.ReturnType );
				}

				var itemParameter = Expression.Parameter( traits.ElementType, "item" );
				unpackToCore =
					Expression.Lambda<Action<Unpacker, T, IMessagePackSerializer>>(
						Expression.Call(
							null,
							unpackCollectionToMethod,
							unpackerParameter,
							Expression.TypeAs( elementSerializerParameter, typeof( MessagePackSerializer<> ).MakeGenericType( traits.ElementType ) ),
							Expression.TypeAs( instanceParameter, typeof( IEnumerable<> ).MakeGenericType( traits.ElementType ) ),
							Expression.Lambda(
								delegateType,
								Expression.Call(
									instanceParameter,
									traits.AddMethod,
									itemParameter
								),
								itemParameter
							)
						),
						unpackerParameter, instanceParameter, elementSerializerParameter
					);
			}
			else
			{
				MethodInfo unpackCollectionToMethod;
				Type delegateType;
				if ( traits.AddMethod.ReturnType == null || traits.AddMethod.ReturnType == typeof( void ) )
				{
					unpackCollectionToMethod = _UnpackHelpers.UnpackNonGenericCollectionTo;
					delegateType = typeof( Action<> ).MakeGenericType( traits.ElementType );
				}
				else
				{
					unpackCollectionToMethod = _UnpackHelpers.UnpackNonGenericCollectionTo_1.MakeGenericMethod( traits.AddMethod.ReturnType );
					delegateType = typeof( Func<,> ).MakeGenericType( traits.ElementType, traits.AddMethod.ReturnType );
				}

				var itemParameter = Expression.Parameter( traits.ElementType, "item" );
				unpackToCore =
					Expression.Lambda<Action<Unpacker, T, IMessagePackSerializer>>(
						Expression.Call(
							null,
							unpackCollectionToMethod,
							unpackerParameter,
							Expression.TypeAs( instanceParameter, typeof( IEnumerable ) ),
							Expression.Lambda(
								delegateType,
								Expression.Call(
									instanceParameter,
									traits.AddMethod,
									itemParameter
								),
								itemParameter
							)
						),
						unpackerParameter, instanceParameter, elementSerializerParameter
					);
			}

#if !SILVERLIGHT
			if ( context.GeneratorOption == SerializationMethodGeneratorOption.CanDump )
			{
				this._unpackToCoreExpression = unpackToCore;
			}
#endif

			this._unpackToCore = unpackToCore.Compile();
		}

		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			this._packToCore( packer, objectTree, this._elementSerializer );
		}

		protected internal override void UnpackToCore( Unpacker unpacker, T collection )
		{
			this._unpackToCore( unpacker, collection, this._elementSerializer );
		}

#if !SILVERLIGHT
		public override string ToString()
		{
			if ( this._packToCoreExpression == null || this._unpackToCoreExpression == null )
			{
				return base.ToString();
			}
			else
			{
				var buffer = new StringBuilder( Int16.MaxValue );
				using ( var writer = new StringWriter( buffer ) )
				{
					this.ToStringCore( writer, 0 );
				}

				return buffer.ToString();
			}
		}

		void IExpressionMessagePackSerializer.ToString( TextWriter writer, int depth )
		{
			this.ToStringCore( writer ?? TextWriter.Null, depth < 0 ? 0 : depth );
		}

		private void ToStringCore( TextWriter writer, int depth )
		{
			var name = this.GetType().Name;
			int indexOfAgusam = name.IndexOf( '`' );
			int nameLength = indexOfAgusam < 0 ? name.Length : indexOfAgusam;
			for ( int i = 0; i < nameLength; i++ )
			{
				writer.Write( name[ i ] );
			}

			writer.Write( "For" );
			writer.WriteLine( typeof( T ) );

			ExpressionDumper.WriteIndent( writer, depth + 1 );
			writer.Write( "PackToCore : " );
			new ExpressionDumper( writer, depth + 2 ).Visit( this._packToCoreExpression );
			writer.WriteLine();

			ExpressionDumper.WriteIndent( writer, depth + 1 );
			writer.Write( "UnpackToCore : " );
			new ExpressionDumper( writer, depth + 2 ).Visit( this._unpackToCoreExpression );
		}
#endif
	}
}
