using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class GachaExecCmd : Command
	{
		private GachaExecCmd()
		{
		}

		private GachaExecCmd(int gacha_id, int gacha_type, int my_use_item_id)
		{
			this.request = new GachaExecRequest();
			GachaExecRequest gachaExecRequest = (GachaExecRequest)this.request;
			gachaExecRequest.gacha_id = gacha_id;
			gachaExecRequest.gacha_type = gacha_type;
			gachaExecRequest.my_use_item_id = my_use_item_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "GachaExec.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static GachaExecCmd Create(int gacha_id, int gacha_type, int my_use_item_id)
		{
			return new GachaExecCmd(gacha_id, gacha_type, my_use_item_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<GachaExecResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/GachaExec";
		}
	}
}
