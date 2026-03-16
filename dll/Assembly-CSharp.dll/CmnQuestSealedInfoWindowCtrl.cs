using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;

public class CmnQuestSealedInfoWindowCtrl : MonoBehaviour
{
	public void Setup(GameObject obj)
	{
		this.sealedInfoData = new CmnQuestSealedInfoWindowCtrl.GUISealedInfoData(obj);
	}

	public CmnQuestSealedInfoWindowCtrl.GUISealedInfoData sealedInfoData;

	public class GUISealedInfoData
	{
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

		public void Setup(List<DataManagerQuest.QuestSealedCharaData> sealedList)
		{
			this.scroll.Clear();
			this.sealedList = sealedList;
			this.sealedList = sealedList.OrderBy<DataManagerQuest.QuestSealedCharaData, int>((DataManagerQuest.QuestSealedCharaData x) => x.questOneId).ToList<DataManagerQuest.QuestSealedCharaData>();
			this.scroll.Setup(sealedList.Count, 0);
			this.scroll.Resize(sealedList.Count, 0);
		}

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

		public GameObject baseObj;

		public PguiOpenWindowCtrl owCtrl;

		public ReuseScroll scroll;

		private PguiButtonCtrl butotnClose;

		private List<DataManagerQuest.QuestSealedCharaData> sealedList;
	}
}
