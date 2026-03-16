using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PresentGetResponse : Response
	{
		public List<UserPresent> userPresentList;

		public List<long> receiveIdList;

		public Assets assets;

		public List<UserReceiveHistory> userReceiveHistoryList;

		public List<GachaResult> gacha_result;
	}
}
