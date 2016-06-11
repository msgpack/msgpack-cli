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
#if CORE_CLR
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR
using System.Globalization;
using System.Linq;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.AbstractSerializers
{
	/// <summary>
	///		Represents type which may not have built metadata.
	/// </summary>
	internal class TypeDefinition
	{
		private static readonly TypeDefinition[] EmptyArray = new TypeDefinition[ 0 ];

		private readonly Flags _flags;

		public bool IsArray
		{
			get { return ( this._runtimeType != null && this._runtimeType.IsArray ) || ( this._flags & Flags.Array ) != 0; }
		}

		public bool IsValueType
		{
			get { return ( this._runtimeType != null && this._runtimeType.GetIsValueType() ) || ( this._flags & Flags.ValueType ) != 0; }
		}

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
			if ( this._runtimeType == null )
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

			if ( this._runtimeType.GetIsGenericTypeDefinition() )
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

				this._resolvedType = this._runtimeType.MakeGenericType( arguments );
			}
			else
			{
				this._resolvedType = this._runtimeType;
			}

			return this._resolvedType;
		}


		public readonly TypeDefinition[] GenericArguments;

		public readonly TypeDefinition ElementType;

		private TypeDefinition( Type runtimeType, string name, TypeDefinition elementType, Flags flags, params TypeDefinition[] genericArguments )
		{
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
				? new TypeDefinition( elementType.ResolveRuntimeType().MakeArrayType(), elementType.ResolveRuntimeType().FullName, elementType, Flags.HasRuntimeType )
				: new TypeDefinition( null, elementType.TypeName, elementType, Flags.Array );
		}

		public override string ToString()
		{
			return
				this._runtimeType != null
					? ( ( this.ResolveRuntimeType( false ) ?? this._runtimeType ) ).GetFullName()
					: ( this._flags & Flags.Array ) != 0
						? ( this.TypeName + "[]" )
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
			HasRuntimeType = unchecked( ( int )0x80000000 )
		}
	}
}