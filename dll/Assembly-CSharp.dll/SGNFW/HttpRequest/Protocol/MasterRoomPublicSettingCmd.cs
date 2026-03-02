using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000438 RID: 1080
	public class MasterRoomPublicSettingCmd : Command
	{
		// Token: 0x06002B23 RID: 11043 RVA: 0x001AC64C File Offset: 0x001AA84C
		private MasterRoomPublicSettingCmd()
		{
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x001AC654 File Offset: 0x001AA854
		private MasterRoomPublicSettingCmd(MasterRoomPublicInfo public_info)
		{
			this.request = new MasterRoomPublicSettingRequest();
			((MasterRoomPublicSettingRequest)this.request).public_info = public_info;
			this.Setting();
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x001AC680 File Offset: 0x001AA880
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

		// Token: 0x06002B26 RID: 11046 RVA: 0x001AC6EC File Offset: 0x001AA8EC
		public static MasterRoomPublicSettingCmd Create(MasterRoomPublicInfo public_info)
		{
			return new MasterRoomPublicSettingCmd(public_info);
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x001AC6F4 File Offset: 0x001AA8F4
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomPublicSettingResponse>(__text);
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x001AC6FC File Offset: 0x001AA8FC
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomPublicSetting";
		}
	}
}
