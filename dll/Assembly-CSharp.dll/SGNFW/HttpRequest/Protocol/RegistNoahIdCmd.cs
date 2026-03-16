using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class RegistNoahIdCmd : Command
	{
		private RegistNoahIdCmd()
		{
		}

		private RegistNoahIdCmd(string noah_id)
		{
			this.request = new RegistNoahIdRequest();
			((RegistNoahIdRequest)this.request).noah_id = noah_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "RegistNoahId.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static RegistNoahIdCmd Create(string noah_id)
		{
			return new RegistNoahIdCmd(noah_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<RegistNoahIdResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/RegistNoahId";
		}
	}
}
