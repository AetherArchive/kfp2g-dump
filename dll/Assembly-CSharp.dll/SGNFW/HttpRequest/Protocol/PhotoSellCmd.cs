using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PhotoSellCmd : Command
	{
		private PhotoSellCmd()
		{
		}

		private PhotoSellCmd(List<long> photo_id)
		{
			this.request = new PhotoSellRequest();
			((PhotoSellRequest)this.request).photo_id = photo_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PhotoSell.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PhotoSellCmd Create(List<long> photo_id)
		{
			return new PhotoSellCmd(photo_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PhotoSellResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PhotoSell";
		}
	}
}
