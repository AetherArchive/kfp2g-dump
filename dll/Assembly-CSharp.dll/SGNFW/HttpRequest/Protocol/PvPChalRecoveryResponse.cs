using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PvPChalRecoveryResponse : Response
	{
		public Assets assets;

		public int limit_chal_count;

		public long last_chal_datetime;
	}
}
