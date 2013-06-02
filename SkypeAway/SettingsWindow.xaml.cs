using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
	class SettingsWindowViewModel : INotifyPropertyChanged
	{
		public IEnumerable<Status> Actions
		{
			get
			{
				return Enum.GetValues(typeof(Status)).Cast<Status>();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void NotifyPropertyChanged(string property)
		{
			if(PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}
	}

	/// <summary>
	/// Interaction logic for SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow : Window
	{
		public SettingsWindow()
		{
			InitializeComponent();

			DataContext = new SettingsWindowViewModel();
		}

		public static RoutedCommand OkCommand = new RoutedCommand();

		private void OkCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Properties.Settings.Default.Save();
			this.Hide();
		}

		public static RoutedCommand CancelCommand = new RoutedCommand();

		private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Properties.Settings.Default.Reload();
			this.Hide();
		}

	}
}
