#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2018 FUJIWARA, Yusuke and contributors
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
//    Odyth
//    Roman-Blinkov
//    Samuel Cragg
//
#endregion -- License Terms --

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
using System.Diagnostics;
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
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
#if UNITY && DEBUG
	public
#else
	internal
#endif
	class SerializationTarget
	{
		// Type names to avoid user who doesn't embed "message pack assembly's attributes" in their code directly.
		private static readonly string MessagePackMemberAttributeTypeName = typeof( MessagePackMemberAttribute ).FullName;
		private static readonly string MessagePackIgnoreAttributeTypeName = typeof( MessagePackIgnoreAttribute ).FullName;
		private static readonly string MessagePackDeserializationConstructorAttributeTypeName = typeof( MessagePackDeserializationConstructorAttribute ).FullName;
		private static readonly string[] EmptyStrings = new string[ 0 ];
		private static readonly SerializingMember[] EmptyMembers = new SerializingMember[ 0 ];
		private static readonly Assembly ThisAssembly = typeof( SerializationTarget ).GetAssembly();

		public IList<SerializingMember> Members { get; private set; }

		public ConstructorInfo DeserializationConstructor { get; private set; }

		private readonly string[] _correspondingMemberNames;

		public bool IsConstructorDeserialization { get; private set; }

		public bool CanDeserialize { get; private set; }

		private SerializationTarget( IList<SerializingMember> members, ConstructorInfo constructor, string[] correspondingMemberNames, bool canDeserialize )
		{
			Trace( "SerializationTarget::ctor(canDeserialize: {0})", canDeserialize );
			this.Members = members;
			this.DeserializationConstructor = constructor;
			this.IsConstructorDeserialization = constructor != null && constructor.GetParameters().Any();
			this.CanDeserialize = canDeserialize;
			this._correspondingMemberNames = correspondingMemberNames ?? EmptyStrings;
		}

		public SerializerCapabilities GetCapabilitiesForObject()
		{
			return this.CanDeserialize ? ( SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) : SerializerCapabilities.PackTo;
		}

		public SerializerCapabilities GetCapabilitiesForCollection( CollectionTraits traits )
		{
			return
				!this.CanDeserialize
					? SerializerCapabilities.PackTo
					: traits.AddMethod == null
						? ( SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom )
						: ( SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom | SerializerCapabilities.UnpackTo );
		}

		private static string[] FindCorrespondingMemberNames( IList<SerializingMember> members, ConstructorInfo constructor )
		{
			if ( constructor == null )
			{
				return null;
			}

			var constructorParameters = constructor.GetParameters();
			return
					constructorParameters.GroupJoin(
						members,
						p => new KeyValuePair<string, Type>( p.Name, p.ParameterType ),
						m => new KeyValuePair<string, Type>( m.Contract.Name, m.Member == null ? null : m.Member.GetMemberValueType() ),
						( p, ms ) => DetermineCorrespondingMemberName( p, ms ),
						MemberConstructorParameterEqualityComparer.Instance
					).ToArray();
		}

		private static string DetermineCorrespondingMemberName( ParameterInfo parameterInfo, IEnumerable<SerializingMember> members )
		{
			var membersArray = members.ToArray();
			switch ( membersArray.Length )
			{
				case 0:
					{
						return null;
					}
				case 1:
					{
						return membersArray[ 0 ].MemberName;
					}
				default:
					{
						ThrowAmbigiousMatchException( parameterInfo, membersArray );
						return null;
					}
			}
		}

		private static void ThrowAmbigiousMatchException( ParameterInfo parameterInfo, ICollection<SerializingMember> members )
		{
			throw new AmbiguousMatchException(
				String.Format(
					CultureInfo.CurrentCulture,
					"There are multiple candiates for corresponding member for parameter '{0}' of constructor. [{1}]",
					parameterInfo,
					String.Join( ", ", members.Select( x => x.ToString() ).ToArray() )
				)
			);
		}

		public string GetCorrespondingMemberName( int constructorParameterIndex )
		{
			return this._correspondingMemberNames[ constructorParameterIndex ];
		}

		public static void VerifyType( Type targetType )
		{
			if ( targetType.GetIsInterface() || targetType.GetIsAbstract() )
			{
				throw SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractType( targetType );
			}
		}

		public static void VerifyCanSerializeTargetType( SerializationContext context, Type targetType )
		{
			if ( context.SerializerOptions.DisablePrivilegedAccess && !targetType.GetIsPublic() && !targetType.GetIsNestedPublic() && !ThisAssembly.Equals( targetType.GetAssembly() ) )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot serialize type '{0}' because it is not public to the serializer.", targetType ) );
			}
		}

		public static SerializationTarget Prepare( SerializationContext context, Type targetType )
		{
			VerifyCanSerializeTargetType( context, targetType );

			IEnumerable<string> memberIgnoreList = context.BindingOptions.GetIgnoringMembers( targetType );
			var getters = GetTargetMembers( targetType )
						  .Where( getter => !memberIgnoreList.Contains( getter.MemberName, StringComparer.Ordinal ) )
						  .OrderBy( entry => entry.Contract.Id )
						  .ToArray();

			if ( getters.Length == 0
				&& !typeof( IPackable ).IsAssignableFrom( targetType )
				&& !typeof( IUnpackable ).IsAssignableFrom( targetType )
#if FEATURE_TAP
				&& ( context.SerializerOptions.WithAsync
					&& ( !typeof( IAsyncPackable ).IsAssignableFrom( targetType )
						&& !typeof( IAsyncUnpackable ).IsAssignableFrom( targetType )
					)
				)
#endif // FEATURE_TAP
			)
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot serialize type '{0}' because it does not have any serializable fields nor properties.", targetType ) );
			}

			var memberCandidates = getters.Where( entry => CheckTargetEligibility( context, entry.Member ) ).ToArray();

			if ( memberCandidates.Length == 0 && !context.CompatibilityOptions.AllowAsymmetricSerializer )
			{
				ConstructorKind constructorKind;
				var deserializationConstructor = FindDeserializationConstructor( context, targetType, out constructorKind );
				var complementedMembers = ComplementMembers( getters, context, targetType );
				var correspondingMemberNames = FindCorrespondingMemberNames( complementedMembers, deserializationConstructor );
				return
					new SerializationTarget(
						complementedMembers,
						deserializationConstructor,
						correspondingMemberNames,
						DetermineCanDeserialize( constructorKind, context, targetType, correspondingMemberNames, allowDefault: false )
					);
			}
			else
			{
				bool? canDeserialize;
				ConstructorKind constructorKind;

				// Try to get default constructor.
				var constructor = targetType.GetConstructor( ReflectionAbstractions.EmptyTypes );
				if ( constructor == null && !targetType.GetIsValueType() )
				{
					// Try to get deserialization constructor.
					var deserializationConstructor = FindDeserializationConstructor( context, targetType, out constructorKind );
					if ( deserializationConstructor == null && !context.CompatibilityOptions.AllowAsymmetricSerializer )
					{
						throw SerializationExceptions.NewTargetDoesNotHavePublicDefaultConstructor( targetType );
					}

					constructor = deserializationConstructor;
					canDeserialize = null;
				}
				else if ( memberCandidates.Length == 0 )
				{
#if DEBUG
					Contract.Assert( context.CompatibilityOptions.AllowAsymmetricSerializer );
#endif // DEBUG
					// Absolutely cannot deserialize in this case.
					canDeserialize = false;
					constructorKind = ConstructorKind.Ambiguous;
				}
				else
				{
					constructorKind = ConstructorKind.Default;
					// Let's prefer annotated constructor here.
					var markedConstructors = FindExplicitDeserializationConstructors( targetType.GetConstructors() );
					if ( markedConstructors.Count == 1 )
					{
						// For backward compatibility, no exceptions are thrown here even if mulitiple deserialization constructor attributes in the type
						// just use default constructor for it.
						constructor = markedConstructors[ 0 ];
						constructorKind = ConstructorKind.Marked;
					}

					// OK, appropriate constructor and setters are found.
					canDeserialize = true;
				}

				if ( constructor != null && constructor.GetParameters().Any() || context.CompatibilityOptions.AllowAsymmetricSerializer )
				{
					// Recalculate members because getter-only/readonly members should be included for constructor deserialization.
					memberCandidates = getters;
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

				var correspondingMemberNames = FindCorrespondingMemberNames( members, constructor );
				return
					new SerializationTarget(
						members,
						constructor,
						correspondingMemberNames,
						canDeserialize ?? DetermineCanDeserialize( constructorKind, context, targetType, correspondingMemberNames, allowDefault: true )
					);
			}
		}

		private static bool HasAnyCorrespondingMembers( IEnumerable<string> correspondingMemberNames )
		{
			return correspondingMemberNames.Count( x => !String.IsNullOrEmpty( x ) ) > 0;
		}

		private static bool HasUnpackableInterface( Type targetType, SerializationContext context )
		{
			return
				typeof( IUnpackable ).IsAssignableFrom( targetType )
#if FEATURE_TAP
						&& ( !context.SerializerOptions.WithAsync || typeof( IAsyncUnpackable ).IsAssignableFrom( targetType ) )
#endif // FEATURE_TAP
						;
		}

		private static bool DetermineCanDeserialize( ConstructorKind kind, SerializationContext context, Type targetType, IEnumerable<string> correspondingMemberNames, bool allowDefault )
		{
			if ( HasUnpackableInterface( targetType, context ) )
			{
				Trace( "SerializationTarget::DetermineCanDeserialize({0}, {1}) -> true: HasUnpackableInterface", targetType, kind );
				return true;
			}

			switch ( kind )
			{
				case ConstructorKind.Marked:
					{
						Trace( "SerializationTarget::DetermineCanDeserialize({0}, {1}) -> true: Marked", targetType, kind );
						return true;
					}
				case ConstructorKind.Parameterful:
					{
						var result = HasAnyCorrespondingMembers( correspondingMemberNames );
						Trace( "SerializationTarget::DetermineCanDeserialize({0}, {1}) -> {2}: HasAnyCorrespondingMembers", targetType, kind, result );
						return result;
					}
				case ConstructorKind.Default:
					{
						Trace( "SerializationTarget::DetermineCanDeserialize({0}, {1}) -> {2}: Default", targetType, kind, allowDefault );
						return allowDefault;
					}
				default:
					{
						Contract.Assert( kind == ConstructorKind.None || kind == ConstructorKind.Ambiguous, "kind == ConstructorKind.None || kind == ConstructorKind.Ambiguous : " + kind );
						return false;
					}
			}
		}

		private static MemberInfo[] GetDistinctMembers( Type type )
		{
			var distinctMembers = new List<MemberInfo>();
			var returningMemberNamesSet = new HashSet<string>();
			while ( type != typeof( object ) && type != null )
			{
				var members =
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
					type.FindMembers(
						MemberTypes.Field | MemberTypes.Property,
						BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly,
						null,
						null
					);
#else
					type.GetTypeInfo().DeclaredFields.Where( f => !f.IsStatic ).OfType<MemberInfo>()
						.Concat( type.GetTypeInfo().DeclaredProperties.Where( p => p.GetMethod != null && !p.GetMethod.IsStatic ) );
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
				foreach ( var memberInfo in members )
				{
					if ( returningMemberNamesSet.Add( memberInfo.Name ) ) //HashSet returns true is new key was added
					{
						distinctMembers.Add( memberInfo );
					}
				}

				type = type.GetBaseType();
			}

			return distinctMembers.ToArray();
		}

		private static IEnumerable<SerializingMember> GetTargetMembers( Type type )
		{
			Contract.Assert( type != null, "type != null" );

			var members = GetDistinctMembers( type );
			var filtered = members.Where( item =>
#if !UNITY
				item.GetCustomAttributesData().Any( a => a.GetAttributeType().FullName == MessagePackMemberAttributeTypeName )
#else
				item.GetCustomAttributes( typeof( MessagePackMemberAttribute ), true ).Any()
#endif // !UNITY
			).ToArray();

			if ( filtered.Length > 0 )
			{
				return GetAnnotatedMembersWithDuplicationDetection( type, filtered );
			}

			if (
#if !UNITY
				type.GetCustomAttributesData().Any( attr =>
					attr.GetAttributeType().FullName == "System.Runtime.Serialization.DataContractAttribute" )
#else
				type.GetCustomAttributes( true ).Any( attr =>
					attr.GetType().FullName == "System.Runtime.Serialization.DataContractAttribute" ) 
#endif // !UNITY
			)
			{
				return GetSystemRuntimeSerializationCompatibleMembers( members );
			}

			return GetPublicUnpreventedMembers( members );
		}

		private static IEnumerable<SerializingMember> GetAnnotatedMembersWithDuplicationDetection( Type type, MemberInfo[] filtered )
		{
			var duplicated =
				filtered.FirstOrDefault(
#if !UNITY
					member => member.GetCustomAttributesData().Any( a => a.GetAttributeType().FullName == MessagePackIgnoreAttributeTypeName )
#else
					member => member.GetCustomAttributes( typeof( MessagePackIgnoreAttribute ), true ).Any()
#endif // !UNITY
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
					{
#if !UNITY
						var attribute = member.GetCustomAttributesData().Single( a => a.GetAttributeType().FullName == MessagePackMemberAttributeTypeName );
						return
							new SerializingMember(
								member,
								new DataMemberContract(
									member,
									( string )GetAttributeProperty( MessagePackMemberAttributeTypeName, attribute, "Name" ),
#if !SILVERLIGHT
									( NilImplication )( ( int? )GetAttributeProperty( MessagePackMemberAttributeTypeName, attribute, "NilImplication" ) ).GetValueOrDefault(),
									( int? )GetAttributeArgument( MessagePackMemberAttributeTypeName, attribute, 0 )
#else
									( NilImplication )GetAttributeProperty( MessagePackMemberAttributeTypeName, attribute, "NilImplication" ),
									( int? )GetAttributeProperty( MessagePackMemberAttributeTypeName, attribute, "Id" )
#endif // !SILVERLIGHT
								)
							);
#else
						var attribute = member.GetCustomAttributes( typeof( MessagePackMemberAttribute ), true ).Single() as MessagePackMemberAttribute;
						return 
							new SerializingMember(
								member,
								new DataMemberContract(
									member,
									attribute.Name,
									attribute.NilImplication,
									attribute.Id
								)
							);
#endif // !UNITY
					}
				);
		}

