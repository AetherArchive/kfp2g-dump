using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000370 RID: 880
	public class CharaKizunaLevelUpCmd : Command
	{
		// Token: 0x0600295E RID: 10590 RVA: 0x001A9A10 File Offset: 0x001A7C10
		private CharaKizunaLevelUpCmd()
		{
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x001A9A18 File Offset: 0x001A7C18
		private CharaKizunaLevelUpCmd(int chara_id, List<UseItem> use_items)
		{
			this.request = new CharaKizunaLevelUpRequest();
			CharaKizunaLevelUpRequest charaKizunaLevelUpRequest = (CharaKizunaLevelUpRequest)this.request;
			charaKizunaLevelUpRequest.chara_id = chara_id;
			charaKizunaLevelUpRequest.use_items = use_items;
			this.Setting();
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x001A9A4C File Offset: 0x001A7C4C
		private void Setting()
		{
			base.Url = "CharaKizunaLevelUp.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x001A9AB8 File Offset: 0x001A7CB8
		public static CharaKizunaLevelUpCmd Create(int chara_id, List<UseItem> use_items)
		{
			return new CharaKizunaLevelUpCmd(chara_id, use_items);
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x001A9AC1 File Offset: 0x001A7CC1
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaKizunaLevelUpResponse>(__text);
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x001A9AC9 File Offset: 0x001A7CC9
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaKizunaLevelUp";
		}
	}
}
