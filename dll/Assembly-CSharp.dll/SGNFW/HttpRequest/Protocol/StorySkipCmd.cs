using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class StorySkipCmd : Command
	{
		private StorySkipCmd()
		{
		}

		private StorySkipCmd(int quest_id, int story_type)
		{
			this.request = new StorySkipRequest();
			StorySkipRequest storySkipRequest = (StorySkipRequest)this.request;
			storySkipRequest.quest_id = quest_id;
			storySkipRequest.story_type = story_type;
			this.Setting();
		}

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

		public static StorySkipCmd Create(int quest_id, int story_type)
		{
			return new StorySkipCmd(quest_id, story_type);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<StorySkipResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/StorySkip";
		}
	}
}
