using System;
using System.Collections.ObjectModel;

namespace ScoresApp.Extensions
{
	public static class FixturesExtensions
	{
		public static ObservableCollection<ScoresApp.ViewModels.FixtureViewModel> ToAppFixtures(this ScoresApp.Service.Responses.Fixtures fixtures)
		{
			var returnData = new ObservableCollection<ScoresApp.ViewModels.FixtureViewModel> ();
			if (fixtures.count > 0) {
				foreach (var responsefixture in fixtures.fixtures) {
					var appfixture = new ScoresApp.Models.Fixture {
						Id = responsefixture._links.self.href,
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

					returnData.Add (new ScoresApp.ViewModels.FixtureViewModel (appfixture));
				}
			}
			return returnData;
		}

		public static ScoresApp.Models.Fixture ToAppFixture(this ScoresApp.Service.Responses.SingleFixture fixture)
		{
			return new ScoresApp.Models.Fixture {
				Date = fixture.fixture.date,
				Status = fixture.fixture.status,
				MatchDay = fixture.fixture.matchday,
				HomeTeam = new ScoresApp.Models.FixtureTeam{
					TeamName = fixture.fixture.homeTeamName,
					TeamGoals = (fixture.fixture.result.goalsHomeTeam.HasValue) ? fixture.fixture.result.goalsHomeTeam.Value : 0,
					TeamImage = fixture.fixture.homeTeamImage
				},
				AwayTeam = new ScoresApp.Models.FixtureTeam{
					TeamName = fixture.fixture.awayTeamName,
					TeamGoals = (fixture.fixture.result.goalsAwayTeam.HasValue) ? fixture.fixture.result.goalsAwayTeam.Value : 0,
					TeamImage = fixture.fixture.awayTeamImage
				}
			};
		}
	}
}

