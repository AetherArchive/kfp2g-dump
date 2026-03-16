using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaFavoriteFlagCmd : Command
	{
		private CharaFavoriteFlagCmd()
		{
		}

		private CharaFavoriteFlagCmd(int chara_id)
		{
			this.request = new CharaFavoriteFlagRequest();
			((CharaFavoriteFlagRequest)this.request).chara_id = chara_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CharaFavoriteFlag.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaFavoriteFlagCmd Create(int chara_id)
		{
			return new CharaFavoriteFlagCmd(chara_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaFavoriteFlagResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaFavoriteFlag";
		}
	}
}
