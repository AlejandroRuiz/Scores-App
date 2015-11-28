using System;
using System.Collections.Generic;

namespace ScoresApp.Service.Responses
{
	public class Players
	{
		public int count { get; set; }
		public List<Player> players { get; set; }
	}

	public class Player
	{
		public string name { get; set; }
		public string position { get; set; }
		public string jerseyNumber { get; set; }
		public string dateOfBirth { get; set; }
		public string nationality { get; set; }
		public string contractUntil { get; set; }
		public string marketValue { get; set; }
	}
}

