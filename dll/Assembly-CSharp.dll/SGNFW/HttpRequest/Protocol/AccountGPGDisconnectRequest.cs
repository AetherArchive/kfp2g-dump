using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccountGPGDisconnectRequest : Request
	{
		public string disconnect_transfer_id;
	}
}
