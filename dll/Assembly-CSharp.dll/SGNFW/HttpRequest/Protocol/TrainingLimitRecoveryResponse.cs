using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingLimitRecoveryResponse : Response
	{
		public Assets assets;

		public int today_recovery_num;
	}
}
