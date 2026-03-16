using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingEndResponse : Response
	{
		public Assets assets;

		public int reward_id;

		public List<KizunaBonus> kizuna_bonuspoint;
	}
}
