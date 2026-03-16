using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingLimitRecoveryRequest : Request
	{
		public int season_id;

		public int dayofweek;
	}
}
