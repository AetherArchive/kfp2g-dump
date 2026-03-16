using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class GPGLoginRequest : Request
	{
		public string gpg_player_id;

		public string auth_code;
	}
}
