// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Runtime.CompilerServices;

namespace MsgPack.Json
{
	/// <summary>
	///		Defines common validation logic for option builders.
	/// </summary>
	internal static class JsonOptionsValidation
	{
		public static InfinityHandling EnsureKnownNonCustom(InfinityHandling value, [CallerArgumentExpression("value")]string paramName = null!)
		{
			switch (value)
			{
				case InfinityHandling.Default:
				case InfinityHandling.Error:
				case InfinityHandling.MinMax:
				{
					break;
				}
				case InfinityHandling.Custom:
				{
					throw new ArgumentException(paramName, $"Value 'Custom' cannot be set via property setter.");
				}
				default:
				{
					throw new ArgumentOutOfRangeException(paramName, $"Value '{value:d}' is not known enum value of enum type '{typeof(InfinityHandling)}'");
				}
			}

			return value;
		}

		public static NaNHandling EnsureKnownNonCustom(NaNHandling value, [CallerArgumentExpression("value")]string paramName = null!)
		{
			switch (value)
			{
				case NaNHandling.Default:
				case NaNHandling.Error:
				case NaNHandling.Null:
				{
					break;
				}
				case NaNHandling.Custom:
				{
					throw new ArgumentException(paramName, $"Value 'Custom' cannot be set via property setter.");
				}
				default:
				{
					throw new ArgumentOutOfRangeException(paramName, $"Value '{value:d}' is not known enum value of enum type '{typeof(NaNHandling)}'");
				}
			}

			return value;
		}
	}
}
