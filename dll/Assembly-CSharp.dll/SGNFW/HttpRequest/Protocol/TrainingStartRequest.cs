using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingStartRequest : Request
	{
		public int season_id;

		public int dayofweek;

		public int quest_id;

		public int deck_id;

		public int kemostatus;
	}
}
