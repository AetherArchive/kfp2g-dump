using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaArtsUpCmd : Command
	{
		private CharaArtsUpCmd()
		{
		}

		private CharaArtsUpCmd(int chara_id, int target_arts_level)
		{
			this.request = new CharaArtsUpRequest();
			CharaArtsUpRequest charaArtsUpRequest = (CharaArtsUpRequest)this.request;
			charaArtsUpRequest.chara_id = chara_id;
			charaArtsUpRequest.target_arts_level = target_arts_level;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CharaArtsUp.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaArtsUpCmd Create(int chara_id, int target_arts_level)
		{
			return new CharaArtsUpCmd(chara_id, target_arts_level);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaArtsUpResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaArtsUp";
		}
	}
}
