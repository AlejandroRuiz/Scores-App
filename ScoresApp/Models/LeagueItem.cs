using System;
using Xamarin.Forms;
using SQLite.Net.Attributes;
using ScoresApp.Database;

namespace ScoresApp.Models
{
	public class LeagueItem
	{
		static LeagueItem _favorites;
		[Ignore]
		public static LeagueItem Favorites
		{
			get {
				if (_favorites == null)
				{
					_favorites = new LeagueItem {
						Text = "Favorites"
					};
				}
				return _favorites;
			}
		}

		static LeagueItem _bundesliga;
		[Ignore]
		public static LeagueItem Bundesliga
		{
			get {
				if (_bundesliga == null)
				{
					_bundesliga = new LeagueItem {
						Text = "Bundesliga",
						Description = "Germany",
						Id = "BL1",
						Image = "http://www.sciencekids.co.nz/images/pictures/flags680/Germany.jpg"
					};
				}
				return _bundesliga;
			}
		}

		static LeagueItem _premiereLeague;
		[Ignore]
		public static LeagueItem PremiereLeague
		{
			get {
				if (_premiereLeague == null)
				{
					_premiereLeague = new LeagueItem {
						Text = "Premiere League",
						Description = "England",
						Id = "PL",
						Image = "http://www.sciencekids.co.nz/images/pictures/flags680/England.jpg"
					};
				}
				return _premiereLeague;
			}
		}

		static LeagueItem _serieA;
		[Ignore]
		public static LeagueItem SerieA
		{
			get {
				if (_serieA == null)
				{
					_serieA = new LeagueItem{
						Text = "Serie A",
						Description = "Italy",
						Id = "SA",
						Image = "http://www.sciencekids.co.nz/images/pictures/flags680/Italy.jpg"
					};
				}
				return _serieA;
			}
		}

		static LeagueItem _primeraDivision;
		[Ignore]
		public static LeagueItem PrimeraDivision
		{
			get {
				if (_primeraDivision == null)
				{
					_primeraDivision = new LeagueItem{
						Text = "Primera Division",
						Description = "Spain",
						Id = "PD",
						Image = "http://www.sciencekids.co.nz/images/pictures/flags680/Spain.jpg"
					};
				}
				return _primeraDivision;
			}
		}

		static LeagueItem _ligue1;
		[Ignore]
		public static LeagueItem Ligue1
		{
			get {
				if (_ligue1 == null)
				{
					_ligue1 = new LeagueItem{
						Text = "Ligue 1",
						Description = "France",
						Id = "FL1",
						Image = "http://www.sciencekids.co.nz/images/pictures/flags680/France.jpg"
					};
				}
				return _ligue1;
			}
		}

		static LeagueItem _eredivisie;
		[Ignore]
		public static LeagueItem Eredivisie
		{
			get {
				if (_eredivisie == null)
				{
					_eredivisie = new LeagueItem{
						Text = "Eredivisie",
						Description = "Netherlands",
						Id = "DED",
						Image = "http://www.sciencekids.co.nz/images/pictures/flags680/Netherlands.jpg"
					};
				}
				return _eredivisie;
			}
		}
		
		public string Text { get; set; }

		public string Description { get; set; }

		public string Image { get; set; }

		[Ignore]
		public Color TextColor {
			get{
				return Color.Black;
			}
		}

		[Ignore]
		public Color DescriptionColor {
			get{
				return Color.FromHex("546792");
			}
		}

		[Ignore]
		public bool IsFavorite {
			get{
				return SqlManager.Cache.IsFavorite (this);
			}
		}

		[PrimaryKey, Unique]
		public string Id { get; set; }

		public LeagueItem ()
		{
		}
	}
}

