using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomPublicSettingCmd : Command
	{
		private MasterRoomPublicSettingCmd()
		{
		}

		private MasterRoomPublicSettingCmd(MasterRoomPublicInfo public_info)
		{
			this.request = new MasterRoomPublicSettingRequest();
			((MasterRoomPublicSettingRequest)this.request).public_info = public_info;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "MasterRoomPublicSetting.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static MasterRoomPublicSettingCmd Create(MasterRoomPublicInfo public_info)
		{
			return new MasterRoomPublicSettingCmd(public_info);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomPublicSettingResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomPublicSetting";
		}
	}
}
