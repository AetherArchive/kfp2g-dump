using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterSkillGrowCmd : Command
	{
		private MasterSkillGrowCmd()
		{
		}

		private MasterSkillGrowCmd(int master_skill_id, List<UseItem> use_items)
		{
			this.request = new MasterSkillGrowRequest();
			MasterSkillGrowRequest masterSkillGrowRequest = (MasterSkillGrowRequest)this.request;
			masterSkillGrowRequest.master_skill_id = master_skill_id;
			masterSkillGrowRequest.use_items = use_items;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "MasterSkillGrow.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static MasterSkillGrowCmd Create(int master_skill_id, List<UseItem> use_items)
		{
			return new MasterSkillGrowCmd(master_skill_id, use_items);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MasterSkillGrowResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MasterSkillGrow";
		}
	}
}
