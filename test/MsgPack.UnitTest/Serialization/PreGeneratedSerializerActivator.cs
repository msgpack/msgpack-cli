#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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
using System.Globalization;
using System.Reflection;

#if !XAMIOS && !XAMDROID && !UNITY
using MsgPack.Serialization.AbstractSerializers;
#endif // !XAMIOS && !XAMDROID && !UNITY

namespace MsgPack.Serialization
{
	internal static partial class PreGeneratedSerializerActivator
	{
		private static readonly IList<Type> _knownTypes = InitializeKnownTypes();
		private static readonly Dictionary<Type, MessagePackSerializerProvider> _arrayBasedSerializers = InitializeSerializers( "ArrayBased", _knownTypes );
		private static readonly Dictionary<Type, MessagePackSerializerProvider> _mapBasedSerializers = InitializeSerializers( "MapBased", _knownTypes );

		public static IEnumerable<Type> KnownTypes
		{
			get { return _knownTypes; }
		}

		private static Dictionary<Type, MessagePackSerializerProvider> InitializeSerializers( string serializationMethodFlavor, IList<Type> knownTypes )
		{
			var result = new Dictionary<Type, MessagePackSerializerProvider>( knownTypes.Count );
			foreach ( var knownType in knownTypes )
			{
				if ( knownType.GetIsInterface() || knownType.GetIsAbstract() )
				{
					// skip
					continue;
				}

				var serializerTypeName =
					"MsgPack.Serialization.GeneratedSerializers." + serializationMethodFlavor + "." + IdentifierUtility.EscapeTypeName( knownType ) + "Serializer";
				var serializerType = typeof( PreGeneratedSerializerActivator ).GetAssembly().GetType( serializerTypeName );

				Type type = knownType;

				var serializer =
					new LazyMessagePackSerializerProvider(
						knownType,
						serializerType != null
							? new Func<SerializationContext, IMessagePackSerializer>( new SerializerActivator( serializerType ).Activate )
							: ( _ =>
							{
								throw new Exception(
									String.Format(
										CultureInfo.CurrentCulture,
										"Pre-generated serializer '{0}' for type '{1}' does not exist in this project.",
										serializerTypeName,
										type
									)
								);
							}
						)
					);
				try
				{
					result.Add( knownType, serializer );
				}
				catch ( ArgumentException )
				{
					// ReSharper disable once NotResolvedInText
					throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "Key '{0}' is already added.", knownType ), "key" );
				}
			}

			return result;
		}

		/// <summary>
		///		Creates new <see cref="SerializationContext"/> for generation based testing.
		/// </summary>
		/// <param name="method"><see cref="SerializationMethod"/>.</param>
		/// <param name="compatibilityOptions"><see cref="PackerCompatibilityOptions"/> for built-in serializers.</param>
		/// <returns>A new <see cref="SerializationContext"/> for generation based testing.</returns>
		public static SerializationContext CreateContext( SerializationMethod method, PackerCompatibilityOptions compatibilityOptions )
		{
			var context = new SerializationContext( compatibilityOptions ) { SerializationMethod = method };

			var serializers =
				method == SerializationMethod.Array
				? _arrayBasedSerializers
				: _mapBasedSerializers;

			foreach ( var entry in serializers )
			{
				context.Serializers.Register( entry.Key, entry.Value );
			}

#if !XAMIOS && !UNITY_IPHONE
			context.IsRuntimeGenerationDisabled = true;
#endif
			return context;
		}

		private sealed class LazyMessagePackSerializerProvider : MessagePackSerializerProvider
		{
			private readonly Func<SerializationContext, IMessagePackSerializer> _activator;
			private readonly Type _targetType;

			public LazyMessagePackSerializerProvider( Type targetType, Func<SerializationContext, IMessagePackSerializer> activator )
			{
				this._targetType = targetType;
				this._activator = activator;
			}

			public override object Get( SerializationContext context, object providerParameter )
			{
				return
					this._targetType.GetIsEnum()
					? new EnumMessagePackSerializerProvider(
							this._targetType,
							( ICustomizableEnumSerializer )this._activator( context )
						).Get( context, providerParameter )
					: this._activator( context );
			}
		}

		private sealed class SerializerActivator
		{
			private static readonly Type[] SerializerConstructorParameterTypes = { typeof( SerializationContext ) };
			private readonly Type _targetType;
			private readonly ConstructorInfo _constructor;

			public SerializerActivator( Type targeType )
			{
				this._targetType = targeType;
				this._constructor = targeType.GetConstructor( SerializerConstructorParameterTypes );
			}

			public IMessagePackSerializer Activate( SerializationContext context )
			{
				if ( this._constructor == null )
				{
					throw new Exception( "A cosntructor of type '" + this._targetType.FullName + "' is not found." );
				}

				return ( IMessagePackSerializer )this._constructor.Invoke( new object[] { context } );
			}
		}
	}
}
