using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class NewFlgUpdateCmd : Command
	{
		private NewFlgUpdateCmd()
		{
		}

		private NewFlgUpdateCmd(List<NewFlg> new_flg_list)
		{
			this.request = new NewFlgUpdateRequest();
			((NewFlgUpdateRequest)this.request).new_flg_list = new_flg_list;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "NewFlgUpdate.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static NewFlgUpdateCmd Create(List<NewFlg> new_flg_list)
		{
			return new NewFlgUpdateCmd(new_flg_list);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<NewFlgUpdateResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/NewFlgUpdate";
		}
	}
}
