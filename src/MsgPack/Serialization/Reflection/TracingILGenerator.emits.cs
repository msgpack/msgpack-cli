#region -- License Terms --
//
// NLiblet
//
// Copyright (C) 2011 FUJIWARA, Yusuke
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

// This code is generated from T4Template TracingILGenerator.emits.tt.
// Do not modify this source code directly.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace MsgPack.Serialization.Reflection
{
	partial class TracingILGenerator
	{
#if DEBUG
		///	<summary>
		///		Emit 'nop' instruction with specified arguments.
		///	</summary>
		public void EmitNop()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Nop );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Nop );
		}

		///	<summary>
		///		Emit 'break' instruction with specified arguments.
		///	</summary>
		public void EmitBreak()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Break );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Break );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'ldarg.0' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdarg_0()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldarg_0 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldarg_0 );
		}

		///	<summary>
		///		Emit 'ldarg.1' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdarg_1()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldarg_1 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldarg_1 );
		}

		///	<summary>
		///		Emit 'ldarg.2' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdarg_2()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldarg_2 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldarg_2 );
		}

		///	<summary>
		///		Emit 'ldarg.3' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdarg_3()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldarg_3 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldarg_3 );
		}

		///	<summary>
		///		Emit 'ldloc.0' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdloc_0()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldloc_0 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldloc_0 );
		}

		///	<summary>
		///		Emit 'ldloc.1' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdloc_1()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldloc_1 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldloc_1 );
		}

		///	<summary>
		///		Emit 'ldloc.2' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdloc_2()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldloc_2 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldloc_2 );
		}

		///	<summary>
		///		Emit 'ldloc.3' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdloc_3()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldloc_3 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldloc_3 );
		}

		///	<summary>
		///		Emit 'stloc.0' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStloc_0()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stloc_0 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stloc_0 );
		}

		///	<summary>
		///		Emit 'stloc.1' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStloc_1()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stloc_1 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stloc_1 );
		}

		///	<summary>
		///		Emit 'stloc.2' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStloc_2()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stloc_2 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stloc_2 );
		}

		///	<summary>
		///		Emit 'stloc.3' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStloc_3()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stloc_3 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stloc_3 );
		}

		///	<summary>
		///		Emit 'ldarg.s' instruction with specified arguments.
		///	</summary>
		///	<param name="value"><see cref="System.Byte"/> as value.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdarg_S( System.Byte value )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldarg_S );
			this.TraceWrite( " " );
			this.TraceOperand( value );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldarg_S, value );
		}

		///	<summary>
		///		Emit 'ldarga.s' instruction with specified arguments.
		///	</summary>
		///	<param name="value"><see cref="System.Byte"/> as value.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdarga_S( System.Byte value )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldarga_S );
			this.TraceWrite( " " );
			this.TraceOperand( value );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldarga_S, value );
		}

#if DEBUG
		///	<summary>
		///		Emit 'starg.s' instruction with specified arguments.
		///	</summary>
		///	<param name="value"><see cref="System.Byte"/> as value.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStarg_S( System.Byte value )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Starg_S );
			this.TraceWrite( " " );
			this.TraceOperand( value );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Starg_S, value );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'ldloc.s' instruction with specified arguments.
		///	</summary>
		///	<param name="value"><see cref="System.Byte"/> as value.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdloc_S( System.Byte value )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldloc_S );
			this.TraceWrite( " " );
			this.TraceOperand( value );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldloc_S, value );
		}

		///	<summary>
		///		Emit 'ldloca.s' instruction with specified arguments.
		///	</summary>
		///	<param name="value"><see cref="System.Byte"/> as value.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdloca_S( System.Byte value )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldloca_S );
			this.TraceWrite( " " );
			this.TraceOperand( value );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldloca_S, value );
		}

		///	<summary>
		///		Emit 'stloc.s' instruction with specified arguments.
		///	</summary>
		///	<param name="value"><see cref="System.Byte"/> as value.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStloc_S( System.Byte value )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stloc_S );
			this.TraceWrite( " " );
			this.TraceOperand( value );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stloc_S, value );
		}

		///	<summary>
		///		Emit 'ldnull' instruction with specified arguments.
		///	</summary>
		public void EmitLdnull()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldnull );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldnull );
		}

		///	<summary>
		///		Emit 'ldc.i4.m1' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdc_I4_M1()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldc_I4_M1 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldc_I4_M1 );
		}

		///	<summary>
		///		Emit 'ldc.i4.0' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdc_I4_0()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldc_I4_0 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldc_I4_0 );
		}

		///	<summary>
		///		Emit 'ldc.i4.1' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdc_I4_1()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldc_I4_1 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldc_I4_1 );
		}

		///	<summary>
		///		Emit 'ldc.i4.2' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdc_I4_2()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldc_I4_2 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldc_I4_2 );
		}

		///	<summary>
		///		Emit 'ldc.i4.3' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdc_I4_3()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldc_I4_3 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldc_I4_3 );
		}

		///	<summary>
		///		Emit 'ldc.i4.4' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdc_I4_4()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldc_I4_4 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldc_I4_4 );
		}

		///	<summary>
		///		Emit 'ldc.i4.5' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdc_I4_5()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldc_I4_5 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldc_I4_5 );
		}

		///	<summary>
		///		Emit 'ldc.i4.6' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdc_I4_6()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldc_I4_6 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldc_I4_6 );
		}

		///	<summary>
		///		Emit 'ldc.i4.7' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdc_I4_7()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldc_I4_7 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldc_I4_7 );
		}

		///	<summary>
		///		Emit 'ldc.i4.8' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdc_I4_8()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldc_I4_8 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldc_I4_8 );
		}

		///	<summary>
		///		Emit 'ldc.i4.s' instruction with specified arguments.
		///	</summary>
		///	<param name="value"><see cref="System.Byte"/> as value.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdc_I4_S( System.Byte value )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldc_I4_S );
			this.TraceWrite( " " );
			this.TraceOperand( value );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldc_I4_S, value );
		}

		///	<summary>
		///		Emit 'ldc.i4' instruction with specified arguments.
		///	</summary>
		///	<param name="value"><see cref="System.Int32"/> as value.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdc_I4( System.Int32 value )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldc_I4 );
			this.TraceWrite( " " );
			this.TraceOperand( value );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldc_I4, value );
		}

		///	<summary>
		///		Emit 'ldc.i8' instruction with specified arguments.
		///	</summary>
		///	<param name="value"><see cref="System.Int64"/> as value.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdc_I8( System.Int64 value )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldc_I8 );
			this.TraceWrite( " " );
			this.TraceOperand( value );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldc_I8, value );
		}

		///	<summary>
		///		Emit 'ldc.r4' instruction with specified arguments.
		///	</summary>
		///	<param name="value"><see cref="System.Byte"/> as value.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdc_R4( System.Single value )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldc_R4 );
			this.TraceWrite( " " );
			this.TraceOperand( value );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldc_R4, value );
		}

		///	<summary>
		///		Emit 'ldc.r8' instruction with specified arguments.
		///	</summary>
		///	<param name="value"><see cref="System.Double"/> as value.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdc_R8( System.Double value )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldc_R8 );
			this.TraceWrite( " " );
			this.TraceOperand( value );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldc_R8, value );
		}

