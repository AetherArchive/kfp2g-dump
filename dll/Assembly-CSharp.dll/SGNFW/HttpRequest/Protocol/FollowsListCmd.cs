using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class FollowsListCmd : Command
	{
		private FollowsListCmd()
		{
			this.request = new FollowsListRequest();
			FollowsListRequest followsListRequest = (FollowsListRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "FollowsList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static FollowsListCmd Create()
		{
			return new FollowsListCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<FollowsListResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/FollowsList";
		}
	}
}
