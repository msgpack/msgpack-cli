#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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
using System.IO;

namespace mpu
{
	/// <summary>
	///		Supports colorized text output.
	/// </summary>
	internal sealed class ColorizedTextWriter
	{
		private readonly TextWriter _writer;
		private readonly Action<ConsoleColor> _colorSetter;
		private readonly Func<ConsoleColor> _colorGetter;

		private ColorizedTextWriter( TextWriter writer, Action<ConsoleColor> colorSetter, Func<ConsoleColor> colorGetter )
		{
			this._writer = writer;
			this._colorGetter = colorGetter;
			this._colorSetter = colorSetter;
		}

		/// <summary>
		///		Gets a instance for standard output stream with coloring if the redirected standard output platform supports it.
		/// </summary>
		/// <returns><see cref="ColorizedTextWriter"/>. This value will not be <c>null</c>.</returns>
		public static ColorizedTextWriter ForConsoleOutput()
		{
			return ForConsole( Console.Out );
		}

		/// <summary>
		///		Gets a instance for standard error stream with coloring if the redirected standard error platform supports it.
		/// </summary>
		/// <returns><see cref="ColorizedTextWriter"/>. This value will not be <c>null</c>.</returns>
		public static ColorizedTextWriter ForConsoleError()
		{
			return ForConsole( Console.Error );
		}

		private static ColorizedTextWriter ForConsole( TextWriter textWriter )
		{
			return
				new ColorizedTextWriter(
					textWriter,
					color => Console.ForegroundColor = color,
					() => Console.ForegroundColor
				);
		}

		/// <summary>
		///		Gets a instance for specified <see cref="TextWriter"/> without coloring.
		/// </summary>
		/// <param name="writer">A <see cref="TextWriter"/>.</param>
		/// <returns><see cref="ColorizedTextWriter"/>. This value will not be <c>null</c>.</returns>
		public static ColorizedTextWriter ForTextWriter( TextWriter writer )
		{
			return new ColorizedTextWriter( writer ?? TextWriter.Null, _ => { }, () => ConsoleColor.Gray );
		}

		/// <summary>
		///		Writes a warning message.
		/// </summary>
		/// <param name="message">A message.</param>
		public void WriteWarning( string message )
		{
			this.WriteLineWithColor( ConsoleColor.Yellow, message );
		}

		/// <summary>
		///		Writes an error message.
		/// </summary>
		/// <param name="message">A message.</param>
		public void WriteError( string message )
		{
			this.WriteLineWithColor( ConsoleColor.Red, message );
		}

		private void WriteLineWithColor( ConsoleColor color, string message )
		{
			var originalColor = this._colorGetter();
			try
			{
				this._colorSetter( color );
				this._writer.WriteLine( message );
			}
			finally
			{
				this._colorSetter( originalColor );
			}
		}

		/// <summary>
		///		Flushes a buffer of underlying <see cref="TextWriter"/>.
		/// </summary>
		public void Flush()
		{
			this._writer.Flush();
		}
	}
}