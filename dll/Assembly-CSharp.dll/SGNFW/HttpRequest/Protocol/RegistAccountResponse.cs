using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class RegistAccountResponse : Response
	{
		public string account_id;

		public string uuid;

		public string transfer_id;
	}
}
