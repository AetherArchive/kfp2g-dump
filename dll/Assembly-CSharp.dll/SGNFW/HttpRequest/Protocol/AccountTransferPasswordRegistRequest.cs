using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccountTransferPasswordRegistRequest : Request
	{
		public string transfer_id;

		public string password;

		public string uuid;

		public string secure_id;
	}
}
