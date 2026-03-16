using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class SelectGrowthEventCharaIdCmd : Command
	{
		private SelectGrowthEventCharaIdCmd()
		{
		}

		private SelectGrowthEventCharaIdCmd(int event_id, int chara_id)
		{
			this.request = new SelectGrowthEventCharaIdRequest();
			SelectGrowthEventCharaIdRequest selectGrowthEventCharaIdRequest = (SelectGrowthEventCharaIdRequest)this.request;
			selectGrowthEventCharaIdRequest.event_id = event_id;
			selectGrowthEventCharaIdRequest.chara_id = chara_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "SelectGrowthEventCharaId.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static SelectGrowthEventCharaIdCmd Create(int event_id, int chara_id)
		{
			return new SelectGrowthEventCharaIdCmd(event_id, chara_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<SelectGrowthEventCharaIdResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/SelectGrowthEventCharaId";
		}
	}
}
