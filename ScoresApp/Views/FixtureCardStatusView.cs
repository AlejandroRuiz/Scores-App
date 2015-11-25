using System;
using Xamarin.Forms;
using ScoresApp.UI;

namespace ScoresApp
{
	public class FixtureCardStatusView: ContentView
	{
		public FixtureCardStatusView ()
		{
			var statusBoxView = new BoxView {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.Fill
			};
					
			statusBoxView.BackgroundColor = ScoresAppStyleKit.FixtureStatusColor;

			Content = statusBoxView;
		}
	}
}

