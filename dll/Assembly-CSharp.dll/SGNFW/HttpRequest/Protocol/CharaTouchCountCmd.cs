using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaTouchCountCmd : Command
	{
		private CharaTouchCountCmd()
		{
		}

		private CharaTouchCountCmd(int charaId, int touchNum)
		{
			this.request = new CharaTouchCountRequest();
			CharaTouchCountRequest charaTouchCountRequest = (CharaTouchCountRequest)this.request;
			charaTouchCountRequest.charaId = charaId;
			charaTouchCountRequest.touchNum = touchNum;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CharaTouchCount.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaTouchCountCmd Create(int charaId, int touchNum)
		{
			return new CharaTouchCountCmd(charaId, touchNum);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaTouchCountResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaTouchCount";
		}
	}
}
