using System;
using System.Collections.ObjectModel;

namespace ScoresApp.Extensions
{
	public static class FixturesExtensions
	{
		public static ObservableCollection<ScoresApp.Models.Fixture> ToAppFixtures(this ScoresApp.Service.Responses.Fixtures fixtures)
		{
			var returnData = new ObservableCollection<ScoresApp.Models.Fixture> ();
			if (fixtures.count > 0) {
				foreach (var responsefixture in fixtures.fixtures) {
					var appfixture = new ScoresApp.Models.Fixture {
						Date = responsefixture.date,
						Status = responsefixture.status,
						MatchDay = responsefixture.matchday,
						HomeTeam = new ScoresApp.Models.FixtureTeam{
							TeamName = responsefixture.homeTeamName,
							TeamGoals = (responsefixture.result.goalsHomeTeam.HasValue) ? responsefixture.result.goalsHomeTeam.Value : 0,
							TeamImage = responsefixture.homeTeamImage
						},
						AwayTeam = new ScoresApp.Models.FixtureTeam{
							TeamName = responsefixture.awayTeamName,
							TeamGoals = (responsefixture.result.goalsAwayTeam.HasValue) ? responsefixture.result.goalsAwayTeam.Value : 0,
							TeamImage = responsefixture.awayTeamImage
						}
					};
					returnData.Add (appfixture);
				}
			}
			return returnData;
		}
	}
}

