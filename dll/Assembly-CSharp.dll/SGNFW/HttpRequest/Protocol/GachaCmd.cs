using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class GachaCmd : Command
	{
		private GachaCmd()
		{
			this.request = new GachaRequest();
			GachaRequest gachaRequest = (GachaRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "Gacha.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static GachaCmd Create()
		{
			return new GachaCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<GachaResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/Gacha";
		}
	}
}
