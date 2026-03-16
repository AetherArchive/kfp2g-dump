using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class DeckListCmd : Command
	{
		private DeckListCmd()
		{
		}

		private DeckListCmd(int deck_type)
		{
			this.request = new DeckListRequest();
			((DeckListRequest)this.request).deck_type = deck_type;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "DeckList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static DeckListCmd Create(int deck_type)
		{
			return new DeckListCmd(deck_type);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<DeckListResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/DeckList";
		}
	}
}
