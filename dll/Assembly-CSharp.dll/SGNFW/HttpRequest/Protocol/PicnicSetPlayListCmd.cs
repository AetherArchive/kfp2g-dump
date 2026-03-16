using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PicnicSetPlayListCmd : Command
	{
		private PicnicSetPlayListCmd()
		{
		}

		private PicnicSetPlayListCmd(List<int> play_id_list)
		{
			this.request = new PicnicSetPlayListRequest();
			((PicnicSetPlayListRequest)this.request).play_id_list = play_id_list;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PicnicSetPlayList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PicnicSetPlayListCmd Create(List<int> play_id_list)
		{
			return new PicnicSetPlayListCmd(play_id_list);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PicnicSetPlayListResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PicnicSetPlayList";
		}
	}
}
