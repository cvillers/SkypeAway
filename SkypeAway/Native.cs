using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SkypeAway.Native
{
	/// <summary>
	/// Contains constants used for function definitions in <see cref="T:SkypeAway.Native.NativeMethods" />.
	/// </summary>
	public static class NativeConstants
	{
		public static readonly uint MK_LBUTTON = 0x0001;

		public static readonly uint VK_DOWN = 0x28;
		public static readonly uint VK_RETURN = 0x0D;

		public static readonly uint WM_KEYDOWN = 0x0100;
		public static readonly uint WM_KEYUP = 0x0101;

		public static readonly uint WM_LBUTTONDOWN = 0x0201;
		public static readonly uint WM_LBUTTONUP = 0x0202;
	}

	/// <summary>
	/// Contains P/Invoke function definitions.
	/// </summary>
	public static class NativeMethods
	{
		public static IntPtr MAKELPARAM(Int16 wLow, Int16 wHigh)
		{
			return (IntPtr)((wLow << 16) | (wHigh & 0xffff));
		}

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindow(
			[MarshalAs(UnmanagedType.LPTStr)] 
			string className, 
			[MarshalAs(UnmanagedType.LPTStr)] 
			string windowTitle);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, 
			[MarshalAs(UnmanagedType.LPTStr)] 
			string className, 
			[MarshalAs(UnmanagedType.LPTStr)] 
			string windowTitle);

		// TODO is IntPtr the right size?
		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
	}
}
