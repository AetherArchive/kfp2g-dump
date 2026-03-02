using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200035E RID: 862
	public class CharaAccessoryOpenCmd : Command
	{
		// Token: 0x0600292F RID: 10543 RVA: 0x001A9503 File Offset: 0x001A7703
		private CharaAccessoryOpenCmd()
		{
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x001A950B File Offset: 0x001A770B
		private CharaAccessoryOpenCmd(int chara_id)
		{
			this.request = new CharaAccessoryOpenRequest();
			((CharaAccessoryOpenRequest)this.request).chara_id = chara_id;
			this.Setting();
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x001A9538 File Offset: 0x001A7738
		private void Setting()
		{
			base.Url = "CharaAccessoryOpen.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x001A95A4 File Offset: 0x001A77A4
		public static CharaAccessoryOpenCmd Create(int chara_id)
		{
			return new CharaAccessoryOpenCmd(chara_id);
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x001A95AC File Offset: 0x001A77AC
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaAccessoryOpenResponse>(__text);
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x001A95B4 File Offset: 0x001A77B4
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaAccessoryOpen";
		}
	}
}
