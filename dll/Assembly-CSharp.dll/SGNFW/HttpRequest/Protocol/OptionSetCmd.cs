using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000476 RID: 1142
	public class OptionSetCmd : Command
	{
		// Token: 0x06002BB4 RID: 11188 RVA: 0x001AD3F2 File Offset: 0x001AB5F2
		private OptionSetCmd()
		{
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x001AD3FA File Offset: 0x001AB5FA
		private OptionSetCmd(List<int> optionList)
		{
			this.request = new OptionSetRequest();
			((OptionSetRequest)this.request).optionList = optionList;
			this.Setting();
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x001AD424 File Offset: 0x001AB624
		private void Setting()
		{
			base.Url = "OptionSet.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x001AD490 File Offset: 0x001AB690
		public static OptionSetCmd Create(List<int> optionList)
		{
			return new OptionSetCmd(optionList);
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x001AD498 File Offset: 0x001AB698
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<OptionSetResponse>(__text);
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x001AD4A0 File Offset: 0x001AB6A0
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/OptionSet";
		}
	}
}
