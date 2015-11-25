using System;
using Xamarin.Forms;
using ScoresApp.UI;

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
	}
}

