#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017 FUJIWARA, Yusuke
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
#endregion -- License Terms --

using System;
using System.Windows;

using NUnitLite.Runner.Silverlight;

namespace MsgPack
{
	public partial class App : Application
	{

		public App()
		{
			this.Startup += this.Application_Startup;
			this.Exit += this.Application_Exit;
			this.UnhandledException += this.Application_UnhandledException;

			this.InitializeComponent();
		}

		private void Application_Startup( object sender, StartupEventArgs e )
		{
			this.RootVisual = new TestPage();
		}

		private void Application_Exit( object sender, EventArgs e )
		{

		}

		private void Application_UnhandledException( object sender, ApplicationUnhandledExceptionEventArgs e )
		{
			if ( !System.Diagnostics.Debugger.IsAttached )
			{

				e.Handled = true;
				Deployment.Current.Dispatcher.BeginInvoke( delegate { ReportErrorToDOM( e ); } );
			}
		}

		private void ReportErrorToDOM( ApplicationUnhandledExceptionEventArgs e )
		{
			try
			{
				string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
				errorMsg = errorMsg.Replace( '"', '\'' ).Replace( "\r\n", @"\n" );

				System.Windows.Browser.HtmlPage.Window.Eval( "throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");" );
			}
			catch ( Exception )
			{
			}
		}
	}
}
