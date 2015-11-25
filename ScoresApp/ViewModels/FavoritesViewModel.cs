using System;
using System.Collections.ObjectModel;
using ScoresApp.Models;
using ScoresApp.Database;
using System.Collections.Generic;
using System.Linq;

namespace ScoresApp.ViewModels
{
	public class FavoritesViewModel : BaseViewModel
	{
		static FavoritesViewModel _favoritesViewModelManager;
		public static FavoritesViewModel FavoritesViewModelManager
		{
			get{ 
				if (_favoritesViewModelManager == null)
					_favoritesViewModelManager = new FavoritesViewModel ();
				return _favoritesViewModelManager;
			}
		}

		static ObservableCollection<LeagueItem> _favoritesLeagues;
		public ObservableCollection<LeagueItem> FavoritesLeagues
		{
			get{
				if (_favoritesLeagues == null) {
					_favoritesLeagues = new ObservableCollection<LeagueItem> (SqlManager.Cache.Favorites);
				}
				return _favoritesLeagues;
			}
		}

		public void UpdateFavorites()
		{
			var newList = SqlManager.Cache.Favorites;
			// nothing changed => do nothing
			if( this.IsEqualToCollection( newList ) ) return;

			// handle deleted items
			IList<LeagueItem> deletedItems = this.GetDeletedItems( newList );
			if( deletedItems.Count > 0 )
			{
				foreach( var deletedItem in deletedItems )
				{
					FavoritesLeagues.Remove( deletedItem );
				}
			}

			// handle added items
			IList<LeagueItem> addedItems = this.GetAddedItems( newList );           
			if( addedItems.Count > 0 )
			{
				foreach( LeagueItem addedItem in addedItems )
				{
					FavoritesLeagues.Add( addedItem );
				}
			}

			// equals now? => return
			if( this.IsEqualToCollection( newList ) ) return;

			// resort entries
			for( int index = 0; index < newList.Count; index++ )
			{
				var item = newList[index];
				int indexOfItem = FavoritesLeagues.IndexOf( item );
				if( indexOfItem != index ) FavoritesLeagues.Move( indexOfItem, index );
			}
		}

		private IList<LeagueItem> GetAddedItems( IEnumerable<LeagueItem> newList )
		{
			IList<LeagueItem> addedItems = new List<LeagueItem>();
			foreach( LeagueItem item in newList )
			{
				if( !this.ContainsItem( item ) ) addedItems.Add( item );
			}
			return addedItems;
		}

		private IList<LeagueItem> GetDeletedItems( IEnumerable<LeagueItem> newList )
		{
			IList<LeagueItem> deletedItems = new List<LeagueItem>();
			foreach( var item in FavoritesLeagues )
			{
				if( !newList.Contains( item ) ) deletedItems.Add( item );
			}
			return deletedItems;
		}

		private bool IsEqualToCollection( IList<LeagueItem> newList )
		{   
			// diffent number of items => collection differs
			if( FavoritesLeagues.Count != newList.Count ) return false;

			for( int i = 0; i < FavoritesLeagues.Count; i++ )
			{
				if( !FavoritesLeagues[i].Equals( newList[i] ) ) return false;
			}
			return true;
		}

		private bool ContainsItem( object value )
		{
			foreach( var item in FavoritesLeagues )
			{
				if( value.Equals( item ) ) return true;
			}
			return false;
		}

		public FavoritesViewModel ()
		{
		}
	}
}

