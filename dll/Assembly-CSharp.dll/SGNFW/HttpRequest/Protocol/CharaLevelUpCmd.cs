using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000377 RID: 887
	public class CharaLevelUpCmd : Command
	{
		// Token: 0x0600296F RID: 10607 RVA: 0x001A9BAF File Offset: 0x001A7DAF
		private CharaLevelUpCmd()
		{
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x001A9BB7 File Offset: 0x001A7DB7
		private CharaLevelUpCmd(int chara_id, List<UseItem> use_items)
		{
			this.request = new CharaLevelUpRequest();
			CharaLevelUpRequest charaLevelUpRequest = (CharaLevelUpRequest)this.request;
			charaLevelUpRequest.chara_id = chara_id;
			charaLevelUpRequest.use_items = use_items;
			this.Setting();
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x001A9BE8 File Offset: 0x001A7DE8
		private void Setting()
		{
			base.Url = "CharaLevelUp.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x001A9C54 File Offset: 0x001A7E54
		public static CharaLevelUpCmd Create(int chara_id, List<UseItem> use_items)
		{
			return new CharaLevelUpCmd(chara_id, use_items);
		}

		// Token: 0x06002973 RID: 10611 RVA: 0x001A9C5D File Offset: 0x001A7E5D
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaLevelUpResponse>(__text);
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x001A9C65 File Offset: 0x001A7E65
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaLevelUp";
		}
	}
}
