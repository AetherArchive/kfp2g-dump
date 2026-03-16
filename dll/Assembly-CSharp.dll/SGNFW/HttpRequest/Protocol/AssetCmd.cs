using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AssetCmd : Command
	{
		private AssetCmd()
		{
			this.request = new AssetRequest();
			AssetRequest assetRequest = (AssetRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "Asset.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static AssetCmd Create()
		{
			return new AssetCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AssetResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/Asset";
		}
	}
}
