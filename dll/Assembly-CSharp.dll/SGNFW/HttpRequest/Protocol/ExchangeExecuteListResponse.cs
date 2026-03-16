using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ExchangeExecuteListResponse : Response
	{
		public List<ExchangeExecuteCountInfo> infoList;
	}
}
