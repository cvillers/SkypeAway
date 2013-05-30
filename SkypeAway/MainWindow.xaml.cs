using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;

using SkypeAway.Native;

namespace SkypeAway
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static RoutedCommand StatusCommand = new RoutedCommand();

		public MainWindow()
		{
			InitializeComponent();
		}

		private static readonly Dictionary<string, int> DownKeyCounts = new Dictionary<string,int>
		{
			{ "Online", 1 },
			{ "Away", 2 },
			{ "DND", 3 },
			{ "Invisible", 4 },
			{ "Offline", 5 },
		};

		private void StatusCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			//MessageBox.Show("Selected " + e.Parameter.ToString());

			IntPtr root = NativeMethods.FindWindow("tSkMainForm", null);
			IntPtr myselfControl = NativeMethods.FindWindowEx(root, IntPtr.Zero, "TMyselfControl", null);

			NativeMethods.PostMessage(myselfControl, NativeConstants.WM_LBUTTONDOWN, (IntPtr)NativeConstants.MK_LBUTTON, NativeMethods.MAKELPARAM(16, 16));
			NativeMethods.PostMessage(myselfControl, NativeConstants.WM_LBUTTONUP, IntPtr.Zero, NativeMethods.MAKELPARAM(16, 16));

			for(int i = 0; i <= DownKeyCounts[e.Parameter.ToString()]; i++)
			{
				NativeMethods.PostMessage(myselfControl, NativeConstants.WM_KEYDOWN, (IntPtr)NativeConstants.VK_DOWN, BuildKeyLParam(NativeConstants.WM_KEYDOWN, 0, 50, true, false));
				NativeMethods.PostMessage(myselfControl, NativeConstants.WM_KEYUP, (IntPtr)NativeConstants.VK_DOWN, BuildKeyLParam(NativeConstants.WM_KEYUP, 0, 50, true, true));
			}

			NativeMethods.PostMessage(myselfControl, NativeConstants.WM_KEYDOWN, (IntPtr)NativeConstants.VK_RETURN, BuildKeyLParam(NativeConstants.WM_KEYDOWN, 0, 0x1c, true, false));
			NativeMethods.PostMessage(myselfControl, NativeConstants.WM_KEYUP, (IntPtr)NativeConstants.VK_RETURN, BuildKeyLParam(NativeConstants.WM_KEYUP, 0, 0x1c, true, true));
		}

		private IntPtr BuildKeyLParam(uint message, int repeat, int scanCode, bool extended, bool previousDown)
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
