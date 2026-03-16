using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaKizunaLevelUpCmd : Command
	{
		private CharaKizunaLevelUpCmd()
		{
		}

		private CharaKizunaLevelUpCmd(int chara_id, List<UseItem> use_items)
		{
			this.request = new CharaKizunaLevelUpRequest();
			CharaKizunaLevelUpRequest charaKizunaLevelUpRequest = (CharaKizunaLevelUpRequest)this.request;
			charaKizunaLevelUpRequest.chara_id = chara_id;
			charaKizunaLevelUpRequest.use_items = use_items;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CharaKizunaLevelUp.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaKizunaLevelUpCmd Create(int chara_id, List<UseItem> use_items)
		{
			return new CharaKizunaLevelUpCmd(chara_id, use_items);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaKizunaLevelUpResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaKizunaLevelUp";
		}
	}
}
