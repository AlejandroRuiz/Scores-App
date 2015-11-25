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
				Padding = new Thickness(0,0,1,1),
				RowSpacing = 0,
				ColumnSpacing = 0,		
				BackgroundColor = Color.FromHex ("E3E3E3"),
				RowDefinitions = {
					new RowDefinition { Height = new GridLength (100, GridUnitType.Absolute) }
				},
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength (4, GridUnitType.Absolute) },
					new ColumnDefinition { Width = new GridLength (300, GridUnitType.Star) }
				}
			};

			grid.Children.Add (
				new FixtureCardStatusView ()
				,0,0);

			grid.Children.Add (new LeagueCardDetailsView (LeagueItem), 1, 0);

			var mainContent = new RelativeLayout ();

			mainContent.Children.Add(grid,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent ((parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent ((parent) => {
					return 100;
				})
			);
			var button = new Button (){
				BackgroundColor = Color.Transparent,
				BorderColor = Color.Transparent,
				BorderRadius = 0,
				BorderWidth = 0,
				Text = ""
			};
			mainContent.Children.Add(button,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent ((parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent ((parent) => {
					return 100;
				})
			);
			button.Clicked += Button_Clicked;

			Content = mainContent;
		}

		void Button_Clicked (object sender, EventArgs e)
		{
			OnCardClicked?.Invoke (LeagueItem);
		}

		public Action<LeagueItem> OnCardClicked { get; set; }
	}
}

