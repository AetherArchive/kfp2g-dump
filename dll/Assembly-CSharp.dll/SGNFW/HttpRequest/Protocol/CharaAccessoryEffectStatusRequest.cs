using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaAccessoryEffectStatusRequest : Request
	{
		public int chara_id;

		public int status;
	}
}
