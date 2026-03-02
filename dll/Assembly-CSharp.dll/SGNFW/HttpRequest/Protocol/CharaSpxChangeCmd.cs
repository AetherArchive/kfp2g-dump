using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200038C RID: 908
	public class CharaSpxChangeCmd : Command
	{
		// Token: 0x060029A7 RID: 10663 RVA: 0x001AA150 File Offset: 0x001A8350
		private CharaSpxChangeCmd()
		{
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x001AA158 File Offset: 0x001A8358
		private CharaSpxChangeCmd(int chara_id, int item_num)
		{
			this.request = new CharaSpxChangeRequest();
			CharaSpxChangeRequest charaSpxChangeRequest = (CharaSpxChangeRequest)this.request;
			charaSpxChangeRequest.chara_id = chara_id;
			charaSpxChangeRequest.item_num = item_num;
			this.Setting();
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x001AA18C File Offset: 0x001A838C
		private void Setting()
		{
			base.Url = "CharaSpxChange.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x060029AA RID: 10666 RVA: 0x001AA1F8 File Offset: 0x001A83F8
		public static CharaSpxChangeCmd Create(int chara_id, int item_num)
		{
			return new CharaSpxChangeCmd(chara_id, item_num);
		}

		// Token: 0x060029AB RID: 10667 RVA: 0x001AA201 File Offset: 0x001A8401
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaSpxChangeResponse>(__text);
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x001AA209 File Offset: 0x001A8409
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaSpxChange";
		}
	}
}
