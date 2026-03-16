using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ItemExchangeResponse : Response
	{
		public Assets assets;

		public List<ExchangeExecuteCountInfo> infoList;
	}
}
