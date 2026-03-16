using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MissionListCmd : Command
	{
		private MissionListCmd()
		{
		}

		private MissionListCmd(List<int> mission_types)
		{
			this.request = new MissionListRequest();
			((MissionListRequest)this.request).mission_types = mission_types;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "MissionList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static MissionListCmd Create(List<int> mission_types)
		{
			return new MissionListCmd(mission_types);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MissionListResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MissionList";
		}
	}
}
