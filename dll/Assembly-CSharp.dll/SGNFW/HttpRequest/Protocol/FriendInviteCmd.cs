using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class FriendInviteCmd : Command
	{
		private FriendInviteCmd()
		{
		}

		private FriendInviteCmd(string noah_id)
		{
			this.request = new FriendInviteRequest();
			((FriendInviteRequest)this.request).noah_id = noah_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "FriendInvite.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		public static FriendInviteCmd Create(string noah_id)
		{
			return new FriendInviteCmd(noah_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<FriendInviteResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/FriendInvite";
		}
	}
}
