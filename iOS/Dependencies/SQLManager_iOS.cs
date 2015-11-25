using System;
using ScoresApp.Dependencies;
using SQLite.Net.Platform.XamarinIOS;
using ScoresApp.iOS.Dependencies;

[assembly: Xamarin.Forms.Dependency (typeof (SQLManager_iOS))]
namespace ScoresApp.iOS.Dependencies
{
	public class SQLManager_iOS:ISQLManager
	{
		#region ISQLManager implementation

		public SQLite.Net.Interop.ISQLitePlatform NativeManager {
			get {
				return new SQLitePlatformIOS ();
			}
		}

		#endregion

		public SQLManager_iOS ()
		{
		}
	}
}

