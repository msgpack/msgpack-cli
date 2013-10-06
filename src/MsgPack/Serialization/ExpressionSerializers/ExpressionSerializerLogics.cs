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
using System.Collections;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
#if MONO
using System.Collections.Generic;
using System.Linq;
#endif

namespace MsgPack.Serialization.ExpressionSerializers
{
	internal static class ExpressionSerializerLogics
	{
		/// <summary>
		///		Creates a delegate which returns collection count.
		/// </summary>
		/// <typeparam name="T">The type of collection element.</typeparam>
		/// <param name="traits">The traits of the collection.</param>
		/// <returns>A delegate which returns collection count.</returns>
		public static Func<T, int> CreateGetCount<T>( CollectionTraits traits )
		{
			var targetParameter = Expression.Parameter( typeof( T ), "target" );
			return Expression.Lambda<Func<T, int>>( CreateGetCountExpression<T>( traits, targetParameter ), targetParameter ).Compile();
		}

		/// <summary>
		///		Creates an <see cref="Expression"/> which returns collection count.
		/// </summary>
		/// <typeparam name="T">The type of collection element.</typeparam>
		/// <param name="traits">The traits of the collection.</param>
		/// <param name="targetParameter">The parameter <see cref="Expression"/> which represents the target collection.</param>
		/// <returns>An <see cref="Expression"/> which returns collection count.</returns>
		public static Expression CreateGetCountExpression<T>( CollectionTraits traits, ParameterExpression targetParameter )
		{
			Expression body;
			if ( typeof( T ).IsArray )
			{
				body = Expression.PropertyOrField( targetParameter, "Length" );
			}
			else if ( traits.CountProperty != null )
			{
				body = Expression.Property( targetParameter, traits.CountProperty );
			}
			else
			{
				body =
					Expression.Call(
						Metadata._Enumerable.Count1Method.MakeGenericMethod( traits.ElementType ),
						targetParameter
					);
			}

			return body;
		}

		/// <summary>
		///		Creates an <see cref="Expression"/> which represents using statement block.
		/// </summary>
		/// <param name="variableType">The type of variable. It must be assignable to <see cref="IDisposable"/>.</param>
		/// <param name="expressionType">The type of entire expression. This can be <c>null</c>.</param>
		/// <param name="right">The right of using declaration .</param>
		/// <param name="bodyCreator">The body creator. The argument is left of using declaration.</param>
		/// <returns>An <see cref="Expression"/> which represents using statement block.</returns>
		public static Expression Using( Type variableType, Type expressionType, Expression right, Func<Expression, Expression> bodyCreator )
		{
			Contract.Requires( typeof( IDisposable ).IsAssignableFrom( variableType ) );

			/*
			 *	var u = right();
			 *	try
			 *	{
			 *		body(u);
			 *	}
			 *	finally
			 *	{
			 *		if( u != null )
			 *		{
			 *			u.Dispose();
			 *		}
			 *	}
			 */

			var usingVariable = Expression.Variable( variableType, "usingVariable" );
			var variables = new[] { usingVariable };
			var assignment = Expression.Assign( usingVariable, right );
			var body =
				Expression.TryFinally(
						bodyCreator( usingVariable ),
						Expression.IfThen(
							Expression.ReferenceNotEqual( usingVariable, Expression.Constant( null ) ),
							Expression.Call( usingVariable, Metadata._IDisposable.Dispose )
						)
					);
			return expressionType == null ? Expression.Block( variables, assignment, body ) : Expression.Block( expressionType, variables, assignment, body );
		}

		/// <summary>
		///		Creates an <see cref="Expression"/> which represents index based for block.
		/// </summary>
		/// <param name="count">The count which limits iteration.</param>
		/// <param name="bodyCreator">The body creator. The argument is <c>i</c> index variable of for loop.</param>
		/// <returns>An <see cref="Expression"/> which represents index based for block.</returns>
		public static Expression For( Expression count, Func<Expression, Expression> bodyCreator )
		{
			/*
			 *	int i = 0;
			 *	while( true )
			 *	{
			 *		if( i == count )
			 *		{
			 *			break;
			 *		}
			 *		
			 *		body(i);
			 *		i++;
			 *	}
			 */
			var indexVariable = Expression.Variable( typeof( int ), "i" );
			var endFor = Expression.Label( "END_FOR" );
			return
				Expression.Block(
					new[] { indexVariable },
					Expression.Assign( indexVariable, Expression.Constant( 0 ) ),
					Expression.Loop(
						Expression.Block(
							Expression.IfThen(
								Expression.Equal( indexVariable, count ),
								Expression.Break( endFor )
							),
							bodyCreator( indexVariable ),
							Expression.PostIncrementAssign( indexVariable )
						),
						endFor
					)
				);
		}

