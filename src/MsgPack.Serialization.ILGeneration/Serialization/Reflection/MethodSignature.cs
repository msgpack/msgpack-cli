// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization.Reflection
{
	internal struct MethodSignature : IEquatable<MethodSignature>
	{
		public string Name { get; }
		public IReadOnlyList<string> ParameterTypes { get; }

		public MethodSignature(MethodBase method)
		{
			this.Name = method.Name;
			this.ParameterTypes = method.GetParameters().Select(p => p.ParameterType.ToString()).ToArray();
		}

		public bool Equals(MethodSignature other)
			=> this.Name == other.Name
				&& this.ParameterTypes.Count == other.ParameterTypes.Count
				&& this.ParameterTypes.SequenceEqual(other.ParameterTypes, StringComparer.Ordinal);

		public override bool Equals(object? obj)
			=> (obj is MethodSignature other) ? this.Equals(other) : false;

		public override int GetHashCode()
		{
			var hashCode = new HashCode();
			hashCode.Add(this.Name);
			foreach(var parameterType in this.ParameterTypes)
			{
				hashCode.Add(parameterType);
			}

			return hashCode.ToHashCode();
		}
	}
}
