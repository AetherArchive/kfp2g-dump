using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class GetGrowthEventCharaIdResponse : Response
	{
		public int chara_id;

		public int event_id;

		public long select_chara_datetime;

		public long quest_clear_datetime;
	}
}
