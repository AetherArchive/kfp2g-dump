using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaPpRelCmd : Command
	{
		private CharaPpRelCmd()
		{
		}

		private CharaPpRelCmd(int chara_id, int target_pp_step)
		{
			this.request = new CharaPpRelRequest();
			CharaPpRelRequest charaPpRelRequest = (CharaPpRelRequest)this.request;
			charaPpRelRequest.chara_id = chara_id;
			charaPpRelRequest.target_pp_step = target_pp_step;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CharaPpRel.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaPpRelCmd Create(int chara_id, int target_pp_step)
		{
			return new CharaPpRelCmd(chara_id, target_pp_step);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaPpRelResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaPpRel";
		}
	}
}
