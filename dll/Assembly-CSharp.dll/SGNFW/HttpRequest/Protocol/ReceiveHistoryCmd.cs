using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000508 RID: 1288
	public class ReceiveHistoryCmd : Command
	{
		// Token: 0x06002D0C RID: 11532 RVA: 0x001AF72E File Offset: 0x001AD92E
		private ReceiveHistoryCmd()
		{
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x001AF736 File Offset: 0x001AD936
		private ReceiveHistoryCmd(int rangeLow, int rangeHigh)
		{
			this.request = new ReceiveHistoryRequest();
			ReceiveHistoryRequest receiveHistoryRequest = (ReceiveHistoryRequest)this.request;
			receiveHistoryRequest.rangeLow = rangeLow;
			receiveHistoryRequest.rangeHigh = rangeHigh;
			this.Setting();
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x001AF768 File Offset: 0x001AD968
		private void Setting()
		{
			base.Url = "ReceiveHistory.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x001AF7D4 File Offset: 0x001AD9D4
		public static ReceiveHistoryCmd Create(int rangeLow, int rangeHigh)
		{
			return new ReceiveHistoryCmd(rangeLow, rangeHigh);
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x001AF7DD File Offset: 0x001AD9DD
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ReceiveHistoryResponse>(__text);
		}

		// Token: 0x06002D11 RID: 11537 RVA: 0x001AF7E5 File Offset: 0x001AD9E5
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ReceiveHistory";
		}
	}
}
