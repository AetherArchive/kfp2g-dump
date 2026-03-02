using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004A1 RID: 1185
	public class PicnicSetPlayListCmd : Command
	{
		// Token: 0x06002C18 RID: 11288 RVA: 0x001ADDA3 File Offset: 0x001ABFA3
		private PicnicSetPlayListCmd()
		{
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x001ADDAB File Offset: 0x001ABFAB
		private PicnicSetPlayListCmd(List<int> play_id_list)
		{
			this.request = new PicnicSetPlayListRequest();
			((PicnicSetPlayListRequest)this.request).play_id_list = play_id_list;
			this.Setting();
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x001ADDD8 File Offset: 0x001ABFD8
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

		// Token: 0x06002C1B RID: 11291 RVA: 0x001ADE44 File Offset: 0x001AC044
		public static PicnicSetPlayListCmd Create(List<int> play_id_list)
		{
			return new PicnicSetPlayListCmd(play_id_list);
		}

		// Token: 0x06002C1C RID: 11292 RVA: 0x001ADE4C File Offset: 0x001AC04C
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PicnicSetPlayListResponse>(__text);
		}

		// Token: 0x06002C1D RID: 11293 RVA: 0x001ADE54 File Offset: 0x001AC054
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PicnicSetPlayList";
		}
	}
}
