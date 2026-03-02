using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000535 RID: 1333
	public class StorySkipCmd : Command
	{
		// Token: 0x06002D79 RID: 11641 RVA: 0x001B01B1 File Offset: 0x001AE3B1
		private StorySkipCmd()
		{
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x001B01B9 File Offset: 0x001AE3B9
		private StorySkipCmd(int quest_id, int story_type)
		{
			this.request = new StorySkipRequest();
			StorySkipRequest storySkipRequest = (StorySkipRequest)this.request;
			storySkipRequest.quest_id = quest_id;
			storySkipRequest.story_type = story_type;
			this.Setting();
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x001B01EC File Offset: 0x001AE3EC
		private void Setting()
		{
			base.Url = "StorySkip.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x001B0258 File Offset: 0x001AE458
		public static StorySkipCmd Create(int quest_id, int story_type)
		{
			return new StorySkipCmd(quest_id, story_type);
		}

		// Token: 0x06002D7D RID: 11645 RVA: 0x001B0261 File Offset: 0x001AE461
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<StorySkipResponse>(__text);
		}

		// Token: 0x06002D7E RID: 11646 RVA: 0x001B0269 File Offset: 0x001AE469
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/StorySkip";
		}
	}
}
