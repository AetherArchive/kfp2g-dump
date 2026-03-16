using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PvPDeckSelectRequest : Request
	{
		public int type_id;

		public int season_id;

		public int deck_id;
	}
}
