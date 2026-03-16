using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class DeckUpdateRequest : Request
	{
		public List<Decks> decks;
	}
}
