using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccountGPGTransferResponse : Response
	{
		public int result;

		public int friend_code;

		public string account_id;

		public string uuid;

		public string user_name;
	}
}
