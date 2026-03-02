using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000420 RID: 1056
	public class MasterRoomAddFuelCmd : Command
	{
		// Token: 0x06002AF2 RID: 10994 RVA: 0x001AC17E File Offset: 0x001AA37E
		private MasterRoomAddFuelCmd()
		{
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x001AC186 File Offset: 0x001AA386
		private MasterRoomAddFuelCmd(List<UseItem> use_items)
		{
			this.request = new MasterRoomAddFuelRequest();
			((MasterRoomAddFuelRequest)this.request).use_items = use_items;
			this.Setting();
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x001AC1B0 File Offset: 0x001AA3B0
		private void Setting()
		{
			base.Url = "MasterRoomAddFuel.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x001AC21C File Offset: 0x001AA41C
		public static MasterRoomAddFuelCmd Create(List<UseItem> use_items)
		{
			return new MasterRoomAddFuelCmd(use_items);
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x001AC224 File Offset: 0x001AA424
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterRoomAddFuelResponse>(__text);
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x001AC22C File Offset: 0x001AA42C
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterRoomAddFuel";
		}
	}
}
