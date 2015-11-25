using System;

using Xamarin.Forms;
using ScoresApp.Pages;
using ScoresApp.Database;
using ScoresApp.Dependencies;

namespace ScoresApp
{
	public class App : Application
	{
		
		public static App CurrentApp {
			get;
			private set;
		}

		public MainMasterDetailPage CurrentMasterDetailPage {
			get{
				return MainPage as MainMasterDetailPage;
			}
		}

		public App ()
		{
			// The root page of your application
			MainPage = new MainMasterDetailPage();
			CurrentApp = this;
			SqlManager.Init (DependencyService.Get<ISQLManager>().NativeManager);
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

