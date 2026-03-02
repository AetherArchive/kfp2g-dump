using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003B8 RID: 952
	public class ExchangeExecuteListCmd : Command
	{
		// Token: 0x06002A09 RID: 10761 RVA: 0x001AAB6F File Offset: 0x001A8D6F
		private ExchangeExecuteListCmd()
		{
			this.request = new ExchangeExecuteListRequest();
			ExchangeExecuteListRequest exchangeExecuteListRequest = (ExchangeExecuteListRequest)this.request;
			this.Setting();
		}

		// Token: 0x06002A0A RID: 10762 RVA: 0x001AAB94 File Offset: 0x001A8D94
		private void Setting()
		{
			base.Url = "ExchangeExecuteList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x001AAC00 File Offset: 0x001A8E00
		public static ExchangeExecuteListCmd Create()
		{
			return new ExchangeExecuteListCmd();
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x001AAC07 File Offset: 0x001A8E07
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ExchangeExecuteListResponse>(__text);
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x001AAC0F File Offset: 0x001A8E0F
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ExchangeExecuteList";
		}
	}
}
