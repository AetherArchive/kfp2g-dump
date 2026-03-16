using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaLevelUpCmd : Command
	{
		private CharaLevelUpCmd()
		{
		}

		private CharaLevelUpCmd(int chara_id, List<UseItem> use_items)
		{
			this.request = new CharaLevelUpRequest();
			CharaLevelUpRequest charaLevelUpRequest = (CharaLevelUpRequest)this.request;
			charaLevelUpRequest.chara_id = chara_id;
			charaLevelUpRequest.use_items = use_items;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CharaLevelUp.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaLevelUpCmd Create(int chara_id, List<UseItem> use_items)
		{
			return new CharaLevelUpCmd(chara_id, use_items);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaLevelUpResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaLevelUp";
		}
	}
}
