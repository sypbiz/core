using System;
using System.Runtime.InteropServices;

namespace syp.biz.Core.Types
{
	/// <summary>
	/// A thread-safe <see cref="Console"/> wrapper.
	/// </summary>
	public sealed class SafeConsole
	{
		#region Imports
		/// <summary>
		/// Allocates a new console for the calling process.
		/// </summary>
		/// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
		/// <remarks>
		/// See <a href="http://pinvoke.net/default.aspx/kernel32/AllocConsole.html">AllocConsole</a> on pInvoke.
		/// </remarks> 
		[DllImport("kernel32", SetLastError = true)]
		private static extern bool AllocConsole();

		/// <summary>
		/// Detaches the calling process from its console.
		/// </summary>
		/// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
		/// <remarks>
		/// See <a href="http://pinvoke.net/default.aspx/kernel32/FreeConsole.html">FreeConsole</a> on pInvoke.
		/// </remarks>
		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		private static extern bool FreeConsole();

		/// <summary>
		/// Retrieves the window handle used by the console associated with the calling process.
		/// </summary>
		/// <returns>The return value is a handle to the window used by the console associated with the calling process or <see cref="IntPtr.Zero"/> if there is no such associated console.</returns>
		/// <remarks>
		/// See <a href="http://pinvoke.net/default.aspx/kernel32/GetConsoleWindow.html">GetConsoleWindow</a> on pInvoke.
		/// </remarks>
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr GetConsoleWindow();
		#endregion Imports

		private static readonly Locker Lock = new Locker(new object());

		/// <summary>
		/// Safely gets or sets the foreground color of the console.
		/// </summary>
		/// <returns>
		/// A <see cref="ConsoleColor"/> that specifies the foreground color of the console; that is, the color of each character that is displayed. The default is gray.
		/// </returns>
		/// <exception cref="ArgumentException">The color specified in a set operation is not a valid member of <see cref="ConsoleColor"/>.</exception>
		/// <exception cref="System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occurred.</exception>
		public ConsoleColor ForegroundColor
		{
			get => Lock.Run(() => Console.ForegroundColor);
			set => Lock.Run(() => Console.ForegroundColor = value);
		}

		#region WriteLine
		/// <summary>Safely writes the current line terminator to the standard output stream.</summary>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine() => Lock.Run(Console.WriteLine);

		/// <summary>Safely writes the specified subarray of Unicode characters, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="buffer">An array of Unicode characters.</param>
		/// <param name="index">The starting position in buffer.</param>
		/// <param name="count">The number of characters to write.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="buffer">buffer</paramref> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index">index</paramref> or <paramref name="count">count</paramref> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="index">index</paramref> plus <paramref name="count">count</paramref> specify a position that is not within <paramref name="buffer">buffer</paramref>.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(char[] buffer, int index, int count) => Lock.Run(() => Console.WriteLine(buffer, index, count));

		/// <summary>Safely writes the text representation of the specified object, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">An object to write using format.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="format">format</paramref> is null.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format">format</paramref> is invalid.</exception>
		public void WriteLine(string format, object arg0) => Lock.Run(() => Console.WriteLine(format, arg0));

		/// <summary>Safely writes the text representation of the specified objects, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to write using format.</param>
		/// <param name="arg1">The second object to write using format.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="format">format</paramref> is null.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format">format</paramref> is invalid.</exception>
		public void WriteLine(string format, object arg0, object arg1) => Lock.Run(() => Console.WriteLine(format, arg0, arg1));

		/// <summary>Safely writes the text representation of the specified objects, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to write using format.</param>
		/// <param name="arg1">The second object to write using format.</param>
		/// <param name="arg2">The third object to write using format.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="format">format</paramref> is null.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format">format</paramref> is invalid.</exception>
		public void WriteLine(string format, object arg0, object arg1, object arg2) => Lock.Run(() => Console.WriteLine(format, arg0, arg1, arg2));

		/// <summary>Safely writes the text representation of the specified array of objects, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg">An array of objects to write using format.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="format"/> or <paramref name="arg"/> is null.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format"/> is invalid.</exception>
		public void WriteLine(string format, params object[] arg) => Lock.Run(() => Console.WriteLine(format, arg));

		/// <summary>Safely writes the text representation of the specified Boolean value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(bool value) => Lock.Run(() => Console.WriteLine(value));

		/// <summary>Safely writes the specified Unicode character, followed by the current line terminator, value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(char value) => Lock.Run(() => Console.WriteLine(value));

