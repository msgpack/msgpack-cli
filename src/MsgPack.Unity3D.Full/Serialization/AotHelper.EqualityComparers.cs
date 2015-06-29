#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015 FUJIWARA, Yusuke
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

// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier

namespace MsgPack.Serialization
{
	partial class AotHelper
	{
		private static Dictionary<RuntimeTypeHandle, object> InitializeEqualityComparerTable()
		{
			var result = new Dictionary<RuntimeTypeHandle, object>( 190 );
			result.Add( typeof( System.AppDomainManagerInitializationOptions ).TypeHandle, new System_AppDomainManagerInitializationOptionsEqualityComparer() );
			result.Add( typeof( System.AppDomainManagerInitializationOptions? ).TypeHandle, new NullableSystem_AppDomainManagerInitializationOptionsEqualityComparer() );
			result.Add( typeof( System.AttributeTargets ).TypeHandle, new System_AttributeTargetsEqualityComparer() );
			result.Add( typeof( System.AttributeTargets? ).TypeHandle, new NullableSystem_AttributeTargetsEqualityComparer() );
			result.Add( typeof( System.Base64FormattingOptions ).TypeHandle, new System_Base64FormattingOptionsEqualityComparer() );
			result.Add( typeof( System.Base64FormattingOptions? ).TypeHandle, new NullableSystem_Base64FormattingOptionsEqualityComparer() );
			result.Add( typeof( System.Boolean ).TypeHandle, new System_BooleanEqualityComparer() );
			result.Add( typeof( System.Boolean? ).TypeHandle, new NullableSystem_BooleanEqualityComparer() );
			result.Add( typeof( System.Byte ).TypeHandle, new System_ByteEqualityComparer() );
			result.Add( typeof( System.Byte? ).TypeHandle, new NullableSystem_ByteEqualityComparer() );
			result.Add( typeof( System.Char ).TypeHandle, new System_CharEqualityComparer() );
			result.Add( typeof( System.Char? ).TypeHandle, new NullableSystem_CharEqualityComparer() );
			result.Add( typeof( System.ConsoleColor ).TypeHandle, new System_ConsoleColorEqualityComparer() );
			result.Add( typeof( System.ConsoleColor? ).TypeHandle, new NullableSystem_ConsoleColorEqualityComparer() );
			result.Add( typeof( System.ConsoleKey ).TypeHandle, new System_ConsoleKeyEqualityComparer() );
			result.Add( typeof( System.ConsoleKey? ).TypeHandle, new NullableSystem_ConsoleKeyEqualityComparer() );
			result.Add( typeof( System.ConsoleKeyInfo ).TypeHandle, new System_ConsoleKeyInfoEqualityComparer() );
			result.Add( typeof( System.ConsoleKeyInfo? ).TypeHandle, new NullableSystem_ConsoleKeyInfoEqualityComparer() );
			result.Add( typeof( System.ConsoleModifiers ).TypeHandle, new System_ConsoleModifiersEqualityComparer() );
			result.Add( typeof( System.ConsoleModifiers? ).TypeHandle, new NullableSystem_ConsoleModifiersEqualityComparer() );
			result.Add( typeof( System.ConsoleSpecialKey ).TypeHandle, new System_ConsoleSpecialKeyEqualityComparer() );
			result.Add( typeof( System.ConsoleSpecialKey? ).TypeHandle, new NullableSystem_ConsoleSpecialKeyEqualityComparer() );
			result.Add( typeof( System.DateTime ).TypeHandle, new System_DateTimeEqualityComparer() );
			result.Add( typeof( System.DateTime? ).TypeHandle, new NullableSystem_DateTimeEqualityComparer() );
			result.Add( typeof( System.DateTimeKind ).TypeHandle, new System_DateTimeKindEqualityComparer() );
			result.Add( typeof( System.DateTimeKind? ).TypeHandle, new NullableSystem_DateTimeKindEqualityComparer() );
			result.Add( typeof( System.DateTimeOffset ).TypeHandle, new System_DateTimeOffsetEqualityComparer() );
			result.Add( typeof( System.DateTimeOffset? ).TypeHandle, new NullableSystem_DateTimeOffsetEqualityComparer() );
			result.Add( typeof( System.DayOfWeek ).TypeHandle, new System_DayOfWeekEqualityComparer() );
			result.Add( typeof( System.DayOfWeek? ).TypeHandle, new NullableSystem_DayOfWeekEqualityComparer() );
			result.Add( typeof( System.Decimal ).TypeHandle, new System_DecimalEqualityComparer() );
			result.Add( typeof( System.Decimal? ).TypeHandle, new NullableSystem_DecimalEqualityComparer() );
			result.Add( typeof( System.Double ).TypeHandle, new System_DoubleEqualityComparer() );
			result.Add( typeof( System.Double? ).TypeHandle, new NullableSystem_DoubleEqualityComparer() );
			result.Add( typeof( System.EnvironmentVariableTarget ).TypeHandle, new System_EnvironmentVariableTargetEqualityComparer() );
			result.Add( typeof( System.EnvironmentVariableTarget? ).TypeHandle, new NullableSystem_EnvironmentVariableTargetEqualityComparer() );
			result.Add( typeof( System.GCCollectionMode ).TypeHandle, new System_GCCollectionModeEqualityComparer() );
			result.Add( typeof( System.GCCollectionMode? ).TypeHandle, new NullableSystem_GCCollectionModeEqualityComparer() );
			result.Add( typeof( System.GCNotificationStatus ).TypeHandle, new System_GCNotificationStatusEqualityComparer() );
			result.Add( typeof( System.GCNotificationStatus? ).TypeHandle, new NullableSystem_GCNotificationStatusEqualityComparer() );
			result.Add( typeof( System.Guid ).TypeHandle, new System_GuidEqualityComparer() );
			result.Add( typeof( System.Guid? ).TypeHandle, new NullableSystem_GuidEqualityComparer() );
			result.Add( typeof( System.Int16 ).TypeHandle, new System_Int16EqualityComparer() );
			result.Add( typeof( System.Int16? ).TypeHandle, new NullableSystem_Int16EqualityComparer() );
			result.Add( typeof( System.Int32 ).TypeHandle, new System_Int32EqualityComparer() );
			result.Add( typeof( System.Int32? ).TypeHandle, new NullableSystem_Int32EqualityComparer() );
			result.Add( typeof( System.Int64 ).TypeHandle, new System_Int64EqualityComparer() );
			result.Add( typeof( System.Int64? ).TypeHandle, new NullableSystem_Int64EqualityComparer() );
			result.Add( typeof( System.IntPtr ).TypeHandle, new System_IntPtrEqualityComparer() );
			result.Add( typeof( System.IntPtr? ).TypeHandle, new NullableSystem_IntPtrEqualityComparer() );
			result.Add( typeof( System.LoaderOptimization ).TypeHandle, new System_LoaderOptimizationEqualityComparer() );
			result.Add( typeof( System.LoaderOptimization? ).TypeHandle, new NullableSystem_LoaderOptimizationEqualityComparer() );
			result.Add( typeof( System.MidpointRounding ).TypeHandle, new System_MidpointRoundingEqualityComparer() );
			result.Add( typeof( System.MidpointRounding? ).TypeHandle, new NullableSystem_MidpointRoundingEqualityComparer() );
			result.Add( typeof( System.ModuleHandle ).TypeHandle, new System_ModuleHandleEqualityComparer() );
			result.Add( typeof( System.ModuleHandle? ).TypeHandle, new NullableSystem_ModuleHandleEqualityComparer() );
			result.Add( typeof( System.PlatformID ).TypeHandle, new System_PlatformIDEqualityComparer() );
			result.Add( typeof( System.PlatformID? ).TypeHandle, new NullableSystem_PlatformIDEqualityComparer() );
			result.Add( typeof( System.RuntimeFieldHandle ).TypeHandle, new System_RuntimeFieldHandleEqualityComparer() );
			result.Add( typeof( System.RuntimeFieldHandle? ).TypeHandle, new NullableSystem_RuntimeFieldHandleEqualityComparer() );
			result.Add( typeof( System.RuntimeMethodHandle ).TypeHandle, new System_RuntimeMethodHandleEqualityComparer() );
			result.Add( typeof( System.RuntimeMethodHandle? ).TypeHandle, new NullableSystem_RuntimeMethodHandleEqualityComparer() );
			result.Add( typeof( System.RuntimeTypeHandle ).TypeHandle, new System_RuntimeTypeHandleEqualityComparer() );
			result.Add( typeof( System.RuntimeTypeHandle? ).TypeHandle, new NullableSystem_RuntimeTypeHandleEqualityComparer() );
			result.Add( typeof( System.SByte ).TypeHandle, new System_SByteEqualityComparer() );
			result.Add( typeof( System.SByte? ).TypeHandle, new NullableSystem_SByteEqualityComparer() );
			result.Add( typeof( System.Single ).TypeHandle, new System_SingleEqualityComparer() );
			result.Add( typeof( System.Single? ).TypeHandle, new NullableSystem_SingleEqualityComparer() );
			result.Add( typeof( System.StringComparison ).TypeHandle, new System_StringComparisonEqualityComparer() );
			result.Add( typeof( System.StringComparison? ).TypeHandle, new NullableSystem_StringComparisonEqualityComparer() );
			result.Add( typeof( System.StringSplitOptions ).TypeHandle, new System_StringSplitOptionsEqualityComparer() );
			result.Add( typeof( System.StringSplitOptions? ).TypeHandle, new NullableSystem_StringSplitOptionsEqualityComparer() );
			result.Add( typeof( System.TimeSpan ).TypeHandle, new System_TimeSpanEqualityComparer() );
			result.Add( typeof( System.TimeSpan? ).TypeHandle, new NullableSystem_TimeSpanEqualityComparer() );
			result.Add( typeof( System.TypeCode ).TypeHandle, new System_TypeCodeEqualityComparer() );
			result.Add( typeof( System.TypeCode? ).TypeHandle, new NullableSystem_TypeCodeEqualityComparer() );
			result.Add( typeof( System.UInt16 ).TypeHandle, new System_UInt16EqualityComparer() );
			result.Add( typeof( System.UInt16? ).TypeHandle, new NullableSystem_UInt16EqualityComparer() );
			result.Add( typeof( System.UInt32 ).TypeHandle, new System_UInt32EqualityComparer() );
			result.Add( typeof( System.UInt32? ).TypeHandle, new NullableSystem_UInt32EqualityComparer() );
			result.Add( typeof( System.UInt64 ).TypeHandle, new System_UInt64EqualityComparer() );
			result.Add( typeof( System.UInt64? ).TypeHandle, new NullableSystem_UInt64EqualityComparer() );
			result.Add( typeof( System.UIntPtr ).TypeHandle, new System_UIntPtrEqualityComparer() );
			result.Add( typeof( System.UIntPtr? ).TypeHandle, new NullableSystem_UIntPtrEqualityComparer() );
			result.Add( typeof( System.Collections.DictionaryEntry ).TypeHandle, new System_Collections_DictionaryEntryEqualityComparer() );
			result.Add( typeof( System.Collections.DictionaryEntry? ).TypeHandle, new NullableSystem_Collections_DictionaryEntryEqualityComparer() );
			result.Add( typeof( System.Diagnostics.DebuggerBrowsableState ).TypeHandle, new System_Diagnostics_DebuggerBrowsableStateEqualityComparer() );
			result.Add( typeof( System.Diagnostics.DebuggerBrowsableState? ).TypeHandle, new NullableSystem_Diagnostics_DebuggerBrowsableStateEqualityComparer() );
			result.Add( typeof( System.Diagnostics.SymbolStore.SymAddressKind ).TypeHandle, new System_Diagnostics_SymbolStore_SymAddressKindEqualityComparer() );
			result.Add( typeof( System.Diagnostics.SymbolStore.SymAddressKind? ).TypeHandle, new NullableSystem_Diagnostics_SymbolStore_SymAddressKindEqualityComparer() );
			result.Add( typeof( System.Diagnostics.SymbolStore.SymbolToken ).TypeHandle, new System_Diagnostics_SymbolStore_SymbolTokenEqualityComparer() );
			result.Add( typeof( System.Diagnostics.SymbolStore.SymbolToken? ).TypeHandle, new NullableSystem_Diagnostics_SymbolStore_SymbolTokenEqualityComparer() );
			result.Add( typeof( System.Globalization.CalendarAlgorithmType ).TypeHandle, new System_Globalization_CalendarAlgorithmTypeEqualityComparer() );
			result.Add( typeof( System.Globalization.CalendarAlgorithmType? ).TypeHandle, new NullableSystem_Globalization_CalendarAlgorithmTypeEqualityComparer() );
			result.Add( typeof( System.Globalization.CalendarWeekRule ).TypeHandle, new System_Globalization_CalendarWeekRuleEqualityComparer() );
			result.Add( typeof( System.Globalization.CalendarWeekRule? ).TypeHandle, new NullableSystem_Globalization_CalendarWeekRuleEqualityComparer() );
			result.Add( typeof( System.Globalization.CompareOptions ).TypeHandle, new System_Globalization_CompareOptionsEqualityComparer() );
			result.Add( typeof( System.Globalization.CompareOptions? ).TypeHandle, new NullableSystem_Globalization_CompareOptionsEqualityComparer() );
			result.Add( typeof( System.Globalization.CultureTypes ).TypeHandle, new System_Globalization_CultureTypesEqualityComparer() );
			result.Add( typeof( System.Globalization.CultureTypes? ).TypeHandle, new NullableSystem_Globalization_CultureTypesEqualityComparer() );
			result.Add( typeof( System.Globalization.DateTimeStyles ).TypeHandle, new System_Globalization_DateTimeStylesEqualityComparer() );
			result.Add( typeof( System.Globalization.DateTimeStyles? ).TypeHandle, new NullableSystem_Globalization_DateTimeStylesEqualityComparer() );
			result.Add( typeof( System.Globalization.DigitShapes ).TypeHandle, new System_Globalization_DigitShapesEqualityComparer() );
			result.Add( typeof( System.Globalization.DigitShapes? ).TypeHandle, new NullableSystem_Globalization_DigitShapesEqualityComparer() );
			result.Add( typeof( System.Globalization.GregorianCalendarTypes ).TypeHandle, new System_Globalization_GregorianCalendarTypesEqualityComparer() );
			result.Add( typeof( System.Globalization.GregorianCalendarTypes? ).TypeHandle, new NullableSystem_Globalization_GregorianCalendarTypesEqualityComparer() );
			result.Add( typeof( System.Globalization.NumberStyles ).TypeHandle, new System_Globalization_NumberStylesEqualityComparer() );
			result.Add( typeof( System.Globalization.NumberStyles? ).TypeHandle, new NullableSystem_Globalization_NumberStylesEqualityComparer() );
			result.Add( typeof( System.Globalization.UnicodeCategory ).TypeHandle, new System_Globalization_UnicodeCategoryEqualityComparer() );
			result.Add( typeof( System.Globalization.UnicodeCategory? ).TypeHandle, new NullableSystem_Globalization_UnicodeCategoryEqualityComparer() );
			result.Add( typeof( System.IO.DriveType ).TypeHandle, new System_IO_DriveTypeEqualityComparer() );
			result.Add( typeof( System.IO.DriveType? ).TypeHandle, new NullableSystem_IO_DriveTypeEqualityComparer() );
			result.Add( typeof( System.IO.FileAccess ).TypeHandle, new System_IO_FileAccessEqualityComparer() );
			result.Add( typeof( System.IO.FileAccess? ).TypeHandle, new NullableSystem_IO_FileAccessEqualityComparer() );
			result.Add( typeof( System.IO.FileAttributes ).TypeHandle, new System_IO_FileAttributesEqualityComparer() );
			result.Add( typeof( System.IO.FileAttributes? ).TypeHandle, new NullableSystem_IO_FileAttributesEqualityComparer() );
			result.Add( typeof( System.IO.FileMode ).TypeHandle, new System_IO_FileModeEqualityComparer() );
			result.Add( typeof( System.IO.FileMode? ).TypeHandle, new NullableSystem_IO_FileModeEqualityComparer() );
			result.Add( typeof( System.IO.FileOptions ).TypeHandle, new System_IO_FileOptionsEqualityComparer() );
			result.Add( typeof( System.IO.FileOptions? ).TypeHandle, new NullableSystem_IO_FileOptionsEqualityComparer() );
			result.Add( typeof( System.IO.FileShare ).TypeHandle, new System_IO_FileShareEqualityComparer() );
			result.Add( typeof( System.IO.FileShare? ).TypeHandle, new NullableSystem_IO_FileShareEqualityComparer() );
#if MSGPACK_UNITY_FULL
			result.Add( typeof( System.IO.HandleInheritability ).TypeHandle, new System_IO_HandleInheritabilityEqualityComparer() );
			result.Add( typeof( System.IO.HandleInheritability? ).TypeHandle, new NullableSystem_IO_HandleInheritabilityEqualityComparer() );
#endif // MSGPACK_UNITY_FULL
			result.Add( typeof( System.IO.SearchOption ).TypeHandle, new System_IO_SearchOptionEqualityComparer() );
			result.Add( typeof( System.IO.SearchOption? ).TypeHandle, new NullableSystem_IO_SearchOptionEqualityComparer() );
			result.Add( typeof( System.IO.SeekOrigin ).TypeHandle, new System_IO_SeekOriginEqualityComparer() );
			result.Add( typeof( System.IO.SeekOrigin? ).TypeHandle, new NullableSystem_IO_SeekOriginEqualityComparer() );
#if MSGPACK_UNITY_FULL
			result.Add( typeof( System.IO.Pipes.PipeAccessRights ).TypeHandle, new System_IO_Pipes_PipeAccessRightsEqualityComparer() );
			result.Add( typeof( System.IO.Pipes.PipeAccessRights? ).TypeHandle, new NullableSystem_IO_Pipes_PipeAccessRightsEqualityComparer() );
#endif // MSGPACK_UNITY_FULL
#if MSGPACK_UNITY_FULL
			result.Add( typeof( System.IO.Pipes.PipeDirection ).TypeHandle, new System_IO_Pipes_PipeDirectionEqualityComparer() );
			result.Add( typeof( System.IO.Pipes.PipeDirection? ).TypeHandle, new NullableSystem_IO_Pipes_PipeDirectionEqualityComparer() );
#endif // MSGPACK_UNITY_FULL
#if MSGPACK_UNITY_FULL
			result.Add( typeof( System.IO.Pipes.PipeOptions ).TypeHandle, new System_IO_Pipes_PipeOptionsEqualityComparer() );
			result.Add( typeof( System.IO.Pipes.PipeOptions? ).TypeHandle, new NullableSystem_IO_Pipes_PipeOptionsEqualityComparer() );
#endif // MSGPACK_UNITY_FULL
#if MSGPACK_UNITY_FULL
			result.Add( typeof( System.IO.Pipes.PipeTransmissionMode ).TypeHandle, new System_IO_Pipes_PipeTransmissionModeEqualityComparer() );
			result.Add( typeof( System.IO.Pipes.PipeTransmissionMode? ).TypeHandle, new NullableSystem_IO_Pipes_PipeTransmissionModeEqualityComparer() );
#endif // MSGPACK_UNITY_FULL
			result.Add( typeof( System.Reflection.AssemblyNameFlags ).TypeHandle, new System_Reflection_AssemblyNameFlagsEqualityComparer() );
			result.Add( typeof( System.Reflection.AssemblyNameFlags? ).TypeHandle, new NullableSystem_Reflection_AssemblyNameFlagsEqualityComparer() );
			result.Add( typeof( System.Reflection.BindingFlags ).TypeHandle, new System_Reflection_BindingFlagsEqualityComparer() );
			result.Add( typeof( System.Reflection.BindingFlags? ).TypeHandle, new NullableSystem_Reflection_BindingFlagsEqualityComparer() );
			result.Add( typeof( System.Reflection.CallingConventions ).TypeHandle, new System_Reflection_CallingConventionsEqualityComparer() );
			result.Add( typeof( System.Reflection.CallingConventions? ).TypeHandle, new NullableSystem_Reflection_CallingConventionsEqualityComparer() );
			result.Add( typeof( System.Reflection.CustomAttributeNamedArgument ).TypeHandle, new System_Reflection_CustomAttributeNamedArgumentEqualityComparer() );
			result.Add( typeof( System.Reflection.CustomAttributeNamedArgument? ).TypeHandle, new NullableSystem_Reflection_CustomAttributeNamedArgumentEqualityComparer() );
			result.Add( typeof( System.Reflection.CustomAttributeTypedArgument ).TypeHandle, new System_Reflection_CustomAttributeTypedArgumentEqualityComparer() );
			result.Add( typeof( System.Reflection.CustomAttributeTypedArgument? ).TypeHandle, new NullableSystem_Reflection_CustomAttributeTypedArgumentEqualityComparer() );
			result.Add( typeof( System.Reflection.EventAttributes ).TypeHandle, new System_Reflection_EventAttributesEqualityComparer() );
			result.Add( typeof( System.Reflection.EventAttributes? ).TypeHandle, new NullableSystem_Reflection_EventAttributesEqualityComparer() );
			result.Add( typeof( System.Reflection.ExceptionHandlingClauseOptions ).TypeHandle, new System_Reflection_ExceptionHandlingClauseOptionsEqualityComparer() );
			result.Add( typeof( System.Reflection.ExceptionHandlingClauseOptions? ).TypeHandle, new NullableSystem_Reflection_ExceptionHandlingClauseOptionsEqualityComparer() );
			result.Add( typeof( System.Reflection.FieldAttributes ).TypeHandle, new System_Reflection_FieldAttributesEqualityComparer() );
			result.Add( typeof( System.Reflection.FieldAttributes? ).TypeHandle, new NullableSystem_Reflection_FieldAttributesEqualityComparer() );
			result.Add( typeof( System.Reflection.GenericParameterAttributes ).TypeHandle, new System_Reflection_GenericParameterAttributesEqualityComparer() );
			result.Add( typeof( System.Reflection.GenericParameterAttributes? ).TypeHandle, new NullableSystem_Reflection_GenericParameterAttributesEqualityComparer() );
			result.Add( typeof( System.Reflection.ImageFileMachine ).TypeHandle, new System_Reflection_ImageFileMachineEqualityComparer() );
			result.Add( typeof( System.Reflection.ImageFileMachine? ).TypeHandle, new NullableSystem_Reflection_ImageFileMachineEqualityComparer() );
			result.Add( typeof( System.Reflection.InterfaceMapping ).TypeHandle, new System_Reflection_InterfaceMappingEqualityComparer() );
			result.Add( typeof( System.Reflection.InterfaceMapping? ).TypeHandle, new NullableSystem_Reflection_InterfaceMappingEqualityComparer() );
			result.Add( typeof( System.Reflection.MemberTypes ).TypeHandle, new System_Reflection_MemberTypesEqualityComparer() );
			result.Add( typeof( System.Reflection.MemberTypes? ).TypeHandle, new NullableSystem_Reflection_MemberTypesEqualityComparer() );
			result.Add( typeof( System.Reflection.MethodAttributes ).TypeHandle, new System_Reflection_MethodAttributesEqualityComparer() );
			result.Add( typeof( System.Reflection.MethodAttributes? ).TypeHandle, new NullableSystem_Reflection_MethodAttributesEqualityComparer() );
			result.Add( typeof( System.Reflection.MethodImplAttributes ).TypeHandle, new System_Reflection_MethodImplAttributesEqualityComparer() );
			result.Add( typeof( System.Reflection.MethodImplAttributes? ).TypeHandle, new NullableSystem_Reflection_MethodImplAttributesEqualityComparer() );
			result.Add( typeof( System.Reflection.ParameterAttributes ).TypeHandle, new System_Reflection_ParameterAttributesEqualityComparer() );
			result.Add( typeof( System.Reflection.ParameterAttributes? ).TypeHandle, new NullableSystem_Reflection_ParameterAttributesEqualityComparer() );
			result.Add( typeof( System.Reflection.ParameterModifier ).TypeHandle, new System_Reflection_ParameterModifierEqualityComparer() );
			result.Add( typeof( System.Reflection.ParameterModifier? ).TypeHandle, new NullableSystem_Reflection_ParameterModifierEqualityComparer() );
			result.Add( typeof( System.Reflection.PortableExecutableKinds ).TypeHandle, new System_Reflection_PortableExecutableKindsEqualityComparer() );
			result.Add( typeof( System.Reflection.PortableExecutableKinds? ).TypeHandle, new NullableSystem_Reflection_PortableExecutableKindsEqualityComparer() );
			result.Add( typeof( System.Reflection.ProcessorArchitecture ).TypeHandle, new System_Reflection_ProcessorArchitectureEqualityComparer() );
			result.Add( typeof( System.Reflection.ProcessorArchitecture? ).TypeHandle, new NullableSystem_Reflection_ProcessorArchitectureEqualityComparer() );
			result.Add( typeof( System.Reflection.PropertyAttributes ).TypeHandle, new System_Reflection_PropertyAttributesEqualityComparer() );
			result.Add( typeof( System.Reflection.PropertyAttributes? ).TypeHandle, new NullableSystem_Reflection_PropertyAttributesEqualityComparer() );
			result.Add( typeof( System.Reflection.ResourceAttributes ).TypeHandle, new System_Reflection_ResourceAttributesEqualityComparer() );
			result.Add( typeof( System.Reflection.ResourceAttributes? ).TypeHandle, new NullableSystem_Reflection_ResourceAttributesEqualityComparer() );
			result.Add( typeof( System.Reflection.ResourceLocation ).TypeHandle, new System_Reflection_ResourceLocationEqualityComparer() );
			result.Add( typeof( System.Reflection.ResourceLocation? ).TypeHandle, new NullableSystem_Reflection_ResourceLocationEqualityComparer() );
			result.Add( typeof( System.Reflection.TypeAttributes ).TypeHandle, new System_Reflection_TypeAttributesEqualityComparer() );
			result.Add( typeof( System.Reflection.TypeAttributes? ).TypeHandle, new NullableSystem_Reflection_TypeAttributesEqualityComparer() );
			result.Add( typeof( System.Resources.UltimateResourceFallbackLocation ).TypeHandle, new System_Resources_UltimateResourceFallbackLocationEqualityComparer() );
			result.Add( typeof( System.Resources.UltimateResourceFallbackLocation? ).TypeHandle, new NullableSystem_Resources_UltimateResourceFallbackLocationEqualityComparer() );
			result.Add( typeof( System.Runtime.GCLatencyMode ).TypeHandle, new System_Runtime_GCLatencyModeEqualityComparer() );
			result.Add( typeof( System.Runtime.GCLatencyMode? ).TypeHandle, new NullableSystem_Runtime_GCLatencyModeEqualityComparer() );
			result.Add( typeof( System.Runtime.CompilerServices.CompilationRelaxations ).TypeHandle, new System_Runtime_CompilerServices_CompilationRelaxationsEqualityComparer() );
			result.Add( typeof( System.Runtime.CompilerServices.CompilationRelaxations? ).TypeHandle, new NullableSystem_Runtime_CompilerServices_CompilationRelaxationsEqualityComparer() );
			result.Add( typeof( System.Runtime.CompilerServices.LoadHint ).TypeHandle, new System_Runtime_CompilerServices_LoadHintEqualityComparer() );
			result.Add( typeof( System.Runtime.CompilerServices.LoadHint? ).TypeHandle, new NullableSystem_Runtime_CompilerServices_LoadHintEqualityComparer() );
			result.Add( typeof( System.Runtime.CompilerServices.MethodCodeType ).TypeHandle, new System_Runtime_CompilerServices_MethodCodeTypeEqualityComparer() );
			result.Add( typeof( System.Runtime.CompilerServices.MethodCodeType? ).TypeHandle, new NullableSystem_Runtime_CompilerServices_MethodCodeTypeEqualityComparer() );
			result.Add( typeof( System.Runtime.CompilerServices.MethodImplOptions ).TypeHandle, new System_Runtime_CompilerServices_MethodImplOptionsEqualityComparer() );
			result.Add( typeof( System.Runtime.CompilerServices.MethodImplOptions? ).TypeHandle, new NullableSystem_Runtime_CompilerServices_MethodImplOptionsEqualityComparer() );
			result.Add( typeof( System.Runtime.ConstrainedExecution.Cer ).TypeHandle, new System_Runtime_ConstrainedExecution_CerEqualityComparer() );
			result.Add( typeof( System.Runtime.ConstrainedExecution.Cer? ).TypeHandle, new NullableSystem_Runtime_ConstrainedExecution_CerEqualityComparer() );
			result.Add( typeof( System.Runtime.ConstrainedExecution.Consistency ).TypeHandle, new System_Runtime_ConstrainedExecution_ConsistencyEqualityComparer() );
			result.Add( typeof( System.Runtime.ConstrainedExecution.Consistency? ).TypeHandle, new NullableSystem_Runtime_ConstrainedExecution_ConsistencyEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.ArrayWithOffset ).TypeHandle, new System_Runtime_InteropServices_ArrayWithOffsetEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.ArrayWithOffset? ).TypeHandle, new NullableSystem_Runtime_InteropServices_ArrayWithOffsetEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.AssemblyRegistrationFlags ).TypeHandle, new System_Runtime_InteropServices_AssemblyRegistrationFlagsEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.AssemblyRegistrationFlags? ).TypeHandle, new NullableSystem_Runtime_InteropServices_AssemblyRegistrationFlagsEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.CallingConvention ).TypeHandle, new System_Runtime_InteropServices_CallingConventionEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.CallingConvention? ).TypeHandle, new NullableSystem_Runtime_InteropServices_CallingConventionEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.CharSet ).TypeHandle, new System_Runtime_InteropServices_CharSetEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.CharSet? ).TypeHandle, new NullableSystem_Runtime_InteropServices_CharSetEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.ClassInterfaceType ).TypeHandle, new System_Runtime_InteropServices_ClassInterfaceTypeEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.ClassInterfaceType? ).TypeHandle, new NullableSystem_Runtime_InteropServices_ClassInterfaceTypeEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.ComInterfaceType ).TypeHandle, new System_Runtime_InteropServices_ComInterfaceTypeEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.ComInterfaceType? ).TypeHandle, new NullableSystem_Runtime_InteropServices_ComInterfaceTypeEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.ComMemberType ).TypeHandle, new System_Runtime_InteropServices_ComMemberTypeEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.ComMemberType? ).TypeHandle, new NullableSystem_Runtime_InteropServices_ComMemberTypeEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.ExporterEventKind ).TypeHandle, new System_Runtime_InteropServices_ExporterEventKindEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.ExporterEventKind? ).TypeHandle, new NullableSystem_Runtime_InteropServices_ExporterEventKindEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.GCHandle ).TypeHandle, new System_Runtime_InteropServices_GCHandleEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.GCHandle? ).TypeHandle, new NullableSystem_Runtime_InteropServices_GCHandleEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.GCHandleType ).TypeHandle, new System_Runtime_InteropServices_GCHandleTypeEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.GCHandleType? ).TypeHandle, new NullableSystem_Runtime_InteropServices_GCHandleTypeEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.HandleRef ).TypeHandle, new System_Runtime_InteropServices_HandleRefEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.HandleRef? ).TypeHandle, new NullableSystem_Runtime_InteropServices_HandleRefEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.ImporterEventKind ).TypeHandle, new System_Runtime_InteropServices_ImporterEventKindEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.ImporterEventKind? ).TypeHandle, new NullableSystem_Runtime_InteropServices_ImporterEventKindEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.LayoutKind ).TypeHandle, new System_Runtime_InteropServices_LayoutKindEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.LayoutKind? ).TypeHandle, new NullableSystem_Runtime_InteropServices_LayoutKindEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.RegistrationClassContext ).TypeHandle, new System_Runtime_InteropServices_RegistrationClassContextEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.RegistrationClassContext? ).TypeHandle, new NullableSystem_Runtime_InteropServices_RegistrationClassContextEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.RegistrationConnectionType ).TypeHandle, new System_Runtime_InteropServices_RegistrationConnectionTypeEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.RegistrationConnectionType? ).TypeHandle, new NullableSystem_Runtime_InteropServices_RegistrationConnectionTypeEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.TypeLibExporterFlags ).TypeHandle, new System_Runtime_InteropServices_TypeLibExporterFlagsEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.TypeLibExporterFlags? ).TypeHandle, new NullableSystem_Runtime_InteropServices_TypeLibExporterFlagsEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.TypeLibFuncFlags ).TypeHandle, new System_Runtime_InteropServices_TypeLibFuncFlagsEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.TypeLibFuncFlags? ).TypeHandle, new NullableSystem_Runtime_InteropServices_TypeLibFuncFlagsEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.TypeLibImporterFlags ).TypeHandle, new System_Runtime_InteropServices_TypeLibImporterFlagsEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.TypeLibImporterFlags? ).TypeHandle, new NullableSystem_Runtime_InteropServices_TypeLibImporterFlagsEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.TypeLibTypeFlags ).TypeHandle, new System_Runtime_InteropServices_TypeLibTypeFlagsEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.TypeLibTypeFlags? ).TypeHandle, new NullableSystem_Runtime_InteropServices_TypeLibTypeFlagsEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.TypeLibVarFlags ).TypeHandle, new System_Runtime_InteropServices_TypeLibVarFlagsEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.TypeLibVarFlags? ).TypeHandle, new NullableSystem_Runtime_InteropServices_TypeLibVarFlagsEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.UnmanagedType ).TypeHandle, new System_Runtime_InteropServices_UnmanagedTypeEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.UnmanagedType? ).TypeHandle, new NullableSystem_Runtime_InteropServices_UnmanagedTypeEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.VarEnum ).TypeHandle, new System_Runtime_InteropServices_VarEnumEqualityComparer() );
			result.Add( typeof( System.Runtime.InteropServices.VarEnum? ).TypeHandle, new NullableSystem_Runtime_InteropServices_VarEnumEqualityComparer() );
			result.Add( typeof( System.Runtime.Remoting.CustomErrorsModes ).TypeHandle, new System_Runtime_Remoting_CustomErrorsModesEqualityComparer() );
			result.Add( typeof( System.Runtime.Remoting.CustomErrorsModes? ).TypeHandle, new NullableSystem_Runtime_Remoting_CustomErrorsModesEqualityComparer() );
			result.Add( typeof( System.Runtime.Remoting.WellKnownObjectMode ).TypeHandle, new System_Runtime_Remoting_WellKnownObjectModeEqualityComparer() );
			result.Add( typeof( System.Runtime.Remoting.WellKnownObjectMode? ).TypeHandle, new NullableSystem_Runtime_Remoting_WellKnownObjectModeEqualityComparer() );
			result.Add( typeof( System.Runtime.Remoting.Activation.ActivatorLevel ).TypeHandle, new System_Runtime_Remoting_Activation_ActivatorLevelEqualityComparer() );
			result.Add( typeof( System.Runtime.Remoting.Activation.ActivatorLevel? ).TypeHandle, new NullableSystem_Runtime_Remoting_Activation_ActivatorLevelEqualityComparer() );
			result.Add( typeof( System.Runtime.Remoting.Channels.ServerProcessing ).TypeHandle, new System_Runtime_Remoting_Channels_ServerProcessingEqualityComparer() );
			result.Add( typeof( System.Runtime.Remoting.Channels.ServerProcessing? ).TypeHandle, new NullableSystem_Runtime_Remoting_Channels_ServerProcessingEqualityComparer() );
			result.Add( typeof( System.Runtime.Remoting.Lifetime.LeaseState ).TypeHandle, new System_Runtime_Remoting_Lifetime_LeaseStateEqualityComparer() );
			result.Add( typeof( System.Runtime.Remoting.Lifetime.LeaseState? ).TypeHandle, new NullableSystem_Runtime_Remoting_Lifetime_LeaseStateEqualityComparer() );
			result.Add( typeof( System.Runtime.Remoting.Metadata.SoapOption ).TypeHandle, new System_Runtime_Remoting_Metadata_SoapOptionEqualityComparer() );
			result.Add( typeof( System.Runtime.Remoting.Metadata.SoapOption? ).TypeHandle, new NullableSystem_Runtime_Remoting_Metadata_SoapOptionEqualityComparer() );
			result.Add( typeof( System.Runtime.Remoting.Metadata.XmlFieldOrderOption ).TypeHandle, new System_Runtime_Remoting_Metadata_XmlFieldOrderOptionEqualityComparer() );
			result.Add( typeof( System.Runtime.Remoting.Metadata.XmlFieldOrderOption? ).TypeHandle, new NullableSystem_Runtime_Remoting_Metadata_XmlFieldOrderOptionEqualityComparer() );
			result.Add( typeof( System.Runtime.Serialization.SerializationEntry ).TypeHandle, new System_Runtime_Serialization_SerializationEntryEqualityComparer() );
			result.Add( typeof( System.Runtime.Serialization.SerializationEntry? ).TypeHandle, new NullableSystem_Runtime_Serialization_SerializationEntryEqualityComparer() );
			result.Add( typeof( System.Runtime.Serialization.StreamingContext ).TypeHandle, new System_Runtime_Serialization_StreamingContextEqualityComparer() );
			result.Add( typeof( System.Runtime.Serialization.StreamingContext? ).TypeHandle, new NullableSystem_Runtime_Serialization_StreamingContextEqualityComparer() );
			result.Add( typeof( System.Runtime.Serialization.StreamingContextStates ).TypeHandle, new System_Runtime_Serialization_StreamingContextStatesEqualityComparer() );
			result.Add( typeof( System.Runtime.Serialization.StreamingContextStates? ).TypeHandle, new NullableSystem_Runtime_Serialization_StreamingContextStatesEqualityComparer() );
			result.Add( typeof( System.Runtime.Serialization.Formatters.FormatterAssemblyStyle ).TypeHandle, new System_Runtime_Serialization_Formatters_FormatterAssemblyStyleEqualityComparer() );
			result.Add( typeof( System.Runtime.Serialization.Formatters.FormatterAssemblyStyle? ).TypeHandle, new NullableSystem_Runtime_Serialization_Formatters_FormatterAssemblyStyleEqualityComparer() );
			result.Add( typeof( System.Runtime.Serialization.Formatters.FormatterTypeStyle ).TypeHandle, new System_Runtime_Serialization_Formatters_FormatterTypeStyleEqualityComparer() );
			result.Add( typeof( System.Runtime.Serialization.Formatters.FormatterTypeStyle? ).TypeHandle, new NullableSystem_Runtime_Serialization_Formatters_FormatterTypeStyleEqualityComparer() );
			result.Add( typeof( System.Runtime.Serialization.Formatters.TypeFilterLevel ).TypeHandle, new System_Runtime_Serialization_Formatters_TypeFilterLevelEqualityComparer() );
			result.Add( typeof( System.Runtime.Serialization.Formatters.TypeFilterLevel? ).TypeHandle, new NullableSystem_Runtime_Serialization_Formatters_TypeFilterLevelEqualityComparer() );
			result.Add( typeof( System.Runtime.Versioning.ResourceScope ).TypeHandle, new System_Runtime_Versioning_ResourceScopeEqualityComparer() );
			result.Add( typeof( System.Runtime.Versioning.ResourceScope? ).TypeHandle, new NullableSystem_Runtime_Versioning_ResourceScopeEqualityComparer() );
			result.Add( typeof( System.Security.HostSecurityManagerOptions ).TypeHandle, new System_Security_HostSecurityManagerOptionsEqualityComparer() );
			result.Add( typeof( System.Security.HostSecurityManagerOptions? ).TypeHandle, new NullableSystem_Security_HostSecurityManagerOptionsEqualityComparer() );
			result.Add( typeof( System.Security.PolicyLevelType ).TypeHandle, new System_Security_PolicyLevelTypeEqualityComparer() );
			result.Add( typeof( System.Security.PolicyLevelType? ).TypeHandle, new NullableSystem_Security_PolicyLevelTypeEqualityComparer() );
			result.Add( typeof( System.Security.SecurityZone ).TypeHandle, new System_Security_SecurityZoneEqualityComparer() );
			result.Add( typeof( System.Security.SecurityZone? ).TypeHandle, new NullableSystem_Security_SecurityZoneEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.AccessControlActions ).TypeHandle, new System_Security_AccessControl_AccessControlActionsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.AccessControlActions? ).TypeHandle, new NullableSystem_Security_AccessControl_AccessControlActionsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.AccessControlModification ).TypeHandle, new System_Security_AccessControl_AccessControlModificationEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.AccessControlModification? ).TypeHandle, new NullableSystem_Security_AccessControl_AccessControlModificationEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.AccessControlSections ).TypeHandle, new System_Security_AccessControl_AccessControlSectionsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.AccessControlSections? ).TypeHandle, new NullableSystem_Security_AccessControl_AccessControlSectionsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.AccessControlType ).TypeHandle, new System_Security_AccessControl_AccessControlTypeEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.AccessControlType? ).TypeHandle, new NullableSystem_Security_AccessControl_AccessControlTypeEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.AceFlags ).TypeHandle, new System_Security_AccessControl_AceFlagsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.AceFlags? ).TypeHandle, new NullableSystem_Security_AccessControl_AceFlagsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.AceQualifier ).TypeHandle, new System_Security_AccessControl_AceQualifierEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.AceQualifier? ).TypeHandle, new NullableSystem_Security_AccessControl_AceQualifierEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.AceType ).TypeHandle, new System_Security_AccessControl_AceTypeEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.AceType? ).TypeHandle, new NullableSystem_Security_AccessControl_AceTypeEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.AuditFlags ).TypeHandle, new System_Security_AccessControl_AuditFlagsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.AuditFlags? ).TypeHandle, new NullableSystem_Security_AccessControl_AuditFlagsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.CompoundAceType ).TypeHandle, new System_Security_AccessControl_CompoundAceTypeEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.CompoundAceType? ).TypeHandle, new NullableSystem_Security_AccessControl_CompoundAceTypeEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.ControlFlags ).TypeHandle, new System_Security_AccessControl_ControlFlagsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.ControlFlags? ).TypeHandle, new NullableSystem_Security_AccessControl_ControlFlagsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.CryptoKeyRights ).TypeHandle, new System_Security_AccessControl_CryptoKeyRightsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.CryptoKeyRights? ).TypeHandle, new NullableSystem_Security_AccessControl_CryptoKeyRightsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.EventWaitHandleRights ).TypeHandle, new System_Security_AccessControl_EventWaitHandleRightsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.EventWaitHandleRights? ).TypeHandle, new NullableSystem_Security_AccessControl_EventWaitHandleRightsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.FileSystemRights ).TypeHandle, new System_Security_AccessControl_FileSystemRightsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.FileSystemRights? ).TypeHandle, new NullableSystem_Security_AccessControl_FileSystemRightsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.InheritanceFlags ).TypeHandle, new System_Security_AccessControl_InheritanceFlagsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.InheritanceFlags? ).TypeHandle, new NullableSystem_Security_AccessControl_InheritanceFlagsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.MutexRights ).TypeHandle, new System_Security_AccessControl_MutexRightsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.MutexRights? ).TypeHandle, new NullableSystem_Security_AccessControl_MutexRightsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.ObjectAceFlags ).TypeHandle, new System_Security_AccessControl_ObjectAceFlagsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.ObjectAceFlags? ).TypeHandle, new NullableSystem_Security_AccessControl_ObjectAceFlagsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.PropagationFlags ).TypeHandle, new System_Security_AccessControl_PropagationFlagsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.PropagationFlags? ).TypeHandle, new NullableSystem_Security_AccessControl_PropagationFlagsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.RegistryRights ).TypeHandle, new System_Security_AccessControl_RegistryRightsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.RegistryRights? ).TypeHandle, new NullableSystem_Security_AccessControl_RegistryRightsEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.ResourceType ).TypeHandle, new System_Security_AccessControl_ResourceTypeEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.ResourceType? ).TypeHandle, new NullableSystem_Security_AccessControl_ResourceTypeEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.SecurityInfos ).TypeHandle, new System_Security_AccessControl_SecurityInfosEqualityComparer() );
			result.Add( typeof( System.Security.AccessControl.SecurityInfos? ).TypeHandle, new NullableSystem_Security_AccessControl_SecurityInfosEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.CipherMode ).TypeHandle, new System_Security_Cryptography_CipherModeEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.CipherMode? ).TypeHandle, new NullableSystem_Security_Cryptography_CipherModeEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.CryptoStreamMode ).TypeHandle, new System_Security_Cryptography_CryptoStreamModeEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.CryptoStreamMode? ).TypeHandle, new NullableSystem_Security_Cryptography_CryptoStreamModeEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.CspProviderFlags ).TypeHandle, new System_Security_Cryptography_CspProviderFlagsEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.CspProviderFlags? ).TypeHandle, new NullableSystem_Security_Cryptography_CspProviderFlagsEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.DSAParameters ).TypeHandle, new System_Security_Cryptography_DSAParametersEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.DSAParameters? ).TypeHandle, new NullableSystem_Security_Cryptography_DSAParametersEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.FromBase64TransformMode ).TypeHandle, new System_Security_Cryptography_FromBase64TransformModeEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.FromBase64TransformMode? ).TypeHandle, new NullableSystem_Security_Cryptography_FromBase64TransformModeEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.KeyNumber ).TypeHandle, new System_Security_Cryptography_KeyNumberEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.KeyNumber? ).TypeHandle, new NullableSystem_Security_Cryptography_KeyNumberEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.PaddingMode ).TypeHandle, new System_Security_Cryptography_PaddingModeEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.PaddingMode? ).TypeHandle, new NullableSystem_Security_Cryptography_PaddingModeEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.RSAParameters ).TypeHandle, new System_Security_Cryptography_RSAParametersEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.RSAParameters? ).TypeHandle, new NullableSystem_Security_Cryptography_RSAParametersEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.X509Certificates.X509ContentType ).TypeHandle, new System_Security_Cryptography_X509Certificates_X509ContentTypeEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.X509Certificates.X509ContentType? ).TypeHandle, new NullableSystem_Security_Cryptography_X509Certificates_X509ContentTypeEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.X509Certificates.X509KeyStorageFlags ).TypeHandle, new System_Security_Cryptography_X509Certificates_X509KeyStorageFlagsEqualityComparer() );
			result.Add( typeof( System.Security.Cryptography.X509Certificates.X509KeyStorageFlags? ).TypeHandle, new NullableSystem_Security_Cryptography_X509Certificates_X509KeyStorageFlagsEqualityComparer() );
			result.Add( typeof( System.Security.Policy.ApplicationVersionMatch ).TypeHandle, new System_Security_Policy_ApplicationVersionMatchEqualityComparer() );
			result.Add( typeof( System.Security.Policy.ApplicationVersionMatch? ).TypeHandle, new NullableSystem_Security_Policy_ApplicationVersionMatchEqualityComparer() );
			result.Add( typeof( System.Security.Policy.PolicyStatementAttribute ).TypeHandle, new System_Security_Policy_PolicyStatementAttributeEqualityComparer() );
			result.Add( typeof( System.Security.Policy.PolicyStatementAttribute? ).TypeHandle, new NullableSystem_Security_Policy_PolicyStatementAttributeEqualityComparer() );
			result.Add( typeof( System.Security.Policy.TrustManagerUIContext ).TypeHandle, new System_Security_Policy_TrustManagerUIContextEqualityComparer() );
			result.Add( typeof( System.Security.Policy.TrustManagerUIContext? ).TypeHandle, new NullableSystem_Security_Policy_TrustManagerUIContextEqualityComparer() );
			result.Add( typeof( System.Security.Principal.PrincipalPolicy ).TypeHandle, new System_Security_Principal_PrincipalPolicyEqualityComparer() );
			result.Add( typeof( System.Security.Principal.PrincipalPolicy? ).TypeHandle, new NullableSystem_Security_Principal_PrincipalPolicyEqualityComparer() );
			result.Add( typeof( System.Security.Principal.TokenAccessLevels ).TypeHandle, new System_Security_Principal_TokenAccessLevelsEqualityComparer() );
			result.Add( typeof( System.Security.Principal.TokenAccessLevels? ).TypeHandle, new NullableSystem_Security_Principal_TokenAccessLevelsEqualityComparer() );
			result.Add( typeof( System.Security.Principal.TokenImpersonationLevel ).TypeHandle, new System_Security_Principal_TokenImpersonationLevelEqualityComparer() );
			result.Add( typeof( System.Security.Principal.TokenImpersonationLevel? ).TypeHandle, new NullableSystem_Security_Principal_TokenImpersonationLevelEqualityComparer() );
			result.Add( typeof( System.Security.Principal.WellKnownSidType ).TypeHandle, new System_Security_Principal_WellKnownSidTypeEqualityComparer() );
			result.Add( typeof( System.Security.Principal.WellKnownSidType? ).TypeHandle, new NullableSystem_Security_Principal_WellKnownSidTypeEqualityComparer() );
			result.Add( typeof( System.Security.Principal.WindowsAccountType ).TypeHandle, new System_Security_Principal_WindowsAccountTypeEqualityComparer() );
			result.Add( typeof( System.Security.Principal.WindowsAccountType? ).TypeHandle, new NullableSystem_Security_Principal_WindowsAccountTypeEqualityComparer() );
			result.Add( typeof( System.Security.Principal.WindowsBuiltInRole ).TypeHandle, new System_Security_Principal_WindowsBuiltInRoleEqualityComparer() );
			result.Add( typeof( System.Security.Principal.WindowsBuiltInRole? ).TypeHandle, new NullableSystem_Security_Principal_WindowsBuiltInRoleEqualityComparer() );
			result.Add( typeof( System.Text.NormalizationForm ).TypeHandle, new System_Text_NormalizationFormEqualityComparer() );
			result.Add( typeof( System.Text.NormalizationForm? ).TypeHandle, new NullableSystem_Text_NormalizationFormEqualityComparer() );
			result.Add( typeof( System.Threading.ApartmentState ).TypeHandle, new System_Threading_ApartmentStateEqualityComparer() );
			result.Add( typeof( System.Threading.ApartmentState? ).TypeHandle, new NullableSystem_Threading_ApartmentStateEqualityComparer() );
			result.Add( typeof( System.Threading.AsyncFlowControl ).TypeHandle, new System_Threading_AsyncFlowControlEqualityComparer() );
			result.Add( typeof( System.Threading.AsyncFlowControl? ).TypeHandle, new NullableSystem_Threading_AsyncFlowControlEqualityComparer() );
			result.Add( typeof( System.Threading.EventResetMode ).TypeHandle, new System_Threading_EventResetModeEqualityComparer() );
			result.Add( typeof( System.Threading.EventResetMode? ).TypeHandle, new NullableSystem_Threading_EventResetModeEqualityComparer() );
			result.Add( typeof( System.Threading.LockCookie ).TypeHandle, new System_Threading_LockCookieEqualityComparer() );
			result.Add( typeof( System.Threading.LockCookie? ).TypeHandle, new NullableSystem_Threading_LockCookieEqualityComparer() );
