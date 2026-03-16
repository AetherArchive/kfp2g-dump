using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PracticeConfirmUpdateCmd : Command
	{
		private PracticeConfirmUpdateCmd()
		{
		}

		private PracticeConfirmUpdateCmd(int confirm)
		{
			this.request = new PracticeConfirmUpdateCmdRequest();
			((PracticeConfirmUpdateCmdRequest)this.request).confirm = confirm;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PracticeConfirmUpdate.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PracticeConfirmUpdateCmd Create(int confirm)
		{
			return new PracticeConfirmUpdateCmd(confirm);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PracticeConfirmUpdateCmdResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PracticeConfirmUpdate";
		}
	}
}