		/// <summary>Safely writes the specified array of Unicode characters, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="buffer">A Unicode character array.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(char[] buffer) => Lock.Run(() => Console.WriteLine(buffer));

		/// <summary>Safely writes the text representation of the specified <see cref="T:System.Decimal"></see> value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(decimal value) => Lock.Run(() => Console.WriteLine(value));

		/// <summary>Safely writes the text representation of the specified double-precision floating-point value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(double value) => Lock.Run(() => Console.WriteLine(value));

		/// <summary>Safely writes the text representation of the specified single-precision floating-point value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(float value) => Lock.Run(() => Console.WriteLine(value));

		/// <summary>Safely writes the text representation of the specified 32-bit signed integer value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(int value) => Lock.Run(() => Console.WriteLine(value));

		/// <summary>Safely writes the text representation of the specified 64-bit signed integer value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(long value) => Lock.Run(() => Console.WriteLine(value));

		/// <summary>Safely writes the text representation of the specified object, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(object value) => Lock.Run(() => Console.WriteLine(value));

		/// <summary>Safely writes the specified string value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(string value) => Lock.Run(() => Console.WriteLine(value));

		/// <summary>Safely writes the text representation of the specified 32-bit unsigned integer value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(uint value) => Lock.Run(() => Console.WriteLine(value));

		/// <summary>Safely writes the text representation of the specified 64-bit unsigned integer value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(ulong value) => Lock.Run(() => Console.WriteLine(value));
		#endregion WriteLine

