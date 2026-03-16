using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AchievementNewAcquisitionCmd : Command
	{
		private AchievementNewAcquisitionCmd()
		{
			this.request = new AchievementNewAcquisitionRequest();
			AchievementNewAcquisitionRequest achievementNewAcquisitionRequest = (AchievementNewAcquisitionRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "AchievementNewAcquisition.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static AchievementNewAcquisitionCmd Create()
		{
			return new AchievementNewAcquisitionCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AchievementNewAcquisitionResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AchievementNewAcquisition";
		}
	}
}
