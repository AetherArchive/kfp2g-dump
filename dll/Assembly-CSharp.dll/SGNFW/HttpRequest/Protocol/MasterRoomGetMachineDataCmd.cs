using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomGetMachineDataCmd : Command
	{
		private MasterRoomGetMachineDataCmd()
		{
			this.request = new MasterRoomGetMachineDataRequest();
			MasterRoomGetMachineDataRequest masterRoomGetMachineDataRequest = (MasterRoomGetMachineDataRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "MasterRoomGetMachineData.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static MasterRoomGetMachineDataCmd Create()
		{
			return new MasterRoomGetMachineDataCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomGetMachineDataResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomGetMachineData";
		}
	}
}
