#region -- License Terms --
//
// NLiblet
//
// Copyright (C) 2011-2015 FUJIWARA, Yusuke
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
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Reflection;
using System.Text;

namespace MsgPack.Serialization.Reflection
{
	/// <summary>
	///		Defines utility extension method for reflection API.
	/// </summary>
	internal static class ReflectionExtensions
	{
		/// <summary>
		///		Determines whether specified <see cref="Type"/> can be assigned to source <see cref="Type"/>.
		/// </summary>
		/// <param name="source">The source type.</param>
		/// <param name="target">The type to compare with the source type.</param>
		/// <returns>
		///   <c>true</c> if <paramref name="source"/> and <paramref name="target"/> represent the same type, 
		///   or if <paramref name="target"/> is in the inheritance hierarchy of <paramref name="source"/>, 
		///   or if <paramref name="target"/> is an interface that <paramref name="source"/> implements, 
		///   or if <paramref name="source"/> is a generic type parameter and <paramref name="target"/> represents one of the constraints of <paramref name="source"/>. 
		///   <c>false</c> if none of these conditions are <c>true</c>, or if <paramref name="target"/> is <c>false</c>. 
		/// </returns>
		public static bool IsAssignableTo(this Type source, Type target)
		{
			Contract.Assert(source != null, "source != null");

			if (target == null)
			{
				return false;
			}

			return target.IsAssignableFrom(source);
		}

#if DEBUG
		/// <summary>
		///		Get IL friendly attributes string.
		/// </summary>
		/// <param name="source"><see cref="MethodAttributes"/>.</param>
		/// <returns>IL friendly attributes string delimited by ASCII whitespace.</returns>
		public static string ToILString(this MethodAttributes source)
		{
			var result = new StringBuilder();

			var memberAccess = source & MethodAttributes.MemberAccessMask;
			switch (memberAccess)
			{
				case MethodAttributes.PrivateScope:
				{
					result.Append("privatescope");
					break;
				}
				case MethodAttributes.Private:
				{
					result.Append("private");
					break;
				}
				case MethodAttributes.FamANDAssem:
				{
					result.Append("famandassem");
					break;
				}
				case MethodAttributes.Assembly:
				{
					result.Append("assembly");
					break;
				}
				case MethodAttributes.Family:
				{
					result.Append("family");
					break;
				}
				case MethodAttributes.FamORAssem:
				{
					result.Append("famorassem");
					break;
				}
				case MethodAttributes.Public:
				{
					result.Append("public");
					break;
				}
			}
			AddString(result, source, MethodAttributes.HideBySig, "hidebysig");
			result.Append((source & MethodAttributes.VtableLayoutMask) == 0 ? " reuseslot" : " newslot");
			AddString(result, source, MethodAttributes.SpecialName, "specialname");
			AddString(result, source, MethodAttributes.RTSpecialName, "rtspecialname");
			AddString(result, source, MethodAttributes.Static, "static");
			AddString(result, source, MethodAttributes.Abstract, "abstract");
			AddString(result, source, MethodAttributes.Virtual, "virtual");
			AddString(result, source, MethodAttributes.Final, "final");
			AddString(result, source, MethodAttributes.CheckAccessOnOverride, "checkaccessonoverride");
			AddString(result, source, MethodAttributes.HasSecurity, "hassecurity");
			AddString(result, source, MethodAttributes.RequireSecObject, "recsecobj");
			AddString(result, source, MethodAttributes.UnmanagedExport, "unmanagedexp");
			AddString(result, source, MethodAttributes.PinvokeImpl, "pinvokeimpl");

			return result.ToString();
		}

		private static void AddString(StringBuilder buffer, MethodAttributes source, MethodAttributes flag, string stringified)
		{
			if ((source & flag) != 0)
			{
				buffer.Append(' ').Append(stringified);
			}
		}

		/// <summary>
		///		Get IL friendly attributes string.
		/// </summary>
		/// <param name="source"><see cref="CallingConventions"/>.</param>
		/// <returns>IL friendly attributes string delimited by ASCII whitespace.</returns>
		public static string ToILString(this CallingConventions source)
		{
			var result = new StringBuilder();

			if ((source & CallingConventions.HasThis) != 0)
			{
				result.Append("instance");
			}

			if ((source & CallingConventions.ExplicitThis) != 0)
			{
				if (result.Length > 0)
				{
					result.Append(' ');
				}

				result.Append("explicit");
			}

			switch ((source & CallingConventions.Any))
			{
				case CallingConventions.Standard:
				{
					if (result.Length > 0)
					{
						result.Append(' ');
					}

					result.Append("standard");
					break;
				}
				case CallingConventions.VarArgs:
				{
					if (result.Length > 0)
					{
						result.Append(' ');
					}

					result.Append("vararg");
					break;
				}
				case CallingConventions.Any:
				{
					if (result.Length > 0)
					{
						result.Append(' ');
					}

					result.Append("any");
					break;
				}
			}

			return result.ToString();
		}

		/// <summary>
		///		Get IL friendly attributes string.
		/// </summary>
		/// <param name="source"><see cref="CallingConventions"/>.</param>
		/// <returns>IL friendly attributes string delimited by ASCII whitespace.</returns>
		public static string ToILString(this MethodImplAttributes source)
		{
			var result = new StringBuilder();

			// ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
			var codeType = source & MethodImplAttributes.CodeTypeMask;
			switch (codeType)
			{
				case MethodImplAttributes.IL:
				{
					result.Append("cil");
					break;
				}
				case MethodImplAttributes.Native:
				{
					result.Append("native");
					break;
				}
				case MethodImplAttributes.OPTIL:
				{
					result.Append("optil");
					break;
				}
				case MethodImplAttributes.Runtime:
				{
					result.Append("runtime");
					break;
				}
			}

			// ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
			result.Append((source & MethodImplAttributes.ManagedMask) == 0 ? " managed" : " unmanaged");

			AddString(result, source, MethodImplAttributes.PreserveSig, "preservesig");
			AddString(result, source, MethodImplAttributes.ForwardRef, "forwardref");
			AddString(result, source, MethodImplAttributes.InternalCall, "internalcall");
			AddString(result, source, MethodImplAttributes.Synchronized, "synchronized");
			AddString(result, source, MethodImplAttributes.NoInlining, "noinlining");
#if !UNITY
			AddString(result, source, MethodImplAttributes.NoOptimization, "nooptimization");
#if !NET35 && !NET40
			AddString(result, source, MethodImplAttributes.AggressiveInlining, "aggressiveinlining");
#endif // !NET35 && !NET40
#endif // !UNITY

			return result.ToString();
		}

		private static void AddString(StringBuilder buffer, MethodImplAttributes source, MethodImplAttributes flag, string stringified)
		{
			// ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
			if ((source & flag) != 0)
			{
				buffer.Append(' ').Append(stringified);
			}
		}
#endif // DEBUG
	}
}
