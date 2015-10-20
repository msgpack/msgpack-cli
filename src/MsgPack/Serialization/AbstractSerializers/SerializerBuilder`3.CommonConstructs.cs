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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct, TObject>
	{
		private static readonly TConstruct[] NoConstructs = new TConstruct[ 0 ];

		///  <summary>
		/// 	Emits the method prologue of general serializer.
		///  </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="method">The kind of implementing general serializer method.</param>
		protected abstract void EmitMethodPrologue( TContext context, SerializerMethod method );

		///  <summary>
		/// 	Emits the method prologue of enum serializer.
		///  </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="method">The kind of implementing enum serializer method.</param>
		protected abstract void EmitMethodPrologue( TContext context, EnumSerializerMethod method );

		///  <summary>
		/// 	Emits the method prologue of general serializer.
		///  </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="method">The kind of implementing general serializer method.</param>
		/// <param name="declaration">The method declaration to be overridden.</param>
		protected abstract void EmitMethodPrologue( TContext context, CollectionSerializerMethod method, MethodInfo declaration );

		///  <summary>
		/// 	Emits the method epiloigue of general serializer.
		///  </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="method">The kind of implementing general serializer method.</param>
		/// <param name="construct">The construct which represent method statements in order. Null entry should be ignored.</param>
		protected abstract void EmitMethodEpilogue( TContext context, SerializerMethod method, TConstruct construct );

		///  <summary>
		/// 	Emits the method epiloigue of enum serializer.
		///  </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="method">The kind of implementing enum serializer method.</param>
		/// <param name="construct">The construct which represent method statements in order. Null entry should be ignored.</param>
		protected abstract void EmitMethodEpilogue( TContext context, EnumSerializerMethod method, TConstruct construct );

		///  <summary>
		/// 	Emits the method epiloigue of enum serializer.
		///  </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="method">The kind of implementing general serializer method.</param>
		/// <param name="construct">The construct which represent method statements in order. Null entry should be ignored.</param>
		protected abstract void EmitMethodEpilogue( TContext context, CollectionSerializerMethod method, TConstruct construct );

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Well patterned" )]
		private TConstruct MakeDefaultParameterValueLiteral( TContext context, TConstruct targetVariable, Type literalType, object literal, bool hasDefault )
		{
			// Supports only C# literals
			if ( literalType == typeof( byte ) )
			{
				return this.MakeByteLiteral( context, !hasDefault ? default( byte ) : ( byte )literal );
			}

			if ( literalType == typeof( sbyte ) )
			{
				return this.MakeSByteLiteral( context, !hasDefault ? default( sbyte ) : ( sbyte )literal );
			}

			if ( literalType == typeof( short ) )
			{
				return this.MakeInt16Literal( context, !hasDefault ? default( short ) : ( short )literal );
			}

			if ( literalType == typeof( ushort ) )
			{
				return this.MakeUInt16Literal( context, !hasDefault ? default( ushort ) : ( ushort )literal );
			}

			if ( literalType == typeof( int ) )
			{
				return this.MakeInt32Literal( context, !hasDefault ? default( int ) : ( int )literal );
			}

			if ( literalType == typeof( uint ) )
			{
				return this.MakeUInt32Literal( context, !hasDefault ? default( uint ) : ( uint )literal );
			}

			if ( literalType == typeof( long ) )
			{
				return this.MakeInt64Literal( context, !hasDefault ? default( long ) : ( long )literal );
			}

			if ( literalType == typeof( ulong ) )
			{
				return this.MakeUInt64Literal( context, !hasDefault ? default( ulong ) : ( ulong )literal );
			}

			if ( literalType == typeof( float ) )
			{
				return this.MakeReal32Literal( context, !hasDefault ? default( float ) : ( float )literal );
			}

			if ( literalType == typeof( double ) )
			{
				return this.MakeReal64Literal( context, !hasDefault ? default( double ) : ( double )literal );
			}

			if ( literalType == typeof( decimal ) )
			{
				return this.MakeDecimalLiteral( context, targetVariable, !hasDefault ? default( decimal ) : ( decimal )literal );
			}

			if ( literalType == typeof( bool ) )
			{
				return this.MakeBooleanLiteral( context, hasDefault && ( bool )literal );
			}

			if ( literalType == typeof( char ) )
			{
				return this.MakeCharLiteral( context, !hasDefault ? default( char ) : ( char )literal );
			}

			if ( literalType.GetIsEnum() )
			{
				return this.MakeEnumLiteral( context, literalType, !hasDefault ? Enum.ToObject( literalType, 0 ) : literal );
			}

			if ( literal != null && hasDefault )
			{
				if ( literalType == typeof( string ) )
				{
					return this.MakeStringLiteral( context, literal as string );
				}

				// Unknown literal.
				if ( literalType.GetIsValueType() )
				{
					throw new NotSupportedException(
						String.Format( CultureInfo.CurrentCulture, "Literal for value type '{0}' is not supported.", literalType )
					);
				}
				else
				{
					throw new NotSupportedException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Literal for reference type '{0}' is not supported except null reference.",
							literalType
						)
					);
				}
			}
			else if ( literalType.GetIsValueType() )
			{
				return this.MakeDefaultLiteral( context, literalType );
			}
			else
			{
				return this.MakeNullLiteral( context, literalType );
			}
		}

		// for C# decimal opt parameter ... [DecimalConstant(...)]
		private TConstruct MakeDecimalLiteral( TContext context, TConstruct targetVariable, decimal constant )
		{
			var bits = Decimal.GetBits( constant );
			return
				this.EmitCreateNewObjectExpression(
					context,
					targetVariable,
					Metadata._Decimal.Constructor,
					this.MakeInt32Literal( context, bits[ 0 ] ), // lo
					this.MakeInt32Literal( context, bits[ 1 ] ), // mid
					this.MakeInt32Literal( context, bits[ 2 ] ), // high
					this.MakeBooleanLiteral( context, ( bits[ 3 ] & 0x80000000 ) != 0 ), // sign
					this.MakeByteLiteral( context, unchecked( ( byte )( bits[ 3 ] >> 16 & 0xFF ) ) ) // scale
				);
		}

		/// <summary>
		///		Emits anonymous <c>null</c> reference literal.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="contextType">The type of null reference.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeNullLiteral( TContext context, Type contextType );

		/// <summary>
		///		Emits the constant <see cref="Byte"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeByteLiteral( TContext context, byte constant );

		/// <summary>
		///		Emits the constant <see cref="SByte"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeSByteLiteral( TContext context, sbyte constant );

		/// <summary>
		///		Emits the constant <see cref="Int16"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeInt16Literal( TContext context, short constant );

		/// <summary>
		///		Emits the constant <see cref="UInt16"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeUInt16Literal( TContext context, ushort constant );

		/// <summary>
		///		Emits the constant <see cref="Int32"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeInt32Literal( TContext context, int constant );

		/// <summary>
		///		Emits the constant <see cref="UInt32"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeUInt32Literal( TContext context, uint constant );

		/// <summary>
		///		Emits the constant <see cref="Int64"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeInt64Literal( TContext context, long constant );

		/// <summary>
		///		Emits the constant <see cref="UInt64"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeUInt64Literal( TContext context, ulong constant );

		/// <summary>
		///		Emits the constant <see cref="Single"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeReal32Literal( TContext context, float constant );

		/// <summary>
		///		Emits the constant <see cref="Double"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeReal64Literal( TContext context, double constant );

		/// <summary>
		///		Emits the constant <see cref="Boolean"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeBooleanLiteral( TContext context, bool constant );

		/// <summary>
		///		Emits the constant <see cref="Char"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeCharLiteral( TContext context, char constant );

		/// <summary>
		///		Emits the constant <see cref="String"/> value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeStringLiteral( TContext context, string constant );

		/// <summary>
		///		Emits the constant enum value.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="type">The type of the enum.</param>
		/// <param name="constant">The constant value.</param>
		/// <returns>The generated construct.</returns>
		/// <exception cref="ArgumentException"><paramref name="type"/> is not enum.</exception>
		protected abstract TConstruct MakeEnumLiteral( TContext context, Type type, object constant ); // boxing is better than complex unboxing issue

		/// <summary>
		///		Emits the constant default(T) value of value type.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="type">The type of the valueType.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct MakeDefaultLiteral( TContext context, Type type );

		/// <summary>
		///		Emits the loading this reference expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitThisReferenceExpression( TContext context );

		/// <summary>
		///		Emits the box expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="valueType">Type of the value to be boxed.</param>
		/// <param name="value">The value to be boxed.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitBoxExpression( TContext context, Type valueType, TConstruct value );

		/// <summary>
		///		Emits the cast or unbox expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="targetType">Type of the value to be casted or be unboxed.</param>
		/// <param name="value">The value to be casted or be unboxed.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitUnboxAnyExpression( TContext context, Type targetType, TConstruct value );

		/// <summary>
		///		Emits the not expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="booleanExpression">The boolean expression to be .</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitNotExpression( TContext context, TConstruct booleanExpression );

		/// <summary>
		///		Emits the equals expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="left">The left expression.</param>
		/// <param name="right">The right expression.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitEqualsExpression( TContext context, TConstruct left, TConstruct right );

		/// <summary>
		///		Emits the not equals expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="left">The left expression.</param>
		/// <param name="right">The right expression.</param>
		/// <returns>The generated construct.</returns>
		protected virtual TConstruct EmitNotEqualsExpression( TContext context, TConstruct left, TConstruct right )
		{
			return this.EmitNotExpression( context, this.EmitEqualsExpression( context, left, right ) );
		}

		/// <summary>
		///		Emits the greater than expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="left">The left expression.</param>
		/// <param name="right">The right expression.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitGreaterThanExpression( TContext context, TConstruct left, TConstruct right );

		/// <summary>
		///		Emits the less than expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="left">The left expression.</param>
		/// <param name="right">The right expression.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitLessThanExpression( TContext context, TConstruct left, TConstruct right );

		/// <summary>
		///		Emits the unary increment expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="int32Value">The int32 value to be incremented.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitIncrement( TContext context, TConstruct int32Value );

		/// <summary>
		///		Emits the elementType-of expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="type">The elementType.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitTypeOfExpression( TContext context, Type type );

		/// <summary>
		///		Emits the sequential statements. Note that the context elementType is void.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="contextType">The type of context.</param>
		/// <param name="statements">The statements.</param>
		/// <returns>The generated construct.</returns>
		protected TConstruct EmitSequentialStatements( TContext context, Type contextType, params TConstruct[] statements )
		{
			return this.EmitSequentialStatements( context, contextType, statements as IEnumerable<TConstruct> );
		}

		/// <summary>
		///		Emits the sequential statements. Note that the context elementType is void.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="contextType">The type of context.</param>
		/// <param name="statements">The statements.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitSequentialStatements( TContext context, Type contextType, IEnumerable<TConstruct> statements );

		/// <summary>
		///		Creates the argument reference.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="type">The type of the parameter for debugging puropose.</param>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="index">The index of the parameters.</param>
		/// <returns>
		///		The generated construct which represents an argument reference.
		/// </returns>
		protected abstract TConstruct ReferArgument( TContext context, Type type, string name, int index );

		/// <summary>
		///		Declares the local variable.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="type">The elementType of the variable.</param>
		/// <param name="name">The name of the variable for debugging puropose.</param>
		/// <returns>
		///		The generated construct which represents local variable declaration AND initialization, and reference.
		/// </returns>
		protected abstract TConstruct DeclareLocal( TContext context, Type type, string name );

		/// <summary>
		///		Emits the create new object expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="variable">The variable which will store created value type object.</param>
		/// <param name="constructor">The constructor.</param>
		/// <param name="arguments">The arguments.</param>
		/// <returns>
		///		The generated construct which represents new obj instruction.
		///		Note that created object remains in context.
		/// </returns>
		protected abstract TConstruct EmitCreateNewObjectExpression(
			TContext context, TConstruct variable, ConstructorInfo constructor, params TConstruct[] arguments
		);

		/// <summary>
		///		Emits the invoke void method.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance for instance method invocation. <c>null</c> for static method.</param>
		/// <param name="method">The method to be invoked.</param>
		/// <param name="arguments">The arguments to be passed.</param>
		/// <returns>
		///		The generated construct.
		/// </returns>
		/// <remarks>
		///		The derived class must emits codes which discard return non-void value.
		/// </remarks>
		protected abstract TConstruct EmitInvokeVoidMethod(
			TContext context, TConstruct instance, MethodInfo method, params TConstruct[] arguments
		);

		/// <summary>
		///		Emits the invoke non-void method.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance for instance method invocation. <c>null</c> for static method.</param>
		/// <param name="method">The method to be invoked.</param>
		/// <param name="arguments">The arguments to be passed.</param>
		/// <returns>
		///		The generated construct which represents new obj instruction.
		///		Note that returned value remains in context.
		/// </returns>
		protected TConstruct EmitInvokeMethodExpression(
			TContext context, TConstruct instance, MethodInfo method, params TConstruct[] arguments
		)
		{
			return
				this.EmitInvokeMethodExpression(
					context, instance, method, ( arguments ?? new TConstruct[ 0 ] ) as IEnumerable<TConstruct>
				);
		}

		/// <summary>
		///		Emits the invoke non-void method.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance for instance method invocation. <c>null</c> for static method.</param>
		/// <param name="method">The method to be invoked.</param>
		/// <param name="arguments">The arguments to be passed.</param>
		/// <returns>
		///		The generated construct which represents new obj instruction.
		///		Note that returned value remains in context.
		/// </returns>
		protected abstract TConstruct EmitInvokeMethodExpression( TContext context, TConstruct instance, MethodInfo method, IEnumerable<TConstruct> arguments );

		/// <summary>
		///		Emits the get member(field or property) value expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="member">The member to be accessed.</param>
		/// <returns>The generated construct.</returns>
		private TConstruct EmitGetMemberValueExpression( TContext context, TConstruct instance, MemberInfo member )
		{
			FieldInfo asField;
			if ( ( asField = member as FieldInfo ) != null )
			{
				return this.EmitGetField( context, instance, asField, !asField.GetHasPublicGetter() );
			}
			else
			{
				var asProperty = member as PropertyInfo;
#if DEBUG
				Contract.Assert( asProperty != null, member.GetType().FullName );
#endif
				return this.EmitGetProperty( context, instance, asProperty, !asProperty.GetHasPublicGetter() );
			}
		}

		private TConstruct EmitGetProperty( TContext context, TConstruct instance, PropertyInfo property, bool withReflection )
		{
			if ( !withReflection )
			{
				return this.EmitGetPropertyExpression( context, instance, property );
			}

			/*
			 * return _method_of(m).Invoke( instance, null );
			 */
			return
				this.EmitUnboxAnyExpression(
					context,
					property.PropertyType,
					this.EmitInvokeMethodExpression(
						context,
						this.EmitMethodOfExpression( context, property.GetGetMethod( true ) ),
						Metadata._MethodBase.Invoke_2,
						instance,
						this.MakeNullLiteral( context, typeof( object[] ) )
					)
				);
		}

		/// <summary>
		///		Emits the get property value expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="property">The property to be accessed.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitGetPropertyExpression( TContext context, TConstruct instance, PropertyInfo property );

		private TConstruct EmitGetField( TContext context, TConstruct instance, FieldInfo field, bool withReflection )
		{
			if ( !withReflection )
			{
				return this.EmitGetFieldExpression( context, instance, field );
			}

			/*
			 * _field_of(f).GetValue( instance );
			 */
			return
				this.EmitUnboxAnyExpression(
					context,
					field.FieldType,
					this.EmitInvokeMethodExpression(
						context,
						this.EmitFieldOfExpression( context, field ),
						Metadata._FieldInfo.GetValue,
						instance
					)
				);
		}

		/// <summary>
		///		Emits the get field value expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="field">The field to be accessed.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitGetFieldExpression( TContext context, TConstruct instance, FieldInfo field );

		/// <summary>
		///		Emits the set member(property or field) value statement.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="member">The member to be accessed.</param>
		/// <param name="value">The value to be stored.</param>
		/// <returns>The generated construct.</returns>
		/// <remarks>
		///		This method generates <c>collection.Add(value)</c> constructs for a read-only member.
		/// </remarks>
		private TConstruct EmitSetMemberValueStatement( TContext context, TConstruct instance, MemberInfo member, TConstruct value )
		{
			TConstruct getCollection;
			CollectionTraits traits;
			FieldInfo asField;
			PropertyInfo asProperty = null;
			if ( ( asField = member as FieldInfo ) != null )
			{
				if ( !asField.IsInitOnly && asField.GetIsPublic() )
				{
					return this.EmitSetField( context, instance, asField, value, false );
				}

				getCollection = this.EmitGetField( context, instance, asField, !asField.GetHasPublicGetter() );
				traits = asField.FieldType.GetCollectionTraits();
			}
			else
			{
				asProperty = member as PropertyInfo;
#if DEBUG
				Contract.Assert( asProperty != null, member.GetType().FullName );
#endif
				var setter = asProperty.GetSetMethod( true );
				if ( setter != null && setter.GetIsPublic() )
				{
					return this.EmitSetProperty( context, instance, asProperty, value, false );
				}

				getCollection = this.EmitGetProperty( context, instance, asProperty, !asProperty.GetHasPublicGetter() );
				traits = asProperty.PropertyType.GetCollectionTraits();
			}

			// use Add(T) for appendable collection elementType read only member.

			switch ( traits.CollectionType )
			{
				case CollectionKind.Array:
				{
					/*
					 *	foreach ( var item in unpacked )
					 *	{
					 *		target.Prop.Add(item);
					 *	}
					 */
					return
						this.EmitStoreCollectionItemsEmitSetCollectionMemberIfNullAndSettable(
							context,
							instance,
							value,
							member.GetMemberValueType(),
							asField,
							asProperty,
							traits.AddMethod == null
								? null
								: this.EmitForEachLoop(
									context,
									traits,
									value,
									current =>
										this.EmitAppendCollectionItem(
											context,
											member,
											traits,
											getCollection,
											current
										)
								)
						);
				}
				case CollectionKind.Map:
				{
					/*
					 *	foreach ( var item in unpacked )
					 *	{
					 *		target.Prop.Add(item.Key, item.Value);
					 *	}
					 */
					Type keyType, valueType;
					GetDictionaryKeyValueType( traits.ElementType, out keyType, out valueType );
					return
						this.EmitStoreCollectionItemsEmitSetCollectionMemberIfNullAndSettable(
							context,
							instance,
							value,
							member.GetMemberValueType(),
							asField,
							asProperty,
							traits.AddMethod == null
								? null
								: this.EmitForEachLoop(
									context,
									traits,
									value,
									// ReSharper disable ImplicitlyCapturedClosure
									current =>
										this.EmitAppendDictionaryItem(
											context,
											traits,
											getCollection,
											keyType,
											this.EmitGetPropertyExpression(
												context,
												current,
#if !NETFX_CORE
												traits.ElementType == typeof( DictionaryEntry )
													? Metadata._DictionaryEntry.Key
													: traits.ElementType.GetProperty( "Key" )
#else
												traits.ElementType.GetProperty( "Key" )
#endif // !NETFX_CORE
											),
											valueType,
											this.EmitGetPropertyExpression(
												context,
												current,
#if !NETFX_CORE
												traits.ElementType == typeof( DictionaryEntry )
													? Metadata._DictionaryEntry.Value
													: traits.ElementType.GetProperty( "Value" )
#else
												traits.ElementType.GetProperty( "Value" )
#endif // !NETFX_CORE
												),
											false
										)
									// ReSharper restore ImplicitlyCapturedClosure
								)
						);
				}
			}

			// Try use reflection
			if ( asField != null )
			{
				return this.EmitSetField( context, instance, asField, value, true );
			}

			if ( asProperty.GetSetMethod( true ) != null )
			{
				return this.EmitSetProperty( context, instance, asProperty, value, true );
			}

			throw new SerializationException(
				String.Format(
					CultureInfo.CurrentCulture,
					"Member '{0}' is read only and its elementType ('{1}') is not an appendable collection",
					member.Name,
					member.GetMemberValueType()
				)
			);
		}

		private TConstruct EmitStoreCollectionItemsEmitSetCollectionMemberIfNullAndSettable(
			TContext context,
			TConstruct instance,
			TConstruct collection,
			Type collectionType,
			FieldInfo asField,
			PropertyInfo asProperty,
			TConstruct storeCollectionItems // null for not appendable like String
		)
		{
			if ( storeCollectionItems != null && ( asField != null && asField.IsInitOnly ) || ( asProperty != null && asProperty.GetSetMethod( true ) == null ) )
			{
				return storeCollectionItems;
			}

			/*
			 * #if APPENDABLE
			 *	if ( instance.MEMBER == null )
			 *  {
			 *		instance.MEMBER = collection:
			 *  }
			 *  else
			 *  {
			 *		(APPANED)
			 *  }
			 * #else
			 *  instance.MEMBER = collection:
			 */

			var invokeSetter =
				asField != null
					? this.EmitSetField( context, instance, asField, collection, !asField.GetHasPublicSetter() )
					: this.EmitSetProperty( context, instance, asProperty, collection, !asProperty.GetHasPublicSetter() );

			if ( storeCollectionItems == null )
			{
				return invokeSetter;
			}

			return
				this.EmitSequentialStatements(
					context,
					storeCollectionItems.ContextType,
					this.EmitConditionalExpression(
						context,
						this.EmitEqualsExpression(
							context,
							asField != null
								? this.EmitGetFieldExpression( context, instance, asField )
								: this.EmitGetPropertyExpression( context, instance, asProperty ),
							this.MakeNullLiteral( context, collectionType )
						),
						invokeSetter, // then
						storeCollectionItems // else
					)
				);
		}

		/// <summary>
		///		Emits the 'methodof' expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="method">The method to be retrieved.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitMethodOfExpression( TContext context, MethodBase method );

		private TConstruct EmitSetProperty(
			TContext context,
			TConstruct instance,
			PropertyInfo property,
			TConstruct value,
			bool withReflection
		)
		{
			if ( !withReflection )
			{
				return this.EmitSetProperty( context, instance, property, value );
			}

			/*
			 * _method_of(m).Invoke( instance, new object[]{ value } );
			 */
			return
				this.EmitInvokeVoidMethod(
					context,
					this.EmitMethodOfExpression( context, property.GetSetMethod( true ) ),
					Metadata._MethodBase.Invoke_2,
					instance,
					this.EmitCreateNewArrayExpression(
						context,
						typeof( object ),
						1,
						new[]
						{
							value.ContextType.GetIsValueType()
							? this.EmitBoxExpression( context, value.ContextType, value )
							: value
						}
					)
				);
		}

		/// <summary>
		///		Emits the set property value statement.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="property">The property to be accessed.</param>
		/// <param name="value">The value to be stored.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitSetProperty( TContext context, TConstruct instance, PropertyInfo property, TConstruct value );

		/// <summary>
		///		Emits the 'fieldof' expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="field">The field to be retrieved.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitFieldOfExpression( TContext context, FieldInfo field );

		private TConstruct EmitSetField(
			TContext context,
			TConstruct instance,
			FieldInfo field,
			TConstruct value,
			bool withReflection )
		{
			if ( !withReflection )
			{
				return this.EmitSetField( context, instance, field, value );
			}

			/*
			 * _field_of(f).SetValue( instance, value );
			 */
			return
				this.EmitInvokeVoidMethod(
					context,
					this.EmitFieldOfExpression( context, field ),
					Metadata._FieldInfo.SetValue,
					instance,
					value.ContextType.GetIsValueType()
					? this.EmitBoxExpression( context, value.ContextType, value )
					: value
				);
		}

		/// <summary>
		///		Emits the set field value statement.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="instance">The instance which stores instance member value.</param>
		/// <param name="field">The field to be accessed.</param>
		/// <param name="value">The value to be stored.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitSetField( TContext context, TConstruct instance, FieldInfo field, TConstruct value );

		/// <summary>
		///		Emits the statement which loads value from the local variable.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="variable">The variable to be loaded.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitLoadVariableExpression( TContext context, TConstruct variable );

		/// <summary>
		///		Emits the statement which stores context value to the local variable.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="variable">The variable to be stored.</param>
		/// <returns>The generated construct.</returns>
		protected virtual TConstruct EmitStoreVariableStatement( TContext context, TConstruct variable )
		{
			return this.EmitStoreVariableStatement( context, variable, null );
		}

		/// <summary>
		///		Emits the statement which stores specified value to the local variable.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="variable">The variable to be stored.</param>
		/// <param name="value">The value to be stored. <c>null</c> for context value.</param>
		/// <returns>The generated construct.</returns>
		protected abstract TConstruct EmitStoreVariableStatement( TContext context, TConstruct variable, TConstruct value );

		/// <summary>
		///		Emits the throwing exception statement.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="expressionType">Type of the entire expression elementType.</param>
		/// <param name="factoryMethod">The factory method which creates exception object with <paramref name="arguments"/>.</param>
		/// <param name="arguments">The arguments of <paramref name="factoryMethod"/>.</param>
		/// <returns>The generated construct.</returns>
		/// <remarks>
		///		The expression elementType is required to acomplish condition expression.
		/// </remarks>
		private TConstruct EmitThrowExpression( TContext context, Type expressionType, MethodInfo factoryMethod, params TConstruct[] arguments )
		{
			return this.EmitThrowExpression( context, expressionType, this.EmitInvokeMethodExpression( context, null, factoryMethod, arguments ) );
		}

		/// <summary>
		///		Emits the throwing exception statement.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="expressionType">Type of the entire expression elementType.</param>
		/// <param name="exceptionExpression">The expression of throwing exception.</param>
		/// <returns>The generated construct.</returns>
		/// <remarks>
		///		The expression elementType is required to acomplish condition expression.
		/// </remarks>
		protected abstract TConstruct EmitThrowExpression( TContext context, Type expressionType, TConstruct exceptionExpression );

		/// <summary>
		///		Emits the try-finally expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="tryStatement">The try expression.</param>
		/// <param name="finallyStatement">The finally statement.</param>
		/// <returns>The generated construct which elementType is elementType of <paramref name="tryStatement"/>.</returns>
		protected abstract TConstruct EmitTryFinally(
			TContext context, TConstruct tryStatement, TConstruct finallyStatement
		);

		/// <summary>
		///		Emits the invariant <see cref="string.Format(IFormatProvider,string,object[])"/> with <see cref="CultureInfo.InvariantCulture"/>.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="format">The format string literal.</param>
		/// <param name="arguments">The arguments to be used.</param>
		/// <returns>The generated construct.</returns>
		private TConstruct EmitInvariantStringFormat( TContext context, string format, params TConstruct[] arguments )
		{
			return
				this.EmitInvokeMethodExpression(
					context,
					null,
					Metadata._String.Format_P,
					this.EmitGetPropertyExpression( context, null, Metadata._CultureInfo.InvariantCulture ),
					this.MakeStringLiteral( context, format ),
					this.EmitCreateNewArrayExpression(
						context,
						typeof( object ),
						arguments.Length,
						arguments.Select( a => a.ContextType.GetIsValueType() ? this.EmitBoxExpression( context, a.ContextType, a ) : a )
					)
				);
		}

		/// <summary>
		///		Emits the create new array expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="elementType">The elementType of the array element.</param>
		/// <param name="length">The length of the array.</param>
		/// <returns>The generated code construct.</returns>
		protected abstract TConstruct EmitCreateNewArrayExpression(
			TContext context, Type elementType, int length
		);

		/// <summary>
		///		Emits the create new array expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="elementType">The elementType of the array element.</param>
		/// <param name="length">The length of the array.</param>
		/// <param name="initialElements">The initial elements of the array.</param>
		/// <returns>The generated code construct.</returns>
		protected abstract TConstruct EmitCreateNewArrayExpression(
			TContext context, Type elementType, int length, IEnumerable<TConstruct> initialElements
		);

		///  <summary>
		/// 		Emits the set array element expression.
		///  </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="array">The array to be set.</param>
		/// <param name="index">The index of the array element to be set.</param>
		/// <param name="value">The value to be set.</param>
		/// <returns>The generated code construct.</returns>
		protected abstract TConstruct EmitSetArrayElementStatement( TContext context, TConstruct array, TConstruct index, TConstruct value );

		/// <summary>
		///		Emits the get serializer expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="targetType">Type of the target of the serializer.</param>
		/// <param name="memberInfo">The metadata of the packing/unpacking member.</param>
		/// <param name="itemsSchema">The schema for collection items. <c>null</c> for non-collection items and non-schema items.</param>
		/// <returns>The generated code construct.</returns>
		/// <remarks>
		///		The serializer reference methodology is implication specific.
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected virtual TConstruct EmitGetSerializerExpression(
			TContext context,
			Type targetType,
			SerializingMember? memberInfo,
			PolymorphismSchema itemsSchema
		)
		{
			if ( memberInfo != null && targetType.GetIsEnum() )
			{
				return
					this.EmitInvokeMethodExpression(
						context,
						context.Context,
						Metadata._SerializationContext.GetSerializer1_Parameter_Method.MakeGenericMethod( targetType ),
						this.EmitBoxExpression(
							context,
							typeof( EnumSerializationMethod ),
							this.EmitInvokeMethodExpression(
								context,
								null,
								Metadata._EnumMessagePackSerializerHelpers.DetermineEnumSerializationMethodMethod,
								context.Context,
								this.EmitTypeOfExpression( context, targetType ),
								this.MakeEnumLiteral(
									context,
									typeof( EnumMemberSerializationMethod ),
									memberInfo.Value.GetEnumMemberSerializationMethod()
								)
							)
						)
					);
			}
			else if ( memberInfo != null && DateTimeMessagePackSerializerHelpers.IsDateTime( targetType ) )
			{
				return
					this.EmitInvokeMethodExpression(
						context,
						context.Context,
						Metadata._SerializationContext.GetSerializer1_Parameter_Method.MakeGenericMethod( targetType ),
						this.EmitBoxExpression(
							context,
							typeof( DateTimeConversionMethod ),
							this.EmitInvokeMethodExpression(
								context,
								null,
								Metadata._DateTimeMessagePackSerializerHelpers.DetermineDateTimeConversionMethodMethod,
								context.Context,
								this.MakeEnumLiteral(
									context,
									typeof( DateTimeMemberConversionMethod ),
									memberInfo.Value.GetDateTimeMemberConversionMethod()
								)
							)
						)
					);
			}
			else
			{
				// Check by try to get serializer now.
				var schemaForMember = itemsSchema ??
									( memberInfo != null
										? PolymorphismSchema.Create( targetType, memberInfo )
										: PolymorphismSchema.Default );
				context.SerializationContext.GetSerializer( targetType, schemaForMember );

				var schema = this.DeclareLocal( context, typeof( PolymorphismSchema ), "__schema" );
				return
					this.EmitSequentialStatements(
						context,
						typeof( MessagePackSerializer<> ).MakeGenericType( targetType ),
						new[] { schema }
						.Concat(
							this.EmitConstructPolymorphismSchema(
								context,
								schema,
								schemaForMember
							)
						).Concat(
							new[]
							{
								this.EmitInvokeMethodExpression(
									context,
									context.Context,
									Metadata._SerializationContext.GetSerializer1_Parameter_Method.MakeGenericMethod( targetType ),
									schema
								)
							}
						)
					);
			}
		}

		/// <summary>
		/// Emits the pack item statements.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="packer">The packer.</param>
		/// <param name="itemType">Type of the item.</param>
		/// <param name="nilImplication">The nil implication of the member.</param>
		/// <param name="memberName">Name of the member.</param>
		/// <param name="item">The item to be packed.</param>
		/// <param name="memberInfo">The metadata of packing member. <c>null</c> for non-object member (collection or tuple items).</param>
		/// <param name="itemsSchema">The schema for collection items. <c>null</c> for non-collection items and non-schema items.</param>
		/// <returns>The generated code construct.</returns>
		private IEnumerable<TConstruct> EmitPackItemStatements(
			TContext context,
			TConstruct packer,
			Type itemType,
			NilImplication nilImplication,
			string memberName, TConstruct item,
			SerializingMember? memberInfo,
			PolymorphismSchema itemsSchema
		)
		{
			var nilImplicationConstruct =
				this._nilImplicationHandler.OnPacking(
					new SerializerBuilderOnPackingParameter( this, context, item, itemType, memberName ),
					nilImplication
				);
			if ( nilImplicationConstruct != null )
			{
				yield return nilImplicationConstruct;
			}

			/*
			 * this._serializerN.PackTo(packer, item);
			 */
			yield return this.EmitSerializeItemExpressionCore( context, packer, itemType, item, memberInfo, itemsSchema );
		}

		/// <summary>
		/// Emits the pack item expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="packer">The packer.</param>
		/// <param name="itemType">Type of the item.</param>
		/// <param name="item">The item to be packed.</param>
		/// <param name="memberInfo">The metadata of packing member. <c>null</c> for non-object member (collection or tuple items).</param>
		/// <param name="itemsSchema">The schema for collection items. <c>null</c> for non-collection items and non-schema items.</param>
		/// <returns>The generated code construct.</returns>
		private TConstruct EmitSerializeItemExpressionCore( TContext context, TConstruct packer, Type itemType, TConstruct item, SerializingMember? memberInfo, PolymorphismSchema itemsSchema )
		{
			return
				this.EmitInvokeVoidMethod(
					context,
					this.EmitGetSerializerExpression( context, itemType, memberInfo, itemsSchema ),
					typeof( MessagePackSerializer<> )
						.MakeGenericType( itemType )
						.GetMethods()
						.Single(
							m =>
								m.Name == "PackTo"
								&& !m.IsStatic
								&& m.IsPublic
						),
					packer,
					item
				);
		}


		/// <summary>
		///		Emits the get items count expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="unpacker">The target unpacker.</param>
		/// <returns>The generated code construct.</returns>
		private TConstruct EmitGetItemsCountExpression( TContext context, TConstruct unpacker )
		{
			return
				this.EmitInvokeMethodExpression(
					context,
					null,
					Metadata._UnpackHelpers.GetItemsCount,
					unpacker
				);
		}

		/// <summary>
		///		Emits the UnpackFrom body for collection which delegates UnpackTo.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="ctor">The constructor of the newly creating collection.</param>
		/// <param name="collectionCapacity">The newly creating collection capacity.</param>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="collection">The unpacking collection.</param>
		/// <returns>The generated code construct.</returns>
		private TConstruct EmitUnpackCollectionWithUnpackToExpression(
			TContext context, ConstructorInfo ctor, TConstruct collectionCapacity, TConstruct unpacker, TConstruct collection
		)
		{
			/*
			 *	TCollection collection = new TCollection( capacity );
			 *	this.UnpackToCore( unpacker, collection );
			 *	return collection;
			 */
			return
				this.EmitSequentialStatements(
					context,
					typeof( void ),
					this.EmitStoreVariableStatement(
						context,
						collection,
						this.EmitCreateNewObjectExpression(
							context,
							collection,
							ctor,
							ctor.GetParameters().Length == 0
							? NoConstructs
							: new[] { collectionCapacity }
						)
					),
					this.EmitInvokeUnpackTo( context, unpacker, collection )
				);
		}

		protected virtual TConstruct EmitInvokeUnpackTo( TContext context, TConstruct unpacker, TConstruct collection )
		{
			return
				this.EmitInvokeVoidMethod(
					context,
					this.EmitThisReferenceExpression( context ),
					MessagePackSerializer<TObject>.UnpackToCoreMethod,
					unpacker,
					collection
				);
		}

		/// <summary>
		///		Emits the unpack item value expression.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="itemType">Type of the item.</param>
		/// <param name="nilImplication">The nil implication.</param>
		/// <param name="unpacker">The reference to the unpacker.</param>
		/// <param name="itemIndex">Index of the item.</param>
		/// <param name="memberName">Name of the member.</param>
		/// <param name="itemsCount">
		///		The reference to items count init-only local variable which remember unpackable items count.
		///		This value will be <c>null</c> for tuples and collections.
		/// </param>
		/// <param name="unpacked">
		///		The reference to unpacked items count local variable which remember unpacked items count.
		///		This value will be <c>null</c> for tuples and collections.
		/// </param>
		/// <param name="memberInfo">The metadata of unpacking member.</param>
		/// <param name="itemsSchema">The schema for collection items. <c>null</c> for non-collection items and non-schema items.</param>
		/// <param name="storeValueStatementEmitter">
		///		The delegate which generates statement for storing unpacked value.
		///		1st parameter is unpacked value, and return value is generated statement.
		/// </param>
		/// <returns>The sequential statement.</returns>
		protected TConstruct EmitUnpackItemValueExpression(
			TContext context,
			Type itemType,
			NilImplication nilImplication,
			TConstruct unpacker,
			TConstruct itemIndex,
			TConstruct memberName,
			TConstruct itemsCount,
			TConstruct unpacked,
			SerializingMember? memberInfo,
			PolymorphismSchema itemsSchema,
			Func<TConstruct, TConstruct> storeValueStatementEmitter
		)
		{
			return
				this.EmitSequentialStatements(
					context,
					typeof( void ),
					this.EmitUnpackItemValueExpressionCore(
						context, itemType, nilImplication, unpacker, itemIndex, memberName, itemsCount, unpacked, memberInfo, itemsSchema, storeValueStatementEmitter
					)
				);
		}

		/// <summary>
		///		Emits the unpack item value expression.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="itemType">Type of the item.</param>
		/// <param name="nilImplication">The nil implication.</param>
		/// <param name="unpacker">The reference to the unpacker.</param>
		/// <param name="itemIndex">Index of the item.</param>
		/// <param name="memberName">Name of the member.</param>
		/// <param name="itemsCount">
		///		The reference to items count init-only local variable which remember unpackable items count.
		///		This value will be <c>null</c> for tuples and collections.
		/// </param>
		/// <param name="unpacked">
		///		The reference to unpacked items count local variable which remember unpacked items count.
		///		This value will be <c>null</c> for tuples and collections.
		/// </param>
		/// <param name="memberInfo">The metadata of unpacking member.</param>
		/// <param name="itemsSchema">The schema for collection items. <c>null</c> for non-collection items and non-schema items.</param>
		/// <param name="storeValueStatementEmitter">
		///		The delegate which generates statement for storing unpacked value.
		///		1st parameter is unpacked value, and return value is generated statement.
		/// </param>
		/// <returns>The elements of sequential statement.</returns>
		private IEnumerable<TConstruct> EmitUnpackItemValueExpressionCore(
			TContext context,
			Type itemType,
			NilImplication nilImplication,
			TConstruct unpacker,
			TConstruct itemIndex,
			TConstruct memberName,
			TConstruct itemsCount,
			TConstruct unpacked,
			SerializingMember? memberInfo,
			PolymorphismSchema itemsSchema,
			Func<TConstruct, TConstruct> storeValueStatementEmitter
		)
		{
			/*
			 *	T? nullable;
			 *	if ( unpacked < itemsCount )
			 *	{
			 *		if ( !unpacker.Read() )
			 *		{
			 *			throw SerializationExceptiuons.MissingItem(...);
			 *		}
			 *	
			 *		if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
			 * 		{
			 * 			nullable = serializer.UnpackFrom( unpacker );
			 * 		}
			 * 		else
			 * 		{
			 * 			using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
			 * 			{
			 * 				nullable = serializer.UnpackFrom( subtreeUnpacker );
			 * 			}
			 * 		}
			 * 	}
			 *	else
			 *	{
			 *		nullable = null;
			 *	}
			 * 
			 *  if ( nullable == null )
			 *  {
			 *  #if MEMBER_DEFAULT
			 *      // nop
			 *  #elif PROHIBIT
			 *		throw SerializationExceptiuons.NullIsProhibited(...);
			 *  #elif VALUE_TYPE
			 *		throw SerializationExceptiuons.ValueTypeCannotbeNull(...);
			 *  #else
			 *		SET_VALUE(item);
			 *  #endif
			 *  }
			 *  else
			 *  {
			 *		SET_VALUE(item);
			 *  }
			 *  
			 *	#if MEMBER_UNPACKING
			 *	unpacked++;
			 *	#endif
			 *  
			 *  context unpacker;
			 */

			// is nilable natually?
			var isNativelyNullable =
				itemType == typeof( MessagePackObject )
				|| !itemType.GetIsValueType()
				|| Nullable.GetUnderlyingType( itemType ) != null;

			// type of temp var for nil implication
			var nullableType = itemType;
			if ( !isNativelyNullable )
			{
				nullableType = typeof( Nullable<> ).MakeGenericType( itemType );
			}

			// temp var for nil implication
			var nullable =
				this.DeclareLocal(
					context,
					nullableType,
					"nullable"
				);

			// try direct unpack
			var directRead =
				Metadata._UnpackHelpers.GetDirectUnpackMethod( nullableType );

			var isNotInCollectionCondition =
				new[]
				{
					this.EmitNotExpression(
						context,
						this.EmitGetPropertyExpression( context, unpacker, Metadata._Unpacker.IsArrayHeader )
					),
					this.EmitNotExpression(
						context,
						this.EmitGetPropertyExpression( context, unpacker, Metadata._Unpacker.IsMapHeader )
					)
				};

			// unpacking item instruction.
			// compose read inst. and unpack inst. now.
			var readAndUnpack =
				directRead != null
				? this.EmitStoreVariableStatement(
					context,
					nullable,
					this.EmitInvokeMethodExpression(
						context,
						null,
						directRead,
						unpacker,
						this.EmitTypeOfExpression( context, typeof( TObject ) ),
						memberName
					)
				) : this.EmitSequentialStatements(
					context,
					typeof( void ),
					this.EmitConditionalExpression(
						context,
						this.EmitNotExpression(
							context,
							this.EmitInvokeMethodExpression( context, unpacker, Metadata._Unpacker.Read )
						),
						this.EmitThrowExpression(
							context, typeof( Unpacker ), SerializationExceptions.NewMissingItemMethod, itemIndex
						),
						null
					),
					itemType == typeof( MessagePackObject )
					? this.EmitAndConditionalExpression(
						context,
						isNotInCollectionCondition,
						this.EmitStoreVariableStatement(
							context,
							nullable,
							this.EmitGetPropertyExpression(
								context,
								unpacker,
								Metadata._Unpacker.LastReadData
							)
						),
						this.EmitStoreVariableStatement(
							context,
							nullable,
							this.EmitInvokeMethodExpression(
								context,
								unpacker,
								Metadata._Unpacker.UnpackSubtreeData
							)
						)
					) : this.EmitAndConditionalExpression(
						context,
						isNotInCollectionCondition,
						this.EmitDeserializeItemExpression( context, unpacker, nullableType, nullable, memberInfo, itemsSchema ),
						this.EmitUsingStatement(
							context,
							typeof( Unpacker ),
							this.EmitInvokeMethodExpression(
								context,
								unpacker,
								Metadata._Unpacker.ReadSubtree
							),
							subtreeUnpacker =>
								this.EmitDeserializeItemExpression( context, subtreeUnpacker, nullableType, nullable, memberInfo, itemsSchema )
						)
					)
				);

			// Unpacked item after nil implication.
			var unpackedItem =
				( Nullable.GetUnderlyingType( nullable.ContextType ) != null
				  && Nullable.GetUnderlyingType( itemType ) == null )
					? this.EmitGetPropertyExpression( context, nullable, nullable.ContextType.GetProperty( "Value" ) )
					: nullable;
			var store =
				storeValueStatementEmitter( unpackedItem );

			// Nil Implication
			TConstruct expressionWhenNil =
				this._nilImplicationHandler.OnUnpacked(
					new SerializerBuilderOnUnpacedParameter( this, context, itemType, memberName, store ),
					nilImplication
				);

			// actually declare local now.
			yield return nullable;

			// Missing member is treated as nil
			if ( unpacked != null )
			{
#if DEBUG
				Contract.Assert( itemsCount != null );
#endif
				yield return
					this.EmitConditionalExpression(
						context,
						this.EmitLessThanExpression(
							context,
							unpacked,
							itemsCount
						),
						readAndUnpack,
						null // if-then, no else -- nil is set to temp var.
					);
			}
			else
			{
				yield return readAndUnpack;
			}

			// compose nil implication and setting real var.
			yield return
				this.EmitConditionalExpression(
					context,
					( !isNativelyNullable || Nullable.GetUnderlyingType( itemType ) != null )
						? this.EmitGetPropertyExpression( context, nullable, nullableType.GetProperty( "HasValue" ) )
						: itemType == typeof( MessagePackObject )
						? this.EmitNotExpression(
							context,
							this.EmitGetPropertyExpression( context, nullable, Metadata._MessagePackObject.IsNil )
						) : this.EmitNotEqualsExpression( context, nullable, this.MakeNullLiteral( context, itemType ) ),
					store,
					expressionWhenNil
				);

			if ( unpacked != null )
			{
#if DEBUG
				Contract.Assert( itemsCount != null );
#endif
				yield return
					this.EmitIncrement(
						context,
						unpacked
					);
			}
		}

		/// <summary>
		///		Emits the using expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="disposableType">Type of the disposable.</param>
		/// <param name="instantiateIDisposableExpression">The expression which instantiate <see cref="IDisposable"/> object.</param>
		/// <param name="usingBodyEmitter">The using body which takes disposable object and returns actual statement.</param>
		/// <returns>The using statement.</returns>
		private TConstruct EmitUsingStatement(
			TContext context, Type disposableType, TConstruct instantiateIDisposableExpression, Func<TConstruct, TConstruct> usingBodyEmitter
		)
		{
			var disposable =
				this.DeclareLocal(
					context,
					disposableType,
					"disposable"
				);
			return
				this.EmitSequentialStatements(
					context,
					typeof( void ),
					disposable,
					this.EmitStoreVariableStatement( context, disposable, instantiateIDisposableExpression ),
					this.EmitTryFinally(
						context,
						usingBodyEmitter( disposable ),
						this.EmitConditionalExpression(
							context,
							this.EmitNotEqualsExpression(
								context,
								disposable,
								this.MakeNullLiteral( context, disposableType )
							),
							this.EmitInvokeMethodExpression(
								context,
								disposable,
								disposableType.GetMethod( "Dispose", ReflectionAbstractions.EmptyTypes )
							),
							null
						)
					)
				);
		}

		/// <summary>
		///		Emits the deserialize item expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="unpacker">The unpacker expression.</param>
		/// <param name="itemType">Type of the item to be deserialized.</param>
		/// <param name="unpacked">The variable which stores unpacked item.</param>
		/// <param name="memberInfo">The metadata of unpacking member.</param>
		/// <param name="itemsSchema">The schema for collection items. <c>null</c> for non-collection items and non-schema items.</param>
		/// <returns>The expression which returns deserialized item.</returns>
		private TConstruct EmitDeserializeItemExpression(
			TContext context, TConstruct unpacker, Type itemType, TConstruct unpacked, SerializingMember? memberInfo, PolymorphismSchema itemsSchema
		)
		{
			return
				this.EmitStoreVariableStatement(
					context,
					unpacked,
					this.EmitDeserializeItemExpressionCore( context, unpacker, itemType, memberInfo, itemsSchema )
				);
		}

		/// <summary>
		///		Emits the deserialize item expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="unpacker">The unpacker expression.</param>
		/// <param name="itemType">Type of the item to be deserialized.</param>
		/// <param name="memberInfo">The metadata of unpacking member.</param>
		/// <param name="itemsSchema">The schema for collection items. <c>null</c> for non-collection items and non-schema items.</param>
		/// <returns>The expression which returns deserialized item.</returns>
		private TConstruct EmitDeserializeItemExpressionCore( TContext context, TConstruct unpacker, Type itemType, SerializingMember? memberInfo, PolymorphismSchema itemsSchema )
		{
			return 
				this.EmitInvokeMethodExpression(
					context,
					this.EmitGetSerializerExpression( context, itemType, memberInfo, itemsSchema ),
					typeof( MessagePackSerializer<> ).MakeGenericType( itemType ).GetMethod( "UnpackFrom" ),
					unpacker
				);
		}

		/// <summary>
		///		Emits the conditional expression (cond?then:else).
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="conditionExpression">The expression which represents conditional.</param>
		/// <param name="thenExpression">The expression which is used when condition is true.</param>
		/// <param name="elseExpression">The expression which is used when condition is false.</param>
		/// <returns>The conditional expression.</returns>
		protected abstract TConstruct EmitConditionalExpression(
			TContext context,
			TConstruct conditionExpression,
			TConstruct thenExpression,
			TConstruct elseExpression
		);

		/// <summary>
		///		Emits the conditional expression (cond?then:else) which has short circuit and expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="conditionExpressions">The expression which represents short circuit and expression.</param>
		/// <param name="thenExpression">The expression which is used when condition is true.</param>
		/// <param name="elseExpression">The expression which is used when condition is false.</param>
		/// <returns>The conditional expression.</returns>
		protected abstract TConstruct EmitAndConditionalExpression(
			TContext context,
			IList<TConstruct> conditionExpressions,
			TConstruct thenExpression,
			TConstruct elseExpression
		);

		///  <summary>
		/// 		Emits string switch statement.
		///  </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="target">Target string expression.</param>
		/// <param name="cases">The case statements. The keys are case condition, and values are actual statement.</param>
		/// <param name="defaultCase">Default case expression.</param>
		/// <returns>The switch statement.</returns>
		protected abstract TConstruct EmitStringSwitchStatement( TContext context, TConstruct target, IDictionary<string, TConstruct> cases, TConstruct defaultCase );

		/// <summary>
		/// 	Emits the return statement
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="expression">The expression to be returned.</param>
		/// <returns>The return statement.</returns>
		protected virtual TConstruct EmitRetrunStatement( TContext context, TConstruct expression )
		{
			return expression;
		}

		/// <summary>
		/// 	Emits the for loop.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="count">The count reference for loop terminiation condition.</param>
		/// <param name="loopBodyEmitter">The loop body emitter which takes for loop context then returns loop body construct.</param>
		/// <returns>The for loop.</returns>
		protected abstract TConstruct EmitForLoop( TContext context, TConstruct count, Func<ForLoopContext, TConstruct> loopBodyEmitter );

		/// <summary>
		/// 	Emits the for-each loop.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="collectionTraits">The traits of the collection.</param>
		/// <param name="collection">The collection reference.</param>
		/// <param name="loopBodyEmitter">The loop body emitter which takes item reference then returns loop body construct.</param>
		/// <returns>The for each loop.</returns>
		protected abstract TConstruct EmitForEachLoop(
			TContext context,
			CollectionTraits collectionTraits,
			TConstruct collection,
			Func<TConstruct, TConstruct> loopBodyEmitter
		);

		/// <summary>
		///		Emits the append collection item.
		/// </summary>
		/// <param name="context">The code generation context.</param>
		/// <param name="member">The read only collection member metadata. <c>null</c> for collection item.</param>
		/// <param name="traits">The traits of the collection.</param>
		/// <param name="collection">The collection to be appended.</param>
		/// <param name="unpackedItem">The unpacked item.</param>
		/// <returns></returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		/// </exception>
		private TConstruct EmitAppendCollectionItem(
			TContext context,
			MemberInfo member,
			CollectionTraits traits,
			TConstruct collection,
			TConstruct unpackedItem
		)
		{
			if ( traits.AddMethod == null )
			{
				if ( member != null )
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Type '{0}' of read only member '{1}' does not have public 'Add' method.",
							member.GetMemberValueType(),
							member
						)
					);
				}
				else
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Type '{0}' does not have public 'Add' method.",
							collection.ContextType
						)
					);
				}
			}

#if DEBUG
			Contract.Assert( traits.AddMethod.DeclaringType != null, "traits.AddMethod.DeclaringType != null" );
#endif // DEBUG
			return
				this.EmitInvokeVoidMethod(
					context,
					traits.AddMethod.DeclaringType.IsAssignableFrom( collection.ContextType )
					? collection
					: this.EmitUnboxAnyExpression( context, traits.AddMethod.DeclaringType, collection ),
					traits.AddMethod,
					unpackedItem
				);
		}

		private TConstruct EmitAppendDictionaryItem( TContext context, CollectionTraits traits, TConstruct dictionary, Type keyType, TConstruct key, Type valueType, TConstruct value, bool withBoxing )
		{
#if DEBUG
			Contract.Assert( traits.AddMethod.DeclaringType != null, "traits.AddMethod.DeclaringType != null" );
#endif // DEBUG
			return
				this.EmitInvokeVoidMethod(
					context,
					traits.AddMethod.DeclaringType.IsAssignableFrom( dictionary.ContextType )
					? dictionary
					: this.EmitUnboxAnyExpression( context, traits.AddMethod.DeclaringType, dictionary ),
					traits.AddMethod,
					withBoxing
					? this.EmitBoxExpression( context, keyType, key )
					: key,
					withBoxing
					? this.EmitBoxExpression( context, valueType, value )
					: value
				);
		}

		private static void GetDictionaryKeyValueType( Type elementType, out Type keyType, out Type valueType )
		{
			if ( elementType == typeof( DictionaryEntry ) )
			{
				keyType = typeof( object );
				valueType = typeof( object );
			}
			else
			{
				keyType = elementType.GetGenericArguments()[ 0 ];
				valueType = elementType.GetGenericArguments()[ 1 ];
			}
		}

		/// <summary>
		///		Retrieves a default constructor of the specified elementType.
		/// </summary>
		/// <param name="instanceType">The target elementType.</param>
		/// <returns>A default constructor of the <paramref name="instanceType"/>.</returns>
		private static ConstructorInfo GetDefaultConstructor( Type instanceType )
		{
#if DEBUG
			Contract.Assert( !instanceType.GetIsValueType() );
#endif

			var ctor = typeof( TObject ).GetConstructor( ReflectionAbstractions.EmptyTypes );
			if ( ctor == null )
			{
				throw SerializationExceptions.NewTargetDoesNotHavePublicDefaultConstructor( instanceType );
			}

			return ctor;
		}

		/// <summary>
		///		Determines the collection constructor arguments.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="constructor">The constructor.</param>
		/// <returns>
		///		An array of constructs representing constructor arguments.
		/// </returns>
		/// <exception cref="System.NotSupportedException">
		///		The <paramref name="constructor"/> has unsupported signature.
		/// </exception>
		private TConstruct[] DetermineCollectionConstructorArguments( TContext context, ConstructorInfo constructor )
		{
			var parameters = constructor.GetParameters();
			switch ( parameters.Length )
			{
				case 0:
				{
					return NoConstructs;
				}
				case 1:
				{
					return new[] { this.GetConstructorArgument( context, parameters[ 0 ] ) };
				}
				case 2:
				{
					return new[] { this.GetConstructorArgument( context, parameters[ 0 ] ), this.GetConstructorArgument( context, parameters[ 1 ] ) };
				}
				default:
				{
					throw new NotSupportedException(
						String.Format( CultureInfo.CurrentCulture, "Constructor signature '{0}' is not supported.", constructor )
					);
				}
			}
		}


		/// <summary>
		///		Gets the construt for constructor argument.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="parameter">The parameter of the constructor parameter.</param>
		/// <returns>The construt for constructor argument.</returns>
		private TConstruct GetConstructorArgument( TContext context, ParameterInfo parameter )
		{
			return
				parameter.ParameterType == typeof( int )
				? context.InitialCapacity
				: this.EmitGetEqualityComparer( context );
		}


		/// <summary>
		///		Emits the construct to get equality comparer via <see cref="UnpackHelpers"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		///		The construct to get equality comparer via <see cref="UnpackHelpers"/>.
		/// </returns>
		private TConstruct EmitGetEqualityComparer( TContext context )
		{
			Type comparisonType;
			switch ( CollectionTraitsOfThis.DetailedCollectionType )
			{
				case CollectionDetailedKind.Array:
				case CollectionDetailedKind.GenericCollection:
				case CollectionDetailedKind.GenericEnumerable:
				case CollectionDetailedKind.GenericList:
#if !NETFX_35 && !UNITY
				case CollectionDetailedKind.GenericSet:
#if !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericReadOnlyCollection:
				case CollectionDetailedKind.GenericReadOnlyList:
#endif // !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
#endif // !NETFX_35 && !UNITY
				{
					comparisonType = CollectionTraitsOfThis.ElementType;
					break;
				}
				case CollectionDetailedKind.GenericDictionary:
#if !NETFX_35 && !UNITY && !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericReadOnlyDictionary:
#endif // !NETFX_35 && !UNITY && !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				{
					comparisonType = CollectionTraitsOfThis.ElementType.GetGenericArguments()[ 0 ];
					break;
				}
				default:
				{
					// non-generic
					comparisonType = typeof( object );
					break;
				}
			}

			return
				this.EmitInvokeMethodExpression(
					context,
					null,
					Metadata._UnpackHelpers.GetEqualityComparer_1Method.MakeGenericMethod( comparisonType )
				);
		}

		/// <summary>
		///		Emits <see cref="PolymorphismSchema"/> construction sequence.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="storage">The local variable which the schema to be stored.</param>
		/// <param name="schema">The <see cref="PolymorphismSchema"/> which contains emitting data.</param>
		/// <returns>
		///		Constructs to emit construct a copy of <paramref name="schema"/>.
		/// </returns>
		protected IEnumerable<TConstruct> EmitConstructPolymorphismSchema(
			TContext context,
			TConstruct storage,
			PolymorphismSchema schema
		)
		{
			if ( schema == null )
			{
				yield return
					this.EmitStoreVariableStatement(
						context,
						storage,
						this.MakeNullLiteral( context, typeof( PolymorphismSchema ) )
					);
				yield break;
			}

			switch ( schema.ChildrenType )
			{
				case PolymorphismSchemaChildrenType.CollectionItems:
				{
					/*
					 * __itemsTypeMap = new Dictionary<string, Type>();
					 * __itemsTypeMap.Add( b, t );
					 * :
					 * __itemsSchema = new PolymorphismSchema( __itemType, __itemsTypeMap, null ); // OR null
					 * __map = new Dictionary<string, Type>();
					 * __map.Add( b, t );
					 * :
					 * storage = new PolymorphismSchema( __type, __map, __itemsSchema );
					 */
					var itemsSchemaVariableName = context.GetUniqueVariableName( "itemsSchema" );
					var itemsSchema =
						schema.ItemSchema.UseDefault
							? this.MakeNullLiteral( context, typeof( PolymorphismSchema ) )
							: this.DeclareLocal( context, typeof( PolymorphismSchema ), itemsSchemaVariableName );
					if ( !schema.ItemSchema.UseDefault )
					{
						yield return itemsSchema;

						foreach (
							var instruction in
								this.EmitConstructLeafPolymorphismSchema( context, itemsSchema, schema.ItemSchema, itemsSchemaVariableName )
						)
						{
							yield return instruction;
						}
					}

					if ( schema.UseDefault )
					{
						yield return
							this.EmitStoreVariableStatement(
								context,
								storage,
								this.EmitInvokeMethodExpression(
									context,
									null,
									PolymorphismSchema.ForContextSpecifiedCollectionMethod,
									this.EmitTypeOfExpression( context, schema.TargetType ),
									itemsSchema
								)
							);
					}
					else if ( schema.UseTypeEmbedding )
					{
						yield return
							this.EmitStoreVariableStatement(
								context,
								storage,
								this.EmitInvokeMethodExpression(
									context,
									null,
									PolymorphismSchema.ForPolymorphicCollectionTypeEmbeddingMethod,
									this.EmitTypeOfExpression( context, schema.TargetType ),
									itemsSchema
								)
							);
					}
					else
					{
						var typeMap =
							this.DeclareLocal(
								context,
								typeof( Dictionary<string, Type> ),
								context.GetUniqueVariableName( "typeMap" )
							);

						yield return typeMap;
						yield return
							this.EmitStoreVariableStatement(
								context,
								typeMap,
								this.EmitCreateNewObjectExpression(
									context,
									typeMap,
									PolymorphismSchema.CodeTypeMapConstructor,
									this.MakeInt32Literal( context, schema.CodeTypeMapping.Count )
								)
							);


						foreach ( var instruction in this.EmitConstructTypeCodeMappingForPolymorphismSchema( context, schema, typeMap ) )
						{
							yield return instruction;
						}

						yield return
							this.EmitStoreVariableStatement(
								context,
								storage,
								this.EmitInvokeMethodExpression(
									context,
									null,
									PolymorphismSchema.ForPolymorphicCollectionCodeTypeMappingMethod,
									this.EmitTypeOfExpression( context, schema.TargetType ),
									typeMap,
									itemsSchema
								)
							);
					}
					break;
				}
				case PolymorphismSchemaChildrenType.DictionaryKeyValues:
				{
					/*
					 * __keysTypeMap = new Dictionary<string, Type>();
					 * __keysTypeMap.Add( b, t );
					 * :
					 * __keysSchema = new PolymorphismSchema( __keyType, __keysTypeMap ); // OR null
					 * __valuesTypeMap = new Dictionary<string, Type>();
					 * __valuesTypeMap.Add( b, t );
					 * :
					 * __valuesSchema = new PolymorphismSchema( __valueType, __valuesTypeMap ); // OR null
					 * __map = new Dictionary<string, Type>();
					 * __map.Add( b, t );
					 * :
					 * storage = new PolymorphismSchema( __type, __map, __keysSchema, __valuesSchema );
					 */
					var keysSchemaVariableName = context.GetUniqueVariableName( "keysSchema" );
					var keysSchema =
						schema.KeySchema.UseDefault
							? this.MakeNullLiteral( context, typeof( PolymorphismSchema ) )
							: this.DeclareLocal( context, typeof( PolymorphismSchema ), keysSchemaVariableName );
					if ( !schema.KeySchema.UseDefault )
					{
						yield return keysSchema;
						foreach (
							var instruction in
								this.EmitConstructLeafPolymorphismSchema( context, keysSchema, schema.KeySchema, keysSchemaVariableName )
						)
						{
							yield return instruction;
						}
					}

					var valuesSchemaVariableName = context.GetUniqueVariableName( "valuesSchema" );
					var valuesSchema =
						schema.ItemSchema.UseDefault
							? this.MakeNullLiteral( context, typeof( PolymorphismSchema ) )
							: this.DeclareLocal( context, typeof( PolymorphismSchema ), valuesSchemaVariableName );
					if ( !schema.ItemSchema.UseDefault )
					{
						yield return valuesSchema;
						foreach (
							var instruction in
								this.EmitConstructLeafPolymorphismSchema( context, valuesSchema, schema.ItemSchema, valuesSchemaVariableName )
						)
						{
							yield return instruction;
						}
					}

					if ( schema.UseDefault )
					{
						yield return
							this.EmitStoreVariableStatement(
								context,
								storage,
								this.EmitInvokeMethodExpression(
									context,
									null,
									PolymorphismSchema.ForContextSpecifiedDictionaryMethod,
									this.EmitTypeOfExpression( context, schema.TargetType ),
									keysSchema,
									valuesSchema
								)
							);
					}
					else if ( schema.UseTypeEmbedding )
					{
						yield return
							this.EmitStoreVariableStatement(
								context,
								storage,
								this.EmitInvokeMethodExpression(
									context,
									null,
									PolymorphismSchema.ForPolymorphicDictionaryTypeEmbeddingMethod,
									this.EmitTypeOfExpression( context, schema.TargetType ),
									keysSchema,
									valuesSchema
								)
							);
					}
					else
					{
						var typeMap =
							this.DeclareLocal(
								context,
								typeof( Dictionary<string, Type> ),
								context.GetUniqueVariableName( "typeMap" )
							);

						yield return typeMap;
						yield return
							this.EmitStoreVariableStatement(
								context,
								typeMap,
								this.EmitCreateNewObjectExpression(
									context,
									typeMap,
									PolymorphismSchema.CodeTypeMapConstructor,
									this.MakeInt32Literal( context, schema.CodeTypeMapping.Count )
								)
							);

						foreach ( var instruction in this.EmitConstructTypeCodeMappingForPolymorphismSchema( context, schema, typeMap ) )
						{
							yield return instruction;
						}

						yield return
							this.EmitStoreVariableStatement(
								context,
								storage,
								this.EmitInvokeMethodExpression(
									context,
									null,
									PolymorphismSchema.ForPolymorphicDictionaryCodeTypeMappingMethod,
									this.EmitTypeOfExpression( context, schema.TargetType ),
									typeMap,
									keysSchema,
									valuesSchema
								)
							);
					}
					break;
				}
#if !WINDOWS_PHONE && !NETFX_35 && !UNITY
				case PolymorphismSchemaChildrenType.TupleItems:
				{
					if ( schema.ChildSchemaList.Count == 0 )
					{
						yield return
							this.EmitStoreVariableStatement(
								context,
								storage,
								this.MakeNullLiteral( context, typeof( PolymorphismSchema ) )
							);
					}

					/*
					 * tupleItemsSchema = new PolymorphismSchema[__arity__];
					 * for(var i = 0; i < __arity__; i++ )
					 * {
					 *   __itemsTypeMap = new Dictionary<string, Type>();
					 *   __itemsTypeMap.Add( b, t );
					 *   :
					 *   itemsSchema = new PolymorphismSchema( __itemType, __itemsTypeMap, null ); // OR null
					 *   tupleItemsSchema[i] = itemsSchema;
					 * }
					 * __map = new Dictionary<string, Type>();
					 * __map.Add( b, t );
					 * :
					 * storage = new PolymorphismSchema( __type, __map, __itemsSchema );
					 */
					var tupleItems = TupleItems.GetTupleItemTypes( schema.TargetType );
					var tupleItemsSchema =
						this.DeclareLocal(
							context,
							typeof( PolymorphismSchema[] ),
							context.GetUniqueVariableName( "tupleItemsSchema" )
						);

					yield return tupleItemsSchema;

					yield return
						this.EmitStoreVariableStatement(
							context,
							tupleItemsSchema,
							this.EmitCreateNewArrayExpression( context, typeof( PolymorphismSchema ), tupleItems.Count )
						);
					for ( var i = 0; i < tupleItems.Count; i++ )
					{
						var variableName = context.GetUniqueVariableName( "tupleItemSchema" );
						var itemSchema = this.DeclareLocal( context, typeof( PolymorphismSchema ), variableName );
						yield return itemSchema;
						foreach ( var statement in this.EmitConstructLeafPolymorphismSchema( context, itemSchema, schema.ChildSchemaList[ i ], variableName ) )
						{
							yield return statement;
						}

						yield return this.EmitSetArrayElementStatement( context, tupleItemsSchema, this.MakeInt32Literal( context, i ), itemSchema );
					}

					yield return
						this.EmitStoreVariableStatement(
							context,
							storage,
							this.EmitInvokeMethodExpression(
								context,
								null,
								PolymorphismSchema.ForPolymorphicTupleMethod,
								this.EmitTypeOfExpression( context, schema.TargetType ),
								tupleItemsSchema
							)
						);
					break;
				}
#endif // !WINDOWS_PHONE && !NETFX_35 && !UNITY
				default:
				{
					foreach ( var instruction in
						this.EmitConstructLeafPolymorphismSchema( context, storage, schema, String.Empty ) )
					{
						yield return instruction;
					}

					break;
				}
			}
		}

		private IEnumerable<TConstruct> EmitConstructLeafPolymorphismSchema( TContext context, TConstruct storage, PolymorphismSchema currentSchema, string prefix )
		{
			/*
			 * __itemsTypeMap = new Dictionary<string, Type>();
			 * __itemsTypeMap.Add( b, t );
			 * :
			 * __itemsSchema = new PolymorphismSchema( __itemType, __itemsTypeMap, null ); // OR null
			 * __map = new Dictionary<string, Type>();
			 * __map.Add( b, t );
			 * :
			 * storage = new PolymorphismSchema( __type, __map, __itemsSchema );
			 */
			if ( currentSchema.UseDefault )
			{
				yield return
					this.EmitStoreVariableStatement(
						context,
						storage,
						this.MakeNullLiteral( context, typeof( PolymorphismSchema ) )
					);
			}
			else if ( currentSchema.UseTypeEmbedding )
			{
				yield return
					this.EmitStoreVariableStatement(
						context,
						storage,
						this.EmitInvokeMethodExpression(
							context,
							null,
							PolymorphismSchema.ForPolymorphicObjectTypeEmbeddingMethod,
							this.EmitTypeOfExpression( context, currentSchema.TargetType )
						)
					);
			}
			else
			{
				var typeMap =
					this.DeclareLocal(
						context,
						typeof( Dictionary<string, Type> ),
						context.GetUniqueVariableName( String.IsNullOrEmpty( prefix ) ? "typeMap" : ( prefix + "TypeMap" ) )
					);

				yield return typeMap;

				foreach ( var instruction in this.EmitConstructTypeCodeMappingForPolymorphismSchema( context, currentSchema, typeMap ) )
				{
					yield return instruction;
				}

				yield return
					this.EmitStoreVariableStatement(
						context,
						storage,
						this.EmitInvokeMethodExpression(
							context,
							null,
							PolymorphismSchema.ForPolymorphicObjectCodeTypeMappingMethod,
							this.EmitTypeOfExpression( context, currentSchema.TargetType ),
							typeMap
						)
					);
			}
		}

		private IEnumerable<TConstruct> EmitConstructTypeCodeMappingForPolymorphismSchema(
			TContext context,
			PolymorphismSchema currentSchema,
			TConstruct typeMap )
		{
			yield return
				this.EmitStoreVariableStatement(
					context,
					typeMap,
					this.EmitCreateNewObjectExpression(
						context,
						typeMap,
						PolymorphismSchema.CodeTypeMapConstructor,
						this.MakeInt32Literal( context, currentSchema.CodeTypeMapping.Count )
					)
				);

			foreach ( var entry in currentSchema.CodeTypeMapping )
			{
				yield return
					this.EmitInvokeMethodExpression(
						context,
						typeMap,
						PolymorphismSchema.AddToCodeTypeMapMethod,
						this.MakeStringLiteral( context, entry.Key ),
						this.EmitTypeOfExpression( context, entry.Value )
					);
			}
		}

		/// <summary>
		///		Represents for-loop context information.
		/// </summary>
		protected class ForLoopContext
		{
			/// <summary>
			///		Gets the counter variable (<c>i</c>).
			/// </summary>
			/// <value>
			///		The counter variable.
			/// </value>
			public TConstruct Counter { get; private set; }

			public ForLoopContext( TConstruct counter )
			{
				this.Counter = counter;
			}
		}
	}
}
