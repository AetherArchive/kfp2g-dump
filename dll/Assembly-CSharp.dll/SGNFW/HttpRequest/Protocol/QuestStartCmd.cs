using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004FF RID: 1279
	public class QuestStartCmd : Command
	{
		// Token: 0x06002CFE RID: 11518 RVA: 0x001AF5FD File Offset: 0x001AD7FD
		private QuestStartCmd()
		{
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x001AF608 File Offset: 0x001AD808
		private QuestStartCmd(int quest_id, int deck_id, int friend_id, int helper_chara_id, int kemostatus, List<long> photo_id_List)
		{
			this.request = new QuestStartRequest();
			QuestStartRequest questStartRequest = (QuestStartRequest)this.request;
			questStartRequest.quest_id = quest_id;
			questStartRequest.deck_id = deck_id;
			questStartRequest.friend_id = friend_id;
			questStartRequest.helper_chara_id = helper_chara_id;
			questStartRequest.kemostatus = kemostatus;
			questStartRequest.photo_id_List = photo_id_List;
			this.Setting();
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x001AF664 File Offset: 0x001AD864
		private void Setting()
		{
			base.Url = "QuestStart.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002D01 RID: 11521 RVA: 0x001AF6D0 File Offset: 0x001AD8D0
		public static QuestStartCmd Create(int quest_id, int deck_id, int friend_id, int helper_chara_id, int kemostatus, List<long> photo_id_List)
		{
			return new QuestStartCmd(quest_id, deck_id, friend_id, helper_chara_id, kemostatus, photo_id_List);
		}

		// Token: 0x06002D02 RID: 11522 RVA: 0x001AF6DF File Offset: 0x001AD8DF
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestStartResponse>(__text);
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x001AF6E7 File Offset: 0x001AD8E7
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestStart";
		}
	}
}
