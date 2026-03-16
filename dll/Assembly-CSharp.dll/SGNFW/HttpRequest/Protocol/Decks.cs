using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class Decks
	{
		public int deck_id;

		public string deck_name;

		public int master_skil_id;

		public int kemostatus;

		public List<Deck> deck;

		public int tactics_param1;

		public int tactics_param2;

		public int tactics_param3;
	}
}
