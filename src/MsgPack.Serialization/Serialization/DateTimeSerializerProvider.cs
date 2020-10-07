// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using MsgPack.Codecs;
using MsgPack.Serialization.BuiltinSerializers;
using MsgPack.Serialization.DefaultSerializers;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Default <see cref="IObjectSerializerProvider"/> implementation for date time like types.
	/// </summary>
	public sealed class DateTimeSerializerProvider : IObjectSerializerProvider
	{
		/// <summary>
		///		Represents input parameters for serializer factory delegate.
		/// </summary>
		public readonly struct SerializerInitializationParameters
		{
			/// <summary>
			///		Gets the owner <see cref="SerializerProvider"/> for the new serializer.
			/// </summary>
			/// <value>
			///		The owner <see cref="SerializerProvider"/> for the new serializer.
			/// </value>
			public SerializerProvider OwnerProvider { get; }

			/// <summary>
			///		Gets the <see cref="DateTimeConversionMethod"/> which the new serializer should handle to.
			/// </summary>
			/// <value>
			///		The <see cref="DateTimeConversionMethod"/> which the new serializer should handle to.
			///		Or <c>null</c> when the serializer should handle with the <see cref="DefaultMethodGetter"/> delegate.
			/// </value>
			public DateTimeConversionMethod? TargetMethod { get; }

			/// <summary>
			///		Gets the delegate which determine final <see cref="DateTimeConversionMethod"/> with <see cref="FormatEncoderOptions.CodecFeatures"/>.
			/// </summary>
			/// <value>
			///		The delegate which determine final <see cref="DateTimeConversionMethod"/> with <see cref="FormatEncoderOptions.CodecFeatures"/>.
			/// </value>
			public Func<CodecFeatures, DateTimeConversionMethod> DefaultMethodGetter { get; }

			internal SerializerInitializationParameters(
				SerializerProvider ownerProvider,
				DateTimeConversionMethod? targetMethod,
				Func<CodecFeatures, DateTimeConversionMethod> defaultMethodGetter
			)
			{
				this.OwnerProvider = ownerProvider;
				this.TargetMethod = targetMethod;
				this.DefaultMethodGetter = defaultMethodGetter;
			}
		}

		private readonly Type _targetType;
#pragma warning disable CS8714
		private readonly IReadOnlyDictionary<DateTimeConversionMethod?, ObjectSerializer> _serializers;
#pragma warning restore CS8714

		/// <summary>
		///		Initializes a new instance of <see cref="DateTimeSerializerProvider"/> object.
		/// </summary>
		/// <param name="targetType">
		///		The target type which this object should handle to.
		///	</param>
		/// <param name="serializerProvider">
		///		The <see cref="SerializerProvider"/> which will be owner of serializers created by <paramref name="serializerFactory"/>.
		///	</param>
		/// <param name="serializerFactory">
		///		The delegate which generates real <see cref="ObjectSerializer"/> which handle <paramref name="targetType"/> with specific <see cref="DateTimeConversionMethod"/>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="targetType"/> is <c>null</c>.
		///		Or, <paramref name="serializerProvider"/> is <c>null</c>.
		///		Or, <paramref name="serializerFactory"/> is <c>null</c>.
		/// </exception>
		public DateTimeSerializerProvider(
			Type targetType,
			SerializerProvider serializerProvider,
			Func<SerializerInitializationParameters, ObjectSerializer> serializerFactory
		)
		{
			this._targetType = Ensure.NotNull(targetType);
			Func<CodecFeatures, DateTimeConversionMethod> defaultMethodGetter =
				Ensure.NotNull(serializerProvider).SerializerGenerationOptions.DateTimeOptions.GetDefaultDateTimeConversionMethod;
			Ensure.NotNull(serializerFactory);

#pragma warning disable CS8714
			void AddSerializer(IDictionary<DateTimeConversionMethod?, ObjectSerializer> dictionary, DateTimeConversionMethod? method)
#pragma warning restore CS8714
				=> dictionary.Add(
					method,
					serializerFactory(new SerializerInitializationParameters(serializerProvider, method, defaultMethodGetter))
				);

			var serializers =
#pragma warning disable CS8714
				new Dictionary<DateTimeConversionMethod?, ObjectSerializer>(4);
#pragma warning restore CS8714
			AddSerializer(serializers, null);
			AddSerializer(serializers, DateTimeConversionMethod.Native);
			AddSerializer(serializers, DateTimeConversionMethod.Timestamp);
			AddSerializer(serializers, DateTimeConversionMethod.UnixEpoc);
			this._serializers = serializers;
		}

		public ObjectSerializer GetSerializer(Type targetType, object? providerParameter)
		{
			if (!Ensure.NotNull(targetType).IsAssignableFrom(this._targetType))
			{
				Throw.IncompatibleTargetType(this._targetType, targetType);
			}

			DateTimeConversionMethod? method = default;
			if (providerParameter is BoxedDateTimeConversionMethod boxed)
			{
				method = boxed.Value;
			}
			else if (providerParameter is DateTimeConversionMethod unboxed)
			{
				method = unboxed;
			}

			return this._serializers[method];
		}

		internal static DateTimeSerializerProvider CreateDateTime(SerializerProvider provider)
			=> new DateTimeSerializerProvider(
				typeof(DateTime),
				provider,
				p => new DateTimeObjectSerializer(p.OwnerProvider, p.TargetMethod)
			);

		internal static DateTimeSerializerProvider CreateDateTimeOffset(SerializerProvider provider)
			=> new DateTimeSerializerProvider(
				typeof(DateTimeOffset),
				provider,
				p => new DateTimeOffsetObjectSerializer(p.OwnerProvider, p.TargetMethod)
			);

		internal static DateTimeSerializerProvider CreateTimestamp(SerializerProvider provider)
			=> new DateTimeSerializerProvider(
				typeof(Timestamp),
				provider,
				p => new TimestampObjectSerializer(p.OwnerProvider, p.TargetMethod)
			);

		internal static DateTimeSerializerProvider CreateFileTime(SerializerProvider provider)
			=> new DateTimeSerializerProvider(
				typeof(Timestamp),
				provider,
				p => new FileTimeOffsetObjectSerializer(p.OwnerProvider, p.TargetMethod)
			);
	}
}
