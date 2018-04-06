using System;
using System.Runtime.InteropServices;

namespace syp.biz.Core.Types
{
	public sealed class SafeConsole
	{
		#region Imports
		// http://pinvoke.net/default.aspx/kernel32/AllocConsole.html
		[DllImport("kernel32", SetLastError = true)]
		private static extern bool AllocConsole();

		// http://pinvoke.net/default.aspx/kernel32/FreeConsole.html
		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		private static extern bool FreeConsole();

		// http://pinvoke.net/default.aspx/kernel32/GetConsoleWindow.html
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr GetConsoleWindow();
		#endregion Imports

		private static readonly Locker Lock = new Locker(new object());

		public ConsoleColor ForegroundColor
		{
			get => Lock.Run(() => Console.ForegroundColor);
			set => Lock.Run(() => Console.ForegroundColor = value);
		}

		#region WriteLine
		public void WriteLine() => Lock.Run(Console.WriteLine);
		public void WriteLine(char[] buffer, int index, int count) => Lock.Run(() => Console.WriteLine(buffer, index, count));
		public void WriteLine(string format, object arg0) => Lock.Run(() => Console.WriteLine(format, arg0));
		public void WriteLine(string format, object arg0, object arg1) => Lock.Run(() => Console.WriteLine(format, arg0, arg1));
		public void WriteLine(string format, object arg0, object arg1, object arg2) => Lock.Run(() => Console.WriteLine(format, arg0, arg1, arg2));
		public void WriteLine(string format, params object[] arg) => Lock.Run(() => Console.WriteLine(format, arg));
		public void WriteLine(bool value) => Lock.Run(() => Console.WriteLine(value));
		public void WriteLine(char value) => Lock.Run(() => Console.WriteLine(value));
		public void WriteLine(char[] buffer) => Lock.Run(() => Console.WriteLine(buffer));
		public void WriteLine(decimal value) => Lock.Run(() => Console.WriteLine(value));
		public void WriteLine(double value) => Lock.Run(() => Console.WriteLine(value));
		public void WriteLine(float value) => Lock.Run(() => Console.WriteLine(value));
		public void WriteLine(int value) => Lock.Run(() => Console.WriteLine(value));
		public void WriteLine(long value) => Lock.Run(() => Console.WriteLine(value));
		public void WriteLine(object value) => Lock.Run(() => Console.WriteLine(value));
		public void WriteLine(string value) => Lock.Run(() => Console.WriteLine(value));
		public void WriteLine(uint value) => Lock.Run(() => Console.WriteLine(value));
		public void WriteLine(ulong value) => Lock.Run(() => Console.WriteLine(value));
		#endregion WriteLine

		#region Color WriteLine
		public void WriteLine(ConsoleColor color, char[] buffer, int index, int count) => this.LockedWithColor(color, () => Console.WriteLine(buffer, index, count));
		public void WriteLine(ConsoleColor color, string format, object arg0) => this.LockedWithColor(color, () => Console.WriteLine(format, arg0));
		public void WriteLine(ConsoleColor color, string format, object arg0, object arg1) => this.LockedWithColor(color, () => Console.WriteLine(format, arg0, arg1));
		public void WriteLine(ConsoleColor color, string format, object arg0, object arg1, object arg2) => this.LockedWithColor(color, () => Console.WriteLine(format, arg0, arg1, arg2));
		public void WriteLine(ConsoleColor color, string format, params object[] arg) => this.LockedWithColor(color, () => Console.WriteLine(format, arg));
		public void WriteLine(ConsoleColor color, bool value) => this.LockedWithColor(color, () => Console.WriteLine(value));
		public void WriteLine(ConsoleColor color, char value) => this.LockedWithColor(color, () => Console.WriteLine(value));
		public void WriteLine(ConsoleColor color, char[] buffer) => this.LockedWithColor(color, () => Console.WriteLine(buffer));
		public void WriteLine(ConsoleColor color, decimal value) => this.LockedWithColor(color, () => Console.WriteLine(value));
		public void WriteLine(ConsoleColor color, double value) => this.LockedWithColor(color, () => Console.WriteLine(value));
		public void WriteLine(ConsoleColor color, float value) => this.LockedWithColor(color, () => Console.WriteLine(value));
		public void WriteLine(ConsoleColor color, int value) => this.LockedWithColor(color, () => Console.WriteLine(value));
		public void WriteLine(ConsoleColor color, long value) => this.LockedWithColor(color, () => Console.WriteLine(value));
		public void WriteLine(ConsoleColor color, object value) => this.LockedWithColor(color, () => Console.WriteLine(value));
		public void WriteLine(ConsoleColor color, string value) => this.LockedWithColor(color, () => Console.WriteLine(value));
		public void WriteLine(ConsoleColor color, uint value) => this.LockedWithColor(color, () => Console.WriteLine(value));
		public void WriteLine(ConsoleColor color, ulong value) => this.LockedWithColor(color, () => Console.WriteLine(value));
		#endregion Color WriteLine

