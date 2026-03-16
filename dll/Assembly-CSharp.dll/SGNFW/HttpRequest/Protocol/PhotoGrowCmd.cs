using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PhotoGrowCmd : Command
	{
		private PhotoGrowCmd()
		{
		}

		private PhotoGrowCmd(long photo_id, List<long> materials)
		{
			this.request = new PhotoGrowRequest();
			PhotoGrowRequest photoGrowRequest = (PhotoGrowRequest)this.request;
			photoGrowRequest.photo_id = photo_id;
			photoGrowRequest.materials = materials;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PhotoGrow.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PhotoGrowCmd Create(long photo_id, List<long> materials)
		{
			return new PhotoGrowCmd(photo_id, materials);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PhotoGrowResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PhotoGrow";
		}
	}
}
