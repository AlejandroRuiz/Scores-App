using System;

namespace ScoresApp.Service.Responses.Team
{
	public class Team
	{
		public Links _links { get; set; }
		public string name { get; set; }
		public string code { get; set; }
		public string shortName { get; set; }
		public string squadMarketValue { get; set; }
		public string crestUrl { get; set; }
	}

	public class Links
	{
		public Self self { get; set; }
		public Fixtures fixtures { get; set; }
		public Players players { get; set; }
	}

	public class Self
	{
		public string href { get; set; }
	}

	public class Fixtures
	{
		public string href { get; set; }
	}

	public class Players
	{
		public string href { get; set; }
	}
}

