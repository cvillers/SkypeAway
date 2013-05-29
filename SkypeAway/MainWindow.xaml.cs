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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

		private void StatusCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			MessageBox.Show("Selected " + e.Parameter.ToString());
		}
	}
}
