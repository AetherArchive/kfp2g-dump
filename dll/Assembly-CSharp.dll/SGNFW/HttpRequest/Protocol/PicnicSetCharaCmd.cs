using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PicnicSetCharaCmd : Command
	{
		private PicnicSetCharaCmd()
		{
		}

		private PicnicSetCharaCmd(int id, int chara_id)
		{
			this.request = new PicnicSetCharaRequest();
			PicnicSetCharaRequest picnicSetCharaRequest = (PicnicSetCharaRequest)this.request;
			picnicSetCharaRequest.id = id;
			picnicSetCharaRequest.chara_id = chara_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PicnicSetChara.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PicnicSetCharaCmd Create(int id, int chara_id)
		{
			return new PicnicSetCharaCmd(id, chara_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PicnicSetCharaResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PicnicSetChara";
		}
	}
}
