using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PlayerInfoResponse : Response
	{
		public Assets assets;

		public PlayerInfo playerInfo;
	}
}
