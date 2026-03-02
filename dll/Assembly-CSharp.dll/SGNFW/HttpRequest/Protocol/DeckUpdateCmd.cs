using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003B2 RID: 946
	public class DeckUpdateCmd : Command
	{
		// Token: 0x060029FE RID: 10750 RVA: 0x001AAA8F File Offset: 0x001A8C8F
		private DeckUpdateCmd()
		{
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x001AAA97 File Offset: 0x001A8C97
		private DeckUpdateCmd(List<Decks> decks)
		{
			this.request = new DeckUpdateRequest();
			((DeckUpdateRequest)this.request).decks = decks;
			this.Setting();
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x001AAAC4 File Offset: 0x001A8CC4
		private void Setting()
		{
			base.Url = "DeckUpdate.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x001AAB30 File Offset: 0x001A8D30
		public static DeckUpdateCmd Create(List<Decks> decks)
		{
			return new DeckUpdateCmd(decks);
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x001AAB38 File Offset: 0x001A8D38
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<DeckUpdateResponse>(__text);
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x001AAB40 File Offset: 0x001A8D40
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/DeckUpdate";
		}
	}
}
