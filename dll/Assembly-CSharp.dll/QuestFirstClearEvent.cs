using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020000C1 RID: 193
public class QuestFirstClearEvent
{
	// Token: 0x060008C6 RID: 2246 RVA: 0x0003813C File Offset: 0x0003633C
	public QuestFirstClearEvent(int _firstClearQuestOneId)
	{
		this.firstClearQuestOneId = _firstClearQuestOneId;
		this.resolveEnumerator = this.ResolveInternal();
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x00038164 File Offset: 0x00036364
	public QuestFirstClearEvent.ResolveResult UpdateResolve()
	{
		if (this.resolveEnumerator != null)
		{
			SceneManager.SceneName? sceneName = this.resolveEnumerator.Current as SceneManager.SceneName?;
			if (!this.resolveEnumerator.MoveNext())
			{
				if (sceneName != null)
				{
					this.resolveResult.nextSceneName = sceneName.Value;
				}
				this.resolveResult.isFinish = true;
			}
		}
		return this.resolveResult;
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x000381C9 File Offset: 0x000363C9
	private IEnumerator ResolveInternal()
	{
		foreach (QuestFirstClearEvent.EventData eventData in QuestFirstClearEvent.eventDataList)
		{
			if (eventData.questOneId == this.firstClearQuestOneId)
			{
				QuestFirstClearEvent.<>c__DisplayClass8_0 CS$<>8__locals1 = new QuestFirstClearEvent.<>c__DisplayClass8_0();
				CS$<>8__locals1.isFinishWindow = false;
				CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, eventData.title, eventData.imagePathList, delegate(bool b)
				{
					CS$<>8__locals1.isFinishWindow = true;
				});
				while (!CS$<>8__locals1.isFinishWindow)
				{
					yield return null;
				}
				while (!CanvasManager.HdlCmnFeedPageWindowCtrl.FinishedClose())
				{
					yield return null;
				}
				CS$<>8__locals1 = null;
			}
		}
		List<QuestFirstClearEvent.EventData>.Enumerator enumerator = default(List<QuestFirstClearEvent.EventData>.Enumerator);
		List<PurchaseProductStatic> list = DataManager.DmPurchase.CreateMstPurchaseProductByQuest(this.firstClearQuestOneId);
		foreach (PurchaseProductStatic purchaseProductStatic in list)
		{
			QuestFirstClearEvent.<>c__DisplayClass8_1 CS$<>8__locals2 = new QuestFirstClearEvent.<>c__DisplayClass8_1();
			if (!string.IsNullOrEmpty(purchaseProductStatic.infoPicturePath) && purchaseProductStatic.infoType == 1)
			{
				CS$<>8__locals2.isFinishWindow = false;
				CS$<>8__locals2.isOpenPurchase = false;
				CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.SHOP_ESCORT, "お知らせ", new List<string> { "Texture2D/Tutorial_Window/Shop_Info/" + purchaseProductStatic.infoPicturePath }, delegate(bool b)
				{
					CS$<>8__locals2.isOpenPurchase = b;
					CS$<>8__locals2.isFinishWindow = true;
				});
				while (!CS$<>8__locals2.isFinishWindow)
				{
					yield return null;
				}
				while (!CanvasManager.HdlCmnFeedPageWindowCtrl.FinishedClose())
				{
					yield return null;
				}
				if (CS$<>8__locals2.isOpenPurchase)
				{
					CanvasManager.HdlSelPurchaseStoneWindowCtrl.Setup(PurchaseProductOne.TabType.Limited);
					while (CanvasManager.HdlSelPurchaseStoneWindowCtrl.IsActiveWindow())
					{
						yield return null;
					}
				}
				CS$<>8__locals2 = null;
			}
		}
		List<PurchaseProductStatic>.Enumerator enumerator2 = default(List<PurchaseProductStatic>.Enumerator);
		int num = 10010303;
		if (DataManager.DmMonthlyPack.nowPackData.PackId == 0 && this.firstClearQuestOneId == num)
		{
			QuestFirstClearEvent.<>c__DisplayClass8_2 CS$<>8__locals3 = new QuestFirstClearEvent.<>c__DisplayClass8_2();
			CS$<>8__locals3.isFinishWindow = false;
			CS$<>8__locals3.isOpenPurchase = false;
			CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.SHOP_ESCORT, "月間パスポート", new List<string> { "Texture2D/Tutorial_Window/Shop_Info/shop_info_001" }, delegate(bool b)
			{
				CS$<>8__locals3.isOpenPurchase = b;
				CS$<>8__locals3.isFinishWindow = true;
			});
			while (!CS$<>8__locals3.isFinishWindow)
			{
				yield return null;
			}
			while (!CanvasManager.HdlCmnFeedPageWindowCtrl.FinishedClose())
			{
				yield return null;
			}
			if (CS$<>8__locals3.isOpenPurchase)
			{
				CanvasManager.HdlSelMonthlyPackWindowCtrl.Setup();
				while (CanvasManager.HdlSelMonthlyPackWindowCtrl.IsActiveWindow())
				{
					yield return null;
				}
			}
			CS$<>8__locals3 = null;
		}
		yield break;
		yield break;
	}

	// Token: 0x04000744 RID: 1860
	private int firstClearQuestOneId;

	// Token: 0x04000745 RID: 1861
	private IEnumerator resolveEnumerator;

	// Token: 0x04000746 RID: 1862
	private QuestFirstClearEvent.ResolveResult resolveResult = new QuestFirstClearEvent.ResolveResult();

	// Token: 0x04000747 RID: 1863
	private static readonly List<QuestFirstClearEvent.EventData> eventDataList = new List<QuestFirstClearEvent.EventData>
	{
		new QuestFirstClearEvent.EventData
		{
			questOneId = 10010203,
			title = "家具について",
			imagePathList = { "Texture2D/Tutorial_Window/Furniture/tutorial_furniture_01", "Texture2D/Tutorial_Window/Furniture/tutorial_furniture_02" }
		}
	};

	// Token: 0x020007B7 RID: 1975
	public class ResolveResult
	{
		// Token: 0x04003471 RID: 13425
		public bool isFinish;

		// Token: 0x04003472 RID: 13426
		public SceneManager.SceneName nextSceneName;
	}

	// Token: 0x020007B8 RID: 1976
	private class EventData
	{
		// Token: 0x04003473 RID: 13427
		public int questOneId;

		// Token: 0x04003474 RID: 13428
		public string title = "";

		// Token: 0x04003475 RID: 13429
		public List<string> imagePathList = new List<string>();
	}
}
