using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AtomInviteCmd : Command
	{
		private AtomInviteCmd()
		{
			this.request = new AtomInviteRequest();
			AtomInviteRequest atomInviteRequest = (AtomInviteRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "atom/AtomInvite.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static AtomInviteCmd Create()
		{
			return new AtomInviteCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AtomInviteResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AtomInvite";
		}
	}
}
