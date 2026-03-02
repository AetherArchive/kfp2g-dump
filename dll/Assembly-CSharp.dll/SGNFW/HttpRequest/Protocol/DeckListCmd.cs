using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003AE RID: 942
	public class DeckListCmd : Command
	{
		// Token: 0x060029F5 RID: 10741 RVA: 0x001AA9BF File Offset: 0x001A8BBF
		private DeckListCmd()
		{
		}

		// Token: 0x060029F6 RID: 10742 RVA: 0x001AA9C7 File Offset: 0x001A8BC7
		private DeckListCmd(int deck_type)
		{
			this.request = new DeckListRequest();
			((DeckListRequest)this.request).deck_type = deck_type;
			this.Setting();
		}

		// Token: 0x060029F7 RID: 10743 RVA: 0x001AA9F4 File Offset: 0x001A8BF4
		private void Setting()
		{
			base.Url = "DeckList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x060029F8 RID: 10744 RVA: 0x001AAA60 File Offset: 0x001A8C60
		public static DeckListCmd Create(int deck_type)
		{
			return new DeckListCmd(deck_type);
		}

		// Token: 0x060029F9 RID: 10745 RVA: 0x001AAA68 File Offset: 0x001A8C68
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<DeckListResponse>(__text);
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x001AAA70 File Offset: 0x001A8C70
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/DeckList";
		}
	}
}
