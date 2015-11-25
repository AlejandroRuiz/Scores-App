using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScoresApp.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		protected virtual void OnPropertyChanged([CallerMemberName] string caller = null)
		{
			PropertyChanged (this, new PropertyChangedEventArgs (caller));
		}
	}

}

