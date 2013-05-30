using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SkypeAway
{
	/*public class TestCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged;

		public void Execute(object parameter)
		{
			var window = new TestWindow();

			window.Show();
		}
	}*/

	/// <summary>
	/// Interaction logic for TrayWindow.xaml
	/// </summary>
	public partial class TrayWindow : Window
	{
		public TrayWindow()
		{
			InitializeComponent();

			Application.Current.Exit += new ExitEventHandler((sender, ev) =>
				{
					TaskbarIcon.Dispose();
				});
		}

		public static RoutedCommand TestCommand = new RoutedCommand();

		private void TestCommand_Executed(object sender, RoutedEventArgs e)
		{
			var window = new TestWindow();

			window.Show();
		}

		public static RoutedCommand QuitCommand = new RoutedCommand();

		private void QuitCommand_Executed(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}
