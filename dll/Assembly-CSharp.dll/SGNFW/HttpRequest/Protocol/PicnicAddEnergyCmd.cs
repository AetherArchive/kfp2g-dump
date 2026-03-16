using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PicnicAddEnergyCmd : Command
	{
		private PicnicAddEnergyCmd()
		{
		}

		private PicnicAddEnergyCmd(int num)
		{
			this.request = new PicnicAddEnergyRequest();
			((PicnicAddEnergyRequest)this.request).num = num;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PicnicAddEnergy.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PicnicAddEnergyCmd Create(int num)
		{
			return new PicnicAddEnergyCmd(num);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PicnicAddEnergyResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PicnicAddEnergy";
		}
	}
}
