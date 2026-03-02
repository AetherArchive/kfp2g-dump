using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000358 RID: 856
	public class CharaAccessoryEffectStatusCmd : Command
	{
		// Token: 0x0600291F RID: 10527 RVA: 0x001A936C File Offset: 0x001A756C
		private CharaAccessoryEffectStatusCmd()
		{
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x001A9374 File Offset: 0x001A7574
		private CharaAccessoryEffectStatusCmd(int chara_id, int status)
		{
			this.request = new CharaAccessoryEffectStatusRequest();
			CharaAccessoryEffectStatusRequest charaAccessoryEffectStatusRequest = (CharaAccessoryEffectStatusRequest)this.request;
			charaAccessoryEffectStatusRequest.chara_id = chara_id;
			charaAccessoryEffectStatusRequest.status = status;
			this.Setting();
		}

		// Token: 0x06002921 RID: 10529 RVA: 0x001A93A8 File Offset: 0x001A75A8
		private void Setting()
		{
			base.Url = "CharaAccessoryEffectStatus.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x001A9414 File Offset: 0x001A7614
		public static CharaAccessoryEffectStatusCmd Create(int chara_id, int status)
		{
			return new CharaAccessoryEffectStatusCmd(chara_id, status);
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x001A941D File Offset: 0x001A761D
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaAccessoryEffectStatusResponse>(__text);
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x001A9425 File Offset: 0x001A7625
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaAccessoryEffectStatus";
		}
	}
}
