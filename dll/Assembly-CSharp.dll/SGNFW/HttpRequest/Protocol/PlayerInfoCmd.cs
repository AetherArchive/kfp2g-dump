using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PlayerInfoCmd : Command
	{
		private PlayerInfoCmd()
		{
			this.request = new PlayerInfoRequest();
			PlayerInfoRequest playerInfoRequest = (PlayerInfoRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PlayerInfo.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PlayerInfoCmd Create()
		{
			return new PlayerInfoCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PlayerInfoResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PlayerInfo";
		}
	}
}
