using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000141 RID: 321
public class GachaWindowBoxInfoCtrl : MonoBehaviour
{
	// Token: 0x060011A2 RID: 4514 RVA: 0x000D58AD File Offset: 0x000D3AAD
	private void Update()
	{
		if (this.IEWindowMove != null && !this.IEWindowMove.MoveNext())
		{
			this.IEWindowMove = null;
		}
	}

	// Token: 0x060011A3 RID: 4515 RVA: 0x000D58CC File Offset: 0x000D3ACC
	public void Initialize()
	{
		this.gachaWindowBoxInfoGuiData = new GachaWindowBoxInfoCtrl.GachaWindowBoxInfoGUI(base.transform);
		this.gachaWindowBoxInfoGuiData.BaseAnim.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.gachaWindowBoxInfoGuiData.InBase.SetActive(false);
		this.gachaWindowBoxInfoGuiData.BtnClose.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnCliskCloseButton), PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x060011A4 RID: 4516 RVA: 0x000D592A File Offset: 0x000D3B2A
	private void OnCliskCloseButton(PguiButtonCtrl button)
	{
		this.Close(null);
	}

	// Token: 0x060011A5 RID: 4517 RVA: 0x000D5933 File Offset: 0x000D3B33
	public void Open(DataManagerGacha.GachaStaticData staticData, UnityAction openEndCb = null, UnityAction closeEndCb = null)
	{
		if (DataManagerGacha.Category.Box != staticData.gachaCategory)
		{
			return;
		}
		if (this.IEWindowMove == null)
		{
			this.currentGachaStaticData = staticData;
			this.IEWindowMove = this.OpenWindow(openEndCb);
			this.windowCloseEndCb = closeEndCb;
		}
	}

	// Token: 0x060011A6 RID: 4518 RVA: 0x000D5962 File Offset: 0x000D3B62
	private void Close(UnityAction closeEndCb = null)
	{
		if (this.IEWindowMove == null)
		{
			this.IEWindowMove = this.CloseWindow(closeEndCb);
		}
	}

	// Token: 0x060011A7 RID: 4519 RVA: 0x000D5979 File Offset: 0x000D3B79
	private IEnumerator OpenWindow(UnityAction openEndCb)
	{
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.RefreshBoxRateInfo();
		this.gachaWindowBoxInfoGuiData.InBase.SetActive(true);
		this.gachaWindowBoxInfoGuiData.BaseAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		bool isLoop = true;
		while (isLoop)
		{
			isLoop = this.gachaWindowBoxInfoGuiData.BaseAnim.ExIsPlaying();
			yield return null;
		}
		if (openEndCb != null)
		{
			openEndCb();
		}
		yield break;
	}

	// Token: 0x060011A8 RID: 4520 RVA: 0x000D598F File Offset: 0x000D3B8F
	private IEnumerator CloseWindow(UnityAction innerCloseEndCb = null)
	{
		this.gachaWindowBoxInfoGuiData.BaseAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
		bool isLoop = true;
		while (isLoop)
		{
			isLoop = this.gachaWindowBoxInfoGuiData.BaseAnim.ExIsPlaying();
			yield return null;
		}
		this.gachaWindowBoxInfoGuiData.InBase.SetActive(false);
		if (innerCloseEndCb != null)
		{
			innerCloseEndCb();
		}
		UnityAction unityAction = this.windowCloseEndCb;
		if (unityAction != null)
		{
			unityAction();
		}
		this.windowCloseEndCb = null;
		this.currentGachaStaticData = null;
		yield break;
	}

