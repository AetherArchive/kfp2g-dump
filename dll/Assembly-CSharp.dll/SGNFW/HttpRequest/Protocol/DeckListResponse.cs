using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class DeckListResponse : Response
	{
		public List<Decks> decks;
	}
}
