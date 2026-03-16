using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CollaboURLCmd : Command
	{
		private CollaboURLCmd()
		{
			this.request = new CollaboURLRequest();
			CollaboURLRequest collaboURLRequest = (CollaboURLRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CollaboUrl.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		public static CollaboURLCmd Create()
		{
			return new CollaboURLCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CollaboURLResponce>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CollaboURL";
		}
	}
}
