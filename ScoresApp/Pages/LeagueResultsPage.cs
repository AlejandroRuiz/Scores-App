using System;
using System.Linq;
using Xamarin.Forms;
using ScoresApp.Models;
using ScoresApp.UI;
using ScoresApp.Defaults;
using System.Threading.Tasks;
using ScoresApp.Views;
using ScoresApp.Database;
using ScoresApp.Controls;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading;
using Xamarin;
using ScoresApp.ViewModels;

namespace ScoresApp.Pages
{
	public class LeagueResultsPage:ContentPage
	{
		public LeagueItem League
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

		FloatingActionButtonView fab;

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
			if (Device.OS == TargetPlatform.iOS) {
				var isfavorite = League.IsFavorite;
				_addItem = new ToolbarItem ();
				_addItem.Text = isfavorite ? "remove" : "add";
				_addItem.Clicked += _addItem_Clicked;
				_addItem.Icon = isfavorite ? "ic_favorite_white" : "ic_favorite_border_white";

				ToolbarItems.Add (_addItem);
			}

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
			var isfavorite = League.IsFavorite;
			var detailText = "Do you want to "+ (isfavorite ? "remove" : "add")+" " + League.Text + " " + (isfavorite ? "from" : "to") + " your favorites?";
			var result = await DisplayAlert (Strings.AppName, detailText, "Ok", "Cancel");
			if (result) {
				if (!isfavorite)
					SqlManager.Cache.AddToFavorite (League);
				else
					SqlManager.Cache.RemeveFromFavorite (League);
				_addItem.Icon = League.IsFavorite ? "ic_favorite_white" : "ic_favorite_border_white";
			}
		}

		public LeagueResultsPage (LeagueItem leagueItem)
		{
			BindingContext = leagueItem;
			Title = League.Text;
			BackgroundColor = ScoresAppStyleKit.PageBackgroundColor;
			CreateLayout ();
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			Title = League.Text;
			StartTimer ();

			Insights.Track(InsightsConstants.LeagueResultsPage);

			cts = new CancellationTokenSource ();
			_fixturesListView.ItemAppearing += _fixturesListView_ItemAppearing;
			_fixturesListView.ItemTapped += _fixturesListView_ItemTapped;
			_fixturesListView.Refreshing += _fixturesListView_Refreshing;
			_mainNoDataLayout.Refreshing += _fixturesListView_Refreshing;

			if(Content  != _fixturesListView)
				_fixturesListView.BeginRefresh ();
		}

		void _fixturesListView_ItemAppearing (object sender, ItemVisibilityEventArgs e)
		{
			if (Device.OS == TargetPlatform.Android) {
				if (e.Item == (_fixturesListView.ItemsSource as ObservableCollection<FixtureViewModel>).Last()) {
					fab.Hide ();
				} else {
					fab.Show ();
				}
			}
		}

		#region AutoUpdate
		bool _isBusy;

		bool _isAutoUpdateEnable;

		bool _isStopTimerRequested;

		void StartAutoUpdate()
		{
			_isAutoUpdateEnable = true;
		}

		void PauseAutoUpdate()
		{
			_isAutoUpdateEnable = false;
		}

		void StopAutoUpdate()
		{
			_isStopTimerRequested = true;
		}

		void StartTimer()
		{
			StartAutoUpdate ();
			Device.StartTimer (TimeSpan.FromSeconds(30), OnUpdate);
		}

		bool OnUpdate()
		{
			if (_isAutoUpdateEnable) {
				if (ReadyToUpdate ()) {
					if (!_isBusy) {
						_isBusy = true;
						BeginUpdate ();
					}
				}
			}
			return !_isStopTimerRequested;
		}

		Task BeginUpdate()
		{
			return Task.Run(async()=>{
				if(cts != null){
					var result = await ScoresApp.Service.WebService.Default.GetLeagueMatches (League.Id, false, cts);
				if (ReadyToUpdate ()) {
					foreach (var fixture in result) {
						var listData = _fixturesListView.ItemsSource as ObservableCollection<FixtureViewModel>;
							var viewModelToUpdate = listData.FirstOrDefault (i => i.Fixture.Id == fixture.Fixture.Id);
						if (viewModelToUpdate != null) {
							viewModelToUpdate.UpdateGoals (fixture);
						}
					}
				}
				}
			_isBusy = false;
			});
		}

		bool ReadyToUpdate()
		{
			if (_fixturesListView.ItemsSource != null) {
				if (_fixturesListView.ItemsSource is ObservableCollection<FixtureViewModel>) {
					if ((_fixturesListView.ItemsSource as ObservableCollection<FixtureViewModel>).Count > 0) {
						return true;
					}
				}
			}
			return false;
		}
		#endregion

		CancellationTokenSource cts;