		/// <summary>
		///		Creates an <see cref="Expression"/> which represents foreach block.
		/// </summary>
		/// <param name="collection">The collection to be enumerated.</param>
		/// <param name="traits">The traits of the collection.</param>
		/// <param name="bodyCreator">The body creator. The argument is <c>Current</c> property of the enumerator.</param>
		/// <returns>An <see cref="Expression"/> which represents foreach block.</returns>
		public static Expression ForEach( Expression collection, CollectionTraits traits, Func<Expression, Expression> bodyCreator )
		{
			/*
			 *	var enumerator = collection.GetEnumerator();
			 *	try
			 *	{
			 *		while(true)
			 *		{
			 *			if ( enumerator.MoveNext() )
			 *			{
			 *				body( enumerator.Current );
			 *			}
			 *			else
			 *			{
			 *				break;
			 *			}
			 *		}
			 *	}
			 *	finally
			 *	{
			 *		var asDisposable = enumerator as IDisposable;
			 *		if( asDisposable != null )
			 *		{
			 *			asDisposable.Dispose();
			 *		}
			 *	}
			 */

#if !MONO
			var enumeratorVariable = Expression.Variable( traits.GetEnumeratorMethod.ReturnType, "enumerator" );
			var enumeratorAssignment = Expression.Assign( enumeratorVariable, Expression.Call( collection, traits.GetEnumeratorMethod ) );
			var currentProperty =
				traits.GetEnumeratorMethod.ReturnType == typeof( IDictionaryEnumerator )
				? traits.GetEnumeratorMethod.ReturnType.GetProperty( "Entry" )
				: traits.GetEnumeratorMethod.ReturnType.GetProperty( "Current" ) ?? Metadata._IEnumerator.Current;
#else
			ParameterExpression enumeratorVariable;
			BinaryExpression enumeratorAssignment;
			PropertyInfo currentProperty;
			if( !traits.GetEnumeratorMethod.ReturnType.IsValueType )
			{
				currentProperty =
					traits.GetEnumeratorMethod.ReturnType == typeof( IDictionaryEnumerator )
					? traits.GetEnumeratorMethod.ReturnType.GetProperty( "Entry" )
					: traits.GetEnumeratorMethod.ReturnType.GetProperty( "Current" ) ?? Metadata._IEnumerator.Current;

				enumeratorVariable = Expression.Variable( traits.GetEnumeratorMethod.ReturnType, "enumerator" );
				enumeratorAssignment = Expression.Assign( enumeratorVariable, Expression.Call( collection, traits.GetEnumeratorMethod ) );
			}
			else if( traits.GetEnumeratorMethod.ReturnType == typeof(IDictionaryEnumerator) )
			{
				currentProperty = traits.GetEnumeratorMethod.ReturnType.GetProperty( "Entry" );
				enumeratorVariable = Expression.Variable( traits.GetEnumeratorMethod.ReturnType, "enumerator" );
				enumeratorAssignment = Expression.Assign( enumeratorVariable, Expression.Call( collection, traits.GetEnumeratorMethod ) );
			}
			else
			{
				// Mono cannot not handle value type local variable with state in expression tree correctly, so value type Enumerator causes infinite loop.
				var currentPropertyType = traits.GetEnumeratorMethod.ReturnType.GetProperty( "Current" ).PropertyType;
				var enumeratorType =
					traits.GetEnumeratorMethod.ReturnType.GetInterfaces()
					.FirstOrDefault( i => 
						i.GetIsGenericType() 
						&& i.GetGenericTypeDefinition() == typeof( IEnumerator<> )  
						&& i.GetGenericArguments()[ 0 ] == currentPropertyType 
					) ?? typeof( IEnumerator );
				currentProperty = enumeratorType.GetProperty( "Current" );
				enumeratorVariable = Expression.Variable( enumeratorType, "enumerator" );
				enumeratorAssignment = 
					Expression.Assign( 
						enumeratorVariable, 
						Expression.Convert( 
							Expression.Call( collection, traits.GetEnumeratorMethod ), 
						enumeratorType
					)
				);
			}
#endif
			var tryBlock = CreateForEachTry( enumeratorVariable, traits, currentProperty, bodyCreator );
			var finallyBlock = CreateForEachFinally( enumeratorVariable, traits );
			return
				Expression.Block(
					new[] { enumeratorVariable },
					enumeratorAssignment,
					finallyBlock == null
					? tryBlock
					: Expression.TryFinally(
						tryBlock,
						enumeratorVariable.Type.GetIsValueType()
						? finallyBlock
						: Expression.IfThen(
							Expression.ReferenceNotEqual( enumeratorVariable, Expression.Constant( null ) ),
							finallyBlock
						)
					)
				);
		}

