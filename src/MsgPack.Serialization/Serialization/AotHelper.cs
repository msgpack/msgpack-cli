// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace MsgPack.Serialization
{
	internal static partial class AotHelper
	{
#pragma warning disable CS0618 // Type or member is obsolete.
		public static void HandleAotError(ExecutionEngineException ex)
#pragma warning restore CS0618 // Type or member is obsolete.
		{
			string api = null;
			if (mayBeGenericArgument.GetIsGenericType())
			{
				var definition = mayBeGenericArgument.GetGenericTypeDefinition();
				if (definition == typeof(ArraySegment<>)
#if MSGPACK_UNITY_FULL
						|| definition == typeof( Stack<> )
						|| definition == typeof( Queue<> )
#endif// MSGPACK_UNITY_FULL
					)
				{
					api = String.Format(CultureInfo.InvariantCulture, "MessagePackSerializer.PrepareCollectionType<{0}>", mayBeGenericArgument.GetGenericArguments()[0].GetFullName());
				}
				else if (definition == typeof(KeyValuePair<,>))
				{
					var genericArguments = mayBeGenericArgument.GetGenericArguments();
					api = String.Format(CultureInfo.InvariantCulture, "MessagePackSerializer.PrepareDictionaryType<{0}, {1}>", genericArguments[0].GetFullName(), genericArguments[1].GetFullName());
				}
			}

			if (api == null)
			{
				api = String.Format(CultureInfo.InvariantCulture, "MessagePackSerializer.PrepareType<{0}>", mayBeGenericArgument.GetFullName());
			}

			throw new InvalidOperationException(
				String.Format(
					CultureInfo.CurrentCulture,
					"An AOT error is occurred. {0} is should be called in advance.",
					api
				),
				mayBeAotError
			);
		}
	}
}
