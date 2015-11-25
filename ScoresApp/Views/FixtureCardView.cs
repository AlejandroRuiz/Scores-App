using System;
using Xamarin.Forms;
using ScoresApp.Models;

namespace ScoresApp.Views
{
	public class FixtureCardView : ContentView
	{
		FixtureCardDetailsView	_homeTeamDetails { get; set; }

		FixtureCardDetailsView _awayTeamDetails { get; set; }

		public FixtureCardView (Fixture fixture)
		{
			_homeTeamDetails = new FixtureCardDetailsView (fixture?.HomeTeam);
			_awayTeamDetails = new FixtureCardDetailsView (fixture?.AwayTeam);

			Grid grid = new Grid {
				Padding = new Thickness(0,1,1,1),
				RowSpacing = 1,
				ColumnSpacing = 0,		
				BackgroundColor = Color.Transparent,
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

			var mainContent = new RelativeLayout (){
				BackgroundColor = Color.White,
				HeightRequest = 160,
				WidthRequest = 300,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};

			mainContent.Children.Add(grid,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent ((parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent ((parent) => {
					return parent.Height;
				})
			);

			var gestureRecognizer = new TapGestureRecognizer ();
			gestureRecognizer.Command = new Command (OnMediaViewTapped);
			gestureRecognizer.NumberOfTapsRequired = 1;
			grid.GestureRecognizers.Add (gestureRecognizer);
			Content = mainContent;
		}

		public void SetCardData(Fixture fixture)
		{
			_homeTeamDetails.SetDetailViewData(fixture?.HomeTeam);
			_awayTeamDetails.SetDetailViewData(fixture?.AwayTeam);
		}

		void OnMediaViewTapped()
		{
		}

	}
}
