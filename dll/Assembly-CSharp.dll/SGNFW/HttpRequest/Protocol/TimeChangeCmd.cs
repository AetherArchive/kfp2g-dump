using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TimeChangeCmd : Command
	{
		private TimeChangeCmd()
		{
		}

		private TimeChangeCmd(string dateTime)
		{
			this.request = new TimeChangeRequest();
			((TimeChangeRequest)this.request).dateTime = dateTime;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "TimeChange.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static TimeChangeCmd Create(string dateTime)
		{
			return new TimeChangeCmd(dateTime);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TimeChangeeResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TimeChange";
		}
	}
}