#if DEBUG
		///	<summary>
		///		Emit 'dup' instruction with specified arguments.
		///	</summary>
		public void EmitDup()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Dup );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Dup );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'pop' instruction with specified arguments.
		///	</summary>
		public void EmitPop()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Pop );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Pop );
		}

#if DEBUG
		///	<summary>
		///		Emit 'jmp' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.MethodInfo"/> as target.</param>
		public void EmitJmp( System.Reflection.MethodInfo target )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( target != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Jmp );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Jmp, target );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'call' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.MethodInfo"/> as target.</param>
		public void EmitCall( System.Reflection.MethodInfo target )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( target != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Call );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Call, target );
		}

#if DEBUG
		///	<summary>
		///		Emit 'br.s' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Br", Justification = "It is IL suffix." )]
		public void EmitBr_S( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Br_S );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Br_S, target );
		}

		///	<summary>
		///		Emit 'brfalse.s' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitBrfalse_S( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Brfalse_S );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Brfalse_S, target );
		}

		///	<summary>
		///		Emit 'brtrue.s' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitBrtrue_S( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Brtrue_S );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Brtrue_S, target );
		}

		///	<summary>
		///		Emit 'beq.s' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitBeq_S( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Beq_S );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Beq_S, target );
		}

		///	<summary>
		///		Emit 'bge.s' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitBge_S( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Bge_S );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Bge_S, target );
		}

		///	<summary>
		///		Emit 'bgt.s' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitBgt_S( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Bgt_S );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Bgt_S, target );
		}

		///	<summary>
		///		Emit 'ble.s' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitBle_S( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ble_S );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ble_S, target );
		}

		///	<summary>
		///		Emit 'blt.s' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitBlt_S( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Blt_S );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Blt_S, target );
		}

		///	<summary>
		///		Emit 'bne.un.s' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitBne_Un_S( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Bne_Un_S );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Bne_Un_S, target );
		}

		///	<summary>
		///		Emit 'bge.un.s' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitBge_Un_S( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Bge_Un_S );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Bge_Un_S, target );
		}

		///	<summary>
		///		Emit 'bgt.un.s' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitBgt_Un_S( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Bgt_Un_S );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Bgt_Un_S, target );
		}

		///	<summary>
		///		Emit 'ble.un.s' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitBle_Un_S( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ble_Un_S );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ble_Un_S, target );
		}

		///	<summary>
		///		Emit 'blt.un.s' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitBlt_Un_S( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Blt_Un_S );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Blt_Un_S, target );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'br' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Br", Justification = "It is IL suffix." )]
		public void EmitBr( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Br );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Br, target );
		}

		///	<summary>
		///		Emit 'brfalse' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		public void EmitBrfalse( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Brfalse );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Brfalse, target );
		}

		///	<summary>
		///		Emit 'brtrue' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		public void EmitBrtrue( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Brtrue );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Brtrue, target );
		}

#if DEBUG
		///	<summary>
		///		Emit 'beq' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		public void EmitBeq( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Beq );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Beq, target );
		}

		///	<summary>
		///		Emit 'bge' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		public void EmitBge( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Bge );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Bge, target );
		}
		///	<summary>
		///		Emit 'bgt' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		public void EmitBgt( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Bgt );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Bgt, target );
		}

		///	<summary>
		///		Emit 'ble' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		public void EmitBle( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ble );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ble, target );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'blt' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		public void EmitBlt( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Blt );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Blt, target );
		}

