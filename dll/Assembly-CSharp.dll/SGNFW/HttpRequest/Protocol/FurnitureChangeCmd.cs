using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class FurnitureChangeCmd : Command
	{
		private FurnitureChangeCmd()
		{
		}

		private FurnitureChangeCmd(List<Furniture> furnitures)
		{
			this.request = new FurnitureChangeRequest();
			((FurnitureChangeRequest)this.request).furnitures = furnitures;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "FurnitureChange.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static FurnitureChangeCmd Create(List<Furniture> furnitures)
		{
			return new FurnitureChangeCmd(furnitures);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<FurnitureChangeResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/FurnitureChange";
		}
	}
}
