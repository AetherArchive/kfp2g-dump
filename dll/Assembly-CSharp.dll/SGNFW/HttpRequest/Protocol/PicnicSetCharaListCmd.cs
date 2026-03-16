using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PicnicSetCharaListCmd : Command
	{
		private PicnicSetCharaListCmd()
		{
		}

		private PicnicSetCharaListCmd(List<int> chara_id_list)
		{
			this.request = new PicnicSetCharaListRequest();
			((PicnicSetCharaListRequest)this.request).chara_id_list = chara_id_list;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PicnicSetCharaList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PicnicSetCharaListCmd Create(List<int> chara_id_list)
		{
			return new PicnicSetCharaListCmd(chara_id_list);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PicnicSetCharaListResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PicnicSetCharaList";
		}
	}
}
