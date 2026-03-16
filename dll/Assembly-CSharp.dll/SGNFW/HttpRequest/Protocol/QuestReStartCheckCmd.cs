using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestReStartCheckCmd : Command
	{
		private QuestReStartCheckCmd()
		{
		}

		private QuestReStartCheckCmd(long hash_id)
		{
			this.request = new QuestReStartCheckRequest();
			((QuestReStartCheckRequest)this.request).hash_id = hash_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "QuestReStartCheck.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static QuestReStartCheckCmd Create(long hash_id)
		{
			return new QuestReStartCheckCmd(hash_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestReStartCheckResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestReStartCheck";
		}
	}
}
