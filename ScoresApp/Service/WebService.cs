using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.ObjectModel;
using ScoresApp.Models;
using System.Net;
using Newtonsoft.Json;
using ScoresApp.Service.Responses;
using System.Collections.Generic;
using System.Linq;
using ScoresApp.Extensions;

namespace ScoresApp.Service
{
	public class WebService
	{
		static WebService _default;
		public static WebService Default {
			get{
				if (_default == null)
					_default = new WebService ();
				return _default;
			}
		}

		HttpClient _webClient;

		public WebService ()
		{
			_webClient = new HttpClient ();
			_webClient.Timeout = TimeSpan.FromSeconds (10);
			_webClient.DefaultRequestHeaders.Add ("X-Auth-Token", "3591f25965d3406ca9f9e8b0e28b95eb");
		}

		public Task<ObservableCollection<Fixture>> GetLeagueMatches(LeagueItem league, CancellationTokenSource token = null)
		{
			if(token == null){
				return Task.Run (() => {
					return GetLeagueMatches(league);
				});
			}

			return Task.Run (() => {
				return GetLeagueMatches(league, token.Token);
			}, token.Token);
		}

		async Task<ObservableCollection<Fixture>> GetLeagueMatches(LeagueItem league, CancellationToken ct)
		{
			try{
				var resultFixtures = await _webClient.GetAsync("http://api.football-data.org/v1/fixtures/?league=" + league.Id, ct);
				if(resultFixtures.StatusCode == HttpStatusCode.OK)
				{
				var responseFixturesText = await resultFixtures.Content.ReadAsStringAsync();
				Fixtures fixtures = JsonConvert.DeserializeObject<Fixtures>(responseFixturesText);

					foreach (var match in fixtures.fixtures) {
						var resulthomeTeam = await _webClient.GetAsync (match._links.homeTeam.href,ct);
						if (resulthomeTeam.StatusCode == HttpStatusCode.OK) {
						var responhomeTeamText = await resulthomeTeam.Content.ReadAsStringAsync();
						var team = JsonConvert.DeserializeObject<ScoresApp.Service.Responses.Team.Team> (responhomeTeamText);
						match.homeTeamImage = team.crestUrl;
						}
						var resultawayTeam = await _webClient.GetAsync (match._links.awayTeam.href,ct);
						if (resultawayTeam.StatusCode == HttpStatusCode.OK) {
						var responawayTeamText = await resultawayTeam.Content.ReadAsStringAsync();
						var team = JsonConvert.DeserializeObject<ScoresApp.Service.Responses.Team.Team> (responawayTeamText);
						match.awayTeamImage = team.crestUrl;
						}
					}
				return fixtures.ToAppFixtures ();
				}
			}catch{
				return new ObservableCollection<Fixture>();
			}
			return new ObservableCollection<Fixture>();
		}
	}
}

