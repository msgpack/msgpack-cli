// #region -- License Terms --
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
// #endregion -- License Terms --

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if !NETFX_35 && !NETFX_40 && !SILVERLIGHT && !UNITY
using System.Collections.ObjectModel;
#endif // !NETFX_35 && !NETFX_40 && !SILVERLIGHT && !UNITY
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using MsgPack.Serialization.Polymorphic;

namespace MsgPack.Serialization
{
	/// <summary>
	///		<para>
	///			This class is used by MessagePack for CLI serializers. Do not use this type from your application code.
	///		</para>
	///		<para>
	///			A provider parameter to support polymorphism.
	///		</para>
	/// </summary>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public sealed class PolymorphismSchema
	{
		private static readonly Dictionary<byte, Type> EmptyMap = new Dictionary<byte, Type>( 0 );

		internal static readonly ConstructorInfo ConstructorForTypeEmbedding =
			typeof( PolymorphismSchema ).GetConstructor( new[] { typeof( Type ), typeof( PolymorphismSchema ) } );
		internal static readonly ConstructorInfo ConstructorForKnownTypeMapping =
			typeof( PolymorphismSchema ).GetConstructor( new[] { typeof( Type ), typeof( IDictionary<byte, Type> ), typeof( PolymorphismSchema ) } );

		internal static readonly ConstructorInfo CodeTypeMapConstructor =
			typeof( Dictionary<,> ).MakeGenericType( typeof( byte ), typeof( Type ) )
				.GetConstructor( ReflectionAbstractions.EmptyTypes );

		internal static readonly MethodInfo AddToCodeTypeMapMethod =
			typeof( Dictionary<,> ).MakeGenericType( typeof( byte ), typeof( Type ) )
				.GetMethod( "Add", new[] { typeof( byte ), typeof( Type ) } );

		private volatile int _hashCode;

		/// <summary>
		///		Gets the type of the serialization target.
		/// </summary>
		/// <value>
		///		The type of the serialization target.
		/// </value>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public Type TargetType { get; private set; }

		/// <summary>
		///		Gets the code type mapping which maps between ext-type codes and .NET <see cref="Type"/>s.
		/// </summary>
		/// <value>
		///		The code type mapping which maps between ext-type codes and .NET <see cref="Type"/>s.
		/// </value>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public IDictionary<byte, Type> CodeTypeMapping { get; private set; }

		internal bool UseDefault { get { return this.CodeTypeMapping == null; } }
		internal bool UseTypeEmbedding { get { return this.CodeTypeMapping != null && this.CodeTypeMapping.Count == 0; } }


		/// <summary>
		///		Gets the schema for collection items of the serialization target collection.
		/// </summary>
		/// <value>
		///		The schema for collection items of the serialization target collection.
		/// </value>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public PolymorphismSchema ItemSchema { get; private set; }

		/// <summary>
		///		Initializes a new instance of the <see cref="PolymorphismSchema"/> class for type embedding.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="itemSchema">The schema for collection items of the serialization target collection.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public PolymorphismSchema( Type targetType, PolymorphismSchema itemSchema )
		{
			if ( targetType == null )
			{
				throw new ArgumentNullException( "targetType" );
			}

			this.TargetType = targetType;
			this.CodeTypeMapping = new ReadOnlyDictionary<byte, Type>( EmptyMap );
			this.ItemSchema = itemSchema;
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="PolymorphismSchema"/> class for known type mapping.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="codeTypeMapping">The code type mapping which maps between ext-type codes and .NET <see cref="Type"/>s.</param>
		/// <param name="itemSchema">The schema for collection items of the serialization target collection.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="targetType"/> is <c>null</c>.</exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public PolymorphismSchema( Type targetType, IDictionary<byte, Type> codeTypeMapping, PolymorphismSchema itemSchema )
		{
			if ( targetType == null )
			{
				throw new ArgumentNullException( "targetType" );
			}

			this.TargetType = targetType;
			this.CodeTypeMapping =
				codeTypeMapping == null
				? null
				: new ReadOnlyDictionary<byte, Type>( new Dictionary<byte, Type>( codeTypeMapping ) );
			this.ItemSchema = itemSchema;
		}

		internal static PolymorphismSchema Create(
			SerializationContext context, 
			Type type, 
#if !UNITY
			SerializingMember? memberMayBeNull 
#else
			SerializingMember memberMayBeNull
#endif // !UNITY
		)
		{
#if DEBUG
			Debug.Assert( !type.GetIsValueType(), "!type.GetIsValueType()" );
			Debug.Assert( memberMayBeNull != null, "memberMayBeNull.HasValue" );
#endif // DEBUG
#if !UNITY
			var member = memberMayBeNull.Value;
#else
			var member = memberMayBeNull;
#endif // !UNITY

			var attributes = member.Member.GetCustomAttributes( false ).OfType<IPolymorhicHelperAttribute>().ToArray();
			if ( attributes.Length == 0 )
			{
				return null;
			}
			else
			{
				var traits = type.GetCollectionTraits();
				if ( traits.CollectionType == CollectionKind.NotCollection )
				{
					var runtimeMemberTypeSpecifier =
						EnsureAtMostOne(
							member,
							attributes,
							PolymorphismTarget.Member,
							PolymorphismTarget.Default
						);
					var knownMemberTypeMap =
						GetTypeMap(
							member, attributes, PolymorphismTarget.Member, PolymorphismTarget.Default
						);

					VerifyMemberTypeSpecification( member, runtimeMemberTypeSpecifier, knownMemberTypeMap );

					if ( knownMemberTypeMap.Count > 0 )
					{
						return new PolymorphismSchema( type, knownMemberTypeMap, null );
					}
					else
					{
						return new PolymorphismSchema( type, null, null );
					}
				}
				else
				{
					var runtimeMemberTypeSpecifier =
						EnsureAtMostOne(
							member, attributes, PolymorphismTarget.Member
						);
					var knownMemberTypeMap =
						GetTypeMap(
							member, attributes, PolymorphismTarget.Member
						);
					VerifyMemberTypeSpecification( member, runtimeMemberTypeSpecifier, knownMemberTypeMap );

					var runtimeItemTypeSpecifier =
						EnsureAtMostOne(
							member,
							attributes,
							PolymorphismTarget.CollectionItem,
							PolymorphismTarget.Default
						);
					var knownItemTypeMap =
						GetTypeMap(
							member,
							attributes,
							PolymorphismTarget.CollectionItem,
							PolymorphismTarget.Default
						);
					if ( runtimeItemTypeSpecifier != null && knownItemTypeMap.Count > 0 )
					{
						throw new SerializationException(
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot specify '{0}' and '{1}' simultaneously to '{2}' for collection items.",
								typeof( MessagePackRuntimeTypeAttribute ),
								typeof( MessagePackKnownTypeAttribute ),
								member.Member
							)
						);
					}

					if ( knownMemberTypeMap.Count > 0 )
					{
						if ( knownItemTypeMap.Count > 0 )
						{
							return
								new PolymorphismSchema(
									type,
									knownMemberTypeMap,
									new PolymorphismSchema( traits.ElementType, knownItemTypeMap, null )
								);
						}
						else if ( runtimeItemTypeSpecifier != null )
						{
							return
								new PolymorphismSchema(
									type,
									knownMemberTypeMap,
									new PolymorphismSchema( traits.ElementType, null )
								);
						}
						else
						{
							return
								new PolymorphismSchema(
									type,
									knownMemberTypeMap,
									null
								);
						}
					}
					else
					{
						if ( knownItemTypeMap.Count > 0 )
						{
							return
								new PolymorphismSchema(
									type,
									null,
									new PolymorphismSchema( traits.ElementType, knownItemTypeMap, null )
								);
						}
						else if ( runtimeItemTypeSpecifier != null )
						{
							return
								new PolymorphismSchema(
									type,
									null,
									new PolymorphismSchema( traits.ElementType, null )
								);
						}
						else
						{
							return new PolymorphismSchema( type, null );
						}
					}
				}
			}
		}

		// ReSharper disable UnusedParameter.Local
		private static void VerifyMemberTypeSpecification(
			SerializingMember member,
			MessagePackRuntimeTypeAttribute runtimeMemberTypeSpecifier,
			IDictionary<byte, Type> knownMemberTypeSpecifiers )
		{
			if ( runtimeMemberTypeSpecifier != null && knownMemberTypeSpecifiers.Count > 0 )
			{
				throw new SerializationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Cannot specify '{0}' and '{1}' simultaneously to '{2}' member itself.",
						typeof( MessagePackRuntimeTypeAttribute ),
						typeof( MessagePackKnownTypeAttribute ),
					// ReSharper disable once PossibleInvalidOperationException
						member.Member
					)
				);
			}
		}
		// ReSharper restore UnusedParameter.Local

		private static MessagePackRuntimeTypeAttribute EnsureAtMostOne( SerializingMember member, IPolymorhicHelperAttribute[] attributes, params PolymorphismTarget[] targets )
		{
			var result = attributes.OfType<MessagePackRuntimeTypeAttribute>().Where( a => targets.Contains( a.Target ) ).ToArray();
			if ( result.Length > 1 )
			{
				throw new SerializationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Cannot specify multiple '{0}' with '{1}' for member '{2}'.",
						typeof( MessagePackRuntimeTypeAttribute ),
						String.Join( 
							CultureInfo.CurrentCulture.TextInfo.ListSeparator, 
							targets
#if NETFX_35 || UNITY
							.Select( x => x.ToString() ).ToArray()
#endif // NETFX_35 || UNITY
						),
						member.Member
					)
				);
			}

			return result.Length > 0 ? result[ 0 ] : null;
		}

		private static IDictionary<byte, Type> GetTypeMap( SerializingMember member, IPolymorhicHelperAttribute[] attributes, params PolymorphismTarget[] targets )
		{
			var result = new Dictionary<byte, Type>();
			foreach (
				var attribute in attributes.OfType<MessagePackKnownTypeAttribute>().Where( a => targets.Contains( a.Target ) )
			)
			{
				try
				{
					result.Add( attribute.BindingCode, attribute.BindingType );
				}
				catch ( ArgumentException )
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Cannot specify multiple types for ext-type code '{0}' with '{1}' for member '{2}'.",
							attribute.BindingCode,
							String.Join(
								CultureInfo.CurrentCulture.TextInfo.ListSeparator, 
								targets
#if NETFX_35 || UNITY
								.Select( x => x.ToString() ).ToArray()
#endif // NETFX_35 || UNITY
							),
							member.Member
						)
					);
				}
			}

			return result;
		}

		/// <summary>
		///		Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		///		A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			// ReSharper disable NonReadonlyMemberInGetHashCode
			var result = this._hashCode;
			if ( result == 0 )
			{
				result =
					this.TargetType.GetHashCode()
					^ ( this.CodeTypeMapping == null
						? 0
						: this.CodeTypeMapping.Select( kv => kv.Key.GetHashCode() ^ kv.Value.GetHashCode() )
							.Aggregate( ( x, y ) => x ^ y )
					)
					^ ( this.ItemSchema == null ? 0 : this.ItemSchema.GetHashCode() );
				// Cache expensive hash-code. It is OK because CodeTypeMapping is immutable.
				this._hashCode = result;
			}

			return result;
			// ReSharper restore NonReadonlyMemberInGetHashCode
		}

		internal static bool Equivalents( PolymorphismSchema left, PolymorphismSchema right )
		{
			if ( left == right )
			{
				return true;
			}

			if ( left == null || right == null )
			{
				return false;
			}

			if ( !left.TargetType.TypeHandle.Equals( right.TargetType.TypeHandle ) )
			{
				return false;
			}

			if ( left.CodeTypeMapping != right.CodeTypeMapping )
			{
				if ( left.CodeTypeMapping == null || right.CodeTypeMapping == null )
				{
					return false;
				}

				if ( left.CodeTypeMapping.Count != right.CodeTypeMapping.Count )
				{
					return false;
				}

				foreach ( var entry in left.CodeTypeMapping )
				{
					Type rightType;
					if ( !right.CodeTypeMapping.TryGetValue( entry.Key, out rightType ) )
					{
						return false;
					}

					if ( !entry.Value.TypeHandle.Equals( rightType.TypeHandle ) )
					{
						return false;
					}
				}
			}

			if ( left.ItemSchema == right.ItemSchema )
			{
				return true;
			}
			else if ( left.ItemSchema == null || right.ItemSchema == null )
			{
				return false;
			}
			else
			{
				return Equivalents( left.ItemSchema, right.ItemSchema );
			}
		}