#if DEBUG
		///	<summary>
		///		Emit 'bne.un' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitBne_Un( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Bne_Un );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Bne_Un, target );
		}

		///	<summary>
		///		Emit 'bge.un' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitBge_Un( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Bge_Un );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Bge_Un, target );
		}

		///	<summary>
		///		Emit 'bgt.un' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitBgt_Un( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Bgt_Un );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Bgt_Un, target );
		}

		///	<summary>
		///		Emit 'ble.un' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitBle_Un( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ble_Un );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ble_Un, target );
		}

		///	<summary>
		///		Emit 'blt.un' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitBlt_Un( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Blt_Un );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Blt_Un, target );
		}

		///	<summary>
		///		Emit 'switch' instruction with specified arguments.
		///	</summary>
		///	<param name="targets"><see cref="System.Reflection.Emit.Label"/>[] as targets.</param>
		public void EmitSwitch( params System.Reflection.Emit.Label[] targets )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( targets != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Switch );
			this.TraceWrite( " " );
			this.TraceOperand( targets );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Switch, targets );
		}

		///	<summary>
		///		Emit 'ldind.i1' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdind_I1()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldind_I1 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldind_I1 );
		}

		///	<summary>
		///		Emit 'ldind.u1' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdind_U1()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldind_U1 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldind_U1 );
		}

		///	<summary>
		///		Emit 'ldind.i2' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdind_I2()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldind_I2 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldind_I2 );
		}

		///	<summary>
		///		Emit 'ldind.u2' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdind_U2()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldind_U2 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldind_U2 );
		}

		///	<summary>
		///		Emit 'ldind.i4' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdind_I4()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldind_I4 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldind_I4 );
		}

		///	<summary>
		///		Emit 'ldind.u4' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdind_U4()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldind_U4 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldind_U4 );
		}

		///	<summary>
		///		Emit 'ldind.i8' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdind_I8()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldind_I8 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldind_I8 );
		}

		///	<summary>
		///		Emit 'ldind.i' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdind_I()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldind_I );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldind_I );
		}

		///	<summary>
		///		Emit 'ldind.r4' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdind_R4()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldind_R4 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldind_R4 );
		}

		///	<summary>
		///		Emit 'ldind.r8' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdind_R8()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldind_R8 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldind_R8 );
		}

		///	<summary>
		///		Emit 'ldind.ref' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdind_Ref()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldind_Ref );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldind_Ref );
		}

		///	<summary>
		///		Emit 'stind.ref' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStind_Ref()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stind_Ref );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stind_Ref );
		}

		///	<summary>
		///		Emit 'stind.i1' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStind_I1()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stind_I1 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stind_I1 );
		}

		///	<summary>
		///		Emit 'stind.i2' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStind_I2()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stind_I2 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stind_I2 );
		}

		///	<summary>
		///		Emit 'stind.i4' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStind_I4()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stind_I4 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stind_I4 );
		}

		///	<summary>
		///		Emit 'stind.i8' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStind_I8()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stind_I8 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stind_I8 );
		}

		///	<summary>
		///		Emit 'stind.r4' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStind_R4()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stind_R4 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stind_R4 );
		}

		///	<summary>
		///		Emit 'stind.r8' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStind_R8()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stind_R8 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stind_R8 );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'add' instruction with specified arguments.
		///	</summary>
		public void EmitAdd()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Add );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Add );
		}

#if DEBUG
		///	<summary>
		///		Emit 'sub' instruction with specified arguments.
		///	</summary>
		public void EmitSub()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Sub );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Sub );
		}

		///	<summary>
		///		Emit 'mul' instruction with specified arguments.
		///	</summary>
		public void EmitMul()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Mul );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Mul );
		}

		///	<summary>
		///		Emit 'div' instruction with specified arguments.
		///	</summary>
		public void EmitDiv()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Div );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Div );
		}

		///	<summary>
		///		Emit 'div.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitDiv_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Div_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Div_Un );
		}

		///	<summary>
		///		Emit 'rem' instruction with specified arguments.
		///	</summary>
		public void EmitRem()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Rem );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Rem );
		}

		///	<summary>
		///		Emit 'rem.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitRem_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Rem_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Rem_Un );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'and' instruction with specified arguments.
		///	</summary>
		public void EmitAnd()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.And );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.And );
		}

