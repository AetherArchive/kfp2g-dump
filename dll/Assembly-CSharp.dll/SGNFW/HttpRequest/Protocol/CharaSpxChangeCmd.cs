using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaSpxChangeCmd : Command
	{
		private CharaSpxChangeCmd()
		{
		}

		private CharaSpxChangeCmd(int chara_id, int item_num)
		{
			this.request = new CharaSpxChangeRequest();
			CharaSpxChangeRequest charaSpxChangeRequest = (CharaSpxChangeRequest)this.request;
			charaSpxChangeRequest.chara_id = chara_id;
			charaSpxChangeRequest.item_num = item_num;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CharaSpxChange.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaSpxChangeCmd Create(int chara_id, int item_num)
		{
			return new CharaSpxChangeCmd(chara_id, item_num);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaSpxChangeResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaSpxChange";
		}
	}
}
