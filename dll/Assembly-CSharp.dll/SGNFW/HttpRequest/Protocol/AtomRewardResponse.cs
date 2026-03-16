using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AtomRewardResponse : Response
	{
		public int reward_id;

		public string info_msg;

		public int response_code;

		public string error_msg;
	}
}
