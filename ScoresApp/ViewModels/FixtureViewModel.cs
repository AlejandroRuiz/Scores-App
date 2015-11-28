using System;
using ScoresApp.Models;
using Xamarin.Forms;

namespace ScoresApp.ViewModels
{
	public class FixtureViewModel:BaseViewModel
	{
		public Fixture Fixture {
			get;
			set;
		}

		public string HomeTeamName
		{
			get{
				return Fixture.HomeTeam.TeamName;
			}
		}

		public string HomeTeamImage
		{
			get{
				return Fixture.HomeTeam.TeamImage;
			}
		}

		public int HomeTeamGoals
		{
			get{
				return Fixture.HomeTeam.TeamGoals;
			}
			set{
				Fixture.HomeTeam.TeamGoals = value;
				OnPropertyChanged ();
			}
		}

		public string AwayTeamName
		{
			get{
				return Fixture.AwayTeam.TeamName;
			}
		}

		public string AwayTeamImage
		{
			get{
				return Fixture.AwayTeam.TeamImage;
			}
		}

		public int AwayTeamGoals
		{
			get{
				return Fixture.AwayTeam.TeamGoals;
			}
			set{
				Fixture.AwayTeam.TeamGoals = value;
				OnPropertyChanged ();
			}
		}


		public FixtureViewModel (Fixture fixture)
		{
			Fixture = fixture;
		}

		public void UpdateGoals (FixtureViewModel fixture)
		{
			Device.BeginInvokeOnMainThread (() => {
				this.AwayTeamGoals = fixture.AwayTeamGoals;
				this.HomeTeamGoals = fixture.HomeTeamGoals;
			});
		}
	}
}

