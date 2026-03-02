using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000389 RID: 905
	public class CharaSelectFaceIconCmd : Command
	{
		// Token: 0x0600299F RID: 10655 RVA: 0x001AA080 File Offset: 0x001A8280
		private CharaSelectFaceIconCmd()
		{
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x001AA088 File Offset: 0x001A8288
		private CharaSelectFaceIconCmd(int chara_id, int icon_id)
		{
			this.request = new CharaSelectFaceIconRequest();
			CharaSelectFaceIconRequest charaSelectFaceIconRequest = (CharaSelectFaceIconRequest)this.request;
			charaSelectFaceIconRequest.chara_id = chara_id;
			charaSelectFaceIconRequest.icon_id = icon_id;
			this.Setting();
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x001AA0BC File Offset: 0x001A82BC
		private void Setting()
		{
			base.Url = "CharaSelectFaceIcon.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x001AA128 File Offset: 0x001A8328
		public static CharaSelectFaceIconCmd Create(int chara_id, int icon_id)
		{
			return new CharaSelectFaceIconCmd(chara_id, icon_id);
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x001AA131 File Offset: 0x001A8331
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaSelectFaceIconResponse>(__text);
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x001AA139 File Offset: 0x001A8339
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaSelectFaceIcon";
		}
	}
}
