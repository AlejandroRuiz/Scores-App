using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace ScoresApp.UITests
{
	[TestFixture (Platform.Android)]
	[TestFixture (Platform.iOS)]
	public class Tests
	{
		IApp app;
		Platform platform;

		public Tests (Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
		public void BeforeEachTest ()
		{
			app = AppInitializer.StartApp (platform);
		}

		[Test]
		public void WelcomeTextIsDisplayed ()
		{
			AppResult[] results = app.WaitForElement (c => c.Marked ("Welcome to Xamarin Forms!"));
			app.Screenshot ("Welcome screen.");

			Assert.IsTrue (results.Any ());
		}

		[Test]
		public void BasicTest ()
		{
			app.Tap(x => x.Class("ImageButton").Marked("OK"));
			app.Screenshot("Tapped on view ImageButton");
			app.Tap(x => x.Class("TextView").Text("Bundesliga"));
			app.Screenshot("Tapped on view TextView with Text: 'Bundesliga'");
			app.Tap(x => x.Class("FloatingActionButton"));
			app.Screenshot("Tapped on view FloatingActionButton");
			app.Tap(x => x.Class("AppCompatButton").Id("button1").Text("Ok"));
			app.Screenshot("Tapped on view AppCompatButton with ID: 'button1' with Text: 'Ok'");
			app.Tap(x => x.Class("ImageButton").Marked("OK"));
			app.Screenshot("Tapped on view ImageButton");
			app.Tap(x => x.Class("TextView").Text("Favorites"));
			app.Screenshot("Tapped on view TextView with Text: 'Favorites'");
			app.Tap(x => x.Class("AppCompatButton"));
			app.Screenshot("Tapped on view AppCompatButton");
			app.Tap(x => x.Class("FloatingActionButton"));
			app.Screenshot("Tapped on view FloatingActionButton");
			app.Tap(x => x.Class("AppCompatButton").Id("button1").Text("Ok"));
			app.Screenshot("Tapped on view AppCompatButton with ID: 'button1' with Text: 'Ok'");
			app.ScrollUp();
			app.Tap(x => x.Class("ImageButton").Index(1));
			app.Screenshot("Tapped on view ImageButton");
		}
	}
}

