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
using System.CodeDom;

namespace MsgPack.Serialization.CodeDomSerializers
{
	internal class ParameterCodeDomConstruct : CodeDomConstruct
	{
		private readonly CodeTypeReference _type;
		private readonly string _name;

		public CodeParameterDeclarationExpression Declaration
		{
			get { return new CodeParameterDeclarationExpression( this._type, this._name ); }
		}

		public CodeArgumentReferenceExpression Reference
		{
			get { return new CodeArgumentReferenceExpression( this._name ); }
		}

		public override bool IsArgument
		{
			get { return true; }
		}

		public override CodeParameterDeclarationExpression AsParameter()
		{
			return this.Declaration;
		}

		public override CodeArgumentReferenceExpression AsArgument()
		{
			return this.Reference;
		}

		public override bool IsExpression
		{
			get { return true; }
		}

		public override CodeExpression AsExpression()
		{
			return this.Reference;
		}

		public ParameterCodeDomConstruct( Type type, string name )
			: base( type )
		{
			this._type = new CodeTypeReference( type );
			this._name = name;
		}
	}
}