		#region Write
		public void Write(char[] buffer, int index, int count) => Lock.Run(() => Console.Write(buffer, index, count));
		public void Write(string format, object arg0) => Lock.Run(() => Console.Write(format, arg0));
		public void Write(string format, object arg0, object arg1) => Lock.Run(() => Console.Write(format, arg0, arg1));
		public void Write(string format, object arg0, object arg1, object arg2) => Lock.Run(() => Console.Write(format, arg0, arg1, arg2));
		public void Write(string format, params object[] arg) => Lock.Run(() => Console.Write(format, arg));
		public void Write(bool value) => Lock.Run(() => Console.Write(value));
		public void Write(char value) => Lock.Run(() => Console.Write(value));
		public void Write(char[] buffer) => Lock.Run(() => Console.Write(buffer));
		public void Write(decimal value) => Lock.Run(() => Console.Write(value));
		public void Write(double value) => Lock.Run(() => Console.Write(value));
		public void Write(float value) => Lock.Run(() => Console.Write(value));
		public void Write(int value) => Lock.Run(() => Console.Write(value));
		public void Write(long value) => Lock.Run(() => Console.Write(value));
		public void Write(object value) => Lock.Run(() => Console.Write(value));
		public void Write(string value) => Lock.Run(() => Console.Write(value));
		public void Write(uint value) => Lock.Run(() => Console.Write(value));
		public void Write(ulong value) => Lock.Run(() => Console.Write(value));
		#endregion Write

		#region Color Write
		public void Write(ConsoleColor color, char[] buffer, int index, int count) => this.LockedWithColor(color, () => Console.Write(buffer, index, count));
		public void Write(ConsoleColor color, string format, object arg0) => this.LockedWithColor(color, () => Console.Write(format, arg0));
		public void Write(ConsoleColor color, string format, object arg0, object arg1) => this.LockedWithColor(color, () => Console.Write(format, arg0, arg1));
		public void Write(ConsoleColor color, string format, object arg0, object arg1, object arg2) => this.LockedWithColor(color, () => Console.Write(format, arg0, arg1, arg2));
		public void Write(ConsoleColor color, string format, params object[] arg) => this.LockedWithColor(color, () => Console.Write(format, arg));
		public void Write(ConsoleColor color, bool value) => this.LockedWithColor(color, () => Console.Write(value));
		public void Write(ConsoleColor color, char value) => this.LockedWithColor(color, () => Console.Write(value));
		public void Write(ConsoleColor color, char[] buffer) => this.LockedWithColor(color, () => Console.Write(buffer));
		public void Write(ConsoleColor color, decimal value) => this.LockedWithColor(color, () => Console.Write(value));
		public void Write(ConsoleColor color, double value) => this.LockedWithColor(color, () => Console.Write(value));
		public void Write(ConsoleColor color, float value) => this.LockedWithColor(color, () => Console.Write(value));
		public void Write(ConsoleColor color, int value) => this.LockedWithColor(color, () => Console.Write(value));
		public void Write(ConsoleColor color, long value) => this.LockedWithColor(color, () => Console.Write(value));
		public void Write(ConsoleColor color, object value) => this.LockedWithColor(color, () => Console.Write(value));
		public void Write(ConsoleColor color, string value) => this.LockedWithColor(color, () => Console.Write(value));
		public void Write(ConsoleColor color, uint value) => this.LockedWithColor(color, () => Console.Write(value));
		public void Write(ConsoleColor color, ulong value) => this.LockedWithColor(color, () => Console.Write(value));
		#endregion Color Write

		#region ReadLine
		public string ReadLine() => Lock.Run(Console.ReadLine);
		public string ReadLine(string message) => this.ReadLine(ConsoleColor.Gray, message);
		public string ReadLine(ConsoleColor color, string message)
		{
			return Lock.Run(() =>
			{
				Console.ForegroundColor = color;
				Console.WriteLine($"{message}: ");
				return Console.ReadLine();
			});
		}
		#endregion ReadLine

		#region ReadKey
		public ConsoleKeyInfo ReadKey() => Lock.Run(Console.ReadKey);
		public ConsoleKeyInfo ReadKey(bool intercept) => Lock.Run(() => Console.ReadKey(intercept));
		public ConsoleKeyInfo ReadKey(bool intercept, TimeSpan timeout)
		{
			Func<bool, ConsoleKeyInfo> operation = Console.ReadKey;
			var invocation = operation.BeginInvoke(intercept, null, null);
			if (!invocation.AsyncWaitHandle.WaitOne(timeout) || !invocation.IsCompleted) throw new TimeoutException();
			return operation.EndInvoke(invocation);
		}

		public bool TryReadKey(bool intercept, TimeSpan timeout, out ConsoleKeyInfo key)
		{
			try
			{
				key = this.ReadKey(intercept, timeout);
				return true;
			}
			catch
			{
				key = default(ConsoleKeyInfo);
				return false;
			}
		}
		#endregion ReadKey

		public static void OpenConsole()
		{
			if (!IsOpen()) AllocConsole();
		}

		public static void CloseConsole() => FreeConsole();
		public static bool IsOpen() => GetConsoleWindow() != IntPtr.Zero;

		private void LockedWithColor(ConsoleColor color, Action action)
		{
			Lock.Run(() =>
			{
				Console.ForegroundColor = color;
				action();
			});
		}
	}
}
