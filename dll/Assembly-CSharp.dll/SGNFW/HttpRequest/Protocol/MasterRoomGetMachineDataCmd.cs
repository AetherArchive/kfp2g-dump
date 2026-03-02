using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200042A RID: 1066
	public class MasterRoomGetMachineDataCmd : Command
	{
		// Token: 0x06002B06 RID: 11014 RVA: 0x001AC38B File Offset: 0x001AA58B
		private MasterRoomGetMachineDataCmd()
		{
			this.request = new MasterRoomGetMachineDataRequest();
			MasterRoomGetMachineDataRequest masterRoomGetMachineDataRequest = (MasterRoomGetMachineDataRequest)this.request;
			this.Setting();
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x001AC3B0 File Offset: 0x001AA5B0
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

		// Token: 0x06002B08 RID: 11016 RVA: 0x001AC41C File Offset: 0x001AA61C
		public static MasterRoomGetMachineDataCmd Create()
		{
			return new MasterRoomGetMachineDataCmd();
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x001AC423 File Offset: 0x001AA623
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomGetMachineDataResponse>(__text);
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x001AC42B File Offset: 0x001AA62B
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomGetMachineData";
		}
	}
}
