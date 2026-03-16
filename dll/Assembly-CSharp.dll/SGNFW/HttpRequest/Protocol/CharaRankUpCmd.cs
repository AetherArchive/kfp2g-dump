using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaRankUpCmd : Command
	{
		private CharaRankUpCmd()
		{
		}

		private CharaRankUpCmd(int chara_id, int target_rank)
		{
			this.request = new CharaRankUpRequest();
			CharaRankUpRequest charaRankUpRequest = (CharaRankUpRequest)this.request;
			charaRankUpRequest.chara_id = chara_id;
			charaRankUpRequest.target_rank = target_rank;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CharaRankUp.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaRankUpCmd Create(int chara_id, int target_rank)
		{
			return new CharaRankUpCmd(chara_id, target_rank);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaRankUpResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaRankUp";
		}
	}
}
