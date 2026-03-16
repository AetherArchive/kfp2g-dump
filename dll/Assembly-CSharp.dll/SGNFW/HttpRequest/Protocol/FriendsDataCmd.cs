using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class FriendsDataCmd : Command
	{
		private FriendsDataCmd()
		{
		}

		private FriendsDataCmd(List<FriendsData> friends_data)
		{
			this.request = new FriendsDataRequest();
			((FriendsDataRequest)this.request).friends_data = friends_data;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "FriendsData.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static FriendsDataCmd Create(List<FriendsData> friends_data)
		{
			return new FriendsDataCmd(friends_data);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<FriendsDataResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/FriendsData";
		}
	}
}
