using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class GPGLoginResponse : Response
	{
		public int result;

		public int after_friend_code;

		public string before_user_name;

		public string after_user_name;

		public string after_transfer_id;
	}
}
