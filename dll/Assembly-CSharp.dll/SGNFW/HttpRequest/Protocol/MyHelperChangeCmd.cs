using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000465 RID: 1125
	public class MyHelperChangeCmd : Command
	{
		// Token: 0x06002B8C RID: 11148 RVA: 0x001AD01F File Offset: 0x001AB21F
		private MyHelperChangeCmd()
		{
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x001AD027 File Offset: 0x001AB227
		private MyHelperChangeCmd(List<MyHelper> helperList)
		{
			this.request = new MyHelperChangeRequest();
			((MyHelperChangeRequest)this.request).helperList = helperList;
			this.Setting();
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x001AD054 File Offset: 0x001AB254
		private void Setting()
		{
			base.Url = "MyHelperChange.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x001AD0C0 File Offset: 0x001AB2C0
		public static MyHelperChangeCmd Create(List<MyHelper> helperList)
		{
			return new MyHelperChangeCmd(helperList);
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x001AD0C8 File Offset: 0x001AB2C8
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MyHelperChangeResponse>(__text);
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x001AD0D0 File Offset: 0x001AB2D0
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MyHelperChange";
		}
	}
}
