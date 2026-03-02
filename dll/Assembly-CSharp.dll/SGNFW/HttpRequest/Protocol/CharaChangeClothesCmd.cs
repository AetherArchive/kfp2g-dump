using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000364 RID: 868
	public class CharaChangeClothesCmd : Command
	{
		// Token: 0x0600293F RID: 10559 RVA: 0x001A9698 File Offset: 0x001A7898
		private CharaChangeClothesCmd()
		{
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x001A96A0 File Offset: 0x001A78A0
		private CharaChangeClothesCmd(int chara_id, int clothes_id)
		{
			this.request = new CharaChangeClothesRequest();
			CharaChangeClothesRequest charaChangeClothesRequest = (CharaChangeClothesRequest)this.request;
			charaChangeClothesRequest.chara_id = chara_id;
			charaChangeClothesRequest.clothes_id = clothes_id;
			this.Setting();
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x001A96D4 File Offset: 0x001A78D4
		private void Setting()
		{
			base.Url = "CharaChangeClothes.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x001A9740 File Offset: 0x001A7940
		public static CharaChangeClothesCmd Create(int chara_id, int clothes_id)
		{
			return new CharaChangeClothesCmd(chara_id, clothes_id);
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x001A9749 File Offset: 0x001A7949
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaChangeClothesResponse>(__text);
		}

		// Token: 0x06002944 RID: 10564 RVA: 0x001A9751 File Offset: 0x001A7951
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaChangeClothes";
		}
	}
}
