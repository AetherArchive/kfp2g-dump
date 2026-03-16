using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestSealedCharaCmd : Command
	{
		private QuestSealedCharaCmd()
		{
			this.request = new QuestSealedCharaRequest();
			QuestSealedCharaRequest questSealedCharaRequest = (QuestSealedCharaRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "QuestSealedChara.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static QuestSealedCharaCmd Create()
		{
			return new QuestSealedCharaCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestSealedCharaResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestSealedChara";
		}
	}
}
