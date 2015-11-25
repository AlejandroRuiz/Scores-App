using System;
using System.Collections.ObjectModel;
using ScoresApp.Models;
using Xamarin.Forms;
using ScoresApp.UI;

namespace ScoresApp.ViewModels
{
	public class MenuViewModel:BaseViewModel
	{
		ObservableCollection<LeagueItem> _menuItems;
		public ObservableCollection<LeagueItem> MenuItems
		{
			get{
				if (_menuItems == null) {
					_menuItems = new ObservableCollection<LeagueItem> (){
						LeagueItem.Favorites,
						LeagueItem.Bundesliga,
						LeagueItem.PremiereLeague,
						LeagueItem.SerieA,
						LeagueItem.PrimeraDivision,
						LeagueItem.Ligue1,
						LeagueItem.Eredivisie
					};
				}
				return _menuItems;
			}
		}

		public int MenuItemsHeightRequest
		{
			get{
				return 50 * _menuItems.Count;
			}
		}

		public string PageTitle
		{
			get{
				return "Menu";
			}
		}

		public string PageIcon
		{
			get{
				return "ic_menu_white";
			}
		}

		public Color PageBackgroundColor
		{
			get{
				return ScoresAppStyleKit.MenuBackgroundColor;
			}
		}

		public MenuViewModel ()
		{
		}
	}
}

