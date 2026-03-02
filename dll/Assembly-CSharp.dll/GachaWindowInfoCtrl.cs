using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000142 RID: 322
public class GachaWindowInfoCtrl : MonoBehaviour
{
	// Token: 0x060011AB RID: 4523 RVA: 0x000D658C File Offset: 0x000D478C
	public void Initialize()
	{
		this.detailWindowGuiData = new GachaWindowInfoCtrl.GachaDetailInfoWindowGUI(base.transform);
		this.detailWindowGuiData.BaseAnim.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.detailWindowGuiData.InBase.SetActive(false);
		this.detailWindowGuiData.BtnClose.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickCloseButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.detailWindowGuiData.LeftArrow.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickArrowButton(btn, 0);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.detailWindowGuiData.RightArrow.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickArrowButton(btn, 1);
		}, PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x060011AC RID: 4524 RVA: 0x000D6624 File Offset: 0x000D4824
	public IEnumerator Open(DataManagerGacha.GachaPackData gpd, UnityAction finishCb)
	{
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if (this.m_Sequence == GachaWindowInfoCtrl.Sequence.INACTIVE)
		{
			this.m_Sequence = GachaWindowInfoCtrl.Sequence.OPEN_START;
			this.m_CallBack = finishCb;
		}
		this.detailWindowGuiData.TabGroup.Setup(0, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectTab));
		this.detailWindowGuiData.SelectGachaStaticData = gpd.staticData;
		this.OnSelectTab(0);
		this.RefleshGachaDetailWindow();
		yield break;
	}

	// Token: 0x060011AD RID: 4525 RVA: 0x000D6644 File Offset: 0x000D4844
	private void SetupGachaRate(GachaWindowInfoCtrl.TotalRatioLabelGUI ratioLabel, List<DataManagerGacha.ProbabilityData.Element> elementList, bool isDecided4)
	{
		for (int i = 0; i < 3; i++)
		{
			if (i < elementList[0].rate.Count)
			{
				DataManagerGacha.ProbabilityData.ItemOne itemOne = elementList[0].rate[i];
				ratioLabel.NumList[i * 4].text = PrjUtil.SetRate(itemOne.normal);
				ratioLabel.NumList[i * 4 + 1].text = (isDecided4 ? PrjUtil.SetRate(itemOne.decided4) : PrjUtil.SetRate(itemOne.decided3));
				ratioLabel.NumList[i * 4 + 2].text = PrjUtil.SetRate(itemOne.decided);
				ratioLabel.NumList[i * 4 + 3].text = PrjUtil.SetRate(itemOne.decidedCeiling);
			}
			else
			{
				ratioLabel.NumList[i * 4].text = "-";
				ratioLabel.NumList[i * 4 + 1].text = "-";
				ratioLabel.NumList[i * 4 + 2].text = "-";
				ratioLabel.NumList[i * 4 + 3].text = "-";
			}
		}
	}

	// Token: 0x060011AE RID: 4526 RVA: 0x000D6784 File Offset: 0x000D4984
	private void SetupRateList()
	{
		DataManagerGacha.GachaStaticData selectGachaStaticData = this.detailWindowGuiData.SelectGachaStaticData;
		DataManagerGacha.ProbabilityData lastRequestRateViewData = DataManager.DmGacha.GetLastRequestRateViewData();
		Transform transform = this.detailWindowGuiData.gachaDetailTabMap[GachaWindowInfoCtrl.TabType.TabRate].Content.transform.Find("RatioTable");
		if (transform != null)
		{
			Object.Destroy(transform.gameObject);
		}
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts01"));
		gameObject.transform.SetParent(this.detailWindowGuiData.gachaDetailTabMap[GachaWindowInfoCtrl.TabType.TabRate].Content.transform, false);
		gameObject.name = "RatioTable";
		GachaWindowInfoCtrl.TabInnerFrameGUI tabInnerFrameGUI = new GachaWindowInfoCtrl.TabInnerFrameGUI(gameObject.transform);
		tabInnerFrameGUI.Title.text = "提供割合";
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.anchoredPosition = new Vector2(0f, component.anchoredPosition.y);
		DataManagerGacha.GachaStaticTypeData gachaStaticTypeData = ((1 < selectGachaStaticData.typeDataList.Count) ? selectGachaStaticData.typeDataList[1] : selectGachaStaticData.typeDataList[0]);
		bool flag = this.IsDecidedFour(gachaStaticTypeData);
		bool flag2 = gachaStaticTypeData.lastTimeBenefitFriends != 0;
		bool flag3 = selectGachaStaticData.highLimit != 0;
		bool flag4 = false;
		bool flag5 = false;
		if (DataManagerGacha.Category.Box == selectGachaStaticData.gachaCategory)
		{
			GachaWindowInfoCtrl.RatioLabelFriendsPhotoGUI ratioLabelFriendsPhotoGUI = new GachaWindowInfoCtrl.RatioLabelFriendsPhotoGUI(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts04_Half"), tabInnerFrameGUI.Base.transform).transform);
			GachaWindowInfoCtrl.RatioLabelItemGUI ratioLabelItemGUI = new GachaWindowInfoCtrl.RatioLabelItemGUI(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts04_Item"), tabInnerFrameGUI.Base.transform).transform);
			ratioLabelItemGUI.Txt_Kind.text = "インテリア";
			GachaWindowInfoCtrl.RatioLabelItemGUI ratioLabelItemGUI2 = new GachaWindowInfoCtrl.RatioLabelItemGUI(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts04_Item"), tabInnerFrameGUI.Base.transform).transform);
			List<DataManagerGacha.ProbabilityData.Element> elements = lastRequestRateViewData.GetElements(DataManagerGacha.ProbabilityData.Type.Chara, DataManagerGacha.ProbabilityData.Category.Rarity);
			List<DataManagerGacha.ProbabilityData.Element> elements2 = lastRequestRateViewData.GetElements(DataManagerGacha.ProbabilityData.Type.Photo, DataManagerGacha.ProbabilityData.Category.Rarity);
			List<DataManagerGacha.ProbabilityData.Element> elements3 = lastRequestRateViewData.GetElements(DataManagerGacha.ProbabilityData.Type.TreeHouseFurniture, DataManagerGacha.ProbabilityData.Category.Rarity);
			List<DataManagerGacha.ProbabilityData.Element> elements4 = lastRequestRateViewData.GetElements(DataManagerGacha.ProbabilityData.Type.Item, DataManagerGacha.ProbabilityData.Category.Rarity);
			ratioLabelFriendsPhotoGUI.baseObj.SetActive(0 < elements[0].rate.Count || 0 < elements2[0].rate.Count);
			ratioLabelItemGUI.baseObj.SetActive(0 < elements3[0].rate.Count);
			ratioLabelItemGUI2.baseObj.SetActive(0 < elements4[0].rate.Count);
			int num;
			int j;
			for (j = 0; j < ratioLabelFriendsPhotoGUI.charaNumList.Count; j = num)
			{
				if (!(null == ratioLabelFriendsPhotoGUI.charaNumList[j]))
				{
					DataManagerGacha.ProbabilityData.ItemOne itemOne = elements[0].rate.Find((DataManagerGacha.ProbabilityData.ItemOne x) => x.rarity == j);
					if (itemOne == null)
					{
						ratioLabelFriendsPhotoGUI.charaNumList[j].text = "-";
					}
					else
					{
						int rarity = itemOne.rarity;
						ratioLabelFriendsPhotoGUI.charaNumList[rarity].text = ((itemOne.normal > double.Epsilon) ? (itemOne.normal.ToString() + "%%") : "-");
					}
				}
				num = j + 1;
			}
			int k;
			for (k = 0; k < ratioLabelFriendsPhotoGUI.photoNumList.Count; k = num)
			{
				if (!(null == ratioLabelFriendsPhotoGUI.photoNumList[k]))
				{
					DataManagerGacha.ProbabilityData.ItemOne itemOne2 = elements2[0].rate.Find((DataManagerGacha.ProbabilityData.ItemOne x) => x.rarity == k);
					if (itemOne2 == null)
					{
						ratioLabelFriendsPhotoGUI.photoNumList[k].text = "-";
					}
					else
					{
						int rarity2 = itemOne2.rarity;
						ratioLabelFriendsPhotoGUI.photoNumList[rarity2].text = ((itemOne2.normal > double.Epsilon) ? (itemOne2.normal.ToString() + "%") : "-");
					}
				}
				num = k + 1;
			}
			int l;
			for (l = 0; l < ratioLabelItemGUI.itemNumList.Count; l = num)
			{
				if (!(null == ratioLabelItemGUI.itemNumList[l]))
				{
					if (l < 3)
					{
						ratioLabelItemGUI.itemNumList[l].text = "-";
						ratioLabelItemGUI.itemNumList[l].gameObject.SetActive(false);
					}
					else
					{
						DataManagerGacha.ProbabilityData.ItemOne itemOne3 = elements3[0].rate.Find((DataManagerGacha.ProbabilityData.ItemOne x) => x.rarity == l);
						if (itemOne3 == null)
						{
							ratioLabelItemGUI.itemNumList[l].text = "-";
						}
						else
						{
							int rarity3 = itemOne3.rarity;
							ratioLabelItemGUI.itemNumList[rarity3].text = ((itemOne3.normal > double.Epsilon) ? (itemOne3.normal.ToString() + "%") : "-");
						}
					}
				}
				num = l + 1;
			}
			int i;
			for (i = 0; i < ratioLabelItemGUI2.itemNumList.Count; i = num)
			{
				if (!(null == ratioLabelItemGUI2.itemNumList[i]))
				{
					DataManagerGacha.ProbabilityData.ItemOne itemOne4 = elements4[0].rate.Find((DataManagerGacha.ProbabilityData.ItemOne x) => x.rarity == i);
					if (itemOne4 == null)
					{
						ratioLabelItemGUI2.itemNumList[i].text = "-";
					}
					else
					{
						int rarity4 = itemOne4.rarity;
						ratioLabelItemGUI2.itemNumList[rarity4].text = ((itemOne4.normal > double.Epsilon) ? (itemOne4.normal.ToString() + "%") : "-");
					}
				}
				num = i + 1;
			}
		}
		else
		{
			List<DataManagerGacha.ProbabilityData.Type> list = new List<DataManagerGacha.ProbabilityData.Type>();
			list.Add(DataManagerGacha.ProbabilityData.Type.Chara);
			list.Add(DataManagerGacha.ProbabilityData.Type.Photo);
			list.Add(DataManagerGacha.ProbabilityData.Type.TreeHouseFurniture);
			list.Add(DataManagerGacha.ProbabilityData.Type.Item);
			string text = "☆" + (flag ? "4" : "3") + "以上確定時";
			string text2 = (flag ? "SR" : "R") + "以上確定時";
			foreach (DataManagerGacha.ProbabilityData.Type type in list)
			{
				List<DataManagerGacha.ProbabilityData.Element> elements5 = lastRequestRateViewData.GetElements(type, DataManagerGacha.ProbabilityData.Category.Rarity);
				if (elements5.Count != 0)
				{
					int num2 = 0;
					foreach (DataManagerGacha.ProbabilityData.Element element in elements5)
					{
						num2 += element.rate.Count;
					}
					if (num2 != 0)
					{
						elements5[0].rate.Sort((DataManagerGacha.ProbabilityData.ItemOne a, DataManagerGacha.ProbabilityData.ItemOne b) => b.rarity - a.rarity);
						GachaWindowInfoCtrl.TotalRatioLabelGUI totalRatioLabelGUI = new GachaWindowInfoCtrl.TotalRatioLabelGUI(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts04"), tabInnerFrameGUI.Base.transform).transform);
						switch (type)
						{
						case DataManagerGacha.ProbabilityData.Type.Chara:
							totalRatioLabelGUI.LabelTitle.text = "フレンズ";
							totalRatioLabelGUI.StarBase.SetActive(true);
							totalRatioLabelGUI.RarityBase.SetActive(false);
							totalRatioLabelGUI.Txt02.text = text;
							flag4 = true;
							break;
						case DataManagerGacha.ProbabilityData.Type.Photo:
							totalRatioLabelGUI.LabelTitle.text = "フォト";
							totalRatioLabelGUI.StarBase.SetActive(true);
							totalRatioLabelGUI.RarityBase.SetActive(false);
							totalRatioLabelGUI.Txt02.text = text;
							flag4 = true;
							break;
						case DataManagerGacha.ProbabilityData.Type.Item:
							totalRatioLabelGUI.LabelTitle.text = "アイテム";
							totalRatioLabelGUI.StarBase.SetActive(false);
							totalRatioLabelGUI.RarityBase.SetActive(true);
							totalRatioLabelGUI.Txt02.text = text2;
							break;
						case DataManagerGacha.ProbabilityData.Type.TreeHouseFurniture:
							totalRatioLabelGUI.LabelTitle.text = "インテリア";
							totalRatioLabelGUI.StarBase.SetActive(false);
							totalRatioLabelGUI.RarityBase.SetActive(true);
							totalRatioLabelGUI.Txt02.text = text2;
							flag5 = true;
							break;
						}
						this.SetupGachaRate(totalRatioLabelGUI, elements5, flag);
						totalRatioLabelGUI.NumAll03.gameObject.SetActive(flag2);
						totalRatioLabelGUI.Txt03.SetActive(flag2);
						totalRatioLabelGUI.NumAll04.gameObject.SetActive(flag3);
						totalRatioLabelGUI.Txt04.SetActive(flag3);
					}
				}
			}
		}
		GachaWindowInfoCtrl.DetailMessageGUI detailMessageGUI = new GachaWindowInfoCtrl.DetailMessageGUI(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts05"), tabInnerFrameGUI.Base.transform).transform);
		StringBuilder stringBuilder = new StringBuilder();
		bool flag6 = this.IsDecidedRarity(gachaStaticTypeData);
		if (flag2 || flag6)
		{
			stringBuilder.AppendFormat("※「{0}回」の利用時、", gachaStaticTypeData.lotTime);
			if (flag2)
			{
				stringBuilder.Append("「フレンズ確定時」");
			}
			if (flag6)
			{
				stringBuilder.Append("「");
				if (flag4)
				{
					stringBuilder.AppendFormat("★{0}", flag ? 4 : 3);
				}
				if (flag5)
				{
					stringBuilder.AppendFormat("{0}{1}{2}", flag4 ? "(" : string.Empty, flag ? "SR" : "R", flag4 ? ")" : string.Empty);
				}
				stringBuilder.Append("以上確定時」");
			}
			int num3 = ((flag2 && flag6) ? 2 : 1);
			int num4 = gachaStaticTypeData.lotTime - num3;
			stringBuilder.AppendFormat("を除いた{0}枠は通常の確率で抽選されます。\n", num4);
		}
		if (flag3)
		{
			stringBuilder.AppendFormat("※{0}回連続で★4フレンズが排出されなかった場合、{0}回目は★4フレンズ確定時の確率で抽選されます。\n", selectGachaStaticData.highLimit);
		}
		stringBuilder.Append(selectGachaStaticData.detailDispText);
		detailMessageGUI.DetailText.text = stringBuilder.ToString();
		this.detailWindowGuiData.gachaDetailTabMap[GachaWindowInfoCtrl.TabType.TabRate].ScrollRect.verticalNormalizedPosition = 1f;
	}

	// Token: 0x060011AF RID: 4527 RVA: 0x000D7250 File Offset: 0x000D5450
	private void SetupFriendsPhotoTreeHouseFurnitureItemList()
	{
		DataManagerGacha.GachaStaticData selectGachaStaticData = this.detailWindowGuiData.SelectGachaStaticData;
		DataManagerGacha.ProbabilityData lastRequestRateViewData = DataManager.DmGacha.GetLastRequestRateViewData();
		List<GachaWindowInfoCtrl.TabType> list = new List<GachaWindowInfoCtrl.TabType>();
		list.Add(GachaWindowInfoCtrl.TabType.TabFriends);
		list.Add(GachaWindowInfoCtrl.TabType.TabPhoto);
		list.Add(GachaWindowInfoCtrl.TabType.TabTreeHouseFurniture);
		list.Add(GachaWindowInfoCtrl.TabType.TabItem);
		bool flag = DataManagerGacha.Category.Box == selectGachaStaticData.gachaCategory;
		DataManagerGacha.GachaStaticTypeData gachaStaticTypeData = ((1 < selectGachaStaticData.typeDataList.Count) ? selectGachaStaticData.typeDataList[1] : selectGachaStaticData.typeDataList[0]);
		bool flag2 = this.IsDecidedFour(gachaStaticTypeData);
		foreach (GachaWindowInfoCtrl.TabType tabType in list)
		{
			Transform transform = this.detailWindowGuiData.gachaDetailTabMap[tabType].Content.transform.Find("BasePanel");
			if (null != transform)
			{
				Object.Destroy(transform.gameObject);
			}
			GameObject gameObject = new GameObject();
			gameObject.name = "BasePanel";
			gameObject.AddComponent<RectTransform>();
			gameObject.transform.SetParent(this.detailWindowGuiData.gachaDetailTabMap[tabType].Content.transform);
			(gameObject.transform as RectTransform).anchoredPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			VerticalLayoutGroup verticalLayoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			verticalLayoutGroup.childControlWidth = false;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.childForceExpandHeight = true;
			List<DataManagerGacha.ProbabilityData.Category> list2 = new List<DataManagerGacha.ProbabilityData.Category>
			{
				DataManagerGacha.ProbabilityData.Category.PickUp,
				DataManagerGacha.ProbabilityData.Category.Other
			};
			Dictionary<DataManagerGacha.ProbabilityData.Category, List<DataManagerGacha.ProbabilityData.Element>> dictionary = new Dictionary<DataManagerGacha.ProbabilityData.Category, List<DataManagerGacha.ProbabilityData.Element>>();
			int num = 0;
			foreach (DataManagerGacha.ProbabilityData.Category category in list2)
			{
				List<DataManagerGacha.ProbabilityData.Element> list3 = lastRequestRateViewData.GetElements(this.TabType2ProbabilityType(tabType), category);
				PrjUtil.InsertionSort<DataManagerGacha.ProbabilityData.Element>(ref list3, (DataManagerGacha.ProbabilityData.Element a, DataManagerGacha.ProbabilityData.Element b) => b.item_num.CompareTo(a.item_num));
				PrjUtil.InsertionSort<DataManagerGacha.ProbabilityData.Element>(ref list3, (DataManagerGacha.ProbabilityData.Element a, DataManagerGacha.ProbabilityData.Element b) => a.item_id.CompareTo(b.item_id));
				PrjUtil.InsertionSort<DataManagerGacha.ProbabilityData.Element>(ref list3, (DataManagerGacha.ProbabilityData.Element a, DataManagerGacha.ProbabilityData.Element b) => a.sortNum.CompareTo(b.sortNum));
				if (flag && DataManagerGacha.ProbabilityData.Category.Other == category)
				{
					List<DataManagerGacha.ProbabilityData.Element> list4 = new List<DataManagerGacha.ProbabilityData.Element>();
					List<DataManagerGacha.ProbabilityData.Element> list5 = new List<DataManagerGacha.ProbabilityData.Element>();
					foreach (DataManagerGacha.ProbabilityData.Element element in list3)
					{
						if (0 < element.numerator)
						{
							list4.Add(element);
						}
						else
						{
							list5.Add(element);
						}
					}
					List<DataManagerGacha.ProbabilityData.Element> list6 = new List<DataManagerGacha.ProbabilityData.Element>();
					list6.AddRange(list4);
					list6.AddRange(list5);
					list3 = list6;
				}
				PrjUtil.InsertionSort<DataManagerGacha.ProbabilityData.Element>(ref list3, (DataManagerGacha.ProbabilityData.Element a, DataManagerGacha.ProbabilityData.Element b) => b.rate[0].rarity.CompareTo(a.rate[0].rarity));
				num += list3.Count;
				dictionary.Add(category, list3);
			}
			this.detailWindowGuiData.TabGroup.m_PguiTabList[(int)tabType].gameObject.SetActive(num != 0);
			this.detailWindowGuiData.gachaDetailTabMap[tabType].NoneText.gameObject.SetActive(num == 0);
			foreach (DataManagerGacha.ProbabilityData.Category category2 in list2)
			{
				if (dictionary[category2].Count != 0)
				{
					GachaWindowInfoCtrl.TabInnerFrameGUI tabInnerFrameGUI = new GachaWindowInfoCtrl.TabInnerFrameGUI(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts01"), gameObject.transform).transform);
					if (DataManagerGacha.ProbabilityData.Category.PickUp == category2)
					{
						string text;
						switch (tabType)
						{
						case GachaWindowInfoCtrl.TabType.TabFriends:
							text = "フレンズ";
							break;
						case GachaWindowInfoCtrl.TabType.TabPhoto:
							text = "フォト";
							break;
						case GachaWindowInfoCtrl.TabType.TabTreeHouseFurniture:
							text = "インテリア";
							break;
						case GachaWindowInfoCtrl.TabType.TabItem:
							text = "アイテム";
							break;
						default:
							text = string.Empty;
							break;
						}
						tabInnerFrameGUI.Title.text = "ピックアップ対象" + text;
					}
					else
					{
						tabInnerFrameGUI.Title.text = "提供割合";
					}
					int num2 = 0;
					using (List<DataManagerGacha.ProbabilityData.Element>.Enumerator enumerator3 = dictionary[category2].GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							GachaWindowInfoCtrl.<>c__DisplayClass24_0 CS$<>8__locals1 = new GachaWindowInfoCtrl.<>c__DisplayClass24_0();
							CS$<>8__locals1.<>4__this = this;
							CS$<>8__locals1.probabilityElement = enumerator3.Current;
							if (num2 == 0 || CS$<>8__locals1.probabilityElement.rate[0].rarity < num2)
							{
								num2 = CS$<>8__locals1.probabilityElement.rate[0].rarity;
								GachaWindowInfoCtrl.RarityStarBar rarityStarBar = new GachaWindowInfoCtrl.RarityStarBar(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts02"), tabInnerFrameGUI.Base.transform).transform);
								rarityStarBar.IconAllObj.SetActive(false);
								rarityStarBar.RarityText.gameObject.SetActive(false);
								DataManagerGacha.ProbabilityData.Type type = CS$<>8__locals1.probabilityElement.type;
								if (type - DataManagerGacha.ProbabilityData.Type.Chara > 1)
								{
									if (type - DataManagerGacha.ProbabilityData.Type.Item > 1)
									{
										goto IL_05A7;
									}
								}
								else
								{
									rarityStarBar.IconAllObj.SetActive(true);
									int num3 = 0;
									using (List<PguiImageCtrl>.Enumerator enumerator4 = rarityStarBar.IconAllList.GetEnumerator())
									{
										while (enumerator4.MoveNext())
										{
											PguiImageCtrl pguiImageCtrl = enumerator4.Current;
											pguiImageCtrl.gameObject.SetActive(num3 < CS$<>8__locals1.probabilityElement.rate[0].rarity);
											num3++;
										}
										goto IL_05A7;
									}
								}
								rarityStarBar.RarityText.gameObject.SetActive(true);
								rarityStarBar.RarityText.text = PrjUtil.Rarity2String(CS$<>8__locals1.probabilityElement.rate[0].rarity);
							}
							IL_05A7:
							GachaWindowInfoCtrl.ItemInfoLabelGUI itemInfoLabelGUI = new GachaWindowInfoCtrl.ItemInfoLabelGUI(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts03"), tabInnerFrameGUI.Base.transform).transform);
							ItemStaticBase isb = DataManager.DmItem.GetItemStaticBase(CS$<>8__locals1.probabilityElement.item_id);
							itemInfoLabelGUI.Icon_Item.Setup(isb, new IconItemCtrl.SetupParam
							{
								useMaxDetail = true
							});
							itemInfoLabelGUI.IconAllObj.SetActive(false);
							itemInfoLabelGUI.RarityText.gameObject.SetActive(false);
							itemInfoLabelGUI.DisableImage.gameObject.SetActive(false);
							int num4 = 0;
							string text2 = string.Empty;
							string text3 = string.Empty;
							bool flag3 = true;
							switch (CS$<>8__locals1.probabilityElement.type)
							{
							case DataManagerGacha.ProbabilityData.Type.Chara:
							{
								text2 = DataManager.DmChara.GetCharaStaticData(DataManager.DmItem.ItemId2ChraId(CS$<>8__locals1.probabilityElement.item_id)).baseData.NickName;
								itemInfoLabelGUI.IconAllObj.SetActive(true);
								using (List<PguiImageCtrl>.Enumerator enumerator4 = itemInfoLabelGUI.IconAllList.GetEnumerator())
								{
									while (enumerator4.MoveNext())
									{
										PguiImageCtrl pguiImageCtrl2 = enumerator4.Current;
										pguiImageCtrl2.gameObject.SetActive(num4 < CS$<>8__locals1.probabilityElement.rate[0].rarity);
										num4++;
									}
									break;
								}
								goto IL_072B;
							}
							case DataManagerGacha.ProbabilityData.Type.Photo:
								goto IL_072B;
							case DataManagerGacha.ProbabilityData.Type.Item:
								itemInfoLabelGUI.RarityText.gameObject.SetActive(true);
								itemInfoLabelGUI.RarityText.text = PrjUtil.Rarity2String(CS$<>8__locals1.probabilityElement.rate[0].rarity);
								text3 = string.Format("×{0}", CS$<>8__locals1.probabilityElement.item_num);
								flag3 = false;
								break;
							case DataManagerGacha.ProbabilityData.Type.TreeHouseFurniture:
								goto IL_079B;
							}
							IL_085C:
							itemInfoLabelGUI.Name.ReplaceTextByDefault(new string[] { "Param01", "Param02", "Param03" }, new string[]
							{
								text2,
								itemInfoLabelGUI.Icon_Item.itemStaticBase.GetName(),
								text3
							});
							bool flag4 = !flag || 0 < CS$<>8__locals1.probabilityElement.numerator;
							itemInfoLabelGUI.DisableImage.gameObject.SetActive(!flag4);
							itemInfoLabelGUI.RateNormal.text = PrjUtil.SetRate(CS$<>8__locals1.probabilityElement.rate[0].normal);
							itemInfoLabelGUI.RateDecided3.text = (flag2 ? PrjUtil.SetRate(CS$<>8__locals1.probabilityElement.rate[0].decided4) : PrjUtil.SetRate(CS$<>8__locals1.probabilityElement.rate[0].decided3));
							itemInfoLabelGUI.RateDecided3.gameObject.SetActive(!flag);
							string text4 = string.Empty;
							if (flag3)
							{
								text4 = "☆" + (flag2 ? "4" : "3") + "以上確定時";
							}
							else
							{
								text4 = (flag2 ? "SR" : "R") + "以上確定時";
							}
							itemInfoLabelGUI.RateDecided3Text.GetComponent<PguiTextCtrl>().text = text4;
							itemInfoLabelGUI.RateDecided3Text.SetActive(!flag);
							bool flag5 = !flag && gachaStaticTypeData.lastTimeBenefitFriends != 0;
							itemInfoLabelGUI.RateDecidedFriends.text = PrjUtil.SetRate(CS$<>8__locals1.probabilityElement.rate[0].decided);
							itemInfoLabelGUI.RateDecidedFriends.gameObject.SetActive(flag5);
							itemInfoLabelGUI.RateDecidedFriendsText.SetActive(flag5);
							bool flag6 = !flag && selectGachaStaticData.highLimit != 0;
							itemInfoLabelGUI.RateDecidedCeiling.text = PrjUtil.SetRate(CS$<>8__locals1.probabilityElement.rate[0].decidedCeiling);
							itemInfoLabelGUI.RateDecidedCeiling.gameObject.SetActive(flag6);
							itemInfoLabelGUI.RateDecidedCeilingText.SetActive(flag6);
							itemInfoLabelGUI.BoxStack.gameObject.SetActive(false);
							DataManagerGacha.GachaItemdata gachaItemdata = selectGachaStaticData.gachaItemData.Find((DataManagerGacha.GachaItemdata x) => x.itemId == CS$<>8__locals1.probabilityElement.item_id);
							if (gachaItemdata != null)
							{
								itemInfoLabelGUI.Mark_Limited.gameObject.SetActive(gachaItemdata.dispClientTypeLimit != 0);
								itemInfoLabelGUI.Mark_New.gameObject.SetActive(gachaItemdata.dispClientTypeLimit == 0 && gachaItemdata.dispClientTypeNew != 0);
								continue;
							}
							itemInfoLabelGUI.Mark_Limited.gameObject.SetActive(false);
							itemInfoLabelGUI.Mark_New.gameObject.SetActive(false);
							continue;
							IL_072B:
							itemInfoLabelGUI.IconAllObj.SetActive(true);
							using (List<PguiImageCtrl>.Enumerator enumerator4 = itemInfoLabelGUI.IconAllList.GetEnumerator())
							{
								while (enumerator4.MoveNext())
								{
									PguiImageCtrl pguiImageCtrl3 = enumerator4.Current;
									pguiImageCtrl3.gameObject.SetActive(num4 < CS$<>8__locals1.probabilityElement.rate[0].rarity);
									num4++;
								}
								goto IL_085C;
							}
							IL_079B:
							itemInfoLabelGUI.Icon_Item.AddOnLongClickListener(delegate(IconItemCtrl x)
							{
								CS$<>8__locals1.<>4__this.OpenTreeHouseFurnitureWindow(isb);
							});
							itemInfoLabelGUI.RarityText.gameObject.SetActive(true);
							itemInfoLabelGUI.RarityText.text = PrjUtil.Rarity2String(CS$<>8__locals1.probabilityElement.rate[0].rarity);
							flag3 = false;
							goto IL_085C;
						}
					}
				}
			}
			this.detailWindowGuiData.gachaDetailTabMap[tabType].ScrollRect.verticalNormalizedPosition = 1f;
		}
	}

	// Token: 0x060011B0 RID: 4528 RVA: 0x000D7ED0 File Offset: 0x000D60D0
	private void SetupOptionList()
	{
		GachaWindowInfoCtrl.TabType tabType = GachaWindowInfoCtrl.TabType.TabOption;
		DataManagerGacha.GachaStaticTypeData gachaStaticTypeData = null;
		foreach (DataManagerGacha.GachaStaticTypeData gachaStaticTypeData2 in this.detailWindowGuiData.SelectGachaStaticData.typeDataList)
		{
			if (gachaStaticTypeData2.bonusItemOneId != 0)
			{
				gachaStaticTypeData = gachaStaticTypeData2;
				break;
			}
			if (gachaStaticTypeData2.bonusItemSetId != 0)
			{
				gachaStaticTypeData = gachaStaticTypeData2;
				break;
			}
		}
		Transform transform = this.detailWindowGuiData.gachaDetailTabMap[tabType].Content.transform.Find("BasePanel");
		if (null != transform)
		{
			Object.Destroy(transform.gameObject);
		}
		bool flag = gachaStaticTypeData == null && this.detailWindowGuiData.SelectGachaStaticData.enableBonusItemIdList.Count < 1;
		this.detailWindowGuiData.TabGroup.m_PguiTabList[(int)tabType].gameObject.SetActive(!flag);
		this.detailWindowGuiData.gachaDetailTabMap[tabType].NoneText.gameObject.SetActive(flag);
		if (flag)
		{
			return;
		}
		GameObject gameObject = new GameObject();
		gameObject.name = "BasePanel";
		gameObject.AddComponent<RectTransform>();
		gameObject.transform.SetParent(this.detailWindowGuiData.gachaDetailTabMap[tabType].Content.transform);
		(gameObject.transform as RectTransform).anchoredPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		VerticalLayoutGroup verticalLayoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
		verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
		verticalLayoutGroup.childControlWidth = false;
		verticalLayoutGroup.childControlHeight = true;
		verticalLayoutGroup.childForceExpandWidth = false;
		verticalLayoutGroup.childForceExpandHeight = true;
		if (gachaStaticTypeData != null)
		{
			GachaWindowInfoCtrl.TabInnerFrameGUI tabInnerFrameGUI = new GachaWindowInfoCtrl.TabInnerFrameGUI(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts_BonusList"), gameObject.transform).transform);
			string text = gachaStaticTypeData.lotTime.ToString() + "回しょうたいのおまけ";
			if (gachaStaticTypeData.bonusItemLimit != 0)
			{
				text = text + "（" + gachaStaticTypeData.bonusItemLimit.ToString() + "回まで）";
			}
			tabInnerFrameGUI.Title.text = text;
			if (gachaStaticTypeData.bonusItemOneId != 0)
			{
				GameObject gameObject2 = new GameObject();
				gameObject2.name = "itemOneBase";
				gameObject2.AddComponent<RectTransform>();
				gameObject2.transform.SetParent(tabInnerFrameGUI.baseObj.transform.Find("Base/ItemBase/BonusItemList"), false);
				IconItemCtrl component = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, gameObject2.transform).GetComponent<IconItemCtrl>();
				this.<SetupOptionList>g__SetupBonusIcon|25_0(component, gachaStaticTypeData.bonusItemOneId, gachaStaticTypeData.bonusItemNumber);
			}
			if (gachaStaticTypeData.bonusItemSetId != 0)
			{
				GameObject gameObject3 = new GameObject();
				gameObject3.name = "itemSetBase";
				gameObject3.AddComponent<RectTransform>();
				gameObject3.transform.SetParent(tabInnerFrameGUI.baseObj.transform.Find("Base/ItemBase/BonusItemList"), false);
				GachaWindowInfoCtrl.<SetupOptionList>g__SetupBonusIconItemSet|25_1(Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, gameObject3.transform).GetComponent<IconItemCtrl>(), gachaStaticTypeData.bonusItemSetId);
			}
		}
		if (0 < this.detailWindowGuiData.SelectGachaStaticData.enableBonusItemIdList.Count)
		{
			GachaWindowInfoCtrl.TabInnerFrameGUI tabInnerFrameGUI2 = new GachaWindowInfoCtrl.TabInnerFrameGUI(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts01"), gameObject.transform).transform);
			tabInnerFrameGUI2.Title.text = "特定のアイテム排出時のおまけ";
			using (List<int>.Enumerator enumerator2 = this.detailWindowGuiData.SelectGachaStaticData.enableBonusItemIdList.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					GachaWindowInfoCtrl.<>c__DisplayClass25_2 CS$<>8__locals1 = new GachaWindowInfoCtrl.<>c__DisplayClass25_2();
					CS$<>8__locals1.<>4__this = this;
					CS$<>8__locals1.itemId = enumerator2.Current;
					DataManagerGacha.GachaItemdata gachaItemdata = this.detailWindowGuiData.SelectGachaStaticData.gachaItemData.Find((DataManagerGacha.GachaItemdata x) => x.itemId == CS$<>8__locals1.itemId);
					GachaWindowInfoCtrl.OmakeInfoLabelGUI omakeInfoLabelGUI = new GachaWindowInfoCtrl.OmakeInfoLabelGUI(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts03_Item"), tabInnerFrameGUI2.Base.transform).transform);
					ItemStaticBase isb = DataManager.DmItem.GetItemStaticBase(gachaItemdata.itemId);
					omakeInfoLabelGUI.Icon_Item.Setup(isb, new IconItemCtrl.SetupParam
					{
						useMaxDetail = true
					});
					omakeInfoLabelGUI.IconAllObj.SetActive(false);
					omakeInfoLabelGUI.RarityText.gameObject.SetActive(false);
					omakeInfoLabelGUI.DisableImage.gameObject.SetActive(false);
					int num = 0;
					string text2 = string.Empty;
					string text3 = string.Empty;
					ItemDef.Kind kind = isb.GetKind();
					if (kind == ItemDef.Kind.CHARA)
					{
						CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(DataManager.DmItem.ItemId2ChraId(gachaItemdata.itemId));
						text2 = charaStaticData.baseData.NickName;
						omakeInfoLabelGUI.IconAllObj.SetActive(true);
						using (List<PguiImageCtrl>.Enumerator enumerator3 = omakeInfoLabelGUI.IconAllList.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								PguiImageCtrl pguiImageCtrl = enumerator3.Current;
								pguiImageCtrl.gameObject.SetActive(num < charaStaticData.baseData.rankLow);
								num++;
							}
							goto IL_05F8;
						}
						goto IL_0507;
					}
					if (kind == ItemDef.Kind.PHOTO)
					{
						goto IL_0507;
					}
					if (kind == ItemDef.Kind.TREEHOUSE_FURNITURE)
					{
						goto IL_0567;
					}
					omakeInfoLabelGUI.RarityText.gameObject.SetActive(true);
					omakeInfoLabelGUI.RarityText.text = PrjUtil.Rarity2String((int)isb.GetRarity());
					text3 = string.Format("×{0}", gachaItemdata.itemNum);
					IL_05F8:
					omakeInfoLabelGUI.Name.ReplaceTextByDefault(new string[] { "Param01", "Param02", "Param03" }, new string[]
					{
						text2,
						omakeInfoLabelGUI.Icon_Item.itemStaticBase.GetName(),
						text3
					});
					omakeInfoLabelGUI.Mark_Limited.gameObject.SetActive(gachaItemdata.dispClientTypeLimit != 0);
					omakeInfoLabelGUI.Mark_New.gameObject.SetActive(gachaItemdata.dispClientTypeLimit == 0 && gachaItemdata.dispClientTypeNew != 0);
					if (gachaItemdata.EnableBonusItem01)
					{
						IconItemCtrl component2 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, omakeInfoLabelGUI.baseObj.transform.Find("Base/Get_Item/Icon_Item01")).GetComponent<IconItemCtrl>();
						this.<SetupOptionList>g__SetupBonusIcon|25_0(component2, gachaItemdata.bonusItemId01, gachaItemdata.bonusItemNum01);
					}
					if (gachaItemdata.EnableBonusItem02)
					{
						IconItemCtrl component3 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, omakeInfoLabelGUI.baseObj.transform.Find("Base/Get_Item/Icon_Item02")).GetComponent<IconItemCtrl>();
						this.<SetupOptionList>g__SetupBonusIcon|25_0(component3, gachaItemdata.bonusItemId02, gachaItemdata.bonusItemNum02);
					}
					if (gachaItemdata.EnableBonusItem03)
					{
						IconItemCtrl component4 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, omakeInfoLabelGUI.baseObj.transform.Find("Base/Get_Item/Icon_Item03")).GetComponent<IconItemCtrl>();
						this.<SetupOptionList>g__SetupBonusIcon|25_0(component4, gachaItemdata.bonusItemId03, gachaItemdata.bonusItemNum03);
						continue;
					}
					continue;
					IL_0507:
					omakeInfoLabelGUI.IconAllObj.SetActive(true);
					using (List<PguiImageCtrl>.Enumerator enumerator3 = omakeInfoLabelGUI.IconAllList.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							PguiImageCtrl pguiImageCtrl2 = enumerator3.Current;
							pguiImageCtrl2.gameObject.SetActive(num < (int)isb.GetRarity());
							num++;
						}
						goto IL_05F8;
					}
					IL_0567:
					omakeInfoLabelGUI.Icon_Item.AddOnLongClickListener(delegate(IconItemCtrl x)
					{
						CS$<>8__locals1.<>4__this.OpenTreeHouseFurnitureWindow(isb);
					});
					omakeInfoLabelGUI.RarityText.gameObject.SetActive(true);
					omakeInfoLabelGUI.RarityText.text = PrjUtil.Rarity2String((int)isb.GetRarity());
					goto IL_05F8;
				}
			}
		}
	}

	// Token: 0x060011B1 RID: 4529 RVA: 0x000D86C8 File Offset: 0x000D68C8
	private void RefleshGachaDetailWindow()
	{
		bool flag = true;
		int num = 0;
		DataManagerGacha.GachaStaticData gachaStaticData = this.detailWindowGuiData.SelectGachaStaticData;
		while (flag)
		{
			if (gachaStaticData.stepPreviousGachaId != 0)
			{
				num++;
				gachaStaticData = DataManager.DmGacha.GetGachaStaticData(gachaStaticData.stepPreviousGachaId);
			}
			else
			{
				flag = false;
			}
		}
		bool flag2 = num != 0 || this.detailWindowGuiData.SelectGachaStaticData.stepNextGachaId != 0;
		switch (this.detailWindowGuiData.SelectGachaStaticData.gachaCategory)
		{
		case DataManagerGacha.Category.KiraKira:
		case DataManagerGacha.Category.Active:
		case DataManagerGacha.Category.SPECIAL:
		case DataManagerGacha.Category.MonthlyPack:
			this.detailWindowGuiData.StepNum.text = string.Empty;
			break;
		case DataManagerGacha.Category.StepUp:
			this.detailWindowGuiData.StepNum.text = string.Format("ステップ<size=32>{0}</size>", num + 1);
			break;
		case DataManagerGacha.Category.Box:
			this.detailWindowGuiData.StepNum.text = string.Format("ぼっくす<size=32>{0}</size>", num + 1);
			break;
		case DataManagerGacha.Category.Roulette:
			this.detailWindowGuiData.StepNum.text = "あとN回無料！";
			break;
		default:
			this.detailWindowGuiData.StepNum.text = string.Format("<size=32>{0}</size>", num + 1);
			break;
		}
		this.detailWindowGuiData.StepNum.gameObject.SetActive(flag2);
		this.detailWindowGuiData.LeftArrow.gameObject.SetActive(this.detailWindowGuiData.SelectGachaStaticData.stepPreviousGachaId != 0);
		this.detailWindowGuiData.RightArrow.gameObject.SetActive(this.detailWindowGuiData.SelectGachaStaticData.stepNextGachaId != 0);
		this.SetupRateList();
		this.SetupFriendsPhotoTreeHouseFurnitureItemList();
		this.SetupOptionList();
	}

	// Token: 0x060011B2 RID: 4530 RVA: 0x000D886E File Offset: 0x000D6A6E
	private IEnumerator UpdateDetailWindow(int gachaId)
	{
		if (gachaId == 0)
		{
			yield break;
		}
		DataManager.DmGacha.RequestActionRateView(gachaId);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.detailWindowGuiData.SelectGachaStaticData = DataManager.DmGacha.GetGachaStaticData(gachaId);
		this.RefleshGachaDetailWindow();
		yield break;
	}

	// Token: 0x060011B3 RID: 4531 RVA: 0x000D8884 File Offset: 0x000D6A84
	public void Close()
	{
		if (this.m_Sequence == GachaWindowInfoCtrl.Sequence.ACTIVE)
		{
			this.m_ReqSequence = GachaWindowInfoCtrl.Sequence.CLOSE_START;
		}
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x000D8898 File Offset: 0x000D6A98
	private void OnClickArrowButton(PguiButtonCtrl button, int direction)
	{
		if (direction == 0)
		{
			this.updateGachaDetail = this.UpdateDetailWindow(this.detailWindowGuiData.SelectGachaStaticData.stepPreviousGachaId);
			return;
		}
		if (direction != 1)
		{
			return;
		}
		this.updateGachaDetail = this.UpdateDetailWindow(this.detailWindowGuiData.SelectGachaStaticData.stepNextGachaId);
	}

	// Token: 0x060011B5 RID: 4533 RVA: 0x000D88E6 File Offset: 0x000D6AE6
	private void OnClickCloseButton(PguiButtonCtrl button)
	{
		if (button == this.detailWindowGuiData.BtnClose)
		{
			this.Close();
		}
	}

	// Token: 0x060011B6 RID: 4534 RVA: 0x000D8904 File Offset: 0x000D6B04
	private bool OnSelectTab(int index)
	{
		this.currentTab = (GachaWindowInfoCtrl.TabType)index;
		foreach (GachaWindowInfoCtrl.GachaInfoTab gachaInfoTab in this.detailWindowGuiData.gachaDetailTabMap.Values)
		{
			gachaInfoTab.baseObj.SetActive(false);
		}
		this.detailWindowGuiData.gachaDetailTabMap[this.currentTab].baseObj.SetActive(true);
		return true;
	}

	// Token: 0x060011B7 RID: 4535 RVA: 0x000D8990 File Offset: 0x000D6B90
	private void Update()
	{
		switch (this.m_Sequence)
		{
		case GachaWindowInfoCtrl.Sequence.INACTIVE:
			if (this.m_ReqSequence == GachaWindowInfoCtrl.Sequence.OPEN_START)
			{
				this.m_Sequence = GachaWindowInfoCtrl.Sequence.OPEN_START;
			}
			break;
		case GachaWindowInfoCtrl.Sequence.OPEN_START:
			this.detailWindowGuiData.InBase.SetActive(true);
			this.detailWindowGuiData.BaseAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			this.m_Sequence = GachaWindowInfoCtrl.Sequence.OPEN_WAIT;
			break;
		case GachaWindowInfoCtrl.Sequence.OPEN_WAIT:
			if (!this.detailWindowGuiData.BaseAnim.ExIsPlaying())
			{
				this.m_Sequence = GachaWindowInfoCtrl.Sequence.ACTIVE;
			}
			break;
		case GachaWindowInfoCtrl.Sequence.ACTIVE:
			if (this.m_ReqSequence == GachaWindowInfoCtrl.Sequence.CALLBACK_ACTION)
			{
				SoundManager.Play("prd_se_dialog_close", false, false);
				this.m_Sequence = GachaWindowInfoCtrl.Sequence.CALLBACK_ACTION;
			}
			else if (this.m_ReqSequence == GachaWindowInfoCtrl.Sequence.CLOSE_START)
			{
				this.m_Sequence = GachaWindowInfoCtrl.Sequence.CLOSE_START;
			}
			break;
		case GachaWindowInfoCtrl.Sequence.CALLBACK_ACTION:
			this.m_Sequence = GachaWindowInfoCtrl.Sequence.CLOSE_START;
			break;
		case GachaWindowInfoCtrl.Sequence.CLOSE_START:
			this.detailWindowGuiData.BaseAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			this.m_Sequence = GachaWindowInfoCtrl.Sequence.CLOSE_WAIT;
			break;
		case GachaWindowInfoCtrl.Sequence.CLOSE_WAIT:
			if (!this.detailWindowGuiData.BaseAnim.ExIsPlaying())
			{
				UnityAction callBack = this.m_CallBack;
				if (callBack != null)
				{
					callBack();
				}
				this.detailWindowGuiData.InBase.SetActive(false);
				this.m_Sequence = GachaWindowInfoCtrl.Sequence.INACTIVE;
			}
			break;
		}
		if (this.updateGachaDetail != null && !this.updateGachaDetail.MoveNext())
		{
			this.updateGachaDetail = null;
		}
		this.m_ReqSequence = GachaWindowInfoCtrl.Sequence.NONE;
	}

	// Token: 0x060011B8 RID: 4536 RVA: 0x000D8AE8 File Offset: 0x000D6CE8
	private DataManagerGacha.ProbabilityData.Type TabType2ProbabilityType(GachaWindowInfoCtrl.TabType type)
	{
		switch (type)
		{
		case GachaWindowInfoCtrl.TabType.TabFriends:
			return DataManagerGacha.ProbabilityData.Type.Chara;
		case GachaWindowInfoCtrl.TabType.TabPhoto:
			return DataManagerGacha.ProbabilityData.Type.Photo;
		case GachaWindowInfoCtrl.TabType.TabTreeHouseFurniture:
			return DataManagerGacha.ProbabilityData.Type.TreeHouseFurniture;
		case GachaWindowInfoCtrl.TabType.TabItem:
			return DataManagerGacha.ProbabilityData.Type.Item;
		}
		return DataManagerGacha.ProbabilityData.Type.Undefined;
	}

	// Token: 0x060011B9 RID: 4537 RVA: 0x000D8B28 File Offset: 0x000D6D28
	private bool IsDecidedFour(DataManagerGacha.GachaStaticTypeData typeData)
	{
		return typeData.lastTimeBenefitRarity == 0 && typeData.lastTimeBenefitRarity4 != 0;
	}

	// Token: 0x060011BA RID: 4538 RVA: 0x000D8B3D File Offset: 0x000D6D3D
	private bool IsDecidedRarity(DataManagerGacha.GachaStaticTypeData typeData)
	{
		return typeData.lastTimeBenefitRarity != 0 || typeData.lastTimeBenefitRarity4 != 0;
	}

	// Token: 0x060011BB RID: 4539 RVA: 0x000D8B54 File Offset: 0x000D6D54
	private void OpenTreeHouseFurnitureWindow(ItemStaticBase isb)
	{
		TreeHouseFurnitureStatic treeHouseFurnitureStaticData = DataManager.DmTreeHouse.GetTreeHouseFurnitureStaticData(isb.GetId());
		CanvasManager.HdlTreeHouseFurnitureWindowCtrl.Open(new TreeHouseFurnitureWindowCtrl.SetupParam
		{
			thfs = treeHouseFurnitureStaticData
		});
	}

	// Token: 0x060011BF RID: 4543 RVA: 0x000D8BAC File Offset: 0x000D6DAC
	[CompilerGenerated]
	private void <SetupOptionList>g__SetupBonusIcon|25_0(IconItemCtrl itemIcon, int id, int num)
	{
		if (null == itemIcon)
		{
			return;
		}
		ItemStaticBase isb = DataManager.DmItem.GetItemStaticBase(id);
		if (isb == null)
		{
			itemIcon.Setup(isb);
			return;
		}
		ItemDef.Kind kind = isb.GetKind();
		if (kind - ItemDef.Kind.CHARA <= 1 || kind == ItemDef.Kind.ACCESSORY_ITEM)
		{
			itemIcon.Setup(isb, new IconItemCtrl.SetupParam
			{
				useMaxDetail = true
			});
			return;
		}
		if (kind != ItemDef.Kind.TREEHOUSE_FURNITURE)
		{
			itemIcon.Setup(isb, num, new IconItemCtrl.SetupParam
			{
				useInfo = true
			});
			return;
		}
		itemIcon.Setup(isb);
		itemIcon.AddOnLongClickListener(delegate(IconItemCtrl x)
		{
			this.OpenTreeHouseFurnitureWindow(isb);
		});
	}

	// Token: 0x060011C0 RID: 4544 RVA: 0x000D8C68 File Offset: 0x000D6E68
	[CompilerGenerated]
	internal static void <SetupOptionList>g__SetupBonusIconItemSet|25_1(IconItemCtrl itemIcon, int id)
	{
		if (null == itemIcon)
		{
			return;
		}
		itemIcon.Setup(DataManager.DmItem.GetItemStaticBase(id), 0, new IconItemCtrl.SetupParam
		{
			useInfo = true,
			viewItemCount = false
		});
		ItemData itemData = new ItemData(id, 0);
		itemIcon.AddOnClickListener(delegate(IconItemCtrl x)
		{
			CanvasManager.HdlItemPresetWindowCtrl.OpenByItem(itemData);
		});
	}

	// Token: 0x04000EE9 RID: 3817
	private GachaWindowInfoCtrl.GachaDetailInfoWindowGUI detailWindowGuiData;

	// Token: 0x04000EEA RID: 3818
	private IEnumerator updateGachaDetail;

	// Token: 0x04000EEB RID: 3819
	private const int LineCount = 3;

	// Token: 0x04000EEC RID: 3820
	private const int ColumnCount = 4;

	// Token: 0x04000EED RID: 3821
	private GachaWindowInfoCtrl.Sequence m_ReqSequence;

	// Token: 0x04000EEE RID: 3822
	private GachaWindowInfoCtrl.Sequence m_Sequence = GachaWindowInfoCtrl.Sequence.INACTIVE;

	// Token: 0x04000EEF RID: 3823
	private GachaWindowInfoCtrl.TabType currentTab;

	// Token: 0x04000EF0 RID: 3824
	private UnityAction m_CallBack;

	// Token: 0x02000AA7 RID: 2727
	public class GachaDetailInfoWindowGUI
	{
		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x0600401E RID: 16414 RVA: 0x001F4652 File Offset: 0x001F2852
		// (set) Token: 0x0600401F RID: 16415 RVA: 0x001F465A File Offset: 0x001F285A
		public PguiButtonCtrl LeftArrow { get; set; }

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06004020 RID: 16416 RVA: 0x001F4663 File Offset: 0x001F2863
		// (set) Token: 0x06004021 RID: 16417 RVA: 0x001F466B File Offset: 0x001F286B
		public PguiButtonCtrl RightArrow { get; set; }

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06004022 RID: 16418 RVA: 0x001F4674 File Offset: 0x001F2874
		// (set) Token: 0x06004023 RID: 16419 RVA: 0x001F467C File Offset: 0x001F287C
		public PguiTextCtrl StepNum { get; set; }

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06004025 RID: 16421 RVA: 0x001F468E File Offset: 0x001F288E
		// (set) Token: 0x06004024 RID: 16420 RVA: 0x001F4685 File Offset: 0x001F2885
		public DataManagerGacha.GachaStaticData SelectGachaStaticData { get; set; }

		// Token: 0x06004026 RID: 16422 RVA: 0x001F4698 File Offset: 0x001F2898
		public GachaDetailInfoWindowGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.InBase = baseTr.Find("Base").gameObject;
			this.BtnClose = baseTr.Find("Base/Window/BtnClose").GetComponent<PguiButtonCtrl>();
			this.TabGroup = baseTr.Find("Base/Window/Base/TabGroup").GetComponent<PguiTabGroupCtrl>();
			this.gachaDetailTabMap = new Dictionary<GachaWindowInfoCtrl.TabType, GachaWindowInfoCtrl.GachaInfoTab>();
			this.gachaDetailTabMap[GachaWindowInfoCtrl.TabType.TabRate] = new GachaWindowInfoCtrl.GachaInfoTab(baseTr.Find("Base/Window/Base/Info_Gachadrop"));
			this.gachaDetailTabMap[GachaWindowInfoCtrl.TabType.TabFriends] = new GachaWindowInfoCtrl.GachaInfoTab(baseTr.Find("Base/Window/Base/Info_Friends"));
			this.gachaDetailTabMap[GachaWindowInfoCtrl.TabType.TabPhoto] = new GachaWindowInfoCtrl.GachaInfoTab(baseTr.Find("Base/Window/Base/Info_Photo"));
			this.gachaDetailTabMap[GachaWindowInfoCtrl.TabType.TabTreeHouseFurniture] = new GachaWindowInfoCtrl.GachaInfoTab(baseTr.Find("Base/Window/Base/Info_Interior"));
			this.gachaDetailTabMap[GachaWindowInfoCtrl.TabType.TabItem] = new GachaWindowInfoCtrl.GachaInfoTab(baseTr.Find("Base/Window/Base/Info_Item"));
			this.gachaDetailTabMap[GachaWindowInfoCtrl.TabType.TabOption] = new GachaWindowInfoCtrl.GachaInfoTab(baseTr.Find("Base/Window/Base/Info_Option"));
			this.LeftArrow = baseTr.Find("Base/Window/LeftBtn/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
			this.RightArrow = baseTr.Find("Base/Window/RightBtn/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
			this.StepNum = baseTr.Find("Base/Window/Num_Step").GetComponent<PguiTextCtrl>();
			this.BtnClose.androidBackKeyTarget = true;
			this.BaseAnim = this.InBase.GetComponent<SimpleAnimation>();
		}

		// Token: 0x040043DB RID: 17371
		public GameObject baseObj;

		// Token: 0x040043DC RID: 17372
		public GameObject InBase;

		// Token: 0x040043DD RID: 17373
		public PguiButtonCtrl BtnClose;

		// Token: 0x040043DE RID: 17374
		public PguiTabGroupCtrl TabGroup;

		// Token: 0x040043DF RID: 17375
		public Dictionary<GachaWindowInfoCtrl.TabType, GachaWindowInfoCtrl.GachaInfoTab> gachaDetailTabMap;

		// Token: 0x040043E3 RID: 17379
		public SimpleAnimation BaseAnim;
	}

	// Token: 0x02000AA8 RID: 2728
	public class GachaInfoTab
	{
		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06004027 RID: 16423 RVA: 0x001F480B File Offset: 0x001F2A0B
		// (set) Token: 0x06004028 RID: 16424 RVA: 0x001F4813 File Offset: 0x001F2A13
		public PguiTextCtrl NoneText { get; set; }

		// Token: 0x06004029 RID: 16425 RVA: 0x001F481C File Offset: 0x001F2A1C
		public GachaInfoTab(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.ScrollRect = baseTr.Find("ScrollView").GetComponent<ScrollRect>();
			this.ScrollRect.scrollSensitivity = ScrollParamDefine.GachaInfo;
			this.Viewport = baseTr.Find("ScrollView/Viewport").GetComponent<RectTransform>();
			this.Content = baseTr.Find("ScrollView/Viewport/Content").GetComponent<RectTransform>();
			Transform transform = baseTr.Find("Txt_None");
			this.NoneText = ((transform != null) ? transform.GetComponent<PguiTextCtrl>() : null);
			if (null != this.NoneText)
			{
				this.NoneText.gameObject.SetActive(false);
			}
		}

		// Token: 0x040043E5 RID: 17381
		public GameObject baseObj;

		// Token: 0x040043E6 RID: 17382
		public ScrollRect ScrollRect;

		// Token: 0x040043E7 RID: 17383
		public RectTransform Viewport;

		// Token: 0x040043E8 RID: 17384
		public RectTransform Content;
	}

	// Token: 0x02000AA9 RID: 2729
	public class TabInnerFrameGUI
	{
		// Token: 0x0600402A RID: 16426 RVA: 0x001F48CC File Offset: 0x001F2ACC
		public TabInnerFrameGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Base = baseTr.Find("Base").GetComponent<PguiImageCtrl>();
			this.Title = baseTr.Find("Base/Title").GetComponent<PguiTextCtrl>();
			this.Rect = this.Base.GetComponent<RectTransform>();
		}

		// Token: 0x040043EA RID: 17386
		public GameObject baseObj;

		// Token: 0x040043EB RID: 17387
		public PguiImageCtrl Base;

		// Token: 0x040043EC RID: 17388
		public PguiTextCtrl Title;

		// Token: 0x040043ED RID: 17389
		public RectTransform Rect;
	}

	// Token: 0x02000AAA RID: 2730
	public class RarityStarBar
	{
		// Token: 0x0600402B RID: 16427 RVA: 0x001F4928 File Offset: 0x001F2B28
		public RarityStarBar(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.IconAllObj = baseTr.Find("Icon_All").gameObject;
			this.IconAllList = new List<PguiImageCtrl>();
			for (int i = 0; i < 4; i++)
			{
				this.IconAllList.Add(baseTr.Find("Icon_All/Icon" + (i + 1).ToString()).GetComponent<PguiImageCtrl>());
			}
			this.RarityText = baseTr.Find("Txt_Rarity").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x040043EE RID: 17390
		public GameObject baseObj;

		// Token: 0x040043EF RID: 17391
		public GameObject IconAllObj;

		// Token: 0x040043F0 RID: 17392
		public List<PguiImageCtrl> IconAllList;

		// Token: 0x040043F1 RID: 17393
		public PguiTextCtrl RarityText;
	}

	// Token: 0x02000AAB RID: 2731
	public class ItemInfoLabelGUI
	{
		// Token: 0x0600402C RID: 16428 RVA: 0x001F49B8 File Offset: 0x001F2BB8
		public ItemInfoLabelGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.IconAllObj = baseTr.Find("Base/Icon_All").gameObject;
			this.IconAllList = new List<PguiImageCtrl>();
			for (int i = 0; i < 4; i++)
			{
				this.IconAllList.Add(baseTr.Find("Base/Icon_All/Icon" + (i + 1).ToString()).GetComponent<PguiImageCtrl>());
			}
			this.RarityText = baseTr.Find("Base/Txt_Rarity").GetComponent<PguiTextCtrl>();
			this.RateNormal = baseTr.Find("Base/Num01").GetComponent<PguiTextCtrl>();
			this.RateDecided3 = baseTr.Find("Base/Num02").GetComponent<PguiTextCtrl>();
			this.RateDecidedFriends = baseTr.Find("Base/Num03").GetComponent<PguiTextCtrl>();
			this.RateDecidedCeiling = baseTr.Find("Base/Num04").GetComponent<PguiTextCtrl>();
			this.BoxStack = baseTr.Find("Base/Num_BoxItem").GetComponent<PguiTextCtrl>();
			this.RateNormalText = baseTr.Find("Base/Txt01").gameObject;
			this.RateDecided3Text = baseTr.Find("Base/Txt02").gameObject;
			this.RateDecidedFriendsText = baseTr.Find("Base/Txt03").gameObject;
			this.RateDecidedCeilingText = baseTr.Find("Base/Txt04").gameObject;
			this.Name = baseTr.Find("Base/Name").GetComponent<PguiTextCtrl>();
			this.Icon_Item = baseTr.Find("Base/Icon_Item/Icon_Item").GetComponent<IconItemCtrl>();
			this.Mark_Limited = baseTr.Find("Base/Mark_Limited").GetComponent<PguiImageCtrl>();
			this.Mark_New = baseTr.Find("Base/Mark_New").GetComponent<PguiImageCtrl>();
			this.DisableImage = baseTr.Find("Base/Disable").GetComponent<PguiImageCtrl>();
		}

		// Token: 0x040043F2 RID: 17394
		public GameObject baseObj;

		// Token: 0x040043F3 RID: 17395
		public GameObject IconAllObj;

		// Token: 0x040043F4 RID: 17396
		public List<PguiImageCtrl> IconAllList;

		// Token: 0x040043F5 RID: 17397
		public PguiTextCtrl RarityText;

		// Token: 0x040043F6 RID: 17398
		public PguiTextCtrl RateNormal;

		// Token: 0x040043F7 RID: 17399
		public PguiTextCtrl RateDecided3;

		// Token: 0x040043F8 RID: 17400
		public PguiTextCtrl RateDecidedFriends;

		// Token: 0x040043F9 RID: 17401
		public PguiTextCtrl RateDecidedCeiling;

		// Token: 0x040043FA RID: 17402
		public PguiTextCtrl BoxStack;

		// Token: 0x040043FB RID: 17403
		public GameObject RateNormalText;

		// Token: 0x040043FC RID: 17404
		public GameObject RateDecided3Text;

		// Token: 0x040043FD RID: 17405
		public GameObject RateDecidedFriendsText;

		// Token: 0x040043FE RID: 17406
		public GameObject RateDecidedCeilingText;

		// Token: 0x040043FF RID: 17407
		public PguiTextCtrl Name;

		// Token: 0x04004400 RID: 17408
		public IconItemCtrl Icon_Item;

		// Token: 0x04004401 RID: 17409
		public PguiImageCtrl Mark_Limited;

		// Token: 0x04004402 RID: 17410
		public PguiImageCtrl Mark_New;

		// Token: 0x04004403 RID: 17411
		public PguiImageCtrl DisableImage;
	}

	// Token: 0x02000AAC RID: 2732
	public class OmakeInfoLabelGUI
	{
		// Token: 0x0600402D RID: 16429 RVA: 0x001F4B7C File Offset: 0x001F2D7C
		public OmakeInfoLabelGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.IconAllObj = baseTr.Find("Base/Icon_All").gameObject;
			this.IconAllList = new List<PguiImageCtrl>();
			for (int i = 0; i < 4; i++)
			{
				this.IconAllList.Add(baseTr.Find("Base/Icon_All/Icon" + (i + 1).ToString()).GetComponent<PguiImageCtrl>());
			}
			this.RarityText = baseTr.Find("Base/Txt_Rarity").GetComponent<PguiTextCtrl>();
			this.Name = baseTr.Find("Base/Name").GetComponent<PguiTextCtrl>();
			this.Icon_Item = baseTr.Find("Base/Icon_Item/Icon_Item").GetComponent<IconItemCtrl>();
			this.Mark_Limited = baseTr.Find("Base/Mark_Limited").GetComponent<PguiImageCtrl>();
			this.Mark_New = baseTr.Find("Base/Mark_New").GetComponent<PguiImageCtrl>();
			this.DisableImage = baseTr.Find("Base/Disable").GetComponent<PguiImageCtrl>();
		}

		// Token: 0x04004404 RID: 17412
		public GameObject baseObj;

		// Token: 0x04004405 RID: 17413
		public GameObject IconAllObj;

		// Token: 0x04004406 RID: 17414
		public List<PguiImageCtrl> IconAllList;

		// Token: 0x04004407 RID: 17415
		public PguiTextCtrl RarityText;

		// Token: 0x04004408 RID: 17416
		public PguiTextCtrl Name;

		// Token: 0x04004409 RID: 17417
		public IconItemCtrl Icon_Item;

		// Token: 0x0400440A RID: 17418
		public PguiImageCtrl Mark_Limited;

		// Token: 0x0400440B RID: 17419
		public PguiImageCtrl Mark_New;

		// Token: 0x0400440C RID: 17420
		public PguiImageCtrl DisableImage;
	}

	// Token: 0x02000AAD RID: 2733
	public class TotalRatioLabelGUI
	{
		// Token: 0x0600402E RID: 16430 RVA: 0x001F4C78 File Offset: 0x001F2E78
		public TotalRatioLabelGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.NumList = new List<PguiTextCtrl>();
			for (int i = 1; i <= 3; i++)
			{
				for (int j = 1; j <= 4; j++)
				{
					string text = "Base/NumAll" + j.ToString("D2") + "/Num" + i.ToString("D2");
					this.NumList.Add(baseTr.Find(text).GetComponent<PguiTextCtrl>());
				}
			}
			this.Txt02 = baseTr.Find("Base/Txt02").GetComponent<PguiTextCtrl>();
			this.NumAll03 = baseTr.Find("Base/NumAll03").gameObject;
			this.Txt03 = baseTr.Find("Base/Txt03").gameObject;
			this.NumAll04 = baseTr.Find("Base/NumAll04").gameObject;
			this.Txt04 = baseTr.Find("Base/Txt04").gameObject;
			this.LabelTitle = baseTr.Find("Base/TitleBase/Txt").GetComponent<PguiTextCtrl>();
			this.StarBase = baseTr.Find("Base/IconAll").gameObject;
			this.RarityBase = baseTr.Find("Base/Rarity_All").gameObject;
		}

		// Token: 0x0400440D RID: 17421
		public GameObject baseObj;

		// Token: 0x0400440E RID: 17422
		public List<PguiTextCtrl> NumList;

		// Token: 0x0400440F RID: 17423
		public PguiTextCtrl Txt02;

		// Token: 0x04004410 RID: 17424
		public GameObject NumAll03;

		// Token: 0x04004411 RID: 17425
		public GameObject Txt03;

		// Token: 0x04004412 RID: 17426
		public GameObject NumAll04;

		// Token: 0x04004413 RID: 17427
		public GameObject Txt04;

		// Token: 0x04004414 RID: 17428
		public PguiTextCtrl LabelTitle;

		// Token: 0x04004415 RID: 17429
		public GameObject StarBase;

		// Token: 0x04004416 RID: 17430
		public GameObject RarityBase;
	}

	// Token: 0x02000AAE RID: 2734
	public class RatioLabelFriendsPhotoGUI
	{
		// Token: 0x0600402F RID: 16431 RVA: 0x001F4DAC File Offset: 0x001F2FAC
		public RatioLabelFriendsPhotoGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.charaNumList = new List<PguiTextCtrl>
			{
				null,
				null,
				baseTr.Find("Friends/NumAll01/Num03").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Friends/NumAll01/Num02").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Friends/NumAll01/Num01").GetComponent<PguiTextCtrl>(),
				null,
				null
			};
			this.photoNumList = new List<PguiTextCtrl>
			{
				null,
				null,
				baseTr.Find("Photo/NumAll01/Num03").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Photo/NumAll01/Num02").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Photo/NumAll01/Num01").GetComponent<PguiTextCtrl>(),
				null,
				null
			};
		}

		// Token: 0x04004417 RID: 17431
		public GameObject baseObj;

		// Token: 0x04004418 RID: 17432
		public List<PguiTextCtrl> charaNumList;

		// Token: 0x04004419 RID: 17433
		public List<PguiTextCtrl> photoNumList;
	}

	// Token: 0x02000AAF RID: 2735
	public class RatioLabelItemGUI
	{
		// Token: 0x06004030 RID: 16432 RVA: 0x001F4EA0 File Offset: 0x001F30A0
		public RatioLabelItemGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Txt_Kind = baseTr.Find("Base/TitleBase/Txt").GetComponent<PguiTextCtrl>();
			this.itemNumList = new List<PguiTextCtrl>
			{
				null,
				baseTr.Find("Base/Num05").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Num04").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Num03").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Num02").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Num01").GetComponent<PguiTextCtrl>()
			};
		}

		// Token: 0x0400441A RID: 17434
		public GameObject baseObj;

		// Token: 0x0400441B RID: 17435
		public PguiTextCtrl Txt_Kind;

		// Token: 0x0400441C RID: 17436
		public List<PguiTextCtrl> itemNumList;
	}

	// Token: 0x02000AB0 RID: 2736
	public class DetailMessageGUI
	{
		// Token: 0x06004031 RID: 16433 RVA: 0x001F4F55 File Offset: 0x001F3155
		public DetailMessageGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.DetailText = baseTr.Find("Txt").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x0400441D RID: 17437
		public GameObject baseObj;

		// Token: 0x0400441E RID: 17438
		public PguiTextCtrl DetailText;
	}

	// Token: 0x02000AB1 RID: 2737
	public enum TabType
	{
		// Token: 0x04004420 RID: 17440
		TabRate,
		// Token: 0x04004421 RID: 17441
		TabFriends,
		// Token: 0x04004422 RID: 17442
		TabPhoto,
		// Token: 0x04004423 RID: 17443
		TabTreeHouseFurniture,
		// Token: 0x04004424 RID: 17444
		TabItem,
		// Token: 0x04004425 RID: 17445
		TabOption
	}

	// Token: 0x02000AB2 RID: 2738
	private enum Sequence
	{
		// Token: 0x04004427 RID: 17447
		NONE,
		// Token: 0x04004428 RID: 17448
		INACTIVE,
		// Token: 0x04004429 RID: 17449
		OPEN_START,
		// Token: 0x0400442A RID: 17450
		OPEN_WAIT,
		// Token: 0x0400442B RID: 17451
		ACTIVE,
		// Token: 0x0400442C RID: 17452
		CALLBACK_ACTION,
		// Token: 0x0400442D RID: 17453
		CLOSE_START,
		// Token: 0x0400442E RID: 17454
		CLOSE_WAIT
	}
}
