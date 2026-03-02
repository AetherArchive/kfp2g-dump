using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200049B RID: 1179
	public class PicnicSetCharaCmd : Command
	{
		// Token: 0x06002C08 RID: 11272 RVA: 0x001ADC0F File Offset: 0x001ABE0F
		private PicnicSetCharaCmd()
		{
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x001ADC17 File Offset: 0x001ABE17
		private PicnicSetCharaCmd(int id, int chara_id)
		{
			this.request = new PicnicSetCharaRequest();
			PicnicSetCharaRequest picnicSetCharaRequest = (PicnicSetCharaRequest)this.request;
			picnicSetCharaRequest.id = id;
			picnicSetCharaRequest.chara_id = chara_id;
			this.Setting();
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x001ADC48 File Offset: 0x001ABE48
		private void Setting()
		{
			base.Url = "PicnicSetChara.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x001ADCB4 File Offset: 0x001ABEB4
		public static PicnicSetCharaCmd Create(int id, int chara_id)
		{
			return new PicnicSetCharaCmd(id, chara_id);
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x001ADCBD File Offset: 0x001ABEBD
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PicnicSetCharaResponse>(__text);
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x001ADCC5 File Offset: 0x001ABEC5
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PicnicSetChara";
		}
	}
}