#if DEBUG
		///	<summary>
		///		Emit 'or' instruction with specified arguments.
		///	</summary>
		public void EmitOr()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Or );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Or );
		}

		///	<summary>
		///		Emit 'xor' instruction with specified arguments.
		///	</summary>
		public void EmitXor()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Xor );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Xor );
		}

		///	<summary>
		///		Emit 'shl' instruction with specified arguments.
		///	</summary>
		public void EmitShl()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Shl );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Shl );
		}

		///	<summary>
		///		Emit 'shr' instruction with specified arguments.
		///	</summary>
		public void EmitShr()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Shr );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Shr );
		}

		///	<summary>
		///		Emit 'shr.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitShr_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Shr_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Shr_Un );
		}

		///	<summary>
		///		Emit 'neg' instruction with specified arguments.
		///	</summary>
		public void EmitNeg()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Neg );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Neg );
		}

		///	<summary>
		///		Emit 'not' instruction with specified arguments.
		///	</summary>
		public void EmitNot()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Not );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Not );
		}

		///	<summary>
		///		Emit 'conv.i1' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_I1()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_I1 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_I1 );
		}

		///	<summary>
		///		Emit 'conv.i2' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_I2()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_I2 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_I2 );
		}

		///	<summary>
		///		Emit 'conv.i4' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_I4()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_I4 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_I4 );
		}

		///	<summary>
		///		Emit 'conv.i8' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_I8()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_I8 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_I8 );
		}

		///	<summary>
		///		Emit 'conv.r4' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_R4()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_R4 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_R4 );
		}

		///	<summary>
		///		Emit 'conv.r8' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_R8()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_R8 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_R8 );
		}

		///	<summary>
		///		Emit 'conv.u4' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_U4()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_U4 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_U4 );
		}

		///	<summary>
		///		Emit 'conv.u8' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_U8()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_U8 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_U8 );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'callvirt' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.MethodInfo"/> as target.</param>
		public void EmitCallvirt( System.Reflection.MethodInfo target )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( target != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Callvirt );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Callvirt, target );
		}

#if DEBUG
		///	<summary>
		///		Emit 'cpobj' instruction with specified arguments.
		///	</summary>
		///	<param name="type"><see cref="System.Type"/> as type.</param>
		public void EmitCpobj( System.Type type )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( type != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Cpobj );
			this.TraceWrite( " " );
			this.TraceOperand( type );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Cpobj, type );
		}

		///	<summary>
		///		Emit 'ldobj' instruction with specified arguments.
		///	</summary>
		///	<param name="type"><see cref="System.Type"/> as type.</param>
		public void EmitLdobj( System.Type type )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( type != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldobj );
			this.TraceWrite( " " );
			this.TraceOperand( type );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldobj, type );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'ldstr' instruction with specified arguments.
		///	</summary>
		///	<param name="value"><see cref="System.String"/> as value.</param>
		public void EmitLdstr( System.String value )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( value != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldstr );
			this.TraceWrite( " " );
			this.TraceOperand( value );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldstr, value );
		}

		///	<summary>
		///		Emit 'newobj' instruction with specified arguments.
		///	</summary>
		///	<param name="constructor"><see cref="System.Reflection.ConstructorInfo"/> as constructor.</param>
		public void EmitNewobj( System.Reflection.ConstructorInfo constructor )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( constructor != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Newobj );
			this.TraceWrite( " " );
			this.TraceOperand( constructor );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Newobj, constructor );
		}

#if DEBUG
		///	<summary>
		///		Emit 'castclass' instruction with specified arguments.
		///	</summary>
		///	<param name="type"><see cref="System.Type"/> as type.</param>
		public void EmitCastclass( System.Type type )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( type != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Castclass );
			this.TraceWrite( " " );
			this.TraceOperand( type );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Castclass, type );
		}

		///	<summary>
		///		Emit 'isinst' instruction with specified arguments.
		///	</summary>
		///	<param name="type"><see cref="System.Type"/> as type.</param>
		public void EmitIsinst( System.Type type )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( type != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Isinst );
			this.TraceWrite( " " );
			this.TraceOperand( type );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Isinst, type );
		}

		///	<summary>
		///		Emit 'conv.r.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitConv_R_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_R_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_R_Un );
		}

		///	<summary>
		///		Emit 'unbox' instruction with specified arguments.
		///	</summary>
		///	<param name="type"><see cref="System.Type"/> as type.</param>
		public void EmitUnbox( System.Type type )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( type != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Unbox );
			this.TraceWrite( " " );
			this.TraceOperand( type );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Unbox, type );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'throw' instruction with specified arguments.
		///	</summary>
		public void EmitThrow()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Throw );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Throw );
		}

		///	<summary>
		///		Emit 'ldfld' instruction with specified arguments.
		///	</summary>
		///	<param name="field"><see cref="System.Reflection.FieldInfo"/> as field.</param>
		public void EmitLdfld( System.Reflection.FieldInfo field )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( field != null );

			// TODO: NLiblet
#if !SILVERLIGHT
			if ( !( field is FieldBuilder ) )
			{
				if ( field.GetRequiredCustomModifiers().Any( item => typeof( IsVolatile ).Equals( item ) ) )
				{
					this.TraceStart();
					this.TraceOpCode( OpCodes.Volatile );
					this.TraceWriteLine();
				}
			}
#endif

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldfld );
			this.TraceWrite( " " );
			this.TraceOperand( field );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldfld, field );
		}

		///	<summary>
		///		Emit 'ldflda' instruction with specified arguments.
		///	</summary>
		///	<param name="field"><see cref="System.Reflection.FieldInfo"/> as field.</param>
		public void EmitLdflda( System.Reflection.FieldInfo field )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( field != null );

			// TODO: NLiblet
#if !SILVERLIGHT
			if ( !( field is FieldBuilder ) )
			{
				if ( field.GetRequiredCustomModifiers().Any( item => typeof( IsVolatile ).Equals( item ) ) )
				{
					this.TraceStart();
					this.TraceOpCode( OpCodes.Volatile );
					this.TraceWriteLine();
				}
			}
