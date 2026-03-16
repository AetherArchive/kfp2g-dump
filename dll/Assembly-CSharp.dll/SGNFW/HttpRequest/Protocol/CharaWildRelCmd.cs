using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaWildRelCmd : Command
	{
		private CharaWildRelCmd()
		{
		}

		private CharaWildRelCmd(int chara_id, List<WildResult> promote_request, int is_promoteup_action)
		{
			this.request = new CharaWildRelRequest();
			CharaWildRelRequest charaWildRelRequest = (CharaWildRelRequest)this.request;
			charaWildRelRequest.chara_id = chara_id;
			charaWildRelRequest.promote_request = promote_request;
			charaWildRelRequest.is_promoteup_action = is_promoteup_action;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CharaWildRel.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaWildRelCmd Create(int chara_id, List<WildResult> promote_request, int is_promoteup_action)
		{
			return new CharaWildRelCmd(chara_id, promote_request, is_promoteup_action);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaWildRelResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaWildRel";
		}
	}
}