		#region Color WriteLine
		/// <summary>Safely writes the specified subarray of Unicode characters, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="buffer">An array of Unicode characters.</param>
		/// <param name="index">The starting position in buffer.</param>
		/// <param name="count">The number of characters to write.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="buffer">buffer</paramref> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index">index</paramref> or <paramref name="count">count</paramref> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="index">index</paramref> plus <paramref name="count">count</paramref> specify a position that is not within <paramref name="buffer">buffer</paramref>.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(ConsoleColor color, char[] buffer, int index, int count) => LockedWithColor(color, () => Console.WriteLine(buffer, index, count));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified object, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">An object to write using format.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="format">format</paramref> is null.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format">format</paramref> is invalid.</exception>
		public void WriteLine(ConsoleColor color, string format, object arg0) => LockedWithColor(color, () => Console.WriteLine(format, arg0));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified objects, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to write using format.</param>
		/// <param name="arg1">The second object to write using format.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="format">format</paramref> is null.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format">format</paramref> is invalid.</exception>
		public void WriteLine(ConsoleColor color, string format, object arg0, object arg1) => LockedWithColor(color, () => Console.WriteLine(format, arg0, arg1));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified objects, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to write using format.</param>
		/// <param name="arg1">The second object to write using format.</param>
		/// <param name="arg2">The third object to write using format.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="format">format</paramref> is null.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format">format</paramref> is invalid.</exception>
		public void WriteLine(ConsoleColor color, string format, object arg0, object arg1, object arg2) => LockedWithColor(color, () => Console.WriteLine(format, arg0, arg1, arg2));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified array of objects, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg">An array of objects to write using format.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="format"/> or <paramref name="arg"/> is null.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format"/> is invalid.</exception>
		public void WriteLine(ConsoleColor color, string format, params object[] arg) => LockedWithColor(color, () => Console.WriteLine(format, arg));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified Boolean value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(ConsoleColor color, bool value) => LockedWithColor(color, () => Console.WriteLine(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the specified Unicode character, followed by the current line terminator, value to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(ConsoleColor color, char value) => LockedWithColor(color, () => Console.WriteLine(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the specified array of Unicode characters, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="buffer">A Unicode character array.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(ConsoleColor color, char[] buffer) => LockedWithColor(color, () => Console.WriteLine(buffer));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified <see cref="T:System.Decimal"></see> value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(ConsoleColor color, decimal value) => LockedWithColor(color, () => Console.WriteLine(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified double-precision floating-point value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(ConsoleColor color, double value) => LockedWithColor(color, () => Console.WriteLine(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified single-precision floating-point value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(ConsoleColor color, float value) => LockedWithColor(color, () => Console.WriteLine(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified 32-bit signed integer value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(ConsoleColor color, int value) => LockedWithColor(color, () => Console.WriteLine(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified 64-bit signed integer value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(ConsoleColor color, long value) => LockedWithColor(color, () => Console.WriteLine(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified object, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(ConsoleColor color, object value) => LockedWithColor(color, () => Console.WriteLine(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the specified string value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(ConsoleColor color, string value) => LockedWithColor(color, () => Console.WriteLine(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified 32-bit unsigned integer value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(ConsoleColor color, uint value) => LockedWithColor(color, () => Console.WriteLine(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified 64-bit unsigned integer value, followed by the current line terminator, to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void WriteLine(ConsoleColor color, ulong value) => LockedWithColor(color, () => Console.WriteLine(value));
		#endregion Color WriteLine

		#region Write
		/// <summary>Safely writes the specified subarray of Unicode characters to the standard output stream.</summary>
		/// <param name="buffer">An array of Unicode characters.</param>
		/// <param name="index">The starting position in buffer.</param>
		/// <param name="count">The number of characters to write.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="buffer">buffer</paramref> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index">index</paramref> or <paramref name="count">count</paramref> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="index">index</paramref> plus <paramref name="count">count</paramref> specify a position that is not within <paramref name="buffer">buffer</paramref>.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(char[] buffer, int index, int count) => Lock.Run(() => Console.Write(buffer, index, count));

		/// <summary>Safely writes the text representation of the specified object to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">An object to write using format.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="format">format</paramref> is null.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format">format</paramref> is invalid.</exception>
		public void Write(string format, object arg0) => Lock.Run(() => Console.Write(format, arg0));

		/// <summary>Safely writes the text representation of the specified objects to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to write using format.</param>
		/// <param name="arg1">The second object to write using format.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="format">format</paramref> is null.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format">format</paramref> is invalid.</exception>
		public void Write(string format, object arg0, object arg1) => Lock.Run(() => Console.Write(format, arg0, arg1));

		/// <summary>Safely writes the text representation of the specified objects to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to write using format.</param>
		/// <param name="arg1">The second object to write using format.</param>
		/// <param name="arg2">The third object to write using format.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="format">format</paramref> is null.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format">format</paramref> is invalid.</exception>
		public void Write(string format, object arg0, object arg1, object arg2) => Lock.Run(() => Console.Write(format, arg0, arg1, arg2));

		/// <summary>Safely writes the text representation of the specified array of objects to the standard output stream using the specified format information.</summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg">An array of objects to write using format.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="format"/> or <paramref name="arg"/> is null.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format"/> is invalid.</exception>
		public void Write(string format, params object[] arg) => Lock.Run(() => Console.Write(format, arg));

		/// <summary>Safely writes the text representation of the specified Boolean value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(bool value) => Lock.Run(() => Console.Write(value));

		/// <summary>Safely writes the specified Unicode character value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(char value) => Lock.Run(() => Console.Write(value));

		/// <summary>Safely writes the specified array of Unicode characters to the standard output stream.</summary>
		/// <param name="buffer">A Unicode character array.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(char[] buffer) => Lock.Run(() => Console.Write(buffer));

		/// <summary>Safely writes the text representation of the specified <see cref="T:System.Decimal"></see> value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(decimal value) => Lock.Run(() => Console.Write(value));

		/// <summary>Safely writes the text representation of the specified double-precision floating-point value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(double value) => Lock.Run(() => Console.Write(value));

		/// <summary>Safely writes the text representation of the specified single-precision floating-point value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(float value) => Lock.Run(() => Console.Write(value));

		/// <summary>Safely writes the text representation of the specified 32-bit signed integer value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(int value) => Lock.Run(() => Console.Write(value));

		/// <summary>Safely writes the text representation of the specified 64-bit signed integer value to the standard output stream.</summary>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(long value) => Lock.Run(() => Console.Write(value));

		/// <summary>Safely writes the text representation of the specified object to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(object value) => Lock.Run(() => Console.Write(value));

		/// <summary>Safely writes the specified string value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(string value) => Lock.Run(() => Console.Write(value));

		/// <summary>Safely writes the text representation of the specified 32-bit unsigned integer value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(uint value) => Lock.Run(() => Console.Write(value));

		/// <summary>Safely writes the text representation of the specified 64-bit unsigned integer value to the standard output stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(ulong value) => Lock.Run(() => Console.Write(value));
		#endregion Write

		#region Color Write
		/// <summary>Safely writes, in <paramref name="color"/>, the specified subarray of Unicode characters to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="buffer">An array of Unicode characters.</param>
		/// <param name="index">The starting position in buffer.</param>
		/// <param name="count">The number of characters to write.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="buffer">buffer</paramref> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index">index</paramref> or <paramref name="count">count</paramref> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="index">index</paramref> plus <paramref name="count">count</paramref> specify a position that is not within <paramref name="buffer">buffer</paramref>.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(ConsoleColor color, char[] buffer, int index, int count) => LockedWithColor(color, () => Console.Write(buffer, index, count));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified object to the standard output stream using the specified format information.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">An object to write using format.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="format">format</paramref> is null.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format">format</paramref> is invalid.</exception>
		public void Write(ConsoleColor color, string format, object arg0) => LockedWithColor(color, () => Console.Write(format, arg0));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified objects to the standard output stream using the specified format information.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to write using format.</param>
		/// <param name="arg1">The second object to write using format.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="format">format</paramref> is null.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format">format</paramref> is invalid.</exception>
		public void Write(ConsoleColor color, string format, object arg0, object arg1) => LockedWithColor(color, () => Console.Write(format, arg0, arg1));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified objects to the standard output stream using the specified format information.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The first object to write using format.</param>
		/// <param name="arg1">The second object to write using format.</param>
		/// <param name="arg2">The third object to write using format.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="format">format</paramref> is null.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format">format</paramref> is invalid.</exception>
		public void Write(ConsoleColor color, string format, object arg0, object arg1, object arg2) => LockedWithColor(color, () => Console.Write(format, arg0, arg1, arg2));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified array of objects to the standard output stream using the specified format information.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg">An array of objects to write using format.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="format"/> or <paramref name="arg"/> is null.</exception>
		/// <exception cref="T:System.FormatException">The format specification in <paramref name="format"/> is invalid.</exception>
		public void Write(ConsoleColor color, string format, params object[] arg) => LockedWithColor(color, () => Console.Write(format, arg));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified Boolean value to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(ConsoleColor color, bool value) => LockedWithColor(color, () => Console.Write(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the specified Unicode character value to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(ConsoleColor color, char value) => LockedWithColor(color, () => Console.Write(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the specified array of Unicode characters to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="buffer">A Unicode character array.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(ConsoleColor color, char[] buffer) => LockedWithColor(color, () => Console.Write(buffer));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified <see cref="T:System.Decimal"></see> value to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(ConsoleColor color, decimal value) => LockedWithColor(color, () => Console.Write(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified double-precision floating-point value to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(ConsoleColor color, double value) => LockedWithColor(color, () => Console.Write(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified single-precision floating-point value to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(ConsoleColor color, float value) => LockedWithColor(color, () => Console.Write(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified 32-bit signed integer value to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(ConsoleColor color, int value) => LockedWithColor(color, () => Console.Write(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified 64-bit signed integer value to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(ConsoleColor color, long value) => LockedWithColor(color, () => Console.Write(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified object to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(ConsoleColor color, object value) => LockedWithColor(color, () => Console.Write(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the specified string value to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(ConsoleColor color, string value) => LockedWithColor(color, () => Console.Write(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified 32-bit unsigned integer value to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(ConsoleColor color, uint value) => LockedWithColor(color, () => Console.Write(value));

		/// <summary>Safely writes, in <paramref name="color"/>, the text representation of the specified 64-bit unsigned integer value to the standard output stream.</summary>
		/// <param name="color">The color to write in.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		public void Write(ConsoleColor color, ulong value) => LockedWithColor(color, () => Console.Write(value));
		#endregion Color Write

		#region ReadLine
		/// <summary>Safely reads the next line of characters from the standard input stream.</summary>
		/// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters in the next line of characters is greater than <see cref="F:System.Int32.MaxValue"></see>.</exception>
		public string ReadLine() => Lock.Run(Console.ReadLine);

		/// <summary>Safely prompts the user with a <paramref name="message"/> in <see cref="ConsoleColor.Gray"/> and reads the next line of characters from the standard input stream.</summary>
		/// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters in the next line of characters is greater than <see cref="F:System.Int32.MaxValue"></see>.</exception>
		/// <exception cref="T:System.ArgumentException">The color specified in a set operation is not a valid member of <see cref="T:System.ConsoleColor"></see>.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		public string ReadLine(string message) => this.ReadLine(ConsoleColor.Gray, message);

		/// <summary>Safely prompts the user with a <paramref name="message"/> in <paramref name="color"/> and reads the next line of characters from the standard input stream.</summary>
		/// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		/// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters in the next line of characters is greater than <see cref="F:System.Int32.MaxValue"></see>.</exception>
		/// <exception cref="T:System.ArgumentException">The color specified in a set operation is not a valid member of <see cref="T:System.ConsoleColor"></see>.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action.</exception>
		public string ReadLine(ConsoleColor color, string message)
		{
			return Lock.Run(() =>
			{
				Console.ForegroundColor = color;
				Console.Write($"{message}: ");
				return Console.ReadLine();
			});
		}
		#endregion ReadLine

		#region ReadKey
		/// <summary>Safely obtains the next character or function key pressed by the user. The pressed key is displayed in the console window.</summary>
		/// <returns>An object that describes the <see cref="T:System.ConsoleKey"></see> constant and Unicode character, if any, that correspond to the pressed console key. The <see cref="T:System.ConsoleKeyInfo"></see> object also describes, in a bitwise combination of <see cref="T:System.ConsoleModifiers"></see> values, whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously with the console key.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Console.In"></see> property is redirected from some stream other than the console.</exception>
		public ConsoleKeyInfo ReadKey() => Lock.Run(Console.ReadKey);

		/// <summary>Safely obtains the next character or function key pressed by the user. The pressed key is optionally displayed in the console window.</summary>
		/// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
		/// <returns>An object that describes the <see cref="T:System.ConsoleKey"></see> constant and Unicode character, if any, that correspond to the pressed console key. The <see cref="T:System.ConsoleKeyInfo"></see> object also describes, in a bitwise combination of <see cref="T:System.ConsoleModifiers"></see> values, whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously with the console key.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Console.In"></see> property is redirected from some stream other than the console.</exception>
		public ConsoleKeyInfo ReadKey(bool intercept) => Lock.Run(() => Console.ReadKey(intercept));

		/// <summary>
		/// Safely obtains the next character or function key pressed by the user. The pressed key is optionally displayed in the console window. Throws an exception if was not able to obtain a key within the given <paramref name="timeout"/>.
		/// </summary>
		/// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
		/// <param name="timeout">The duration to wait for the key to be obtained.</param>
		/// <returns>
		/// An object that describes the <see cref="T:System.ConsoleKey"></see> constant and Unicode character, if any, that correspond to the pressed console key.<br/>
		/// The <see cref="T:System.ConsoleKeyInfo"></see> object also describes, in a bitwise combination of <see cref="T:System.ConsoleModifiers"></see> values, whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously with the console key.
		/// </returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Console.In"></see> property is redirected from some stream other than the console.</exception>
		/// <exception cref="TimeoutException">Thrown if the key was not obtained in the given <paramref name="timeout"/>.</exception>
		public ConsoleKeyInfo ReadKey(bool intercept, TimeSpan timeout)
		{
			Func<bool, ConsoleKeyInfo> operation = Console.ReadKey;
			var invocation = operation.BeginInvoke(intercept, null, null);
			if (!invocation.AsyncWaitHandle.WaitOne(timeout) || !invocation.IsCompleted) throw new TimeoutException();
			return operation.EndInvoke(invocation);
		}

		/// <summary>
		/// Safely obtains the next character or function key pressed by the user. The pressed key is optionally displayed in the console window.
		/// </summary>
		/// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
		/// <param name="timeout">The duration to wait for the key to be obtained.</param>
		/// <param name="key">
		/// An object that describes the <see cref="T:System.ConsoleKey"></see> constant and Unicode character, if any, that correspond to the pressed console key.<br/>
		/// The <see cref="T:System.ConsoleKeyInfo"></see> object also describes, in a bitwise combination of <see cref="T:System.ConsoleModifiers"></see> values, whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously with the console key.
		/// </param>
		/// <returns><c>true</c> if key was obtained in the given <paramref name="timeout"/>, otherwise <c>false</c>.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Console.In"></see> property is redirected from some stream other than the console.</exception>
		public bool TryReadKey(bool intercept, TimeSpan timeout, out ConsoleKeyInfo key)
		{
			try
			{
				key = this.ReadKey(intercept, timeout);
				return true;
			}
			catch (TimeoutException)
			{
				key = default;
				return false;
			}
		}
		#endregion ReadKey

		/// <summary>
		/// Allocates a new console for the calling process.
		/// </summary>
		public static void OpenConsole()
		{
			if (!IsOpen()) AllocConsole();
		}

		/// <summary>
		/// Detaches the calling process from its console. 
		/// </summary>
		public static void CloseConsole() => FreeConsole();

		/// <summary>
		/// Checks if the current process has an opened console window.
		/// </summary>
		/// <returns><c>true</c> if a console window is opened for the current process, otherwise <c>false</c>.</returns>
		public static bool IsOpen() => GetConsoleWindow() != IntPtr.Zero;

		private static void LockedWithColor(ConsoleColor color, Action action)
		{
			Lock.Run(() =>
			{
				Console.ForegroundColor = color;
				action();
			});
		}
	}
}
