using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004F9 RID: 1273
	public class QuestSkipCmd : Command
	{
		// Token: 0x06002CEE RID: 11502 RVA: 0x001AF41E File Offset: 0x001AD61E
		private QuestSkipCmd()
		{
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x001AF428 File Offset: 0x001AD628
		private QuestSkipCmd(int quest_id, int deck_id, int friend_id, int helper_chara_id, int skip_num, int kemostatus, List<long> helper_photo_id_list)
		{
			this.request = new QuestSkipRequest();
			QuestSkipRequest questSkipRequest = (QuestSkipRequest)this.request;
			questSkipRequest.quest_id = quest_id;
			questSkipRequest.deck_id = deck_id;
			questSkipRequest.friend_id = friend_id;
			questSkipRequest.helper_chara_id = helper_chara_id;
			questSkipRequest.kemostatus = kemostatus;
			questSkipRequest.skip_num = skip_num;
			questSkipRequest.helper_photo_id_list = helper_photo_id_list;
			this.Setting();
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x001AF48C File Offset: 0x001AD68C
		private void Setting()
		{
			base.Url = "QuestSkip.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x001AF4F8 File Offset: 0x001AD6F8
		public static QuestSkipCmd Create(int quest_id, int deck_id, int friend_id, int helper_chara_id, int skip_num, int kemostatus, List<long> helper_photo_id_list)
		{
			return new QuestSkipCmd(quest_id, deck_id, friend_id, helper_chara_id, skip_num, kemostatus, helper_photo_id_list);
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x001AF509 File Offset: 0x001AD709
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestSkipResponse>(__text);
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x001AF511 File Offset: 0x001AD711
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestSkip";
		}
	}
}
