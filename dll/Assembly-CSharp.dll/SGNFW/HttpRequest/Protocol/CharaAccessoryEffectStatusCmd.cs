using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaAccessoryEffectStatusCmd : Command
	{
		private CharaAccessoryEffectStatusCmd()
		{
		}

		private CharaAccessoryEffectStatusCmd(int chara_id, int status)
		{
			this.request = new CharaAccessoryEffectStatusRequest();
			CharaAccessoryEffectStatusRequest charaAccessoryEffectStatusRequest = (CharaAccessoryEffectStatusRequest)this.request;
			charaAccessoryEffectStatusRequest.chara_id = chara_id;
			charaAccessoryEffectStatusRequest.status = status;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CharaAccessoryEffectStatus.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaAccessoryEffectStatusCmd Create(int chara_id, int status)
		{
			return new CharaAccessoryEffectStatusCmd(chara_id, status);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaAccessoryEffectStatusResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaAccessoryEffectStatus";
		}
	}
}
