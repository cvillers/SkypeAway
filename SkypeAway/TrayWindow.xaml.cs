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

using Microsoft.Win32;

namespace SkypeAway
{
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

			SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
		}

		/// <summary>
		/// Handler for the session status switch event.
		/// </summary>
		/// <param name="sender">Sending object.</param>
		/// <param name="e">The event.</param>
		void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
		{
			switch(e.Reason)
			{
				case SessionSwitchReason.SessionLock:
				{
					if(Properties.Settings.Default.ActivateOnLock)
					{
						Status newStatus = (Status)Enum.Parse(typeof(Status), Properties.Settings.Default.OnLockAction);

						Skype.ChangeStatus(newStatus);
					}

					break;
				}
				case SessionSwitchReason.SessionUnlock:
				{
					if(Properties.Settings.Default.ActivateOnUnlock)
					{
						Status newStatus = (Status)Enum.Parse(typeof(Status), Properties.Settings.Default.OnUnlockAction);

						Skype.ChangeStatus(newStatus);
					}

					break;
				}
				case SessionSwitchReason.RemoteConnect:
				{
					if(Properties.Settings.Default.ActivateOnRdConnect)
					{
						Status newStatus = (Status)Enum.Parse(typeof(Status), Properties.Settings.Default.OnUnlockAction);

						Skype.ChangeStatus(newStatus);
					}

					break;
				}
				case SessionSwitchReason.RemoteDisconnect:
				{
					if(Properties.Settings.Default.ActivateOnRdDisconnect)
					{
						Status newStatus = (Status)Enum.Parse(typeof(Status), Properties.Settings.Default.OnLockAction);

						Skype.ChangeStatus(newStatus);
					}

					break;
				}
			}
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
