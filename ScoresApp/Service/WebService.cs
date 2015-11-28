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
							}
							var resultawayTeam = await _webClient.GetAsync (match._links.awayTeam.href,ct);
							if (resultawayTeam.StatusCode == HttpStatusCode.OK) {
								var responawayTeamText = await resultawayTeam.Content.ReadAsStringAsync();
								var team = JsonConvert.DeserializeObject<ScoresApp.Service.Responses.Team.Team> (responawayTeamText);
								match.awayTeamImage = team.crestUrl;
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

		async Task<Fixture> GetFixtureData (string fixtureID, CancellationToken ct)
		{
			try{
				var fixtureResult = await _webClient.GetAsync(ServerStrings.BaseUrl+ServerStrings.FixturesPath+"/"+fixtureID);
				if(fixtureResult.StatusCode == HttpStatusCode.OK)
				{
					var responseFixtureText = await fixtureResult.Content.ReadAsStringAsync();
					SingleFixture fixture = JsonConvert.DeserializeObject<SingleFixture>(responseFixtureText);
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

