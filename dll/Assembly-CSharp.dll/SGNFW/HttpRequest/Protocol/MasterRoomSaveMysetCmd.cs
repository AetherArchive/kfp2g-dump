using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200043D RID: 1085
	public class MasterRoomSaveMysetCmd : Command
	{
		// Token: 0x06002B2D RID: 11053 RVA: 0x001AC723 File Offset: 0x001AA923
		private MasterRoomSaveMysetCmd()
		{
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x001AC72B File Offset: 0x001AA92B
		private MasterRoomSaveMysetCmd(int myset_id)
		{
			this.request = new MasterRoomSaveMysetRequest();
			((MasterRoomSaveMysetRequest)this.request).myset_id = myset_id;
			this.Setting();
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x001AC758 File Offset: 0x001AA958
		private void Setting()
		{
			base.Url = "MasterRoomSaveMyset.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x001AC7C4 File Offset: 0x001AA9C4
		public static MasterRoomSaveMysetCmd Create(int myset_id)
		{
			return new MasterRoomSaveMysetCmd(myset_id);
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x001AC7CC File Offset: 0x001AA9CC
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomSaveMysetResponse>(__text);
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x001AC7D4 File Offset: 0x001AA9D4
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomSaveMyset";
		}
	}
}
