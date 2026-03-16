using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class GachaExecResponse : Response
	{
		public Assets assets;

		public Gacha gacha;

		public List<GachaResult> gacha_result;

		public GachaResult gachatype_omake;

		public List<GachaResult> gachatype_omake_preset;
	}
}
