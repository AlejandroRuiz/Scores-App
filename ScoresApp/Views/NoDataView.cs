using System;
using Xamarin.Forms;

namespace ScoresApp.Views
{
	public class NoDataView : ContentView
	{
		Label _noFavoritesLabel;

		Image _noFavoritesImage;

		void CreateLayout(string icon, string text)
		{

			_noFavoritesLabel = new Label {
				Text = text,
				FontSize = 25,
				FontFamily = "AvenirNext-DemiBold",
				TextColor = Color.Black,
				HorizontalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			_noFavoritesImage = new Image{
				Source = icon
			};

			Content = new StackLayout {
				Spacing = 20,
				Children = {
					_noFavoritesImage,
					_noFavoritesLabel
				}
			};
		}

		public NoDataView (string icon, string text)
		{
			CreateLayout (icon, text);
		}
	}
}

