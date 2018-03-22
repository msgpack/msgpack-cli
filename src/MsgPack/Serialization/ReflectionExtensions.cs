#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2018 FUJIWARA, Yusuke and contributors
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
// Contributors:
//    Samuel Cragg
//
#endregion -- License Terms --

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Globalization;
using System.Reflection;

namespace MsgPack.Serialization
{
#if UNITY && DEBUG
	public
#else
	internal
#endif
	static partial class ReflectionExtensions
	{
		private static readonly Type[] ExceptionConstructorWithInnerParameterTypes = { typeof( string ), typeof( Exception ) };
		private static readonly Type[] ObjectAddParameterTypes = { typeof( object ) };

		public static Type[] GetParameterTypes( this MethodBase source )
		{
			var parameters = source.GetParameters();
			Type[] parameterTypes = new Type[ parameters.Length ];
			for ( var i = 0; i < parameters.Length; i++ )
			{
				parameterTypes[ i ] = parameters[ i ].ParameterType;
			}

			return parameterTypes;
		}

		public static Type GetMemberValueType( this MemberInfo source )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

#if !NETFX_CORE && !NETSTANDARD1_1 && !NETSTANDARD1_3
			var asType = source as Type;
			if ( asType != null )
			{
				// Nested type.
				return asType;
			}
#else
#if DEBUG
			Contract.Assert( typeof( MemberInfo ).IsAssignableFrom( typeof( Type ) ), "Type is assginable to MemberInfo on this platform, so should not step in this line." );
			Contract.Assert( typeof( Type ).IsAssignableFrom( typeof( TypeInfo ) ), "TypeInfo is assginable to Type on this platform, so should not step in this line." );
#endif // DEBUG
			var asTypeInfo = source as TypeInfo;
			if ( asTypeInfo != null )
			{
				// Nested type.
				return asTypeInfo.AsType();
			}
#endif // !NETFX_CORE

			var asProperty = source as PropertyInfo;
			var asField = source as FieldInfo;

			if ( asProperty == null && asField == null )
			{
				throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "'{0}'({1}) is not field nor property.", source, source.GetType() ) );
			}

			return asProperty != null ? asProperty.PropertyType : asField.FieldType;
		}
	}
}
