using System;
using Xamarin.Forms;
using ScoresApp.Models;
using ScoresApp.UI;
using ScoresApp.Controls;

namespace ScoresApp.Views
{
	public class FixtureCardDetailsView : ContentView
	{
		Label _teamName;

		SvgImage _teamImage;

		public Label TeamGoals;

		public FixtureCardDetailsView (FixtureTeam team)
		{
			_teamName = new Label () {
				LineBreakMode = LineBreakMode.TailTruncation,
				Text = team?.TeamName,
				FontSize = 18,
				TextColor = Color.Black,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			TeamGoals = new Label () {
				Text = team?.TeamGoals.ToString(),
				FontSize = 15,
				FontFamily = "AvenirNext-DemiBold",
				TextColor = ScoresAppStyleKit.NavigationBarTextColor,
				BackgroundColor = ScoresAppStyleKit.NavigationBarBackgroundColor,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = 20,
				WidthRequest = 20,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.EndAndExpand
			};

			_teamImage = new SvgImage {
				SvgPath = team?.TeamImage,
				HeightRequest = 40,
				WidthRequest = 40,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			var stack = new StackLayout () {
				BackgroundColor = ScoresAppStyleKit.MenuBackgroundColor,
				Spacing = 10,
				Padding = new Thickness (5, 0, 5, 0),
				Orientation = StackOrientation.Horizontal,
				Children = {
					_teamImage,
					_teamName,
					TeamGoals
				}
			};

			Content = stack;
		}

		public void SetDetailViewData(FixtureTeam team)
		{
			_teamName.Text = team?.TeamName;
			_teamImage.SvgPath = team?.TeamImage;
		}
	}
}