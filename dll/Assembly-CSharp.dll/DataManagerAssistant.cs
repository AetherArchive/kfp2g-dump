using System;
using System.Collections.Generic;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x0200006E RID: 110
public class DataManagerAssistant
{
	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x06000315 RID: 789 RVA: 0x00017B2A File Offset: 0x00015D2A
	public DataManagerAssistant.UserAssistantData UserData
	{
		get
		{
			return this.userData;
		}
	}

	// Token: 0x06000316 RID: 790 RVA: 0x00017B32 File Offset: 0x00015D32
	public DataManagerAssistant(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x06000317 RID: 791 RVA: 0x00017B41 File Offset: 0x00015D41
	public void InitializeMstData(MstManager mst)
	{
		this.mstAssistantList = mst.GetMst<List<MstAssistantData>>(MstType.ASSISTANT_DATA);
	}

	// Token: 0x06000318 RID: 792 RVA: 0x00017B54 File Offset: 0x00015D54
	public void UpdateAssistantDataByServer(AssistantData assistantData)
	{
		this.userData = new DataManagerAssistant.UserAssistantData(assistantData);
	}

	// Token: 0x06000319 RID: 793 RVA: 0x00017B62 File Offset: 0x00015D62
	public List<MstAssistantData> GetMstAssistantList()
	{
		return this.mstAssistantList;
	}

	// Token: 0x0600031A RID: 794 RVA: 0x00017B6C File Offset: 0x00015D6C
	public List<MstAssistantData> GetShowDataList()
	{
		List<MstAssistantData> list = new List<MstAssistantData>();
		foreach (MstAssistantData mstAssistantData in this.mstAssistantList)
		{
			DateTime dateTime = new DateTime(PrjUtil.ConvertTimeToTicks(mstAssistantData.startTime));
			if (!(TimeManager.Now < dateTime))
			{
				list.Add(mstAssistantData);
			}
		}
		return list;
	}

	// Token: 0x0600031B RID: 795 RVA: 0x00017BE8 File Offset: 0x00015DE8
	public List<int> GetPurchaseList(SelAssistantCtrl.Scene scene)
	{
		if (scene != SelAssistantCtrl.Scene.SHOP)
		{
			return this.userData.purchaseListQuest;
		}
		return this.userData.purchaseListShop;
	}

	// Token: 0x0600031C RID: 796 RVA: 0x00017C04 File Offset: 0x00015E04
	public void RequestUpdateShopAssistant(int shopAssistantCharaId)
	{
		this.parentData.ServerRequest(ShopAssistantUpdateCmd.Create(shopAssistantCharaId), new Action<Command>(this.CbUpdateShopAssistantCmd));
	}

	// Token: 0x0600031D RID: 797 RVA: 0x00017C23 File Offset: 0x00015E23
	public void RequestUpdateQuestAssistant(int questAssistantCharaId)
	{
		this.parentData.ServerRequest(QuestAssistantUpdateCmd.Create(questAssistantCharaId), new Action<Command>(this.CbUpdateQuestAssistantCmd));
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00017C42 File Offset: 0x00015E42
	public void RequestResistShopAssistant(int resist)
	{
		this.parentData.ServerRequest(ResistShopAssistantCmd.Create(resist), new Action<Command>(this.CbResistShopAssistantCmd));
	}

	// Token: 0x0600031F RID: 799 RVA: 0x00017C61 File Offset: 0x00015E61
	public void RequestResistQuestAssistant(int resist)
	{
		this.parentData.ServerRequest(ResistQuestAssistantCmd.Create(resist), new Action<Command>(this.CbResistQuestAssistantCmd));
	}

	// Token: 0x06000320 RID: 800 RVA: 0x00017C80 File Offset: 0x00015E80
	private void CbUpdateShopAssistantCmd(Command cmd)
	{
		ShopAssistantUpdateResponse shopAssistantUpdateResponse = cmd.response as ShopAssistantUpdateResponse;
		this.UpdateAssistantDataByServer(shopAssistantUpdateResponse.assistant_data);
	}

	// Token: 0x06000321 RID: 801 RVA: 0x00017CA8 File Offset: 0x00015EA8
	private void CbUpdateQuestAssistantCmd(Command cmd)
	{
		QuestAssistantUpdateResponse questAssistantUpdateResponse = cmd.response as QuestAssistantUpdateResponse;
		this.UpdateAssistantDataByServer(questAssistantUpdateResponse.assistant_data);
	}

	// Token: 0x06000322 RID: 802 RVA: 0x00017CD0 File Offset: 0x00015ED0
	private void CbResistShopAssistantCmd(Command cmd)
	{
		ResistShopAssistantResponse resistShopAssistantResponse = cmd.response as ResistShopAssistantResponse;
		this.parentData.UpdateUserAssetByAssets(resistShopAssistantResponse.assets);
	}

	// Token: 0x06000323 RID: 803 RVA: 0x00017CFC File Offset: 0x00015EFC
	private void CbResistQuestAssistantCmd(Command cmd)
	{
		ResistQuestAssistantResponse resistQuestAssistantResponse = cmd.response as ResistQuestAssistantResponse;
		this.parentData.UpdateUserAssetByAssets(resistQuestAssistantResponse.assets);
	}

	// Token: 0x0400049E RID: 1182
	private DataManager parentData;

	// Token: 0x0400049F RID: 1183
	private DataManagerAssistant.UserAssistantData userData;

	// Token: 0x040004A0 RID: 1184
	private List<MstAssistantData> mstAssistantList;

	// Token: 0x0200062C RID: 1580
	public class UserAssistantData
	{
		// Token: 0x06003014 RID: 12308 RVA: 0x001BA32A File Offset: 0x001B852A
		public UserAssistantData(AssistantData assistantData)
		{
			this.purchaseListQuest = assistantData.purchaseListQuest;
			this.purchaseListShop = assistantData.purchaseListShop;
			this.questAssistantCharaId = assistantData.questAssistantCharaId;
			this.shopAssistantCharaId = assistantData.shopAssistantCharaId;
		}

		// Token: 0x04002DDA RID: 11738
		public List<int> purchaseListQuest;

		// Token: 0x04002DDB RID: 11739
		public List<int> purchaseListShop;

		// Token: 0x04002DDC RID: 11740
		public int questAssistantCharaId;

		// Token: 0x04002DDD RID: 11741
		public int shopAssistantCharaId;
	}
}
