using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003D3 RID: 979
	public class GachaExecCmd : Command
	{
		// Token: 0x06002A43 RID: 10819 RVA: 0x001AB0D2 File Offset: 0x001A92D2
		private GachaExecCmd()
		{
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x001AB0DA File Offset: 0x001A92DA
		private GachaExecCmd(int gacha_id, int gacha_type, int my_use_item_id)
		{
			this.request = new GachaExecRequest();
			GachaExecRequest gachaExecRequest = (GachaExecRequest)this.request;
			gachaExecRequest.gacha_id = gacha_id;
			gachaExecRequest.gacha_type = gacha_type;
			gachaExecRequest.my_use_item_id = my_use_item_id;
			this.Setting();
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x001AB114 File Offset: 0x001A9314
		private void Setting()
		{
			base.Url = "GachaExec.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x001AB180 File Offset: 0x001A9380
		public static GachaExecCmd Create(int gacha_id, int gacha_type, int my_use_item_id)
		{
			return new GachaExecCmd(gacha_id, gacha_type, my_use_item_id);
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x001AB18A File Offset: 0x001A938A
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<GachaExecResponse>(__text);
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x001AB192 File Offset: 0x001A9392
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/GachaExec";
		}
	}
}
