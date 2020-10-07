// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MsgPack
{
	internal partial class Ensure
	{
		public static T Defined<T>(T value, [CallerArgumentExpression("value")] string? paramName = null)
			where T : Enum
		{
			if (EnumValidationHelper<T>.Instance.IsDefined(value))
			{
				throw new ArgumentOutOfRangeException(paramName);
			}

			return value;
		}

		private sealed class EnumValidationHelper<T>
			where T : Enum
		{
			public static readonly EnumValidationHelper<T> Instance = new EnumValidationHelper<T>();

			private readonly ulong _undefinedFlags;
			private readonly ulong[] _constants;
			private readonly bool _isFlags;

			private EnumValidationHelper()
			{
				this._isFlags = typeof(T).IsDefined(typeof(FlagsAttribute));
				var underlyingType = Enum.GetUnderlyingType(typeof(T));
				var values =
					(Enum.GetValues(typeof(T)) as object[])
					.Select(v => Convert.ToUInt64(v))
					.ToArray();

				if (!this._isFlags)
				{
					Array.Sort(values);
					this._constants = values;
					this._undefinedFlags = default!;
				}
				else
				{
					this._constants = Array.Empty<ulong>();
					var flags = default(ulong);
					foreach(var flag in values)
					{
						flags |= flag;
					}

					this._undefinedFlags = ~flags;
				}
			}

			public bool IsDefined(T value)
			{
				var bits = Unsafe.As<T, ulong>(ref value);

				if (this._isFlags)
				{
					return (bits & this._undefinedFlags) == 0;
				}
				else
				{
					return Array.BinarySearch(this._constants, bits) >= 0;
				}
			}
		}
	}
}
