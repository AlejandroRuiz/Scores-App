using System;

namespace ScoresApp.Defaults
{
	public static class ServerStrings
	{
		public static string ApiKey
		{
			get{
				return "3591f25965d3406ca9f9e8b0e28b95eb";
			}
		}

		public static string BaseUrl
		{
			get{
				return "http://api.football-data.org/v1";
			}
		}

		public static string FixturesPath
		{
			get{
				return "/fixtures";
			}
		}
	}

	public static class Strings
	{
		public static string AppName
		{
			get{
				return "Scores App";
			}
		}

		public static string AppLogoIcon
		{
			get{
				return "ic_blur_circular_48pt";
			}
		}

		public static string LeagueText
		{
			get{
				return "Leagues";
			}
		}

		public static string NoFavoritesText
		{
			get{
				return "No Favorites Yet";
			}
		}

		public static string HeartIcon
		{
			get{
				return "ic_favorite_48pt";
			}
		}
	}

	public static class Integers
	{
		public static int AnimationSpeed {
			get {
				return 500;
			}
		}
	}

	public static class InsightsConstants
	{
		public static string LeagueResultsPage
		{
			get{
				return "League Results Page";
			}
		}

		public static string MainMasterDetailPage
		{
			get{
				return "Main Master Detail Page";
			}
		}

		public static string MenuPage
		{
			get{
				return "Menu Page";
			}
		}

		public static string WelcomePage
		{
			get{
				return "Welcome Page";
			}
		}
	}
}

