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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		Implements expression tree based serializer for general object.
	/// </summary>
	/// <typeparam name="T">The type of target object.</typeparam>
	internal abstract class ObjectExpressionMessagePackSerializer<T> : MessagePackSerializer<T>
#if !SILVERLIGHT
, IExpressionMessagePackSerializer
#endif
	{
		private readonly Func<T, object>[] _memberGetters;

		protected Func<T, object>[] MemberGetters
		{
			get { return this._memberGetters; }
		}

		private readonly Action<T, object>[] _memberSetters;

		private readonly IMessagePackSerializer[] _memberSerializers;

		protected IMessagePackSerializer[] MemberSerializers
		{
			get { return this._memberSerializers; }
		}

		private readonly NilImplication[] _nilImplications;
		private readonly bool[] _isCollection;
		private readonly string[] _memberNames;

		protected string[] MemberNames
		{
			get { return this._memberNames; }
		}

		private readonly Dictionary<string, int> _indexMap;

		private readonly Func<T> _createInstance;

		protected ObjectExpressionMessagePackSerializer( SerializationContext context, SerializingMember[] members )
		{
			this._createInstance = Expression.Lambda<Func<T>>( Expression.New( typeof( T ).GetConstructor( ReflectionAbstractions.EmptyTypes ) ) ).Compile();
			this._memberSerializers = members.Select( m => context.GetSerializer( m.Member.GetMemberValueType() ) ).ToArray();
			this._indexMap =
				members
				.Zip( Enumerable.Range( 0, members.Length ), ( m, i ) => new KeyValuePair<MemberInfo, int>( m.Member, i ) )
				.ToDictionary( kv => kv.Key.Name, kv => kv.Value );

			var targetParameter = Expression.Parameter( typeof( T ), "target" );
			this._isCollection = members.Select( m => m.Member.GetMemberValueType().GetCollectionTraits() ).Select( t => t.CollectionType != CollectionKind.NotCollection ).ToArray();
			this._nilImplications = members.Select( m => m.Contract.NilImplication ).ToArray();
			this._memberNames = members.Select( m => m.Contract.Name ).ToArray();
			this._memberGetters =
				members.Select(
					m =>
						Expression.Lambda<Func<T, object>>(
							Expression.Convert(
								Expression.PropertyOrField(
									targetParameter,
									m.Member.Name
								),
								typeof( object )
							),
							targetParameter
						).Compile()
				).ToArray();
			var valueParameter = Expression.Parameter( typeof( object ), "value" );
			this._memberSetters =
				members.Select(
					m =>
						CanWrite( m.Member )
						? Expression.Lambda<Action<T, object>>(
							Expression.Assign(
								Expression.PropertyOrField(
									targetParameter,
									m.Member.Name
								),
								m.Member.GetMemberValueType().GetIsValueType()
								? Expression.Condition(
									Expression.ReferenceEqual( valueParameter, Expression.Constant( null ) ),
									Expression.Throw(
										Expression.Call(
											null,
											SerializationExceptions.NewValueTypeCannotBeNull3Method,
											Expression.Constant( m.Member.Name ),
											Expression.Quote(
											Expression.Constant( m.Member.GetMemberValueType() ) ),
											Expression.Quote(
											Expression.Constant( m.Member.DeclaringType ) )
										),
										m.Member.GetMemberValueType()
									),
									Expression.Convert( valueParameter, m.Member.GetMemberValueType() )
								) as Expression
								: Expression.Convert( valueParameter, m.Member.GetMemberValueType() )
							),
							targetParameter,
							valueParameter
						).Compile()
						: UnpackHelpers.IsReadOnlyAppendableCollectionMember( m.Member )
						? default( Action<T, object> )
						: Expression.Lambda<Action<T, object>>(
							Expression.Throw(
								Expression.New(
									Metadata._NotImplementedException.ctor_String,
									Expression.Constant(
										String.Format( CultureInfo.CurrentCulture, "Cannot handle read only member '{0}'.", m.Member )
									)
								)
							),
							targetParameter,
							valueParameter
						).Compile()
				).ToArray();
		}

		private static bool CanWrite( MemberInfo member )
		{
			PropertyInfo asProperty = member as PropertyInfo;
			if ( asProperty != null )
			{
				return asProperty.CanWrite;
			}
			else
			{
				var asFieldInfo = member as FieldInfo;
				return !asFieldInfo.IsInitOnly && !asFieldInfo.IsLiteral;
			}
		}

		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			// FIXME: Redesign missing element handling
			if ( unpacker.IsArrayHeader && unpacker.ItemsCount != this._memberSerializers.Length )
			{
				throw SerializationExceptions.NewUnexpectedArrayLength( this._memberSerializers.Length, unchecked( ( int )unpacker.ItemsCount ) );
			}

			// Assume subtree unpacker
			var instance = this._createInstance();
			if ( unpacker.IsArrayHeader )
			{
				this.UnpackFromArray( unpacker, instance );
			}
			else
			{
				this.UnpackFromMap( unpacker, instance );
			}

			return instance;
		}

		private void UnpackFromArray( Unpacker unpacker, T instance )
		{
			for ( int i = 0; i < this.MemberSerializers.Length; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				if ( unpacker.Data.Value.IsNil )
				{
					switch ( this._nilImplications[ i ] )
					{
						case NilImplication.Null:
						{
							if ( this._memberSetters[ i ] == null )
							{
								throw SerializationExceptions.NewReadOnlyMemberItemsMustNotBeNull( this._memberNames[ i ] );
							}

							this._memberSetters[ i ]( instance, null );
							break;
						}
						case NilImplication.MemberDefault:
						{
							break;
						}
						case NilImplication.Prohibit:
						{
							throw SerializationExceptions.NewNullIsProhibited( this._memberNames[ i ] );
						}
					}

					continue;
				}

				this._memberSetters[ i ]( instance, this.MemberSerializers[ i ].UnpackFrom( unpacker ) );
			}
		}

		private void UnpackFromMap( Unpacker unpacker, T instance )
		{
			while ( unpacker.Read() )
			{
				var memberName = unpacker.Data.Value.AsString();
				int index;
				if ( !this._indexMap.TryGetValue( memberName, out index ) )
				{
					// Drains unused value.
					if ( !unpacker.Read() )
					{
						throw SerializationExceptions.NewUnexpectedEndOfStream();
					}

					// TODO: unknown member handling.

					continue;
				}

				// Fetches value
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				if ( unpacker.Data.Value.IsNil )
				{
					switch ( this._nilImplications[ index ] )
					{
						case NilImplication.Null:
						{
							if ( this._memberSetters[ index ] == null )
							{
								throw SerializationExceptions.NewReadOnlyMemberItemsMustNotBeNull( this._memberNames[ index ] );
							}

							this._memberSetters[ index ]( instance, null );
							continue;
						}
						case NilImplication.MemberDefault:
						{
							continue;
						}
						case NilImplication.Prohibit:
						{
							throw SerializationExceptions.NewNullIsProhibited( this._memberNames[ index ] );
						}
					}
				}

				this._memberSetters[ index ]( instance, this.MemberSerializers[ index ].UnpackFrom( unpacker ) );
			}
		}

#if !SILVERLIGHT
		public override string ToString()
		{
			var buffer = new StringBuilder( Int16.MaxValue );
			using ( var writer = new StringWriter( buffer ) )
			{
				this.ToStringCore( writer, 0 );
			}

			return buffer.ToString();
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

			for ( int i = 0; i < this._memberSerializers.Length; i++ )
			{
				ExpressionDumper.WriteIndent( writer, depth + 1 );
				writer.Write( this._memberNames[ i ] );
				writer.Write( " : " );
				var expressionSerializer = this._memberSerializers[ i ] as IExpressionMessagePackSerializer;
				if ( expressionSerializer != null )
				{
					expressionSerializer.ToString( writer, depth + 2 );
				}
				else
				{
					writer.Write( this._memberSerializers[ i ] );
				}

				writer.WriteLine();
			}
		}
#endif
	}
}