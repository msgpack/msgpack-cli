#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
#if NETFX_CORE
using System.Reflection;
#endif
using System.Text;

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		<see cref="MessagePackSerializer{T}"/> for a map collection using expression tree.
	/// </summary>
	/// <typeparam name="T">The type of element.</typeparam>
	internal class MapExpressionMessagePackSerializer<T> : MessagePackSerializer<T>
#if !SILVERLIGHT
		, IExpressionMessagePackSerializer
#endif
	{
		private readonly Func<T, int> _getCount;
		private readonly CollectionTraits _traits;
		private readonly IMessagePackSerializer _keySerializer;
		private readonly IMessagePackSerializer _valueSerializer;
		private readonly Action<Packer, T, IMessagePackSerializer, IMessagePackSerializer> _packToCore;
		private readonly Action<Unpacker, T, IMessagePackSerializer, IMessagePackSerializer, int> _unpackToCore;
		private readonly Func<int, T> _createInstanceWithCapacity;
		private readonly Func<T> _createInstance;
#if !SILVERLIGHT
		private readonly Expression _packToCoreExpression;
		private readonly Expression _unpackToCoreExpression;
#endif

		public MapExpressionMessagePackSerializer( SerializationContext context, CollectionTraits traits )
		{
			Contract.Assert( typeof( IEnumerable ).IsAssignableFrom( typeof( T ) ), typeof( T ) + " is IEnumerable" );
			Contract.Assert( traits.ElementType == typeof( DictionaryEntry ) || ( traits.ElementType.GetIsGenericType() && traits.ElementType.GetGenericTypeDefinition() == typeof( KeyValuePair<,> ) ), "Element type " + traits.ElementType + " is not KeyValuePair<TKey,TValue>." );
			this._traits = traits;
			this._keySerializer = context.GetSerializer( traits.ElementType.GetGenericArguments()[ 0 ] );
			this._valueSerializer = context.GetSerializer( traits.ElementType.GetGenericArguments()[ 1 ] );
			this._getCount = ExpressionSerializerLogics.CreateGetCount<T>( traits );

			var constructor = ExpressionSerializerLogics.GetCollectionConstructor<T>();
			if ( constructor.GetParameters().Length == 1 )
			{
				this._createInstance = null;

				var capacityParameter = Expression.Parameter( typeof( int ), "parameter" );
				this._createInstanceWithCapacity =
					Expression.Lambda<Func<int, T>>(
						Expression.New( constructor, capacityParameter ),
						capacityParameter
					).Compile();
			}
			else
			{
				this._createInstanceWithCapacity = null;
				this._createInstance =
					Expression.Lambda<Func<T>>(
						Expression.New( constructor )
					).Compile();
			}

			var packerParameter = Expression.Parameter( typeof( Packer ), "packer" );
			var objectTreeParameter = Expression.Parameter( typeof( T ), "objectTree" );
			var keySerializerParameter = Expression.Parameter( typeof( IMessagePackSerializer ), "keySerializer" );
			var valueSerializerParameter = Expression.Parameter( typeof( IMessagePackSerializer ), "valueSerializer" );
			var keyType = traits.ElementType.GetGenericArguments()[ 0 ];
			var valueType = traits.ElementType.GetGenericArguments()[ 1 ];
			var keySerializerType = typeof( MessagePackSerializer<> ).MakeGenericType( keyType );
			var valueSerializerType = typeof( MessagePackSerializer<> ).MakeGenericType( valueType );

			/*
				 *	packer.PackMapHeader( objectTree.Count() );
				 *	foreach( var item in objectTree )
				 *	{
				 *		elementSerializer.PackTo( packer, item.Key );
				 *		elementSerializer.PackTo( packer, item.Value );
				 *	}
				 */
			var packToCore =
				Expression.Lambda<Action<Packer, T, IMessagePackSerializer, IMessagePackSerializer>>(
					Expression.Block(
						Expression.Call(
							packerParameter,
							Metadata._Packer.PackMapHeader,
							ExpressionSerializerLogics.CreateGetCountExpression<T>( traits, objectTreeParameter )
						),
						ExpressionSerializerLogics.ForEach(
							objectTreeParameter,
							traits,
							elementVariable =>
								Expression.Block(
									Expression.Call(
										Expression.TypeAs( keySerializerParameter, keySerializerType ),
										typeof( MessagePackSerializer<> ).MakeGenericType( keyType ).GetMethod( "PackTo" ),
										packerParameter,
										Expression.Property( elementVariable, traits.ElementType.GetProperty( "Key" ) )
									),
									Expression.Call(
										Expression.TypeAs( valueSerializerParameter, valueSerializerType ),
										typeof( MessagePackSerializer<> ).MakeGenericType( valueType ).GetMethod( "PackTo" ),
										packerParameter,
										Expression.Property( elementVariable, traits.ElementType.GetProperty( "Value" ) )
									)
								)
						)
					), packerParameter, objectTreeParameter, keySerializerParameter, valueSerializerParameter
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
			 *		T key;
			 *		if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
			 *		{
			 *			key = this.ElementSerializer.UnpackFrom( unpacker );
			 *		}
			 *		else
			 *		{
			 *			using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
			 *			{
			 *				key = this.ElementSerializer.UnpackFrom( subtreeUnpacker );
			 *			}
			 *		}
			 *	
			 *		T value;
			 *		if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
			 *		{
			 *			value = this.ElementSerializer.UnpackFrom( unpacker );
			 *		}
			 *		else
			 *		{
			 *			using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
			 *			{
			 *				value = this.ElementSerializer.UnpackFrom( subtreeUnpacker );
			 *			}
			 *		}
			 *		
			 *		instance.Add( key, value );
			 *	}
			 */

			var keyVariable = Expression.Variable( traits.ElementType.GetGenericArguments()[ 0 ], "key" );
			var valueVariable = Expression.Variable( traits.ElementType.GetGenericArguments()[ 1 ], "value" );

			var unpackToCore =
				Expression.Lambda<Action<Unpacker, T, IMessagePackSerializer, IMessagePackSerializer, int>>(
					ExpressionSerializerLogics.For(
						countParamter,
						indexVariable =>
							Expression.Block(
								new[] { keyVariable, valueVariable },
								Expression.IfThen(
									Expression.IsFalse(
										Expression.Call( unpackerParameter, Metadata._Unpacker.Read )
									),
									Expression.Throw(
										Expression.Call( SerializationExceptions.NewMissingItemMethod, indexVariable )
									)
								),
								Expression.Assign(
									keyVariable,
									ExpressionSerializerLogics.CreateUnpackItem( unpackerParameter, typeof( MessagePackSerializer<> ).MakeGenericType( keyType ).GetMethod( "UnpackFrom" ), keySerializerParameter, keySerializerType )
								),
								Expression.Assign(
									valueVariable,
									ExpressionSerializerLogics.CreateUnpackItem( unpackerParameter, typeof( MessagePackSerializer<> ).MakeGenericType( valueType ).GetMethod( "UnpackFrom" ), valueSerializerParameter, valueSerializerType )
								),
								Expression.Call(
									instanceParameter,
									traits.AddMethod,
									keyVariable,
									valueVariable
								) as Expression
							)
					),
					unpackerParameter, instanceParameter, keySerializerParameter, valueSerializerParameter, countParamter
				);

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
			this._packToCore( packer, objectTree, this._keySerializer, this._valueSerializer );
		}

		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			var instance = this._createInstanceWithCapacity == null ? this._createInstance() : this._createInstanceWithCapacity( UnpackHelpers.GetItemsCount( unpacker ) );
			this.UnpackTo( unpacker, instance );
			return instance;
		}

		protected internal override void UnpackToCore( Unpacker unpacker, T collection )
		{
			this._unpackToCore( unpacker, collection, this._keySerializer, this._valueSerializer, UnpackHelpers.GetItemsCount( unpacker ) );
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
			new ExpressionDumper( writer, depth + 1 ).Visit( this._packToCoreExpression );
			writer.WriteLine();

			ExpressionDumper.WriteIndent( writer, depth + 1 );
			writer.Write( "UnpackToCore : " );
			new ExpressionDumper( writer, depth + 1 ).Visit( this._unpackToCoreExpression );
		}
#endif
	}
}
