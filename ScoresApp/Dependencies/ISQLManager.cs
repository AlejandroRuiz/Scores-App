using System;
using SQLite.Net.Interop;

namespace ScoresApp.Dependencies
{
	public interface ISQLManager
	{
		ISQLitePlatform NativeManager { get; }
	}
}

