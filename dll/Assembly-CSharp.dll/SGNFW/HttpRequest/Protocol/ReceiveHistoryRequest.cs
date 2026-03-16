using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ReceiveHistoryRequest : Request
	{
		public int rangeLow;

		public int rangeHigh;
	}
}
