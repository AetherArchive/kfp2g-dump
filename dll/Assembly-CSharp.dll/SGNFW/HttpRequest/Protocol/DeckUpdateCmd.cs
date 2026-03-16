using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class DeckUpdateCmd : Command
	{
		private DeckUpdateCmd()
		{
		}

		private DeckUpdateCmd(List<Decks> decks)
		{
			this.request = new DeckUpdateRequest();
			((DeckUpdateRequest)this.request).decks = decks;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "DeckUpdate.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static DeckUpdateCmd Create(List<Decks> decks)
		{
			return new DeckUpdateCmd(decks);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<DeckUpdateResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/DeckUpdate";
		}
	}
}
