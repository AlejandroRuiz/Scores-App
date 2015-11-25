using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using ScoresApp.Database;
using SQLite.Net.Platform.XamarinAndroid;

namespace ScoresApp.Droid
{
	[Activity (Label = "Scores App",
		Icon = "@drawable/ic_launcher",
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		ScreenOrientation = ScreenOrientation.Portrait
	)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

			FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
			FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;

			LoadApplication (new App ());
		}
	}
}

