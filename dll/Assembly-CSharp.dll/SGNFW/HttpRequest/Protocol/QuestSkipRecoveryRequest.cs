using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestSkipRecoveryRequest : Request
	{
		public int quest_id;

		public int use_item_id;

		public int skip_recovery_num;
	}
}
