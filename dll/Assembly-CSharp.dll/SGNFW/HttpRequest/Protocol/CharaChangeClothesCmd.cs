using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaChangeClothesCmd : Command
	{
		private CharaChangeClothesCmd()
		{
		}

		private CharaChangeClothesCmd(int chara_id, int clothes_id)
		{
			this.request = new CharaChangeClothesRequest();
			CharaChangeClothesRequest charaChangeClothesRequest = (CharaChangeClothesRequest)this.request;
			charaChangeClothesRequest.chara_id = chara_id;
			charaChangeClothesRequest.clothes_id = clothes_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CharaChangeClothes.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaChangeClothesCmd Create(int chara_id, int clothes_id)
		{
			return new CharaChangeClothesCmd(chara_id, clothes_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaChangeClothesResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaChangeClothes";
		}
	}
}
