using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;

// Token: 0x02000199 RID: 409
public class CmnQuestSealedInfoWindowCtrl : MonoBehaviour
{
	// Token: 0x06001B28 RID: 6952 RVA: 0x0015D986 File Offset: 0x0015BB86
	public void Setup(GameObject obj)
	{
		this.sealedInfoData = new CmnQuestSealedInfoWindowCtrl.GUISealedInfoData(obj);
	}

	// Token: 0x04001484 RID: 5252
	public CmnQuestSealedInfoWindowCtrl.GUISealedInfoData sealedInfoData;

	// Token: 0x02000EAC RID: 3756
	public class GUISealedInfoData
	{
		// Token: 0x06004D57 RID: 19799 RVA: 0x00232514 File Offset: 0x00230714
		public GUISealedInfoData(GameObject obj)
		{
			this.baseObj = obj;
			Transform transform = this.baseObj.transform;
			string text = "All/Base/Window/";
			this.owCtrl = transform.Find("All").GetComponent<PguiOpenWindowCtrl>();
			this.scroll = transform.Find(text + "ScrollView").GetComponent<ReuseScroll>();
			this.butotnClose = transform.Find(text + "ButtonC").GetComponent<PguiButtonCtrl>();
			this.scroll.InitForce();
			ReuseScroll reuseScroll = this.scroll;
			reuseScroll.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll.onUpdateItem, new Action<int, GameObject>(this.UpdateSealdInfoItem));
			this.butotnClose.AddOnClickListener(delegate(PguiButtonCtrl button)
			{
				this.owCtrl.ForceClose();
			}, PguiButtonCtrl.SoundType.DEFAULT);
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x002325D8 File Offset: 0x002307D8
		public void Setup(List<DataManagerQuest.QuestSealedCharaData> sealedList)
		{
			this.scroll.Clear();
			this.sealedList = sealedList;
			this.sealedList = sealedList.OrderBy<DataManagerQuest.QuestSealedCharaData, int>((DataManagerQuest.QuestSealedCharaData x) => x.questOneId).ToList<DataManagerQuest.QuestSealedCharaData>();
			this.scroll.Setup(sealedList.Count, 0);
			this.scroll.Resize(sealedList.Count, 0);
		}

		// Token: 0x06004D59 RID: 19801 RVA: 0x0023264C File Offset: 0x0023084C
		private void UpdateSealdInfoItem(int index, GameObject go)
		{
			go.SetActive(this.sealedList.Count<DataManagerQuest.QuestSealedCharaData>() >= index);
			if (this.sealedList.Count<DataManagerQuest.QuestSealedCharaData>() <= index)
			{
				return;
			}
			Transform transform = go.transform.Find("BaseImage");
			GameObject gameObject = transform.Find("IconCharaList").gameObject;
			foreach (GameObject gameObject2 in gameObject.GetChildList())
			{
				Object.Destroy(gameObject2);
			}
			DataManagerQuest.QuestSealedCharaData questSealedCharaData = this.sealedList[index];
			QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(questSealedCharaData.questOneId);
			foreach (int num in questSealedCharaData.sealedList)
			{
				IconItemCtrl component = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, go.transform.Find("BaseImage/Icon_Item")).GetComponent<IconItemCtrl>();
				ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(num);
				component.transform.SetParent(gameObject.transform, false);
				component.Setup(itemStaticBase);
				component.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
			}
			transform.Find("Txt_TargetQuestName").GetComponent<PguiTextCtrl>().text = questOnePackData.questOne.questName;
		}

		// Token: 0x04005433 RID: 21555
		public GameObject baseObj;

		// Token: 0x04005434 RID: 21556
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04005435 RID: 21557
		public ReuseScroll scroll;

		// Token: 0x04005436 RID: 21558
		private PguiButtonCtrl butotnClose;

		// Token: 0x04005437 RID: 21559
		private List<DataManagerQuest.QuestSealedCharaData> sealedList;
	}
}
