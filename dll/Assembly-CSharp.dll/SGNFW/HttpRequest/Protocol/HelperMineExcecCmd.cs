using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class HelperMineExcecCmd : Command
	{
		private HelperMineExcecCmd()
		{
		}

		private HelperMineExcecCmd(int action_type, List<int> target_friend_id_list)
		{
			this.request = new HelperMineExcecRequest();
			HelperMineExcecRequest helperMineExcecRequest = (HelperMineExcecRequest)this.request;
			helperMineExcecRequest.action_type = action_type;
			helperMineExcecRequest.target_friend_id_list = target_friend_id_list;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "HelperMineExcec.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static HelperMineExcecCmd Create(int action_type, List<int> target_friend_id_list)
		{
			return new HelperMineExcecCmd(action_type, target_friend_id_list);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<HelperMineExcecResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/HelperMineExcec";
		}
	}
}
