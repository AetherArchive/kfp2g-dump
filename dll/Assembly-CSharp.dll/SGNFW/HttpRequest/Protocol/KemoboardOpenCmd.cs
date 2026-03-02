using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000407 RID: 1031
	public class KemoboardOpenCmd : Command
	{
		// Token: 0x06002ABB RID: 10939 RVA: 0x001ABC1B File Offset: 0x001A9E1B
		private KemoboardOpenCmd()
		{
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x001ABC23 File Offset: 0x001A9E23
		private KemoboardOpenCmd(int panel_id)
		{
			this.request = new KemoboardOpenRequest();
			((KemoboardOpenRequest)this.request).panel_id = panel_id;
			this.Setting();
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x001ABC50 File Offset: 0x001A9E50
		private void Setting()
		{
			base.Url = "KemoboardOpen.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x001ABCBC File Offset: 0x001A9EBC
		public static KemoboardOpenCmd Create(int panel_id)
		{
			return new KemoboardOpenCmd(panel_id);
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x001ABCC4 File Offset: 0x001A9EC4
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<KemoboardOpenResponse>(__text);
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x001ABCCC File Offset: 0x001A9ECC
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/KemoboardOpen";
		}
	}
}
