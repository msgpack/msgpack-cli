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

using System;
using System.Collections.Generic;
#if CORE_CLR || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || NETSTANDARD1_1
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization.AbstractSerializers
{
	/// <summary>
	///		Represents constructor which may not have built metadata.
	/// </summary>
	internal sealed class ConstructorDefinition
	{
		private ConstructorInfo _runtimeConstructor;

		public ConstructorInfo ResolveRuntimeConstructor()
		{
			if ( this._runtimeConstructor == null )
			{
				this._runtimeConstructor =
					this.DeclaringType.ResolveRuntimeType().GetConstructors()
						.SingleOrDefault( c =>
							c.GetParameterTypes()
								.SequenceEqual( this.ParameterTypes.Select( t => t.ResolveRuntimeType() ) )
					);
			}

			return this._runtimeConstructor;
		}

		public readonly TypeDefinition DeclaringType;
		public readonly TypeDefinition[] ParameterTypes;

		public ConstructorDefinition( TypeDefinition declaringType, params TypeDefinition[] parameterTypes )
		{
			this.DeclaringType = declaringType;
			this._runtimeConstructor = null;
			this.ParameterTypes = parameterTypes.ToArray();
		}

		public ConstructorDefinition( ConstructorInfo runtimeConstructor )
			: this( runtimeConstructor, runtimeConstructor.GetParameters().Select( p => TypeDefinition.Object( p.ParameterType ) ) ) {}

		public ConstructorDefinition( ConstructorInfo runtimeConstructor, IEnumerable<TypeDefinition> parameterTypes )
		{
#if DEBUG
			Contract.Assert( runtimeConstructor.DeclaringType != null, "runtimeConstructor.DeclaringType != null" );
#endif // DEBUG
			this.DeclaringType = runtimeConstructor.DeclaringType;
			this._runtimeConstructor = runtimeConstructor;
			this.ParameterTypes = parameterTypes.ToArray();
		}

		public override string ToString()
		{
			return
				String.Format(
					CultureInfo.InvariantCulture,
					"{0}..ctor({1})",
					this.DeclaringType,
					String.Join( ", ", this.ParameterTypes.Select( p => p == null ? "(null)" : p.ToString() ).ToArray() )
				);
		}

		public static implicit operator ConstructorDefinition( ConstructorInfo constructor )
		{
			return new ConstructorDefinition( constructor );
		}
	}
}