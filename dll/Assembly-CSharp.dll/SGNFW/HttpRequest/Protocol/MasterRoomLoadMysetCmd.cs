using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomLoadMysetCmd : Command
	{
		private MasterRoomLoadMysetCmd()
		{
		}

		private MasterRoomLoadMysetCmd(int myset_id)
		{
			this.request = new MasterRoomLoadMysetRequest();
			((MasterRoomLoadMysetRequest)this.request).myset_id = myset_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "MasterRoomLoadMyset.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static MasterRoomLoadMysetCmd Create(int myset_id)
		{
			return new MasterRoomLoadMysetCmd(myset_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomLoadMysetResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomLoadMyset";
		}
	}
}
