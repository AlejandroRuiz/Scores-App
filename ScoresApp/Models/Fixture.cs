using System;

namespace ScoresApp.Models
{
	public class Fixture
	{
		public string Date { get; set; }
		public string Status { get; set; }
		public int MatchDay { get; set; }

		public FixtureTeam HomeTeam { get; set; }
		public FixtureTeam AwayTeam { get; set; }
	}
}

