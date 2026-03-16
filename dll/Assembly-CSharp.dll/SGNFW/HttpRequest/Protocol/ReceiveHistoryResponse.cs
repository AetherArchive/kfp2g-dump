using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ReceiveHistoryResponse : Response
	{
		public List<UserReceiveHistory> userReceiveHistoryList;
	}
}
