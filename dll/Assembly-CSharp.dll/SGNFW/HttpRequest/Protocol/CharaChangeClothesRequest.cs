using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaChangeClothesRequest : Request
	{
		public int chara_id;

		public int clothes_id;
	}
}
