using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaNanairoAbilityReleaseCmd : Command
	{
		private CharaNanairoAbilityReleaseCmd()
		{
		}

		private CharaNanairoAbilityReleaseCmd(int chara_id)
		{
			this.request = new CharaNanairoAbilityReleaseRequest();
			((CharaNanairoAbilityReleaseRequest)this.request).chara_id = chara_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CharaNanairoAbilityRelease.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaNanairoAbilityReleaseCmd Create(int chara_id)
		{
			return new CharaNanairoAbilityReleaseCmd(chara_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaNanairoAbilityReleaseResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaNanairoAbilityRelease";
		}
	}
}
