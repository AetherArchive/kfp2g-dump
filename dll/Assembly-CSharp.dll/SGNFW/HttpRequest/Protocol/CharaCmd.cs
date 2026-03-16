using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaCmd : Command
	{
		private CharaCmd()
		{
			this.request = new CharaRequest();
			CharaRequest charaRequest = (CharaRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "Chara.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaCmd Create()
		{
			return new CharaCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/Chara";
		}
	}
}
