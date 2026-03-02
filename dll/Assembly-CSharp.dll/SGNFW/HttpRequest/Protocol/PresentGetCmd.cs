using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004BF RID: 1215
	public class PresentGetCmd : Command
	{
		// Token: 0x06002C61 RID: 11361 RVA: 0x001AE4A0 File Offset: 0x001AC6A0
		private PresentGetCmd()
		{
		}

		// Token: 0x06002C62 RID: 11362 RVA: 0x001AE4A8 File Offset: 0x001AC6A8
		private PresentGetCmd(List<long> targetIdList, int rangeLow, int rangeHigh, int histRangeLow, int histRangeHigh)
		{
			this.request = new PresentGetRequest();
			PresentGetRequest presentGetRequest = (PresentGetRequest)this.request;
			presentGetRequest.targetIdList = targetIdList;
			presentGetRequest.rangeLow = rangeLow;
			presentGetRequest.rangeHigh = rangeHigh;
			presentGetRequest.histRangeLow = histRangeLow;
			presentGetRequest.histRangeHigh = histRangeHigh;
			this.Setting();
		}

		// Token: 0x06002C63 RID: 11363 RVA: 0x001AE4FC File Offset: 0x001AC6FC
		private void Setting()
		{
			base.Url = "PresentGet.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002C64 RID: 11364 RVA: 0x001AE568 File Offset: 0x001AC768
		public static PresentGetCmd Create(List<long> targetIdList, int rangeLow, int rangeHigh, int histRangeLow, int histRangeHigh)
		{
			return new PresentGetCmd(targetIdList, rangeLow, rangeHigh, histRangeLow, histRangeHigh);
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x001AE575 File Offset: 0x001AC775
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PresentGetResponse>(__text);
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x001AE57D File Offset: 0x001AC77D
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PresentGet";
		}
	}
}
