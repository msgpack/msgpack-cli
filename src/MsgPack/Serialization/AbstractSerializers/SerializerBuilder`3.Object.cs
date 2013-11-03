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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct, TObject>
	{
		private void BuildObjectSerializer( TContext context )
		{
			if ( typeof( TObject ).IsInterface || typeof( TObject ).IsAbstract )
			{
				throw SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractType( typeof( TObject ) );
			}

			var entries = GetTargetMembers().OrderBy( item => item.Contract.Id ).ToArray();

			if ( entries.Length == 0 )
			{
				throw SerializationExceptions.NewNoSerializableFieldsException( typeof( TObject ) );
			}

			if ( entries.All( item => item.Contract.Id == DataMemberContract.UnspecifiedId ) )
			{
				// Alphabetical order.
				this.BuildObjectSerializer( context, entries.OrderBy( item => item.Contract.Name ).ToArray() );
				return;
			}

			// ID order.

			Contract.Assert( entries[ 0 ].Contract.Id >= 0 );

			if ( context.SerializationContext.CompatibilityOptions.OneBoundDataMemberOrder && entries[ 0 ].Contract.Id == 0 )
			{
				throw new NotSupportedException( "Cannot specify order value 0 on DataMemberAttribute when SerializationContext.CompatibilityOptions.OneBoundDataMemberOrder is set to true." );
			}

			var maxId = entries.Max( item => item.Contract.Id );
			var result = new List<SerializingMember>( maxId + 1 );
			for ( int source = 0, destination = context.SerializationContext.CompatibilityOptions.OneBoundDataMemberOrder ? 1 : 0; source < entries.Length; source++, destination++ )
			{
				Contract.Assert( entries[ source ].Contract.Id >= 0 );

				if ( entries[ source ].Contract.Id < destination )
				{
					throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The member ID '{0}' is duplicated in the '{1}' type.", entries[ source ].Contract.Id, typeof( TObject ) ) );
				}

				while ( entries[ source ].Contract.Id > destination )
				{
					result.Add( new SerializingMember() );
					destination++;
				}

				result.Add( entries[ source ] );
			}

			this.BuildObjectSerializer( context, result.ToArray() );
		}

		private static IEnumerable<SerializingMember> GetTargetMembers()
		{
#if !NETFX_CORE
			var members =
				typeof( TObject ).FindMembers(
					MemberTypes.Field | MemberTypes.Property,
					BindingFlags.Public | BindingFlags.Instance,
					( member, criteria ) => true,
					null
				);
			var filtered = members.Where( item => Attribute.IsDefined( item, typeof( MessagePackMemberAttribute ) ) ).ToArray();
#else
			var members =
				typeof( TObject ).GetRuntimeFields().Where( f => f.IsPublic && !f.IsStatic ).OfType<MemberInfo>().Concat( typeof( TObject ).GetRuntimeProperties().Where( p => p.GetMethod != null && p.GetMethod.IsPublic && !p.GetMethod.IsStatic ) );
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

			if ( typeof( TObject ).IsDefined( typeof( DataContractAttribute ) ) )
			{
				return
					members.Where( item => item.IsDefined( typeof( DataMemberAttribute ) ) )
					.Select( member =>
						new SerializingMember(
							member,
							new DataMemberContract( member, member.GetCustomAttribute<DataMemberAttribute>() )
						)
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

		private void BuildObjectSerializer( TContext context, SerializingMember[] entries )
		{
			if ( typeof( IPackable ).IsAssignableFrom( typeof( TObject ) ) )
			{
				this.BuildIPackablePackTo( context );
			}
			else
			{
				this.BuildObjectPackTo( context, entries );
			}

			if ( typeof( IPackable ).IsAssignableFrom( typeof( TObject ) ) )
			{
				this.BuildIUnpackableUnpackFrom( context );
			}
			else
			{
				this.BuildObjectUnpackFrom( context, entries );
			}
		}

		private void BuildIPackablePackTo( TContext context )
		{
			this.BeginPackToMethod( context );
			TConstruct construct = null;
			try
			{
				construct =
					this.EmitInvokeVoidMethod(
						context,
						context.PackingTarget,
						typeof( TObject ).GetInterfaceMap( typeof( IPackable ) ).TargetMethods.Single(),
						context.Packer,
						 this.MakeNullLiteral( context )
					);
			}
			finally
			{
				this.EndPackToMethod( context, construct );
			}
		}

		private void BuildObjectPackTo( TContext context, SerializingMember[] entries )
		{
			this.BeginPackToMethod( context );
			TConstruct construct = null;
			try
			{
				construct =
					this.EmitSequentialStatements(
						context,
						context.SerializationContext.SerializationMethod == SerializationMethod.Array
						? this.BuildObjectPackToWithArray( context, entries )
						: this.BuildObjectPackToWithMap( context, entries )
					);
			}
			finally
			{
				this.EndPackToMethod( context, construct );
			}
		}

		private IEnumerable<TConstruct> BuildObjectPackToWithArray( TContext context, IList<SerializingMember> entries )
		{
			yield return
				this.EmitPutArrayHeaderExpression(
					context, this.MakeInt32Literal( context, entries.Count )
				);

			for ( int i = 0; i < entries.Count; i++ )
			{
				if ( entries[ i ].Member == null )
				{
					// missing member, always nil
					yield return
						this.EmitInvokeVoidMethod( context, context.Packer, Metadata._Packer.PackNull );
				}
				else
				{
					yield return
							this.EmitPackItemExpression(
								context,
								context.Packer,
								entries[ i ].Member.GetMemberValueType(),
								entries[ i ].Contract.NilImplication,
								entries[ i ].Contract.Name,
								this.EmitGetMemberValueExpression( context, context.PackingTarget, entries[ i ].Member )
							);
				}
			}
		}

		private IEnumerable<TConstruct> BuildObjectPackToWithMap( TContext context, IList<SerializingMember> entries )
		{
			yield return
				this.EmitPutMapHeaderExpression( context, this.MakeInt32Literal( context, entries.Count( e => e.Member != null ) ) );

			for ( int i = 0; i < entries.Count; i++ )
			{
				if ( entries[ i ].Member == null )
				{
					// skip
					continue;
				}

				yield return
					this.EmitPackItemExpression(
						context,
						context.Packer,
						typeof( string ),
						NilImplication.Null,
						null,
						this.MakeStringLiteral( context, entries[ i ].Contract.Name )
					);

				yield return
					this.EmitPackItemExpression(
						context,
						context.Packer,
						entries[ i ].Member.GetMemberValueType(),
						entries[ i ].Contract.NilImplication,
						entries[ i ].Contract.Name,
						this.EmitGetMemberValueExpression( context, context.PackingTarget, entries[ i ].Member )
					);
			}
		}

		private void BuildIUnpackableUnpackFrom( TContext context )
		{

			this.BeginUnpackFromMethod( context );
			TConstruct construct = null;
			try
			{
				var result =
					this.DeclareLocal(
						context,
						typeof( TObject ),
						"result",
						this.EmitCreateNewObjectExpression(
							context,
							GetDefaultConstructor()
						)
					);
				construct =
					this.EmitStatementExpression(
						context,
						this.EmitInvokeVoidMethod(
							context,
							result,
							typeof( TObject ).GetInterfaceMap( typeof( IUnpackable ) ).TargetMethods.Single(),
							context.Unpacker
						),
						result
					);
			}
			finally
			{
				this.EndUnpackFromMethod( context, construct );
			}
		}

		private void BuildObjectUnpackFrom( TContext context, SerializingMember[] entries )
		{
			/*
			 *	#if T is IUnpackable
			 *  result.UnpackFromMessage( unpacker );
			 *	#else
			 *	if( unpacker.IsArrayHeader )
			 *	{
			 *		...
			 *	}
			 *	else
			 *	{
			 *		...
			 *	}
			 *	#endif
			 */

			this.BeginUnpackFromMethod( context );
			TConstruct construct = null;
			try
			{
				var result =
					this.DeclareLocal(
						context,
						typeof( TObject ),
						"result",
						this.EmitCreateNewObjectExpression(
							context,
							GetDefaultConstructor()
						)
					);
				construct =
					this.EmitStatementExpression(
						context,
						this.EmitSequentialStatements(
							context,
							typeof( TObject ),
							result,
							this.EmitConditionalExpression(
								context,
								this.EmitGetPropretyExpression( context, context.Unpacker, Metadata._Unpacker.IsArrayHeader ),
								this.EmitObjectUnpackFromArray( context, result, entries ),
								this.EmitObjectUnpackFromMap( context, result, entries )
							)
						),
						result
					);
			}
			finally
			{
				this.EndUnpackFromMethod( context, construct );
			}
		}

		private TConstruct EmitObjectUnpackFromArray( TContext context, TConstruct result, IList<SerializingMember> entries )
		{
			// TODO: Supports ExtensionObject like round-tripping.
			return
				this.EmitSequentialStatements(
					context, typeof( TObject ), this.EmitObjectUnpackFromArrayCore( context, result, entries )
				);
		}

		private IEnumerable<TConstruct> EmitObjectUnpackFromArrayCore( TContext context, TConstruct result, IList<SerializingMember> entries )
		{
			/*
			 *	int unpacked = 0;
			 *	int itemsCount = ITEMS_COUNT( unpacker );
			 *		:
			 *	T? item;
			 *	if ( unpacked >= itemsCount )
			 *	{
			 *		item = null;
			 *	}
			 */

			var unpacked =
				this.DeclareLocal(
					context,
					typeof( int ),
					"unpacked",
					null
				);

			yield return unpacked;

			var itemsCount =
				this.DeclareLocal(
					context,
					typeof( int ),
					"itemsCount",
					this.EmitGetItemsCountExpression( context, context.Unpacker )
				);
			yield return itemsCount;
			for ( int i = 0; i < entries.Count; i++ )
			{
				var count = i;
				yield return
					this.EmitUnpackItemValueExpression(
						context,
						entries[ count ].Member.GetMemberValueType(),
						entries[ count ].Contract.NilImplication,
						context.Unpacker,
						result,
						this.MakeInt32Literal( context, count ),
						this.MakeStringLiteral( context, entries[ count ].Contract.Name ),
						itemsCount,
						unpacked,
						( unpacking, ni, value ) =>
							this.EmitSetMemberValue( context, unpacking, entries[ count ].Member, value )
					);
			}
		}

		private TConstruct EmitObjectUnpackFromMap( TContext context, TConstruct result, IList<SerializingMember> entries )
		{
			// TODO: Supports ExtensionObject like round-tripping.

			return
				this.EmitSequentialStatements(
					context, typeof( TObject ), this.EmitObjectUnpackFromMapCore( context, result, entries )
				);
		}

		private IEnumerable<TConstruct> EmitObjectUnpackFromMapCore( TContext context, TConstruct result, IList<SerializingMember> entries )
		{
			var itemsCount =
				this.DeclareLocal(
					context,
					typeof( int ),
					"itemsCount",
					this.EmitGetItemsCountExpression( context, context.Unpacker )
				);
			yield return itemsCount;

			yield return
				this.EmitForLoop(
					context,
					itemsCount,
					result,
					loopContext =>
					{
						var key =
							this.DeclareLocal(
								context,
								typeof( string ),
								"key",
								null
							);
						var unpackKey =
							this.EmitUnpackItemValueExpression(
								context,
								typeof( string ),
								context.DictionaryKeyNilImplication,
								context.Unpacker,
								result,
								loopContext.Counter,
								this.MakeStringLiteral( context, "MemberName" ),
								null,
								null,
								( _1, _2, unpackedKey ) =>
									this.EmitSetVariable( context, key, unpackedKey )
							);
						var assigns =
							this.EmitStringSwitchStatement(
								context,
								key,
								entries.ToDictionary(
									entry => entry.Contract.Name,
									entry =>
									this.EmitUnpackItemValueExpression(
										context,
										entry.Member.GetMemberValueType(),
										entry.Contract.NilImplication,
										context.Unpacker,
										result,
										loopContext.Counter,
										key,
										null,
										null,
										( _1, _2, value ) =>
											this.EmitSetMemberValue( context, result, entry.Member, value )
									)
								)
							);
						return this.EmitSequentialStatements( context, key, unpackKey, assigns );
					}
				);
		}
	}
}
