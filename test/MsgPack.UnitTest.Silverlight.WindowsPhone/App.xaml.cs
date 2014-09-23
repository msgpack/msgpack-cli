using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MsgPack.Resources;

namespace MsgPack
{
	public partial class App : Application
	{
		public static PhoneApplicationFrame RootFrame { get; private set; }

		public App()
		{
			UnhandledException += Application_UnhandledException;
			InitializeComponent();
			InitializePhoneApplication();
			InitializeLanguage();

			if ( Thread.CurrentThread.CurrentUICulture.Name != "en-US" )
			{
				// It's cool.
				throw new NotSupportedException( "MSTest cannot run under non-en-US cultures. You must change display locale from Settings page and restart the Emulator. Current locale is :" + Thread.CurrentThread.CurrentUICulture.Name );
			}

			if ( Debugger.IsAttached )
			{
				Application.Current.Host.Settings.EnableFrameRateCounter = true;
				PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
			}

		}
		private void Application_Launching( object sender, LaunchingEventArgs e )
		{
		}
		private void Application_Activated( object sender, ActivatedEventArgs e )
		{
		}
		private void Application_Deactivated( object sender, DeactivatedEventArgs e )
		{
		}
		private void Application_Closing( object sender, ClosingEventArgs e )
		{
		}
		private void RootFrame_NavigationFailed( object sender, NavigationFailedEventArgs e )
		{
			if ( Debugger.IsAttached )
			{
				Debugger.Break();
			}
		}
		private void Application_UnhandledException( object sender, ApplicationUnhandledExceptionEventArgs e )
		{
			if ( Debugger.IsAttached )
			{
				Debug.WriteLine( e.ExceptionObject );
				Debugger.Break();
			}
		}

		#region Phone application initialization
		private bool phoneApplicationInitialized = false;
		private void InitializePhoneApplication()
		{
			if ( phoneApplicationInitialized )
				return;
			RootFrame = new PhoneApplicationFrame();
			RootFrame.Navigated += CompleteInitializePhoneApplication;
			RootFrame.NavigationFailed += RootFrame_NavigationFailed;
			RootFrame.Navigated += CheckForResetNavigation;
			phoneApplicationInitialized = true;
		}
		private void CompleteInitializePhoneApplication( object sender, NavigationEventArgs e )
		{
			if ( RootVisual != RootFrame )
				RootVisual = RootFrame;
			RootFrame.Navigated -= CompleteInitializePhoneApplication;
		}

		private void CheckForResetNavigation( object sender, NavigationEventArgs e )
		{
			if ( e.NavigationMode == NavigationMode.Reset )
				RootFrame.Navigated += ClearBackStackAfterReset;
		}

		private void ClearBackStackAfterReset( object sender, NavigationEventArgs e )
		{
			RootFrame.Navigated -= ClearBackStackAfterReset;
			if ( e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh )
				return;
			while ( RootFrame.RemoveBackEntry() != null )
			{
				;
			}
		}

		#endregion
		private void InitializeLanguage()
		{
			try
			{
				RootFrame.Language = XmlLanguage.GetLanguage( AppResources.ResourceLanguage );
				FlowDirection flow = ( FlowDirection )Enum.Parse( typeof( FlowDirection ), AppResources.ResourceFlowDirection );
				RootFrame.FlowDirection = flow;
			}
			catch
			{

				if ( Debugger.IsAttached )
				{
					Debugger.Break();
				}

				throw;
			}
		}
	}
}