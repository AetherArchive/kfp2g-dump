using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaPxChangeCmd : Command
	{
		private CharaPxChangeCmd()
		{
		}

		private CharaPxChangeCmd(int chara_id, int item_num)
		{
			this.request = new CharaPxChangeRequest();
			CharaPxChangeRequest charaPxChangeRequest = (CharaPxChangeRequest)this.request;
			charaPxChangeRequest.chara_id = chara_id;
			charaPxChangeRequest.item_num = item_num;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CharaPxChange.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaPxChangeCmd Create(int chara_id, int item_num)
		{
			return new CharaPxChangeCmd(chara_id, item_num);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaPxChangeResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaPxChange";
		}
	}
}
