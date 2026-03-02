using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200038F RID: 911
	public class CharaTouchCountCmd : Command
	{
		// Token: 0x060029AF RID: 10671 RVA: 0x001AA220 File Offset: 0x001A8420
		private CharaTouchCountCmd()
		{
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x001AA228 File Offset: 0x001A8428
		private CharaTouchCountCmd(int charaId, int touchNum)
		{
			this.request = new CharaTouchCountRequest();
			CharaTouchCountRequest charaTouchCountRequest = (CharaTouchCountRequest)this.request;
			charaTouchCountRequest.charaId = charaId;
			charaTouchCountRequest.touchNum = touchNum;
			this.Setting();
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x001AA25C File Offset: 0x001A845C
		private void Setting()
		{
			base.Url = "CharaTouchCount.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x001AA2C8 File Offset: 0x001A84C8
		public static CharaTouchCountCmd Create(int charaId, int touchNum)
		{
			return new CharaTouchCountCmd(charaId, touchNum);
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x001AA2D1 File Offset: 0x001A84D1
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaTouchCountResponse>(__text);
		}

		// Token: 0x060029B4 RID: 10676 RVA: 0x001AA2D9 File Offset: 0x001A84D9
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaTouchCount";
		}
	}
}