#endif

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldflda );
			this.TraceWrite( " " );
			this.TraceOperand( field );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldflda, field );
		}

		///	<summary>
		///		Emit 'stfld' instruction with specified arguments.
		///	</summary>
		///	<param name="field"><see cref="System.Reflection.FieldInfo"/> as field.</param>
		public void EmitStfld( System.Reflection.FieldInfo field )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( field != null );

			// TODO: NLiblet
#if !SILVERLIGHT
			if ( !( field is FieldBuilder ) )
			{
				if ( field.GetRequiredCustomModifiers().Any( item => typeof( IsVolatile ).Equals( item ) ) )
				{
					this.TraceStart();
					this.TraceOpCode( OpCodes.Volatile );
					this.TraceWriteLine();
				}
			}
#endif

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stfld );
			this.TraceWrite( " " );
			this.TraceOperand( field );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stfld, field );
		}

#if DEBUG
		///	<summary>
		///		Emit 'ldsfld' instruction with specified arguments.
		///	</summary>
		///	<param name="field"><see cref="System.Reflection.FieldInfo"/> as field.</param>
		public void EmitLdsfld( System.Reflection.FieldInfo field )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( field != null );

			// TODO: NLiblet
#if !SILVERLIGHT
			if ( !( field is FieldBuilder ) )
			{
				if ( field.GetRequiredCustomModifiers().Any( item => typeof( IsVolatile ).Equals( item ) ) )
				{
					this.TraceStart();
					this.TraceOpCode( OpCodes.Volatile );
					this.TraceWriteLine();
				}
			}
#endif

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldsfld );
			this.TraceWrite( " " );
			this.TraceOperand( field );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldsfld, field );
		}

		///	<summary>
		///		Emit 'ldsflda' instruction with specified arguments.
		///	</summary>
		///	<param name="field"><see cref="System.Reflection.FieldInfo"/> as field.</param>
		public void EmitLdsflda( System.Reflection.FieldInfo field )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( field != null );

			// TODO: NLiblet
#if !SILVERLIGHT
			if ( !( field is FieldBuilder ) )
			{
				if ( field.GetRequiredCustomModifiers().Any( item => typeof( IsVolatile ).Equals( item ) ) )
				{
					this.TraceStart();
					this.TraceOpCode( OpCodes.Volatile );
					this.TraceWriteLine();
				}
			}
#endif

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldsflda );
			this.TraceWrite( " " );
			this.TraceOperand( field );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldsflda, field );
		}

		///	<summary>
		///		Emit 'stsfld' instruction with specified arguments.
		///	</summary>
		///	<param name="field"><see cref="System.Reflection.FieldInfo"/> as field.</param>
		public void EmitStsfld( System.Reflection.FieldInfo field )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( field != null );

#if !SILVERLIGHT
			// TODO: NLiblet
			if ( !( field is FieldBuilder ) )
			{
				if ( field.GetRequiredCustomModifiers().Any( item => typeof( IsVolatile ).Equals( item ) ) )
				{
					this.TraceStart();
					this.TraceOpCode( OpCodes.Volatile );
					this.TraceWriteLine();
				}
			}
#endif

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stsfld );
			this.TraceWrite( " " );
			this.TraceOperand( field );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stsfld, field );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'stobj' instruction with specified arguments.
		///	</summary>
		///	<param name="type"><see cref="System.Type"/> as type.</param>
		public void EmitStobj( System.Type type )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( type != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stobj );
			this.TraceWrite( " " );
			this.TraceOperand( type );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stobj, type );
		}

#if DEBUG
		///	<summary>
		///		Emit 'conv.ovf.i1.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitConv_Ovf_I1_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_I1_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_I1_Un );
		}

		///	<summary>
		///		Emit 'conv.ovf.i2.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitConv_Ovf_I2_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_I2_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_I2_Un );
		}

		///	<summary>
		///		Emit 'conv.ovf.i4.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitConv_Ovf_I4_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_I4_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_I4_Un );
		}

		///	<summary>
		///		Emit 'conv.ovf.i8.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitConv_Ovf_I8_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_I8_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_I8_Un );
		}

		///	<summary>
		///		Emit 'conv.ovf.u1.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitConv_Ovf_U1_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_U1_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_U1_Un );
		}

		///	<summary>
		///		Emit 'conv.ovf.u2.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitConv_Ovf_U2_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_U2_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_U2_Un );
		}

		///	<summary>
		///		Emit 'conv.ovf.u4.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitConv_Ovf_U4_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_U4_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_U4_Un );
		}

		///	<summary>
		///		Emit 'conv.ovf.u8.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitConv_Ovf_U8_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_U8_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_U8_Un );
		}

		///	<summary>
		///		Emit 'conv.ovf.i.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitConv_Ovf_I_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_I_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_I_Un );
		}

		///	<summary>
		///		Emit 'conv.ovf.u.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitConv_Ovf_U_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_U_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_U_Un );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'box' instruction with specified arguments.
		///	</summary>
		///	<param name="type"><see cref="System.Type"/> as type.</param>
		public void EmitBox( System.Type type )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( type != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Box );
			this.TraceWrite( " " );
			this.TraceOperand( type );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Box, type );
		}

		///	<summary>
		///		Emit 'newarr' instruction with specified arguments.
		///	</summary>
		///	<param name="type"><see cref="System.Type"/> as type.</param>
		public void EmitNewarr( System.Type type )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( type != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Newarr );
			this.TraceWrite( " " );
			this.TraceOperand( type );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Newarr, type );
		}

