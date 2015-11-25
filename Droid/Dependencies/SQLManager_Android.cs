using System;
using ScoresApp.Dependencies;
using SQLite.Net.Platform.XamarinAndroid;
using ScoresApp.Droid.Dependencies;

[assembly: Xamarin.Forms.Dependency (typeof (SQLManager_Android))]
namespace ScoresApp.Droid.Dependencies
{
	public class SQLManager_Android:ISQLManager
	{
		#region ISQLManager implementation

		public SQLite.Net.Interop.ISQLitePlatform NativeManager {
			get {
				return new SQLitePlatformAndroid ();
			}
		}

		#endregion

		public SQLManager_Android ()
		{
		}
	}
}