	// Token: 0x060011A9 RID: 4521 RVA: 0x000D59A8 File Offset: 0x000D3BA8
	private void RefreshBoxRateInfo()
	{
		DataManagerGacha.ProbabilityData lastRequestRateViewData = DataManager.DmGacha.GetLastRequestRateViewData();
		if (lastRequestRateViewData == null)
		{
			return;
		}
		foreach (object obj in this.gachaWindowBoxInfoGuiData.ScrollContent.transform)
		{
			Object.Destroy(((Transform)obj).gameObject);
		}
		List<DataManagerGacha.ProbabilityData.Type> list = new List<DataManagerGacha.ProbabilityData.Type>
		{
			DataManagerGacha.ProbabilityData.Type.Chara,
			DataManagerGacha.ProbabilityData.Type.Photo,
			DataManagerGacha.ProbabilityData.Type.TreeHouseFurniture,
			DataManagerGacha.ProbabilityData.Type.Item
		};
		Dictionary<DataManagerGacha.ProbabilityData.Type, List<DataManagerGacha.ProbabilityData.Element>> dictionary = new Dictionary<DataManagerGacha.ProbabilityData.Type, List<DataManagerGacha.ProbabilityData.Element>>();
		int num = 0;
		foreach (DataManagerGacha.ProbabilityData.Type type in list)
		{
			List<DataManagerGacha.ProbabilityData.Element> elements = lastRequestRateViewData.GetElements(type, DataManagerGacha.ProbabilityData.Category.PickUp);
			PrjUtil.InsertionSort<DataManagerGacha.ProbabilityData.Element>(ref elements, (DataManagerGacha.ProbabilityData.Element a, DataManagerGacha.ProbabilityData.Element b) => a.item_num.CompareTo(b.item_num));
			PrjUtil.InsertionSort<DataManagerGacha.ProbabilityData.Element>(ref elements, (DataManagerGacha.ProbabilityData.Element a, DataManagerGacha.ProbabilityData.Element b) => a.item_id.CompareTo(b.item_id));
			PrjUtil.InsertionSort<DataManagerGacha.ProbabilityData.Element>(ref elements, (DataManagerGacha.ProbabilityData.Element a, DataManagerGacha.ProbabilityData.Element b) => a.sortNum.CompareTo(b.sortNum));
			PrjUtil.InsertionSort<DataManagerGacha.ProbabilityData.Element>(ref elements, (DataManagerGacha.ProbabilityData.Element a, DataManagerGacha.ProbabilityData.Element b) => b.rate[0].rarity.CompareTo(a.rate[0].rarity));
			dictionary.Add(type, elements);
			num += elements.Count;
		}
		if (0 < num)
		{
			GachaWindowBoxInfoCtrl.TabInnerFrameGUI tabInnerFrameGUI = new GachaWindowBoxInfoCtrl.TabInnerFrameGUI(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts01"), this.gachaWindowBoxInfoGuiData.ScrollContent.transform).transform);
			tabInnerFrameGUI.Title.text = "ピックアップ";
			foreach (DataManagerGacha.ProbabilityData.Type type2 in list)
			{
				if (dictionary[type2].Count != 0)
				{
					int num2 = 0;
					using (List<DataManagerGacha.ProbabilityData.Element>.Enumerator enumerator3 = dictionary[type2].GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							DataManagerGacha.ProbabilityData.Element probabilityElement = enumerator3.Current;
							if (num2 == 0 || probabilityElement.rate[0].rarity < num2)
							{
								num2 = probabilityElement.rate[0].rarity;
								GachaWindowInfoCtrl.RarityStarBar rarityStarBar = new GachaWindowInfoCtrl.RarityStarBar(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts02"), tabInnerFrameGUI.Base.transform).transform);
								rarityStarBar.IconAllObj.SetActive(false);
								rarityStarBar.RarityText.gameObject.SetActive(false);
								DataManagerGacha.ProbabilityData.Type type3 = probabilityElement.type;
								if (type3 - DataManagerGacha.ProbabilityData.Type.Chara > 1)
								{
									if (type3 - DataManagerGacha.ProbabilityData.Type.Item > 1)
									{
										goto IL_0359;
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
											pguiImageCtrl.gameObject.SetActive(num3 < probabilityElement.rate[0].rarity);
											num3++;
										}
										goto IL_0359;
									}
								}
								rarityStarBar.RarityText.gameObject.SetActive(true);
								rarityStarBar.RarityText.text = PrjUtil.Rarity2String(probabilityElement.rate[0].rarity);
							}
							IL_0359:
							GachaWindowInfoCtrl.ItemInfoLabelGUI itemInfoLabelGUI = new GachaWindowInfoCtrl.ItemInfoLabelGUI(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts03"), tabInnerFrameGUI.Base.transform).transform);
							itemInfoLabelGUI.Icon_Item.Setup(DataManager.DmItem.GetItemStaticBase(probabilityElement.item_id), new IconItemCtrl.SetupParam
							{
								useMaxDetail = true
							});
							itemInfoLabelGUI.IconAllObj.SetActive(false);
							itemInfoLabelGUI.RarityText.gameObject.SetActive(false);
							int num4 = 0;
							string text = string.Empty;
							string text2 = string.Empty;
							switch (probabilityElement.type)
							{
							case DataManagerGacha.ProbabilityData.Type.Chara:
							{
								text = DataManager.DmChara.GetCharaStaticData(DataManager.DmItem.ItemId2ChraId(probabilityElement.item_id)).baseData.NickName;
								itemInfoLabelGUI.IconAllObj.SetActive(true);
								using (List<PguiImageCtrl>.Enumerator enumerator4 = itemInfoLabelGUI.IconAllList.GetEnumerator())
								{
									while (enumerator4.MoveNext())
									{
										PguiImageCtrl pguiImageCtrl2 = enumerator4.Current;
										pguiImageCtrl2.gameObject.SetActive(num4 < probabilityElement.rate[0].rarity);
										num4++;
									}
									break;
								}
								goto IL_04A6;
							}
							case DataManagerGacha.ProbabilityData.Type.Photo:
								goto IL_04A6;
							case DataManagerGacha.ProbabilityData.Type.Item:
								itemInfoLabelGUI.RarityText.gameObject.SetActive(true);
								itemInfoLabelGUI.RarityText.text = PrjUtil.Rarity2String(probabilityElement.rate[0].rarity);
								text2 = "×" + probabilityElement.item_num.ToString();
								break;
							case DataManagerGacha.ProbabilityData.Type.TreeHouseFurniture:
								goto IL_0511;
							}
							IL_05DA:
							itemInfoLabelGUI.Name.ReplaceTextByDefault(new string[] { "Param01", "Param02", "Param03" }, new string[]
							{
								text,
								itemInfoLabelGUI.Icon_Item.itemStaticBase.GetName(),
								text2
							});
							bool flag = 0 < probabilityElement.numerator;
							itemInfoLabelGUI.DisableImage.gameObject.SetActive(!flag);
							itemInfoLabelGUI.RateNormal.gameObject.SetActive(false);
							itemInfoLabelGUI.RateNormalText.SetActive(false);
							itemInfoLabelGUI.RateDecided3.gameObject.SetActive(false);
							itemInfoLabelGUI.RateDecided3Text.SetActive(false);
							itemInfoLabelGUI.RateDecidedFriends.gameObject.SetActive(false);
							itemInfoLabelGUI.RateDecidedFriendsText.SetActive(false);
							itemInfoLabelGUI.RateDecidedCeiling.gameObject.SetActive(false);
							itemInfoLabelGUI.RateDecidedCeilingText.SetActive(false);
							itemInfoLabelGUI.BoxStack.text = probabilityElement.numerator.ToString() + " / " + probabilityElement.denominator.ToString();
							DataManagerGacha.GachaItemdata gachaItemdata = this.currentGachaStaticData.gachaItemData.Find((DataManagerGacha.GachaItemdata x) => x.itemId == probabilityElement.item_id);
							if (gachaItemdata != null)
							{
								itemInfoLabelGUI.Mark_Limited.gameObject.SetActive(gachaItemdata.dispClientTypeLimit != 0);
								itemInfoLabelGUI.Mark_New.gameObject.SetActive(gachaItemdata.dispClientTypeLimit == 0 && gachaItemdata.dispClientTypeNew != 0);
								continue;
							}
							itemInfoLabelGUI.Mark_Limited.gameObject.SetActive(false);
							itemInfoLabelGUI.Mark_New.gameObject.SetActive(false);
							continue;
							IL_04A6:
							itemInfoLabelGUI.IconAllObj.SetActive(true);
							using (List<PguiImageCtrl>.Enumerator enumerator4 = itemInfoLabelGUI.IconAllList.GetEnumerator())
							{
								while (enumerator4.MoveNext())
								{
									PguiImageCtrl pguiImageCtrl3 = enumerator4.Current;
									pguiImageCtrl3.gameObject.SetActive(num4 < probabilityElement.rate[0].rarity);
									num4++;
								}
								goto IL_05DA;
							}
							IL_0511:
							itemInfoLabelGUI.Icon_Item.AddOnLongClickListener(delegate(IconItemCtrl x)
							{
								TreeHouseFurnitureStatic treeHouseFurnitureStaticData = DataManager.DmTreeHouse.GetTreeHouseFurnitureStaticData(probabilityElement.item_id);
								CanvasManager.HdlTreeHouseFurnitureWindowCtrl.Open(new TreeHouseFurnitureWindowCtrl.SetupParam
								{
									thfs = treeHouseFurnitureStaticData
								});
							});
							itemInfoLabelGUI.RarityText.gameObject.SetActive(true);
							itemInfoLabelGUI.RarityText.text = PrjUtil.Rarity2String(probabilityElement.rate[0].rarity);
							text2 = "×" + probabilityElement.item_num.ToString();
							goto IL_05DA;
						}
					}
				}
			}
		}
		Dictionary<DataManagerGacha.ProbabilityData.Type, List<DataManagerGacha.ProbabilityData.Element>> dictionary2 = new Dictionary<DataManagerGacha.ProbabilityData.Type, List<DataManagerGacha.ProbabilityData.Element>>();
		Dictionary<DataManagerGacha.ProbabilityData.Type, List<DataManagerGacha.ProbabilityData.Element>> dictionary3 = new Dictionary<DataManagerGacha.ProbabilityData.Type, List<DataManagerGacha.ProbabilityData.Element>>();
		int num5 = 0;
		foreach (DataManagerGacha.ProbabilityData.Type type4 in list)
		{
			List<DataManagerGacha.ProbabilityData.Element> elements2 = lastRequestRateViewData.GetElements(type4, DataManagerGacha.ProbabilityData.Category.Other);
			PrjUtil.InsertionSort<DataManagerGacha.ProbabilityData.Element>(ref elements2, (DataManagerGacha.ProbabilityData.Element a, DataManagerGacha.ProbabilityData.Element b) => b.item_num.CompareTo(a.item_num));
			PrjUtil.InsertionSort<DataManagerGacha.ProbabilityData.Element>(ref elements2, (DataManagerGacha.ProbabilityData.Element a, DataManagerGacha.ProbabilityData.Element b) => a.item_id.CompareTo(b.item_id));
			PrjUtil.InsertionSort<DataManagerGacha.ProbabilityData.Element>(ref elements2, (DataManagerGacha.ProbabilityData.Element a, DataManagerGacha.ProbabilityData.Element b) => a.sortNum.CompareTo(b.sortNum));
			PrjUtil.InsertionSort<DataManagerGacha.ProbabilityData.Element>(ref elements2, (DataManagerGacha.ProbabilityData.Element a, DataManagerGacha.ProbabilityData.Element b) => b.rate[0].rarity.CompareTo(a.rate[0].rarity));
			List<DataManagerGacha.ProbabilityData.Element> list2 = new List<DataManagerGacha.ProbabilityData.Element>();
			List<DataManagerGacha.ProbabilityData.Element> list3 = new List<DataManagerGacha.ProbabilityData.Element>();
			foreach (DataManagerGacha.ProbabilityData.Element element in elements2)
			{
				if (0 < element.numerator)
				{
					list2.Add(element);
				}
				else
				{
					list3.Add(element);
				}
			}
			dictionary2.Add(type4, list2);
			dictionary3.Add(type4, list3);
			num5 += elements2.Count;
		}
		if (0 < num5)
		{
			GachaWindowBoxInfoCtrl.TabInnerFrameGUI tabInnerFrameGUI2 = new GachaWindowBoxInfoCtrl.TabInnerFrameGUI(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts01"), this.gachaWindowBoxInfoGuiData.ScrollContent.transform).transform);
			tabInnerFrameGUI2.Title.text = "提供一覧";
			foreach (DataManagerGacha.ProbabilityData.Type type5 in list)
			{
				foreach (DataManagerGacha.ProbabilityData.Element element2 in dictionary2[type5])
				{
					new GachaWindowBoxInfoCtrl.BoxInfoLabel(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts_BoxItem"), tabInnerFrameGUI2.Base.transform).transform).Setup(element2);
				}
			}
			foreach (DataManagerGacha.ProbabilityData.Type type6 in list)
			{
				foreach (DataManagerGacha.ProbabilityData.Element element3 in dictionary3[type6])
				{
					new GachaWindowBoxInfoCtrl.BoxInfoLabel(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GachaInfo_Parts_BoxItem"), tabInnerFrameGUI2.Base.transform).transform).Setup(element3);
				}
			}
		}
	}

	// Token: 0x04000EE5 RID: 3813
	public GachaWindowBoxInfoCtrl.GachaWindowBoxInfoGUI gachaWindowBoxInfoGuiData;

	// Token: 0x04000EE6 RID: 3814
	private IEnumerator IEWindowMove;

	// Token: 0x04000EE7 RID: 3815
	private UnityAction windowCloseEndCb;

	// Token: 0x04000EE8 RID: 3816
	private DataManagerGacha.GachaStaticData currentGachaStaticData;

	// Token: 0x02000AA0 RID: 2720
	public class GachaWindowBoxInfoGUI
	{
		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06003FFD RID: 16381 RVA: 0x001F3E0F File Offset: 0x001F200F
		// (set) Token: 0x06003FFE RID: 16382 RVA: 0x001F3E17 File Offset: 0x001F2017
		public PguiButtonCtrl LeftArrow { get; set; }

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06003FFF RID: 16383 RVA: 0x001F3E20 File Offset: 0x001F2020
		// (set) Token: 0x06004000 RID: 16384 RVA: 0x001F3E28 File Offset: 0x001F2028
		public PguiButtonCtrl RightArrow { get; set; }

		// Token: 0x06004001 RID: 16385 RVA: 0x001F3E34 File Offset: 0x001F2034
		public GachaWindowBoxInfoGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.InBase = baseTr.Find("Base").gameObject;
			this.BtnClose = baseTr.Find("Base/Window/BtnClose").GetComponent<PguiButtonCtrl>();
			this.BtnClose.androidBackKeyTarget = true;
			this.LeftArrow = baseTr.Find("Base/Window/LeftBtn/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
			this.LeftArrow.gameObject.SetActive(false);
			this.RightArrow = baseTr.Find("Base/Window/RightBtn/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
			this.RightArrow.gameObject.SetActive(false);
			this.ScrollContent = baseTr.Find("Base/Window/Base/Info_Gachadrop/ScrollView/Viewport/Content").gameObject;
			this.BaseAnim = this.InBase.GetComponent<SimpleAnimation>();
			baseTr.Find("Base/Window/Base/Info_Gachadrop/ScrollView").GetComponent<ScrollRect>().scrollSensitivity = ScrollParamDefine.GachaBoxInfo;
		}

		// Token: 0x040043B2 RID: 17330
		public GameObject baseObj;

		// Token: 0x040043B3 RID: 17331
		public GameObject InBase;

		// Token: 0x040043B4 RID: 17332
		public PguiButtonCtrl BtnClose;

		// Token: 0x040043B7 RID: 17335
		public SimpleAnimation BaseAnim;

		// Token: 0x040043B8 RID: 17336
		public GameObject ScrollContent;
	}

	// Token: 0x02000AA1 RID: 2721
	public class TabInnerFrameGUI
	{
		// Token: 0x06004002 RID: 16386 RVA: 0x001F3F1C File Offset: 0x001F211C
		public TabInnerFrameGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Base = baseTr.Find("Base").GetComponent<PguiImageCtrl>();
			Transform transform = baseTr.Find("Title");
			this.Title = ((transform != null) ? transform.GetComponent<PguiTextCtrl>() : null);
			PguiTextCtrl pguiTextCtrl;
			if ((pguiTextCtrl = this.Title) == null)
			{
				Transform transform2 = baseTr.Find("Base/Title");
				pguiTextCtrl = ((transform2 != null) ? transform2.GetComponent<PguiTextCtrl>() : null);
			}
			this.Title = pguiTextCtrl;
			this.Rect = this.Base.GetComponent<RectTransform>();
		}

		// Token: 0x040043B9 RID: 17337
		public GameObject baseObj;

		// Token: 0x040043BA RID: 17338
		public PguiImageCtrl Base;

		// Token: 0x040043BB RID: 17339
		public PguiTextCtrl Title;

		// Token: 0x040043BC RID: 17340
		public RectTransform Rect;
	}

	// Token: 0x02000AA2 RID: 2722
	public class BoxInfoLabel
	{
		// Token: 0x06004003 RID: 16387 RVA: 0x001F3FA8 File Offset: 0x001F21A8
		public BoxInfoLabel(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.InBase = baseTr.Find("Base").gameObject;
			this.InBaseImage = this.InBase.GetComponent<Image>();
			this.StarIconAllObj = baseTr.Find("Base/Icon_All").gameObject;
			this.StarIconImageList = new List<PguiImageCtrl>
			{
				baseTr.Find("Base/Icon_All/Icon1").GetComponent<PguiImageCtrl>(),
				baseTr.Find("Base/Icon_All/Icon2").GetComponent<PguiImageCtrl>(),
				baseTr.Find("Base/Icon_All/Icon3").GetComponent<PguiImageCtrl>(),
				baseTr.Find("Base/Icon_All/Icon4").GetComponent<PguiImageCtrl>()
			};
			this.RarityText = baseTr.Find("Base/Txt_Rarity").GetComponent<PguiTextCtrl>();
			this.ItemNameText = baseTr.Find("Base/Txt_ItemName").GetComponent<PguiTextCtrl>();
			this.NumPercentText = baseTr.Find("Base/Num_Percent").GetComponent<PguiTextCtrl>();
			this.MarkKind = baseTr.Find("Base/Mark_Kind").GetComponent<PguiReplaceSpriteCtrl>();
			this.DisableImage = baseTr.Find("Disable").GetComponent<PguiImageCtrl>();
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x001F40D8 File Offset: 0x001F22D8
		public void Setup(DataManagerGacha.ProbabilityData.Element element)
		{
			this.DisableImage.gameObject.SetActive(false);
			ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(element.item_id);
			if (itemStaticBase == null)
			{
				return;
			}
			bool flag = 0 < element.numerator;
			this.NumPercentText.text = element.numerator.ToString() + " / " + element.denominator.ToString();
			this.DisableImage.gameObject.SetActive(!flag);
			this.StarIconAllObj.SetActive(false);
			this.RarityText.gameObject.SetActive(false);
			string text = string.Empty;
			string text2 = string.Empty;
			ItemDef.Kind kind = itemStaticBase.GetKind();
			if (kind != ItemDef.Kind.CHARA)
			{
				if (kind != ItemDef.Kind.PHOTO)
				{
					this.ItemNameText.text = itemStaticBase.GetName();
					this.RarityText.gameObject.SetActive(true);
					this.RarityText.text = PrjUtil.Rarity2String((int)itemStaticBase.GetRarity());
					this.MarkKind.Replace(1);
					text2 = string.Format("×{0}", element.item_num);
				}
				else
				{
					this.StarIconAllObj.SetActive(true);
					int num = 0;
					foreach (PguiImageCtrl pguiImageCtrl in this.StarIconImageList)
					{
						pguiImageCtrl.gameObject.SetActive(num < (int)itemStaticBase.GetRarity());
						num++;
					}
					this.MarkKind.Replace(3);
				}
			}
			else
			{
				CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(itemStaticBase.GetId());
				text = charaStaticData.baseData.NickName;
				this.StarIconAllObj.SetActive(true);
				int num2 = 0;
				foreach (PguiImageCtrl pguiImageCtrl2 in this.StarIconImageList)
				{
					pguiImageCtrl2.gameObject.SetActive(num2 < charaStaticData.baseData.rankLow);
					num2++;
				}
				this.MarkKind.Replace(2);
			}
			this.ItemNameText.ReplaceTextByDefault(new string[] { "Param01", "Param02", "Param03" }, new string[]
			{
				text,
				itemStaticBase.GetName(),
				text2
			});
		}

		// Token: 0x040043BD RID: 17341
		public GameObject baseObj;

		// Token: 0x040043BE RID: 17342
		public GameObject InBase;

		// Token: 0x040043BF RID: 17343
		public Image InBaseImage;

		// Token: 0x040043C0 RID: 17344
		public GameObject StarIconAllObj;

		// Token: 0x040043C1 RID: 17345
		public List<PguiImageCtrl> StarIconImageList;

		// Token: 0x040043C2 RID: 17346
		public PguiTextCtrl RarityText;

		// Token: 0x040043C3 RID: 17347
		public PguiTextCtrl ItemNameText;

		// Token: 0x040043C4 RID: 17348
		public PguiTextCtrl NumPercentText;

		// Token: 0x040043C5 RID: 17349
		public PguiReplaceSpriteCtrl MarkKind;

		// Token: 0x040043C6 RID: 17350
		public PguiImageCtrl DisableImage;
	}
}
