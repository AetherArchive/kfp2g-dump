using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000511 RID: 1297
	public class RegistNoahIdCmd : Command
	{
		// Token: 0x06002D24 RID: 11556 RVA: 0x001AF9A9 File Offset: 0x001ADBA9
		private RegistNoahIdCmd()
		{
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x001AF9B1 File Offset: 0x001ADBB1
		private RegistNoahIdCmd(string noah_id)
		{
			this.request = new RegistNoahIdRequest();
			((RegistNoahIdRequest)this.request).noah_id = noah_id;
			this.Setting();
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x001AF9DC File Offset: 0x001ADBDC
		private void Setting()
		{
			base.Url = "RegistNoahId.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x001AFA48 File Offset: 0x001ADC48
		public static RegistNoahIdCmd Create(string noah_id)
		{
			return new RegistNoahIdCmd(noah_id);
		}

		// Token: 0x06002D28 RID: 11560 RVA: 0x001AFA50 File Offset: 0x001ADC50
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<RegistNoahIdResponse>(__text);
		}

		// Token: 0x06002D29 RID: 11561 RVA: 0x001AFA58 File Offset: 0x001ADC58
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/RegistNoahId";
		}
	}
}