		async Task LoadFixtures()
		{

			Device.BeginInvokeOnMainThread (async() => {
				if(_loadingDataView == null)
				{
					_loadingDataView = new LoadingDataView {
						VerticalOptions = LayoutOptions.CenterAndExpand,
						HorizontalOptions = LayoutOptions.CenterAndExpand,
					};
					if(Device.OS == TargetPlatform.iOS)
						_loadingDataView.Opacity = 0;
				}
				if(Device.OS == TargetPlatform.Android)
				{
					DroidLayout(new StackLayout{
						Opacity = 0,
						Children = {
							_loadingDataView
						}
					});
				}else{
					Content = _loadingDataView;
					await Content.FadeTo (1, (uint)Integers.AnimationSpeed,  Easing.SinIn);
				}
				if(_loadingDataView != null)
					_loadingDataView.RotateImage();
			});

			PauseAutoUpdate ();
			var result = await ScoresApp.Service.WebService.Default.GetLeagueMatches (League.Id, true, cts);
			_fixturesListView.ItemsSource = result;
			StartAutoUpdate ();

			Device.BeginInvokeOnMainThread(async()=>{
				_fixturesListView.EndRefresh();
				_mainNoDataLayout.EndRefresh();
				if(result.Count > 0){
					if(Device.OS == TargetPlatform.Android)
					{
						DroidLayout(_fixturesListView);
					}else{
						await Content.FadeTo (0, (uint)Integers.AnimationSpeed,  Easing.SinOut);
						Content = _fixturesListView;
						await Content.FadeTo (1, (uint)Integers.AnimationSpeed,  Easing.SinIn);
					}
				}else{
					if(Device.OS == TargetPlatform.Android)
					{
						DroidLayout(_mainNoDataLayout);
					}else{
						await Content.FadeTo (0, (uint)Integers.AnimationSpeed,  Easing.SinOut);
						Content = _mainNoDataLayout;
						await Content.FadeTo (1, (uint)Integers.AnimationSpeed,  Easing.SinIn);
					}
				}
				_loadingDataView = null;
			});
		}

		async void FabAction()
		{
			var isfavorite = League.IsFavorite;
			var detailText = "Do you want to "+ (isfavorite ? "remove" : "add")+" " + League.Text + " " + (isfavorite ? "from" : "to") + " your favorites?";
			var result = await DisplayAlert (Strings.AppName, detailText, "Ok", "Cancel");
			if (result) {
				if (!isfavorite)
					SqlManager.Cache.AddToFavorite (League);
				else
					SqlManager.Cache.RemeveFromFavorite (League);
				fab.ImageName = League.IsFavorite ? "ic_favorite_white" : "ic_favorite_border_white";
			}
		}

		async void DroidLayout(View mainView)
		{
			if (this.Content is AbsoluteLayout) {
				await (this.Content as AbsoluteLayout).Children[0].FadeTo (0, (uint)Integers.AnimationSpeed,  Easing.SinIn);
				(this.Content as AbsoluteLayout).Children.RemoveAt (0);
				AbsoluteLayout.SetLayoutFlags(mainView, AbsoluteLayoutFlags.All);
				AbsoluteLayout.SetLayoutBounds(mainView, new Rectangle(0f, 0f, 1f, 1f));
				(this.Content as AbsoluteLayout).Children.Insert (0, mainView);
				mainView.Opacity = 0;
				await mainView.FadeTo (1, (uint)Integers.AnimationSpeed,  Easing.SinIn);
				return;
			}

			fab = new FloatingActionButtonView
			{
				ImageName = League.IsFavorite ? "ic_favorite_white" : "ic_favorite_border_white",
				ColorNormal = Color.FromHex("009688"),
				ColorPressed = Color.FromHex("80CBC4"),
				ColorRipple = Color.FromHex("E0F2F1"),
				Clicked = (sender, args) => FabAction(),
			};

			var absolute = new AbsoluteLayout
			{ 
				VerticalOptions = LayoutOptions.FillAndExpand, 
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};

			// Position the pageLayout to fill the entire screen.
			// Manage positioning of child elements on the page by editing the pageLayout.
			AbsoluteLayout.SetLayoutFlags(mainView, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(mainView, new Rectangle(0f, 0f, 1f, 1f));
			absolute.Children.Add(mainView);

			// Overlay the FAB in the bottom-right corner
			AbsoluteLayout.SetLayoutFlags(fab, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds(fab, new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			absolute.Children.Add(fab);

			Content = absolute;
			await mainView.FadeTo (1, (uint)Integers.AnimationSpeed,  Easing.SinIn);
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			StopAutoUpdate ();
			if (cts != null) {
				cts.Cancel();
				cts = new CancellationTokenSource();
			}
			_fixturesListView.ItemTapped -= _fixturesListView_ItemTapped;
			_fixturesListView.Refreshing -= _fixturesListView_Refreshing;
			_mainNoDataLayout.Refreshing -= _fixturesListView_Refreshing;
			_fixturesListView.ItemAppearing -= _fixturesListView_ItemAppearing;
		}

		async void RefreshingAction ()
		{
			await LoadFixtures ();
		}

		async void _fixturesListView_Refreshing (object sender, EventArgs e)
		{
			await LoadFixtures ();
		}

		async void _fixturesListView_ItemTapped (object sender, ItemTappedEventArgs e)
		{
			_fixturesListView.SelectedItem = null;
			this.Title = "";
			await Navigation.PushAsync (new MatchDetailPage(e.Item as FixtureViewModel));
		}
	}
}

