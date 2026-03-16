using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PresentGetCmd : Command
	{
		private PresentGetCmd()
		{
		}

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

		public static PresentGetCmd Create(List<long> targetIdList, int rangeLow, int rangeHigh, int histRangeLow, int histRangeHigh)
		{
			return new PresentGetCmd(targetIdList, rangeLow, rangeHigh, histRangeLow, histRangeHigh);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PresentGetResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PresentGet";
		}
	}
}
