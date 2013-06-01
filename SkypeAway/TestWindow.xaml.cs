using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
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
	public partial class TestWindow : Window
	{
		public static RoutedCommand StatusCommand = new RoutedCommand();

		public TestWindow()
		{
			InitializeComponent();
		}

		public void StatusCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Status newStatus = (Status)Enum.Parse(typeof(Status), e.Parameter.ToString());

			Skype.ChangeStatus(newStatus);
		}
	}
}
