using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000443 RID: 1091
	public class MasterRoomSetCharaCmd : Command
	{
		// Token: 0x06002B3D RID: 11069 RVA: 0x001AC8C1 File Offset: 0x001AAAC1
		private MasterRoomSetCharaCmd()
		{
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x001AC8C9 File Offset: 0x001AAAC9
		private MasterRoomSetCharaCmd(List<MasterRoomChara> chara_list)
		{
			this.request = new MasterRoomSetCharaRequest();
			((MasterRoomSetCharaRequest)this.request).chara_list = chara_list;
			this.Setting();
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x001AC8F4 File Offset: 0x001AAAF4
		private void Setting()
		{
			base.Url = "MasterRoomSetChara.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x001AC960 File Offset: 0x001AAB60
		public static MasterRoomSetCharaCmd Create(List<MasterRoomChara> chara_list)
		{
			return new MasterRoomSetCharaCmd(chara_list);
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x001AC968 File Offset: 0x001AAB68
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomSetCharaResponse>(__text);
		}

		// Token: 0x06002B42 RID: 11074 RVA: 0x001AC970 File Offset: 0x001AAB70
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomSetChara";
		}
	}
}
