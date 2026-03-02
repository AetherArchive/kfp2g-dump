using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000446 RID: 1094
	public class MasterRoomSetFurnitureCmd : Command
	{
		// Token: 0x06002B45 RID: 11077 RVA: 0x001AC987 File Offset: 0x001AAB87
		private MasterRoomSetFurnitureCmd()
		{
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x001AC98F File Offset: 0x001AAB8F
		private MasterRoomSetFurnitureCmd(List<MasterRoomFurniture> furniture_list)
		{
			this.request = new MasterRoomSetFurnitureRequest();
			((MasterRoomSetFurnitureRequest)this.request).furniture_list = furniture_list;
			this.Setting();
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x001AC9BC File Offset: 0x001AABBC
		private void Setting()
		{
			base.Url = "MasterRoomSetFurniture.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x001ACA28 File Offset: 0x001AAC28
		public static MasterRoomSetFurnitureCmd Create(List<MasterRoomFurniture> furniture_list)
		{
			return new MasterRoomSetFurnitureCmd(furniture_list);
		}

		// Token: 0x06002B49 RID: 11081 RVA: 0x001ACA30 File Offset: 0x001AAC30
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomSetFurnitureResponse>(__text);
		}

		// Token: 0x06002B4A RID: 11082 RVA: 0x001ACA38 File Offset: 0x001AAC38
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomSetFurniture";
		}
	}
}
