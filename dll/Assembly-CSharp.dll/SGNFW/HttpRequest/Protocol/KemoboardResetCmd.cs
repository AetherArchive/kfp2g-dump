using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200040A RID: 1034
	public class KemoboardResetCmd : Command
	{
		// Token: 0x06002AC3 RID: 10947 RVA: 0x001ABCE3 File Offset: 0x001A9EE3
		private KemoboardResetCmd()
		{
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x001ABCEB File Offset: 0x001A9EEB
		private KemoboardResetCmd(int area_id, int use_stone_flg)
		{
			this.request = new KemoboardResetRequest();
			KemoboardResetRequest kemoboardResetRequest = (KemoboardResetRequest)this.request;
			kemoboardResetRequest.area_id = area_id;
			kemoboardResetRequest.use_stone_flg = use_stone_flg;
			this.Setting();
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x001ABD1C File Offset: 0x001A9F1C
		private void Setting()
		{
			base.Url = "KemoboardReset.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x001ABD88 File Offset: 0x001A9F88
		public static KemoboardResetCmd Create(int area_id, int use_stone_flg)
		{
			return new KemoboardResetCmd(area_id, use_stone_flg);
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x001ABD91 File Offset: 0x001A9F91
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<KemoboardResetResponse>(__text);
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x001ABD99 File Offset: 0x001A9F99
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/KemoboardReset";
		}
	}
}
