using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class HelperSearchCmd : Command
	{
		private HelperSearchCmd()
		{
		}

		private HelperSearchCmd(int target_friend_id)
		{
			this.request = new HelperSearchRequest();
			((HelperSearchRequest)this.request).target_friend_id = target_friend_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "HelperSearch.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static HelperSearchCmd Create(int target_friend_id)
		{
			return new HelperSearchCmd(target_friend_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<HelperSearchResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/HelperSearch";
		}
	}
}
