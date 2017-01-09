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

			InitializeComponent();
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
			// アプリケーションがデバッガーの外側で実行されている場合、ブラウザーの
			// 例外メカニズムによって例外が報告されます。これにより、IE ではステータス バーに
			// 黄色の通知アイコンが表示され、Firefox にはスクリプト エラーが表示されます。
			if ( !System.Diagnostics.Debugger.IsAttached )
			{

				// メモ : これにより、アプリケーションは例外がスローされた後も実行され続け、例外は
				// ハンドルされません。
				// 実稼動アプリケーションでは、このエラー処理は、Web サイトにエラーを報告し、
				// アプリケーションを停止させるものに置換される必要があります。
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
