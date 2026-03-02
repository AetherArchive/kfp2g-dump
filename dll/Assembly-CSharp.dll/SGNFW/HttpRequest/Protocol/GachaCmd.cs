using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003D0 RID: 976
	public class GachaCmd : Command
	{
		// Token: 0x06002A3C RID: 10812 RVA: 0x001AB01B File Offset: 0x001A921B
		private GachaCmd()
		{
			this.request = new GachaRequest();
			GachaRequest gachaRequest = (GachaRequest)this.request;
			this.Setting();
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x001AB040 File Offset: 0x001A9240
		private void Setting()
		{
			base.Url = "Gacha.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x001AB0AC File Offset: 0x001A92AC
		public static GachaCmd Create()
		{
			return new GachaCmd();
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x001AB0B3 File Offset: 0x001A92B3
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<GachaResponse>(__text);
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x001AB0BB File Offset: 0x001A92BB
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/Gacha";
		}
	}
}
