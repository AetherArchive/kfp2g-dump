using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PicnicUseFoodCmd : Command
	{
		private PicnicUseFoodCmd()
		{
		}

		private PicnicUseFoodCmd(List<int> itemList)
		{
			this.request = new PicnicUseFoodRequest();
			((PicnicUseFoodRequest)this.request).itemList = itemList;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PicnicUseFood.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PicnicUseFoodCmd Create(List<int> itemList)
		{
			return new PicnicUseFoodCmd(itemList);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PicnicUseFoodResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PicnicUseFood";
		}
	}
}
