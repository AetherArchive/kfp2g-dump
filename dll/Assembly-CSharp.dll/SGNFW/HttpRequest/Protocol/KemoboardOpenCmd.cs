using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class KemoboardOpenCmd : Command
	{
		private KemoboardOpenCmd()
		{
		}

		private KemoboardOpenCmd(int panel_id)
		{
			this.request = new KemoboardOpenRequest();
			((KemoboardOpenRequest)this.request).panel_id = panel_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "KemoboardOpen.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static KemoboardOpenCmd Create(int panel_id)
		{
			return new KemoboardOpenCmd(panel_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<KemoboardOpenResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/KemoboardOpen";
		}
	}
}
