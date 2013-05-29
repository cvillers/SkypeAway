using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SkypeAway.NativeMethods
{
	/// <summary>
	/// Contains P/Invoke function definitions.
	/// </summary>
	public static class NativeMethods
	{
		[DllImport("user32.dll", SetLastError = true)]
		static extern IntPtr FindWindow(string className, string windowTitle);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
	}
}
