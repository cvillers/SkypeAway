using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using log4net;

using SkypeAway.Native;

namespace SkypeAway
{
	public enum Status
	{
		Online,
		Away,
		DND,
		Invisible,
		Offline
	}

	public static class Skype
	{
		private static ILog log = LogManager.GetLogger(typeof(Skype));

		private static readonly Dictionary<Status, int> DownKeyCounts = new Dictionary<Status, int>
		{
			{ Status.Online, 1 },
			{ Status.Away, 2 },
			{ Status.DND, 3 },
			{ Status.Invisible, 4 },
			{ Status.Offline, 5 },
		};

		private static IntPtr myselfControl = IntPtr.Zero;

		public static void ChangeStatus(Status status)
		{
			IntPtr root = NativeMethods.FindWindow("tSkMainForm", null);

			if(root == IntPtr.Zero || root == null)
			{
				log.ErrorFormat("Skype main form not found, is Skype running? Error code: 0x{0:X}", Marshal.GetLastWin32Error());
				return;
			}

			IntPtr myselfControl = NativeMethods.FindWindowEx(root, IntPtr.Zero, "TMyselfControl", null);

			if(myselfControl == IntPtr.Zero || myselfControl == null)
			{
				log.ErrorFormat("Skype self control not found, is Skype the right version and responsive? Error code: 0x{0:X}", Marshal.GetLastWin32Error());
				return;
			}

			PostMyselfMessage(NativeConstants.WM_LBUTTONDOWN, (IntPtr)NativeConstants.MK_LBUTTON, NativeMethods.MAKELPARAM(16, 16));
			PostMyselfMessage(NativeConstants.WM_LBUTTONUP, IntPtr.Zero, NativeMethods.MAKELPARAM(16, 16));

			Thread.Sleep(50);

			for(int i = 1; i <= DownKeyCounts[status]; i++)
			{
				PostMyselfMessage(NativeConstants.WM_KEYDOWN, (IntPtr)NativeConstants.VK_DOWN, BuildKeyLParam(NativeConstants.WM_KEYDOWN, 0, 50, true, false));
				Thread.Sleep(50);
				PostMyselfMessage(NativeConstants.WM_KEYUP, (IntPtr)NativeConstants.VK_DOWN, BuildKeyLParam(NativeConstants.WM_KEYUP, 0, 50, true, true));
			}

			Thread.Sleep(50);

			PostMyselfMessage(NativeConstants.WM_KEYDOWN, (IntPtr)NativeConstants.VK_RETURN, BuildKeyLParam(NativeConstants.WM_KEYDOWN, 0, 0x1c, true, false));
			PostMyselfMessage(NativeConstants.WM_KEYUP, (IntPtr)NativeConstants.VK_RETURN, BuildKeyLParam(NativeConstants.WM_KEYUP, 0, 0x1c, true, true));
		}

		/// <summary>
		/// Wrapper for <see cref="M:SkypeAway.Native.NativeMethods.PostMessage"/> which checks errors. Posts to the TMyselfControl stored in <see cref="F:SkypeAway.Skype.myselfControl"/>.
		/// </summary>
		/// <param name="msg">The message to send.</param>
		/// <param name="wParam">The wParam of the message.</param>
		/// <param name="lParam">The lParam of the message.</param>
		/// <returns></returns>
		private static bool PostMyselfMessage(uint msg, IntPtr wParam, IntPtr lParam)
		{
			bool result = NativeMethods.PostMessage(myselfControl, msg, wParam, lParam);

			if(!result)
			{
				log.ErrorFormat("Sending message to Skype self control failed. Error code: 0x{0:X}", Marshal.GetLastWin32Error());
				return false;
			}

			return true;
		}

		private static IntPtr BuildKeyLParam(uint message, int repeat, int scanCode, bool extended, bool previousDown)
		{
			BitVector32 vec = new BitVector32(0);

			BitVector32.Section sec1 = BitVector32.CreateSection(15);
			BitVector32.Section sec2 = BitVector32.CreateSection(8);
			BitVector32.Section sec3 = BitVector32.CreateSection(1);
			BitVector32.Section sec4 = BitVector32.CreateSection(4);
			BitVector32.Section sec5 = BitVector32.CreateSection(1);
			BitVector32.Section sec6 = BitVector32.CreateSection(1);
			BitVector32.Section sec7 = BitVector32.CreateSection(1);

			vec[sec1] = repeat;
			vec[sec2] = scanCode;
			vec[sec3] = extended ? 1 : 0;
			vec[sec4] = 0;
			vec[sec5] = 0;
			vec[sec6] = message == NativeConstants.WM_KEYUP ? 1 : previousDown ? 1 : 0;
			vec[sec7] = message == NativeConstants.WM_KEYDOWN ? 0 : 1;

			return (IntPtr)vec.Data;
		}
	}
}
