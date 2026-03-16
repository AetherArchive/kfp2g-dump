using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestSkipCmd : Command
	{
		private QuestSkipCmd()
		{
		}

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

		public static QuestSkipCmd Create(int quest_id, int deck_id, int friend_id, int helper_chara_id, int skip_num, int kemostatus, List<long> helper_photo_id_list)
		{
			return new QuestSkipCmd(quest_id, deck_id, friend_id, helper_chara_id, skip_num, kemostatus, helper_photo_id_list);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestSkipResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestSkip";
		}
	}
}
