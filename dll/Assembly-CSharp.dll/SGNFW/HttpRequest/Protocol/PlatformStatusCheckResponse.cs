using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PlatformStatusCheckResponse : Response
	{
		public int phase;

		public int stone_charge_num;

		public string repayment_id;

		public int freeze_flg;

		public long last_check_datetime;
	}
}
