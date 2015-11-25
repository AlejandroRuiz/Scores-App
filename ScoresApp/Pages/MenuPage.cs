using System;
using Xamarin.Forms;
using ScoresApp.UI;
using System.Collections.ObjectModel;
using ScoresApp.Models;
using ScoresApp.ViewModels;
using ScoresApp.Defaults;

namespace ScoresApp.Pages
{
	public class MenuPage : ContentPage
	{
		#region UI

		Image _ballImage;
		Label _leagueLabel;
		ListView _menuListView;
		StackLayout _mainLayout;

		void CreateLayout()
		{

			_ballImage = new Image {
				HeightRequest = 50,
				WidthRequest = 50,
				Source = Strings.AppLogoIcon
			};

			_leagueLabel = new Label {
				Text = Strings.LeagueText,
				TextColor = Color.Black,
				FontSize = 25,
				HorizontalTextAlignment = TextAlignment.Center
			};

			_menuListView = new ListView {
				BindingContext = ViewModel,
				ItemTemplate = new DataTemplate(typeof(ImageCell)),
				RowHeight = 50,
				SeparatorVisibility = SeparatorVisibility.None,
				VerticalOptions = LayoutOptions.Start,
				BackgroundColor = ScoresAppStyleKit.MenuBackgroundColor
			};

			_mainLayout = new StackLayout {
				Padding = new Thickness(10,30,10,30),
				Children = {
					_ballImage,
					_leagueLabel,
					_menuListView
				},
				Spacing = 20
			};
			Content = _mainLayout;
		}

		#endregion

		#region Constructos

		public MenuPage ()
		{
			BindingContext = new MenuViewModel ();
			CreateLayout ();
			SetBindings ();
		}

		#endregion

		#region ViewModel

		MenuViewModel ViewModel
		{
			get{
				return BindingContext as MenuViewModel;
			}
		}

		void SetBindings()
		{
			this.SetBinding (ContentPage.TitleProperty, nameof(ViewModel.PageTitle));
			this.SetBinding (ContentPage.IconProperty, nameof(ViewModel.PageIcon));
			this.SetBinding (ContentPage.BackgroundColorProperty, nameof(ViewModel.PageBackgroundColor));

			this._menuListView.SetBinding (ListView.ItemsSourceProperty, nameof(ViewModel.MenuItems));
			this._menuListView.SetBinding (ListView.HeightRequestProperty, nameof (ViewModel.MenuItemsHeightRequest));

			this._menuListView.ItemTemplate.SetBinding (ImageCell.TextProperty, nameof(LeagueItem.Favorites.Text));
			this._menuListView.ItemTemplate.SetBinding (ImageCell.TextColorProperty, nameof(LeagueItem.Favorites.TextColor));
			this._menuListView.ItemTemplate.SetBinding (ImageCell.DetailProperty, nameof(LeagueItem.Favorites.Description));
			this._menuListView.ItemTemplate.SetBinding (ImageCell.DetailColorProperty, nameof(LeagueItem.Favorites.DescriptionColor));
		}

		#endregion

		#region ContentPage LifeCycle

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			_menuListView.ItemTapped += _menuListView_ItemTapped;
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			_menuListView.ItemTapped -= _menuListView_ItemTapped;
		}

		#endregion

		#region Handlers

		void _menuListView_ItemTapped (object sender, ItemTappedEventArgs e)
		{
			_menuListView.SelectedItem = null;
			var leagueItem = e.Item as LeagueItem;
			if (leagueItem.Id == LeagueItem.Favorites.Id)
				App.CurrentApp.CurrentMasterDetailPage.NavigateToPage (new WelcomePage ());
			else
				App.CurrentApp.CurrentMasterDetailPage.NavigateToPage (new LeagueResultsPage (leagueItem));
		}

		#endregion
	}
}

