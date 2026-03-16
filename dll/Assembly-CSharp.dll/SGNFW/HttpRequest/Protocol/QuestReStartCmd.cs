using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestReStartCmd : Command
	{
		private QuestReStartCmd()
		{
		}

		private QuestReStartCmd(int restart_type)
		{
			this.request = new QuestReStartRequest();
			((QuestReStartRequest)this.request).restart_type = restart_type;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "QuestReStart.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static QuestReStartCmd Create(int restart_type)
		{
			return new QuestReStartCmd(restart_type);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestReStartResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestReStart";
		}
	}
}
