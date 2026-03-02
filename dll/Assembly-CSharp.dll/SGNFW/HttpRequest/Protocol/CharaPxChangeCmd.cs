using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000383 RID: 899
	public class CharaPxChangeCmd : Command
	{
		// Token: 0x0600298F RID: 10639 RVA: 0x001A9EE0 File Offset: 0x001A80E0
		private CharaPxChangeCmd()
		{
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x001A9EE8 File Offset: 0x001A80E8
		private CharaPxChangeCmd(int chara_id, int item_num)
		{
			this.request = new CharaPxChangeRequest();
			CharaPxChangeRequest charaPxChangeRequest = (CharaPxChangeRequest)this.request;
			charaPxChangeRequest.chara_id = chara_id;
			charaPxChangeRequest.item_num = item_num;
			this.Setting();
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x001A9F1C File Offset: 0x001A811C
		private void Setting()
		{
			base.Url = "CharaPxChange.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x001A9F88 File Offset: 0x001A8188
		public static CharaPxChangeCmd Create(int chara_id, int item_num)
		{
			return new CharaPxChangeCmd(chara_id, item_num);
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x001A9F91 File Offset: 0x001A8191
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaPxChangeResponse>(__text);
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x001A9F99 File Offset: 0x001A8199
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaPxChange";
		}
	}
}
