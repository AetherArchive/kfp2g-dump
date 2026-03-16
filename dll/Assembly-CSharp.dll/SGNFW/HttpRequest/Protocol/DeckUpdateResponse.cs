using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class DeckUpdateResponse : Response
	{
		public int deck_result;

		public Assets assets;
	}
}
