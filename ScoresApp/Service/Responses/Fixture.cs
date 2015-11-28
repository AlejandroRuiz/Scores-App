using System;
using System.Collections.Generic;

namespace ScoresApp.Service.Responses
{
	public class SingleFixture
	{
		public SingleFixture ()
		{
			
		}

		public ScoresApp.Service.Responses.Fixtures.Fixture fixture { get; set; }

		public List<ScoresApp.Service.Responses.Player> homePlayers { get; set; } = new List<ScoresApp.Service.Responses.Player>();

		public List<ScoresApp.Service.Responses.Player> awayPlayers { get; set; } = new List<ScoresApp.Service.Responses.Player>();
	}
}

