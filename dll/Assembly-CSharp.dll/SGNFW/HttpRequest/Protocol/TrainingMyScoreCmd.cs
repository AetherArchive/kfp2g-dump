using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000547 RID: 1351
	public class TrainingMyScoreCmd : Command
	{
		// Token: 0x06002DA7 RID: 11687 RVA: 0x001B06F8 File Offset: 0x001AE8F8
		private TrainingMyScoreCmd()
		{
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x001B0700 File Offset: 0x001AE900
		private TrainingMyScoreCmd(int season_id)
		{
			this.request = new TrainingMyScoreRequest();
			((TrainingMyScoreRequest)this.request).season_id = season_id;
			this.Setting();
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x001B072C File Offset: 0x001AE92C
		private void Setting()
		{
			base.Url = "TrainingMyScore.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x001B0798 File Offset: 0x001AE998
		public static TrainingMyScoreCmd Create(int season_id)
		{
			return new TrainingMyScoreCmd(season_id);
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x001B07A0 File Offset: 0x001AE9A0
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TrainingMyScoreResponse>(__text);
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x001B07A8 File Offset: 0x001AE9A8
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TrainingMyScore";
		}
	}
}
