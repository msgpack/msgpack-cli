using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MsgPack.UnitTest.Xamios
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main( string[] args )
		{
			EnsureLinked();
			// if you want to use a different Application Delegate class from "UnitTestAppDelegate"
			// you can specify it here.
			UIApplication.Main( args, null, "UnitTestAppDelegate" );
		}

		static void EnsureLinked()
		{
			var result = new List<object>();
			result.Add( Tuple.Create( 1 ).Item1 );
			result.Add( Tuple.Create( 1, 2 ).Item1 );
			result.Add( Tuple.Create( 1, 2 ).Item2 );
			result.Add( Tuple.Create( 1, 2, 3 ).Item1 );
			result.Add( Tuple.Create( 1, 2, 3 ).Item2 );
			result.Add( Tuple.Create( 1, 2, 3 ).Item3 );
			result.Add( Tuple.Create( 1, 2, 3, 4 ).Item1 );
			result.Add( Tuple.Create( 1, 2, 3, 4 ).Item2 );
			result.Add( Tuple.Create( 1, 2, 3, 4 ).Item3 );
			result.Add( Tuple.Create( 1, 2, 3, 4 ).Item4 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5 ).Item1 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5 ).Item2 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5 ).Item3 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5 ).Item4 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5 ).Item5 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6 ).Item1 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6 ).Item2 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6 ).Item3 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6 ).Item4 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6 ).Item5 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6 ).Item6 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6, 7 ).Item1 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6, 7 ).Item2 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6, 7 ).Item3 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6, 7 ).Item4 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6, 7 ).Item5 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6, 7 ).Item6 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6, 7 ).Item7 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6, 7, 8 ).Item1 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6, 7, 8 ).Item2 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6, 7, 8 ).Item3 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6, 7, 8 ).Item4 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6, 7, 8 ).Item5 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6, 7, 8 ).Item6 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6, 7, 8 ).Item7 );
			result.Add( Tuple.Create( 1, 2, 3, 4, 5, 6, 7, 8 ).Rest.Item1 );
		}
	}
}
