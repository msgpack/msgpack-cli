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
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct, TObject>
	{
		private void BuildEnumSerializer( TContext context )
		{
			var underyingType = Enum.GetUnderlyingType( typeof( TObject ) );
			context.BaseType =
				typeof( EnumSerializer<,> ).MakeGenericType( typeof( TObject ), underyingType );

			this.BuildPackUnderlyingValueTo( context, underyingType );
			this.BuildGetUnderlyingValueString( context, underyingType );
			this.BuildUnpackFromUnderlyingValue( context, underyingType );
			this.BuildTryParse( context, underyingType );
		}

		private void BuildPackUnderlyingValueTo( TContext context, Type underyingType )
		{
			this.EmitMethodPrologue(
				context,
				"PackUnderyingValueTo",
				typeof( void ),
				Tuple.Create( "packer", typeof( Packer ) ),
				Tuple.Create( "enumValue", typeof( EnumSerializationMethod ) )
			);

			TConstruct construct = null;
			try
			{
				construct =
					this.EmitInvokeVoidMethod(
						context,
						this.ReferArgument( "packer", 1 ),
						typeof( Packer ).GetMethod( "Pack", new[] { underyingType } ),
						this.EmitEnumToUnderlyingCastExpression( context, underyingType, this.ReferArgument( "enumValue", 2 ) )
					);
			}
			finally
			{
				this.EmitMethodEpilogue(
					context,
					construct
				);
			}
		}

		private void BuildGetUnderlyingValueString( TContext context, Type underyingType )
		{
			this.EmitMethodPrologue(
				context,
				"GetUnderlyingValueString",
				typeof( string ),
				Tuple.Create( "enumValue", typeof( EnumSerializationMethod ) )
			);

			TConstruct construct = null;
			try
			{
				construct =
					this.EmitInvokeMethodExpression(
						context,
						this.ReferArgument( "enumValue", 1 ),
						underyingType.GetMethod( "ToString", RefelectionExtensions.EmptyTypes )
					);
			}
			finally
			{
				this.EmitMethodEpilogue(
					context,
					construct
				);
			}
		}

		private void BuildUnpackFromUnderlyingValue( TContext context, Type underyingType )
		{
			this.EmitMethodPrologue(
				context,
				"GetUnderlyingValueString",
				typeof( TObject ),
				Tuple.Create( "messagePackObject", typeof( MessagePackObject ) )
			);

			TConstruct construct = null;
			try
			{
				construct =
					this.EmitEnumFromUnderlyingCastExpression(
						context,
						underyingType,
						this.EmitInvokeMethodExpression(
							context,
							this.ReferArgument( "messagePackObject", 1 ),
							typeof( MessagePackObject ).GetMethod( "As" + underyingType.Name, RefelectionExtensions.EmptyTypes )
						)
					);
			}
			finally
			{
				this.EmitMethodEpilogue(
					context,
					construct
				);
			}
		}

		private void BuildTryParse( TContext context, Type underyingType )
		{
			this.EmitMethodPrologue(
				context,
				"TryParse",
				typeof( bool ),
				Tuple.Create( "value", typeof( string ) ),
				Tuple.Create( "underlying", underyingType.MakeByRefType() )
			);

			TConstruct construct = null;
			try
			{
				construct =
					this.EmitRetrunStatement(
						context,
						this.EmitInvokeMethodExpression(
							context,
							null,
							underyingType.GetMethod( "TryParse", new[] { typeof( string ), underyingType.MakeByRefType() } ),
							this.ReferArgument( "value", 1 ),
							this.ReferArgument( "underying", 2 )
						)
					);
			}
			finally
			{
				this.EmitMethodEpilogue(
					context,
					construct
				);
			}
		}

		private TConstruct BuildCreateEnumSerializerInstance( TContext context, SerializingMember member )
		{
			EnumSerializationMethod method = context.SerializationContext.EnumSerializationMethod;
			var attributesOnMember = member.Member.GetCustomAttributes( typeof( MessagePackEnumMemberAttribute ), true );
			if ( attributesOnMember.Length > 0 )
			{
				switch ( ( attributesOnMember[ 0 ] as MessagePackEnumMemberAttribute ).SerializationMethod )
				{
					case EnumMemberSerializationMethod.ByName:
					{
						method = EnumSerializationMethod.ByName;
						break;
					}
					case EnumMemberSerializationMethod.ByUnderlyingValue:
					{
						method = EnumSerializationMethod.ByUnderlyingValue;
						break;
					}
					default:
					{
						var attributesOnType = member.Member.GetMemberValueType().GetCustomAttributes( typeof( MessagePackEnumAttribute ), false );
						if ( attributesOnType.Length > 0 )
						{
							method = ( attributesOnMember[ 0 ] as MessagePackEnumAttribute ).SerializationMethod;
						}

						break;
					}
				}
			}

			return
				this.BuildCreateEnumSerializerInstance(
					context,
					member.Member.GetMemberValueType(),
					method,
					GetNameValueMap(),
					GetValueNameMap()
				);
		}

		protected abstract TConstruct BuildCreateEnumSerializerInstance(
			TContext context,
			Type type,
			EnumSerializationMethod method,
			IDictionary<string, TObject> nameValueMap,
			IDictionary<TObject, string> valuenameMap
		);

		private static IDictionary<string, TObject> GetNameValueMap()
		{
			return
				Enum.GetValues( typeof( TObject ) )
					.Cast<TObject>()
					.ToDictionary( e => e.ToString() );
		}

		private static IDictionary<TObject, string> GetValueNameMap()
		{
			var result = new Dictionary<TObject, string>();
			foreach ( TObject entry in Enum.GetValues( typeof( TObject ) ) )
			{
				result[ entry ] = entry.ToString();
			}

			return result;
		}
	}

	internal abstract class EnumSerializer<TEnum, TUnderlying> : MessagePackSerializer<TEnum>
	{
		private readonly EnumSerializationMethod _serializationMethod;
		private readonly IDictionary<TEnum, string> _valueNameMap;
		private readonly IDictionary<string, TEnum> _nameValueMap;

		protected EnumSerializer( PackerCompatibilityOptions options, EnumSerializationMethod serializationMethod, IDictionary<TEnum, string> valueNameMap, IDictionary<string, TEnum> nameValueMap )
			: base( options )
		{
			this._serializationMethod = serializationMethod;
			this._valueNameMap = valueNameMap;
			this._nameValueMap = nameValueMap;
		}

		protected internal override void PackToCore( Packer packer, TEnum objectTree )
		{
			if ( this._serializationMethod == EnumSerializationMethod.ByUnderlyingValue )
			{
				PackUnderlyingValueTo( packer, objectTree );
			}
			else
			{
				string asString;
				if ( !this._valueNameMap.TryGetValue( objectTree, out asString ) )
				{
					asString = this.GetUnderlyingValueString( objectTree );
				}

				packer.PackString( asString );
			}
		}

		protected abstract void PackUnderlyingValueTo( Packer packer, TEnum enumValue );
		protected abstract string GetUnderlyingValueString( TEnum enumValue );

		protected internal override TEnum UnpackFromCore( Unpacker unpacker )
		{
			if ( unpacker.LastReadData.IsRaw )
			{
				var asString = unpacker.LastReadData.AsString();
				TEnum result;
				if ( !this._nameValueMap.TryGetValue( asString, out result ) )
				{
					TUnderlying underlying;
					if ( !this.TryParse( asString, out underlying ) )
					{
						throw new SerializationException(
							String.Format(
								CultureInfo.CurrentCulture,
								"Name '{0}' is not member of enum type '{1}'.",
								asString,
								typeof( TEnum )
								)
							);
					}
				}

				return result;
			}
			else if ( unpacker.LastReadData.IsTypeOf<TUnderlying>().GetValueOrDefault() )
			{
				return UnpackFromUnderlyingValue( unpacker.LastReadData );
			}
			else
			{
				throw new SerializationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Type '{0}' is not underlying type of enum type '{1}'.",
						unpacker.LastReadData.UnderlyingType,
						typeof( TEnum )
					)
				);
			}
		}

		protected abstract TEnum UnpackFromUnderlyingValue( MessagePackObject messagePackObject );
		protected abstract bool TryParse( string value, out TUnderlying underlying );
	}


	// Dummy
	/*
	internal sealed class EnumSerializationMethodSerializer : EnumSerializer<EnumSerializationMethod, int>
	{
		public EnumSerializationMethodSerializer( PackerCompatibilityOptions options, EnumSerializationMethod serializationMethod, IDictionary<EnumSerializationMethod, string> valueNameMap, IDictionary<string, EnumSerializationMethod> nameValueMap )
			: base( options, serializationMethod, valueNameMap, nameValueMap ) { }

		protected override void PackUnderlyingValueTo( Packer packer, EnumSerializationMethod enumValue )
		{
			packer.Pack( this.ToUnderlyingValue( enumValue ) );
		}

		protected override string GetUnderlyingValueString( EnumSerializationMethod enumValue )
		{
			return this.ToUnderlyingValue( enumValue ).ToString();
		}

		private int ToUnderlyingValue( EnumSerializationMethod enumValue )
		{
			return ( int )enumValue;
		}

		protected override EnumSerializationMethod UnpackFromUnderlyingValue( MessagePackObject messagePackObject )
		{
			return this.FromUnderlyingValue( messagePackObject.AsInt32() );
		}

		protected override bool TryParse( string value, out int underlying )
		{
			return Int32.TryParse( value, out underlying );
		}

		private EnumSerializationMethod FromUnderlyingValue( int underlying )
		{
			return ( EnumSerializationMethod )underlying;
		}
	}
	 */

	public enum EnumSerializationMethod
	{
		ByName = 0,
		ByUnderlyingValue
	}

	public enum EnumMemberSerializationMethod
	{
		Default = 0,
		ByName,
		ByUnderlyingValue
	}

	[AttributeUsage( AttributeTargets.Enum, Inherited = false, AllowMultiple = false )]
	public sealed class MessagePackEnumAttribute : Attribute
	{
		public EnumSerializationMethod SerializationMethod { get; set; }

		public MessagePackEnumAttribute() { }
	}

	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false )]
	public sealed class MessagePackEnumMemberAttribute : Attribute
	{
		public EnumMemberSerializationMethod SerializationMethod { get; set; }

		public MessagePackEnumMemberAttribute() { }
	}
}