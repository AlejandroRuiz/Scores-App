using System;
using System.Collections.Generic;

namespace ScoresApp.Service.Responses
{
	public class Fixtures
	{
		public string timeFrameStart { get; set; }
		public string timeFrameEnd { get; set; }
		public int count { get; set; }
		public List<Fixture> fixtures { get; set; }

		public Fixtures ()
		{
			fixtures = new List<Fixture> ();
		}

		public class Self
		{
			public string href { get; set; }
		}

		public class Soccerseason
		{
			public string href { get; set; }
		}

		public class HomeTeam
		{
			public string href { get; set; }
		}

		public class AwayTeam
		{
			public string href { get; set; }
		}

		public class Links
		{
			public Self self { get; set; }
			public Soccerseason soccerseason { get; set; }
			public HomeTeam homeTeam { get; set; }
			public AwayTeam awayTeam { get; set; }
		}

		public class Result
		{
			public int? goalsHomeTeam { get; set; }
			public int? goalsAwayTeam { get; set; }
		}

		public class Fixture
		{
			public Links _links { get; set; }
			public string date { get; set; }
			public string status { get; set; }
			public int matchday { get; set; }
			public string homeTeamName { get; set; }
			public string awayTeamName { get; set; }
			public string homeTeamImage { get; set; }
			public string awayTeamImage { get; set; }
			public string homeShortTeamName { get; set; }
			public string awayShortTeamName { get; set; }
			public Result result { get; set; }
		}
	}
}

