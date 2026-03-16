using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestLimitRecoveryRequest : Request
	{
		public int quest_id;

		public bool is_raid;
	}
}
