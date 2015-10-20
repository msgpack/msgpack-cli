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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.Polymorphic
{
	/// <summary>
	///		Implements polymorphic serializer which uses closed known types and interoperable ext-type feature.
	/// </summary>
	/// <typeparam name="T">The base type of the polymorhic member.</typeparam>
	internal sealed class KnownTypePolymorphicMessagePackSerializer<T> : MessagePackSerializer<T>, IPolymorphicDeserializer
	{
		private readonly PolymorphismSchema _schema;
		private readonly IDictionary<string, RuntimeTypeHandle> _typeHandleMap;
		private readonly IDictionary<RuntimeTypeHandle, string> _typeCodeMap;

		public KnownTypePolymorphicMessagePackSerializer( SerializationContext ownerContext, PolymorphismSchema schema )
			: base( ownerContext )
		{
			if ( typeof( T ).GetIsValueType() )
			{
				throw SerializationExceptions.NewValueTypeCannotBePolymorphic( typeof( T ) );
			}

			this._schema = schema.FilterSelf();
			this._typeHandleMap = BuildTypeCodeTypeHandleMap( schema.CodeTypeMapping );
			this._typeCodeMap = BuildTypeHandleTypeCodeMap( schema.CodeTypeMapping );
		}

		private static IDictionary<string, RuntimeTypeHandle> BuildTypeCodeTypeHandleMap( IDictionary<string, Type> typeMap )
		{
			return typeMap.ToDictionary( kv => kv.Key, kv => kv.Value.TypeHandle );
		}

		private static IDictionary<RuntimeTypeHandle, string> BuildTypeHandleTypeCodeMap( IDictionary<string, Type> typeMap )
		{
			var result = new Dictionary<RuntimeTypeHandle, string>( typeMap.Count );
			foreach ( var typeHandleTypeCodeMapping in typeMap.GroupBy( kv => kv.Value ) )
			{
				if ( typeHandleTypeCodeMapping.Count() > 1 )
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Type '{0}' is mapped to multiple extension type codes({1}).",
							typeHandleTypeCodeMapping.Key,
							String.Join(
								CultureInfo.CurrentCulture.TextInfo.ListSeparator,
								typeHandleTypeCodeMapping.Select( kv => kv.Key )
#if NETFX_35 || UNITY
								.Select( b => b.ToString( CultureInfo.InvariantCulture ) ).ToArray()
#endif // NETFX_35 || UNITY
 )
						)
					);
				}

				result.Add( typeHandleTypeCodeMapping.Key.TypeHandle, typeHandleTypeCodeMapping.First().Key );
			}

			return result;
		}

		private IMessagePackSerializer GetActualTypeSerializer( Type actualType )
		{
			var result = this.OwnerContext.GetSerializer( actualType, this._schema );
			if ( result == null )
			{
				throw new SerializationException(
					String.Format( CultureInfo.CurrentCulture, "Cannot get serializer for actual type {0} from context.", actualType )
				);
			}

			return result;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes", Justification = "Avoided via ownsStream: false" )]
		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			string typeCode;
			if ( !this._typeCodeMap.TryGetValue( objectTree.GetType().TypeHandle, out typeCode ) )
			{
				throw new SerializationException( 
					String.Format( 
						CultureInfo.CurrentCulture, 
						"Type '{0}' in assembly '{1}' is not defined as known types.",
						objectTree.GetType().GetFullName(), 
						objectTree.GetType().GetAssembly()
					) 
				);
			}

			TypeInfoEncoder.Encode( packer, this._typeCodeMap[ objectTree.GetType().TypeHandle ] );
			this.GetActualTypeSerializer( objectTree.GetType() ).PackTo( packer, objectTree );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			return
				TypeInfoEncoder.Decode(
					unpacker,
					u =>
					{
						var typeCode = u.LastReadData.AsString();

						RuntimeTypeHandle typeHandle;
						if ( !this._typeHandleMap.TryGetValue( typeCode, out typeHandle ) )
						{
							throw new SerializationException(
								String.Format( CultureInfo.CurrentCulture, "Unknown type {0}.", StringEscape.ForDisplay( typeCode ) )
							);
						}

						return Type.GetTypeFromHandle( typeHandle );
					},
					( t, u ) => ( T ) this.GetActualTypeSerializer( t ).UnpackFrom( u )
				);
		}

		object IPolymorphicDeserializer.PolymorphicUnpackFrom( Unpacker unpacker )
		{
			return this.UnpackFromCore( unpacker );
		}

		protected internal override void UnpackToCore( Unpacker unpacker, T collection )
		{
			this.GetActualTypeSerializer( collection.GetType() ).UnpackTo( unpacker, collection );
		}
	}
}