#if MSGPACK_UNITY_FULL
			result.Add( typeof( System.Threading.LockRecursionPolicy ).TypeHandle, new System_Threading_LockRecursionPolicyEqualityComparer() );
			result.Add( typeof( System.Threading.LockRecursionPolicy? ).TypeHandle, new NullableSystem_Threading_LockRecursionPolicyEqualityComparer() );
#endif // MSGPACK_UNITY_FULL
			result.Add( typeof( System.Threading.NativeOverlapped ).TypeHandle, new System_Threading_NativeOverlappedEqualityComparer() );
			result.Add( typeof( System.Threading.NativeOverlapped? ).TypeHandle, new NullableSystem_Threading_NativeOverlappedEqualityComparer() );
			result.Add( typeof( System.Threading.ThreadPriority ).TypeHandle, new System_Threading_ThreadPriorityEqualityComparer() );
			result.Add( typeof( System.Threading.ThreadPriority? ).TypeHandle, new NullableSystem_Threading_ThreadPriorityEqualityComparer() );
			result.Add( typeof( System.Threading.ThreadState ).TypeHandle, new System_Threading_ThreadStateEqualityComparer() );
			result.Add( typeof( System.Threading.ThreadState? ).TypeHandle, new NullableSystem_Threading_ThreadStateEqualityComparer() );
			result.Add( typeof( System.Object ).TypeHandle, new BoxingGenericEqualityComparer<System.Object>() );
