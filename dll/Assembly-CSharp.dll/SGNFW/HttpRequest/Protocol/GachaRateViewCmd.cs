using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class GachaRateViewCmd : Command
	{
		private GachaRateViewCmd()
		{
		}

		private GachaRateViewCmd(int gacha_id)
		{
			this.request = new GachaRateViewRequest();
			((GachaRateViewRequest)this.request).gacha_id = gacha_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "GachaRateView.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static GachaRateViewCmd Create(int gacha_id)
		{
			return new GachaRateViewCmd(gacha_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<GachaRateViewResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/GachaRateView";
		}
	}
}
