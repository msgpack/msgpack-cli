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
#endregion -- License Terms --

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines base features to handle nil implication.
	/// </summary>
	/// <typeparam name="TAction">The type of the actual action for handking.</typeparam>
	/// <typeparam name="TCondition">The type of the condition for packing value nil check.</typeparam>
	/// <typeparam name="TPackingParameter">The type of the implementation specific on-packing methods parameter.</typeparam>
	/// <typeparam name="TUnpackedParameter">The type of the  implementation specific on-unpacked methods parameter.</typeparam>
	internal abstract class NilImplicationHandler<TAction, TCondition, TPackingParameter, TUnpackedParameter>
		where TCondition : class
		where TPackingParameter : INilImplicationHandlerParameter
		where TAction : class
		where TUnpackedParameter : INilImplicationHandlerOnUnpackedParameter<TAction>
	{
		public TAction OnPacking( TPackingParameter parameter, NilImplication nilImplication )
		{
			switch ( nilImplication )
			{
				case NilImplication.Prohibit:
				{
					TCondition condition = null;
					if ( parameter.ItemType == typeof( MessagePackObject ) )
					{
						condition = this.OnPackingMessagePackObject( parameter );
					}
					else if ( !parameter.ItemType.GetIsValueType() )
					{
						condition = this.OnPackingReferenceTypeObject( parameter );
					}
					else if ( Nullable.GetUnderlyingType( parameter.ItemType ) != null )
					{
						condition = this.OnPackingNullableValueTypeObject( parameter );
					}

					if ( condition != null )
					{
						return this.OnPackingCore( parameter, condition );
					}

					break;
				}
			}

			return null;
		}

		protected abstract TCondition OnPackingMessagePackObject( TPackingParameter parameter );

		protected abstract TCondition OnPackingReferenceTypeObject( TPackingParameter parameter );

		protected abstract TCondition OnPackingNullableValueTypeObject( TPackingParameter parameter );

		protected abstract TAction OnPackingCore( TPackingParameter parameter, TCondition condition );

		public TAction OnUnpacked( TUnpackedParameter parameter, NilImplication nilImplication )
		{
			/*
			 *  #if MEMBER_DEFAULT
			 *      // nop
			 *  #elif PROHIBIT
			 *		throw SerializationExceptiuons.NullIsProhibited(...);
			 *  #elif VALUE_TYPE
			 *		throw SerializationExceptiuons.ValueTypeCannotbeNull(...);
			 *  #else
			 *		SET_VALUE(item);
			 *  #endif
			 */

			// is nilable natually?
			var isNativelyNullable =
				parameter.ItemType == typeof( MessagePackObject )
				|| !parameter.ItemType.GetIsValueType()
				|| Nullable.GetUnderlyingType( parameter.ItemType ) != null;

			// Nil Implication
			switch ( nilImplication )
			{
				case NilImplication.MemberDefault:
				{
					// Nothing to
					return OnNopOnUnpacked( parameter );
				}
				case NilImplication.Prohibit:
				{
					return OnThrowNullIsProhibitedExceptionOnUnpacked( parameter );
				}
				case NilImplication.Null:
				{
					return !isNativelyNullable ? this.OnThrowValueTypeCannotBeNull3OnUnpacked( parameter ) : parameter.Store;
				}
				default:
				{
					throw new SerializationException(
						String.Format( CultureInfo.CurrentCulture, "Unknown NilImplication value '{0}'.", ( int )nilImplication )
					);
				}
			}
		}

		protected abstract TAction OnNopOnUnpacked( TUnpackedParameter parameter );

		protected abstract TAction OnThrowNullIsProhibitedExceptionOnUnpacked( TUnpackedParameter parameter );

		protected abstract TAction OnThrowValueTypeCannotBeNull3OnUnpacked( TUnpackedParameter parameter );
	}
}
