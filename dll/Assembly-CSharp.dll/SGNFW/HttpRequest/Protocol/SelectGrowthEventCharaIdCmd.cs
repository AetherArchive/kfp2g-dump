using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200051E RID: 1310
	public class SelectGrowthEventCharaIdCmd : Command
	{
		// Token: 0x06002D40 RID: 11584 RVA: 0x001AFC1F File Offset: 0x001ADE1F
		private SelectGrowthEventCharaIdCmd()
		{
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x001AFC27 File Offset: 0x001ADE27
		private SelectGrowthEventCharaIdCmd(int event_id, int chara_id)
		{
			this.request = new SelectGrowthEventCharaIdRequest();
			SelectGrowthEventCharaIdRequest selectGrowthEventCharaIdRequest = (SelectGrowthEventCharaIdRequest)this.request;
			selectGrowthEventCharaIdRequest.event_id = event_id;
			selectGrowthEventCharaIdRequest.chara_id = chara_id;
			this.Setting();
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x001AFC58 File Offset: 0x001ADE58
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

		// Token: 0x06002D43 RID: 11587 RVA: 0x001AFCC4 File Offset: 0x001ADEC4
		public static SelectGrowthEventCharaIdCmd Create(int event_id, int chara_id)
		{
			return new SelectGrowthEventCharaIdCmd(event_id, chara_id);
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x001AFCCD File Offset: 0x001ADECD
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<SelectGrowthEventCharaIdResponse>(__text);
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x001AFCD5 File Offset: 0x001ADED5
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/SelectGrowthEventCharaId";
		}
	}
}
