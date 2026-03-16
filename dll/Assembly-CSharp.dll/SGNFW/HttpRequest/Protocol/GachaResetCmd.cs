using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class GachaResetCmd : Command
	{
		private GachaResetCmd()
		{
		}

		private GachaResetCmd(int gacha_id)
		{
			this.request = new GachaResetRequest();
			((GachaResetRequest)this.request).gacha_id = gacha_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "GachaReset.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static GachaResetCmd Create(int gacha_id)
		{
			return new GachaResetCmd(gacha_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<GachaResetResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/GachaReset";
		}
	}
}
