// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace MsgPack.Serialization.Reflection
{
	/// <summary>
	///		Defines IL transpilation logics.
	/// </summary>
	internal static partial class ILTranspiler
	{
		/// <summary>
		///		Ports specified method IL instructions to the specified <see cref="ILGenerator"/> in context of specified <see cref="ILGenerationContext"/>.
		/// </summary>
		/// <param name="context">A context for current code generation.</param>
		/// <param name="source">Real <see cref="MethodBase"/> which contains IL source.</param>
		/// <param name="destination"><see cref="ILGenerator"/> which will be emitted ported IL instructions.</param>
		public static void Port(ILGenerationContext context, MethodBase source, ILGenerator destination)
		{
			var methodBody = source.GetMethodBody();
			if (methodBody == null)
			{
				throw new ArgumentException($"The method {source} does not have method body.", nameof(source));
			}

			var tries = methodBody.ExceptionHandlingClauses.ToLookup(e => e.TryOffset);
			var filters = methodBody.ExceptionHandlingClauses.ToLookup(e => e.FilterOffset);
			var handlers = methodBody.ExceptionHandlingClauses.ToLookup(e => e.HandlerOffset);
			var endClauses = methodBody.ExceptionHandlingClauses.ToLookup(e => e.HandlerOffset);

			var locals = methodBody.LocalVariables.OrderBy(l => l.LocalIndex).Select(l => destination.DeclareLocal(l.LocalType, l.IsPinned)).ToArray();
			var labels = new Dictionary<int, List<Label>>();

			var ilStream = methodBody.GetILAsByteArray()!.AsSpan();
			var emits = new List<(int Offset, List<Action<ILGenerator>> Emitters)>();

			// Scan
			ParseILStream(context, source, destination, locals, labels, ilStream, emits);

			// Real emit
			EmitILStream(destination, tries, filters, handlers, endClauses, labels, emits);
		}

		private static void ParseILStream(ILGenerationContext context, MethodBase source, ILGenerator destination, LocalBuilder[] locals, Dictionary<int, List<Label>> labels, Span<byte> ilStream, List<(int Offset, List<Action<ILGenerator>> Emitters)> emits)
		{
			var offset = 0;
			while (offset < ilStream.Length)
			{
				var currentOffset = offset;
				var currentEmits = new List<Action<ILGenerator>>();

				var opCode = Parse(ilStream);
				ilStream = ilStream.Slice(1);
				offset++;
				switch (opCode.OperandType)
				{
					case OperandType.InlineBrTarget:
					{
						var branchOffset = BinaryPrimitives.ReadInt32LittleEndian(ilStream);
						var label = destination.DefineLabel();
						ilStream = ilStream.Slice(sizeof(int));
						offset += sizeof(int);
						AddLabel(labels, branchOffset + offset, label);
						currentEmits.Add(il => il.Emit(opCode, label));
						break;
					}
					case OperandType.InlineField:
					{
						var token = BinaryPrimitives.ReadInt32LittleEndian(ilStream);
						var field = context.ResolveField(source, token);
						ilStream = ilStream.Slice(sizeof(int));
						offset += sizeof(int);
						currentEmits.Add(il => il.Emit(opCode, field!));
						break;
					}
					case OperandType.InlineI:
					{
						var immediate = BinaryPrimitives.ReadInt32LittleEndian(ilStream);
						ilStream = ilStream.Slice(sizeof(int));
						offset += sizeof(int);
						currentEmits.Add(il => il.Emit(opCode, immediate));
						break;
					}
					case OperandType.InlineI8:
					{
						var immediate = BinaryPrimitives.ReadInt64LittleEndian(ilStream);
						ilStream = ilStream.Slice(sizeof(long));
						offset += sizeof(long);
						currentEmits.Add(il => il.Emit(opCode, immediate));
						break;
					}
					case OperandType.InlineMethod:
					{
						var token = BinaryPrimitives.ReadInt32LittleEndian(ilStream);
						ilStream = ilStream.Slice(sizeof(int));
						offset += sizeof(int);

						if (opCode == OpCodes.Call || opCode == OpCodes.Callvirt)
						{
							var method = context.ResolveMethod(source, token);
							currentEmits.Add(il => il.EmitCall(opCode, method, null));
						}
						else if (opCode == OpCodes.Newobj)
						{
							var constructor = context.ResolveConstructor(source, token);
							currentEmits.Add(il => il.Emit(opCode, constructor));
						}
						else if (opCode == OpCodes.Ldftn || opCode == OpCodes.Ldvirtftn || opCode == OpCodes.Jmp)
						{
							var method = context.ResolveMethod(source, token);
							currentEmits.Add(il => il.Emit(opCode, method));
						}
						else
						{
							throw new NotSupportedException($"Unknown opcode '{opCode.Name}'(0x{opCode.Value:X}) at offset {offset}.");
						}

						break;
					}
					case OperandType.InlineNone:
					{
						currentEmits.Add(il => il.Emit(opCode));
						break;
					}
					case OperandType.InlineR:
					{
						var immediate = BitConverter.Int64BitsToDouble(BinaryPrimitives.ReadInt64LittleEndian(ilStream));
						ilStream = ilStream.Slice(sizeof(long));
						offset += sizeof(long);
						currentEmits.Add(il => il.Emit(opCode, immediate));
						break;
					}
					case OperandType.InlineSig:
					{
						throw new NotImplementedException($"OpCode '{opCode.Name}' at offset {offset} in {source} is not implemented.");
					}
					case OperandType.InlineString:
					{
						var token = BinaryPrimitives.ReadInt32LittleEndian(ilStream);
						ilStream = ilStream.Slice(sizeof(int));
						offset += sizeof(int);
						var literal = source.Module.ResolveString(token);
						currentEmits.Add(il => il.Emit(opCode, literal));
						break;
					}
					case OperandType.InlineSwitch:
					{
						var caseCount = BinaryPrimitives.ReadInt32LittleEndian(ilStream);
						ilStream = ilStream.Slice(sizeof(int));
						offset += sizeof(int);

						var caseLabels = new List<Label>(caseCount);
						var caseBaseOffset = offset + (caseCount * sizeof(int));
						for (var i = 0; i < caseLabels.Count; i++)
						{
							var caseTarget = BinaryPrimitives.ReadInt32LittleEndian(ilStream);
							ilStream = ilStream.Slice(sizeof(int));
							offset += sizeof(int);
							var label = destination.DefineLabel();
							caseLabels.Add(label);
							AddLabel(labels, caseTarget + caseBaseOffset, label);
						}

						currentEmits.Add(il => il.Emit(opCode, caseLabels.ToArray()));
						break;
					}
					case OperandType.InlineTok:
					{
						var token = BinaryPrimitives.ReadInt32LittleEndian(ilStream);
						ilStream = ilStream.Slice(sizeof(int));
						offset += sizeof(int);
						currentEmits.Add(il => il.Emit(opCode, token));
						break;
					}
					case OperandType.InlineType:
					{
						var token = BinaryPrimitives.ReadInt32LittleEndian(ilStream);
						ilStream = ilStream.Slice(sizeof(int));
						offset += sizeof(int);
						var type = context.ResolveType(source, token);
						currentEmits.Add(il => il.Emit(opCode, type));
						break;
					}
					case OperandType.InlineVar:
					{
						var index = BinaryPrimitives.ReadUInt16LittleEndian(ilStream);
						ilStream = ilStream.Slice(sizeof(ushort));
						offset += sizeof(ushort);
						var local = locals[index];
						currentEmits.Add(il => il.Emit(opCode, local));
						break;
					}
					case OperandType.ShortInlineBrTarget:
					{
						var branchOffset = (sbyte)ilStream[0];
						var label = destination.DefineLabel();
						ilStream = ilStream.Slice(sizeof(sbyte));
						offset += sizeof(sbyte);
						AddLabel(labels, branchOffset + offset, label);
						currentEmits.Add(il => il.Emit(opCode, label));
						break;
					}
					case OperandType.ShortInlineI:
					{
						var immediate = (sbyte)ilStream[0];
						ilStream = ilStream.Slice(sizeof(sbyte));
						offset += sizeof(sbyte);
						currentEmits.Add(il => il.Emit(opCode, immediate));
						break;
					}
					case OperandType.ShortInlineR:
					{
						var immediate = BitConverter.Int32BitsToSingle(BinaryPrimitives.ReadInt32LittleEndian(ilStream));
						ilStream = ilStream.Slice(sizeof(int));
						offset += sizeof(int);
						currentEmits.Add(il => il.Emit(opCode, immediate));
						break;
					}
					case OperandType.ShortInlineVar:
					{
						var index = ilStream[0];
						ilStream = ilStream.Slice(sizeof(byte));
						offset += sizeof(byte);
						var local = locals[index];
						currentEmits.Add(il => il.Emit(opCode, local));
						break;
					}
					default:
					{
						throw new NotSupportedException($"Unknown operand type {opCode.OperandType}({opCode.OperandType:D}) for opcode '{opCode.Name}' at offset {offset}.");
					}
				} // switch

				emits.Add((currentOffset, currentEmits));
			} // while
		}

		private static void EmitILStream(ILGenerator destination, ILookup<int, ExceptionHandlingClause> tries, ILookup<int, ExceptionHandlingClause> filters, ILookup<int, ExceptionHandlingClause> handlers, ILookup<int, ExceptionHandlingClause> endClauses, Dictionary<int, List<Label>> labels, List<(int Offset, List<Action<ILGenerator>> Emitters)> emits)
		{
			foreach (var emit in emits)
			{
				if (labels.TryGetValue(emit.Offset, out var labelList))
				{
					foreach (var label in labelList)
					{
						destination.MarkLabel(label);
					}
				}

				foreach (var @try in tries[emit.Offset])
				{
					destination.BeginExceptionBlock();
				}

				foreach (var filter in filters[emit.Offset])
				{
					destination.BeginExceptFilterBlock();
				}

				foreach (var handler in handlers[emit.Offset])
				{
					if (handler.Flags == ExceptionHandlingClauseOptions.Clause)
					{
						destination.BeginCatchBlock(handler.CatchType ?? typeof(object));
					}

					if ((handler.Flags & ExceptionHandlingClauseOptions.Fault) != 0)
					{
						destination.BeginFaultBlock();
					}

					if ((handler.Flags & ExceptionHandlingClauseOptions.Finally) != 0)
					{
						destination.BeginFaultBlock();
					}
				}

				foreach (var endClause in endClauses[emit.Offset])
				{
					destination.EndExceptionBlock();
				}

				foreach (var emitter in emit.Emitters)
				{
					emitter(destination);
				}
			}
		}

		private static void AddLabel(Dictionary<int, List<Label>> labels, int offset, Label label)
		{
			if(!labels.TryGetValue(offset, out var labelList))
			{
				labelList = new List<Label>();
				labels[offset] = labelList;
			}

			labelList.Add(label);
		}
	}
}
