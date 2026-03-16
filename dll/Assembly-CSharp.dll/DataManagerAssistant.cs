using System;
using System.Collections.Generic;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class DataManagerAssistant
{
	public DataManagerAssistant.UserAssistantData UserData
	{
		get
		{
			return this.userData;
		}
	}

	public DataManagerAssistant(DataManager p)
	{
		this.parentData = p;
	}

	public void InitializeMstData(MstManager mst)
	{
		this.mstAssistantList = mst.GetMst<List<MstAssistantData>>(MstType.ASSISTANT_DATA);
	}

	public void UpdateAssistantDataByServer(AssistantData assistantData)
	{
		this.userData = new DataManagerAssistant.UserAssistantData(assistantData);
	}

	public List<MstAssistantData> GetMstAssistantList()
	{
		return this.mstAssistantList;
	}

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

	public List<int> GetPurchaseList(SelAssistantCtrl.Scene scene)
	{
		if (scene != SelAssistantCtrl.Scene.SHOP)
		{
			return this.userData.purchaseListQuest;
		}
		return this.userData.purchaseListShop;
	}

	public void RequestUpdateShopAssistant(int shopAssistantCharaId)
	{
		this.parentData.ServerRequest(ShopAssistantUpdateCmd.Create(shopAssistantCharaId), new Action<Command>(this.CbUpdateShopAssistantCmd));
	}

	public void RequestUpdateQuestAssistant(int questAssistantCharaId)
	{
		this.parentData.ServerRequest(QuestAssistantUpdateCmd.Create(questAssistantCharaId), new Action<Command>(this.CbUpdateQuestAssistantCmd));
	}

	public void RequestResistShopAssistant(int resist)
	{
		this.parentData.ServerRequest(ResistShopAssistantCmd.Create(resist), new Action<Command>(this.CbResistShopAssistantCmd));
	}

	public void RequestResistQuestAssistant(int resist)
	{
		this.parentData.ServerRequest(ResistQuestAssistantCmd.Create(resist), new Action<Command>(this.CbResistQuestAssistantCmd));
	}

	private void CbUpdateShopAssistantCmd(Command cmd)
	{
		ShopAssistantUpdateResponse shopAssistantUpdateResponse = cmd.response as ShopAssistantUpdateResponse;
		this.UpdateAssistantDataByServer(shopAssistantUpdateResponse.assistant_data);
	}

	private void CbUpdateQuestAssistantCmd(Command cmd)
	{
		QuestAssistantUpdateResponse questAssistantUpdateResponse = cmd.response as QuestAssistantUpdateResponse;
		this.UpdateAssistantDataByServer(questAssistantUpdateResponse.assistant_data);
	}

	private void CbResistShopAssistantCmd(Command cmd)
	{
		ResistShopAssistantResponse resistShopAssistantResponse = cmd.response as ResistShopAssistantResponse;
		this.parentData.UpdateUserAssetByAssets(resistShopAssistantResponse.assets);
	}

	private void CbResistQuestAssistantCmd(Command cmd)
	{
		ResistQuestAssistantResponse resistQuestAssistantResponse = cmd.response as ResistQuestAssistantResponse;
		this.parentData.UpdateUserAssetByAssets(resistQuestAssistantResponse.assets);
	}

	private DataManager parentData;

	private DataManagerAssistant.UserAssistantData userData;

	private List<MstAssistantData> mstAssistantList;

	public class UserAssistantData
	{
		public UserAssistantData(AssistantData assistantData)
		{
			this.purchaseListQuest = assistantData.purchaseListQuest;
			this.purchaseListShop = assistantData.purchaseListShop;
			this.questAssistantCharaId = assistantData.questAssistantCharaId;
			this.shopAssistantCharaId = assistantData.shopAssistantCharaId;
		}

		public List<int> purchaseListQuest;

		public List<int> purchaseListShop;

		public int questAssistantCharaId;

		public int shopAssistantCharaId;
	}
}
