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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

// FIXME: Unify collection/dictionary handling between emit and expression tree to improve quality and reduce maintenance costs.

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

		private readonly MemberSetter[] _memberSetters;

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
		private readonly Action<T, Packer, PackingOptions> _packToMessage;
		private readonly UnpackFromMessageInvocation _unpackFromMessage;

		protected ObjectExpressionMessagePackSerializer( SerializationContext context, SerializingMember[] members )
			: base( ( context ?? SerializationContext.Default ).CompatibilityOptions.PackerCompatibilityOptions )
		{
			if ( typeof( T ).GetIsAbstract() || typeof( T ).GetIsInterface() )
			{
				throw SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractType( typeof( T ) );
			}

			this._createInstance =
				Expression.Lambda<Func<T>>(
					typeof( T ).GetIsValueType()
						? Expression.Default( typeof( T ) ) as Expression
						: Expression.New( typeof( T ).GetConstructor( ReflectionAbstractions.EmptyTypes ) )
					).Compile();
			var isPackable = typeof( IPackable ).IsAssignableFrom( typeof( T ) );
			var isUnpackable = typeof( IUnpackable ).IsAssignableFrom( typeof( T ) );

			var targetParameter = Expression.Parameter( typeof( T ), "target" );

			if ( isPackable && isUnpackable )
			{
				this._memberSerializers = null;
				this._indexMap = null;
				this._isCollection = null;
				this._nilImplications = null;
				this._memberNames = null;
			}
			else
			{
				this._memberSerializers =
					members.Select(
						m => m.Member == null ? NullSerializer.Instance : context.GetSerializer( m.Member.GetMemberValueType() ) ).ToArray
						(

						);
				this._indexMap =
					members
						.Zip( Enumerable.Range( 0, members.Length ), ( m, i ) => new KeyValuePair<SerializingMember, int>( m, i ) )
						.Where( kv => kv.Key.Member != null )
						.ToDictionary( kv => kv.Key.Contract.Name, kv => kv.Value );

				this._isCollection =
					members.Select(
						m => m.Member == null ? CollectionTraits.NotCollection : m.Member.GetMemberValueType().GetCollectionTraits() ).
						Select( t => t.CollectionType != CollectionKind.NotCollection ).ToArray();

				// NilImplication validity check
				foreach ( var member in members )
				{
					switch ( member.Contract.NilImplication )
					{
						case NilImplication.Null:
						{
							if ( member.Member.GetMemberValueType().GetIsValueType() &&
								Nullable.GetUnderlyingType( member.Member.GetMemberValueType() ) == null )
							{
								throw SerializationExceptions.NewValueTypeCannotBeNull(
									member.Contract.Name, member.Member.GetMemberValueType(), member.Member.DeclaringType
								);
							}

							if ( !member.Member.CanSetValue() )
							{
								throw SerializationExceptions.NewReadOnlyMemberItemsMustNotBeNull( member.Contract.Name );
							}

							break;
						}
					}
				}

				this._nilImplications = members.Select( m => m.Contract.NilImplication ).ToArray();
				this._memberNames = members.Select( m => m.Contract.Name ).ToArray();
			}

			if ( isPackable )
			{
				var packerParameter = Expression.Parameter( typeof( Packer ), "packer" );
				var optionsParameter = Expression.Parameter( typeof( PackingOptions ), "options" );
				this._packToMessage =
					Expression.Lambda<Action<T, Packer, PackingOptions>>(
						Expression.Call(
							targetParameter,
							typeof( T ).GetInterfaceMap( typeof( IPackable ) ).TargetMethods.Single(),
							packerParameter,
							optionsParameter
						),
						targetParameter, packerParameter, optionsParameter
					).Compile();
				this._memberGetters = null;
			}
			else
			{
				this._packToMessage = null;
				this._memberGetters =
					members.Select(
						m =>
						m.Member == null
						? Expression.Lambda<Func<T, object>>(
							Expression.Constant( null ),
							targetParameter
						).Compile()
						: CreateMemberGetter( targetParameter, m )
					).ToArray();
			}

			var refTargetParameter = Expression.Parameter( typeof( T ).MakeByRefType(), "target" );
			if ( isUnpackable )
			{
				var unpackerParameter = Expression.Parameter( typeof( Unpacker ), "unpacker" );
				this._unpackFromMessage =
					Expression.Lambda<UnpackFromMessageInvocation>(
						Expression.Call(
							refTargetParameter,
							typeof( T ).GetInterfaceMap( typeof( IUnpackable ) ).TargetMethods.Single(),
							unpackerParameter
						),
						refTargetParameter, unpackerParameter
					).Compile();
				this._memberSetters = null;
			}
			else
			{
				this._unpackFromMessage = null;

				var valueParameter = Expression.Parameter( typeof( object ), "value" );
				this._memberSetters =
					members.Select(
						m =>
						m.Member == null
						? Expression.Lambda<MemberSetter>(
							Expression.Empty(),
							refTargetParameter,
							valueParameter
						).Compile()
						: m.Member.CanSetValue()
						? Expression.Lambda<MemberSetter>(
							Expression.Assign(
								Expression.PropertyOrField(
									refTargetParameter,
									m.Member.Name
								),
								Expression.Call(
									Metadata._UnpackHelpers.ConvertWithEnsuringNotNull_1Method.MakeGenericMethod( m.Member.GetMemberValueType() ),
									valueParameter,
									Expression.Constant( m.Member.Name ),
									Expression.Call( // Using RuntimeTypeHandle to avoid WinRT expression tree issue.
										null,
										Metadata._Type.GetTypeFromHandle,
										Expression.Constant( m.Member.DeclaringType.TypeHandle )
									)
								)
							),
							refTargetParameter,
							valueParameter
						).Compile()
						: UnpackHelpers.IsReadOnlyAppendableCollectionMember( m.Member )
						? default( MemberSetter )
						: ThrowGetOnlyMemberIsInvalid( m.Member )
				).ToArray();
			}
		}

		private static Func<T, object> CreateMemberGetter( ParameterExpression targetParameter, SerializingMember member )
		{
			var coreGetter =
				Expression.Lambda<Func<T, object>>( 
					Expression.Convert(
						Expression.PropertyOrField(
							targetParameter,
							member.Member.Name
						),
						typeof( object )
					),
					targetParameter 
				).Compile();
			if( member.Contract.NilImplication == NilImplication.Prohibit )
			{
				var name = member.Contract.Name;
				return target =>
				       {
					       var gotten = coreGetter( target );
						   if( gotten == null )
						   {
							   throw SerializationExceptions.NewNullIsProhibited( name );
						   }

					       return gotten;
				       };
			}
			else
			{
				return coreGetter;
			}
		}

		private static MemberSetter ThrowGetOnlyMemberIsInvalid( MemberInfo member )
		{
			var asProperty = member as PropertyInfo;
			if ( asProperty != null )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot set value to '{0}.{1}' property.", asProperty.DeclaringType, asProperty.Name ) );
			}
			else
			{
				Contract.Assert( member is FieldInfo, member.ToString() + ":" + member.GetType() );
				throw new SerializationException(
					String.Format(
						CultureInfo.CurrentCulture, "Cannot set value to '{0}.{1}' field.", member.DeclaringType, member.Name
					)
				);
			}
		}

		protected internal sealed override void PackToCore( Packer packer, T objectTree )
		{
			if ( this._packToMessage != null )
			{
				this._packToMessage( objectTree, packer, null );
			}
			else
			{
				this.PackToCoreOverride( packer, objectTree );
			}
		}

		protected abstract void PackToCoreOverride( Packer packer, T objectTree );

		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			// Assume subtree unpacker
			var instance = this._createInstance();

			if ( this._unpackFromMessage != null )
			{
				this._unpackFromMessage( ref instance, unpacker );
			}
			else
			{
				if ( unpacker.IsArrayHeader )
				{
					this.UnpackFromArray( unpacker, ref instance );
				}
				else
				{
					this.UnpackFromMap( unpacker, ref instance );
				}
			}

			return instance;
		}

		private void UnpackFromArray( Unpacker unpacker, ref T instance )
		{
			int unpacked = 0;
			int itemsCount = checked( ( int )unpacker.ItemsCount );
			for ( int i = 0; i < this.MemberSerializers.Length; i++ )
			{
				if ( unpacked == itemsCount )
				{
					// It is OK to avoid skip missing member because default NilImplication is MemberDefault so it is harmless.
					this.HandleNilImplication( ref instance, i );
				}
				else
				{
					if ( !unpacker.Read() )
					{
						throw SerializationExceptions.NewUnexpectedEndOfStream();
					}

					if ( unpacker.LastReadData.IsNil )
					{
						this.HandleNilImplication( ref instance, i );
					}
					else
					{
						if ( unpacker.IsArrayHeader || unpacker.IsMapHeader )
						{
							using ( var subtreeUnpacker = unpacker.ReadSubtree() )
							{
								this.UnpackMemberInArray( subtreeUnpacker, ref instance, i );
							}
						}
						else
						{
							this.UnpackMemberInArray( unpacker, ref instance, i );
						}
					}

					unpacked++;
				}
			}
		}

		private void HandleNilImplication( ref T instance, int index )
		{
			switch ( this._nilImplications[ index ] )
			{
				case NilImplication.Null:
				{
					this._memberSetters[ index ]( ref instance, null );
					break;
				}
				case NilImplication.MemberDefault:
				{
					break;
				}
				case NilImplication.Prohibit:
				{
					throw SerializationExceptions.NewNullIsProhibited( this._memberNames[ index ] );
				}
			}
		}

		private void UnpackMemberInArray( Unpacker unpacker, ref T instance, int i )
		{
			if ( this._memberSetters[ i ] == null )
			{
				// Use null as marker because index mapping cannot be constructed in the constructor.
				this._memberSerializers[ i ].UnpackTo( unpacker, this._memberGetters[ i ]( instance ) );
			}
			else
			{
				this._memberSetters[ i ]( ref instance, this._memberSerializers[ i ].UnpackFrom( unpacker ) );
			}
		}

		private void UnpackFromMap( Unpacker unpacker, ref T instance )
		{
			while ( unpacker.Read() )
			{
				var memberName = GetMemberName( unpacker );
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

				if ( unpacker.LastReadData.IsNil )
				{
					switch ( this._nilImplications[ index ] )
					{
						case NilImplication.Null:
						{
							this._memberSetters[ index ]( ref instance, null );
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

				if ( unpacker.IsArrayHeader || unpacker.IsMapHeader )
				{
					using ( var subtreeUnpacker = unpacker.ReadSubtree() )
					{
						this.UnpackMemberInMap( subtreeUnpacker, ref instance, index );
					}
				}
				else
				{
					this.UnpackMemberInMap( unpacker, ref instance, index );
				}
			}
		}

		private static string GetMemberName( Unpacker unpacker )
		{
			try
			{
				return unpacker.LastReadData.AsString();
			}
			catch ( InvalidOperationException ex )
			{
				throw new InvalidMessagePackStreamException( "Cannot get a member name from stream.", ex );
			}
		}

		private void UnpackMemberInMap( Unpacker unpacker, ref T instance, int index )
		{
			if ( this._memberSetters[ index ] == null )
			{
				// Use null as marker because index mapping cannot be constructed in the constructor.
				this._memberSerializers[ index ].UnpackTo( unpacker, this._memberGetters[ index ]( instance ) );
			}
			else
			{
				this._memberSetters[ index ]( ref instance, this._memberSerializers[ index ].UnpackFrom( unpacker ) );
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

		private sealed class NullSerializer : IMessagePackSingleObjectSerializer
		{
			public static readonly NullSerializer Instance = new NullSerializer();

			private NullSerializer()
			{
			}

			public void PackTo( Packer packer, object objectTree )
			{
				if ( packer == null )
				{
					throw new ArgumentNullException( "packer" );
				}

				Contract.EndContractBlock();

				packer.PackNull();
			}

			public object UnpackFrom( Unpacker unpacker )
			{
				if ( unpacker == null )
				{
					throw new ArgumentNullException( "unpacker" );
				}

				Contract.Ensures( Contract.Result<object>() == null );

				// Always returns null.
				return null;
			}

			public void UnpackTo( Unpacker unpacker, object collection )
			{
				if ( unpacker == null )
				{
					throw new ArgumentNullException( "unpacker" );
				}

				if ( collection == null )
				{
					throw new ArgumentNullException( "collection" );
				}

				if ( !( collection is T ) )
				{
					throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "'{0}' is not compatible for '{1}'.", collection.GetType(), typeof( T ) ), "collection" );
				}

				throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, "This operation is not supported by '{0}'.", this.GetType() ) );
			}

			public byte[] PackSingleObject( object objectTree )
			{
				using ( var stream = new MemoryStream() )
				using ( var packer = Packer.Create( stream ) )
				{
					this.PackTo( packer, objectTree );
					return stream.ToArray();
				}
			}

			public object UnpackSingleObject( byte[] buffer )
			{
				using ( var stream = new MemoryStream( buffer ) )
				using ( var unpacker = Unpacker.Create( stream ) )
				{
					return this.UnpackFrom( unpacker );
				}
			}
		}

		private delegate void UnpackFromMessageInvocation( ref T target, Unpacker value );
		protected delegate void MemberSetter( ref T target, object memberValue );
	}
}