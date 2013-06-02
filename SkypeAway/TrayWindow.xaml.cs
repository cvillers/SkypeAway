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

using log4net;

namespace SkypeAway
{
	/// <summary>
	/// Interaction logic for TrayWindow.xaml
	/// </summary>
	public partial class TrayWindow : Window
	{
		private static ILog log = LogManager.GetLogger(typeof(TrayWindow));

		public TrayWindow()
		{
			log.Debug("Starting up");

			InitializeComponent();

			Application.Current.Exit += new ExitEventHandler((sender, ev) =>
				{
					log.Debug("Shutting down");
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
						log.DebugFormat("ActivateOnLock: Changing status to {0}", Properties.Settings.Default.LockAction);
						Skype.ChangeStatus(Properties.Settings.Default.LockAction);
					}

					break;
				}
				case SessionSwitchReason.SessionUnlock:
				{
					if(Properties.Settings.Default.ActivateOnUnlock)
					{
						log.DebugFormat("ActivateOnUnlock: Changing status to {0}", Properties.Settings.Default.UnlockAction);
						Skype.ChangeStatus(Properties.Settings.Default.UnlockAction);
					}

					break;
				}
				case SessionSwitchReason.RemoteConnect:
				{
					if(Properties.Settings.Default.ActivateOnRdConnect)
					{
						log.DebugFormat("ActivateOnRdConnect: Changing status to {0}", Properties.Settings.Default.RdConnectAction);
						Skype.ChangeStatus(Properties.Settings.Default.RdConnectAction);
					}

					break;
				}
				case SessionSwitchReason.RemoteDisconnect:
				{
					if(Properties.Settings.Default.ActivateOnRdDisconnect)
					{
						log.DebugFormat("ActivateOnRdDisconnect: Changing status to {0}", Properties.Settings.Default.RdDisconnectAction);
						Skype.ChangeStatus(Properties.Settings.Default.RdDisconnectAction);
					}

					break;
				}
			}
		}

		public static RoutedCommand TestCommand = new RoutedCommand();

		private TestWindow testWindow;

		private void TestCommand_Executed(object sender, RoutedEventArgs e)
		{
			if(testWindow == null)
				testWindow = new TestWindow();

			testWindow.Show();
		}

		public static RoutedCommand SettingsCommand = new RoutedCommand();

		private SettingsWindow settingsWindow;

		private void SettingsCommand_Executed(object sender, RoutedEventArgs e)
		{
			if(settingsWindow == null)
				settingsWindow = new SettingsWindow();

			settingsWindow.Show();
		}

		public static RoutedCommand QuitCommand = new RoutedCommand();

		private void QuitCommand_Executed(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}
