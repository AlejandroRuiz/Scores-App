using System;
using Xamarin.Forms;
using ScoresApp.UI;
using ScoresApp.Defaults;
using ScoresApp.Database;
using ScoresApp.Views;
using ScoresApp.Models;
using System.Collections.Generic;
using ScoresApp.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace ScoresApp.Pages
{
	public class WelcomePage:ContentPage
	{

		#region UI
		ScrollView _mainContent;
		StackLayout _mainLayot;
		NoDataView _noFavoritesView;

		async void CreateLayout()
		{
			if (_noFavoritesView == null) {
				_noFavoritesView = new NoDataView (Strings.HeartIcon, Strings.NoFavoritesText) {
					VerticalOptions = LayoutOptions.CenterAndExpand,
					HorizontalOptions = LayoutOptions.CenterAndExpand
				};
			}
			_noFavoritesView.Opacity = 0;

			if (_mainLayot == null) {
				_mainLayot = new StackLayout {
					Spacing = 20,
					Padding = new Thickness (10)
				};
			}

			if (_mainContent == null) {
				_mainContent = new ScrollView {
					Opacity = 0,
					Content = _mainLayot
				};
			}

			if (Content == null) {
				
				if (ViewModel.FavoritesLeagues.Count == 0) {
					Content = _noFavoritesView;
					await Content.FadeTo (1, (uint)Integers.AnimationSpeed, Easing.SinIn);
					return;
				} else {
					foreach (var league in SqlManager.Cache.Favorites) {
						var card = new LeagueCardView (league);
						card.OnCardClicked = OnCardClicked;
						_mainLayot.Children.Add (card);
					}
				}

				Content = _mainContent;

				await Content.FadeTo (1, (uint)Integers.AnimationSpeed, Easing.SinIn);
			} else {
				if (_mainContent.Content is StackLayout) {
					if (ViewModel.FavoritesLeagues.Count == 0) {
						foreach (var card in (_mainContent.Content as StackLayout).Children) {
							await card.FadeTo (0, (uint)Integers.AnimationSpeed, Easing.SinIn);
						}
						Content = _noFavoritesView;
						await _noFavoritesView.FadeTo (1, (uint)Integers.AnimationSpeed, Easing.SinIn);
					} else if(ViewModel.FavoritesLeagues.Count != (_mainContent.Content as StackLayout).Children.Count) {
						var removedIndex = -1;
						for(int i = 0; i < (_mainContent.Content as StackLayout).Children.Count; i++)
						{
							if (removedIndex != -1) {
								/*(_mainContent.Content as StackLayout).Children [i].TranslateTo (
									(_mainContent.Content as StackLayout).Children [i - 1].Y,
									(_mainContent.Content as StackLayout).Children [i].X,
									(uint)Integers.AnimationSpeed,
									Easing.Linear
								);*/
							}

							if (ViewModel.FavoritesLeagues.FirstOrDefault (item => item.Id == ((_mainContent.Content as StackLayout).Children[i] as LeagueCardView).LeagueItem.Id) == null && removedIndex == -1) {
								removedIndex = i;
								await (_mainContent.Content as StackLayout).Children [i].FadeTo (0, (uint)Integers.AnimationSpeed, Easing.SinIn);
								await Task.Delay (100);
								(_mainContent.Content as StackLayout).Children.RemoveAt (i);
								break;
							}
						}
						/*if (!ViewModel.FavoritesLeagues.Contains ((card as LeagueCardView).LeagueItem)) {
							card.FadeTo (0, (uint)Integers.AnimationSpeed, Easing.SinIn);
						}*/
						//Content = 
					}
				} else {
					Content = _noFavoritesView;
					await _noFavoritesView.FadeTo (1, (uint)Integers.AnimationSpeed, Easing.SinIn);
				}
			}
		}

		#endregion

		#region Constructos

		public WelcomePage ()
		{
			BindingContext = FavoritesViewModel.FavoritesViewModelManager;
			Title = Strings.AppName;
			BackgroundColor = ScoresAppStyleKit.PageBackgroundColor;
		}

		#endregion

		#region ViewModel

		FavoritesViewModel ViewModel
		{
			get{
				return BindingContext as FavoritesViewModel;
			}
		}

		void SetBindings()
		{
			
		}

		#endregion

		#region ContentPage LifeCycle

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			Title = Strings.AppName;
			CreateLayout ();
		}

		protected override void OnDisappearing ()
		{
			Title = string.Empty;
			base.OnDisappearing ();
		}

		#endregion

		#region Handlers

		void OnCardClicked(LeagueItem item)
		{
			Title = "";
			Navigation.PushAsync (new LeagueResultsPage (item));
		}

		#endregion
	}
}

