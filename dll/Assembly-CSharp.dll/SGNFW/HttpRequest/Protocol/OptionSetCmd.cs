using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class OptionSetCmd : Command
	{
		private OptionSetCmd()
		{
		}

		private OptionSetCmd(List<int> optionList)
		{
			this.request = new OptionSetRequest();
			((OptionSetRequest)this.request).optionList = optionList;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "OptionSet.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static OptionSetCmd Create(List<int> optionList)
		{
			return new OptionSetCmd(optionList);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<OptionSetResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/OptionSet";
		}
	}
}
