#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2015 FUJIWARA, Yusuke
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
using System.Globalization;

namespace MsgPack.Serialization.ReflectionSerializers
{
	/// <summary>
	///		Implements reflection-based enum serializer for restricted platforms.
	/// </summary>
#if !UNITY
	internal class ReflectionEnumMessagePackSerializer<T> : EnumMessagePackSerializer<T>
		where T : struct
#else
	internal class ReflectionEnumMessagePackSerializer : UnityEnumMessagePackSerializer
#endif // !UNITY
	{
#if !UNITY
		public ReflectionEnumMessagePackSerializer( SerializationContext context )
			: base(
				context, 
				EnumMessagePackSerializerHelpers.DetermineEnumSerializationMethod(
					context,
					typeof( T ),
					EnumMemberSerializationMethod.Default
				)
			) { }
#else
		public ReflectionEnumMessagePackSerializer( SerializationContext context, Type targetType )
			: base(
				context,
				targetType,
				EnumMessagePackSerializerHelpers.DetermineEnumSerializationMethod(
					context,
					targetType,
					EnumMemberSerializationMethod.Default
				)
			) { }
#endif // !UNITY

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
#if !UNITY
		protected internal override void PackUnderlyingValueTo( Packer packer, T enumValue )
#else
		protected internal override void PackUnderlyingValueTo( Packer packer, object enumValue )
#endif // !UNITY
		{
			packer.Pack( UInt64.Parse( ( ( IFormattable ) enumValue ).ToString( "D", CultureInfo.InvariantCulture ), CultureInfo.InvariantCulture ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
#if !UNITY
		protected internal override T UnpackFromUnderlyingValue( MessagePackObject messagePackObject )
		{
			return ( T )Enum.Parse( typeof( T ), messagePackObject.ToString(), false );
		}
#else
		protected internal override object UnpackFromUnderlyingValue( MessagePackObject messagePackObject )
		{
			return Enum.Parse( this.TargetType, messagePackObject.ToString(), false );
		}
#endif // !UNITY
	}
}
