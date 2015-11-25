using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace ScoresApp.Views
{
	public class LoadingDataView:ContentView
	{
		Image _loadingImage;

		Label _loadingLabel;

		void CreateLayout()
		{
			_loadingLabel = new Label {
				Text = "Loading Data",
				FontSize = 25,
				FontFamily = "AvenirNext-DemiBold",
				TextColor = Color.Black,
				HorizontalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			_loadingImage = new Image{
				Source = "ic_autorenew_48pt"
			};

			Content = new StackLayout {
				Spacing = 20,
				Children = {
					_loadingImage,
					_loadingLabel
				}
			};
		}

		public async void RotateImage()
		{
			int i = 0;
			while (true) {
				if (i == 10)
					i = 0;
				await Task.Delay (100);
				_loadingImage.Rotation = (360 / 10) * (i + 1);
				i++;
			}
		}

		public LoadingDataView ()
		{
			CreateLayout ();
		}

	}
}