#if !UNITY
#if !SILVERLIGHT
		private static object GetAttributeArgument( string attributeName, CustomAttributeData attribute, int index )
		{
			var arguments = attribute.GetConstructorArguments();
			if ( arguments.Count < index )
			{
				// Use default.
				return null;
			}

			return arguments[ index ].Value;
		}
#endif // !SILVERLIGHT

#if !SILVERLIGHT
		private static object GetAttributeProperty( string attributeName, CustomAttributeData attribute, string propertyName )
#else
		private static object GetAttributeProperty( string attributeName, Attribute attribute, string propertyName )
#endif // !SILVERLIGHT
		{
			var property = attribute.GetNamedArguments().SingleOrDefault( a => a.GetMemberName() == propertyName );
			if ( property.GetMemberName() == null )
			{
				// Use default.
				return null;
			}

			return property.GetTypedValue().Value;
		}
#endif // !UNITY

		private static IEnumerable<SerializingMember> GetSystemRuntimeSerializationCompatibleMembers( MemberInfo[] members )
		{
			return
				members.Select(
					item =>
						new
						{
							member = item,
							data =
#if !UNITY
								item.GetCustomAttributesData()
								.FirstOrDefault(
									data => data.GetAttributeType().FullName == "System.Runtime.Serialization.DataMemberAttribute"
								)
#else
								item.GetCustomAttributes( true )
								.FirstOrDefault(
									data => data.GetType().FullName == "System.Runtime.Serialization.DataMemberAttribute"
								)
#endif // !UNITY
						}
					).Where( item => item.data != null )
					.Select(
						item =>
						{
#if !UNITY
							var name =
								item.data.GetNamedArguments()
								.Where( arg => arg.GetMemberName() == "Name" )
								.Select( arg => ( string )arg.GetTypedValue().Value )
								.FirstOrDefault();
							var id =
								item.data.GetNamedArguments()
								.Where( arg => arg.GetMemberName() == "Order" )
								.Select( arg => ( int? )arg.GetTypedValue().Value )
								.FirstOrDefault();
							if ( id == -1 )
							{
								id = null;
							}

							return
								new SerializingMember(
									item.member,
									new DataMemberContract( item.member, name, NilImplication.MemberDefault, id )
								);
#else
							var nameProperty = item.data.GetType().GetProperty( "Name" );
							var orderProperty = item.data.GetType().GetProperty( "Order" );
							
							if ( nameProperty == null || !nameProperty.CanRead || nameProperty.GetGetMethod().GetParameters().Length > 0 )
							{
								throw new SerializationException(
									String.Format(
										CultureInfo.CurrentCulture,
										"Failed to get Name property from {0} type.",
										item.data.GetType().AssemblyQualifiedName
									)
								);
							} 

							if( orderProperty == null || !orderProperty.CanRead || orderProperty.GetGetMethod().GetParameters().Length > 0 )
							{
								throw new SerializationException(
									String.Format(
										CultureInfo.CurrentCulture,
										"Failed to get Order property from {0} type.",
										item.data.GetType().AssemblyQualifiedName
									)
								);
							}

							var name = ( string )nameProperty.GetValue( item.data, null );
							var id = ( int? )orderProperty.GetValue( item.data, null );
							if ( id == -1 )
							{
								id = null;
							}

							return
								new SerializingMember(
									item.member,
									new DataMemberContract( item.member, name, NilImplication.MemberDefault, id )
								);							
#endif // !UNITY
						}
					);
		}

		private static IEnumerable<SerializingMember> GetPublicUnpreventedMembers( MemberInfo[] members )
		{
			return members.Where(
				member =>
					member.GetIsPublic()
					&& !member
#if !UNITY
						.GetCustomAttributesData()
						.Select( data => data.GetAttributeType().FullName )
#else
						.GetCustomAttributes( true )
						.Select( data => data.GetType().FullName )
#endif // !UNITY
						.Any( attr =>
							attr == "MsgPack.Serialization.MessagePackIgnoreAttribute"
							|| attr == "System.Runtime.Serialization.IgnoreDataMemberAttribute"
						)
					&& !IsNonSerializedField( member )
				).Select( member => new SerializingMember( member, new DataMemberContract( member ) ) );
		}

		private static bool IsNonSerializedField( MemberInfo member )
		{
			var asField = member as FieldInfo;
			if ( asField == null )
			{
				return false;
			}

			return ( asField.Attributes & FieldAttributes.NotSerialized ) != 0;
		}

		private static ConstructorInfo FindDeserializationConstructor( SerializationContext context, Type targetType, out ConstructorKind constructorKind )
		{
			var constructors = targetType.GetConstructors().ToArray();

			if ( constructors.Length == 0 )
			{
				if ( context.CompatibilityOptions.AllowAsymmetricSerializer )
				{
					constructorKind = ConstructorKind.None;
					return null;
				}
				else
				{
					throw NewTypeCannotBeSerializedException( targetType );
				}
			}

			// The marked construtor is always preferred.
			var markedConstructors = FindExplicitDeserializationConstructors( constructors );
			switch ( markedConstructors.Count )
			{
				case 0:
					{
						break;
					}
				case 1:
					{
						// OK use it for deserialization.
						constructorKind = ConstructorKind.Marked;
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
#if DEBUG
			Trace( "SerializationTarget::FindDeserializationConstructor.MostRich({0}) -> {1}", targetType, String.Join( ";", mostRichConstructors.Select( x => x.ToString() ).ToArray() ) );
#endif // DEBUG
			switch ( mostRichConstructors.Length )
			{
				case 1:
					{
						if ( mostRichConstructors[ 0 ].GetParameters().Length == 0 )
						{
							if ( context.CompatibilityOptions.AllowAsymmetricSerializer )
							{
								constructorKind = ConstructorKind.Default;
								return mostRichConstructors[ 0 ];
							}
							else
							{
								throw NewTypeCannotBeSerializedException( targetType );
							}
						}

						// OK try use it but it may not handle deserialization correctly.
						constructorKind = ConstructorKind.Parameterful;
						return mostRichConstructors[ 0 ];
					}
				default:
					{
						if ( context.CompatibilityOptions.AllowAsymmetricSerializer )
						{
							constructorKind = ConstructorKind.Ambiguous;
							return null;
						}
						else
						{
							throw new SerializationException(
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
		}

		private static IList<ConstructorInfo> FindExplicitDeserializationConstructors( IEnumerable<ConstructorInfo> construtors )
		{
			return
				construtors
				.Where( ctor =>
#if !UNITY
					ctor.GetCustomAttributesData().Any( a =>
						a.GetAttributeType().FullName
#else
					ctor.GetCustomAttributes( true ).Any( a =>
						a.GetType().FullName
#endif // !UNITY
						== MessagePackDeserializationConstructorAttributeTypeName
					)
				).ToArray();
		}

		private static SerializationException NewTypeCannotBeSerializedException( Type targetType )
		{
			return
				new SerializationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Cannot serialize type '{0}' because it does not have any serializable fields nor properties, and it does not have any public constructors with parameters.",
						targetType
					)
				);
		}


		private static bool CheckTargetEligibility( SerializationContext context, MemberInfo member )
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

#if !NETSTANDARD1_1 && !NETSTANDARD1_3
				var setter = asProperty.GetSetMethod( true );
#else
				var setter = asProperty.SetMethod;
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
				if ( setter != null )
				{
					if ( setter.GetIsPublic() )
					{
						return true;
					}

					if ( !context.SerializerOptions.DisablePrivilegedAccess )
					{
						// Can deserialize non-public setter if privileged.
						return true;
					}
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
#if DEBUG
				Contract.Assert( false, "Unknown type member " + member );
#endif // DEBUG
				// ReSharper disable once HeuristicUnreachableCode
				return true;
			}

			var traits = returnType.GetCollectionTraits( CollectionTraitOptions.WithAddMethod, allowNonCollectionEnumerableTypes: false );
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
			if ( candidates.Count == 0 )
			{
				return candidates;
			}

			if ( candidates[ 0 ].Contract.Id < 0 )
			{
				return candidates;
			}

			if ( context.CompatibilityOptions.OneBoundDataMemberOrder && candidates[ 0 ].Contract.Id == 0 )
			{
				throw new NotSupportedException(
					"Cannot specify order value 0 on DataMemberAttribute when SerializationContext.CompatibilityOptions.OneBoundDataMemberOrder is set to true."
				);
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
#if DEBUG
				Contract.Assert( candidates[ source ].Contract.Id >= 0, "candidates[ source ].Contract.Id >= 0" );
#endif // DEBUG

				if ( candidates[ source ].Contract.Id < destination )
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"The member ID '{0}' is duplicated in the '{1}' elementType.",
							candidates[ source ].Contract.Id,
							targetType
						)
					);
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
						SerializationExceptions.ThrowValueTypeCannotBeNull( serializingMember.Member.ToString(), itemType, type );
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
#if DEBUG
						Contract.Assert( asProperty != null, serializingMember.Member.ToString() );
#endif // DEBUG
						isReadOnly = asProperty.GetSetMethod() == null;
					}

					if ( isReadOnly )
					{
						SerializationExceptions.ThrowNullIsProhibited( serializingMember.Member.ToString() );
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

		public static SerializationTarget CreateForCollection( ConstructorInfo collectionConstructor, bool canDeserialize )
		{
			return new SerializationTarget( EmptyMembers, collectionConstructor, EmptyStrings, canDeserialize );
		}

#if !NET35
		public static SerializationTarget CreateForTuple( IList<Type> itemTypes )
		{
			return new SerializationTarget( itemTypes.Select( ( _, i ) => new SerializingMember( GetTupleItemNameFromIndex( i ) ) ).ToArray(), null, null, true );
		}

		public static string GetTupleItemNameFromIndex( int i )
		{
			return "Item" + ( i + 1 ).ToString( "D", CultureInfo.InvariantCulture );
		}
#endif // !NET35

#if FEATURE_CODEGEN
		public static bool BuiltInSerializerExists( ISerializerGeneratorConfiguration configuration, Type type, CollectionTraits traits )
		{
			return GenericSerializer.IsSupported( type, traits, configuration.PreferReflectionBasedSerializer ) || SerializerRepository.InternalDefault.ContainsFor( type );
		}
#endif // FEATURE_CODEGEN

		private sealed class MemberConstructorParameterEqualityComparer : EqualityComparer<KeyValuePair<string, Type>>
		{
			public static readonly IEqualityComparer<KeyValuePair<string, Type>> Instance = new MemberConstructorParameterEqualityComparer();

			private MemberConstructorParameterEqualityComparer() { }

			public override bool Equals( KeyValuePair<string, Type> x, KeyValuePair<string, Type> y )
			{
				return String.Equals( x.Key, y.Key, StringComparison.OrdinalIgnoreCase ) && x.Value == y.Value;
			}

			public override int GetHashCode( KeyValuePair<string, Type> obj )
			{
				return ( obj.Key == null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode( obj.Key ) ) ^ ( obj.Value == null ? 0 : obj.Value.GetHashCode() );
			}
		}

		[Conditional( "DEBUG" )]
		private static void Trace( string format, params object[] args )
		{
#if !SILVERLIGHT && !WINDOWS_PHONE && !NETFX_CORE
			Tracer.Binding.TraceEvent( Tracer.EventType.Trace, Tracer.EventId.Trace, format, args );
#endif // !SILVERLIGHT && !WINDOWS_PHONE && !NETFX_CORE
		}

		private enum ConstructorKind
		{
			None = 0,
			Marked,
			Default,
			Parameterful,
			Ambiguous
		}
	}
}