#if NETFX_35 || NETFX_40 || SILVERLIGHT || UNITY
		private sealed class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
		{
			private readonly IDictionary<TKey, TValue> _underlying;

			ICollection<TKey> IDictionary<TKey, TValue>.Keys
			{
				get { return this._underlying.Keys; }
			}

			ICollection<TValue> IDictionary<TKey, TValue>.Values
			{
				get { return this._underlying.Values; }
			}

			TValue IDictionary<TKey, TValue>.this[ TKey key ]
			{
				get { return this._underlying[ key ]; }
				set
				{
					throw new NotSupportedException();
				}
			}

			int ICollection<KeyValuePair<TKey, TValue>>.Count
			{
				get { return this._underlying.Count; }
			}

			bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
			{
				get { return true; }
			}

			public ReadOnlyDictionary( IDictionary<TKey, TValue> underlying )
			{
				this._underlying = underlying;
			}

			bool IDictionary<TKey, TValue>.ContainsKey( TKey key )
			{
				return this._underlying.ContainsKey( key );
			}

			bool IDictionary<TKey, TValue>.TryGetValue( TKey key, out TValue value )
			{
				return this._underlying.TryGetValue( key, out value );
			}

			bool ICollection<KeyValuePair<TKey, TValue>>.Contains( KeyValuePair<TKey, TValue> item )
			{
				return this._underlying.Contains( item );
			}

			void ICollection<KeyValuePair<TKey, TValue>>.CopyTo( KeyValuePair<TKey, TValue>[] array, int arrayIndex )
			{
				this._underlying.CopyTo( array, arrayIndex );
			}

			IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
			{
				return this._underlying.GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this._underlying.GetEnumerator();
			}

			void IDictionary<TKey, TValue>.Add( TKey key, TValue value )
			{
				throw new NotSupportedException();
			}

			bool IDictionary<TKey, TValue>.Remove( TKey key )
			{
				throw new NotSupportedException();
			}

			void ICollection<KeyValuePair<TKey, TValue>>.Add( KeyValuePair<TKey, TValue> item )
			{
				throw new NotSupportedException();
			}

			void ICollection<KeyValuePair<TKey, TValue>>.Clear()
			{
				throw new NotSupportedException();
			}

			bool ICollection<KeyValuePair<TKey, TValue>>.Remove( KeyValuePair<TKey, TValue> item )
			{
				throw new NotSupportedException();
			}
		}
#endif // NETFX_35 || NETFX_40 || SILVERLIGHT || UNITY
	}
}