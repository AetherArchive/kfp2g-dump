using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000538 RID: 1336
	public class TimeChangeCmd : Command
	{
		// Token: 0x06002D81 RID: 11649 RVA: 0x001B0280 File Offset: 0x001AE480
		private TimeChangeCmd()
		{
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x001B0288 File Offset: 0x001AE488
		private TimeChangeCmd(string dateTime)
		{
			this.request = new TimeChangeRequest();
			((TimeChangeRequest)this.request).dateTime = dateTime;
			this.Setting();
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x001B02B4 File Offset: 0x001AE4B4
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

		// Token: 0x06002D84 RID: 11652 RVA: 0x001B0320 File Offset: 0x001AE520
		public static TimeChangeCmd Create(string dateTime)
		{
			return new TimeChangeCmd(dateTime);
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x001B0328 File Offset: 0x001AE528
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TimeChangeeResponse>(__text);
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x001B0330 File Offset: 0x001AE530
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TimeChange";
		}
	}
}
