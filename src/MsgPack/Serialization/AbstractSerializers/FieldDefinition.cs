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
#if CORE_CLR || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || NETSTANDARD1_1
using System.Globalization;
using System.Reflection;

namespace MsgPack.Serialization.AbstractSerializers
{
	/// <summary>
	///		Represents field which may not have built metadata.
	/// </summary>
	internal sealed class FieldDefinition
	{
		private FieldInfo _runtimeField;

		public FieldInfo ResolveRuntimeField()
		{
			if ( this._runtimeField == null )
			{
				if ( this.DeclaringType == null )
				{
					throw new InvalidOperationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"'{0}' is building private method, but its MethodBuilder is not specified this instance.",
							this.FieldName
						)
					);
				}

				var field = this.DeclaringType.ResolveRuntimeType().GetField( this.FieldName );
				if ( field == null )
				{
					throw new InvalidOperationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"The FieldInfo of {0} is not supplied.",
							this
						)
					);
				}

				this._runtimeField = field;
			}

			return this._runtimeField;
		}

		// Null for CodeDOM
		public readonly TypeDefinition DeclaringType;
		public readonly string FieldName;
		public readonly TypeDefinition FieldType;

		public FieldDefinition( TypeDefinition declaringType, string fieldName, TypeDefinition fieldType )
		{
			this.DeclaringType = declaringType;
			this.FieldName = fieldName;
			this.FieldType = fieldType;
			this._runtimeField = null;
		}

		public FieldDefinition( FieldInfo runtimeField )
		{
#if DEBUG
			Contract.Assert( runtimeField.DeclaringType != null, "runtimeField.DeclaringType != null" );
#endif // DEBUG
			this.DeclaringType = runtimeField.DeclaringType;
			this.FieldName = runtimeField.Name;
			this.FieldType = runtimeField.FieldType;
			this._runtimeField = runtimeField;
		}

		public override string ToString()
		{
			return
				String.Format(
					CultureInfo.InvariantCulture,
					"{0} {1}.{2}",
					this.FieldType,
					this.DeclaringType,
					this.FieldName
				);
		}

		public static implicit operator FieldDefinition( FieldInfo field )
		{
			return new FieldDefinition( field );
		}
	}
}