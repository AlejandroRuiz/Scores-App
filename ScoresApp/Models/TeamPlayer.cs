using System;
using Xamarin.Forms;

namespace ScoresApp.Models
{
	public class TeamPlayer
	{
		static TeamPlayer _dummyPlayer;
		public static TeamPlayer DummyPlayer
		{
			get {
				if (_dummyPlayer == null)
				{
					_dummyPlayer = new TeamPlayer ();
				}
				return _dummyPlayer;
			}
		}

		public string Name { get; set; }
		public string Position { get; set; }
		public string JerseyNumber { get; set; }
		public string DateOfBirth { get; set; }
		public string Nationality { get; set; }
		public string ContractUntil { get; set; }
		public string MarketValue { get; set; }

		public string Title
		{
			get{
				return JerseyNumber+" "+Name;
			}
		}

		public string Detail
		{
			get{
				return Position;
			}
		}

		public string Icon
		{
			get{
				return "ic_account_circle";
			}
		}

		public Color TextColor {
			get{
				return Color.Black;
			}
		}

		public Color DescriptionColor {
			get{
				return Color.FromHex("546792");
			}
		}

		public TeamPlayer ()
		{
			
		}
	}
}

