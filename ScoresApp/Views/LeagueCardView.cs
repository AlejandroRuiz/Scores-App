using System;
using Xamarin.Forms;
using ScoresApp.Models;
using ScoresApp.UI;

namespace ScoresApp.Views
{
	public class LeagueCardView : ContentView
	{
		public LeagueItem LeagueItem
		{
			get{
				return BindingContext as LeagueItem;
			}
		}

		public LeagueCardView(LeagueItem context)
		{
			BindingContext = context;

			Grid grid = new Grid {
				Padding = new Thickness(1,1,2,2),
				RowSpacing = 0,
				ColumnSpacing = 0,		
				BackgroundColor = Color.FromHex ("E3E3E3").MultiplyAlpha(0.5),
				RowDefinitions = {
					new RowDefinition { Height = new GridLength (100, GridUnitType.Absolute) }
				},
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength (4, GridUnitType.Absolute) },
					new ColumnDefinition {
						
					}
				}
			};

			grid.Children.Add (
				new FixtureCardStatusView ()
				,0,0);
			grid.Children.Add (new LeagueCardDetailsView (LeagueItem), 1, 0);

			var tgr = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
			tgr.Tapped += (sender, args) => {
				OnCardClicked?.Invoke (LeagueItem);
			};
			grid.GestureRecognizers.Add (tgr);
			Content = grid;
		}

		void Button_Clicked (object sender, EventArgs e)
		{
			OnCardClicked?.Invoke (LeagueItem);
		}

		public Action<LeagueItem> OnCardClicked { get; set; }
	}
}

