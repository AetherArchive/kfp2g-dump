using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class KemoboardResetCmd : Command
	{
		private KemoboardResetCmd()
		{
		}

		private KemoboardResetCmd(int area_id, int use_stone_flg)
		{
			this.request = new KemoboardResetRequest();
			KemoboardResetRequest kemoboardResetRequest = (KemoboardResetRequest)this.request;
			kemoboardResetRequest.area_id = area_id;
			kemoboardResetRequest.use_stone_flg = use_stone_flg;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "KemoboardReset.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static KemoboardResetCmd Create(int area_id, int use_stone_flg)
		{
			return new KemoboardResetCmd(area_id, use_stone_flg);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<KemoboardResetResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/KemoboardReset";
		}
	}
}
