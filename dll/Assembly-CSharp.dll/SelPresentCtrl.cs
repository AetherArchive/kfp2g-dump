using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SGNFW.HttpRequest.Protocol;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000169 RID: 361
public class SelPresentCtrl : MonoBehaviour
{
	// Token: 0x1700038D RID: 909
	// (get) Token: 0x06001557 RID: 5463 RVA: 0x0010C4E3 File Offset: 0x0010A6E3
	// (set) Token: 0x06001558 RID: 5464 RVA: 0x0010C4EB File Offset: 0x0010A6EB
	private SelPresentCtrl.Mode CurrentMode { get; set; }

	// Token: 0x06001559 RID: 5465 RVA: 0x0010C4F4 File Offset: 0x0010A6F4
	public void Init()
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)AssetManager.GetAssetData("ScenePresent/GUI/Prefab/GUI_Present"), base.transform);
		this.guiData = new SelPresentCtrl.GUI(gameObject.transform);
		ReuseScroll scrollViewPresent = this.guiData.ScrollViewPresent;
		scrollViewPresent.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollViewPresent.onStartItem, new Action<int, GameObject>(this.OnStartPresent));
		ReuseScroll scrollViewPresent2 = this.guiData.ScrollViewPresent;
		scrollViewPresent2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollViewPresent2.onUpdateItem, new Action<int, GameObject>(this.OnUpdatePresent));
		this.guiData.ScrollViewPresent.Setup(10, 0);
		ReuseScroll scrollViewHistory = this.guiData.ScrollViewHistory;
		scrollViewHistory.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollViewHistory.onStartItem, new Action<int, GameObject>(this.OnStartHistory));
		ReuseScroll scrollViewHistory2 = this.guiData.ScrollViewHistory;
		scrollViewHistory2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollViewHistory2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateHistory));
		this.guiData.ScrollViewHistory.Setup(10, 0);
		this.guiData.Btn_Get.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickAllGetButton), PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x0600155A RID: 5466 RVA: 0x0010C620 File Offset: 0x0010A820
	public void Setup()
	{
		this.guiData.PresentList.SetActive(false);
		this.guiData.HistoryList.SetActive(false);
		this.guiData.TabGroup.Setup(0, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectTab));
		this.CurrentMode = SelPresentCtrl.Mode.INVALID;
		this.UpdateScrollInfo();
		this.ChangeSelectTab(SelPresentCtrl.Mode.PRESENT, false);
		this.guiData.ScrollViewPresent.ForceFocus(0);
		this.guiData.ScrollViewHistory.ForceFocus(0);
		CanvasManager.HdlSelPurchaseStoneWindowCtrl.AddOnSuccessPurchaseListener(new UnityAction(this.RequestPresentSceneUpdate));
	}

	// Token: 0x0600155B RID: 5467 RVA: 0x0010C6B9 File Offset: 0x0010A8B9
	public void Disable()
	{
		CanvasManager.HdlSelPurchaseStoneWindowCtrl.RemoveOnSuccessPurchaseListener(new UnityAction(this.RequestPresentSceneUpdate));
	}

	// Token: 0x0600155C RID: 5468 RVA: 0x0010C6D1 File Offset: 0x0010A8D1
	public void Destroy()
	{
		if (this.playGachaAuth != null)
		{
			this.playGachaAuth.DestroyAllObject();
		}
	}

	// Token: 0x0600155D RID: 5469 RVA: 0x0010C6E8 File Offset: 0x0010A8E8
	private bool IsAllGetButton()
	{
		bool flag = false;
		foreach (DataManagerPresent.UserPresentData userPresentData in this.userPresentDataList)
		{
			ItemData itemData = userPresentData.GetItemData();
			if (itemData.staticData != null)
			{
				ItemDef.Kind kind = itemData.staticData.GetKind();
				if (kind != ItemDef.Kind.CHARA)
				{
					if (kind == ItemDef.Kind.PHOTO)
					{
						if (SelPresentCtrl.PhotoGetLimit() < itemData.num)
						{
							continue;
						}
					}
					else if (SelPresentCtrl.ItemGetLimit(itemData) < (long)itemData.num)
					{
						continue;
					}
					flag = true;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x0600155E RID: 5470 RVA: 0x0010C77C File Offset: 0x0010A97C
	private long IndexToID(int index)
	{
		if (DataManager.IsServerRequesting())
		{
			return -1L;
		}
		if (index < 0 || index >= this.userPresentDataList.Count)
		{
			return -1L;
		}
		return this.userPresentDataList[index].id;
	}

	// Token: 0x0600155F RID: 5471 RVA: 0x0010C7AE File Offset: 0x0010A9AE
	private void Start()
	{
	}

	// Token: 0x06001560 RID: 5472 RVA: 0x0010C7B0 File Offset: 0x0010A9B0
	private void Update()
	{
		if (this.RequestPresentGet != null)
		{
			this.RequestPresentGet.MoveNext();
		}
		if (this.RequestSelUpdate != null)
		{
			this.RequestSelUpdate.MoveNext();
		}
	}

	// Token: 0x06001561 RID: 5473 RVA: 0x0010C7DA File Offset: 0x0010A9DA
	public IEnumerator PlayPresentAuth(DataManagerPresent.UserPresentData presentData, List<int> haveCharaList, List<GachaResult> replaceItemList)
	{
		if (ItemDef.Id2Kind(presentData.itemId) == ItemDef.Kind.CHARA)
		{
			this.playGachaAuth = new GachaAuthCtrl();
			this.playGachaAuth.Initialize();
			List<ItemData> list = new List<ItemData>();
			foreach (GachaResult gachaResult in replaceItemList)
			{
				if (1 == gachaResult.rep_flg)
				{
					foreach (RepItem repItem in gachaResult.rep_item_list)
					{
						list.Add(new ItemData(repItem.rep_item_id, repItem.rep_item_num));
					}
				}
			}
			IEnumerator func = this.playGachaAuth.PlayCharaPresentAuth(presentData, haveCharaList, list);
			while (func.MoveNext())
			{
				yield return null;
			}
			this.playGachaAuth.DestroyAllObject();
			this.playGachaAuth = null;
			func = null;
		}
		yield break;
	}

	// Token: 0x06001562 RID: 5474 RVA: 0x0010C7FE File Offset: 0x0010A9FE
	private IEnumerator PresentDataUpdate(bool isAllGetRequest, DataManagerPresent.UserPresentData presentData = null)
	{
		List<int> haveCharaList = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values).ConvertAll<int>((CharaPackData item) => item.staticData.GetId());
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.guiData.baseObj.SetActive(false);
		CanvasManager.HdlCmnMenu.SetupMenu(false, "プレゼント", null, "", null, null);
		if (presentData != null)
		{
			IEnumerator func = this.PlayPresentAuth(presentData, haveCharaList, DataManager.DmPresent.LastReceiveReplacePresentList);
			while (func.MoveNext())
			{
				yield return null;
			}
			func = null;
		}
		CanvasManager.HdlMissionProgressCtrl.IsPresentReceiveAuth = false;
		this.guiData.baseObj.SetActive(true);
		CanvasManager.HdlCmnMenu.SetupMenu(true, "プレゼント", null, "", null, null);
		string text = SelPresentCtrl.MakeResultMassage(isAllGetRequest);
		List<ItemData> list = DataManager.DmPresent.GetUserReceivePresentList().ConvertAll<ItemData>((DataManagerPresent.UserPresentData x) => new ItemData(x.itemId, x.itemNum));
		List<ItemData> list2 = new List<ItemData>();
		list2.AddRange(SelPresentCtrl.<PresentDataUpdate>g__ItemDataList|23_2(DataManager.DmItem.GetUserDispItemList(list, DataManagerItem.DispType.Common)));
		list2.AddRange(SelPresentCtrl.<PresentDataUpdate>g__ItemDataList|23_2(DataManager.DmItem.GetUserDispItemList(list, DataManagerItem.DispType.Growth)));
		list2.AddRange(SelPresentCtrl.<PresentDataUpdate>g__ItemDataList|23_2(DataManager.DmItem.GetUserDispItemList(list, DataManagerItem.DispType.Photo)));
		list2.AddRange(SelPresentCtrl.<PresentDataUpdate>g__ItemDataList|23_2(DataManager.DmItem.GetUserDispItemList(list, DataManagerItem.DispType.Accessory)));
		list2.AddRange(SelPresentCtrl.<PresentDataUpdate>g__ItemDataList|23_2(DataManager.DmItem.GetUserDispItemList(list, DataManagerItem.DispType.Decoration)));
		GetMultiItemWindowCtrl.SetupParam setupParam = new GetMultiItemWindowCtrl.SetupParam
		{
			titleText = "確認",
			messageText = text,
			innerTitleText = "入手したアイテム"
		};
		setupParam.callBack = delegate(int x)
		{
			this.RequestPresentGet = null;
			return true;
		};
		if (list2.Count != 0)
		{
			CanvasManager.HdlGetItemSetWindowCtrl.Setup(list2, setupParam, false, 0);
			CanvasManager.HdlGetItemSetWindowCtrl.Open();
		}
		else
		{
			CanvasManager.HdlOpenWindowBasic.Setup("確認", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, delegate
			{
				this.RequestPresentGet = null;
			}, false);
			CanvasManager.HdlOpenWindowBasic.Open();
		}
		this.UpdateScrollInfo();
		yield break;
	}

	// Token: 0x06001563 RID: 5475 RVA: 0x0010C81B File Offset: 0x0010AA1B
	public void RequestPresentSceneUpdate()
	{
		this.RequestSelUpdate = this.PresentSceneUpdate();
	}

	// Token: 0x06001564 RID: 5476 RVA: 0x0010C829 File Offset: 0x0010AA29
	private IEnumerator PresentSceneUpdate()
	{
		DataManager.DmPresent.RequestGetPresentList();
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		DataManager.DmPresent.RequestGetHistoryist();
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.UpdateScrollInfo();
		yield break;
	}

	// Token: 0x06001565 RID: 5477 RVA: 0x0010C838 File Offset: 0x0010AA38
	private void OnClickGetButton(PguiButtonCtrl button)
	{
		if (this.RequestPresentGet != null)
		{
			return;
		}
		if (CanvasManager.HdlCmnMenu.IsSceneManagerMoving)
		{
			return;
		}
		SelPresentCtrl.GuiPresentBar guiPresentBar = this.guiData.presentBarList.Find((SelPresentCtrl.GuiPresentBar item) => item.Btn_Get == button);
		if (guiPresentBar != null && guiPresentBar.itemIndex < this.userPresentDataList.Count)
		{
			long num = this.IndexToID(guiPresentBar.itemIndex);
			if (num != -1L)
			{
				CanvasManager.HdlMissionProgressCtrl.IsPresentReceiveAuth = true;
				DataManager.DmPresent.RequestActionPresentGetOne(num, this.userPresentDataList[guiPresentBar.itemIndex]);
				this.RequestPresentGet = this.PresentDataUpdate(false, this.userPresentDataList[guiPresentBar.itemIndex]);
			}
		}
	}

	// Token: 0x06001566 RID: 5478 RVA: 0x0010C8F2 File Offset: 0x0010AAF2
	private void OnClickAllGetButton(PguiButtonCtrl button)
	{
		if (this.RequestPresentGet != null)
		{
			return;
		}
		if (CanvasManager.HdlCmnMenu.IsSceneManagerMoving)
		{
			return;
		}
		CanvasManager.HdlMissionProgressCtrl.IsPresentReceiveAuth = true;
		DataManager.DmPresent.RequestActionPresentGetAll();
		this.RequestPresentGet = this.PresentDataUpdate(true, null);
	}

	// Token: 0x06001567 RID: 5479 RVA: 0x0010C930 File Offset: 0x0010AB30
	private static string MakeResultMassage(bool isAllGetRequest)
	{
		List<DataManagerPresent.UserPresentData> userReceivePresentList = DataManager.DmPresent.GetUserReceivePresentList();
		List<DataManagerPresent.UserPresentData> userPresentList = DataManager.DmPresent.GetUserPresentList();
		string text = "";
		if (0 < userReceivePresentList.Count)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (DataManagerPresent.UserPresentData userPresentData in userReceivePresentList)
			{
				if (userPresentData.GetItemData().staticData.GetKind() == ItemDef.Kind.STONE)
				{
					num3 += userPresentData.itemNum;
				}
				else if (userPresentData.itemId == 30101)
				{
					num2 += userPresentData.itemNum;
				}
				else
				{
					num++;
				}
			}
			if (isAllGetRequest)
			{
				text += "プレゼントを受け取りました";
				if (DataManager.DmPresent.IsOverLimit)
				{
					text += "\n所持数上限を超えるアイテムは受け取ることができませんでした";
				}
				if (DataManager.DmPresent.IsGoldStock)
				{
					text = string.Concat(new string[]
					{
						text,
						"\n所持数上限を超える",
						DataManager.DmItem.GetItemStaticBase(30101).GetName(),
						"は",
						DataManager.DmItem.GetItemStaticBase(30090).GetName(),
						"に補充されました"
					});
				}
				bool flag = userPresentList.Find((DataManagerPresent.UserPresentData item) => ItemDef.Id2Kind(item.itemId) == ItemDef.Kind.CHARA) != null;
				bool flag2 = userPresentList.Find((DataManagerPresent.UserPresentData item) => ItemDef.Id2Kind(item.itemId) == ItemDef.Kind.CLOTHES) != null;
				bool flag3 = userPresentList.Find((DataManagerPresent.UserPresentData item) => ItemDef.Id2Kind(item.itemId) == ItemDef.Kind.ACHIEVEMENT) != null;
				if (flag || flag2 || flag3)
				{
					string text2 = "\n※";
					if (flag)
					{
						text2 += ((flag2 || flag3) ? "フレンズ、" : "フレンズは");
					}
					if (flag2)
					{
						text2 += (flag3 ? "着替え、" : "着替えは");
					}
					if (flag3)
					{
						text2 += "称号は";
					}
					text2 += "一括受け取りの対象外です";
					text += text2;
				}
			}
			else
			{
				List<GachaResult> lastReceiveReplacePresentList = DataManager.DmPresent.LastReceiveReplacePresentList;
				if (lastReceiveReplacePresentList.Count == 0)
				{
					text += "プレゼントを受け取りました";
				}
				else
				{
					Dictionary<int, ItemStaticBase> itemStaticMap = DataManager.DmItem.GetItemStaticMap();
					userReceivePresentList.Find((DataManagerPresent.UserPresentData item) => ItemDef.Id2Kind(item.itemId) == ItemDef.Kind.CHARA);
					foreach (GachaResult gachaResult in lastReceiveReplacePresentList)
					{
						if (itemStaticMap.ContainsKey(gachaResult.item_id))
						{
							text += itemStaticMap[gachaResult.item_id].GetName();
							text += " はすでに所持していたため";
							foreach (RepItem repItem in gachaResult.rep_item_list)
							{
								text = string.Concat(new string[]
								{
									text,
									"\n",
									itemStaticMap[repItem.rep_item_id].GetName(),
									"×",
									repItem.rep_item_num.ToString()
								});
							}
							text += " に変換されました";
							if (itemStaticMap[gachaResult.item_id].GetKind() == ItemDef.Kind.CHARA)
							{
								text += "\n\n<color=red>※受け取ったフレンズの強化状態が\n所持しているフレンズより高いステータスは\n強化状態を上書きしました</color>";
							}
						}
					}
				}
				if (DataManager.DmPresent.IsGoldStock)
				{
					text = string.Concat(new string[]
					{
						text,
						"\n所持数上限を超える",
						DataManager.DmItem.GetItemStaticBase(30101).GetName(),
						"は",
						DataManager.DmItem.GetItemStaticBase(30090).GetName(),
						"に補充されました"
					});
				}
			}
		}
		else
		{
			text += "所持数上限を超えるアイテムを\n受け取ることができませんでした";
		}
		if (text.Count<char>((char c) => c == '\n') >= 3)
		{
			text = "<size=22>" + text + "</size>";
		}
		return text;
	}

	// Token: 0x06001568 RID: 5480 RVA: 0x0010CDA0 File Offset: 0x0010AFA0
	private bool OnSelectTab(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				this.ChangeSelectTab(SelPresentCtrl.Mode.HISTORY, false);
			}
		}
		else
		{
			this.ChangeSelectTab(SelPresentCtrl.Mode.PRESENT, false);
		}
		return true;
	}

	// Token: 0x06001569 RID: 5481 RVA: 0x0010CDC0 File Offset: 0x0010AFC0
	private void UpdateScrollInfo()
	{
		this.userPresentDataList = new List<DataManagerPresent.UserPresentData>(DataManager.DmPresent.GetUserPresentList());
		this.userReceiveHistoryData = new List<DataManagerPresent.UserReceiveHistoryData>(DataManager.DmPresent.GetUserReceiveHistoryList());
		this.guiData.ScrollViewPresent.ResizeFocesNoMove(this.userPresentDataList.Count);
		this.guiData.ScrollViewHistory.ResizeFocesNoMove(this.userReceiveHistoryData.Count);
		this.ChangeSelectTab(this.CurrentMode, true);
	}

	// Token: 0x0600156A RID: 5482 RVA: 0x0010CE3C File Offset: 0x0010B03C
	private void ChangeSelectTab(SelPresentCtrl.Mode requestMode, bool isForce)
	{
		if (!isForce && this.CurrentMode == requestMode)
		{
			return;
		}
		this.CurrentMode = requestMode;
		SelPresentCtrl.Mode currentMode = this.CurrentMode;
		if (currentMode == SelPresentCtrl.Mode.PRESENT)
		{
			this.guiData.Txt_Caution.text = "受け取り可能なプレゼントはありません";
			this.guiData.Txt_Caution.gameObject.SetActive(this.userPresentDataList.Count == 0);
			this.guiData.Num_Present.text = this.userPresentDataList.Count.ToString() + "/" + DataManager.DmPresent.MaxPresentDataNum.ToString();
			this.guiData.Txt_Info.gameObject.SetActive(false);
			bool flag = this.IsAllGetButton();
			this.guiData.Btn_Get.SetActEnable(flag, false, false);
			this.guiData.PresentList.SetActive(true);
			this.guiData.HistoryList.SetActive(false);
			this.guiData.Btn_Get.gameObject.SetActive(true);
			return;
		}
		if (currentMode != SelPresentCtrl.Mode.HISTORY)
		{
			return;
		}
		this.guiData.Txt_Caution.text = "受け取ったプレゼントはありません";
		this.guiData.Num_Present.text = "";
		this.guiData.Txt_Caution.gameObject.SetActive(this.userReceiveHistoryData.Count == 0);
		this.guiData.Txt_Info.gameObject.SetActive(true);
		this.guiData.PresentList.SetActive(false);
		this.guiData.HistoryList.SetActive(true);
		this.guiData.Btn_Get.gameObject.SetActive(false);
	}

	// Token: 0x0600156B RID: 5483 RVA: 0x0010CFEC File Offset: 0x0010B1EC
	private void OnStartPresent(int index, GameObject go)
	{
		IconItemCtrl component = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, go.transform.Find("BaseImage/Icon_Item")).GetComponent<IconItemCtrl>();
		PguiButtonCtrl component2 = go.transform.Find("BaseImage/Btn_Get").GetComponent<PguiButtonCtrl>();
		if (component2)
		{
			component2.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGetButton), PguiButtonCtrl.SoundType.DEFAULT);
		}
		if (this.userPresentDataList != null && index < this.userPresentDataList.Count)
		{
			DataManagerPresent.UserPresentData userPresentData = this.userPresentDataList[index];
			if (component)
			{
				component.Setup(userPresentData.GetItemData());
				this.SetPresentCheck(component2, userPresentData);
			}
		}
		this.guiData.presentBarList.Add(new SelPresentCtrl.GuiPresentBar(go.transform, index));
	}

	// Token: 0x0600156C RID: 5484 RVA: 0x0010D0AC File Offset: 0x0010B2AC
	private void OnUpdatePresent(int index, GameObject go)
	{
		SelPresentCtrl.GuiPresentBar guiPresentBar = new SelPresentCtrl.GuiPresentBar(go.transform, 0);
		PguiButtonCtrl component = go.transform.Find("BaseImage/Btn_Get").GetComponent<PguiButtonCtrl>();
		if (this.userPresentDataList != null && index < this.userPresentDataList.Count)
		{
			DataManagerPresent.UserPresentData userPresentData = this.userPresentDataList[index];
			int num = index % this.guiData.presentBarList.Count;
			this.guiData.presentBarList[num].itemIndex = index;
			PguiTextCtrl itemNameText = guiPresentBar.ItemNameText;
			ItemStaticBase staticData = userPresentData.GetItemData().staticData;
			itemNameText.text = ((staticData != null) ? staticData.GetName() : null);
			guiPresentBar.InfoText.text = userPresentData.labelText;
			ItemStaticBase staticData2 = userPresentData.GetItemData().staticData;
			DateTime? dateTime = ((staticData2 != null) ? staticData2.endTime : null);
			if (dateTime == null)
			{
				guiPresentBar.DateText.gameObject.SetActive(false);
			}
			else
			{
				guiPresentBar.DateText.gameObject.SetActive(true);
				ItemStaticBase staticData3 = userPresentData.GetItemData().staticData;
				DateTime value = ((staticData3 != null) ? staticData3.endTime : null).Value;
				guiPresentBar.DateText.text = "※このアイテムは " + TimeManager.FormattedTime(value, TimeManager.Format.yyyyMMdd_hhmm) + " に消失します。";
				guiPresentBar.DateText.GetComponent<Text>().color = guiPresentBar.DateColor.GetGameObjectById("CAUTION");
			}
			if (null != guiPresentBar.IconItem)
			{
				guiPresentBar.IconItem.Setup(userPresentData.GetItemData());
				this.SetPresentCheck(component, userPresentData);
			}
		}
	}

	// Token: 0x0600156D RID: 5485 RVA: 0x0010D248 File Offset: 0x0010B448
	private void OnStartHistory(int index, GameObject go)
	{
		IconItemCtrl component = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, go.transform.Find("BaseImage/Icon_Item")).GetComponent<IconItemCtrl>();
		PguiButtonCtrl component2 = go.transform.Find("BaseImage/Btn_Get").GetComponent<PguiButtonCtrl>();
		if (component2)
		{
			component2.gameObject.SetActive(false);
		}
		if (this.userReceiveHistoryData != null && index < this.userReceiveHistoryData.Count)
		{
			DataManagerPresent.UserReceiveHistoryData userReceiveHistoryData = this.userReceiveHistoryData[index];
			if (component)
			{
				component.Setup(userReceiveHistoryData.GetItemData());
			}
		}
	}

	// Token: 0x0600156E RID: 5486 RVA: 0x0010D2DC File Offset: 0x0010B4DC
	private void OnUpdateHistory(int index, GameObject go)
	{
		SelPresentCtrl.GuiPresentBar guiPresentBar = new SelPresentCtrl.GuiPresentBar(go.transform, 0);
		if (this.userReceiveHistoryData != null && index < this.userReceiveHistoryData.Count)
		{
			DataManagerPresent.UserReceiveHistoryData userReceiveHistoryData = this.userReceiveHistoryData[index];
			guiPresentBar.ItemNameText.text = userReceiveHistoryData.GetItemData().staticData.GetName();
			guiPresentBar.InfoText.text = userReceiveHistoryData.labelText;
			guiPresentBar.DateText.text = "獲得日時:" + TimeManager.FormattedTime(userReceiveHistoryData.receiveTime, TimeManager.Format.yyyyMMdd_hhmm);
			guiPresentBar.DateText.GetComponent<Text>().color = guiPresentBar.DateColor.GetGameObjectById("NORMAL");
			if (null != guiPresentBar.IconItem)
			{
				guiPresentBar.IconItem.Setup(userReceiveHistoryData.GetItemData());
			}
		}
	}

	// Token: 0x0600156F RID: 5487 RVA: 0x0010D3AC File Offset: 0x0010B5AC
	private void SetPresentCheck(PguiButtonCtrl button, DataManagerPresent.UserPresentData presentData)
	{
		if (button == null || presentData == null || presentData.GetItemData().staticData == null)
		{
			return;
		}
		ItemData itemData = presentData.GetItemData();
		PguiTextCtrl component = button.transform.Find("Txt_Caution").GetComponent<PguiTextCtrl>();
		component.text = component.m_OriginalText;
		ItemDef.Kind kind = itemData.staticData.GetKind();
		bool flag;
		if (kind != ItemDef.Kind.CHARA)
		{
			if (kind != ItemDef.Kind.PHOTO)
			{
				if (kind != ItemDef.Kind.ACCESSORY_ITEM)
				{
					flag = (long)itemData.num <= SelPresentCtrl.ItemGetLimit(itemData);
					if (!flag && itemData.staticData.GetId() == DataManagerPhoto.PHOTO_STOCK_RELEASEITEM_ID)
					{
						component.text = "※所持枠解放上限※";
					}
				}
				else
				{
					flag = itemData.num <= SelPresentCtrl.AccGetLimit();
				}
			}
			else
			{
				flag = itemData.num <= SelPresentCtrl.PhotoGetLimit();
			}
		}
		else
		{
			flag = true;
		}
		button.SetActEnable(flag, false, false);
	}

	// Token: 0x06001570 RID: 5488 RVA: 0x0010D47F File Offset: 0x0010B67F
	private static int PhotoGetLimit()
	{
		return DataManager.DmPhoto.PhotoStockLimit - DataManager.DmPhoto.GetUserPhotoMap().Count;
	}

	// Token: 0x06001571 RID: 5489 RVA: 0x0010D49B File Offset: 0x0010B69B
	private static int AccGetLimit()
	{
		return DataManager.DmChAccessory.AccessoryStockLimit - DataManager.DmChAccessory.GetUserAccessoryList().Count;
	}

	// Token: 0x06001572 RID: 5490 RVA: 0x0010D4B8 File Offset: 0x0010B6B8
	private static long ItemGetLimit(ItemData itemData)
	{
		if (itemData.id == 30101)
		{
			return DataManagerItem.GetUserHaveMaxNum(itemData.id, 0) - (long)DataManager.DmItem.GetUserItemData(itemData.id).num;
		}
		return (long)(itemData.staticData.GetStackMax() - DataManager.DmItem.GetUserItemData(itemData.id).num);
	}

	// Token: 0x06001574 RID: 5492 RVA: 0x0010D520 File Offset: 0x0010B720
	[CompilerGenerated]
	internal static List<ItemData> <PresentDataUpdate>g__ItemDataList|23_2(List<ItemData> inputList)
	{
		if (inputList.Count == 0)
		{
			return new List<ItemData>();
		}
		List<ItemData> list = new List<ItemData>();
		ItemInput itemInput = new ItemInput(inputList[0].id, 0);
		foreach (ItemData itemData in inputList)
		{
			if (itemInput.itemId == itemData.id)
			{
				itemInput.num += itemData.num;
			}
			else
			{
				list.Add(new ItemData(itemInput.itemId, itemInput.num));
				itemInput = new ItemInput(itemData.id, itemData.num);
			}
		}
		list.Add(new ItemData(itemInput.itemId, itemInput.num));
		return list;
	}

	// Token: 0x040011B8 RID: 4536
	public bool isDebug;

	// Token: 0x040011B9 RID: 4537
	private SelPresentCtrl.GUI guiData;

	// Token: 0x040011BB RID: 4539
	private List<DataManagerPresent.UserPresentData> userPresentDataList;

	// Token: 0x040011BC RID: 4540
	private List<DataManagerPresent.UserReceiveHistoryData> userReceiveHistoryData;

	// Token: 0x040011BD RID: 4541
	private GachaAuthCtrl playGachaAuth;

	// Token: 0x040011BE RID: 4542
	private IEnumerator RequestPresentGet;

	// Token: 0x040011BF RID: 4543
	private IEnumerator RequestSelUpdate;

	// Token: 0x02000BFA RID: 3066
	public enum Mode
	{
		// Token: 0x04004928 RID: 18728
		INVALID,
		// Token: 0x04004929 RID: 18729
		PRESENT,
		// Token: 0x0400492A RID: 18730
		HISTORY
	}

	// Token: 0x02000BFB RID: 3067
	public class GuiPresentBar
	{
		// Token: 0x0600446B RID: 17515 RVA: 0x0020820C File Offset: 0x0020640C
		public GuiPresentBar(Transform baseTr, int index)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Get = baseTr.Find("BaseImage/Btn_Get").GetComponent<PguiButtonCtrl>();
			this.ItemNameText = baseTr.Find("BaseImage/Txt_ItemName").GetComponent<PguiTextCtrl>();
			this.InfoText = baseTr.Find("BaseImage/Txt_Info").GetComponent<PguiTextCtrl>();
			this.DateText = baseTr.Find("BaseImage/Txt_Date").GetComponent<PguiTextCtrl>();
			this.DateColor = baseTr.Find("BaseImage/Txt_Date").GetComponent<PguiColorCtrl>();
			this.IconItem = baseTr.Find("BaseImage/Icon_Item/Icon_Item(Clone)").GetComponent<IconItemCtrl>();
			this.itemIndex = index;
		}

		// Token: 0x0400492B RID: 18731
		public GameObject baseObj;

		// Token: 0x0400492C RID: 18732
		public PguiButtonCtrl Btn_Get;

		// Token: 0x0400492D RID: 18733
		public PguiTextCtrl ItemNameText;

		// Token: 0x0400492E RID: 18734
		public PguiTextCtrl InfoText;

		// Token: 0x0400492F RID: 18735
		public PguiTextCtrl DateText;

		// Token: 0x04004930 RID: 18736
		public PguiColorCtrl DateColor;

		// Token: 0x04004931 RID: 18737
		public IconItemCtrl IconItem;

		// Token: 0x04004932 RID: 18738
		public int itemIndex;
	}

	// Token: 0x02000BFC RID: 3068
	public class GUI
	{
		// Token: 0x0600446C RID: 17516 RVA: 0x002082B8 File Offset: 0x002064B8
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Get = baseTr.Find("Btn_Get").GetComponent<PguiButtonCtrl>();
			this.Txt_Caution = baseTr.Find("Base/Txt_Caution").GetComponent<PguiTextCtrl>();
			this.Num_Present = baseTr.Find("Base/Num_Present").GetComponent<PguiTextCtrl>();
			this.Txt_Info = baseTr.Find("Base/Txt_Info").GetComponent<PguiTextCtrl>();
			this.ScrollViewPresent = baseTr.Find("Base/PresentList/ScrollViewPresent").GetComponent<ReuseScroll>();
			this.ScrollViewHistory = baseTr.Find("Base/HistoryList/ScrollViewHistory").GetComponent<ReuseScroll>();
			this.TabGroup = baseTr.Find("Base/TabGroup").GetComponent<PguiTabGroupCtrl>();
			this.PresentList = baseTr.Find("Base/PresentList").gameObject;
			this.HistoryList = baseTr.Find("Base/HistoryList").gameObject;
			this.ResPresentListBar = (GameObject)AssetManager.GetAssetData("ScenePresent/GUI/Prefab/Present_ListBar");
			this.presentBarList = new List<SelPresentCtrl.GuiPresentBar>();
		}

		// Token: 0x04004933 RID: 18739
		public GameObject baseObj;

		// Token: 0x04004934 RID: 18740
		public PguiButtonCtrl Btn_Get;

		// Token: 0x04004935 RID: 18741
		public PguiTextCtrl Txt_Caution;

		// Token: 0x04004936 RID: 18742
		public PguiTextCtrl Num_Present;

		// Token: 0x04004937 RID: 18743
		public PguiTextCtrl Txt_Info;

		// Token: 0x04004938 RID: 18744
		public ReuseScroll ScrollViewPresent;

		// Token: 0x04004939 RID: 18745
		public ReuseScroll ScrollViewHistory;

		// Token: 0x0400493A RID: 18746
		public PguiTabGroupCtrl TabGroup;

		// Token: 0x0400493B RID: 18747
		public GameObject ResPresentListBar;

		// Token: 0x0400493C RID: 18748
		public GameObject PresentList;

		// Token: 0x0400493D RID: 18749
		public GameObject HistoryList;

		// Token: 0x0400493E RID: 18750
		public List<SelPresentCtrl.GuiPresentBar> presentBarList;
	}
}
