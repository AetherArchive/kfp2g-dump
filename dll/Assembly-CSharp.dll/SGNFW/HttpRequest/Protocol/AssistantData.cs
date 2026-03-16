using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class AssistantData
	{
		public List<int> purchaseListQuest;

		public List<int> purchaseListShop;

		public int questAssistantCharaId;

		public int shopAssistantCharaId;
	}
}
