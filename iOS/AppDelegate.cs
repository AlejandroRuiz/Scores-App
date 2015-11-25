using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using ScoresApp.Database;
using SQLite.Net.Platform.XamarinIOS;
using Refractored.XamForms.PullToRefresh.iOS;

namespace ScoresApp.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

			// Code for starting up the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
			#endif

			SqlManager.Init (new SQLitePlatformIOS());

			PullToRefreshLayoutRenderer.Init ();

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

