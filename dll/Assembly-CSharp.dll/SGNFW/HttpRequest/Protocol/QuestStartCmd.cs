using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestStartCmd : Command
	{
		private QuestStartCmd()
		{
		}

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

		public static QuestStartCmd Create(int quest_id, int deck_id, int friend_id, int helper_chara_id, int kemostatus, List<long> photo_id_List)
		{
			return new QuestStartCmd(quest_id, deck_id, friend_id, helper_chara_id, kemostatus, photo_id_List);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestStartResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestStart";
		}
	}
}
