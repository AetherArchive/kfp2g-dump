using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaAccessoryOpenCmd : Command
	{
		private CharaAccessoryOpenCmd()
		{
		}

		private CharaAccessoryOpenCmd(int chara_id)
		{
			this.request = new CharaAccessoryOpenRequest();
			((CharaAccessoryOpenRequest)this.request).chara_id = chara_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CharaAccessoryOpen.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaAccessoryOpenCmd Create(int chara_id)
		{
			return new CharaAccessoryOpenCmd(chara_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaAccessoryOpenResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaAccessoryOpen";
		}
	}
}
