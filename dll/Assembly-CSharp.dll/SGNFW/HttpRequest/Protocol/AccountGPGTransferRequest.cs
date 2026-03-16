using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccountGPGTransferRequest : Request
	{
		public string after_transfer_id;

		public string device;
	}
}