		private static Expression CreateForEachTry( ParameterExpression enumeratorVariable, CollectionTraits traits, PropertyInfo currentProperty, Func<Expression, Expression> bodyCreator )
		{
			var endLoop = Expression.Label( "END_FOREACH" );

			return
				Expression.Loop(
					Expression.IfThenElse(
						Expression.Call( enumeratorVariable, Metadata._IEnumerator.MoveNext ),
						bodyCreator( Expression.Property( enumeratorVariable, currentProperty ) ),
						Expression.Break( endLoop )
					),
					endLoop
				);
		}

		private static Expression CreateForEachFinally( Expression enumeratorVariable, CollectionTraits traits )
		{
			if ( !typeof( IDisposable ).IsAssignableFrom( traits.GetEnumeratorMethod.ReturnType ) )
			{
				return null;
			}

			if ( traits.GetEnumeratorMethod.ReturnType.GetMethod( "Dispose", ReflectionAbstractions.EmptyTypes ) != null )
			{
				return Expression.Call( enumeratorVariable, Metadata._IDisposable.Dispose );
			}
			else
			{
				return Expression.Call( Expression.TypeAs( enumeratorVariable, typeof( IDisposable ) ), Metadata._IDisposable.Dispose );
			}
		}

		private static readonly Type[] _containsCapacity = new[] { typeof( int ) };

		/// <summary>
		///		Returns an appropriate <see cref="ConstructorInfo"/> of collection.
		/// </summary>
		/// <param name="context">The serialization context which holds default collection type.</param>
		/// <param name="type">The type of the collection.</param>
		/// <returns>An appropriate <see cref="ConstructorInfo"/> of collection.</returns>
		/// <remarks>
		///		If the collection has <c>.ctor(int capacity)</c>, then it will be returned.
		///		Otherwise, default constructor will be returned.
		///		Note that this method cannot determine whether a single <see cref="Int32"/> parameter truely represents 'capacity' or not.
		/// </remarks>
		public static ConstructorInfo GetCollectionConstructor( SerializationContext context, Type type )
		{
			return type.GetConstructor( _containsCapacity ) ?? type.GetConstructor( ReflectionAbstractions.EmptyTypes );
		}

		/// <summary>
		///		Creates an <see cref="Expression"/> which unpack an item.
		/// </summary>
		/// <param name="unpackerParameter">The parameter <see cref="Expression"/> which holds the unpacker.</param>
		/// <param name="unpackFrom">The target <c>UnpackFrom</c> method of the serializer for item type.</param>
		/// <param name="serializerParameter">The parameter <see cref="Expression"/> which has <c>UnpackFrom </c>will be invoked.</param>
		/// <param name="serializerType">The type of the item serializer.</param>
		/// <returns>An <see cref="Expression"/> which unpack an item.</returns>
		public static Expression CreateUnpackItem( Expression unpackerParameter, MethodInfo unpackFrom, Expression serializerParameter, Type serializerType )
		{
			return
				Expression.Condition(
					Expression.AndAlso(
						Expression.IsFalse(
							Expression.Property( unpackerParameter, Metadata._Unpacker.IsArrayHeader )
						),
						Expression.IsFalse(
							Expression.Property( unpackerParameter, Metadata._Unpacker.IsMapHeader )
						)
					),
					unpackFrom.IsStatic
					? Expression.Call(
						null,
						unpackFrom,
						Expression.TypeAs( serializerParameter, serializerType ),
						unpackerParameter
					) : Expression.Call(
						Expression.TypeAs( serializerParameter, serializerType ),
						unpackFrom,
						unpackerParameter
					),
					ExpressionSerializerLogics.Using(
						typeof( Unpacker ),
						unpackFrom.ReturnType,
						Expression.Call(
							unpackerParameter,
							Metadata._Unpacker.ReadSubtree
						),
						usingVariable =>
							unpackFrom.IsStatic
							? Expression.Call(
								null,
								unpackFrom,
								Expression.TypeAs( serializerParameter, serializerType ),
								usingVariable
							) : Expression.Call(
								Expression.TypeAs( serializerParameter, serializerType ),
								unpackFrom,
								usingVariable
							)
					)
				);
		}
	}
}
