using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000361 RID: 865
	public class CharaArtsUpCmd : Command
	{
		// Token: 0x06002937 RID: 10551 RVA: 0x001A95CB File Offset: 0x001A77CB
		private CharaArtsUpCmd()
		{
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x001A95D3 File Offset: 0x001A77D3
		private CharaArtsUpCmd(int chara_id, int target_arts_level)
		{
			this.request = new CharaArtsUpRequest();
			CharaArtsUpRequest charaArtsUpRequest = (CharaArtsUpRequest)this.request;
			charaArtsUpRequest.chara_id = chara_id;
			charaArtsUpRequest.target_arts_level = target_arts_level;
			this.Setting();
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x001A9604 File Offset: 0x001A7804
		private void Setting()
		{
			base.Url = "CharaArtsUp.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x001A9670 File Offset: 0x001A7870
		public static CharaArtsUpCmd Create(int chara_id, int target_arts_level)
		{
			return new CharaArtsUpCmd(chara_id, target_arts_level);
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x001A9679 File Offset: 0x001A7879
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaArtsUpResponse>(__text);
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x001A9681 File Offset: 0x001A7881
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaArtsUp";
		}
	}
}
