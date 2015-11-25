using System;
using Xamarin.Forms;
using ScoresApp.Models;

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

		Fixture Fixture{
			get{
				return BindingContext as Fixture;
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

