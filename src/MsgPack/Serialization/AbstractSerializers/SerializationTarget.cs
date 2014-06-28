#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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
//    Takeshi KIRIYA
//
#endregion -- License Terms --

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization.AbstractSerializers
{
	/// <summary>
	///		Implements serialization target member extraction logics.
	/// </summary>
	internal static class SerializationTarget
	{
		public static IEnumerable<SerializingMember> GetTargetMembers( Type type )
		{
			Contract.Assert( type != null );
#if !NETFX_CORE
			var members =
				type.FindMembers(
					MemberTypes.Field | MemberTypes.Property,
					BindingFlags.Public | BindingFlags.Instance,
					( member, criteria ) => CheckTargetEligibility( member ),
					null
				);
			var filtered = members.Where( item => Attribute.IsDefined( item, typeof( MessagePackMemberAttribute ) ) ).ToArray();
#else
			var members =
				typeof( TObject ).GetRuntimeFields().Where( f => f.IsPublic && !f.IsStatic ).OfType<MemberInfo>()
					.Concat( typeof( TObject ).GetRuntimeProperties().Where( p => p.GetMethod != null && p.GetMethod.IsPublic && !p.GetMethod.IsStatic ) )
					.Where( CheckTargetEligibility );
			var filtered = members.Where( item => item.IsDefined( typeof( MessagePackMemberAttribute ) ) ).ToArray();
#endif

			if ( filtered.Length > 0 )
			{
				return
					filtered.Select( member =>
						new SerializingMember(
							member,
							new DataMemberContract( member, member.GetCustomAttribute<MessagePackMemberAttribute>() )
						)
					);
			}

			if ( type.GetCustomAttributesData().Any( attr =>
				attr.GetAttributeType().FullName == "System.Runtime.Serialization.DataContractAttribute" ) )
			{
				return
					members.Select(
						item =>
						new
						{
							member = item,
							data = item.GetCustomAttributesData()
								.FirstOrDefault(
									data => data.GetAttributeType().FullName == "System.Runtime.Serialization.DataMemberAttribute" )
						}
					).Where( item => item.data != null )
					.Select(
						item =>
						{
							var name = item.data.GetNamedArguments()
								.Where( arg => arg.GetMemberName() == "Name" )
								.Select( arg => ( string )arg.GetTypedValue().Value )
								.FirstOrDefault();
							var id = item.data.GetNamedArguments()
								.Where( arg => arg.GetMemberName() == "Order" )
								.Select( arg => ( int? )arg.GetTypedValue().Value )
								.FirstOrDefault();

							return
								new SerializingMember(
									item.member,
									new DataMemberContract( item.member, name, NilImplication.MemberDefault, id )
								);
						}
					);
			}
#if SILVERLIGHT || NETFX_CORE
			return members.Select( member => new SerializingMember( member, new DataMemberContract( member ) ) );
#else
			return
				members.Where( item => !Attribute.IsDefined( item, typeof( NonSerializedAttribute ) ) )
				.Select( member => new SerializingMember( member, new DataMemberContract( member ) ) );
#endif
		}

		private static bool CheckTargetEligibility( MemberInfo member )
		{
			var asProperty = member as PropertyInfo;
			var asField = member as FieldInfo;
			Type returnType;

			if ( asProperty != null )
			{
#if !NETFX_CORE
				if ( asProperty.GetSetMethod() != null )
#else
				if ( asProperty.SetMethod != null && asProperty.SetMethod.IsPublic )
#endif
				{
					return true;
				}

				returnType = asProperty.PropertyType;
			}
			else if ( asField != null )
			{
				if ( !asField.IsInitOnly )
				{
					return true;
				}

				returnType = asField.FieldType;
			}
			else
			{
				return true;
			}

			var traits = returnType.GetCollectionTraits();
			switch ( traits.CollectionType )
			{
				case CollectionKind.Array:
				case CollectionKind.Map:
				{
					return traits.AddMethod != null;
				}
				default:
				{
					return false;
				}
			}
		}
	}
}
