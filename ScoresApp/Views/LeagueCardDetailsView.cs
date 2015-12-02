using System;
using Xamarin.Forms;
using ScoresApp.Models;
using ScoresApp.UI;

namespace ScoresApp
{
	public class LeagueCardDetailsView : ContentView
	{
		LeagueItem LeagueItem
		{
			get{
				return BindingContext as LeagueItem;
			}
		}

		Image _flagImage;

		Label _leagueNameLabel;

		Label _leagueCountryLabel;

		public LeagueCardDetailsView (LeagueItem item)
		{
			BindingContext = item;

			_leagueNameLabel = new Label {
				Text = item.Text,
				FontSize = 15,
				FontFamily = "AvenirNext-DemiBold",
				TextColor = LeagueItem.Favorites.TextColor,
				HorizontalTextAlignment = TextAlignment.Center,
				IsEnabled = true,
				InputTransparent = true
			};

			_leagueCountryLabel = new Label {
				Text = item.Description,
				FontSize = 10,
				FontFamily = "AvenirNext-DemiBold",
				TextColor = LeagueItem.Favorites.DescriptionColor,
				HorizontalTextAlignment = TextAlignment.Center,
				IsEnabled = true,
				InputTransparent = true
			};

			_flagImage = new Image {
				Source = LeagueItem.Image,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Aspect = Aspect.AspectFill,
				WidthRequest = 120,
				HorizontalOptions = LayoutOptions.Start,
				IsEnabled = true,
				InputTransparent = true
			};

			Content = new StackLayout {
				HeightRequest = 100,
				HorizontalOptions = LayoutOptions.Fill,
				BackgroundColor = ScoresAppStyleKit.PageBackgroundColor,
				Orientation = StackOrientation.Horizontal,
				IsEnabled = true,
				InputTransparent = true,
				Children = {
					_flagImage,
					new StackLayout {
						Spacing = 5,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.Center,
						Children = {
							_leagueNameLabel,
							_leagueCountryLabel
						},
						IsEnabled = true,
						InputTransparent = true
					}
				}
			};
		}
	}
}

