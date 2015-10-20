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
// Contributors:
//    Takeshi KIRIYA
//
#endregion -- License Terms --

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using MsgPack.Serialization.DefaultSerializers;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Implements serialization target member extraction logics.
	/// </summary>
	internal class SerializationTarget
	{
		public IList<SerializingMember> Members { get; private set; }
		public ConstructorInfo DeserializationConstructor { get; private set; }
		public bool IsConstructorDeserialization 
		{
			get { return this.DeserializationConstructor != null && this.DeserializationConstructor.GetParameters().Length > 0; }
		}

		private SerializationTarget( IList<SerializingMember> members, ConstructorInfo constructor )
		{
			this.Members = members;
			this.DeserializationConstructor = constructor;
		}

		public string FindCorrespondingMemberName( ParameterInfo parameterInfo )
		{
			return
				this.Members.Where(
					item =>
						parameterInfo.Name.Equals( item.Contract.Name, StringComparison.OrdinalIgnoreCase ) &&
						item.Member.GetMemberValueType() == parameterInfo.ParameterType
				).Select( item => item.Contract.Name )
				.FirstOrDefault();
		}

		public static void VerifyType( Type targetType )
		{
			if ( targetType.GetIsInterface() || targetType.GetIsAbstract() )
			{
				throw SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractType( targetType );
			}
		}

		public static SerializationTarget Prepare( SerializationContext context, Type targetType )
		{
			var getters = GetTargetMembers( targetType ).OrderBy( entry => entry.Contract.Id ).ToArray();

			if ( getters.Length == 0 )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot serialize type '{0}' because it does not have any serializable fields nor properties.", targetType ) );
			}

			var memberCandidates = getters.Where( entry => CheckTargetEligibility( entry.Member ) ).ToArray();

			if ( memberCandidates.Length == 0 )
			{
				var constructor = FindDeserializationConstructor( targetType );
				return new SerializationTarget( ComplementMembers( getters, context, targetType ), constructor );
			}

			var defaultConstructor = targetType.GetConstructor( ReflectionAbstractions.EmptyTypes );
			if ( defaultConstructor == null && !targetType.GetIsValueType() )
			{
				throw SerializationExceptions.NewTargetDoesNotHavePublicDefaultConstructor( targetType );
			}

			// Because members' order is equal to declared order is NOT guaranteed, so explicit ordering is required.
			IList<SerializingMember> members;
			if ( memberCandidates.All( item => item.Contract.Id == DataMemberContract.UnspecifiedId ) )
			{
				// Alphabetical order.
				members = memberCandidates.OrderBy( item => item.Contract.Name ).ToArray();
			}
			else
			{
				// ID order.
				members = ComplementMembers( memberCandidates, context, targetType );
			}

			return new SerializationTarget( members, defaultConstructor );
		}

		private static IEnumerable<SerializingMember> GetTargetMembers( Type type )
		{
#if DEBUG && !UNITY
			Contract.Assert( type != null, "type != null" );
#endif // DEBUG && !UNITY
#if !NETFX_CORE
			var members =
				type.FindMembers(
					MemberTypes.Field | MemberTypes.Property,
					BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
					null,
					null
				);
#else
			var members =
				type.GetRuntimeFields().Where( f => !f.IsStatic ).OfType<MemberInfo>()
					.Concat( type.GetRuntimeProperties().Where( p => p.GetMethod != null && !p.GetMethod.IsStatic ) )
					.ToArray();
#endif
			var filtered = members.Where( item => item.IsDefined( typeof( MessagePackMemberAttribute ) ) ).ToArray();

			if ( filtered.Length > 0 )
			{
				return GetAnnotatedMembersWithDuplicationDetection( type, filtered );
			}

			if ( type.GetCustomAttributesData().Any( attr =>
				attr.GetAttributeType().FullName == "System.Runtime.Serialization.DataContractAttribute" ) )
			{
				return GetSystemRuntimeSerializationCompatibleMembers( members );
			}

			return GetPublicUnpreventedMembers( members );
		}

		private static IEnumerable<SerializingMember> GetAnnotatedMembersWithDuplicationDetection( Type type, MemberInfo[] filtered )
		{
			var duplicated =
				filtered.FirstOrDefault(
					member => member.IsDefined( typeof( MessagePackIgnoreAttribute ) )
					);

			if ( duplicated != null )
			{
				throw new SerializationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"A member '{0}' of type '{1}' is marked with both MessagePackMemberAttribute and MessagePackIgnoreAttribute.",
						duplicated.Name,
						type
						)
					);
			}

			return
				filtered.Select(
					member =>
						new SerializingMember(
							member,
							new DataMemberContract( member, member.GetCustomAttribute<MessagePackMemberAttribute>() )
							)
					);
		}

		private static IEnumerable<SerializingMember> GetSystemRuntimeSerializationCompatibleMembers( MemberInfo[] members )
		{
			return
				members.Select(
					item =>
						new
						{
							member = item,
							data = 
								item.GetCustomAttributesData()
								.FirstOrDefault(
									data => data.GetAttributeType().FullName == "System.Runtime.Serialization.DataMemberAttribute" 
								)
						}
					).Where( item => item.data != null )
					.Select(
						item =>
						{
							var name = 
								item.data.GetNamedArguments()
								.Where( arg => arg.GetMemberName() == "Name" )
								.Select( arg => ( string ) arg.GetTypedValue().Value )
								.FirstOrDefault();
							var id = 
								item.data.GetNamedArguments()
								.Where( arg => arg.GetMemberName() == "Order" )
#if !UNITY
								.Select( arg => ( int? )arg.GetTypedValue().Value )
#else
								.Select( arg => arg.GetTypedValue().Value )
#endif
								.FirstOrDefault();
#if SILVERLIGHT
							if ( id == -1 )
							{
								// Shim for Silverlight returns -1 because GetNamedArguments() extension method cannot recognize whether the argument was actually specified or not.
								id = null;
							}
#endif // SILVERLIGHT

							return
								new SerializingMember(
									item.member,
#if !UNITY
									new DataMemberContract( item.member, name, NilImplication.MemberDefault, id )
#else
									new DataMemberContract( item.member, name, NilImplication.MemberDefault, ( int? )id )
#endif // !UNITY
								);
						}
					);
		}

		private static IEnumerable<SerializingMember> GetPublicUnpreventedMembers( MemberInfo[] members )
		{
			return members.Where(
				member => member.GetIsPublic()
#if !SILVERLIGHT && !NETFX_CORE
						&& !Attribute.IsDefined( member, typeof( NonSerializedAttribute ) )
#endif // !SILVERLIGHT && !NETFX_CORE
						&& !member.IsDefined( typeof( MessagePackIgnoreAttribute ) ) 
				).Select( member => new SerializingMember( member, new DataMemberContract( member ) ) );
		}


		private static ConstructorInfo FindDeserializationConstructor( Type targetType )
		{
			var constructors = targetType.GetConstructors().ToArray();

			if ( constructors.Length == 0 )
			{
				throw NewTypeCannotBeSerializedException( targetType );
			}

			// The marked construtor is always preferred.
			var markedConstructors = constructors.Where( ctor => ctor.IsDefined( typeof( MessagePackDeserializationConstructorAttribute ) ) ).ToArray();
			switch ( markedConstructors.Length )
			{
				case 0:
				{
					break;
				}
				case 1:
				{
					// OK Use it
					return markedConstructors[ 0 ];
				}
				default:
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"There are multiple constructors marked with MessagePackDeserializationConstrutorAttribute in type '{0}'.",
							targetType
						)
					);
				}
			}

			// A constructor which has most parameters will be used.
			var mostRichConstructors =
				constructors.GroupBy( ctor => ctor.GetParameters().Length ).OrderByDescending( g => g.Key ).First().ToArray();
			switch ( mostRichConstructors.Length )
			{
				case 1:
				{
					if ( mostRichConstructors[ 0 ].GetParameters().Length == 0 )
					{
						throw NewTypeCannotBeSerializedException( targetType );
					}

					// OK Use it
					return mostRichConstructors[ 0 ];
				}
				default:
				{
					throw
						new SerializationException(
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot serialize type '{0}' because it does not have any serializable fields nor properties, and serializer generator failed to determine constructor to deserialize among({1}).",
								targetType,
								String.Join( ", ", mostRichConstructors.Select( ctor => ctor.ToString() ).ToArray() )
							)
						);
				}
			}
		}

		private static SerializationException NewTypeCannotBeSerializedException( Type targetType )
		{
			return new SerializationException(
				String.Format(
					CultureInfo.CurrentCulture,
					"Cannot serialize type '{0}' because it does not have any serializable fields nor properties, and it does not have any public constructors with parameters.",
					targetType
					) 
				);
		}


		private static bool CheckTargetEligibility( MemberInfo member )
		{
			var asProperty = member as PropertyInfo;
			var asField = member as FieldInfo;
			Type returnType;

			if ( asProperty != null )
			{
				if ( asProperty.GetIndexParameters().Length > 0 )
				{
					// Indexer cannot be target except the type itself implements IDictionary or IDictionary<TKey,TValue>
					return false;
				}

#if !NETFX_CORE
				if ( asProperty.GetSetMethod( true ) != null )
#else
				if ( asProperty.SetMethod != null )
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

		private static IList<SerializingMember> ComplementMembers( IList<SerializingMember> candidates, SerializationContext context, Type targetType )
		{
			if ( candidates[ 0 ].Contract.Id < 0 )
			{
				return candidates;
			}

			if ( context.CompatibilityOptions.OneBoundDataMemberOrder && candidates[ 0 ].Contract.Id == 0 )
			{
				throw new NotSupportedException(
					"Cannot specify order value 0 on DataMemberAttribute when SerializationContext.CompatibilityOptions.OneBoundDataMemberOrder is set to true." );
			}

#if !UNITY
			var maxId = candidates.Max( item => item.Contract.Id );
#else
			int maxId = -1;
			foreach ( var id in candidates.Select( item => item.Contract.Id ) )
			{
				maxId = Math.Max( id, maxId );
			}
#endif
			var result = new List<SerializingMember>( maxId + 1 );
			for ( int source = 0, destination = context.CompatibilityOptions.OneBoundDataMemberOrder ? 1 : 0;
				source < candidates.Count;
				source++, destination++ )
			{
#if DEBUG && !UNITY
				Contract.Assert( candidates[ source ].Contract.Id >= 0, "candidates[ source ].Contract.Id >= 0" );
#endif // DEBUG && !UNITY

				if ( candidates[ source ].Contract.Id < destination )
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"The member ID '{0}' is duplicated in the '{1}' elementType.",
							candidates[ source ].Contract.Id,
							targetType ) );
				}

				while ( candidates[ source ].Contract.Id > destination )
				{
					result.Add( new SerializingMember() );
					destination++;
				}

				result.Add( candidates[ source ] );
			}

			VerifyNilImplication( targetType, result );
			VerifyKeyUniqueness( result );
			return result;
		}

		private static void VerifyNilImplication( Type type, IEnumerable<SerializingMember> entries )
		{
			foreach ( var serializingMember in entries )
			{
				if ( serializingMember.Contract.NilImplication == NilImplication.Null )
				{
					var itemType = serializingMember.Member.GetMemberValueType();

					if ( itemType != typeof( MessagePackObject )
						&& itemType.GetIsValueType()
						&& Nullable.GetUnderlyingType( itemType ) == null )
					{
						throw SerializationExceptions.NewValueTypeCannotBeNull( serializingMember.Member.ToString(), itemType, type );
					}

					bool isReadOnly;
					FieldInfo asField;
					PropertyInfo asProperty;
					if ( ( asField = serializingMember.Member as FieldInfo ) != null )
					{
						isReadOnly = asField.IsInitOnly;
					}
					else
					{
						asProperty = serializingMember.Member as PropertyInfo;
#if DEBUG && !UNITY_IPHONE && !UNITY_ANDROID
						Contract.Assert( asProperty != null, serializingMember.Member.ToString() );
#endif
						isReadOnly = asProperty.GetSetMethod() == null;
					}

					if ( isReadOnly )
					{
						throw SerializationExceptions.NewNullIsProhibited( serializingMember.Member.ToString() );
					}
				}
			}
		}

		private static void VerifyKeyUniqueness( IList<SerializingMember> result )
		{
			var duplicated = new Dictionary<string, List<MemberInfo>>();
			var existents = new Dictionary<string, SerializingMember>();
			foreach ( var member in result )
			{
				if ( member.Contract.Name == null )
				{
					continue;
				}

				try
				{
					existents.Add( member.Contract.Name, member );
				}
				catch ( ArgumentException )
				{
					List<MemberInfo> list;
					if ( duplicated.TryGetValue( member.Contract.Name, out list ) )
					{
						list.Add( member.Member );
					}
					else
					{
						duplicated.Add( member.Contract.Name, new List<MemberInfo> { existents[ member.Contract.Name ].Member, member.Member } );
					}
				}
			}

			if ( duplicated.Count > 0 )
			{
				throw new InvalidOperationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Some member keys specified with custom attributes are duplicated. Details: {{{0}}}",
						String.Join(
							",",
							duplicated.Select(
								kv => String.Format(
									CultureInfo.CurrentCulture,
									"\"{0}\":[{1}]",
									kv.Key,
									String.Join( ",", kv.Value.Select( m => String.Format( CultureInfo.InvariantCulture, "{0}.{1}({2})", m.DeclaringType, m.Name, ( m is FieldInfo ) ? "Field" : "Property" ) ).ToArray() )
								)
							).ToArray()
						)
					)
				);
			}
		}

#if !NETFX_CORE && !SILVERLIGHT
		public static bool BuiltInSerializerExists( ISerializerGeneratorConfiguration configuration, Type type, CollectionTraits traits )
		{
			return GenericSerializer.IsSupported( type, traits, configuration.PreferReflectionBasedSerializer ) || SerializerRepository.InternalDefault.Contains( type );
		}
#endif // !NETFX_CORE && !SILVERLIGHT
	}
}
