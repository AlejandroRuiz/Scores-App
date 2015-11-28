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
using ScoresApp.Defaults;
using ScoresApp.ViewModels;

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
			_webClient.Timeout = TimeSpan.FromSeconds (30);
			_webClient.DefaultRequestHeaders.Add ("X-Auth-Token", ServerStrings.ApiKey);
		}

		public Task<ObservableCollection<FixtureViewModel>> GetLeagueMatches(string leagueId, bool requestDetailInfo = true, CancellationTokenSource token = null)
		{
			if(token == null){
				return Task.Run (() => {
					return GetLeagueMatches(leagueId);
				});
			}

			return Task.Run (() => {
				return GetLeagueMatches(leagueId, requestDetailInfo, token.Token);
			}, token.Token);
		}

		async Task<ObservableCollection<FixtureViewModel>> GetLeagueMatches(string leagueId, bool requestDetailInfo, CancellationToken ct)
		{
			try{
				var resultFixtures = await _webClient.GetAsync(ServerStrings.BaseUrl+ServerStrings.FixturesPath+"/?league=" + leagueId, ct);
				if(resultFixtures.StatusCode == HttpStatusCode.OK)
				{
					var responseFixturesText = await resultFixtures.Content.ReadAsStringAsync();
					Fixtures fixtures = JsonConvert.DeserializeObject<Fixtures>(responseFixturesText);
					if(requestDetailInfo){
						foreach (var match in fixtures.fixtures) {
							var resulthomeTeam = await _webClient.GetAsync (match._links.homeTeam.href,ct);
							if (resulthomeTeam.StatusCode == HttpStatusCode.OK) {
								var responhomeTeamText = await resulthomeTeam.Content.ReadAsStringAsync();
								var team = JsonConvert.DeserializeObject<ScoresApp.Service.Responses.Team.Team> (responhomeTeamText);
								match.homeTeamImage = team.crestUrl;
								match.homeShortTeamName = team.shortName;
							}
							var resultawayTeam = await _webClient.GetAsync (match._links.awayTeam.href,ct);
							if (resultawayTeam.StatusCode == HttpStatusCode.OK) {
								var responawayTeamText = await resultawayTeam.Content.ReadAsStringAsync();
								var team = JsonConvert.DeserializeObject<ScoresApp.Service.Responses.Team.Team> (responawayTeamText);
								match.awayTeamImage = team.crestUrl;
								match.awayShortTeamName = team.shortName;
							}
						}
					}
				return fixtures.ToAppFixtures ();
				}
			}catch(Exception ex){
				Xamarin.Insights.Report (ex);
				return new ObservableCollection<FixtureViewModel>();
			}
			return new ObservableCollection<FixtureViewModel>();
		}

		public Task<Fixture> GetFixtureData(string fixtureID,  bool requestDetailInfo = true, CancellationTokenSource token = null)
		{
			if(token == null){
				return Task.Run (() => {
					return GetFixtureData(fixtureID);
				});
			}

			return Task.Run (() => {
				return GetFixtureData(fixtureID, requestDetailInfo, token.Token);
			}, token.Token);
		}

		async Task<Fixture> GetFixtureData (string fixtureID, bool requestDetailInfo, CancellationToken ct)
		{
			try{
				var fixtureResult = await _webClient.GetAsync(fixtureID);
				if(fixtureResult.StatusCode == HttpStatusCode.OK)
				{
					var responseFixtureText = await fixtureResult.Content.ReadAsStringAsync();
					SingleFixture fixture = JsonConvert.DeserializeObject<SingleFixture>(responseFixtureText);

					if(requestDetailInfo){
					var resulthomeTeam = await _webClient.GetAsync (fixture.fixture._links.homeTeam.href,ct);
					if (resulthomeTeam.StatusCode == HttpStatusCode.OK) {
						var respondhomeTeam = await resulthomeTeam.Content.ReadAsStringAsync();
						var team = JsonConvert.DeserializeObject<ScoresApp.Service.Responses.Team.Team> (respondhomeTeam);

						var resulthomeTeamPlayers = await _webClient.GetAsync (team._links.players.href, ct);
						if (resulthomeTeamPlayers.StatusCode == HttpStatusCode.OK) {
							var respondhomeTeamPlayers = await resulthomeTeamPlayers.Content.ReadAsStringAsync();
							var players = JsonConvert.DeserializeObject<ScoresApp.Service.Responses.Players> (respondhomeTeamPlayers);
								fixture.homePlayers = players.players.OrderBy(i => i.jerseyNumber).ToList();
						}
					}

					var resultawayTeam = await _webClient.GetAsync (fixture.fixture._links.awayTeam.href,ct);
					if (resultawayTeam.StatusCode == HttpStatusCode.OK) {
						var respondawayTeam = await resultawayTeam.Content.ReadAsStringAsync();
						var team = JsonConvert.DeserializeObject<ScoresApp.Service.Responses.Team.Team> (respondawayTeam);

						var resultawayTeamPlayers = await _webClient.GetAsync (team._links.players.href, ct);
						if (resultawayTeamPlayers.StatusCode == HttpStatusCode.OK) {
							var respondawayTeamPlayers = await resultawayTeamPlayers.Content.ReadAsStringAsync();
							var players = JsonConvert.DeserializeObject<ScoresApp.Service.Responses.Players> (respondawayTeamPlayers);
								fixture.awayPlayers = players.players.OrderBy(i => i.jerseyNumber).ToList();
							}
						}
					}

					return fixture.ToAppFixture();
				}
			}catch(Exception ex){
				Xamarin.Insights.Report (ex);
				return default(Fixture);
			}
			return default(Fixture);
		}
	}
}

