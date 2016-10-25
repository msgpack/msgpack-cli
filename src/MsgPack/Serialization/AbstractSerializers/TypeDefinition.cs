#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015 FUJIWARA, Yusuke
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
#if CORE_CLR || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || NETSTANDARD1_1
using System.Globalization;
using System.Linq;
using System.Reflection;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.AbstractSerializers
{
	/// <summary>
	///		Represents type which may not have built metadata.
	/// </summary>
	internal class TypeDefinition
	{
		private static readonly TypeDefinition[] EmptyArray = new TypeDefinition[ 0 ];

		public static readonly TypeDefinition ObjectType = Object( typeof( object ) );
		public static readonly TypeDefinition ByteType = Object( typeof( byte ) );
		public static readonly TypeDefinition SByteType = Object( typeof( sbyte ) );
		public static readonly TypeDefinition Int16Type = Object( typeof( short ) );
		public static readonly TypeDefinition UInt16Type = Object( typeof( ushort ) );
		public static readonly TypeDefinition Int32Type = Object( typeof( int ) );
		public static readonly TypeDefinition UInt32Type = Object( typeof( uint ) );
		public static readonly TypeDefinition Int64Type = Object( typeof( long ) );
		public static readonly TypeDefinition UInt64Type = Object( typeof( ulong ) );
		public static readonly TypeDefinition SingleType = Object( typeof( float ) );
		public static readonly TypeDefinition DoubleType = Object( typeof( double ) );
		public static readonly TypeDefinition BooleanType = Object( typeof( bool ) );
		public static readonly TypeDefinition CharType = Object( typeof( char ) );
		public static readonly TypeDefinition StringType = Object( typeof( string ) );
		public static readonly TypeDefinition VoidType = Object( typeof( void ) );
		public static readonly TypeDefinition ObjectArrayType = Array( ObjectType );
		public static readonly TypeDefinition TypeType = Object( typeof( Type ) );
		public static readonly TypeDefinition MethodBaseType = Object( typeof( MethodBase ) );
		public static readonly TypeDefinition FieldInfoType = Object( typeof( FieldInfo ) );
		// ReSharper disable once InconsistentNaming
		public static readonly TypeDefinition IListOfStringType = Object( typeof( IList<string> ) );
		public static readonly TypeDefinition DictionaryOfStringAndTypeType = Object( typeof( Dictionary<string, Type> ) );
		public static readonly TypeDefinition MessagePackObjectType = Object( typeof( MessagePackObject ) );
		public static readonly TypeDefinition PackerType = Object( typeof( Packer ) );
		public static readonly TypeDefinition UnpackerType = Object( typeof( Unpacker ) );
		public static readonly TypeDefinition PackHelpersType = Object( typeof( PackHelpers ) );
		public static readonly TypeDefinition UnpackHelpersType = Object( typeof( UnpackHelpers ) );
		public static readonly TypeDefinition NilImplicationType = Object( typeof( NilImplication ) );
		public static readonly TypeDefinition SerializationMethodType = Object( typeof( SerializationMethod ) );
		public static readonly TypeDefinition EnumSerializationMethodType = Object( typeof( EnumSerializationMethod ) );
		public static readonly TypeDefinition EnumMemberSerializationMethodType = Object( typeof( EnumMemberSerializationMethod ) );
		public static readonly TypeDefinition DateTimeConversionMethodType = Object( typeof( DateTimeConversionMethod ) );
		public static readonly TypeDefinition DateTimeMemberConversionMethodType = Object( typeof( DateTimeMemberConversionMethod ) );
		public static readonly TypeDefinition PackingOptionsType = Object( typeof( PackingOptions ) );
		public static readonly TypeDefinition PolymorphismSchemaType = Object( typeof( PolymorphismSchema ) );
		public static readonly TypeDefinition PolymorphismSchemaArrayType = Array( PolymorphismSchemaType );
#if FEATURE_TAP
		public static readonly TypeDefinition CancellationTokenType = Object( typeof( CancellationToken ) );
		public static readonly TypeDefinition TaskType = Object( typeof( Task ) );
#endif // FEATURE_TAP

		private readonly Flags _flags;

		public bool IsArray
		{
			get { return ( this._runtimeType != null && this._runtimeType.IsArray ) || ( this._flags & Flags.Array ) != 0; }
		}

		public bool IsValueType
		{
			get { return ( this._runtimeType != null && this._runtimeType.GetIsValueType() ) || ( this._flags & Flags.ValueType ) != 0; }
		}

#if DEBUG
		public bool IsRef
		{
			get { return ( this._runtimeType != null && this._runtimeType.IsByRef ) || ( this._flags & Flags.Ref ) != 0; }
		}
#endif // DEBUG

		public readonly string TypeName;

		private readonly Type _runtimeType;
		private Type _resolvedType;

		public Type TryGetRuntimeType()
		{
			return this._runtimeType;
		}

		public bool HasRuntimeTypeFully()
		{
			return this._runtimeType != null && this.GenericArguments.All( t => t.HasRuntimeTypeFully() );
		}

		public Type ResolveRuntimeType()
		{
			return
				this._resolvedType
				?? this.ResolveRuntimeType( true );
		}

		private Type ResolveRuntimeType( bool throws )
		{
			if ( this._runtimeType == null && this.ElementType == null )
			{
				if ( throws )
				{
					throw new InvalidOperationException(
						String.Format( CultureInfo.CurrentCulture, "'{0}' does not have runtime type yet.", this.TypeName )
					);
				}
				else
				{
					return null;
				}
			}

			var resolvingType = this._runtimeType ?? this.ElementType.TryGetRuntimeType() ?? this.ElementType.ResolveRuntimeType( false );
			if ( resolvingType == null )
			{
				return null;
			}

			if ( resolvingType.GetIsGenericTypeDefinition() )
			{
				var arguments = this.GenericArguments.Select( t => t.ResolveRuntimeType( throws ) ).ToArray();
				if ( arguments.Any( a => a == null ) )
				{
					if ( throws )
					{
						throw new InvalidOperationException(
							String.Format(
								CultureInfo.CurrentCulture,
								"'{0}' have generic arguments which do not have runtime type yet.",
								this.TypeName
							)
						);
					}
					else
					{
						return null;
					}
				}

				resolvingType = resolvingType.MakeGenericType( arguments );
			}

			if ( ( this._flags & Flags.Array ) != 0 )
			{
				resolvingType = resolvingType.MakeArrayType();
			}

			if ( ( this._flags & Flags.Ref ) != 0 )
			{
				resolvingType = resolvingType.MakeByRefType();
			}

			this._resolvedType = resolvingType;
			return resolvingType;
		}


		public readonly TypeDefinition[] GenericArguments;

		public readonly TypeDefinition ElementType;

		private TypeDefinition( Type runtimeType, string name, TypeDefinition elementType, Flags flags, params TypeDefinition[] genericArguments )
		{
#if DEBUG
			Contract.Assert(
				runtimeType == null || !runtimeType.GetIsGenericTypeDefinition() || runtimeType.GetGenericTypeParameters().Length == genericArguments.Length,
				runtimeType?.GetFullName() + " == <" + String.Join( ", ", genericArguments.Select( t => t.ToString() ).ToArray() ) + ">"
			);
#endif // DEBUG

			this.TypeName = name;
			this._runtimeType = runtimeType;
			this._flags = flags;
			this.ElementType = elementType;
			this.GenericArguments = genericArguments ?? EmptyArray;
		}

		public static TypeDefinition Object( Type type )
		{
			return new TypeDefinition( type, type.FullName, type.IsArray ? Object( type.GetElementType() ) : null, Flags.HasRuntimeType );
		}

		public static TypeDefinition Object( string name )
		{
			return new TypeDefinition( null, name, null, Flags.None );
		}

		public static TypeDefinition GenericValueType( Type definition, params TypeDefinition[] arguments )
		{
			return Generic( true, definition, arguments );
		}

		public static TypeDefinition GenericReferenceType( Type definition, params TypeDefinition[] arguments )
		{
			return Generic( false, definition, arguments );
		}

		private static TypeDefinition Generic( bool isValueType, Type definition, TypeDefinition[] arguments )
		{
#if DEBUG
			Contract.Assert( definition.FullName.Contains( "`" ), definition.FullName + " does not have ` mark." );
#endif // DEBUG
			return
				new TypeDefinition(
					definition,
					definition.FullName.Remove( definition.FullName.IndexOf( '`' ) ),
					null,
					isValueType ? Flags.ValueType : Flags.None,
					arguments
				);

		}

		public static TypeDefinition Array( TypeDefinition elementType )
		{
			return
				elementType.HasRuntimeTypeFully()
				? new TypeDefinition( elementType.ResolveRuntimeType().MakeArrayType(), elementType.ResolveRuntimeType().FullName + "[]", elementType, Flags.HasRuntimeType )
				: new TypeDefinition( null, elementType.TypeName, elementType, Flags.Array );
		}

		public static TypeDefinition ManagedReference( TypeDefinition elementType )
		{
			return
				elementType.HasRuntimeTypeFully()
				? new TypeDefinition( elementType.ResolveRuntimeType().MakeByRefType(), elementType.ResolveRuntimeType().FullName, elementType, Flags.HasRuntimeType )
				: new TypeDefinition( null, elementType.TypeName, elementType, Flags.Ref );
		}

		public override string ToString()
		{
			return
				this._runtimeType != null
					? ( ( this.ResolveRuntimeType( false ) ?? this._runtimeType ) ).GetFullName()
					: ( this._flags & Flags.Array ) != 0
					? ( this.TypeName + "[]" )
					: ( this._flags & Flags.Ref ) != 0
					? ( this.TypeName + "&" )
					: this.TypeName;
		}

		public static implicit operator TypeDefinition( Type type )
		{
			return Object( type );
		}

		[Flags]
		private enum Flags
		{
			None = 0,
			Array = 0x1,
			ValueType = 0x2,
			Ref = 0x4,
			HasRuntimeType = unchecked( ( int )0x80000000 )
		}
	}
}