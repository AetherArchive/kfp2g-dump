using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004F1 RID: 1265
	public class QuestReStartCmd : Command
	{
		// Token: 0x06002CDD RID: 11485 RVA: 0x001AF28F File Offset: 0x001AD48F
		private QuestReStartCmd()
		{
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x001AF297 File Offset: 0x001AD497
		private QuestReStartCmd(int restart_type)
		{
			this.request = new QuestReStartRequest();
			((QuestReStartRequest)this.request).restart_type = restart_type;
			this.Setting();
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x001AF2C4 File Offset: 0x001AD4C4
		private void Setting()
		{
			base.Url = "QuestReStart.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x001AF330 File Offset: 0x001AD530
		public static QuestReStartCmd Create(int restart_type)
		{
			return new QuestReStartCmd(restart_type);
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x001AF338 File Offset: 0x001AD538
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestReStartResponse>(__text);
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x001AF340 File Offset: 0x001AD540
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestReStart";
		}
	}
}
