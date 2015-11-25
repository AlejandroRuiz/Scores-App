using System;
using Xamarin.Forms;
using ScoresApp.Models;
using ScoresApp.UI;
using ScoresApp.Defaults;
using System.Threading.Tasks;
using ScoresApp.Views;
using ScoresApp.Database;
using Refractored.XamForms.PullToRefresh;

namespace ScoresApp.Pages
{
	public class LeagueResultsPage:ContentPage
	{
		public LeagueItem Item
		{
			get{
				return BindingContext as LeagueItem;
			}
		}

		ListView _fixturesListView;

		ToolbarItem _addItem;

		NoDataView _noDataView;

		LoadingDataView _loadingDataView;

		#region PulltoRefreshUI

		Image _arrowImage;
		Label _pullText;
		StackLayout _pullToRefreshLayout;
		ListView _mainNoDataLayout;

		#endregion

		void CreateLayout()
		{

			_noDataView = new NoDataView("ic_cloud_off_48pt", "No Data Found") {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};

			_loadingDataView = new LoadingDataView {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};

			_arrowImage = new Image {
				Source = "ic_arrow_downward"
			};
			_pullText = new Label () {
				Text = "Pull To Refresh",
				FontSize = 15,
				TextColor = Color.Black,
				HorizontalTextAlignment = TextAlignment.Center

			};

			_pullToRefreshLayout = new StackLayout {
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Spacing = 5,
				Padding = new Thickness(0,5,0,0),
				Children = {
					_arrowImage,
					_pullText
				}
			};

			_mainNoDataLayout = new ListView {
				SeparatorVisibility = SeparatorVisibility.None,
				IsPullToRefreshEnabled = true,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Header = new StackLayout{
					Spacing = 200,
					Children = {
						_pullToRefreshLayout,
						_noDataView
					}
				}
			};

			var isfavorite = Item.IsFavorite;
			_addItem = new ToolbarItem ();
			_addItem.Text = isfavorite ? "remove" : "add";
			_addItem.Clicked += _addItem_Clicked;
			_addItem.Icon = isfavorite ? "ic_favorite_white" : "ic_favorite_border_white";

			ToolbarItems.Add (_addItem);

			_fixturesListView = new ListView (ListViewCachingStrategy.RecycleElement) {
				RowHeight = 120,
				IsPullToRefreshEnabled = true,
				BackgroundColor = Color.White,
				ItemTemplate = new DataTemplate(typeof(FixtureLayout)),
				SeparatorVisibility = SeparatorVisibility.None,
				Opacity = 0
			};
		}

		async void _addItem_Clicked (object sender, EventArgs e)
		{
			var isfavorite = Item.IsFavorite;
			var detailText = "Do you want to "+ (isfavorite ? "remove" : "add")+" " + Item.Text + " " + (isfavorite ? "from" : "to") + " your favorites?";
			var result = await DisplayAlert (Strings.AppName, detailText, "Ok", "Cancel");
			if (result) {
				if (!isfavorite)
					SqlManager.Cache.AddToFavorite (Item);
				else
					SqlManager.Cache.RemeveFromFavorite (Item);
				_addItem.Icon = Item.IsFavorite ? "ic_favorite_white" : "ic_favorite_border_white";
			}
		}

		public LeagueResultsPage (LeagueItem leagueItem)
		{
			BindingContext = leagueItem;
			Title = Item.Text;
			BackgroundColor = ScoresAppStyleKit.PageBackgroundColor;
			CreateLayout ();
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			_fixturesListView.ItemTapped += _fixturesListView_ItemTapped;
			_fixturesListView.Refreshing += _fixturesListView_Refreshing;
			_mainNoDataLayout.Refreshing += _fixturesListView_Refreshing;
			_fixturesListView.BeginRefresh ();
		}

		async Task LoadFixtures()
		{

			Device.BeginInvokeOnMainThread (async() => {
				if(_loadingDataView == null)
				{
					_loadingDataView = new LoadingDataView {
						VerticalOptions = LayoutOptions.CenterAndExpand,
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						Opacity = 0
					};
				}

				Content = _loadingDataView;
				await Content.FadeTo (1, (uint)Integers.AnimationSpeed,  Easing.SinIn);
				_loadingDataView.RotateImage();
			});

			var result = await ScoresApp.Service.WebService.Default.GetLeagueMatches (Item);
			_fixturesListView.ItemsSource = result;

			Device.BeginInvokeOnMainThread(async()=>{
				await Content.FadeTo (0, (uint)Integers.AnimationSpeed,  Easing.SinOut);
				_fixturesListView.EndRefresh();
				_mainNoDataLayout.EndRefresh();
				if(result.Count > 0){
					Content= _fixturesListView;
				}else{
					Content = _mainNoDataLayout;
				}
				_loadingDataView = null;
				await Content.FadeTo (1, (uint)Integers.AnimationSpeed,  Easing.SinIn);
			});
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			_fixturesListView.ItemTapped -= _fixturesListView_ItemTapped;
			_fixturesListView.Refreshing -= _fixturesListView_Refreshing;
			_mainNoDataLayout.Refreshing -= _fixturesListView_Refreshing;
		}

		async void RefreshingAction ()
		{
			await LoadFixtures ();
		}

		async void _fixturesListView_Refreshing (object sender, EventArgs e)
		{
			await LoadFixtures ();
		}

		void _fixturesListView_ItemTapped (object sender, ItemTappedEventArgs e)
		{
			_fixturesListView.SelectedItem = null;
		}
	}
}

