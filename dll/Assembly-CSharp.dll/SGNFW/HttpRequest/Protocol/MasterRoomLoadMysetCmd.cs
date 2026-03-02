using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200042D RID: 1069
	public class MasterRoomLoadMysetCmd : Command
	{
		// Token: 0x06002B0D RID: 11021 RVA: 0x001AC442 File Offset: 0x001AA642
		private MasterRoomLoadMysetCmd()
		{
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x001AC44A File Offset: 0x001AA64A
		private MasterRoomLoadMysetCmd(int myset_id)
		{
			this.request = new MasterRoomLoadMysetRequest();
			((MasterRoomLoadMysetRequest)this.request).myset_id = myset_id;
			this.Setting();
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x001AC474 File Offset: 0x001AA674
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

		// Token: 0x06002B10 RID: 11024 RVA: 0x001AC4E0 File Offset: 0x001AA6E0
		public static MasterRoomLoadMysetCmd Create(int myset_id)
		{
			return new MasterRoomLoadMysetCmd(myset_id);
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x001AC4E8 File Offset: 0x001AA6E8
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomLoadMysetResponse>(__text);
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x001AC4F0 File Offset: 0x001AA6F0
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomLoadMyset";
		}
	}
}
