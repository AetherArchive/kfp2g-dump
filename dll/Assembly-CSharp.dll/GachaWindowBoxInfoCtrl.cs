using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GachaWindowBoxInfoCtrl : MonoBehaviour
{
	private void Update()
	{
		if (this.IEWindowMove != null && !this.IEWindowMove.MoveNext())
		{
			this.IEWindowMove = null;
		}
	}

	public void Initialize()
	{
		this.gachaWindowBoxInfoGuiData = new GachaWindowBoxInfoCtrl.GachaWindowBoxInfoGUI(base.transform);
		this.gachaWindowBoxInfoGuiData.BaseAnim.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.gachaWindowBoxInfoGuiData.InBase.SetActive(false);
		this.gachaWindowBoxInfoGuiData.BtnClose.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnCliskCloseButton), PguiButtonCtrl.SoundType.DEFAULT);
	}

	private void OnCliskCloseButton(PguiButtonCtrl button)
	{
		this.Close(null);
	}

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

	private void Close(UnityAction closeEndCb = null)
	{
		if (this.IEWindowMove == null)
		{
			this.IEWindowMove = this.CloseWindow(closeEndCb);
		}
	}

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

	public GachaWindowBoxInfoCtrl.GachaWindowBoxInfoGUI gachaWindowBoxInfoGuiData;

	private IEnumerator IEWindowMove;

	private UnityAction windowCloseEndCb;

	private DataManagerGacha.GachaStaticData currentGachaStaticData;

	public class GachaWindowBoxInfoGUI
	{
		public PguiButtonCtrl LeftArrow { get; set; }

		public PguiButtonCtrl RightArrow { get; set; }

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

		public GameObject baseObj;

		public GameObject InBase;

		public PguiButtonCtrl BtnClose;

		public SimpleAnimation BaseAnim;

		public GameObject ScrollContent;
	}

	public class TabInnerFrameGUI
	{
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

		public GameObject baseObj;

		public PguiImageCtrl Base;

		public PguiTextCtrl Title;

		public RectTransform Rect;
	}

	public class BoxInfoLabel
	{
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

		public GameObject baseObj;

		public GameObject InBase;

		public Image InBaseImage;

		public GameObject StarIconAllObj;

		public List<PguiImageCtrl> StarIconImageList;

		public PguiTextCtrl RarityText;

		public PguiTextCtrl ItemNameText;

		public PguiTextCtrl NumPercentText;

		public PguiReplaceSpriteCtrl MarkKind;

		public PguiImageCtrl DisableImage;
	}
}
