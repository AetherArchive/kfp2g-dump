using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaSelectFaceIconCmd : Command
	{
		private CharaSelectFaceIconCmd()
		{
		}

		private CharaSelectFaceIconCmd(int chara_id, int icon_id)
		{
			this.request = new CharaSelectFaceIconRequest();
			CharaSelectFaceIconRequest charaSelectFaceIconRequest = (CharaSelectFaceIconRequest)this.request;
			charaSelectFaceIconRequest.chara_id = chara_id;
			charaSelectFaceIconRequest.icon_id = icon_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CharaSelectFaceIcon.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaSelectFaceIconCmd Create(int chara_id, int icon_id)
		{
			return new CharaSelectFaceIconCmd(chara_id, icon_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaSelectFaceIconResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaSelectFaceIcon";
		}
	}
}
