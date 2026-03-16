using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccountTransferResponse : Response
	{
		public int result;

		public string account_id;

		public string uuid;

		public int dmm_data_linked_flg;
	}
}
