using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class KizunaConfirmUpdateCmd : Command
	{
		private KizunaConfirmUpdateCmd()
		{
		}

		private KizunaConfirmUpdateCmd(int confirm)
		{
			this.request = new KizunaConfirmUpdateCmdRequest();
			((KizunaConfirmUpdateCmdRequest)this.request).confirm = confirm;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "KizunaConfirmUpdate.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static KizunaConfirmUpdateCmd Create(int confirm)
		{
			return new KizunaConfirmUpdateCmd(confirm);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<KizunaConfirmUpdateCmdResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/KizunaConfirmUpdate";
		}
	}
}
