using System;
using System.IO;
using SQLite.Net.Interop;
using SQLite.Net;
using ScoresApp.Models;
using System.Linq;
using System.Collections.Generic;
using ScoresApp.ViewModels;

namespace ScoresApp.Database
{
	public class SqlManager
	{

		public static void Init (ISQLitePlatform SQLitePlatform)
		{
			_cache = new SqlManager (SQLitePlatform);
		}

		private SqlManager (ISQLitePlatform SQLitePlatform)
		{
			var DBPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DB_NAME);
			Connection = new SQLiteConnection (SQLitePlatform, DBPath);
			CreateTables ();
		}

		const string DB_NAME = "ScoresAppDatabase.db";

		static SqlManager _cache { get; set; } 

		public static void DeleteSingleton()
		{
			_cache = null;
		}

		public SQLiteConnection Connection { get; private set; }

		void CreateTables()
		{
			if (Connection != null) {
				//Connection.CreateTable<Configuration> ();
				Connection.CreateTable<LeagueItem> ();
			} else {
				throw new FileNotFoundException ("Database File Not Found");
			}
		}

		public List<LeagueItem> Favorites
		{
			get{ 
				return Connection.Table<LeagueItem> ().ToList ();
			}
		}

		public bool IsFavorite(LeagueItem item)
		{
			var source = Favorites.FirstOrDefault( i => i.Id == item.Id);
			return source != null;
		}

		public void AddToFavorite(LeagueItem item)
		{
			if (Favorites.FirstOrDefault (i => i.Id == item.Id) == null)
				Connection.Insert (item);
			FavoritesViewModel.FavoritesViewModelManager.UpdateFavorites ();
		}

		public void RemeveFromFavorite(LeagueItem item)
		{
			if (Favorites.FirstOrDefault (i => i.Id == item.Id) != null)
				Connection.Delete<LeagueItem> (item.Id);
			FavoritesViewModel.FavoritesViewModelManager.UpdateFavorites ();
		}

		public static SqlManager Cache
		{
			get{
				if (_cache == null) {
					throw new NotImplementedException ("You Must Call static method Init(ISQLitePlatform SQLitePlatform) " +
					"before to use database");
				}

				return _cache;
			}
		}
	}
}