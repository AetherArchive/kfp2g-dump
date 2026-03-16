using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PhotoReleaseCmd : Command
	{
		private PhotoReleaseCmd()
		{
		}

		private PhotoReleaseCmd(List<long> photoIdList)
		{
			this.request = new PhotoReleaseRequest();
			((PhotoReleaseRequest)this.request).photoIdList = photoIdList;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PhotoRelease.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PhotoReleaseCmd Create(List<long> photoIdList)
		{
			return new PhotoReleaseCmd(photoIdList);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PhotoReleaseResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PhotoRelease";
		}
	}
}
