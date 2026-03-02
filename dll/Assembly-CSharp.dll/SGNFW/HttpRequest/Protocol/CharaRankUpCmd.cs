using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000386 RID: 902
	public class CharaRankUpCmd : Command
	{
		// Token: 0x06002997 RID: 10647 RVA: 0x001A9FB0 File Offset: 0x001A81B0
		private CharaRankUpCmd()
		{
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x001A9FB8 File Offset: 0x001A81B8
		private CharaRankUpCmd(int chara_id, int target_rank)
		{
			this.request = new CharaRankUpRequest();
			CharaRankUpRequest charaRankUpRequest = (CharaRankUpRequest)this.request;
			charaRankUpRequest.chara_id = chara_id;
			charaRankUpRequest.target_rank = target_rank;
			this.Setting();
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x001A9FEC File Offset: 0x001A81EC
		private void Setting()
		{
			base.Url = "CharaRankUp.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x0600299A RID: 10650 RVA: 0x001AA058 File Offset: 0x001A8258
		public static CharaRankUpCmd Create(int chara_id, int target_rank)
		{
			return new CharaRankUpCmd(chara_id, target_rank);
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x001AA061 File Offset: 0x001A8261
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaRankUpResponse>(__text);
		}

		// Token: 0x0600299C RID: 10652 RVA: 0x001AA069 File Offset: 0x001A8269
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaRankUp";
		}
	}
}
