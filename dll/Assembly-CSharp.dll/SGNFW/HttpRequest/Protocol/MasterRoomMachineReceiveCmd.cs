using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000431 RID: 1073
	public class MasterRoomMachineReceiveCmd : Command
	{
		// Token: 0x06002B17 RID: 11031 RVA: 0x001AC55F File Offset: 0x001AA75F
		private MasterRoomMachineReceiveCmd()
		{
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x001AC567 File Offset: 0x001AA767
		private MasterRoomMachineReceiveCmd(bool is_all, int furniture_index)
		{
			this.request = new MasterRoomMachineReceiveRequest();
			MasterRoomMachineReceiveRequest masterRoomMachineReceiveRequest = (MasterRoomMachineReceiveRequest)this.request;
			masterRoomMachineReceiveRequest.is_all = is_all;
			masterRoomMachineReceiveRequest.furniture_index = furniture_index;
			this.Setting();
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x001AC598 File Offset: 0x001AA798
		private void Setting()
		{
			base.Url = "MasterRoomMachineReceive.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x001AC604 File Offset: 0x001AA804
		public static MasterRoomMachineReceiveCmd Create(bool is_all, int furniture_indexs)
		{
			return new MasterRoomMachineReceiveCmd(is_all, furniture_indexs);
		}

		// Token: 0x06002B1B RID: 11035 RVA: 0x001AC60D File Offset: 0x001AA80D
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomMachineReceiveResponse>(__text);
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x001AC615 File Offset: 0x001AA815
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomMachineReceive";
		}
	}
}