#if DEBUG
		///	<summary>
		///		Emit 'ldlen' instruction with specified arguments.
		///	</summary>
		public void EmitLdlen()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldlen );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldlen );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'ldelema' instruction with specified arguments.
		///	</summary>
		///	<param name="type"><see cref="System.Type"/> as type.</param>
		public void EmitLdelema( Type type )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( type != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldelema );
			this.TraceWrite( " " );
			this.TraceOperand( type );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldelema, type );
		}

#if DEBUG
		///	<summary>
		///		Emit 'ldelem.i1' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdelem_I1()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldelem_I1 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldelem_I1 );
		}

		///	<summary>
		///		Emit 'ldelem.u1' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdelem_U1()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldelem_U1 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldelem_U1 );
		}

		///	<summary>
		///		Emit 'ldelem.i2' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdelem_I2()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldelem_I2 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldelem_I2 );
		}

		///	<summary>
		///		Emit 'ldelem.u2' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdelem_U2()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldelem_U2 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldelem_U2 );
		}

		///	<summary>
		///		Emit 'ldelem.i4' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdelem_I4()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldelem_I4 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldelem_I4 );
		}

		///	<summary>
		///		Emit 'ldelem.u4' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdelem_U4()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldelem_U4 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldelem_U4 );
		}

		///	<summary>
		///		Emit 'ldelem.i8' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdelem_I8()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldelem_I8 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldelem_I8 );
		}

		///	<summary>
		///		Emit 'ldelem.i' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdelem_I()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldelem_I );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldelem_I );
		}

		///	<summary>
		///		Emit 'ldelem.r4' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdelem_R4()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldelem_R4 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldelem_R4 );
		}

		///	<summary>
		///		Emit 'ldelem.r8' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdelem_R8()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldelem_R8 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldelem_R8 );
		}

		///	<summary>
		///		Emit 'ldelem.ref' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLdelem_Ref()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldelem_Ref );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldelem_Ref );
		}

		///	<summary>
		///		Emit 'stelem.i' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStelem_I()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stelem_I );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stelem_I );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'stelem.i1' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStelem_I1()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stelem_I1 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stelem_I1 );
		}

		///	<summary>
		///		Emit 'stelem.i2' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStelem_I2()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stelem_I2 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stelem_I2 );
		}

		///	<summary>
		///		Emit 'stelem.i4' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStelem_I4()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stelem_I4 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stelem_I4 );
		}

		///	<summary>
		///		Emit 'stelem.i8' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStelem_I8()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stelem_I8 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stelem_I8 );
		}

		///	<summary>
		///		Emit 'stelem.r4' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStelem_R4()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stelem_R4 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stelem_R4 );
		}

		///	<summary>
		///		Emit 'stelem.r8' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStelem_R8()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stelem_R8 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stelem_R8 );
		}

		///	<summary>
		///		Emit 'stelem.ref' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStelem_Ref()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stelem_Ref );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stelem_Ref );
		}

#if DEBUG
		///	<summary>
		///		Emit 'ldelem' instruction with specified arguments.
		///	</summary>
		///	<param name="type"><see cref="System.Type"/> as type.</param>
		public void EmitLdelem( System.Type type )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( type != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldelem );
			this.TraceWrite( " " );
			this.TraceOperand( type );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldelem, type );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'stelem' instruction with specified arguments.
		///	</summary>
		///	<param name="type"><see cref="System.Type"/> as type.</param>
		public void EmitStelem( System.Type type )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( type != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stelem );
			this.TraceWrite( " " );
			this.TraceOperand( type );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stelem, type );
		}

		///	<summary>
		///		Emit 'unbox.any' instruction with specified arguments.
		///	</summary>
		///	<param name="type"><see cref="System.Type"/> as type.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitUnbox_Any( System.Type type )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( type != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Unbox_Any );
			this.TraceWrite( " " );
			this.TraceOperand( type );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Unbox_Any, type );
		}

