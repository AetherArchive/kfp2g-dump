using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PlayVideoCmd : Command
	{
		private PlayVideoCmd()
		{
		}

		private PlayVideoCmd(int video_id)
		{
			this.request = new PlayVideoRequest();
			((PlayVideoRequest)this.request).video_id = video_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PlayVideo.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PlayVideoCmd Create(int video_id)
		{
			return new PlayVideoCmd(video_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PlayVideoResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PlayVideo";
		}
	}
}
