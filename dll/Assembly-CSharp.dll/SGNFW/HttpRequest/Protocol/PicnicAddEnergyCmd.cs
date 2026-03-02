using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000493 RID: 1171
	public class PicnicAddEnergyCmd : Command
	{
		// Token: 0x06002BF6 RID: 11254 RVA: 0x001ADA72 File Offset: 0x001ABC72
		private PicnicAddEnergyCmd()
		{
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x001ADA7A File Offset: 0x001ABC7A
		private PicnicAddEnergyCmd(int num)
		{
			this.request = new PicnicAddEnergyRequest();
			((PicnicAddEnergyRequest)this.request).num = num;
			this.Setting();
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x001ADAA4 File Offset: 0x001ABCA4
		private void Setting()
		{
			base.Url = "PicnicAddEnergy.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x001ADB10 File Offset: 0x001ABD10
		public static PicnicAddEnergyCmd Create(int num)
		{
			return new PicnicAddEnergyCmd(num);
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x001ADB18 File Offset: 0x001ABD18
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PicnicAddEnergyResponse>(__text);
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x001ADB20 File Offset: 0x001ABD20
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PicnicAddEnergy";
		}
	}
}
