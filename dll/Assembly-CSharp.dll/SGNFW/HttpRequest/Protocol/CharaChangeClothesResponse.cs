using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaChangeClothesResponse : Response
	{
		public Assets assets;

		public CcResult cc_result;
	}
}