#if DEBUG
		///	<summary>
		///		Emit 'conv.ovf.i1' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_Ovf_I1()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_I1 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_I1 );
		}

		///	<summary>
		///		Emit 'conv.ovf.u1' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_Ovf_U1()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_U1 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_U1 );
		}

		///	<summary>
		///		Emit 'conv.ovf.i2' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_Ovf_I2()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_I2 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_I2 );
		}

		///	<summary>
		///		Emit 'conv.ovf.u2' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_Ovf_U2()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_U2 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_U2 );
		}

		///	<summary>
		///		Emit 'conv.ovf.i4' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_Ovf_I4()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_I4 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_I4 );
		}

		///	<summary>
		///		Emit 'conv.ovf.u4' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_Ovf_U4()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_U4 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_U4 );
		}

		///	<summary>
		///		Emit 'conv.ovf.i8' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_Ovf_I8()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_I8 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_I8 );
		}

		///	<summary>
		///		Emit 'conv.ovf.u8' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_Ovf_U8()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_U8 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_U8 );
		}

		///	<summary>
		///		Emit 'refanyval' instruction with specified arguments.
		///	</summary>
		///	<param name="type"><see cref="System.Type"/> as type.</param>
		public void EmitRefanyval( System.Type type )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( type != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Refanyval );
			this.TraceWrite( " " );
			this.TraceOperand( type );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Refanyval, type );
		}

		///	<summary>
		///		Emit 'ckfinite' instruction with specified arguments.
		///	</summary>
		public void EmitCkfinite()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ckfinite );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ckfinite );
		}

		///	<summary>
		///		Emit 'mkrefany' instruction with specified arguments.
		///	</summary>
		///	<param name="type"><see cref="System.Type"/> as type.</param>
		public void EmitMkrefany( System.Type type )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( type != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Mkrefany );
			this.TraceWrite( " " );
			this.TraceOperand( type );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Mkrefany, type );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'ldtoken' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Type"/> as target.</param>
		public void EmitLdtoken( System.Type target )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( target != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldtoken );
			this.TraceWrite( " " );
			this.TraceOperandToken( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldtoken, target );
		}

		///	<summary>
		///		Emit 'ldtoken' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.MethodBase"/> as target.</param>
		public void EmitLdtoken( System.Reflection.MethodBase target )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( target != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldtoken );
			this.TraceWrite( " " );
			this.TraceOperandToken( target );
			this.TraceWriteLine();

			System.Reflection.MethodInfo asMethodInfo;
			if ( ( asMethodInfo = target as System.Reflection.MethodInfo ) != null )
			{
				this._underlying.Emit( OpCodes.Ldtoken, asMethodInfo );
			}
			else
			{
				Contract.Assert( target is System.Reflection.ConstructorInfo );
				this._underlying.Emit( OpCodes.Ldtoken, target as System.Reflection.ConstructorInfo );
			}
		}

		///	<summary>
		///		Emit 'ldtoken' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.FieldInfo"/> as target.</param>
		public void EmitLdtoken( System.Reflection.FieldInfo target )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( target != null );

			// TODO: NLiblet
#if !SILVERLIGHT
			if ( !( target is FieldBuilder ) )
			{
				if ( target.GetRequiredCustomModifiers().Any( item => typeof( IsVolatile ).Equals( item ) ) )
				{
					this.TraceStart();
					this.TraceOpCode( OpCodes.Volatile );
					this.TraceWriteLine();
				}
			}
#endif

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldtoken );
			this.TraceWrite( " " );
			this.TraceOperandToken( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldtoken, target );
		}

#if DEBUG
		///	<summary>
		///		Emit 'conv.u2' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_U2()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_U2 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_U2 );
		}

		///	<summary>
		///		Emit 'conv.u1' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_U1()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_U1 );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_U1 );
		}

		///	<summary>
		///		Emit 'conv.i' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_I()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_I );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_I );
		}

		///	<summary>
		///		Emit 'conv.ovf.i' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_Ovf_I()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_I );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_I );
		}

		///	<summary>
		///		Emit 'conv.ovf.u' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_Ovf_U()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_Ovf_U );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_Ovf_U );
		}

		///	<summary>
		///		Emit 'add.ovf' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitAdd_Ovf()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Add_Ovf );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Add_Ovf );
		}

		///	<summary>
		///		Emit 'add.ovf.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitAdd_Ovf_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Add_Ovf_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Add_Ovf_Un );
		}

		///	<summary>
		///		Emit 'mul.ovf' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitMul_Ovf()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Mul_Ovf );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Mul_Ovf );
		}

		///	<summary>
		///		Emit 'mul.ovf.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitMul_Ovf_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Mul_Ovf_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Mul_Ovf_Un );
		}

		///	<summary>
		///		Emit 'sub.ovf' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitSub_Ovf()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Sub_Ovf );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Sub_Ovf );
		}

		///	<summary>
		///		Emit 'sub.ovf.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitSub_Ovf_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Sub_Ovf_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Sub_Ovf_Un );
		}

		///	<summary>
		///		Emit 'endfinally' instruction with specified arguments.
		///	</summary>
		public void EmitEndfinally()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Endfinally );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Endfinally );
		}

		///	<summary>
		///		Emit 'leave' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		public void EmitLeave( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Leave );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Leave, target );
		}

		///	<summary>
		///		Emit 'leave.s' instruction with specified arguments.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.Emit.Label"/> as target.</param>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitLeave_S( System.Reflection.Emit.Label target )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Leave_S );
			this.TraceWrite( " " );
			this.TraceOperand( target );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Leave_S, target );
		}

		///	<summary>
		///		Emit 'stind.i' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitStind_I()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stind_I );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stind_I );
		}

		///	<summary>
		///		Emit 'conv.u' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		public void EmitConv_U()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Conv_U );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Conv_U );
		}

		///	<summary>
		///		Emit 'arglist' instruction with specified arguments.
		///	</summary>
		public void EmitArglist()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Arglist );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Arglist );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'ceq' instruction with specified arguments.
		///	</summary>
		public void EmitCeq()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ceq );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ceq );
		}

		///	<summary>
		///		Emit 'cgt' instruction with specified arguments.
		///	</summary>
		public void EmitCgt()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Cgt );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Cgt );
		}

#if DEBUG
		///	<summary>
		///		Emit 'cgt.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitCgt_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Cgt_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Cgt_Un );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'clt' instruction with specified arguments.
		///	</summary>
		public void EmitClt()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Clt );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Clt );
		}

