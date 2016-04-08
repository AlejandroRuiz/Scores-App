using System;
using Xamarin.Forms;
using ScoresApp.Models;
using ScoresApp.ViewModels;

namespace ScoresApp.Views
{
	public class FixtureCardView : ContentView
	{
		FixtureCardDetailsView	_homeTeamDetails { get; set; }

		FixtureCardDetailsView _awayTeamDetails { get; set; }

		public FixtureCardView (FixtureViewModel fixture)
		{
			_homeTeamDetails = new FixtureCardDetailsView (fixture?.Fixture?.HomeTeam);
			_homeTeamDetails.TeamGoals.BindingContext = fixture;
			_homeTeamDetails.TeamGoals.SetBinding(Label.TextProperty, nameof(FixtureViewModel.HomeTeamGoals));

			_awayTeamDetails = new FixtureCardDetailsView (fixture?.Fixture?.AwayTeam);
			_awayTeamDetails.TeamGoals.BindingContext = fixture;
			_awayTeamDetails.TeamGoals.SetBinding(Label.TextProperty, nameof(FixtureViewModel.AwayTeamGoals));

			Grid grid = new Grid {
				Padding = new Thickness(1,1,2,2),
				RowSpacing = 1,
				ColumnSpacing = 0,		
				BackgroundColor = Color.FromHex ("E3E3E3").MultiplyAlpha(0.5),
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowDefinitions = {
					new RowDefinition { Height = new GridLength (50, GridUnitType.Absolute) },
					new RowDefinition { Height = new GridLength (50, GridUnitType.Absolute) }
				},
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength (4, GridUnitType.Absolute) },
					new ColumnDefinition { Width = new GridLength (96, GridUnitType.Star) }
				}
			};

			grid.Children.Add (
				new FixtureCardStatusView ()
				, 0, 1, 0, 2);

			grid.Children.Add (_homeTeamDetails, 1, 0);

			grid.Children.Add (_awayTeamDetails, 1, 1);

			Content = grid;
		}

		public void SetCardData(FixtureViewModel fixture)
		{
			_homeTeamDetails.TeamGoals.BindingContext = fixture;
			_awayTeamDetails.TeamGoals.BindingContext = fixture;

			_homeTeamDetails.SetDetailViewData(fixture?.Fixture.HomeTeam);
			_awayTeamDetails.SetDetailViewData(fixture?.Fixture.AwayTeam);
		}

	}
}
