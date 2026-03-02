using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200049E RID: 1182
	public class PicnicSetCharaListCmd : Command
	{
		// Token: 0x06002C10 RID: 11280 RVA: 0x001ADCDC File Offset: 0x001ABEDC
		private PicnicSetCharaListCmd()
		{
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x001ADCE4 File Offset: 0x001ABEE4
		private PicnicSetCharaListCmd(List<int> chara_id_list)
		{
			this.request = new PicnicSetCharaListRequest();
			((PicnicSetCharaListRequest)this.request).chara_id_list = chara_id_list;
			this.Setting();
		}

		// Token: 0x06002C12 RID: 11282 RVA: 0x001ADD10 File Offset: 0x001ABF10
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

		// Token: 0x06002C13 RID: 11283 RVA: 0x001ADD7C File Offset: 0x001ABF7C
		public static PicnicSetCharaListCmd Create(List<int> chara_id_list)
		{
			return new PicnicSetCharaListCmd(chara_id_list);
		}

		// Token: 0x06002C14 RID: 11284 RVA: 0x001ADD84 File Offset: 0x001ABF84
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PicnicSetCharaListResponse>(__text);
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x001ADD8C File Offset: 0x001ABF8C
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PicnicSetCharaList";
		}
	}
}
