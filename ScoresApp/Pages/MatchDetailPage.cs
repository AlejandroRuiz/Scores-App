using System;
using Xamarin.Forms;
using ScoresApp.ViewModels;
using ScoresApp.Controls;
using ScoresApp.UI;
using System.Threading.Tasks;
using ScoresApp.Service;
using System.Threading;
using ScoresApp.Models;

namespace ScoresApp.Pages
{
	public class MatchDetailPage : ContentPage
	{
		Label _goalsLabel;

		SvgImage _homeTeamImage;
		ListView _homeTeamPlayers;

		SvgImage _awayTeamImage;
		ListView _awayTeamPlayers;

		public FixtureViewModel ViewModel
		{
			get{
				return BindingContext as FixtureViewModel;
			}
		}

		ContentView GetHeader()
		{
			var activity = new ActivityIndicator ();
			activity.IsRunning = true;
			activity.Color = ScoresAppStyleKit.NavigationBarBackgroundColor;

			return new ContentView {
				Padding = new Thickness(0,20,0,0),
				Content = activity
			};
		}

		void CreateLayout()
		{
			_homeTeamImage = new SvgImage {
				SvgPath = ViewModel.HomeTeamImage
			};

			_homeTeamPlayers = new ListView{
				BackgroundColor = Color.White,
				ItemTemplate = new DataTemplate(typeof(TextCell)),
				//SeparatorVisibility = SeparatorVisibility.None,
				SeparatorColor = ScoresAppStyleKit.MenuBackgroundColor,
				Header = GetHeader(),
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			//_homeTeamPlayers.ItemTemplate.SetBinding (TextCell.ImageSourceProperty, nameof (TeamPlayer.DummyPlayer.Icon));

			_homeTeamPlayers.ItemTemplate.SetBinding (TextCell.TextProperty, nameof (TeamPlayer.DummyPlayer.Title));
			_homeTeamPlayers.ItemTemplate.SetBinding (TextCell.TextColorProperty, nameof (TeamPlayer.DummyPlayer.TextColor));

			_homeTeamPlayers.ItemTemplate.SetBinding (TextCell.DetailProperty, nameof (TeamPlayer.DummyPlayer.Detail));
			_homeTeamPlayers.ItemTemplate.SetBinding (TextCell.DetailColorProperty, nameof (TeamPlayer.DummyPlayer.DescriptionColor));

			_awayTeamPlayers = new ListView {
				BackgroundColor = Color.White,
				ItemTemplate = new DataTemplate(typeof(TextCell)),
				//SeparatorVisibility = SeparatorVisibility.None,
				SeparatorColor = ScoresAppStyleKit.MenuBackgroundColor,
				Header = GetHeader(),
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			//_awayTeamPlayers.ItemTemplate.SetBinding (TextCell.ImageSourceProperty, nameof (TeamPlayer.DummyPlayer.Icon));

			_awayTeamPlayers.ItemTemplate.SetBinding (TextCell.TextProperty, nameof (TeamPlayer.DummyPlayer.Title));
			_awayTeamPlayers.ItemTemplate.SetBinding (TextCell.TextColorProperty, nameof (TeamPlayer.DummyPlayer.TextColor));

			_awayTeamPlayers.ItemTemplate.SetBinding (TextCell.DetailProperty, nameof (TeamPlayer.DummyPlayer.Detail));
			_awayTeamPlayers.ItemTemplate.SetBinding (TextCell.DetailColorProperty, nameof (TeamPlayer.DummyPlayer.DescriptionColor));

			if(Device.OS == TargetPlatform.iOS){
				_homeTeamPlayers.RowHeight = 50;
				_awayTeamPlayers.RowHeight = 50;
			}

			_goalsLabel = new Label {
				Text = ViewModel.HomeTeamGoals +" - "+ViewModel.AwayTeamGoals,
				FontSize = 50,
				FontFamily = "AvenirNext-DemiBold",
				TextColor = Color.Black,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			_awayTeamImage = new SvgImage {
				SvgPath = ViewModel.AwayTeamImage
			};

			var headerBackgroundLayout = new StackLayout {
				HeightRequest  = 100,
				Spacing = 0,
				Children = {
					new BoxView{
						HeightRequest = 50,
						Color = ScoresAppStyleKit.NavigationBarBackgroundColor
					},
					new BoxView{
						HeightRequest = 50,
						Color = ScoresAppStyleKit.MenuBackgroundColor
					}
				}
			};

			var realtiveHeaderLayout = new RelativeLayout (){
				HeightRequest = 100,
				VerticalOptions = LayoutOptions.Start
			};
			realtiveHeaderLayout.Children.Add (headerBackgroundLayout,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent ((parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent ((parent) => {
					return parent.Height;
				})
			);

			realtiveHeaderLayout.Children.Add (_homeTeamImage,
				Constraint.Constant(10),
				Constraint.RelativeToParent ((parent) => {
					return (100 / 2) - 40;
				}),
				Constraint.RelativeToParent ((parent) => {
					return 80;
				}),
				Constraint.RelativeToParent ((parent) => {
					return 80;
				})
			);

			realtiveHeaderLayout.Children.Add (_goalsLabel,
				Constraint.Constant (0),
				Constraint.Constant (0),
				Constraint.RelativeToParent ((parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent ((parent) => {
					return 100;
				})
			);

			realtiveHeaderLayout.Children.Add (_awayTeamImage,
				Constraint.RelativeToParent ((parent) => {
					return parent.Width - 10 - 80;
				}),
				Constraint.RelativeToParent ((parent) => {
					return (100 / 2) - 40;
				}),
				Constraint.RelativeToParent ((parent) => {
					return 80;
				}),
				Constraint.RelativeToParent ((parent) => {
					return 80;
				})
			);
			Content = new StackLayout {
				Spacing = 0,
				BackgroundColor = Color.White,
				Children = {
					realtiveHeaderLayout,
					new StackLayout {
						VerticalOptions  = LayoutOptions.FillAndExpand,
						BackgroundColor = ScoresAppStyleKit.MenuBackgroundColor,
						Spacing = 2,
						Orientation = StackOrientation.Horizontal,
						Children = {
							_homeTeamPlayers,
							_awayTeamPlayers
						}
					}
				}
			};
		}

		public MatchDetailPage (FixtureViewModel vm)
		{
			BindingContext = vm;
			this.SetBinding (ContentPage.TitleProperty, path:nameof (vm.MatchTitle));
			CreateLayout ();
		}

		CancellationTokenSource cts;

		async Task LoadData()
		{
			var data = await WebService.Default.GetFixtureData (ViewModel.Fixture.Id, true,  cts);
			if (data != null) {
				Device.BeginInvokeOnMainThread (() => {
					_homeTeamPlayers.Header = null;
					_homeTeamPlayers.ItemsSource = data.HomeTeam.Players;
					_awayTeamPlayers.Header = null;
					_awayTeamPlayers.ItemsSource = data.AwayTeam.Players;
				});
				StartAutoUpdate ();
			}
		}

		protected async override void OnAppearing ()
		{
			base.OnAppearing ();
			cts = new CancellationTokenSource ();
			_homeTeamPlayers.ItemTapped += _homeTeamPlayers_ItemTapped;
			_awayTeamPlayers.ItemTapped += _awayTeamPlayers_ItemTapped;
			await LoadData ();
			StartTimer ();
		}

		void _awayTeamPlayers_ItemTapped (object sender, ItemTappedEventArgs e)
		{
			_awayTeamPlayers.SelectedItem = null;
		}

		void _homeTeamPlayers_ItemTapped (object sender, ItemTappedEventArgs e)
		{
			_homeTeamPlayers.SelectedItem = null;
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			if (cts != null) {
				cts.Cancel();
				cts = new CancellationTokenSource();
			}
			PauseAutoUpdate ();
			StopAutoUpdate ();
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
					var result = await ScoresApp.Service.WebService.Default.GetFixtureData (ViewModel.Fixture.Id, false, cts);
					if (ReadyToUpdate () && result != null) {
						_goalsLabel.Text = result.HomeTeam.TeamGoals +" - "+result.AwayTeam.TeamGoals;
					}
				}
				_isBusy = false;
			});
		}

		bool ReadyToUpdate()
		{
			return true;
		}
		#endregion
	}
}

