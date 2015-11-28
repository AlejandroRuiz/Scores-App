using System;
using Xamarin.Forms;
using ScoresApp.Models;
using ScoresApp.ViewModels;

namespace ScoresApp.Views
{
	public class FixtureLayout : ViewCell
	{
		public FixtureLayout ()
		{
			this.Height = 120;
			CreateLayout ();
		}

		void CreateLayout()
		{
			_mainCardView = new FixtureCardView (Fixture);
			View = new StackLayout {
				Children = {
					_mainCardView
				}
			};
		}

		FixtureCardView _mainCardView { get; set; }

		FixtureViewModel Fixture{
			get{
				return BindingContext as FixtureViewModel;
			}
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();
			if (Fixture == null)
				return;
			_mainCardView.SetCardData (Fixture);
		}
	}
}