#if MSGPACK_UNITY_FULL
			result.Add( typeof( System.Uri ).TypeHandle, new BoxingGenericEqualityComparer<System.Uri>() );
#endif // MSGPACK_UNITY_FULL
			result.Add( typeof( System.Version ).TypeHandle, new BoxingGenericEqualityComparer<System.Version>() );
			result.Add( typeof( System.String ).TypeHandle, System.StringComparer.Ordinal );
			result.Add( typeof( MsgPack.MessagePackObject ).TypeHandle, MsgPack.MessagePackObjectEqualityComparer.Instance );
			return result;
		}


		private sealed class System_AppDomainManagerInitializationOptionsEqualityComparer : IEqualityComparer<System.AppDomainManagerInitializationOptions>
		{
			public System_AppDomainManagerInitializationOptionsEqualityComparer() {}

			public bool Equals( System.AppDomainManagerInitializationOptions left, System.AppDomainManagerInitializationOptions right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.AppDomainManagerInitializationOptions obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_AppDomainManagerInitializationOptionsEqualityComparer : IEqualityComparer<System.AppDomainManagerInitializationOptions?>
		{
			public NullableSystem_AppDomainManagerInitializationOptionsEqualityComparer() {}

			public bool Equals( System.AppDomainManagerInitializationOptions? left, System.AppDomainManagerInitializationOptions? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.AppDomainManagerInitializationOptions? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_AttributeTargetsEqualityComparer : IEqualityComparer<System.AttributeTargets>
		{
			public System_AttributeTargetsEqualityComparer() {}

			public bool Equals( System.AttributeTargets left, System.AttributeTargets right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.AttributeTargets obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_AttributeTargetsEqualityComparer : IEqualityComparer<System.AttributeTargets?>
		{
			public NullableSystem_AttributeTargetsEqualityComparer() {}

			public bool Equals( System.AttributeTargets? left, System.AttributeTargets? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.AttributeTargets? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Base64FormattingOptionsEqualityComparer : IEqualityComparer<System.Base64FormattingOptions>
		{
			public System_Base64FormattingOptionsEqualityComparer() {}

			public bool Equals( System.Base64FormattingOptions left, System.Base64FormattingOptions right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Base64FormattingOptions obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Base64FormattingOptionsEqualityComparer : IEqualityComparer<System.Base64FormattingOptions?>
		{
			public NullableSystem_Base64FormattingOptionsEqualityComparer() {}

			public bool Equals( System.Base64FormattingOptions? left, System.Base64FormattingOptions? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Base64FormattingOptions? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_BooleanEqualityComparer : IEqualityComparer<System.Boolean>
		{
			public System_BooleanEqualityComparer() {}

			public bool Equals( System.Boolean left, System.Boolean right )
			{
				return left == right;
			}

			public int GetHashCode( System.Boolean obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_BooleanEqualityComparer : IEqualityComparer<System.Boolean?>
		{
			public NullableSystem_BooleanEqualityComparer() {}

			public bool Equals( System.Boolean? left, System.Boolean? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value == right.Value;
			}

			public int GetHashCode( System.Boolean? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_ByteEqualityComparer : IEqualityComparer<System.Byte>
		{
			public System_ByteEqualityComparer() {}

			public bool Equals( System.Byte left, System.Byte right )
			{
				return left == right;
			}

			public int GetHashCode( System.Byte obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_ByteEqualityComparer : IEqualityComparer<System.Byte?>
		{
			public NullableSystem_ByteEqualityComparer() {}

			public bool Equals( System.Byte? left, System.Byte? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value == right.Value;
			}

			public int GetHashCode( System.Byte? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_CharEqualityComparer : IEqualityComparer<System.Char>
		{
			public System_CharEqualityComparer() {}

			public bool Equals( System.Char left, System.Char right )
			{
				return left == right;
			}

			public int GetHashCode( System.Char obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_CharEqualityComparer : IEqualityComparer<System.Char?>
		{
			public NullableSystem_CharEqualityComparer() {}

			public bool Equals( System.Char? left, System.Char? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value == right.Value;
			}

			public int GetHashCode( System.Char? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_ConsoleColorEqualityComparer : IEqualityComparer<System.ConsoleColor>
		{
			public System_ConsoleColorEqualityComparer() {}

			public bool Equals( System.ConsoleColor left, System.ConsoleColor right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.ConsoleColor obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_ConsoleColorEqualityComparer : IEqualityComparer<System.ConsoleColor?>
		{
			public NullableSystem_ConsoleColorEqualityComparer() {}

			public bool Equals( System.ConsoleColor? left, System.ConsoleColor? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.ConsoleColor? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_ConsoleKeyEqualityComparer : IEqualityComparer<System.ConsoleKey>
		{
			public System_ConsoleKeyEqualityComparer() {}

			public bool Equals( System.ConsoleKey left, System.ConsoleKey right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.ConsoleKey obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_ConsoleKeyEqualityComparer : IEqualityComparer<System.ConsoleKey?>
		{
			public NullableSystem_ConsoleKeyEqualityComparer() {}

			public bool Equals( System.ConsoleKey? left, System.ConsoleKey? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.ConsoleKey? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_ConsoleKeyInfoEqualityComparer : IEqualityComparer<System.ConsoleKeyInfo>
		{
			public System_ConsoleKeyInfoEqualityComparer() {}

			public bool Equals( System.ConsoleKeyInfo left, System.ConsoleKeyInfo right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.ConsoleKeyInfo obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_ConsoleKeyInfoEqualityComparer : IEqualityComparer<System.ConsoleKeyInfo?>
		{
			public NullableSystem_ConsoleKeyInfoEqualityComparer() {}

			public bool Equals( System.ConsoleKeyInfo? left, System.ConsoleKeyInfo? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.ConsoleKeyInfo? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_ConsoleModifiersEqualityComparer : IEqualityComparer<System.ConsoleModifiers>
		{
			public System_ConsoleModifiersEqualityComparer() {}

			public bool Equals( System.ConsoleModifiers left, System.ConsoleModifiers right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.ConsoleModifiers obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_ConsoleModifiersEqualityComparer : IEqualityComparer<System.ConsoleModifiers?>
		{
			public NullableSystem_ConsoleModifiersEqualityComparer() {}

			public bool Equals( System.ConsoleModifiers? left, System.ConsoleModifiers? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.ConsoleModifiers? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_ConsoleSpecialKeyEqualityComparer : IEqualityComparer<System.ConsoleSpecialKey>
		{
			public System_ConsoleSpecialKeyEqualityComparer() {}

			public bool Equals( System.ConsoleSpecialKey left, System.ConsoleSpecialKey right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.ConsoleSpecialKey obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_ConsoleSpecialKeyEqualityComparer : IEqualityComparer<System.ConsoleSpecialKey?>
		{
			public NullableSystem_ConsoleSpecialKeyEqualityComparer() {}

			public bool Equals( System.ConsoleSpecialKey? left, System.ConsoleSpecialKey? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.ConsoleSpecialKey? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_DateTimeEqualityComparer : IEqualityComparer<System.DateTime>
		{
			public System_DateTimeEqualityComparer() {}

			public bool Equals( System.DateTime left, System.DateTime right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.DateTime obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_DateTimeEqualityComparer : IEqualityComparer<System.DateTime?>
		{
			public NullableSystem_DateTimeEqualityComparer() {}

			public bool Equals( System.DateTime? left, System.DateTime? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.DateTime? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_DateTimeKindEqualityComparer : IEqualityComparer<System.DateTimeKind>
		{
			public System_DateTimeKindEqualityComparer() {}

			public bool Equals( System.DateTimeKind left, System.DateTimeKind right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.DateTimeKind obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_DateTimeKindEqualityComparer : IEqualityComparer<System.DateTimeKind?>
		{
			public NullableSystem_DateTimeKindEqualityComparer() {}

			public bool Equals( System.DateTimeKind? left, System.DateTimeKind? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.DateTimeKind? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_DateTimeOffsetEqualityComparer : IEqualityComparer<System.DateTimeOffset>
		{
			public System_DateTimeOffsetEqualityComparer() {}

			public bool Equals( System.DateTimeOffset left, System.DateTimeOffset right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.DateTimeOffset obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_DateTimeOffsetEqualityComparer : IEqualityComparer<System.DateTimeOffset?>
		{
			public NullableSystem_DateTimeOffsetEqualityComparer() {}

			public bool Equals( System.DateTimeOffset? left, System.DateTimeOffset? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.DateTimeOffset? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_DayOfWeekEqualityComparer : IEqualityComparer<System.DayOfWeek>
		{
			public System_DayOfWeekEqualityComparer() {}

			public bool Equals( System.DayOfWeek left, System.DayOfWeek right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.DayOfWeek obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_DayOfWeekEqualityComparer : IEqualityComparer<System.DayOfWeek?>
		{
			public NullableSystem_DayOfWeekEqualityComparer() {}

			public bool Equals( System.DayOfWeek? left, System.DayOfWeek? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.DayOfWeek? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_DecimalEqualityComparer : IEqualityComparer<System.Decimal>
		{
			public System_DecimalEqualityComparer() {}

			public bool Equals( System.Decimal left, System.Decimal right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Decimal obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_DecimalEqualityComparer : IEqualityComparer<System.Decimal?>
		{
			public NullableSystem_DecimalEqualityComparer() {}

			public bool Equals( System.Decimal? left, System.Decimal? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Decimal? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_DoubleEqualityComparer : IEqualityComparer<System.Double>
		{
			public System_DoubleEqualityComparer() {}

			public bool Equals( System.Double left, System.Double right )
			{
				return left == right;
			}

			public int GetHashCode( System.Double obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_DoubleEqualityComparer : IEqualityComparer<System.Double?>
		{
			public NullableSystem_DoubleEqualityComparer() {}

			public bool Equals( System.Double? left, System.Double? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value == right.Value;
			}

			public int GetHashCode( System.Double? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_EnvironmentVariableTargetEqualityComparer : IEqualityComparer<System.EnvironmentVariableTarget>
		{
			public System_EnvironmentVariableTargetEqualityComparer() {}

			public bool Equals( System.EnvironmentVariableTarget left, System.EnvironmentVariableTarget right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.EnvironmentVariableTarget obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_EnvironmentVariableTargetEqualityComparer : IEqualityComparer<System.EnvironmentVariableTarget?>
		{
			public NullableSystem_EnvironmentVariableTargetEqualityComparer() {}

			public bool Equals( System.EnvironmentVariableTarget? left, System.EnvironmentVariableTarget? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.EnvironmentVariableTarget? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_GCCollectionModeEqualityComparer : IEqualityComparer<System.GCCollectionMode>
		{
			public System_GCCollectionModeEqualityComparer() {}

			public bool Equals( System.GCCollectionMode left, System.GCCollectionMode right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.GCCollectionMode obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_GCCollectionModeEqualityComparer : IEqualityComparer<System.GCCollectionMode?>
		{
			public NullableSystem_GCCollectionModeEqualityComparer() {}

			public bool Equals( System.GCCollectionMode? left, System.GCCollectionMode? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.GCCollectionMode? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_GCNotificationStatusEqualityComparer : IEqualityComparer<System.GCNotificationStatus>
		{
			public System_GCNotificationStatusEqualityComparer() {}

			public bool Equals( System.GCNotificationStatus left, System.GCNotificationStatus right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.GCNotificationStatus obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_GCNotificationStatusEqualityComparer : IEqualityComparer<System.GCNotificationStatus?>
		{
			public NullableSystem_GCNotificationStatusEqualityComparer() {}

			public bool Equals( System.GCNotificationStatus? left, System.GCNotificationStatus? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.GCNotificationStatus? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_GuidEqualityComparer : IEqualityComparer<System.Guid>
		{
			public System_GuidEqualityComparer() {}

			public bool Equals( System.Guid left, System.Guid right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Guid obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_GuidEqualityComparer : IEqualityComparer<System.Guid?>
		{
			public NullableSystem_GuidEqualityComparer() {}

			public bool Equals( System.Guid? left, System.Guid? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Guid? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Int16EqualityComparer : IEqualityComparer<System.Int16>
		{
			public System_Int16EqualityComparer() {}

			public bool Equals( System.Int16 left, System.Int16 right )
			{
				return left == right;
			}

			public int GetHashCode( System.Int16 obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Int16EqualityComparer : IEqualityComparer<System.Int16?>
		{
			public NullableSystem_Int16EqualityComparer() {}

			public bool Equals( System.Int16? left, System.Int16? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value == right.Value;
			}

			public int GetHashCode( System.Int16? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Int32EqualityComparer : IEqualityComparer<System.Int32>
		{
			public System_Int32EqualityComparer() {}

			public bool Equals( System.Int32 left, System.Int32 right )
			{
				return left == right;
			}

			public int GetHashCode( System.Int32 obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Int32EqualityComparer : IEqualityComparer<System.Int32?>
		{
			public NullableSystem_Int32EqualityComparer() {}

			public bool Equals( System.Int32? left, System.Int32? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value == right.Value;
			}

			public int GetHashCode( System.Int32? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Int64EqualityComparer : IEqualityComparer<System.Int64>
		{
			public System_Int64EqualityComparer() {}

			public bool Equals( System.Int64 left, System.Int64 right )
			{
				return left == right;
			}

			public int GetHashCode( System.Int64 obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Int64EqualityComparer : IEqualityComparer<System.Int64?>
		{
			public NullableSystem_Int64EqualityComparer() {}

			public bool Equals( System.Int64? left, System.Int64? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value == right.Value;
			}

			public int GetHashCode( System.Int64? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_IntPtrEqualityComparer : IEqualityComparer<System.IntPtr>
		{
			public System_IntPtrEqualityComparer() {}

			public bool Equals( System.IntPtr left, System.IntPtr right )
			{
				return left == right;
			}

			public int GetHashCode( System.IntPtr obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_IntPtrEqualityComparer : IEqualityComparer<System.IntPtr?>
		{
			public NullableSystem_IntPtrEqualityComparer() {}

			public bool Equals( System.IntPtr? left, System.IntPtr? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value == right.Value;
			}

			public int GetHashCode( System.IntPtr? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_LoaderOptimizationEqualityComparer : IEqualityComparer<System.LoaderOptimization>
		{
			public System_LoaderOptimizationEqualityComparer() {}

			public bool Equals( System.LoaderOptimization left, System.LoaderOptimization right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.LoaderOptimization obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_LoaderOptimizationEqualityComparer : IEqualityComparer<System.LoaderOptimization?>
		{
			public NullableSystem_LoaderOptimizationEqualityComparer() {}

			public bool Equals( System.LoaderOptimization? left, System.LoaderOptimization? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.LoaderOptimization? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_MidpointRoundingEqualityComparer : IEqualityComparer<System.MidpointRounding>
		{
			public System_MidpointRoundingEqualityComparer() {}

			public bool Equals( System.MidpointRounding left, System.MidpointRounding right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.MidpointRounding obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_MidpointRoundingEqualityComparer : IEqualityComparer<System.MidpointRounding?>
		{
			public NullableSystem_MidpointRoundingEqualityComparer() {}

			public bool Equals( System.MidpointRounding? left, System.MidpointRounding? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.MidpointRounding? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_ModuleHandleEqualityComparer : IEqualityComparer<System.ModuleHandle>
		{
			public System_ModuleHandleEqualityComparer() {}

			public bool Equals( System.ModuleHandle left, System.ModuleHandle right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.ModuleHandle obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_ModuleHandleEqualityComparer : IEqualityComparer<System.ModuleHandle?>
		{
			public NullableSystem_ModuleHandleEqualityComparer() {}

			public bool Equals( System.ModuleHandle? left, System.ModuleHandle? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.ModuleHandle? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_PlatformIDEqualityComparer : IEqualityComparer<System.PlatformID>
		{
			public System_PlatformIDEqualityComparer() {}

			public bool Equals( System.PlatformID left, System.PlatformID right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.PlatformID obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_PlatformIDEqualityComparer : IEqualityComparer<System.PlatformID?>
		{
			public NullableSystem_PlatformIDEqualityComparer() {}

			public bool Equals( System.PlatformID? left, System.PlatformID? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.PlatformID? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_RuntimeFieldHandleEqualityComparer : IEqualityComparer<System.RuntimeFieldHandle>
		{
			public System_RuntimeFieldHandleEqualityComparer() {}

			public bool Equals( System.RuntimeFieldHandle left, System.RuntimeFieldHandle right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.RuntimeFieldHandle obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_RuntimeFieldHandleEqualityComparer : IEqualityComparer<System.RuntimeFieldHandle?>
		{
			public NullableSystem_RuntimeFieldHandleEqualityComparer() {}

			public bool Equals( System.RuntimeFieldHandle? left, System.RuntimeFieldHandle? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.RuntimeFieldHandle? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_RuntimeMethodHandleEqualityComparer : IEqualityComparer<System.RuntimeMethodHandle>
		{
			public System_RuntimeMethodHandleEqualityComparer() {}

			public bool Equals( System.RuntimeMethodHandle left, System.RuntimeMethodHandle right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.RuntimeMethodHandle obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_RuntimeMethodHandleEqualityComparer : IEqualityComparer<System.RuntimeMethodHandle?>
		{
			public NullableSystem_RuntimeMethodHandleEqualityComparer() {}

			public bool Equals( System.RuntimeMethodHandle? left, System.RuntimeMethodHandle? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.RuntimeMethodHandle? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_RuntimeTypeHandleEqualityComparer : IEqualityComparer<System.RuntimeTypeHandle>
		{
			public System_RuntimeTypeHandleEqualityComparer() {}

			public bool Equals( System.RuntimeTypeHandle left, System.RuntimeTypeHandle right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.RuntimeTypeHandle obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_RuntimeTypeHandleEqualityComparer : IEqualityComparer<System.RuntimeTypeHandle?>
		{
			public NullableSystem_RuntimeTypeHandleEqualityComparer() {}

			public bool Equals( System.RuntimeTypeHandle? left, System.RuntimeTypeHandle? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.RuntimeTypeHandle? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_SByteEqualityComparer : IEqualityComparer<System.SByte>
		{
			public System_SByteEqualityComparer() {}

			public bool Equals( System.SByte left, System.SByte right )
			{
				return left == right;
			}

			public int GetHashCode( System.SByte obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_SByteEqualityComparer : IEqualityComparer<System.SByte?>
		{
			public NullableSystem_SByteEqualityComparer() {}

			public bool Equals( System.SByte? left, System.SByte? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value == right.Value;
			}

			public int GetHashCode( System.SByte? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_SingleEqualityComparer : IEqualityComparer<System.Single>
		{
			public System_SingleEqualityComparer() {}

			public bool Equals( System.Single left, System.Single right )
			{
				return left == right;
			}

			public int GetHashCode( System.Single obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_SingleEqualityComparer : IEqualityComparer<System.Single?>
		{
			public NullableSystem_SingleEqualityComparer() {}

			public bool Equals( System.Single? left, System.Single? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value == right.Value;
			}

			public int GetHashCode( System.Single? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_StringComparisonEqualityComparer : IEqualityComparer<System.StringComparison>
		{
			public System_StringComparisonEqualityComparer() {}

			public bool Equals( System.StringComparison left, System.StringComparison right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.StringComparison obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_StringComparisonEqualityComparer : IEqualityComparer<System.StringComparison?>
		{
			public NullableSystem_StringComparisonEqualityComparer() {}

			public bool Equals( System.StringComparison? left, System.StringComparison? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.StringComparison? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_StringSplitOptionsEqualityComparer : IEqualityComparer<System.StringSplitOptions>
		{
			public System_StringSplitOptionsEqualityComparer() {}

			public bool Equals( System.StringSplitOptions left, System.StringSplitOptions right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.StringSplitOptions obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_StringSplitOptionsEqualityComparer : IEqualityComparer<System.StringSplitOptions?>
		{
			public NullableSystem_StringSplitOptionsEqualityComparer() {}

			public bool Equals( System.StringSplitOptions? left, System.StringSplitOptions? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.StringSplitOptions? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_TimeSpanEqualityComparer : IEqualityComparer<System.TimeSpan>
		{
			public System_TimeSpanEqualityComparer() {}

			public bool Equals( System.TimeSpan left, System.TimeSpan right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.TimeSpan obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_TimeSpanEqualityComparer : IEqualityComparer<System.TimeSpan?>
		{
			public NullableSystem_TimeSpanEqualityComparer() {}

			public bool Equals( System.TimeSpan? left, System.TimeSpan? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.TimeSpan? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_TypeCodeEqualityComparer : IEqualityComparer<System.TypeCode>
		{
			public System_TypeCodeEqualityComparer() {}

			public bool Equals( System.TypeCode left, System.TypeCode right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.TypeCode obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_TypeCodeEqualityComparer : IEqualityComparer<System.TypeCode?>
		{
			public NullableSystem_TypeCodeEqualityComparer() {}

			public bool Equals( System.TypeCode? left, System.TypeCode? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.TypeCode? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_UInt16EqualityComparer : IEqualityComparer<System.UInt16>
		{
			public System_UInt16EqualityComparer() {}

			public bool Equals( System.UInt16 left, System.UInt16 right )
			{
				return left == right;
			}

			public int GetHashCode( System.UInt16 obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_UInt16EqualityComparer : IEqualityComparer<System.UInt16?>
		{
			public NullableSystem_UInt16EqualityComparer() {}

			public bool Equals( System.UInt16? left, System.UInt16? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value == right.Value;
			}

			public int GetHashCode( System.UInt16? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_UInt32EqualityComparer : IEqualityComparer<System.UInt32>
		{
			public System_UInt32EqualityComparer() {}

			public bool Equals( System.UInt32 left, System.UInt32 right )
			{
				return left == right;
			}

			public int GetHashCode( System.UInt32 obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_UInt32EqualityComparer : IEqualityComparer<System.UInt32?>
		{
			public NullableSystem_UInt32EqualityComparer() {}

			public bool Equals( System.UInt32? left, System.UInt32? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value == right.Value;
			}

			public int GetHashCode( System.UInt32? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_UInt64EqualityComparer : IEqualityComparer<System.UInt64>
		{
			public System_UInt64EqualityComparer() {}

			public bool Equals( System.UInt64 left, System.UInt64 right )
			{
				return left == right;
			}

			public int GetHashCode( System.UInt64 obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_UInt64EqualityComparer : IEqualityComparer<System.UInt64?>
		{
			public NullableSystem_UInt64EqualityComparer() {}

			public bool Equals( System.UInt64? left, System.UInt64? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value == right.Value;
			}

			public int GetHashCode( System.UInt64? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_UIntPtrEqualityComparer : IEqualityComparer<System.UIntPtr>
		{
			public System_UIntPtrEqualityComparer() {}

			public bool Equals( System.UIntPtr left, System.UIntPtr right )
			{
				return left == right;
			}

			public int GetHashCode( System.UIntPtr obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_UIntPtrEqualityComparer : IEqualityComparer<System.UIntPtr?>
		{
			public NullableSystem_UIntPtrEqualityComparer() {}

			public bool Equals( System.UIntPtr? left, System.UIntPtr? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value == right.Value;
			}

			public int GetHashCode( System.UIntPtr? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Collections_DictionaryEntryEqualityComparer : IEqualityComparer<System.Collections.DictionaryEntry>
		{
			public System_Collections_DictionaryEntryEqualityComparer() {}

			public bool Equals( System.Collections.DictionaryEntry left, System.Collections.DictionaryEntry right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Collections.DictionaryEntry obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Collections_DictionaryEntryEqualityComparer : IEqualityComparer<System.Collections.DictionaryEntry?>
		{
			public NullableSystem_Collections_DictionaryEntryEqualityComparer() {}

			public bool Equals( System.Collections.DictionaryEntry? left, System.Collections.DictionaryEntry? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Collections.DictionaryEntry? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Diagnostics_DebuggerBrowsableStateEqualityComparer : IEqualityComparer<System.Diagnostics.DebuggerBrowsableState>
		{
			public System_Diagnostics_DebuggerBrowsableStateEqualityComparer() {}

			public bool Equals( System.Diagnostics.DebuggerBrowsableState left, System.Diagnostics.DebuggerBrowsableState right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Diagnostics.DebuggerBrowsableState obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Diagnostics_DebuggerBrowsableStateEqualityComparer : IEqualityComparer<System.Diagnostics.DebuggerBrowsableState?>
		{
			public NullableSystem_Diagnostics_DebuggerBrowsableStateEqualityComparer() {}

			public bool Equals( System.Diagnostics.DebuggerBrowsableState? left, System.Diagnostics.DebuggerBrowsableState? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Diagnostics.DebuggerBrowsableState? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Diagnostics_SymbolStore_SymAddressKindEqualityComparer : IEqualityComparer<System.Diagnostics.SymbolStore.SymAddressKind>
		{
			public System_Diagnostics_SymbolStore_SymAddressKindEqualityComparer() {}

			public bool Equals( System.Diagnostics.SymbolStore.SymAddressKind left, System.Diagnostics.SymbolStore.SymAddressKind right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Diagnostics.SymbolStore.SymAddressKind obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Diagnostics_SymbolStore_SymAddressKindEqualityComparer : IEqualityComparer<System.Diagnostics.SymbolStore.SymAddressKind?>
		{
			public NullableSystem_Diagnostics_SymbolStore_SymAddressKindEqualityComparer() {}

			public bool Equals( System.Diagnostics.SymbolStore.SymAddressKind? left, System.Diagnostics.SymbolStore.SymAddressKind? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Diagnostics.SymbolStore.SymAddressKind? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Diagnostics_SymbolStore_SymbolTokenEqualityComparer : IEqualityComparer<System.Diagnostics.SymbolStore.SymbolToken>
		{
			public System_Diagnostics_SymbolStore_SymbolTokenEqualityComparer() {}

			public bool Equals( System.Diagnostics.SymbolStore.SymbolToken left, System.Diagnostics.SymbolStore.SymbolToken right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Diagnostics.SymbolStore.SymbolToken obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Diagnostics_SymbolStore_SymbolTokenEqualityComparer : IEqualityComparer<System.Diagnostics.SymbolStore.SymbolToken?>
		{
			public NullableSystem_Diagnostics_SymbolStore_SymbolTokenEqualityComparer() {}

			public bool Equals( System.Diagnostics.SymbolStore.SymbolToken? left, System.Diagnostics.SymbolStore.SymbolToken? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Diagnostics.SymbolStore.SymbolToken? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Globalization_CalendarAlgorithmTypeEqualityComparer : IEqualityComparer<System.Globalization.CalendarAlgorithmType>
		{
			public System_Globalization_CalendarAlgorithmTypeEqualityComparer() {}

			public bool Equals( System.Globalization.CalendarAlgorithmType left, System.Globalization.CalendarAlgorithmType right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Globalization.CalendarAlgorithmType obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Globalization_CalendarAlgorithmTypeEqualityComparer : IEqualityComparer<System.Globalization.CalendarAlgorithmType?>
		{
			public NullableSystem_Globalization_CalendarAlgorithmTypeEqualityComparer() {}

			public bool Equals( System.Globalization.CalendarAlgorithmType? left, System.Globalization.CalendarAlgorithmType? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Globalization.CalendarAlgorithmType? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Globalization_CalendarWeekRuleEqualityComparer : IEqualityComparer<System.Globalization.CalendarWeekRule>
		{
			public System_Globalization_CalendarWeekRuleEqualityComparer() {}

			public bool Equals( System.Globalization.CalendarWeekRule left, System.Globalization.CalendarWeekRule right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Globalization.CalendarWeekRule obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Globalization_CalendarWeekRuleEqualityComparer : IEqualityComparer<System.Globalization.CalendarWeekRule?>
		{
			public NullableSystem_Globalization_CalendarWeekRuleEqualityComparer() {}

			public bool Equals( System.Globalization.CalendarWeekRule? left, System.Globalization.CalendarWeekRule? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Globalization.CalendarWeekRule? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Globalization_CompareOptionsEqualityComparer : IEqualityComparer<System.Globalization.CompareOptions>
		{
			public System_Globalization_CompareOptionsEqualityComparer() {}

			public bool Equals( System.Globalization.CompareOptions left, System.Globalization.CompareOptions right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Globalization.CompareOptions obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Globalization_CompareOptionsEqualityComparer : IEqualityComparer<System.Globalization.CompareOptions?>
		{
			public NullableSystem_Globalization_CompareOptionsEqualityComparer() {}

			public bool Equals( System.Globalization.CompareOptions? left, System.Globalization.CompareOptions? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Globalization.CompareOptions? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Globalization_CultureTypesEqualityComparer : IEqualityComparer<System.Globalization.CultureTypes>
		{
			public System_Globalization_CultureTypesEqualityComparer() {}

			public bool Equals( System.Globalization.CultureTypes left, System.Globalization.CultureTypes right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Globalization.CultureTypes obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Globalization_CultureTypesEqualityComparer : IEqualityComparer<System.Globalization.CultureTypes?>
		{
			public NullableSystem_Globalization_CultureTypesEqualityComparer() {}

			public bool Equals( System.Globalization.CultureTypes? left, System.Globalization.CultureTypes? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Globalization.CultureTypes? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Globalization_DateTimeStylesEqualityComparer : IEqualityComparer<System.Globalization.DateTimeStyles>
		{
			public System_Globalization_DateTimeStylesEqualityComparer() {}

			public bool Equals( System.Globalization.DateTimeStyles left, System.Globalization.DateTimeStyles right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Globalization.DateTimeStyles obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Globalization_DateTimeStylesEqualityComparer : IEqualityComparer<System.Globalization.DateTimeStyles?>
		{
			public NullableSystem_Globalization_DateTimeStylesEqualityComparer() {}

			public bool Equals( System.Globalization.DateTimeStyles? left, System.Globalization.DateTimeStyles? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Globalization.DateTimeStyles? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Globalization_DigitShapesEqualityComparer : IEqualityComparer<System.Globalization.DigitShapes>
		{
			public System_Globalization_DigitShapesEqualityComparer() {}

			public bool Equals( System.Globalization.DigitShapes left, System.Globalization.DigitShapes right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Globalization.DigitShapes obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Globalization_DigitShapesEqualityComparer : IEqualityComparer<System.Globalization.DigitShapes?>
		{
			public NullableSystem_Globalization_DigitShapesEqualityComparer() {}

			public bool Equals( System.Globalization.DigitShapes? left, System.Globalization.DigitShapes? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Globalization.DigitShapes? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Globalization_GregorianCalendarTypesEqualityComparer : IEqualityComparer<System.Globalization.GregorianCalendarTypes>
		{
			public System_Globalization_GregorianCalendarTypesEqualityComparer() {}

			public bool Equals( System.Globalization.GregorianCalendarTypes left, System.Globalization.GregorianCalendarTypes right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Globalization.GregorianCalendarTypes obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Globalization_GregorianCalendarTypesEqualityComparer : IEqualityComparer<System.Globalization.GregorianCalendarTypes?>
		{
			public NullableSystem_Globalization_GregorianCalendarTypesEqualityComparer() {}

			public bool Equals( System.Globalization.GregorianCalendarTypes? left, System.Globalization.GregorianCalendarTypes? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Globalization.GregorianCalendarTypes? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Globalization_NumberStylesEqualityComparer : IEqualityComparer<System.Globalization.NumberStyles>
		{
			public System_Globalization_NumberStylesEqualityComparer() {}

			public bool Equals( System.Globalization.NumberStyles left, System.Globalization.NumberStyles right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Globalization.NumberStyles obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Globalization_NumberStylesEqualityComparer : IEqualityComparer<System.Globalization.NumberStyles?>
		{
			public NullableSystem_Globalization_NumberStylesEqualityComparer() {}

			public bool Equals( System.Globalization.NumberStyles? left, System.Globalization.NumberStyles? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Globalization.NumberStyles? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Globalization_UnicodeCategoryEqualityComparer : IEqualityComparer<System.Globalization.UnicodeCategory>
		{
			public System_Globalization_UnicodeCategoryEqualityComparer() {}

			public bool Equals( System.Globalization.UnicodeCategory left, System.Globalization.UnicodeCategory right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Globalization.UnicodeCategory obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Globalization_UnicodeCategoryEqualityComparer : IEqualityComparer<System.Globalization.UnicodeCategory?>
		{
			public NullableSystem_Globalization_UnicodeCategoryEqualityComparer() {}

			public bool Equals( System.Globalization.UnicodeCategory? left, System.Globalization.UnicodeCategory? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Globalization.UnicodeCategory? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_IO_DriveTypeEqualityComparer : IEqualityComparer<System.IO.DriveType>
		{
			public System_IO_DriveTypeEqualityComparer() {}

			public bool Equals( System.IO.DriveType left, System.IO.DriveType right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.IO.DriveType obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_IO_DriveTypeEqualityComparer : IEqualityComparer<System.IO.DriveType?>
		{
			public NullableSystem_IO_DriveTypeEqualityComparer() {}

			public bool Equals( System.IO.DriveType? left, System.IO.DriveType? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.IO.DriveType? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_IO_FileAccessEqualityComparer : IEqualityComparer<System.IO.FileAccess>
		{
			public System_IO_FileAccessEqualityComparer() {}

			public bool Equals( System.IO.FileAccess left, System.IO.FileAccess right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.IO.FileAccess obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_IO_FileAccessEqualityComparer : IEqualityComparer<System.IO.FileAccess?>
		{
			public NullableSystem_IO_FileAccessEqualityComparer() {}

			public bool Equals( System.IO.FileAccess? left, System.IO.FileAccess? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.IO.FileAccess? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_IO_FileAttributesEqualityComparer : IEqualityComparer<System.IO.FileAttributes>
		{
			public System_IO_FileAttributesEqualityComparer() {}

			public bool Equals( System.IO.FileAttributes left, System.IO.FileAttributes right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.IO.FileAttributes obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_IO_FileAttributesEqualityComparer : IEqualityComparer<System.IO.FileAttributes?>
		{
			public NullableSystem_IO_FileAttributesEqualityComparer() {}

			public bool Equals( System.IO.FileAttributes? left, System.IO.FileAttributes? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.IO.FileAttributes? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_IO_FileModeEqualityComparer : IEqualityComparer<System.IO.FileMode>
		{
			public System_IO_FileModeEqualityComparer() {}

			public bool Equals( System.IO.FileMode left, System.IO.FileMode right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.IO.FileMode obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_IO_FileModeEqualityComparer : IEqualityComparer<System.IO.FileMode?>
		{
			public NullableSystem_IO_FileModeEqualityComparer() {}

			public bool Equals( System.IO.FileMode? left, System.IO.FileMode? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.IO.FileMode? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_IO_FileOptionsEqualityComparer : IEqualityComparer<System.IO.FileOptions>
		{
			public System_IO_FileOptionsEqualityComparer() {}

			public bool Equals( System.IO.FileOptions left, System.IO.FileOptions right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.IO.FileOptions obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_IO_FileOptionsEqualityComparer : IEqualityComparer<System.IO.FileOptions?>
		{
			public NullableSystem_IO_FileOptionsEqualityComparer() {}

			public bool Equals( System.IO.FileOptions? left, System.IO.FileOptions? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.IO.FileOptions? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_IO_FileShareEqualityComparer : IEqualityComparer<System.IO.FileShare>
		{
			public System_IO_FileShareEqualityComparer() {}

			public bool Equals( System.IO.FileShare left, System.IO.FileShare right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.IO.FileShare obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_IO_FileShareEqualityComparer : IEqualityComparer<System.IO.FileShare?>
		{
			public NullableSystem_IO_FileShareEqualityComparer() {}

			public bool Equals( System.IO.FileShare? left, System.IO.FileShare? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.IO.FileShare? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}
#if MSGPACK_UNITY_FULL

		private sealed class System_IO_HandleInheritabilityEqualityComparer : IEqualityComparer<System.IO.HandleInheritability>
		{
			public System_IO_HandleInheritabilityEqualityComparer() {}

			public bool Equals( System.IO.HandleInheritability left, System.IO.HandleInheritability right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.IO.HandleInheritability obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_IO_HandleInheritabilityEqualityComparer : IEqualityComparer<System.IO.HandleInheritability?>
		{
			public NullableSystem_IO_HandleInheritabilityEqualityComparer() {}

			public bool Equals( System.IO.HandleInheritability? left, System.IO.HandleInheritability? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.IO.HandleInheritability? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}
#endif // MSGPACK_UNITY_FULL

		private sealed class System_IO_SearchOptionEqualityComparer : IEqualityComparer<System.IO.SearchOption>
		{
			public System_IO_SearchOptionEqualityComparer() {}

			public bool Equals( System.IO.SearchOption left, System.IO.SearchOption right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.IO.SearchOption obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_IO_SearchOptionEqualityComparer : IEqualityComparer<System.IO.SearchOption?>
		{
			public NullableSystem_IO_SearchOptionEqualityComparer() {}

			public bool Equals( System.IO.SearchOption? left, System.IO.SearchOption? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.IO.SearchOption? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_IO_SeekOriginEqualityComparer : IEqualityComparer<System.IO.SeekOrigin>
		{
			public System_IO_SeekOriginEqualityComparer() {}

			public bool Equals( System.IO.SeekOrigin left, System.IO.SeekOrigin right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.IO.SeekOrigin obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_IO_SeekOriginEqualityComparer : IEqualityComparer<System.IO.SeekOrigin?>
		{
			public NullableSystem_IO_SeekOriginEqualityComparer() {}

			public bool Equals( System.IO.SeekOrigin? left, System.IO.SeekOrigin? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.IO.SeekOrigin? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}
#if MSGPACK_UNITY_FULL

		private sealed class System_IO_Pipes_PipeAccessRightsEqualityComparer : IEqualityComparer<System.IO.Pipes.PipeAccessRights>
		{
			public System_IO_Pipes_PipeAccessRightsEqualityComparer() {}

			public bool Equals( System.IO.Pipes.PipeAccessRights left, System.IO.Pipes.PipeAccessRights right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.IO.Pipes.PipeAccessRights obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_IO_Pipes_PipeAccessRightsEqualityComparer : IEqualityComparer<System.IO.Pipes.PipeAccessRights?>
		{
			public NullableSystem_IO_Pipes_PipeAccessRightsEqualityComparer() {}

			public bool Equals( System.IO.Pipes.PipeAccessRights? left, System.IO.Pipes.PipeAccessRights? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.IO.Pipes.PipeAccessRights? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}
#endif // MSGPACK_UNITY_FULL
#if MSGPACK_UNITY_FULL

		private sealed class System_IO_Pipes_PipeDirectionEqualityComparer : IEqualityComparer<System.IO.Pipes.PipeDirection>
		{
			public System_IO_Pipes_PipeDirectionEqualityComparer() {}

			public bool Equals( System.IO.Pipes.PipeDirection left, System.IO.Pipes.PipeDirection right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.IO.Pipes.PipeDirection obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_IO_Pipes_PipeDirectionEqualityComparer : IEqualityComparer<System.IO.Pipes.PipeDirection?>
		{
			public NullableSystem_IO_Pipes_PipeDirectionEqualityComparer() {}

			public bool Equals( System.IO.Pipes.PipeDirection? left, System.IO.Pipes.PipeDirection? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.IO.Pipes.PipeDirection? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}
#endif // MSGPACK_UNITY_FULL
#if MSGPACK_UNITY_FULL

		private sealed class System_IO_Pipes_PipeOptionsEqualityComparer : IEqualityComparer<System.IO.Pipes.PipeOptions>
		{
			public System_IO_Pipes_PipeOptionsEqualityComparer() {}

			public bool Equals( System.IO.Pipes.PipeOptions left, System.IO.Pipes.PipeOptions right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.IO.Pipes.PipeOptions obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_IO_Pipes_PipeOptionsEqualityComparer : IEqualityComparer<System.IO.Pipes.PipeOptions?>
		{
			public NullableSystem_IO_Pipes_PipeOptionsEqualityComparer() {}

			public bool Equals( System.IO.Pipes.PipeOptions? left, System.IO.Pipes.PipeOptions? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.IO.Pipes.PipeOptions? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}
#endif // MSGPACK_UNITY_FULL
#if MSGPACK_UNITY_FULL

		private sealed class System_IO_Pipes_PipeTransmissionModeEqualityComparer : IEqualityComparer<System.IO.Pipes.PipeTransmissionMode>
		{
			public System_IO_Pipes_PipeTransmissionModeEqualityComparer() {}

			public bool Equals( System.IO.Pipes.PipeTransmissionMode left, System.IO.Pipes.PipeTransmissionMode right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.IO.Pipes.PipeTransmissionMode obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_IO_Pipes_PipeTransmissionModeEqualityComparer : IEqualityComparer<System.IO.Pipes.PipeTransmissionMode?>
		{
			public NullableSystem_IO_Pipes_PipeTransmissionModeEqualityComparer() {}

			public bool Equals( System.IO.Pipes.PipeTransmissionMode? left, System.IO.Pipes.PipeTransmissionMode? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.IO.Pipes.PipeTransmissionMode? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}
#endif // MSGPACK_UNITY_FULL

		private sealed class System_Reflection_AssemblyNameFlagsEqualityComparer : IEqualityComparer<System.Reflection.AssemblyNameFlags>
		{
			public System_Reflection_AssemblyNameFlagsEqualityComparer() {}

			public bool Equals( System.Reflection.AssemblyNameFlags left, System.Reflection.AssemblyNameFlags right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.AssemblyNameFlags obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_AssemblyNameFlagsEqualityComparer : IEqualityComparer<System.Reflection.AssemblyNameFlags?>
		{
			public NullableSystem_Reflection_AssemblyNameFlagsEqualityComparer() {}

			public bool Equals( System.Reflection.AssemblyNameFlags? left, System.Reflection.AssemblyNameFlags? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.AssemblyNameFlags? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_BindingFlagsEqualityComparer : IEqualityComparer<System.Reflection.BindingFlags>
		{
			public System_Reflection_BindingFlagsEqualityComparer() {}

			public bool Equals( System.Reflection.BindingFlags left, System.Reflection.BindingFlags right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.BindingFlags obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_BindingFlagsEqualityComparer : IEqualityComparer<System.Reflection.BindingFlags?>
		{
			public NullableSystem_Reflection_BindingFlagsEqualityComparer() {}

			public bool Equals( System.Reflection.BindingFlags? left, System.Reflection.BindingFlags? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.BindingFlags? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_CallingConventionsEqualityComparer : IEqualityComparer<System.Reflection.CallingConventions>
		{
			public System_Reflection_CallingConventionsEqualityComparer() {}

			public bool Equals( System.Reflection.CallingConventions left, System.Reflection.CallingConventions right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.CallingConventions obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_CallingConventionsEqualityComparer : IEqualityComparer<System.Reflection.CallingConventions?>
		{
			public NullableSystem_Reflection_CallingConventionsEqualityComparer() {}

			public bool Equals( System.Reflection.CallingConventions? left, System.Reflection.CallingConventions? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.CallingConventions? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_CustomAttributeNamedArgumentEqualityComparer : IEqualityComparer<System.Reflection.CustomAttributeNamedArgument>
		{
			public System_Reflection_CustomAttributeNamedArgumentEqualityComparer() {}

			public bool Equals( System.Reflection.CustomAttributeNamedArgument left, System.Reflection.CustomAttributeNamedArgument right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.CustomAttributeNamedArgument obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_CustomAttributeNamedArgumentEqualityComparer : IEqualityComparer<System.Reflection.CustomAttributeNamedArgument?>
		{
			public NullableSystem_Reflection_CustomAttributeNamedArgumentEqualityComparer() {}

			public bool Equals( System.Reflection.CustomAttributeNamedArgument? left, System.Reflection.CustomAttributeNamedArgument? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.CustomAttributeNamedArgument? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_CustomAttributeTypedArgumentEqualityComparer : IEqualityComparer<System.Reflection.CustomAttributeTypedArgument>
		{
			public System_Reflection_CustomAttributeTypedArgumentEqualityComparer() {}

			public bool Equals( System.Reflection.CustomAttributeTypedArgument left, System.Reflection.CustomAttributeTypedArgument right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.CustomAttributeTypedArgument obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_CustomAttributeTypedArgumentEqualityComparer : IEqualityComparer<System.Reflection.CustomAttributeTypedArgument?>
		{
			public NullableSystem_Reflection_CustomAttributeTypedArgumentEqualityComparer() {}

			public bool Equals( System.Reflection.CustomAttributeTypedArgument? left, System.Reflection.CustomAttributeTypedArgument? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.CustomAttributeTypedArgument? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_EventAttributesEqualityComparer : IEqualityComparer<System.Reflection.EventAttributes>
		{
			public System_Reflection_EventAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.EventAttributes left, System.Reflection.EventAttributes right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.EventAttributes obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_EventAttributesEqualityComparer : IEqualityComparer<System.Reflection.EventAttributes?>
		{
			public NullableSystem_Reflection_EventAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.EventAttributes? left, System.Reflection.EventAttributes? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.EventAttributes? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_ExceptionHandlingClauseOptionsEqualityComparer : IEqualityComparer<System.Reflection.ExceptionHandlingClauseOptions>
		{
			public System_Reflection_ExceptionHandlingClauseOptionsEqualityComparer() {}

			public bool Equals( System.Reflection.ExceptionHandlingClauseOptions left, System.Reflection.ExceptionHandlingClauseOptions right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.ExceptionHandlingClauseOptions obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_ExceptionHandlingClauseOptionsEqualityComparer : IEqualityComparer<System.Reflection.ExceptionHandlingClauseOptions?>
		{
			public NullableSystem_Reflection_ExceptionHandlingClauseOptionsEqualityComparer() {}

			public bool Equals( System.Reflection.ExceptionHandlingClauseOptions? left, System.Reflection.ExceptionHandlingClauseOptions? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.ExceptionHandlingClauseOptions? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_FieldAttributesEqualityComparer : IEqualityComparer<System.Reflection.FieldAttributes>
		{
			public System_Reflection_FieldAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.FieldAttributes left, System.Reflection.FieldAttributes right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.FieldAttributes obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_FieldAttributesEqualityComparer : IEqualityComparer<System.Reflection.FieldAttributes?>
		{
			public NullableSystem_Reflection_FieldAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.FieldAttributes? left, System.Reflection.FieldAttributes? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.FieldAttributes? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_GenericParameterAttributesEqualityComparer : IEqualityComparer<System.Reflection.GenericParameterAttributes>
		{
			public System_Reflection_GenericParameterAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.GenericParameterAttributes left, System.Reflection.GenericParameterAttributes right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.GenericParameterAttributes obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_GenericParameterAttributesEqualityComparer : IEqualityComparer<System.Reflection.GenericParameterAttributes?>
		{
			public NullableSystem_Reflection_GenericParameterAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.GenericParameterAttributes? left, System.Reflection.GenericParameterAttributes? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.GenericParameterAttributes? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_ImageFileMachineEqualityComparer : IEqualityComparer<System.Reflection.ImageFileMachine>
		{
			public System_Reflection_ImageFileMachineEqualityComparer() {}

			public bool Equals( System.Reflection.ImageFileMachine left, System.Reflection.ImageFileMachine right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.ImageFileMachine obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_ImageFileMachineEqualityComparer : IEqualityComparer<System.Reflection.ImageFileMachine?>
		{
			public NullableSystem_Reflection_ImageFileMachineEqualityComparer() {}

			public bool Equals( System.Reflection.ImageFileMachine? left, System.Reflection.ImageFileMachine? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.ImageFileMachine? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_InterfaceMappingEqualityComparer : IEqualityComparer<System.Reflection.InterfaceMapping>
		{
			public System_Reflection_InterfaceMappingEqualityComparer() {}

			public bool Equals( System.Reflection.InterfaceMapping left, System.Reflection.InterfaceMapping right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.InterfaceMapping obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_InterfaceMappingEqualityComparer : IEqualityComparer<System.Reflection.InterfaceMapping?>
		{
			public NullableSystem_Reflection_InterfaceMappingEqualityComparer() {}

			public bool Equals( System.Reflection.InterfaceMapping? left, System.Reflection.InterfaceMapping? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.InterfaceMapping? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_MemberTypesEqualityComparer : IEqualityComparer<System.Reflection.MemberTypes>
		{
			public System_Reflection_MemberTypesEqualityComparer() {}

			public bool Equals( System.Reflection.MemberTypes left, System.Reflection.MemberTypes right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.MemberTypes obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_MemberTypesEqualityComparer : IEqualityComparer<System.Reflection.MemberTypes?>
		{
			public NullableSystem_Reflection_MemberTypesEqualityComparer() {}

			public bool Equals( System.Reflection.MemberTypes? left, System.Reflection.MemberTypes? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.MemberTypes? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_MethodAttributesEqualityComparer : IEqualityComparer<System.Reflection.MethodAttributes>
		{
			public System_Reflection_MethodAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.MethodAttributes left, System.Reflection.MethodAttributes right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.MethodAttributes obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_MethodAttributesEqualityComparer : IEqualityComparer<System.Reflection.MethodAttributes?>
		{
			public NullableSystem_Reflection_MethodAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.MethodAttributes? left, System.Reflection.MethodAttributes? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.MethodAttributes? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_MethodImplAttributesEqualityComparer : IEqualityComparer<System.Reflection.MethodImplAttributes>
		{
			public System_Reflection_MethodImplAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.MethodImplAttributes left, System.Reflection.MethodImplAttributes right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.MethodImplAttributes obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_MethodImplAttributesEqualityComparer : IEqualityComparer<System.Reflection.MethodImplAttributes?>
		{
			public NullableSystem_Reflection_MethodImplAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.MethodImplAttributes? left, System.Reflection.MethodImplAttributes? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.MethodImplAttributes? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_ParameterAttributesEqualityComparer : IEqualityComparer<System.Reflection.ParameterAttributes>
		{
			public System_Reflection_ParameterAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.ParameterAttributes left, System.Reflection.ParameterAttributes right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.ParameterAttributes obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_ParameterAttributesEqualityComparer : IEqualityComparer<System.Reflection.ParameterAttributes?>
		{
			public NullableSystem_Reflection_ParameterAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.ParameterAttributes? left, System.Reflection.ParameterAttributes? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.ParameterAttributes? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_ParameterModifierEqualityComparer : IEqualityComparer<System.Reflection.ParameterModifier>
		{
			public System_Reflection_ParameterModifierEqualityComparer() {}

			public bool Equals( System.Reflection.ParameterModifier left, System.Reflection.ParameterModifier right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.ParameterModifier obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_ParameterModifierEqualityComparer : IEqualityComparer<System.Reflection.ParameterModifier?>
		{
			public NullableSystem_Reflection_ParameterModifierEqualityComparer() {}

			public bool Equals( System.Reflection.ParameterModifier? left, System.Reflection.ParameterModifier? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.ParameterModifier? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_PortableExecutableKindsEqualityComparer : IEqualityComparer<System.Reflection.PortableExecutableKinds>
		{
			public System_Reflection_PortableExecutableKindsEqualityComparer() {}

			public bool Equals( System.Reflection.PortableExecutableKinds left, System.Reflection.PortableExecutableKinds right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.PortableExecutableKinds obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_PortableExecutableKindsEqualityComparer : IEqualityComparer<System.Reflection.PortableExecutableKinds?>
		{
			public NullableSystem_Reflection_PortableExecutableKindsEqualityComparer() {}

			public bool Equals( System.Reflection.PortableExecutableKinds? left, System.Reflection.PortableExecutableKinds? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.PortableExecutableKinds? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_ProcessorArchitectureEqualityComparer : IEqualityComparer<System.Reflection.ProcessorArchitecture>
		{
			public System_Reflection_ProcessorArchitectureEqualityComparer() {}

			public bool Equals( System.Reflection.ProcessorArchitecture left, System.Reflection.ProcessorArchitecture right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.ProcessorArchitecture obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_ProcessorArchitectureEqualityComparer : IEqualityComparer<System.Reflection.ProcessorArchitecture?>
		{
			public NullableSystem_Reflection_ProcessorArchitectureEqualityComparer() {}

			public bool Equals( System.Reflection.ProcessorArchitecture? left, System.Reflection.ProcessorArchitecture? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.ProcessorArchitecture? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_PropertyAttributesEqualityComparer : IEqualityComparer<System.Reflection.PropertyAttributes>
		{
			public System_Reflection_PropertyAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.PropertyAttributes left, System.Reflection.PropertyAttributes right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.PropertyAttributes obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_PropertyAttributesEqualityComparer : IEqualityComparer<System.Reflection.PropertyAttributes?>
		{
			public NullableSystem_Reflection_PropertyAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.PropertyAttributes? left, System.Reflection.PropertyAttributes? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.PropertyAttributes? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_ResourceAttributesEqualityComparer : IEqualityComparer<System.Reflection.ResourceAttributes>
		{
			public System_Reflection_ResourceAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.ResourceAttributes left, System.Reflection.ResourceAttributes right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.ResourceAttributes obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_ResourceAttributesEqualityComparer : IEqualityComparer<System.Reflection.ResourceAttributes?>
		{
			public NullableSystem_Reflection_ResourceAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.ResourceAttributes? left, System.Reflection.ResourceAttributes? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.ResourceAttributes? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_ResourceLocationEqualityComparer : IEqualityComparer<System.Reflection.ResourceLocation>
		{
			public System_Reflection_ResourceLocationEqualityComparer() {}

			public bool Equals( System.Reflection.ResourceLocation left, System.Reflection.ResourceLocation right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.ResourceLocation obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_ResourceLocationEqualityComparer : IEqualityComparer<System.Reflection.ResourceLocation?>
		{
			public NullableSystem_Reflection_ResourceLocationEqualityComparer() {}

			public bool Equals( System.Reflection.ResourceLocation? left, System.Reflection.ResourceLocation? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.ResourceLocation? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Reflection_TypeAttributesEqualityComparer : IEqualityComparer<System.Reflection.TypeAttributes>
		{
			public System_Reflection_TypeAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.TypeAttributes left, System.Reflection.TypeAttributes right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Reflection.TypeAttributes obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Reflection_TypeAttributesEqualityComparer : IEqualityComparer<System.Reflection.TypeAttributes?>
		{
			public NullableSystem_Reflection_TypeAttributesEqualityComparer() {}

			public bool Equals( System.Reflection.TypeAttributes? left, System.Reflection.TypeAttributes? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Reflection.TypeAttributes? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Resources_UltimateResourceFallbackLocationEqualityComparer : IEqualityComparer<System.Resources.UltimateResourceFallbackLocation>
		{
			public System_Resources_UltimateResourceFallbackLocationEqualityComparer() {}

			public bool Equals( System.Resources.UltimateResourceFallbackLocation left, System.Resources.UltimateResourceFallbackLocation right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Resources.UltimateResourceFallbackLocation obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Resources_UltimateResourceFallbackLocationEqualityComparer : IEqualityComparer<System.Resources.UltimateResourceFallbackLocation?>
		{
			public NullableSystem_Resources_UltimateResourceFallbackLocationEqualityComparer() {}

			public bool Equals( System.Resources.UltimateResourceFallbackLocation? left, System.Resources.UltimateResourceFallbackLocation? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Resources.UltimateResourceFallbackLocation? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_GCLatencyModeEqualityComparer : IEqualityComparer<System.Runtime.GCLatencyMode>
		{
			public System_Runtime_GCLatencyModeEqualityComparer() {}

			public bool Equals( System.Runtime.GCLatencyMode left, System.Runtime.GCLatencyMode right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.GCLatencyMode obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_GCLatencyModeEqualityComparer : IEqualityComparer<System.Runtime.GCLatencyMode?>
		{
			public NullableSystem_Runtime_GCLatencyModeEqualityComparer() {}

			public bool Equals( System.Runtime.GCLatencyMode? left, System.Runtime.GCLatencyMode? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.GCLatencyMode? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_CompilerServices_CompilationRelaxationsEqualityComparer : IEqualityComparer<System.Runtime.CompilerServices.CompilationRelaxations>
		{
			public System_Runtime_CompilerServices_CompilationRelaxationsEqualityComparer() {}

			public bool Equals( System.Runtime.CompilerServices.CompilationRelaxations left, System.Runtime.CompilerServices.CompilationRelaxations right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.CompilerServices.CompilationRelaxations obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_CompilerServices_CompilationRelaxationsEqualityComparer : IEqualityComparer<System.Runtime.CompilerServices.CompilationRelaxations?>
		{
			public NullableSystem_Runtime_CompilerServices_CompilationRelaxationsEqualityComparer() {}

			public bool Equals( System.Runtime.CompilerServices.CompilationRelaxations? left, System.Runtime.CompilerServices.CompilationRelaxations? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.CompilerServices.CompilationRelaxations? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_CompilerServices_LoadHintEqualityComparer : IEqualityComparer<System.Runtime.CompilerServices.LoadHint>
		{
			public System_Runtime_CompilerServices_LoadHintEqualityComparer() {}

			public bool Equals( System.Runtime.CompilerServices.LoadHint left, System.Runtime.CompilerServices.LoadHint right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.CompilerServices.LoadHint obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_CompilerServices_LoadHintEqualityComparer : IEqualityComparer<System.Runtime.CompilerServices.LoadHint?>
		{
			public NullableSystem_Runtime_CompilerServices_LoadHintEqualityComparer() {}

			public bool Equals( System.Runtime.CompilerServices.LoadHint? left, System.Runtime.CompilerServices.LoadHint? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.CompilerServices.LoadHint? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_CompilerServices_MethodCodeTypeEqualityComparer : IEqualityComparer<System.Runtime.CompilerServices.MethodCodeType>
		{
			public System_Runtime_CompilerServices_MethodCodeTypeEqualityComparer() {}

			public bool Equals( System.Runtime.CompilerServices.MethodCodeType left, System.Runtime.CompilerServices.MethodCodeType right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.CompilerServices.MethodCodeType obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_CompilerServices_MethodCodeTypeEqualityComparer : IEqualityComparer<System.Runtime.CompilerServices.MethodCodeType?>
		{
			public NullableSystem_Runtime_CompilerServices_MethodCodeTypeEqualityComparer() {}

			public bool Equals( System.Runtime.CompilerServices.MethodCodeType? left, System.Runtime.CompilerServices.MethodCodeType? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.CompilerServices.MethodCodeType? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_CompilerServices_MethodImplOptionsEqualityComparer : IEqualityComparer<System.Runtime.CompilerServices.MethodImplOptions>
		{
			public System_Runtime_CompilerServices_MethodImplOptionsEqualityComparer() {}

			public bool Equals( System.Runtime.CompilerServices.MethodImplOptions left, System.Runtime.CompilerServices.MethodImplOptions right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.CompilerServices.MethodImplOptions obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_CompilerServices_MethodImplOptionsEqualityComparer : IEqualityComparer<System.Runtime.CompilerServices.MethodImplOptions?>
		{
			public NullableSystem_Runtime_CompilerServices_MethodImplOptionsEqualityComparer() {}

			public bool Equals( System.Runtime.CompilerServices.MethodImplOptions? left, System.Runtime.CompilerServices.MethodImplOptions? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.CompilerServices.MethodImplOptions? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_ConstrainedExecution_CerEqualityComparer : IEqualityComparer<System.Runtime.ConstrainedExecution.Cer>
		{
			public System_Runtime_ConstrainedExecution_CerEqualityComparer() {}

			public bool Equals( System.Runtime.ConstrainedExecution.Cer left, System.Runtime.ConstrainedExecution.Cer right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.ConstrainedExecution.Cer obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_ConstrainedExecution_CerEqualityComparer : IEqualityComparer<System.Runtime.ConstrainedExecution.Cer?>
		{
			public NullableSystem_Runtime_ConstrainedExecution_CerEqualityComparer() {}

			public bool Equals( System.Runtime.ConstrainedExecution.Cer? left, System.Runtime.ConstrainedExecution.Cer? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.ConstrainedExecution.Cer? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_ConstrainedExecution_ConsistencyEqualityComparer : IEqualityComparer<System.Runtime.ConstrainedExecution.Consistency>
		{
			public System_Runtime_ConstrainedExecution_ConsistencyEqualityComparer() {}

			public bool Equals( System.Runtime.ConstrainedExecution.Consistency left, System.Runtime.ConstrainedExecution.Consistency right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.ConstrainedExecution.Consistency obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_ConstrainedExecution_ConsistencyEqualityComparer : IEqualityComparer<System.Runtime.ConstrainedExecution.Consistency?>
		{
			public NullableSystem_Runtime_ConstrainedExecution_ConsistencyEqualityComparer() {}

			public bool Equals( System.Runtime.ConstrainedExecution.Consistency? left, System.Runtime.ConstrainedExecution.Consistency? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.ConstrainedExecution.Consistency? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_ArrayWithOffsetEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.ArrayWithOffset>
		{
			public System_Runtime_InteropServices_ArrayWithOffsetEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.ArrayWithOffset left, System.Runtime.InteropServices.ArrayWithOffset right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.ArrayWithOffset obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_ArrayWithOffsetEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.ArrayWithOffset?>
		{
			public NullableSystem_Runtime_InteropServices_ArrayWithOffsetEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.ArrayWithOffset? left, System.Runtime.InteropServices.ArrayWithOffset? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.ArrayWithOffset? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_AssemblyRegistrationFlagsEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.AssemblyRegistrationFlags>
		{
			public System_Runtime_InteropServices_AssemblyRegistrationFlagsEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.AssemblyRegistrationFlags left, System.Runtime.InteropServices.AssemblyRegistrationFlags right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.AssemblyRegistrationFlags obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_AssemblyRegistrationFlagsEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.AssemblyRegistrationFlags?>
		{
			public NullableSystem_Runtime_InteropServices_AssemblyRegistrationFlagsEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.AssemblyRegistrationFlags? left, System.Runtime.InteropServices.AssemblyRegistrationFlags? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.AssemblyRegistrationFlags? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_CallingConventionEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.CallingConvention>
		{
			public System_Runtime_InteropServices_CallingConventionEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.CallingConvention left, System.Runtime.InteropServices.CallingConvention right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.CallingConvention obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_CallingConventionEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.CallingConvention?>
		{
			public NullableSystem_Runtime_InteropServices_CallingConventionEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.CallingConvention? left, System.Runtime.InteropServices.CallingConvention? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.CallingConvention? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_CharSetEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.CharSet>
		{
			public System_Runtime_InteropServices_CharSetEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.CharSet left, System.Runtime.InteropServices.CharSet right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.CharSet obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_CharSetEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.CharSet?>
		{
			public NullableSystem_Runtime_InteropServices_CharSetEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.CharSet? left, System.Runtime.InteropServices.CharSet? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.CharSet? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_ClassInterfaceTypeEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.ClassInterfaceType>
		{
			public System_Runtime_InteropServices_ClassInterfaceTypeEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.ClassInterfaceType left, System.Runtime.InteropServices.ClassInterfaceType right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.ClassInterfaceType obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_ClassInterfaceTypeEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.ClassInterfaceType?>
		{
			public NullableSystem_Runtime_InteropServices_ClassInterfaceTypeEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.ClassInterfaceType? left, System.Runtime.InteropServices.ClassInterfaceType? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.ClassInterfaceType? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_ComInterfaceTypeEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.ComInterfaceType>
		{
			public System_Runtime_InteropServices_ComInterfaceTypeEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.ComInterfaceType left, System.Runtime.InteropServices.ComInterfaceType right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.ComInterfaceType obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_ComInterfaceTypeEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.ComInterfaceType?>
		{
			public NullableSystem_Runtime_InteropServices_ComInterfaceTypeEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.ComInterfaceType? left, System.Runtime.InteropServices.ComInterfaceType? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.ComInterfaceType? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_ComMemberTypeEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.ComMemberType>
		{
			public System_Runtime_InteropServices_ComMemberTypeEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.ComMemberType left, System.Runtime.InteropServices.ComMemberType right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.ComMemberType obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_ComMemberTypeEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.ComMemberType?>
		{
			public NullableSystem_Runtime_InteropServices_ComMemberTypeEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.ComMemberType? left, System.Runtime.InteropServices.ComMemberType? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.ComMemberType? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_ExporterEventKindEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.ExporterEventKind>
		{
			public System_Runtime_InteropServices_ExporterEventKindEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.ExporterEventKind left, System.Runtime.InteropServices.ExporterEventKind right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.ExporterEventKind obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_ExporterEventKindEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.ExporterEventKind?>
		{
			public NullableSystem_Runtime_InteropServices_ExporterEventKindEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.ExporterEventKind? left, System.Runtime.InteropServices.ExporterEventKind? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.ExporterEventKind? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_GCHandleEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.GCHandle>
		{
			public System_Runtime_InteropServices_GCHandleEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.GCHandle left, System.Runtime.InteropServices.GCHandle right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.GCHandle obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_GCHandleEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.GCHandle?>
		{
			public NullableSystem_Runtime_InteropServices_GCHandleEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.GCHandle? left, System.Runtime.InteropServices.GCHandle? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.GCHandle? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_GCHandleTypeEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.GCHandleType>
		{
			public System_Runtime_InteropServices_GCHandleTypeEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.GCHandleType left, System.Runtime.InteropServices.GCHandleType right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.GCHandleType obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_GCHandleTypeEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.GCHandleType?>
		{
			public NullableSystem_Runtime_InteropServices_GCHandleTypeEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.GCHandleType? left, System.Runtime.InteropServices.GCHandleType? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.GCHandleType? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_HandleRefEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.HandleRef>
		{
			public System_Runtime_InteropServices_HandleRefEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.HandleRef left, System.Runtime.InteropServices.HandleRef right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.HandleRef obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_HandleRefEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.HandleRef?>
		{
			public NullableSystem_Runtime_InteropServices_HandleRefEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.HandleRef? left, System.Runtime.InteropServices.HandleRef? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.HandleRef? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_ImporterEventKindEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.ImporterEventKind>
		{
			public System_Runtime_InteropServices_ImporterEventKindEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.ImporterEventKind left, System.Runtime.InteropServices.ImporterEventKind right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.ImporterEventKind obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_ImporterEventKindEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.ImporterEventKind?>
		{
			public NullableSystem_Runtime_InteropServices_ImporterEventKindEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.ImporterEventKind? left, System.Runtime.InteropServices.ImporterEventKind? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.ImporterEventKind? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_LayoutKindEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.LayoutKind>
		{
			public System_Runtime_InteropServices_LayoutKindEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.LayoutKind left, System.Runtime.InteropServices.LayoutKind right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.LayoutKind obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_LayoutKindEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.LayoutKind?>
		{
			public NullableSystem_Runtime_InteropServices_LayoutKindEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.LayoutKind? left, System.Runtime.InteropServices.LayoutKind? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.LayoutKind? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_RegistrationClassContextEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.RegistrationClassContext>
		{
			public System_Runtime_InteropServices_RegistrationClassContextEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.RegistrationClassContext left, System.Runtime.InteropServices.RegistrationClassContext right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.RegistrationClassContext obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_RegistrationClassContextEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.RegistrationClassContext?>
		{
			public NullableSystem_Runtime_InteropServices_RegistrationClassContextEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.RegistrationClassContext? left, System.Runtime.InteropServices.RegistrationClassContext? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.RegistrationClassContext? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_RegistrationConnectionTypeEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.RegistrationConnectionType>
		{
			public System_Runtime_InteropServices_RegistrationConnectionTypeEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.RegistrationConnectionType left, System.Runtime.InteropServices.RegistrationConnectionType right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.RegistrationConnectionType obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_RegistrationConnectionTypeEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.RegistrationConnectionType?>
		{
			public NullableSystem_Runtime_InteropServices_RegistrationConnectionTypeEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.RegistrationConnectionType? left, System.Runtime.InteropServices.RegistrationConnectionType? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.RegistrationConnectionType? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_TypeLibExporterFlagsEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.TypeLibExporterFlags>
		{
			public System_Runtime_InteropServices_TypeLibExporterFlagsEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.TypeLibExporterFlags left, System.Runtime.InteropServices.TypeLibExporterFlags right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.TypeLibExporterFlags obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_TypeLibExporterFlagsEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.TypeLibExporterFlags?>
		{
			public NullableSystem_Runtime_InteropServices_TypeLibExporterFlagsEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.TypeLibExporterFlags? left, System.Runtime.InteropServices.TypeLibExporterFlags? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.TypeLibExporterFlags? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_TypeLibFuncFlagsEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.TypeLibFuncFlags>
		{
			public System_Runtime_InteropServices_TypeLibFuncFlagsEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.TypeLibFuncFlags left, System.Runtime.InteropServices.TypeLibFuncFlags right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.TypeLibFuncFlags obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_TypeLibFuncFlagsEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.TypeLibFuncFlags?>
		{
			public NullableSystem_Runtime_InteropServices_TypeLibFuncFlagsEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.TypeLibFuncFlags? left, System.Runtime.InteropServices.TypeLibFuncFlags? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.TypeLibFuncFlags? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_TypeLibImporterFlagsEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.TypeLibImporterFlags>
		{
			public System_Runtime_InteropServices_TypeLibImporterFlagsEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.TypeLibImporterFlags left, System.Runtime.InteropServices.TypeLibImporterFlags right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.TypeLibImporterFlags obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_TypeLibImporterFlagsEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.TypeLibImporterFlags?>
		{
			public NullableSystem_Runtime_InteropServices_TypeLibImporterFlagsEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.TypeLibImporterFlags? left, System.Runtime.InteropServices.TypeLibImporterFlags? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.TypeLibImporterFlags? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_TypeLibTypeFlagsEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.TypeLibTypeFlags>
		{
			public System_Runtime_InteropServices_TypeLibTypeFlagsEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.TypeLibTypeFlags left, System.Runtime.InteropServices.TypeLibTypeFlags right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.TypeLibTypeFlags obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_TypeLibTypeFlagsEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.TypeLibTypeFlags?>
		{
			public NullableSystem_Runtime_InteropServices_TypeLibTypeFlagsEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.TypeLibTypeFlags? left, System.Runtime.InteropServices.TypeLibTypeFlags? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.TypeLibTypeFlags? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_TypeLibVarFlagsEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.TypeLibVarFlags>
		{
			public System_Runtime_InteropServices_TypeLibVarFlagsEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.TypeLibVarFlags left, System.Runtime.InteropServices.TypeLibVarFlags right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.TypeLibVarFlags obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_TypeLibVarFlagsEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.TypeLibVarFlags?>
		{
			public NullableSystem_Runtime_InteropServices_TypeLibVarFlagsEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.TypeLibVarFlags? left, System.Runtime.InteropServices.TypeLibVarFlags? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.TypeLibVarFlags? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_UnmanagedTypeEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.UnmanagedType>
		{
			public System_Runtime_InteropServices_UnmanagedTypeEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.UnmanagedType left, System.Runtime.InteropServices.UnmanagedType right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.UnmanagedType obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_UnmanagedTypeEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.UnmanagedType?>
		{
			public NullableSystem_Runtime_InteropServices_UnmanagedTypeEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.UnmanagedType? left, System.Runtime.InteropServices.UnmanagedType? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.UnmanagedType? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_InteropServices_VarEnumEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.VarEnum>
		{
			public System_Runtime_InteropServices_VarEnumEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.VarEnum left, System.Runtime.InteropServices.VarEnum right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.InteropServices.VarEnum obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_InteropServices_VarEnumEqualityComparer : IEqualityComparer<System.Runtime.InteropServices.VarEnum?>
		{
			public NullableSystem_Runtime_InteropServices_VarEnumEqualityComparer() {}

			public bool Equals( System.Runtime.InteropServices.VarEnum? left, System.Runtime.InteropServices.VarEnum? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.InteropServices.VarEnum? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_Remoting_CustomErrorsModesEqualityComparer : IEqualityComparer<System.Runtime.Remoting.CustomErrorsModes>
		{
			public System_Runtime_Remoting_CustomErrorsModesEqualityComparer() {}

			public bool Equals( System.Runtime.Remoting.CustomErrorsModes left, System.Runtime.Remoting.CustomErrorsModes right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.Remoting.CustomErrorsModes obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_Remoting_CustomErrorsModesEqualityComparer : IEqualityComparer<System.Runtime.Remoting.CustomErrorsModes?>
		{
			public NullableSystem_Runtime_Remoting_CustomErrorsModesEqualityComparer() {}

			public bool Equals( System.Runtime.Remoting.CustomErrorsModes? left, System.Runtime.Remoting.CustomErrorsModes? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.Remoting.CustomErrorsModes? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_Remoting_WellKnownObjectModeEqualityComparer : IEqualityComparer<System.Runtime.Remoting.WellKnownObjectMode>
		{
			public System_Runtime_Remoting_WellKnownObjectModeEqualityComparer() {}

			public bool Equals( System.Runtime.Remoting.WellKnownObjectMode left, System.Runtime.Remoting.WellKnownObjectMode right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.Remoting.WellKnownObjectMode obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_Remoting_WellKnownObjectModeEqualityComparer : IEqualityComparer<System.Runtime.Remoting.WellKnownObjectMode?>
		{
			public NullableSystem_Runtime_Remoting_WellKnownObjectModeEqualityComparer() {}

			public bool Equals( System.Runtime.Remoting.WellKnownObjectMode? left, System.Runtime.Remoting.WellKnownObjectMode? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.Remoting.WellKnownObjectMode? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_Remoting_Activation_ActivatorLevelEqualityComparer : IEqualityComparer<System.Runtime.Remoting.Activation.ActivatorLevel>
		{
			public System_Runtime_Remoting_Activation_ActivatorLevelEqualityComparer() {}

			public bool Equals( System.Runtime.Remoting.Activation.ActivatorLevel left, System.Runtime.Remoting.Activation.ActivatorLevel right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.Remoting.Activation.ActivatorLevel obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_Remoting_Activation_ActivatorLevelEqualityComparer : IEqualityComparer<System.Runtime.Remoting.Activation.ActivatorLevel?>
		{
			public NullableSystem_Runtime_Remoting_Activation_ActivatorLevelEqualityComparer() {}

			public bool Equals( System.Runtime.Remoting.Activation.ActivatorLevel? left, System.Runtime.Remoting.Activation.ActivatorLevel? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.Remoting.Activation.ActivatorLevel? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_Remoting_Channels_ServerProcessingEqualityComparer : IEqualityComparer<System.Runtime.Remoting.Channels.ServerProcessing>
		{
			public System_Runtime_Remoting_Channels_ServerProcessingEqualityComparer() {}

			public bool Equals( System.Runtime.Remoting.Channels.ServerProcessing left, System.Runtime.Remoting.Channels.ServerProcessing right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.Remoting.Channels.ServerProcessing obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_Remoting_Channels_ServerProcessingEqualityComparer : IEqualityComparer<System.Runtime.Remoting.Channels.ServerProcessing?>
		{
			public NullableSystem_Runtime_Remoting_Channels_ServerProcessingEqualityComparer() {}

			public bool Equals( System.Runtime.Remoting.Channels.ServerProcessing? left, System.Runtime.Remoting.Channels.ServerProcessing? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.Remoting.Channels.ServerProcessing? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_Remoting_Lifetime_LeaseStateEqualityComparer : IEqualityComparer<System.Runtime.Remoting.Lifetime.LeaseState>
		{
			public System_Runtime_Remoting_Lifetime_LeaseStateEqualityComparer() {}

			public bool Equals( System.Runtime.Remoting.Lifetime.LeaseState left, System.Runtime.Remoting.Lifetime.LeaseState right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.Remoting.Lifetime.LeaseState obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_Remoting_Lifetime_LeaseStateEqualityComparer : IEqualityComparer<System.Runtime.Remoting.Lifetime.LeaseState?>
		{
			public NullableSystem_Runtime_Remoting_Lifetime_LeaseStateEqualityComparer() {}

			public bool Equals( System.Runtime.Remoting.Lifetime.LeaseState? left, System.Runtime.Remoting.Lifetime.LeaseState? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.Remoting.Lifetime.LeaseState? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_Remoting_Metadata_SoapOptionEqualityComparer : IEqualityComparer<System.Runtime.Remoting.Metadata.SoapOption>
		{
			public System_Runtime_Remoting_Metadata_SoapOptionEqualityComparer() {}

			public bool Equals( System.Runtime.Remoting.Metadata.SoapOption left, System.Runtime.Remoting.Metadata.SoapOption right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.Remoting.Metadata.SoapOption obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_Remoting_Metadata_SoapOptionEqualityComparer : IEqualityComparer<System.Runtime.Remoting.Metadata.SoapOption?>
		{
			public NullableSystem_Runtime_Remoting_Metadata_SoapOptionEqualityComparer() {}

			public bool Equals( System.Runtime.Remoting.Metadata.SoapOption? left, System.Runtime.Remoting.Metadata.SoapOption? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.Remoting.Metadata.SoapOption? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_Remoting_Metadata_XmlFieldOrderOptionEqualityComparer : IEqualityComparer<System.Runtime.Remoting.Metadata.XmlFieldOrderOption>
		{
			public System_Runtime_Remoting_Metadata_XmlFieldOrderOptionEqualityComparer() {}

			public bool Equals( System.Runtime.Remoting.Metadata.XmlFieldOrderOption left, System.Runtime.Remoting.Metadata.XmlFieldOrderOption right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.Remoting.Metadata.XmlFieldOrderOption obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_Remoting_Metadata_XmlFieldOrderOptionEqualityComparer : IEqualityComparer<System.Runtime.Remoting.Metadata.XmlFieldOrderOption?>
		{
			public NullableSystem_Runtime_Remoting_Metadata_XmlFieldOrderOptionEqualityComparer() {}

			public bool Equals( System.Runtime.Remoting.Metadata.XmlFieldOrderOption? left, System.Runtime.Remoting.Metadata.XmlFieldOrderOption? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.Remoting.Metadata.XmlFieldOrderOption? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_Serialization_SerializationEntryEqualityComparer : IEqualityComparer<System.Runtime.Serialization.SerializationEntry>
		{
			public System_Runtime_Serialization_SerializationEntryEqualityComparer() {}

			public bool Equals( System.Runtime.Serialization.SerializationEntry left, System.Runtime.Serialization.SerializationEntry right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.Serialization.SerializationEntry obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_Serialization_SerializationEntryEqualityComparer : IEqualityComparer<System.Runtime.Serialization.SerializationEntry?>
		{
			public NullableSystem_Runtime_Serialization_SerializationEntryEqualityComparer() {}

			public bool Equals( System.Runtime.Serialization.SerializationEntry? left, System.Runtime.Serialization.SerializationEntry? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.Serialization.SerializationEntry? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_Serialization_StreamingContextEqualityComparer : IEqualityComparer<System.Runtime.Serialization.StreamingContext>
		{
			public System_Runtime_Serialization_StreamingContextEqualityComparer() {}

			public bool Equals( System.Runtime.Serialization.StreamingContext left, System.Runtime.Serialization.StreamingContext right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.Serialization.StreamingContext obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_Serialization_StreamingContextEqualityComparer : IEqualityComparer<System.Runtime.Serialization.StreamingContext?>
		{
			public NullableSystem_Runtime_Serialization_StreamingContextEqualityComparer() {}

			public bool Equals( System.Runtime.Serialization.StreamingContext? left, System.Runtime.Serialization.StreamingContext? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.Serialization.StreamingContext? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_Serialization_StreamingContextStatesEqualityComparer : IEqualityComparer<System.Runtime.Serialization.StreamingContextStates>
		{
			public System_Runtime_Serialization_StreamingContextStatesEqualityComparer() {}

			public bool Equals( System.Runtime.Serialization.StreamingContextStates left, System.Runtime.Serialization.StreamingContextStates right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.Serialization.StreamingContextStates obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_Serialization_StreamingContextStatesEqualityComparer : IEqualityComparer<System.Runtime.Serialization.StreamingContextStates?>
		{
			public NullableSystem_Runtime_Serialization_StreamingContextStatesEqualityComparer() {}

			public bool Equals( System.Runtime.Serialization.StreamingContextStates? left, System.Runtime.Serialization.StreamingContextStates? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.Serialization.StreamingContextStates? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_Serialization_Formatters_FormatterAssemblyStyleEqualityComparer : IEqualityComparer<System.Runtime.Serialization.Formatters.FormatterAssemblyStyle>
		{
			public System_Runtime_Serialization_Formatters_FormatterAssemblyStyleEqualityComparer() {}

			public bool Equals( System.Runtime.Serialization.Formatters.FormatterAssemblyStyle left, System.Runtime.Serialization.Formatters.FormatterAssemblyStyle right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.Serialization.Formatters.FormatterAssemblyStyle obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_Serialization_Formatters_FormatterAssemblyStyleEqualityComparer : IEqualityComparer<System.Runtime.Serialization.Formatters.FormatterAssemblyStyle?>
		{
			public NullableSystem_Runtime_Serialization_Formatters_FormatterAssemblyStyleEqualityComparer() {}

			public bool Equals( System.Runtime.Serialization.Formatters.FormatterAssemblyStyle? left, System.Runtime.Serialization.Formatters.FormatterAssemblyStyle? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.Serialization.Formatters.FormatterAssemblyStyle? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_Serialization_Formatters_FormatterTypeStyleEqualityComparer : IEqualityComparer<System.Runtime.Serialization.Formatters.FormatterTypeStyle>
		{
			public System_Runtime_Serialization_Formatters_FormatterTypeStyleEqualityComparer() {}

			public bool Equals( System.Runtime.Serialization.Formatters.FormatterTypeStyle left, System.Runtime.Serialization.Formatters.FormatterTypeStyle right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.Serialization.Formatters.FormatterTypeStyle obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_Serialization_Formatters_FormatterTypeStyleEqualityComparer : IEqualityComparer<System.Runtime.Serialization.Formatters.FormatterTypeStyle?>
		{
			public NullableSystem_Runtime_Serialization_Formatters_FormatterTypeStyleEqualityComparer() {}

			public bool Equals( System.Runtime.Serialization.Formatters.FormatterTypeStyle? left, System.Runtime.Serialization.Formatters.FormatterTypeStyle? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.Serialization.Formatters.FormatterTypeStyle? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_Serialization_Formatters_TypeFilterLevelEqualityComparer : IEqualityComparer<System.Runtime.Serialization.Formatters.TypeFilterLevel>
		{
			public System_Runtime_Serialization_Formatters_TypeFilterLevelEqualityComparer() {}

			public bool Equals( System.Runtime.Serialization.Formatters.TypeFilterLevel left, System.Runtime.Serialization.Formatters.TypeFilterLevel right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.Serialization.Formatters.TypeFilterLevel obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_Serialization_Formatters_TypeFilterLevelEqualityComparer : IEqualityComparer<System.Runtime.Serialization.Formatters.TypeFilterLevel?>
		{
			public NullableSystem_Runtime_Serialization_Formatters_TypeFilterLevelEqualityComparer() {}

			public bool Equals( System.Runtime.Serialization.Formatters.TypeFilterLevel? left, System.Runtime.Serialization.Formatters.TypeFilterLevel? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.Serialization.Formatters.TypeFilterLevel? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Runtime_Versioning_ResourceScopeEqualityComparer : IEqualityComparer<System.Runtime.Versioning.ResourceScope>
		{
			public System_Runtime_Versioning_ResourceScopeEqualityComparer() {}

			public bool Equals( System.Runtime.Versioning.ResourceScope left, System.Runtime.Versioning.ResourceScope right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Runtime.Versioning.ResourceScope obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Runtime_Versioning_ResourceScopeEqualityComparer : IEqualityComparer<System.Runtime.Versioning.ResourceScope?>
		{
			public NullableSystem_Runtime_Versioning_ResourceScopeEqualityComparer() {}

			public bool Equals( System.Runtime.Versioning.ResourceScope? left, System.Runtime.Versioning.ResourceScope? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Runtime.Versioning.ResourceScope? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_HostSecurityManagerOptionsEqualityComparer : IEqualityComparer<System.Security.HostSecurityManagerOptions>
		{
			public System_Security_HostSecurityManagerOptionsEqualityComparer() {}

			public bool Equals( System.Security.HostSecurityManagerOptions left, System.Security.HostSecurityManagerOptions right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.HostSecurityManagerOptions obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_HostSecurityManagerOptionsEqualityComparer : IEqualityComparer<System.Security.HostSecurityManagerOptions?>
		{
			public NullableSystem_Security_HostSecurityManagerOptionsEqualityComparer() {}

			public bool Equals( System.Security.HostSecurityManagerOptions? left, System.Security.HostSecurityManagerOptions? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.HostSecurityManagerOptions? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_PolicyLevelTypeEqualityComparer : IEqualityComparer<System.Security.PolicyLevelType>
		{
			public System_Security_PolicyLevelTypeEqualityComparer() {}

			public bool Equals( System.Security.PolicyLevelType left, System.Security.PolicyLevelType right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.PolicyLevelType obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_PolicyLevelTypeEqualityComparer : IEqualityComparer<System.Security.PolicyLevelType?>
		{
			public NullableSystem_Security_PolicyLevelTypeEqualityComparer() {}

			public bool Equals( System.Security.PolicyLevelType? left, System.Security.PolicyLevelType? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.PolicyLevelType? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_SecurityZoneEqualityComparer : IEqualityComparer<System.Security.SecurityZone>
		{
			public System_Security_SecurityZoneEqualityComparer() {}

			public bool Equals( System.Security.SecurityZone left, System.Security.SecurityZone right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.SecurityZone obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_SecurityZoneEqualityComparer : IEqualityComparer<System.Security.SecurityZone?>
		{
			public NullableSystem_Security_SecurityZoneEqualityComparer() {}

			public bool Equals( System.Security.SecurityZone? left, System.Security.SecurityZone? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.SecurityZone? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_AccessControlActionsEqualityComparer : IEqualityComparer<System.Security.AccessControl.AccessControlActions>
		{
			public System_Security_AccessControl_AccessControlActionsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.AccessControlActions left, System.Security.AccessControl.AccessControlActions right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.AccessControlActions obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_AccessControlActionsEqualityComparer : IEqualityComparer<System.Security.AccessControl.AccessControlActions?>
		{
			public NullableSystem_Security_AccessControl_AccessControlActionsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.AccessControlActions? left, System.Security.AccessControl.AccessControlActions? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.AccessControlActions? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_AccessControlModificationEqualityComparer : IEqualityComparer<System.Security.AccessControl.AccessControlModification>
		{
			public System_Security_AccessControl_AccessControlModificationEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.AccessControlModification left, System.Security.AccessControl.AccessControlModification right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.AccessControlModification obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_AccessControlModificationEqualityComparer : IEqualityComparer<System.Security.AccessControl.AccessControlModification?>
		{
			public NullableSystem_Security_AccessControl_AccessControlModificationEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.AccessControlModification? left, System.Security.AccessControl.AccessControlModification? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.AccessControlModification? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_AccessControlSectionsEqualityComparer : IEqualityComparer<System.Security.AccessControl.AccessControlSections>
		{
			public System_Security_AccessControl_AccessControlSectionsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.AccessControlSections left, System.Security.AccessControl.AccessControlSections right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.AccessControlSections obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_AccessControlSectionsEqualityComparer : IEqualityComparer<System.Security.AccessControl.AccessControlSections?>
		{
			public NullableSystem_Security_AccessControl_AccessControlSectionsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.AccessControlSections? left, System.Security.AccessControl.AccessControlSections? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.AccessControlSections? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_AccessControlTypeEqualityComparer : IEqualityComparer<System.Security.AccessControl.AccessControlType>
		{
			public System_Security_AccessControl_AccessControlTypeEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.AccessControlType left, System.Security.AccessControl.AccessControlType right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.AccessControlType obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_AccessControlTypeEqualityComparer : IEqualityComparer<System.Security.AccessControl.AccessControlType?>
		{
			public NullableSystem_Security_AccessControl_AccessControlTypeEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.AccessControlType? left, System.Security.AccessControl.AccessControlType? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.AccessControlType? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_AceFlagsEqualityComparer : IEqualityComparer<System.Security.AccessControl.AceFlags>
		{
			public System_Security_AccessControl_AceFlagsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.AceFlags left, System.Security.AccessControl.AceFlags right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.AceFlags obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_AceFlagsEqualityComparer : IEqualityComparer<System.Security.AccessControl.AceFlags?>
		{
			public NullableSystem_Security_AccessControl_AceFlagsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.AceFlags? left, System.Security.AccessControl.AceFlags? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.AceFlags? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_AceQualifierEqualityComparer : IEqualityComparer<System.Security.AccessControl.AceQualifier>
		{
			public System_Security_AccessControl_AceQualifierEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.AceQualifier left, System.Security.AccessControl.AceQualifier right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.AceQualifier obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_AceQualifierEqualityComparer : IEqualityComparer<System.Security.AccessControl.AceQualifier?>
		{
			public NullableSystem_Security_AccessControl_AceQualifierEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.AceQualifier? left, System.Security.AccessControl.AceQualifier? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.AceQualifier? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_AceTypeEqualityComparer : IEqualityComparer<System.Security.AccessControl.AceType>
		{
			public System_Security_AccessControl_AceTypeEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.AceType left, System.Security.AccessControl.AceType right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.AceType obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_AceTypeEqualityComparer : IEqualityComparer<System.Security.AccessControl.AceType?>
		{
			public NullableSystem_Security_AccessControl_AceTypeEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.AceType? left, System.Security.AccessControl.AceType? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.AceType? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_AuditFlagsEqualityComparer : IEqualityComparer<System.Security.AccessControl.AuditFlags>
		{
			public System_Security_AccessControl_AuditFlagsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.AuditFlags left, System.Security.AccessControl.AuditFlags right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.AuditFlags obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_AuditFlagsEqualityComparer : IEqualityComparer<System.Security.AccessControl.AuditFlags?>
		{
			public NullableSystem_Security_AccessControl_AuditFlagsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.AuditFlags? left, System.Security.AccessControl.AuditFlags? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.AuditFlags? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_CompoundAceTypeEqualityComparer : IEqualityComparer<System.Security.AccessControl.CompoundAceType>
		{
			public System_Security_AccessControl_CompoundAceTypeEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.CompoundAceType left, System.Security.AccessControl.CompoundAceType right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.CompoundAceType obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_CompoundAceTypeEqualityComparer : IEqualityComparer<System.Security.AccessControl.CompoundAceType?>
		{
			public NullableSystem_Security_AccessControl_CompoundAceTypeEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.CompoundAceType? left, System.Security.AccessControl.CompoundAceType? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.CompoundAceType? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_ControlFlagsEqualityComparer : IEqualityComparer<System.Security.AccessControl.ControlFlags>
		{
			public System_Security_AccessControl_ControlFlagsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.ControlFlags left, System.Security.AccessControl.ControlFlags right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.ControlFlags obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_ControlFlagsEqualityComparer : IEqualityComparer<System.Security.AccessControl.ControlFlags?>
		{
			public NullableSystem_Security_AccessControl_ControlFlagsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.ControlFlags? left, System.Security.AccessControl.ControlFlags? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.ControlFlags? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_CryptoKeyRightsEqualityComparer : IEqualityComparer<System.Security.AccessControl.CryptoKeyRights>
		{
			public System_Security_AccessControl_CryptoKeyRightsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.CryptoKeyRights left, System.Security.AccessControl.CryptoKeyRights right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.CryptoKeyRights obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_CryptoKeyRightsEqualityComparer : IEqualityComparer<System.Security.AccessControl.CryptoKeyRights?>
		{
			public NullableSystem_Security_AccessControl_CryptoKeyRightsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.CryptoKeyRights? left, System.Security.AccessControl.CryptoKeyRights? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.CryptoKeyRights? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_EventWaitHandleRightsEqualityComparer : IEqualityComparer<System.Security.AccessControl.EventWaitHandleRights>
		{
			public System_Security_AccessControl_EventWaitHandleRightsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.EventWaitHandleRights left, System.Security.AccessControl.EventWaitHandleRights right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.EventWaitHandleRights obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_EventWaitHandleRightsEqualityComparer : IEqualityComparer<System.Security.AccessControl.EventWaitHandleRights?>
		{
			public NullableSystem_Security_AccessControl_EventWaitHandleRightsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.EventWaitHandleRights? left, System.Security.AccessControl.EventWaitHandleRights? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.EventWaitHandleRights? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_FileSystemRightsEqualityComparer : IEqualityComparer<System.Security.AccessControl.FileSystemRights>
		{
			public System_Security_AccessControl_FileSystemRightsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.FileSystemRights left, System.Security.AccessControl.FileSystemRights right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.FileSystemRights obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_FileSystemRightsEqualityComparer : IEqualityComparer<System.Security.AccessControl.FileSystemRights?>
		{
			public NullableSystem_Security_AccessControl_FileSystemRightsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.FileSystemRights? left, System.Security.AccessControl.FileSystemRights? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.FileSystemRights? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_InheritanceFlagsEqualityComparer : IEqualityComparer<System.Security.AccessControl.InheritanceFlags>
		{
			public System_Security_AccessControl_InheritanceFlagsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.InheritanceFlags left, System.Security.AccessControl.InheritanceFlags right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.InheritanceFlags obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_InheritanceFlagsEqualityComparer : IEqualityComparer<System.Security.AccessControl.InheritanceFlags?>
		{
			public NullableSystem_Security_AccessControl_InheritanceFlagsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.InheritanceFlags? left, System.Security.AccessControl.InheritanceFlags? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.InheritanceFlags? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_MutexRightsEqualityComparer : IEqualityComparer<System.Security.AccessControl.MutexRights>
		{
			public System_Security_AccessControl_MutexRightsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.MutexRights left, System.Security.AccessControl.MutexRights right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.MutexRights obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_MutexRightsEqualityComparer : IEqualityComparer<System.Security.AccessControl.MutexRights?>
		{
			public NullableSystem_Security_AccessControl_MutexRightsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.MutexRights? left, System.Security.AccessControl.MutexRights? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.MutexRights? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_ObjectAceFlagsEqualityComparer : IEqualityComparer<System.Security.AccessControl.ObjectAceFlags>
		{
			public System_Security_AccessControl_ObjectAceFlagsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.ObjectAceFlags left, System.Security.AccessControl.ObjectAceFlags right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.ObjectAceFlags obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_ObjectAceFlagsEqualityComparer : IEqualityComparer<System.Security.AccessControl.ObjectAceFlags?>
		{
			public NullableSystem_Security_AccessControl_ObjectAceFlagsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.ObjectAceFlags? left, System.Security.AccessControl.ObjectAceFlags? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.ObjectAceFlags? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_PropagationFlagsEqualityComparer : IEqualityComparer<System.Security.AccessControl.PropagationFlags>
		{
			public System_Security_AccessControl_PropagationFlagsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.PropagationFlags left, System.Security.AccessControl.PropagationFlags right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.PropagationFlags obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_PropagationFlagsEqualityComparer : IEqualityComparer<System.Security.AccessControl.PropagationFlags?>
		{
			public NullableSystem_Security_AccessControl_PropagationFlagsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.PropagationFlags? left, System.Security.AccessControl.PropagationFlags? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.PropagationFlags? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_RegistryRightsEqualityComparer : IEqualityComparer<System.Security.AccessControl.RegistryRights>
		{
			public System_Security_AccessControl_RegistryRightsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.RegistryRights left, System.Security.AccessControl.RegistryRights right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.RegistryRights obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_RegistryRightsEqualityComparer : IEqualityComparer<System.Security.AccessControl.RegistryRights?>
		{
			public NullableSystem_Security_AccessControl_RegistryRightsEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.RegistryRights? left, System.Security.AccessControl.RegistryRights? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.RegistryRights? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_ResourceTypeEqualityComparer : IEqualityComparer<System.Security.AccessControl.ResourceType>
		{
			public System_Security_AccessControl_ResourceTypeEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.ResourceType left, System.Security.AccessControl.ResourceType right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.ResourceType obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_ResourceTypeEqualityComparer : IEqualityComparer<System.Security.AccessControl.ResourceType?>
		{
			public NullableSystem_Security_AccessControl_ResourceTypeEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.ResourceType? left, System.Security.AccessControl.ResourceType? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.ResourceType? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_AccessControl_SecurityInfosEqualityComparer : IEqualityComparer<System.Security.AccessControl.SecurityInfos>
		{
			public System_Security_AccessControl_SecurityInfosEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.SecurityInfos left, System.Security.AccessControl.SecurityInfos right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.AccessControl.SecurityInfos obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_AccessControl_SecurityInfosEqualityComparer : IEqualityComparer<System.Security.AccessControl.SecurityInfos?>
		{
			public NullableSystem_Security_AccessControl_SecurityInfosEqualityComparer() {}

			public bool Equals( System.Security.AccessControl.SecurityInfos? left, System.Security.AccessControl.SecurityInfos? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.AccessControl.SecurityInfos? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Cryptography_CipherModeEqualityComparer : IEqualityComparer<System.Security.Cryptography.CipherMode>
		{
			public System_Security_Cryptography_CipherModeEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.CipherMode left, System.Security.Cryptography.CipherMode right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Cryptography.CipherMode obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Cryptography_CipherModeEqualityComparer : IEqualityComparer<System.Security.Cryptography.CipherMode?>
		{
			public NullableSystem_Security_Cryptography_CipherModeEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.CipherMode? left, System.Security.Cryptography.CipherMode? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Cryptography.CipherMode? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Cryptography_CryptoStreamModeEqualityComparer : IEqualityComparer<System.Security.Cryptography.CryptoStreamMode>
		{
			public System_Security_Cryptography_CryptoStreamModeEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.CryptoStreamMode left, System.Security.Cryptography.CryptoStreamMode right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Cryptography.CryptoStreamMode obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Cryptography_CryptoStreamModeEqualityComparer : IEqualityComparer<System.Security.Cryptography.CryptoStreamMode?>
		{
			public NullableSystem_Security_Cryptography_CryptoStreamModeEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.CryptoStreamMode? left, System.Security.Cryptography.CryptoStreamMode? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Cryptography.CryptoStreamMode? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Cryptography_CspProviderFlagsEqualityComparer : IEqualityComparer<System.Security.Cryptography.CspProviderFlags>
		{
			public System_Security_Cryptography_CspProviderFlagsEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.CspProviderFlags left, System.Security.Cryptography.CspProviderFlags right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Cryptography.CspProviderFlags obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Cryptography_CspProviderFlagsEqualityComparer : IEqualityComparer<System.Security.Cryptography.CspProviderFlags?>
		{
			public NullableSystem_Security_Cryptography_CspProviderFlagsEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.CspProviderFlags? left, System.Security.Cryptography.CspProviderFlags? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Cryptography.CspProviderFlags? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Cryptography_DSAParametersEqualityComparer : IEqualityComparer<System.Security.Cryptography.DSAParameters>
		{
			public System_Security_Cryptography_DSAParametersEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.DSAParameters left, System.Security.Cryptography.DSAParameters right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Cryptography.DSAParameters obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Cryptography_DSAParametersEqualityComparer : IEqualityComparer<System.Security.Cryptography.DSAParameters?>
		{
			public NullableSystem_Security_Cryptography_DSAParametersEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.DSAParameters? left, System.Security.Cryptography.DSAParameters? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Cryptography.DSAParameters? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Cryptography_FromBase64TransformModeEqualityComparer : IEqualityComparer<System.Security.Cryptography.FromBase64TransformMode>
		{
			public System_Security_Cryptography_FromBase64TransformModeEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.FromBase64TransformMode left, System.Security.Cryptography.FromBase64TransformMode right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Cryptography.FromBase64TransformMode obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Cryptography_FromBase64TransformModeEqualityComparer : IEqualityComparer<System.Security.Cryptography.FromBase64TransformMode?>
		{
			public NullableSystem_Security_Cryptography_FromBase64TransformModeEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.FromBase64TransformMode? left, System.Security.Cryptography.FromBase64TransformMode? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Cryptography.FromBase64TransformMode? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Cryptography_KeyNumberEqualityComparer : IEqualityComparer<System.Security.Cryptography.KeyNumber>
		{
			public System_Security_Cryptography_KeyNumberEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.KeyNumber left, System.Security.Cryptography.KeyNumber right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Cryptography.KeyNumber obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Cryptography_KeyNumberEqualityComparer : IEqualityComparer<System.Security.Cryptography.KeyNumber?>
		{
			public NullableSystem_Security_Cryptography_KeyNumberEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.KeyNumber? left, System.Security.Cryptography.KeyNumber? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Cryptography.KeyNumber? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Cryptography_PaddingModeEqualityComparer : IEqualityComparer<System.Security.Cryptography.PaddingMode>
		{
			public System_Security_Cryptography_PaddingModeEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.PaddingMode left, System.Security.Cryptography.PaddingMode right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Cryptography.PaddingMode obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Cryptography_PaddingModeEqualityComparer : IEqualityComparer<System.Security.Cryptography.PaddingMode?>
		{
			public NullableSystem_Security_Cryptography_PaddingModeEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.PaddingMode? left, System.Security.Cryptography.PaddingMode? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Cryptography.PaddingMode? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Cryptography_RSAParametersEqualityComparer : IEqualityComparer<System.Security.Cryptography.RSAParameters>
		{
			public System_Security_Cryptography_RSAParametersEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.RSAParameters left, System.Security.Cryptography.RSAParameters right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Cryptography.RSAParameters obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Cryptography_RSAParametersEqualityComparer : IEqualityComparer<System.Security.Cryptography.RSAParameters?>
		{
			public NullableSystem_Security_Cryptography_RSAParametersEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.RSAParameters? left, System.Security.Cryptography.RSAParameters? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Cryptography.RSAParameters? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Cryptography_X509Certificates_X509ContentTypeEqualityComparer : IEqualityComparer<System.Security.Cryptography.X509Certificates.X509ContentType>
		{
			public System_Security_Cryptography_X509Certificates_X509ContentTypeEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.X509Certificates.X509ContentType left, System.Security.Cryptography.X509Certificates.X509ContentType right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Cryptography.X509Certificates.X509ContentType obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Cryptography_X509Certificates_X509ContentTypeEqualityComparer : IEqualityComparer<System.Security.Cryptography.X509Certificates.X509ContentType?>
		{
			public NullableSystem_Security_Cryptography_X509Certificates_X509ContentTypeEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.X509Certificates.X509ContentType? left, System.Security.Cryptography.X509Certificates.X509ContentType? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Cryptography.X509Certificates.X509ContentType? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Cryptography_X509Certificates_X509KeyStorageFlagsEqualityComparer : IEqualityComparer<System.Security.Cryptography.X509Certificates.X509KeyStorageFlags>
		{
			public System_Security_Cryptography_X509Certificates_X509KeyStorageFlagsEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.X509Certificates.X509KeyStorageFlags left, System.Security.Cryptography.X509Certificates.X509KeyStorageFlags right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Cryptography.X509Certificates.X509KeyStorageFlags obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Cryptography_X509Certificates_X509KeyStorageFlagsEqualityComparer : IEqualityComparer<System.Security.Cryptography.X509Certificates.X509KeyStorageFlags?>
		{
			public NullableSystem_Security_Cryptography_X509Certificates_X509KeyStorageFlagsEqualityComparer() {}

			public bool Equals( System.Security.Cryptography.X509Certificates.X509KeyStorageFlags? left, System.Security.Cryptography.X509Certificates.X509KeyStorageFlags? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Cryptography.X509Certificates.X509KeyStorageFlags? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Policy_ApplicationVersionMatchEqualityComparer : IEqualityComparer<System.Security.Policy.ApplicationVersionMatch>
		{
			public System_Security_Policy_ApplicationVersionMatchEqualityComparer() {}

			public bool Equals( System.Security.Policy.ApplicationVersionMatch left, System.Security.Policy.ApplicationVersionMatch right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Policy.ApplicationVersionMatch obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Policy_ApplicationVersionMatchEqualityComparer : IEqualityComparer<System.Security.Policy.ApplicationVersionMatch?>
		{
			public NullableSystem_Security_Policy_ApplicationVersionMatchEqualityComparer() {}

			public bool Equals( System.Security.Policy.ApplicationVersionMatch? left, System.Security.Policy.ApplicationVersionMatch? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Policy.ApplicationVersionMatch? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Policy_PolicyStatementAttributeEqualityComparer : IEqualityComparer<System.Security.Policy.PolicyStatementAttribute>
		{
			public System_Security_Policy_PolicyStatementAttributeEqualityComparer() {}

			public bool Equals( System.Security.Policy.PolicyStatementAttribute left, System.Security.Policy.PolicyStatementAttribute right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Policy.PolicyStatementAttribute obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Policy_PolicyStatementAttributeEqualityComparer : IEqualityComparer<System.Security.Policy.PolicyStatementAttribute?>
		{
			public NullableSystem_Security_Policy_PolicyStatementAttributeEqualityComparer() {}

			public bool Equals( System.Security.Policy.PolicyStatementAttribute? left, System.Security.Policy.PolicyStatementAttribute? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Policy.PolicyStatementAttribute? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Policy_TrustManagerUIContextEqualityComparer : IEqualityComparer<System.Security.Policy.TrustManagerUIContext>
		{
			public System_Security_Policy_TrustManagerUIContextEqualityComparer() {}

			public bool Equals( System.Security.Policy.TrustManagerUIContext left, System.Security.Policy.TrustManagerUIContext right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Policy.TrustManagerUIContext obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Policy_TrustManagerUIContextEqualityComparer : IEqualityComparer<System.Security.Policy.TrustManagerUIContext?>
		{
			public NullableSystem_Security_Policy_TrustManagerUIContextEqualityComparer() {}

			public bool Equals( System.Security.Policy.TrustManagerUIContext? left, System.Security.Policy.TrustManagerUIContext? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Policy.TrustManagerUIContext? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Principal_PrincipalPolicyEqualityComparer : IEqualityComparer<System.Security.Principal.PrincipalPolicy>
		{
			public System_Security_Principal_PrincipalPolicyEqualityComparer() {}

			public bool Equals( System.Security.Principal.PrincipalPolicy left, System.Security.Principal.PrincipalPolicy right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Principal.PrincipalPolicy obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Principal_PrincipalPolicyEqualityComparer : IEqualityComparer<System.Security.Principal.PrincipalPolicy?>
		{
			public NullableSystem_Security_Principal_PrincipalPolicyEqualityComparer() {}

			public bool Equals( System.Security.Principal.PrincipalPolicy? left, System.Security.Principal.PrincipalPolicy? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Principal.PrincipalPolicy? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Principal_TokenAccessLevelsEqualityComparer : IEqualityComparer<System.Security.Principal.TokenAccessLevels>
		{
			public System_Security_Principal_TokenAccessLevelsEqualityComparer() {}

			public bool Equals( System.Security.Principal.TokenAccessLevels left, System.Security.Principal.TokenAccessLevels right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Principal.TokenAccessLevels obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Principal_TokenAccessLevelsEqualityComparer : IEqualityComparer<System.Security.Principal.TokenAccessLevels?>
		{
			public NullableSystem_Security_Principal_TokenAccessLevelsEqualityComparer() {}

			public bool Equals( System.Security.Principal.TokenAccessLevels? left, System.Security.Principal.TokenAccessLevels? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Principal.TokenAccessLevels? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Principal_TokenImpersonationLevelEqualityComparer : IEqualityComparer<System.Security.Principal.TokenImpersonationLevel>
		{
			public System_Security_Principal_TokenImpersonationLevelEqualityComparer() {}

			public bool Equals( System.Security.Principal.TokenImpersonationLevel left, System.Security.Principal.TokenImpersonationLevel right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Principal.TokenImpersonationLevel obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Principal_TokenImpersonationLevelEqualityComparer : IEqualityComparer<System.Security.Principal.TokenImpersonationLevel?>
		{
			public NullableSystem_Security_Principal_TokenImpersonationLevelEqualityComparer() {}

			public bool Equals( System.Security.Principal.TokenImpersonationLevel? left, System.Security.Principal.TokenImpersonationLevel? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Principal.TokenImpersonationLevel? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Principal_WellKnownSidTypeEqualityComparer : IEqualityComparer<System.Security.Principal.WellKnownSidType>
		{
			public System_Security_Principal_WellKnownSidTypeEqualityComparer() {}

			public bool Equals( System.Security.Principal.WellKnownSidType left, System.Security.Principal.WellKnownSidType right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Principal.WellKnownSidType obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Principal_WellKnownSidTypeEqualityComparer : IEqualityComparer<System.Security.Principal.WellKnownSidType?>
		{
			public NullableSystem_Security_Principal_WellKnownSidTypeEqualityComparer() {}

			public bool Equals( System.Security.Principal.WellKnownSidType? left, System.Security.Principal.WellKnownSidType? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Principal.WellKnownSidType? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Principal_WindowsAccountTypeEqualityComparer : IEqualityComparer<System.Security.Principal.WindowsAccountType>
		{
			public System_Security_Principal_WindowsAccountTypeEqualityComparer() {}

			public bool Equals( System.Security.Principal.WindowsAccountType left, System.Security.Principal.WindowsAccountType right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Principal.WindowsAccountType obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Principal_WindowsAccountTypeEqualityComparer : IEqualityComparer<System.Security.Principal.WindowsAccountType?>
		{
			public NullableSystem_Security_Principal_WindowsAccountTypeEqualityComparer() {}

			public bool Equals( System.Security.Principal.WindowsAccountType? left, System.Security.Principal.WindowsAccountType? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Principal.WindowsAccountType? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Security_Principal_WindowsBuiltInRoleEqualityComparer : IEqualityComparer<System.Security.Principal.WindowsBuiltInRole>
		{
			public System_Security_Principal_WindowsBuiltInRoleEqualityComparer() {}

			public bool Equals( System.Security.Principal.WindowsBuiltInRole left, System.Security.Principal.WindowsBuiltInRole right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Security.Principal.WindowsBuiltInRole obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Security_Principal_WindowsBuiltInRoleEqualityComparer : IEqualityComparer<System.Security.Principal.WindowsBuiltInRole?>
		{
			public NullableSystem_Security_Principal_WindowsBuiltInRoleEqualityComparer() {}

			public bool Equals( System.Security.Principal.WindowsBuiltInRole? left, System.Security.Principal.WindowsBuiltInRole? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Security.Principal.WindowsBuiltInRole? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Text_NormalizationFormEqualityComparer : IEqualityComparer<System.Text.NormalizationForm>
		{
			public System_Text_NormalizationFormEqualityComparer() {}

			public bool Equals( System.Text.NormalizationForm left, System.Text.NormalizationForm right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Text.NormalizationForm obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Text_NormalizationFormEqualityComparer : IEqualityComparer<System.Text.NormalizationForm?>
		{
			public NullableSystem_Text_NormalizationFormEqualityComparer() {}

			public bool Equals( System.Text.NormalizationForm? left, System.Text.NormalizationForm? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Text.NormalizationForm? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Threading_ApartmentStateEqualityComparer : IEqualityComparer<System.Threading.ApartmentState>
		{
			public System_Threading_ApartmentStateEqualityComparer() {}

			public bool Equals( System.Threading.ApartmentState left, System.Threading.ApartmentState right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Threading.ApartmentState obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Threading_ApartmentStateEqualityComparer : IEqualityComparer<System.Threading.ApartmentState?>
		{
			public NullableSystem_Threading_ApartmentStateEqualityComparer() {}

			public bool Equals( System.Threading.ApartmentState? left, System.Threading.ApartmentState? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Threading.ApartmentState? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Threading_AsyncFlowControlEqualityComparer : IEqualityComparer<System.Threading.AsyncFlowControl>
		{
			public System_Threading_AsyncFlowControlEqualityComparer() {}

			public bool Equals( System.Threading.AsyncFlowControl left, System.Threading.AsyncFlowControl right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Threading.AsyncFlowControl obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Threading_AsyncFlowControlEqualityComparer : IEqualityComparer<System.Threading.AsyncFlowControl?>
		{
			public NullableSystem_Threading_AsyncFlowControlEqualityComparer() {}

			public bool Equals( System.Threading.AsyncFlowControl? left, System.Threading.AsyncFlowControl? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Threading.AsyncFlowControl? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Threading_EventResetModeEqualityComparer : IEqualityComparer<System.Threading.EventResetMode>
		{
			public System_Threading_EventResetModeEqualityComparer() {}

			public bool Equals( System.Threading.EventResetMode left, System.Threading.EventResetMode right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Threading.EventResetMode obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Threading_EventResetModeEqualityComparer : IEqualityComparer<System.Threading.EventResetMode?>
		{
			public NullableSystem_Threading_EventResetModeEqualityComparer() {}

			public bool Equals( System.Threading.EventResetMode? left, System.Threading.EventResetMode? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Threading.EventResetMode? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Threading_LockCookieEqualityComparer : IEqualityComparer<System.Threading.LockCookie>
		{
			public System_Threading_LockCookieEqualityComparer() {}

			public bool Equals( System.Threading.LockCookie left, System.Threading.LockCookie right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Threading.LockCookie obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Threading_LockCookieEqualityComparer : IEqualityComparer<System.Threading.LockCookie?>
		{
			public NullableSystem_Threading_LockCookieEqualityComparer() {}

			public bool Equals( System.Threading.LockCookie? left, System.Threading.LockCookie? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Threading.LockCookie? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}
#if MSGPACK_UNITY_FULL

		private sealed class System_Threading_LockRecursionPolicyEqualityComparer : IEqualityComparer<System.Threading.LockRecursionPolicy>
		{
			public System_Threading_LockRecursionPolicyEqualityComparer() {}

			public bool Equals( System.Threading.LockRecursionPolicy left, System.Threading.LockRecursionPolicy right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Threading.LockRecursionPolicy obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Threading_LockRecursionPolicyEqualityComparer : IEqualityComparer<System.Threading.LockRecursionPolicy?>
		{
			public NullableSystem_Threading_LockRecursionPolicyEqualityComparer() {}

			public bool Equals( System.Threading.LockRecursionPolicy? left, System.Threading.LockRecursionPolicy? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Threading.LockRecursionPolicy? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}
#endif // MSGPACK_UNITY_FULL

		private sealed class System_Threading_NativeOverlappedEqualityComparer : IEqualityComparer<System.Threading.NativeOverlapped>
		{
			public System_Threading_NativeOverlappedEqualityComparer() {}

			public bool Equals( System.Threading.NativeOverlapped left, System.Threading.NativeOverlapped right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Threading.NativeOverlapped obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Threading_NativeOverlappedEqualityComparer : IEqualityComparer<System.Threading.NativeOverlapped?>
		{
			public NullableSystem_Threading_NativeOverlappedEqualityComparer() {}

			public bool Equals( System.Threading.NativeOverlapped? left, System.Threading.NativeOverlapped? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Threading.NativeOverlapped? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Threading_ThreadPriorityEqualityComparer : IEqualityComparer<System.Threading.ThreadPriority>
		{
			public System_Threading_ThreadPriorityEqualityComparer() {}

			public bool Equals( System.Threading.ThreadPriority left, System.Threading.ThreadPriority right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Threading.ThreadPriority obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Threading_ThreadPriorityEqualityComparer : IEqualityComparer<System.Threading.ThreadPriority?>
		{
			public NullableSystem_Threading_ThreadPriorityEqualityComparer() {}

			public bool Equals( System.Threading.ThreadPriority? left, System.Threading.ThreadPriority? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Threading.ThreadPriority? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}

		private sealed class System_Threading_ThreadStateEqualityComparer : IEqualityComparer<System.Threading.ThreadState>
		{
			public System_Threading_ThreadStateEqualityComparer() {}

			public bool Equals( System.Threading.ThreadState left, System.Threading.ThreadState right )
			{
				return left.Equals( right );
			}

			public int GetHashCode( System.Threading.ThreadState obj )
			{
				return obj.GetHashCode();
			}
		}

		private sealed class NullableSystem_Threading_ThreadStateEqualityComparer : IEqualityComparer<System.Threading.ThreadState?>
		{
			public NullableSystem_Threading_ThreadStateEqualityComparer() {}

			public bool Equals( System.Threading.ThreadState? left, System.Threading.ThreadState? right )
			{
				if ( !left.HasValue )
				{
					return !right.HasValue;
				}

				if ( !right.HasValue )
				{
					return false;
				}
				return left.Value.Equals( right.Value );
			}

			public int GetHashCode( System.Threading.ThreadState? obj )
			{
				return !obj.HasValue ? 0 : obj.GetHashCode();
			}
		}
	}
}