#if DEBUG
		///	<summary>
		///		Emit 'clt.un' instruction with specified arguments.
		///	</summary>
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "IL itself contains '.', so it must be replaced with '_'." )]
		[SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Un", Justification = "It is IL suffix." )]
		public void EmitClt_Un()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Clt_Un );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Clt_Un );
		}

		///	<summary>
		///		Emit 'ldftn' instruction with specified arguments.
		///	</summary>
		///	<param name="method"><see cref="System.Reflection.MethodInfo"/> as method.</param>
		public void EmitLdftn( System.Reflection.MethodInfo method )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( method != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldftn );
			this.TraceWrite( " " );
			this.TraceOperand( method );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldftn, method );
		}

		///	<summary>
		///		Emit 'ldvirtftn' instruction with specified arguments.
		///	</summary>
		///	<param name="method"><see cref="System.Reflection.MethodInfo"/> as method.</param>
		public void EmitLdvirtftn( System.Reflection.MethodInfo method )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( method != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldvirtftn );
			this.TraceWrite( " " );
			this.TraceOperand( method );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldvirtftn, method );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'ldarg' instruction with specified arguments.
		///	</summary>
		///	<param name="index"><see cref="System.Int32"/> as index.</param>
		public void EmitLdarg( System.Int32 index )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( UInt16.MinValue <= index && index <= UInt16.MaxValue );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldarg );
			this.TraceWrite( " " );
			this.TraceOperand( index );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldarg, index );
		}

		///	<summary>
		///		Emit 'ldarga' instruction with specified arguments.
		///	</summary>
		///	<param name="index"><see cref="System.Int32"/> as index.</param>
		public void EmitLdarga( System.Int32 index )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( UInt16.MinValue <= index && index <= UInt16.MaxValue );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldarga );
			this.TraceWrite( " " );
			this.TraceOperand( index );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldarga, index );
		}

#if DEBUG
		///	<summary>
		///		Emit 'starg' instruction with specified arguments.
		///	</summary>
		///	<param name="index"><see cref="System.Int32"/> as index.</param>
		public void EmitStarg( System.Int32 index )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( UInt16.MinValue <= index && index <= UInt16.MaxValue );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Starg );
			this.TraceWrite( " " );
			this.TraceOperand( index );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Starg, index );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'ldloc' instruction with specified arguments.
		///	</summary>
		///	<param name="index"><see cref="System.Int32"/> as index.</param>
		public void EmitLdloc( System.Int32 index )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( UInt16.MinValue <= index && index <= UInt16.MaxValue );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldloc );
			this.TraceWrite( " " );
			this.TraceOperand( index );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldloc, index );
		}

		///	<summary>
		///		Emit 'ldloca' instruction with specified arguments.
		///	</summary>
		///	<param name="index"><see cref="System.Int32"/> as index.</param>
		public void EmitLdloca( System.Int32 index )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( UInt16.MinValue <= index && index <= UInt16.MaxValue );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ldloca );
			this.TraceWrite( " " );
			this.TraceOperand( index );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Ldloca, index );
		}

		///	<summary>
		///		Emit 'stloc' instruction with specified arguments.
		///	</summary>
		///	<param name="index"><see cref="System.Int32"/> as index.</param>
		public void EmitStloc( System.Int32 index )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( UInt16.MinValue <= index && index <= UInt16.MaxValue );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Stloc );
			this.TraceWrite( " " );
			this.TraceOperand( index );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Stloc, index );
		}

#if DEBUG
		///	<summary>
		///		Emit 'localloc' instruction with specified arguments.
		///	</summary>
		public void EmitLocalloc()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Localloc );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Localloc );
		}

		///	<summary>
		///		Emit 'endfilter' instruction with specified arguments.
		///	</summary>
		public void EmitEndfilter()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Endfilter );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Endfilter );
		}
#endif // DEBUG

		///	<summary>
		///		Emit 'initobj' instruction with specified arguments.
		///	</summary>
		///	<param name="type"><see cref="System.Type"/> as type.</param>
		public void EmitInitobj( System.Type type )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( type != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Initobj );
			this.TraceWrite( " " );
			this.TraceOperand( type );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Initobj, type );
		}

#if DEBUG
		///	<summary>
		///		Emit 'cpblk' instruction with specified arguments.
		///	</summary>
		public void EmitCpblk()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Cpblk );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Cpblk );
		}

		///	<summary>
		///		Emit 'initblk' instruction with specified arguments.
		///	</summary>
		public void EmitInitblk()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Initblk );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Initblk );
		}

		///	<summary>
		///		Emit 'rethrow' instruction with specified arguments.
		///	</summary>
		public void EmitRethrow()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Rethrow );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Rethrow );
		}

		///	<summary>
		///		Emit 'sizeof' instruction with specified arguments.
		///	</summary>
		///	<param name="type"><see cref="System.Type"/> as type.</param>
		public void EmitSizeof( System.Type type )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( type != null );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Sizeof );
			this.TraceWrite( " " );
			this.TraceOperand( type );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Sizeof, type );
		}

		///	<summary>
		///		Emit 'refanytype' instruction with specified arguments.
		///	</summary>
		public void EmitRefanytype()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Refanytype );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Refanytype );
		}
#endif // DEBUG
	}
}
