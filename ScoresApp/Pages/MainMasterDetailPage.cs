using System;
using Xamarin.Forms;
using ScoresApp.UI;
using Xamarin;
using ScoresApp.Defaults;
using System.Collections.Generic;

namespace ScoresApp.Pages
{
	public class MainMasterDetailPage:MasterDetailPage
	{
		public MainMasterDetailPage ()
		{
			Master = new MenuPage ();
			NavigateToPage (new WelcomePage ());
		}

		string _currentPage { get; set; }

		public void NavigateToPage(Page toPage)
		{
			this.IsPresented = false;

			if (toPage.Title == _currentPage)
				return;
			
			this._currentPage = toPage.Title;
			this.Detail = new NavigationPage (toPage) {
				BarBackgroundColor = ScoresAppStyleKit.NavigationBarBackgroundColor,
				BarTextColor = ScoresAppStyleKit.NavigationBarTextColor
			};
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			Insights.Track(InsightsConstants.MainMasterDetailPage);

			//Record Test User To Xamarin Insights
			Insights.Identify(
				"elgoberlivel@gmail.com", 
				new Dictionary<string, string> 
				{
					{ Insights.Traits.Email, "elgoberlivel@gmail.com"},
					{ Insights.Traits.FirstName, "Alejandro" },
					{ Insights.Traits.LastName, "Ruiz" }
				}
			);
		}
	}
}

