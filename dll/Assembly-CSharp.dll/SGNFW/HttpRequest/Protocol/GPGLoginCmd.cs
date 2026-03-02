using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003E8 RID: 1000
	public class GPGLoginCmd : Command
	{
		// Token: 0x06002A71 RID: 10865 RVA: 0x001AB4FC File Offset: 0x001A96FC
		private GPGLoginCmd()
		{
		}

		// Token: 0x06002A72 RID: 10866 RVA: 0x001AB504 File Offset: 0x001A9704
		private GPGLoginCmd(string gpg_player_id, string auth_code)
		{
			this.request = new GPGLoginRequest();
			GPGLoginRequest gpgloginRequest = (GPGLoginRequest)this.request;
			gpgloginRequest.gpg_player_id = gpg_player_id;
			gpgloginRequest.auth_code = auth_code;
			this.Setting();
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x001AB538 File Offset: 0x001A9738
		private void Setting()
		{
			base.Url = "common/GPGLogin.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002A74 RID: 10868 RVA: 0x001AB5A4 File Offset: 0x001A97A4
		public static GPGLoginCmd Create(string gpg_player_id, string auth_code)
		{
			return new GPGLoginCmd(gpg_player_id, auth_code);
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x001AB5AD File Offset: 0x001A97AD
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<GPGLoginResponse>(__text);
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x001AB5B5 File Offset: 0x001A97B5
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/GPGLogin";
		}

		// Token: 0x020010E3 RID: 4323
		public enum RESULT_CODE
		{
			// Token: 0x04005D59 RID: 23897
			INVALID,
			// Token: 0x04005D5A RID: 23898
			SUCCESS_NO_DATA_NO_TRANSFER,
			// Token: 0x04005D5B RID: 23899
			SUCCESS_NO_DATA_YES_TRANSFER,
			// Token: 0x04005D5C RID: 23900
			SUCCESS_YES_DATA_NO_TRANSFER,
			// Token: 0x04005D5D RID: 23901
			SUCCESS_YES_DATA_YES_TRANSFER
		}
	}
}
