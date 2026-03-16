using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestSkipRecoveryResponse : Response
	{
		public Assets assets;

		public List<Quest> quests;
	}
}
