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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using MsgPack.Serialization.DefaultSerializers;

using NUnit.Framework;

namespace MsgPack.Serialization
{
	internal static partial class PreGeneratedSerializerActivator
	{
		private static readonly Type[] _serializerCtorTypes = new[] { typeof( SerializationContext ) };
		private static readonly IList<Type> _knownTypes = InitializeKnownTypes();
		private static readonly Dictionary<Type, Func<SerializationContext, IMessagePackSerializer>> _arrayBasedActivators = InitializeActivators( "ArrayBased", _knownTypes );
		private static readonly Dictionary<Type, Func<SerializationContext, IMessagePackSerializer>> _mapBasedAactivators = InitializeActivators( "MapBased", _knownTypes );

		public static IEnumerable<Type> KnownTypes
		{
			get { return _knownTypes; }
		}

		private static Dictionary<Type, Func<SerializationContext, IMessagePackSerializer>> InitializeActivators( string serializationMethodFlavor, IList<Type> knownTypes )
		{
			var contextParameter = Expression.Parameter( typeof( SerializationContext ), "context" );
			var result = new Dictionary<Type, Func<SerializationContext, IMessagePackSerializer>>( knownTypes.Count );
			foreach ( var knownType in knownTypes )
			{
				if ( knownType.IsInterface )
				{
					// skip
					continue;
				}

				var serializerTypeName =
					"MsgPack.Serialization.GeneratedSerializers." + serializationMethodFlavor + "." + IdentifierUtility.EscapeTypeName( knownType ) + "Serializer";
				var serializerType = Assembly.GetExecutingAssembly().GetType( serializerTypeName );

				Func<SerializationContext, IMessagePackSerializer> activator =
					serializerType != null
					? Expression.Lambda<Func<SerializationContext, IMessagePackSerializer>>(
							Expression.New( serializerType.GetConstructor( _serializerCtorTypes ), contextParameter ),
							contextParameter
						).Compile()
					: ( _ =>
							{
								throw new Exception(
									String.Format(
										CultureInfo.CurrentCulture,
										"Pre-generated serializer '{0}' for type '{1}' does not exist in this project.",
										serializerTypeName,
										knownType
										)
									);
							}
						);

				try
				{
					result.Add( knownType, activator );
				}
				catch ( ArgumentException )
				{
					throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "Key '{0}' is already added.", knownType ), "key" );
				}
			}

			return result;
		}

		/// <summary>
		///		Instantiates new pre-generated serializer instance.
		/// </summary>
		/// <typeparam name="T">The type to serialize.</typeparam>
		/// <param name="context">Ignored.</param>
		/// <returns>A new pre-generated serializer instance.</returns>
		internal static MessagePackSerializer<T> CreateInternal<T>( SerializationContext context )
		{
#if !XAMIOS && !UNITY_IPHONE
			// Simulate Xamarin iOS or Unity iOS behavior...

			if ( typeof( T ) == typeof( MessagePackObject )
				|| typeof( T ).GetCollectionTraits().CollectionType != CollectionKind.NotCollection
				|| ( typeof( T ).GetIsGenericType() && typeof( T ).FullName.StartsWith( "System.Tuple`" ) )
			)
			{
				return MessagePackSerializer.CreateInternal<T>( context );
			}

			var defaultSerializer = SerializerRepository.Default.Get<T>( context );
			if ( defaultSerializer != null )
			{
				return defaultSerializer;
			}

			var genericSerializer = GenericSerializer.Create<T>( context );
			if ( genericSerializer != null )
			{
				return genericSerializer;
			}

			if ( typeof( T ).IsInterface )
			{
				return CreateCore( context.DefaultCollectionTypes.Get( typeof( T ) ), context ) as MessagePackSerializer<T>;
			}

			return CreateCore( typeof( T ), context ) as MessagePackSerializer<T>;
#else
			var activators =
				context.SerializationMethod == SerializationMethod.Array
				? _arrayBasedActivators
				: _mapBasedAactivators;

			Func<SerializationContext, IMessagePackSerializer> activator;
			if ( activators.TryGetValue( typeof( T ), out activator ) )
			{
				return activator( context ) as MessagePackSerializer<T>;
			}

			foreach ( var entry in activators )
			{
				if ( !context.Serializers.Register( entry.Key, entry.Value( context ) ) )
				{
					// fast path
					break;
				}
			}

			return context.GetSerializer<T>();
#endif // if !XAMIOS && !UNITY_IPHONE
		}

		private static object CreateCore( Type targetType, SerializationContext context )
		{
			var activators =
				context.SerializationMethod == SerializationMethod.Array
				? _arrayBasedActivators
				: _mapBasedAactivators;
			Func<SerializationContext, IMessagePackSerializer> activator;
			if ( !activators.TryGetValue( targetType, out activator ) )
			{
				throw new Exception( String.Format( CultureInfo.CurrentCulture, "Unknown type '{0}'.", targetType ) );
			}

			return activator( context );
		}
	}
}
