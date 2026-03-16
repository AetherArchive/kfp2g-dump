using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PvPEndResponse : Response
	{
		public Assets assets;

		public PvPResult pvp_result;

		public List<int> pvpspecialReleaseIdList;

		public List<KizunaBonus> kizuna_bonuspoint;
	}
}
