#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

using MsgPack.Serialization.Polymorphic;

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
							? new Func<SerializationContext, PolymorphismSchema, IMessagePackSerializer>( SerializerActivator.Create( knownType, serializerType, knownType ).Activate )
							: ( ( c, s ) =>
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
				context.Serializers.Register( entry.Key, entry.Value, null, null, SerializerRegistrationOptions.None );
			}

#if !XAMIOS && !UNITY_IPHONE
			context.IsRuntimeGenerationDisabled = true;
#endif
			return context;
		}

		private sealed class LazyMessagePackSerializerProvider : MessagePackSerializerProvider
		{
			private readonly Func<SerializationContext, PolymorphismSchema, IMessagePackSerializer> _activator;
			private readonly Type _targetType;

			public LazyMessagePackSerializerProvider( Type targetType, Func<SerializationContext, PolymorphismSchema, IMessagePackSerializer> activator )
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
							( ICustomizableEnumSerializer )this._activator( context, providerParameter as PolymorphismSchema )
						).Get( context, providerParameter )
					: this._activator( context, providerParameter as PolymorphismSchema );
			}
		}


		private interface ISerializerActivator
		{
			IMessagePackSerializer Activate( SerializationContext context, PolymorphismSchema schema );
		}

		private class SerializerActivator
		{
			protected static readonly Type[] SerializerConstructorParameterTypes1 = { typeof( SerializationContext ) };
			protected static readonly Type[] SerializerConstructorParameterTypes3 = { typeof( SerializationContext ), typeof( Type ), typeof( PolymorphismSchema ) };
			
			public static ISerializerActivator Create( Type targetType, Type serializerType, Type serializationTargetType )
			{
				return
#if !UNITY
					( ISerializerActivator )Activator.CreateInstance(
						typeof( SerializerActivator<> ).MakeGenericType( targetType ),
						serializerType,
						serializationTargetType
					);
#else
					new SerializerActivatorImpl( targetType, serializerType, serializationTargetType );
#endif // !UNITY
			}
		}

#if !UNITY
		private sealed class SerializerActivator<T> : SerializerActivator, ISerializerActivator
#else
		private sealed class SerializerActivatorImpl : SerializerActivator, ISerializerActivator
#endif // !UNITY
		{
#if UNITY
			private readonly Type _targetType;
#endif // UNITY
			private readonly Type _serializerType;
			private readonly Type _serializationTargetType;
			private readonly ConstructorInfo _constructor1;
			private readonly ConstructorInfo _constructor3;

#if !UNITY
			public SerializerActivator( Type serializerType, Type serializationTargetType )
#else
			public SerializerActivatorImpl( Type targetType, Type serializerType, Type serializationTargetType )
#endif // !UNITY
			{
#if UNITY
				this._targetType = targetType;
#endif // UNITY
				this._serializerType = serializerType;
				this._serializationTargetType = serializationTargetType;
				this._constructor1 = serializerType.GetConstructor( SerializerConstructorParameterTypes1 );
				this._constructor3 = serializerType.GetConstructor( SerializerConstructorParameterTypes3 );
			}

			public IMessagePackSerializer Activate( SerializationContext context, PolymorphismSchema schema )
			{
				if ( this._constructor1 == null && this._constructor3 == null )
				{
					throw new Exception( "A cosntructor of type '" + this._serializerType.FullName + "' is not found." );
				}

#if !UNITY
				MessagePackSerializer<T> serializer;
				if ( this._constructor1 != null )
				{
					serializer = ( MessagePackSerializer<T> )this._constructor1.InvokePreservingExceptionType( context );
				}
				else
				{
					serializer = ( MessagePackSerializer<T> )this._constructor3.InvokePreservingExceptionType( context, this._serializationTargetType, null );
				}

				return new PolymorphicSerializerProvider<T>( serializer ).Get( context, schema ?? PolymorphismSchema.Default ) as IMessagePackSerializer;
#else
				IMessagePackSingleObjectSerializer serializer;
				if ( this._constructor1 != null )
				{
					serializer = this._constructor1.InvokePreservingExceptionType( context ) as IMessagePackSingleObjectSerializer;
				}
				else
				{
					serializer = this._constructor3.InvokePreservingExceptionType( context, this._serializationTargetType, null ) as IMessagePackSingleObjectSerializer;
				}

				return 
					ReflectionExtensions.CreateInstancePreservingExceptionType<MessagePackSerializerProvider>(
						typeof( PolymorphicSerializerProvider<> ).MakeGenericType( this._targetType ),
						context,
						serializer
					).Get( context, schema ?? PolymorphismSchema.Default ) as IMessagePackSerializer;
#endif // !UNITY
			}
		}
	}
}
