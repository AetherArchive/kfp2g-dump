using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PlayerInfoChangeCmd : Command
	{
		private PlayerInfoChangeCmd()
		{
		}

		private PlayerInfoChangeCmd(PlayerInfo playerInfo)
		{
			this.request = new PlayerInfoChangeRequest();
			((PlayerInfoChangeRequest)this.request).playerInfo = playerInfo;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PlayerInfoChange.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PlayerInfoChangeCmd Create(PlayerInfo playerInfo)
		{
			return new PlayerInfoChangeCmd(playerInfo);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PlayerInfoChangeResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PlayerInfoChange";
		}
	}
}
