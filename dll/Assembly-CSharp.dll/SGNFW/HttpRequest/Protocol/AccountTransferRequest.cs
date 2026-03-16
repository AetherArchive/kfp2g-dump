using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccountTransferRequest : Request
	{
		public string transfer_id;

		public string password;

		public int dmm_viewer_id;

		public string device;
	}
}
