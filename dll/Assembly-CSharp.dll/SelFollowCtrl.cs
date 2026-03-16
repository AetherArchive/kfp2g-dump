using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

public class SelFollowCtrl : MonoBehaviour
{
	private SelFollowCtrl.TabType SelectTabType { get; set; }

	private int SelectSndFilter { get; set; }

	private int SelectRcvFilter { get; set; }

	public void Init()
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)AssetManager.GetAssetData("SceneFriend/GUI/Prefab/GUI_Friend"), base.transform);
		this.guiData = new SelFollowCtrl.GUI(gameObject.transform);
		ReuseScroll scrollViewSend = this.guiData.ScrollViewSend;
		scrollViewSend.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollViewSend.onStartItem, new Action<int, GameObject>(this.OnStartItemSend));
		ReuseScroll scrollViewSend2 = this.guiData.ScrollViewSend;
		scrollViewSend2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollViewSend2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItemSend));
		this.guiData.ScrollViewSend.Setup(0, 0);
		ReuseScroll scrollViewRcv = this.guiData.ScrollViewRcv;
		scrollViewRcv.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollViewRcv.onStartItem, new Action<int, GameObject>(this.OnStartItemRcv));
		ReuseScroll scrollViewRcv2 = this.guiData.ScrollViewRcv;
		scrollViewRcv2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollViewRcv2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItemRcv));
		this.guiData.ScrollViewRcv.Setup(0, 0);
		this.guiData.Btn_Search.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Copy.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
	}

	public void Setup()
	{
		this.guiData.ScrollViewSend.Resize(0, 0);
		this.guiData.ScrollViewRcv.Resize(0, 0);
		this.guiData.FollowFollowerSearchTab.Setup(0, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectTab));
		this.guiData.AttributeFilterTab.Setup(0, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectFilter));
		this.guiData.Txt_SearchID.text = string.Empty;
		this.guiData.Txt_MyID.text = DataManager.DmUserInfo.friendId.ToString();
		this.UpdateFollowFollowerList();
		this.SelectRcvFilter = -1;
		this.SelectSndFilter = -1;
		this.SelectTabType = SelFollowCtrl.TabType.SEND_VIEW;
		this.SetSelectTab(this.SelectTabType);
		this.UpdateTab();
	}

	private void UpdateFollowFollowerList()
	{
		this.receiveFollowHelperList = new List<HelperPackData>(DataManager.DmHelper.GetReceiveFollowHelperList());
		this.sendFollowHelperList = new List<HelperPackData>(DataManager.DmHelper.GetSendFollowHelperList());
	}

	private void SetSelectTab(SelFollowCtrl.TabType tabtype)
	{
		this.guiData.AllListObj.SetActive(SelFollowCtrl.TabType.RECEIVE_VIEW == tabtype || SelFollowCtrl.TabType.SEND_VIEW == tabtype);
		this.guiData.AttributeFilterTab.gameObject.SetActive(SelFollowCtrl.TabType.RECEIVE_VIEW == tabtype || SelFollowCtrl.TabType.SEND_VIEW == tabtype);
		this.guiData.ScrollViewSend.gameObject.SetActive(SelFollowCtrl.TabType.SEND_VIEW == tabtype);
		this.guiData.ScrollViewRcv.gameObject.SetActive(SelFollowCtrl.TabType.RECEIVE_VIEW == tabtype);
		this.guiData.SearchObj.SetActive(SelFollowCtrl.TabType.SEARCH == tabtype);
		if (tabtype == SelFollowCtrl.TabType.SEND_VIEW)
		{
			if (this.guiData.AttributeFilterTab.SelectIndex != this.SelectSndFilter)
			{
				this.SelectSndFilter = ((-1 == this.SelectSndFilter) ? 0 : this.SelectSndFilter);
				this.guiData.AttributeFilterTab.SelectTab(this.SelectSndFilter);
			}
			this.guiData.Txt_None.gameObject.SetActive(this.sendFollowHelperList.Count <= 0);
			this.guiData.Txt_None.text = "誰もフォローしていません";
			return;
		}
		if (tabtype != SelFollowCtrl.TabType.RECEIVE_VIEW)
		{
			return;
		}
		if (this.guiData.AttributeFilterTab.SelectIndex != this.SelectRcvFilter)
		{
			this.SelectRcvFilter = ((-1 == this.SelectRcvFilter) ? 0 : this.SelectRcvFilter);
			this.guiData.AttributeFilterTab.SelectTab(this.SelectRcvFilter);
		}
		this.guiData.Txt_None.gameObject.SetActive(this.receiveFollowHelperList.Count <= 0);
		this.guiData.Txt_None.text = "まだフォロワーがいません";
	}

	private void UpdateTab()
	{
		switch (this.SelectTabType)
		{
		case SelFollowCtrl.TabType.SEND_VIEW:
		{
			SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData
			{
				register = SortFilterDefine.RegisterType.HELP_FOLLOW,
				filterButton = null,
				sortButton = this.guiData.sortFilter.Btn_Sort,
				sortUdButton = this.guiData.sortFilter.Btn_SortUpDown,
				funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget
				{
					helperList = this.sendFollowHelperList
				},
				funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
				{
					this.sendFollowHelperList = item.helperList;
					if (this.sendFollowHelperList != null)
					{
						this.sndFollowSortType = item.sortType;
						int num = this.sendFollowHelperList.Count / 2 + ((this.sendFollowHelperList.Count % 2 == 0) ? 0 : 1);
						this.guiData.ScrollViewSend.ResizeFocesNoMove(num);
						this.guiData.Txt_None.gameObject.SetActive(this.sendFollowHelperList.Count <= 0);
					}
				}
			};
			CanvasManager.HdlOpenWindowSortFilter.Register(registerData, false, null);
			CanvasManager.HdlOpenWindowSortFilter.RegistSortCharaAttribute((CharaDef.AttributeType)this.SelectSndFilter);
			CanvasManager.HdlOpenWindowSortFilter.SolutionList(registerData.register, null);
			this.guiData.Num_Own.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				"フォロー数",
				this.sendFollowHelperList.Count.ToString() + "/" + DataManager.DmHelper.SendFollowNum.ToString()
			});
			return;
		}
		case SelFollowCtrl.TabType.RECEIVE_VIEW:
		{
			SortWindowCtrl.RegisterData registerData2 = new SortWindowCtrl.RegisterData
			{
				register = SortFilterDefine.RegisterType.HELP_FOLLOWER,
				filterButton = null,
				sortButton = this.guiData.sortFilter.Btn_Sort,
				sortUdButton = this.guiData.sortFilter.Btn_SortUpDown,
				funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget
				{
					helperList = this.receiveFollowHelperList
				},
				funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
				{
					this.receiveFollowHelperList = item.helperList;
					if (this.receiveFollowHelperList != null)
					{
						this.rcvFollowSortType = item.sortType;
						int num2 = this.receiveFollowHelperList.Count / 2 + ((this.receiveFollowHelperList.Count % 2 == 0) ? 0 : 1);
						this.guiData.ScrollViewRcv.ResizeFocesNoMove(num2);
						this.guiData.Txt_None.gameObject.SetActive(this.receiveFollowHelperList.Count <= 0);
					}
				}
			};
			CanvasManager.HdlOpenWindowSortFilter.Register(registerData2, false, null);
			CanvasManager.HdlOpenWindowSortFilter.RegistSortCharaAttribute((CharaDef.AttributeType)this.SelectRcvFilter);
			CanvasManager.HdlOpenWindowSortFilter.SolutionList(registerData2.register, null);
			this.guiData.Num_Own.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				"フォロワー数",
				this.receiveFollowHelperList.Count.ToString() + "/" + DataManager.DmHelper.ReceiveFollowNum.ToString()
			});
			break;
		}
		case SelFollowCtrl.TabType.SEARCH:
			break;
		default:
			return;
		}
	}

	private void UpdateAttributeFilter()
	{
		SelFollowCtrl.TabType selectTabType = this.SelectTabType;
		if (selectTabType == SelFollowCtrl.TabType.SEND_VIEW)
		{
			this.sendFollowHelperList = new List<HelperPackData>(DataManager.DmHelper.GetSendFollowHelperList());
			int num = this.sendFollowHelperList.Count / 2 + ((this.sendFollowHelperList.Count % 2 == 0) ? 0 : 1);
			this.guiData.ScrollViewSend.ResizeFocesNoMove(num);
			CanvasManager.HdlOpenWindowSortFilter.RegistSortCharaAttribute((CharaDef.AttributeType)this.SelectSndFilter);
			CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.HELP_FOLLOW, null);
			return;
		}
		if (selectTabType != SelFollowCtrl.TabType.RECEIVE_VIEW)
		{
			return;
		}
		this.receiveFollowHelperList = new List<HelperPackData>(DataManager.DmHelper.GetReceiveFollowHelperList());
		int num2 = this.receiveFollowHelperList.Count / 2 + ((this.receiveFollowHelperList.Count % 2 == 0) ? 0 : 1);
		this.guiData.ScrollViewRcv.ResizeFocesNoMove(num2);
		CanvasManager.HdlOpenWindowSortFilter.RegistSortCharaAttribute((CharaDef.AttributeType)this.SelectRcvFilter);
		CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.HELP_FOLLOWER, null);
	}

	private void UpdateBarItem(int index, GameObject go, List<HelperPackData> heplerList)
	{
		for (int i = 0; i < 2; i++)
		{
			GameObject friendBar = go.transform.Find("FriendBar0" + (i + 1).ToString() + "/FriendListBar").gameObject;
			SelFollowCtrl.GuiFriendBar guiFriendBar = this.guiData.friendBarList.Find((SelFollowCtrl.GuiFriendBar item) => item.baseObj == friendBar);
			int listIndex = index * 2 + i;
			if (listIndex < heplerList.Count)
			{
				friendBar.SetActive(true);
				SelFollowCtrl.TabType selectTabType = this.SelectTabType;
				if (selectTabType != SelFollowCtrl.TabType.SEND_VIEW)
				{
					if (selectTabType == SelFollowCtrl.TabType.RECEIVE_VIEW)
					{
						guiFriendBar.Setup(heplerList[listIndex], this.SelectRcvFilter, this.rcvFollowSortType, false, false);
					}
				}
				else
				{
					guiFriendBar.Setup(heplerList[listIndex], this.SelectSndFilter, this.sndFollowSortType, true, false);
				}
				if (heplerList[listIndex].isReceiveFollow)
				{
					if (heplerList[listIndex].isSendFollow)
					{
						guiFriendBar.Btn_Follow_L.AddOnClickListener(delegate(PguiButtonCtrl button)
						{
							this.OnClickRequestFollowRelease(button, heplerList[listIndex]);
						}, PguiButtonCtrl.SoundType.DEFAULT);
						guiFriendBar.Btn_Follow_R.AddOnClickListener(delegate(PguiButtonCtrl button)
						{
							this.OnClickRequestHelperRelease(button, heplerList[listIndex]);
						}, PguiButtonCtrl.SoundType.DEFAULT);
					}
					else
					{
						guiFriendBar.Btn_Follow_L.AddOnClickListener(delegate(PguiButtonCtrl button)
						{
							this.OnClickRequestFollowRequest(button, heplerList[listIndex]);
						}, PguiButtonCtrl.SoundType.DEFAULT);
						guiFriendBar.Btn_Follow_R.AddOnClickListener(delegate(PguiButtonCtrl button)
						{
							this.OnClickRequestHelperRelease(button, heplerList[listIndex]);
						}, PguiButtonCtrl.SoundType.DEFAULT);
					}
				}
				else if (heplerList[listIndex].isSendFollow)
				{
					guiFriendBar.Btn_Follow_R.AddOnClickListener(delegate(PguiButtonCtrl button)
					{
						this.OnClickRequestFollowRelease(button, heplerList[listIndex]);
					}, PguiButtonCtrl.SoundType.DEFAULT);
				}
				guiFriendBar.Achievement.Setup(heplerList[listIndex].achievementId, true, false);
				guiFriendBar.Achievement.InvalidTouchPanel();
			}
			else
			{
				friendBar.SetActive(false);
			}
		}
	}

	private void Update()
	{
		if (this.currentEnumerator != null && !this.currentEnumerator.MoveNext())
		{
			this.currentEnumerator = null;
		}
	}

	private void OnClickButton(PguiButtonCtrl button)
	{
		if (button == this.guiData.Btn_Search)
		{
			int num;
			if (int.TryParse(this.guiData.InputField.text, out num))
			{
				this.currentEnumerator = this.RequestSearchUser(num);
				return;
			}
		}
		else if (button == this.guiData.Btn_Copy)
		{
			GUIUtility.systemCopyBuffer = DataManager.DmUserInfo.friendId.ToString();
			CanvasManager.HdlOpenWindowBasic.Setup("コピーしました", "そのままペーストしてください", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
		}
	}

	private void OnClickButtonUser(PguiButtonCtrl button)
	{
		this.currentEnumerator = this.RequestUser(this.guiData.friendBarList.Find((SelFollowCtrl.GuiFriendBar item) => item.baseButton == button).helperPackData);
	}

	private void OnClickRequestFollowRequest(PguiButtonCtrl button, HelperPackData hpd)
	{
		this.currentEnumerator = this.RequestFollowRequest(hpd);
	}

	private void OnClickRequestFollowRelease(PguiButtonCtrl button, HelperPackData hpd)
	{
		this.currentEnumerator = this.RequestFollowRelease(hpd);
	}

	private void OnClickRequestHelperRelease(PguiButtonCtrl button, HelperPackData hpd)
	{
		this.currentEnumerator = this.RequestHelperRelease(hpd);
	}

	private bool OnSelectTab(int index)
	{
		SelFollowCtrl.TabType tabType;
		switch (index)
		{
		case 0:
			tabType = SelFollowCtrl.TabType.SEND_VIEW;
			break;
		case 1:
			tabType = SelFollowCtrl.TabType.RECEIVE_VIEW;
			break;
		case 2:
			tabType = SelFollowCtrl.TabType.SEARCH;
			break;
		default:
			return false;
		}
		if (this.SelectTabType != tabType)
		{
			this.SelectTabType = tabType;
			this.SetSelectTab(this.SelectTabType);
			this.UpdateTab();
		}
		return true;
	}

	private bool OnSelectFilter(int filterIndex)
	{
		SelFollowCtrl.TabType selectTabType = this.SelectTabType;
		if (selectTabType != SelFollowCtrl.TabType.SEND_VIEW)
		{
			if (selectTabType != SelFollowCtrl.TabType.RECEIVE_VIEW)
			{
				return true;
			}
			this.SelectRcvFilter = filterIndex;
		}
		else
		{
			this.SelectSndFilter = filterIndex;
		}
		this.UpdateAttributeFilter();
		return true;
	}

	private void OnStartItemSend(int index, GameObject go)
	{
		this.SetStartItem(index, go);
	}

	private void OnStartItemRcv(int index, GameObject go)
	{
		this.SetStartItem(index, go);
	}

	private void SetStartItem(int index, GameObject go)
	{
		for (int i = 0; i < 2; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.guiData.ResFriendListBar, go.transform.Find("FriendBar0" + (i + 1).ToString()));
			gameObject.name = "FriendListBar";
			SelFollowCtrl.GuiFriendBar guiFriendBar = new SelFollowCtrl.GuiFriendBar(gameObject.transform);
			guiFriendBar.baseButton.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonUser), PguiButtonCtrl.SoundType.DEFAULT);
			this.guiData.friendBarList.Add(guiFriendBar);
		}
	}

	private void OnUpdateItemSend(int index, GameObject go)
	{
		this.UpdateBarItem(index, go, this.sendFollowHelperList);
	}

	private void OnUpdateItemRcv(int index, GameObject go)
	{
		this.UpdateBarItem(index, go, this.receiveFollowHelperList);
	}

	private IEnumerator RequestSearchUser(int friendId)
	{
		string text;
		if (friendId == DataManager.DmUserInfo.friendId)
		{
			text = "自分を検索することはできません";
		}
		else if (DataManager.DmHelper.GetSendFollowHelperList().Find((HelperPackData item) => item.friendId == friendId) != null)
		{
			text = "既にフォロー済みです";
		}
		else
		{
			DataManager.DmHelper.RequestSearchHelper(friendId);
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			HelperPackData searchHelperResult = DataManager.DmHelper.GetSearchHelperResult();
			if (searchHelperResult != null)
			{
				IEnumerator func = DataManagerHelper.RequestFollowApply(searchHelperResult, 0, true);
				while (func.MoveNext())
				{
					yield return null;
				}
				this.UpdateFollowFollowerList();
				yield break;
			}
			text = "該当するプレイヤーが見つかりませんでした";
		}
		bool isWindowFinish = false;
		CanvasManager.HdlOpenWindowBasic.Setup("検索結果", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
		{
			isWindowFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!isWindowFinish)
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator RequestUser(HelperPackData helperPackData)
	{
		if (helperPackData == null)
		{
			yield break;
		}
		DataManager.DmHelper.RequestSearchHelper(helperPackData.friendId);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		helperPackData = DataManager.DmHelper.GetSearchHelperResult();
		if (helperPackData == null)
		{
			yield break;
		}
		SelFollowCtrl.<>c__DisplayClass43_0 CS$<>8__locals1 = new SelFollowCtrl.<>c__DisplayClass43_0();
		CS$<>8__locals1.isWindowFinish = false;
		CanvasManager.HdlFollowWindowCtrl.Setup("詳細", null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
		{
			CS$<>8__locals1.isWindowFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlFollowWindowCtrl.SetUserProfile(helperPackData);
		CanvasManager.HdlFollowWindowCtrl.Open();
		while (!CS$<>8__locals1.isWindowFinish)
		{
			yield return null;
		}
		CS$<>8__locals1 = null;
		yield break;
	}

	private IEnumerator RequestFollowRequest(HelperPackData helperPackData)
	{
		if (helperPackData == null)
		{
			yield break;
		}
		DataManager.DmHelper.RequestFollowLimit(helperPackData.friendId);
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		DataManagerHelper.ExeStatusType result = (DataManagerHelper.ExeStatusType)DataManager.DmHelper.GetFollowLimitStatus();
		IEnumerator ienum = SelFollowCtrl.OpenFollowResult(result, helperPackData.userName, true);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = null;
		if (result != DataManagerHelper.ExeStatusType.SUCCESS)
		{
			yield break;
		}
		DataManagerHelper.ActionType reqType = DataManagerHelper.ActionType.FOLLOW_APPLY;
		SelFollowCtrl.<>c__DisplayClass44_0 CS$<>8__locals1 = new SelFollowCtrl.<>c__DisplayClass44_0();
		CS$<>8__locals1.isWindowFinish = false;
		CS$<>8__locals1.owAnswer = 0;
		CanvasManager.HdlOpenWindowBasic.Setup("フォロー確認", "フォローしますか？", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			CS$<>8__locals1.owAnswer = index;
			CS$<>8__locals1.isWindowFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!CS$<>8__locals1.isWindowFinish)
		{
			yield return null;
		}
		if (CS$<>8__locals1.owAnswer != 1)
		{
			yield break;
		}
		CS$<>8__locals1 = null;
		DataManager.DmHelper.RequestActionMineHelper(new List<HelperPackData> { helperPackData }, reqType);
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		result = DataManager.DmHelper.GetLastExeStatusType();
		ienum = SelFollowCtrl.OpenFollowResult(result, helperPackData.userName, false);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		this.UpdateFollowFollowerList();
		this.UpdateTab();
		SelFollowCtrl.TabType selectTabType = this.SelectTabType;
		if (selectTabType != SelFollowCtrl.TabType.SEND_VIEW)
		{
			if (selectTabType == SelFollowCtrl.TabType.RECEIVE_VIEW)
			{
				this.guiData.Num_Own.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
				{
					"フォロワー数",
					this.receiveFollowHelperList.Count.ToString() + "/" + DataManager.DmHelper.ReceiveFollowNum.ToString()
				});
			}
		}
		else
		{
			this.guiData.Num_Own.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				"フォロー数",
				this.sendFollowHelperList.Count.ToString() + "/" + DataManager.DmHelper.SendFollowNum.ToString()
			});
		}
		ienum = null;
		yield break;
	}

	private static IEnumerator OpenFollowResult(DataManagerHelper.ExeStatusType type, string targetUserName, bool isCheck)
	{
		string text = "";
		if (isCheck && type == DataManagerHelper.ExeStatusType.SUCCESS)
		{
			yield break;
		}
		bool flag;
		if (!isCheck && type == DataManagerHelper.ExeStatusType.SUCCESS)
		{
			text = targetUserName + PrjUtil.MakeMessage(" をフォローしました！");
			flag = true;
		}
		else if (type != DataManagerHelper.ExeStatusType.SUCCESS)
		{
			if (!isCheck)
			{
				text = PrjUtil.MakeMessage("フォローに失敗しました");
			}
			if (type == DataManagerHelper.ExeStatusType.FAIL_MY_OVER)
			{
				text += PrjUtil.MakeMessage("\n\nフォロー数上限のため、選択したプレイヤーをフォローできません\n");
			}
			else if (type == DataManagerHelper.ExeStatusType.FAIL_OTHER_OVER)
			{
				text += PrjUtil.MakeMessage("\n\n選択したプレイヤーのフォロワー数上限のため、フォローできません\n");
			}
			flag = true;
		}
		else
		{
			flag = false;
		}
		if (!flag)
		{
			yield break;
		}
		SelFollowCtrl.<>c__DisplayClass45_0 CS$<>8__locals1 = new SelFollowCtrl.<>c__DisplayClass45_0();
		CS$<>8__locals1.isWindowFinish = false;
		string text2 = (isCheck ? "確認" : "結果");
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage(text2), text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
		{
			CS$<>8__locals1.isWindowFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!CS$<>8__locals1.isWindowFinish)
		{
			yield return null;
		}
		CS$<>8__locals1 = null;
		yield break;
	}

	private IEnumerator RequestFollowRelease(HelperPackData helperPackData)
	{
		if (helperPackData == null)
		{
			yield break;
		}
		string keyword = "フォロー";
		DataManagerHelper.ActionType reqType = DataManagerHelper.ActionType.FOLLOW_RELEASE;
		SelFollowCtrl.<>c__DisplayClass46_0 CS$<>8__locals1 = new SelFollowCtrl.<>c__DisplayClass46_0();
		CS$<>8__locals1.isWindowFinish = false;
		CS$<>8__locals1.owAnswer = 0;
		CanvasManager.HdlOpenWindowBasic.Setup(keyword + "解除確認", keyword + "を解除しますか？", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			CS$<>8__locals1.owAnswer = index;
			CS$<>8__locals1.isWindowFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!CS$<>8__locals1.isWindowFinish)
		{
			yield return null;
		}
		if (CS$<>8__locals1.owAnswer != 1)
		{
			yield break;
		}
		CS$<>8__locals1 = null;
		DataManager.DmHelper.RequestActionMineHelper(new List<HelperPackData> { helperPackData }, reqType);
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		string text = ((DataManager.DmHelper.GetLastExeStatusType() == DataManagerHelper.ExeStatusType.SUCCESS) ? (keyword + "解除しました") : "解除に失敗しました");
		SelFollowCtrl.<>c__DisplayClass46_1 CS$<>8__locals2 = new SelFollowCtrl.<>c__DisplayClass46_1();
		CS$<>8__locals2.isWindowFinish = false;
		CanvasManager.HdlOpenWindowBasic.Setup("結果", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
		{
			CS$<>8__locals2.isWindowFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!CS$<>8__locals2.isWindowFinish)
		{
			yield return null;
		}
		CS$<>8__locals2 = null;
		this.UpdateFollowFollowerList();
		this.UpdateTab();
		SelFollowCtrl.TabType selectTabType = this.SelectTabType;
		if (selectTabType != SelFollowCtrl.TabType.SEND_VIEW)
		{
			if (selectTabType == SelFollowCtrl.TabType.RECEIVE_VIEW)
			{
				this.guiData.Num_Own.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
				{
					"フォロワー数",
					this.receiveFollowHelperList.Count.ToString() + "/" + DataManager.DmHelper.ReceiveFollowNum.ToString()
				});
			}
		}
		else
		{
			this.guiData.Num_Own.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				"フォロー数",
				this.sendFollowHelperList.Count.ToString() + "/" + DataManager.DmHelper.SendFollowNum.ToString()
			});
		}
		yield break;
	}

	private IEnumerator RequestHelperRelease(HelperPackData helperPackData)
	{
		if (helperPackData == null)
		{
			yield break;
		}
		string keyword = "フォロワー";
		DataManagerHelper.ActionType reqType = DataManagerHelper.ActionType.FOLLOWER_RELEASE;
		SelFollowCtrl.<>c__DisplayClass47_0 CS$<>8__locals1 = new SelFollowCtrl.<>c__DisplayClass47_0();
		CS$<>8__locals1.isWindowFinish = false;
		CS$<>8__locals1.owAnswer = 0;
		CanvasManager.HdlOpenWindowBasic.Setup(keyword + "解除確認", keyword + "を解除しますか？", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			CS$<>8__locals1.owAnswer = index;
			CS$<>8__locals1.isWindowFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!CS$<>8__locals1.isWindowFinish)
		{
			yield return null;
		}
		if (CS$<>8__locals1.owAnswer != 1)
		{
			yield break;
		}
		CS$<>8__locals1 = null;
		DataManager.DmHelper.RequestActionMineHelper(new List<HelperPackData> { helperPackData }, reqType);
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		string text = ((DataManager.DmHelper.GetLastExeStatusType() == DataManagerHelper.ExeStatusType.SUCCESS) ? (keyword + "解除しました") : "解除に失敗しました");
		SelFollowCtrl.<>c__DisplayClass47_1 CS$<>8__locals2 = new SelFollowCtrl.<>c__DisplayClass47_1();
		CS$<>8__locals2.isWindowFinish = false;
		CanvasManager.HdlOpenWindowBasic.Setup("結果", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
		{
			CS$<>8__locals2.isWindowFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!CS$<>8__locals2.isWindowFinish)
		{
			yield return null;
		}
		CS$<>8__locals2 = null;
		this.UpdateFollowFollowerList();
		this.UpdateTab();
		SelFollowCtrl.TabType selectTabType = this.SelectTabType;
		if (selectTabType != SelFollowCtrl.TabType.SEND_VIEW)
		{
			if (selectTabType == SelFollowCtrl.TabType.RECEIVE_VIEW)
			{
				this.guiData.Num_Own.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
				{
					"フォロワー数",
					this.receiveFollowHelperList.Count.ToString() + "/" + DataManager.DmHelper.ReceiveFollowNum.ToString()
				});
			}
		}
		else
		{
			this.guiData.Num_Own.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				"フォロー数",
				this.sendFollowHelperList.Count.ToString() + "/" + DataManager.DmHelper.SendFollowNum.ToString()
			});
		}
		yield break;
	}

	private SelFollowCtrl.GUI guiData;

	private IEnumerator currentEnumerator;

	private List<HelperPackData> sendFollowHelperList;

	private List<HelperPackData> receiveFollowHelperList;

	private SortFilterDefine.SortType sndFollowSortType = SortFilterDefine.SortType.LOGIN;

	private SortFilterDefine.SortType rcvFollowSortType = SortFilterDefine.SortType.LOGIN;

	private enum TabType
	{
		INVALID,
		SEND_VIEW,
		RECEIVE_VIEW,
		SEARCH
	}

	public class SortFilter
	{
		public SortFilter(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_FilterOnOff = baseTr.Find("Btn_FilterOnOff").GetComponent<PguiButtonCtrl>();
			this.Btn_Sort = baseTr.Find("Btn_Sort").GetComponent<PguiButtonCtrl>();
			this.Btn_SortUpDown = baseTr.Find("Btn_SortUpDown").GetComponent<PguiButtonCtrl>();
			this.Img_Up = baseTr.Find("Btn_SortUpDown/BaseImage/Img_Up").GetComponent<PguiImageCtrl>();
			this.Img_Down = baseTr.Find("Btn_SortUpDown/BaseImage/Img_Down").GetComponent<PguiImageCtrl>();
		}

		public const int SCROLL_ITEM_NUN_H = 3;

		public GameObject baseObj;

		public PguiButtonCtrl Btn_FilterOnOff;

		public PguiButtonCtrl Btn_Sort;

		public PguiButtonCtrl Btn_SortUpDown;

		public PguiImageCtrl Img_Up;

		public PguiImageCtrl Img_Down;
	}

	public class GuiFriendBar
	{
		public GuiFriendBar(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.baseButton = baseTr.GetComponent<PguiButtonCtrl>();
			if (baseTr.Find("BaseImage/Btn_Follow_L"))
			{
				this.Btn_Follow_L = baseTr.Find("BaseImage/Btn_Follow_L").GetComponent<PguiButtonCtrl>();
				this.Follow_L_text = this.Btn_Follow_L.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>();
			}
			if (baseTr.Find("BaseImage/Btn_Follow_R"))
			{
				this.Btn_Follow_R = baseTr.Find("BaseImage/Btn_Follow_R").GetComponent<PguiButtonCtrl>();
				this.Follow_R_text = this.Btn_Follow_R.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>();
			}
			this.Mark_Friend = baseTr.Find("BaseImage/Mark_Friend").GetComponent<PguiImageCtrl>();
			this.Txt_FriendName = baseTr.Find("BaseImage/Txt_FriendName").GetComponent<PguiTextCtrl>();
			this.Num_Rank = baseTr.Find("BaseImage/Num_Rank").GetComponent<PguiTextCtrl>();
			this.Txt_LastLogin = baseTr.Find("BaseImage/Txt_LastLogin").GetComponent<PguiTextCtrl>();
			this.Txt_Comment = baseTr.Find("BaseImage/Comment/Txt_Comment").GetComponent<PguiTextCtrl>();
			this.iconChara = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara, baseTr.Find("BaseImage/Icon_Chara")).GetComponent<IconCharaCtrl>();
			if (baseTr.Find("BaseImage/Icon_Attribute"))
			{
				this.Icon_Attribute = baseTr.Find("BaseImage/Icon_Attribute").GetComponent<PguiReplaceSpriteCtrl>();
			}
			this.Achievement = baseTr.Find("BaseImage/Achievement").GetComponent<AchievementCtrl>();
		}

		public void Setup(HelperPackData hpd, int attr, SortFilterDefine.SortType sortType, bool isSendView, bool dispFavorite)
		{
			this.helperPackData = hpd;
			this.Txt_FriendName.text = this.helperPackData.userName;
			this.Txt_Comment.text = this.helperPackData.comment;
			this.Txt_LastLogin.text = "最終ログイン" + TimeManager.MakeTimeSpanText(this.helperPackData.lastLoginTime, TimeManager.Now) + "前";
			this.Num_Rank.ReplaceTextByDefault("Param01", this.helperPackData.level.ToString());
			bool flag = false;
			if (dispFavorite)
			{
				this.iconChara.SetupHelper(this.helperPackData.FavoriteChara, this.helperPackData, sortType, false, null, false);
			}
			else if (0 <= attr && attr <= 6)
			{
				this.iconChara.SetupHelper(this.helperPackData.HelperCharaSetList[attr].helpChara, this.helperPackData, sortType, false, null, isSendView);
				if (this.helperPackData.HelperCharaSetList[attr].helpChara == null)
				{
					flag = true;
				}
				else if (this.helperPackData.HelperCharaSetList[attr].equipAccessory)
				{
					this.iconChara.DispMarkAccessory(true);
				}
			}
			else
			{
				this.iconChara.SetupHelper(this.helperPackData, sortType, false, null, isSendView);
			}
			this.SetAttrIcon(attr, flag);
			if (this.Btn_Follow_R != null && this.Btn_Follow_L != null)
			{
				if (this.helperPackData.isReceiveFollow)
				{
					this.Mark_Friend.gameObject.SetActive(true);
					this.Btn_Follow_R.gameObject.SetActive(true);
					this.Follow_R_text.text = "フォロワー解除";
					if (this.helperPackData.isSendFollow)
					{
						this.Mark_Friend.SetImageByName("mark_friend_both");
						this.Btn_Follow_L.gameObject.SetActive(true);
						this.Follow_L_text.text = "フォロー解除";
					}
					else
					{
						this.Mark_Friend.SetImageByName("mark_friend_receive");
						this.Btn_Follow_L.gameObject.SetActive(true);
						this.Follow_L_text.text = "フォロー";
					}
				}
				else if (this.helperPackData.isSendFollow)
				{
					this.Mark_Friend.gameObject.SetActive(true);
					this.Mark_Friend.SetImageByName("mark_friend_send");
					this.Btn_Follow_L.gameObject.SetActive(false);
					this.Btn_Follow_R.gameObject.SetActive(true);
					this.Follow_R_text.text = "フォロー解除";
				}
				else
				{
					this.Mark_Friend.gameObject.SetActive(false);
				}
			}
			else
			{
				this.Mark_Friend.gameObject.SetActive(false);
			}
			this.Achievement.Setup(this.helperPackData.achievementId, true, false);
		}

		public void SetAttrIcon(int index, bool isDisp)
		{
			if (null == this.Icon_Attribute)
			{
				return;
			}
			this.Icon_Attribute.gameObject.SetActive(isDisp);
			CharaDef.AttributeType attributeType = this.Index2AttrType(index);
			this.Icon_Attribute.InitForce();
			switch (attributeType)
			{
			case CharaDef.AttributeType.ALL:
				this.Icon_Attribute.Replace(7);
				return;
			case CharaDef.AttributeType.RED:
				this.Icon_Attribute.Replace(1);
				return;
			case CharaDef.AttributeType.GREEN:
				this.Icon_Attribute.Replace(3);
				return;
			case CharaDef.AttributeType.BLUE:
				this.Icon_Attribute.Replace(2);
				return;
			case CharaDef.AttributeType.PINK:
				this.Icon_Attribute.Replace(4);
				return;
			case CharaDef.AttributeType.LIME:
				this.Icon_Attribute.Replace(6);
				return;
			case CharaDef.AttributeType.AQUA:
				this.Icon_Attribute.Replace(5);
				return;
			default:
				this.Icon_Attribute.Replace(7);
				return;
			}
		}

		private CharaDef.AttributeType Index2AttrType(int index)
		{
			switch (index)
			{
			case 1:
				return CharaDef.AttributeType.RED;
			case 2:
				return CharaDef.AttributeType.GREEN;
			case 3:
				return CharaDef.AttributeType.BLUE;
			case 4:
				return CharaDef.AttributeType.PINK;
			case 5:
				return CharaDef.AttributeType.LIME;
			case 6:
				return CharaDef.AttributeType.AQUA;
			}
			return CharaDef.AttributeType.ALL;
		}

		public GameObject baseObj;

		public PguiButtonCtrl baseButton;

		public PguiButtonCtrl Btn_Follow_L;

		public PguiTextCtrl Follow_L_text;

		public PguiButtonCtrl Btn_Follow_R;

		public PguiTextCtrl Follow_R_text;

		public PguiImageCtrl Mark_Friend;

		public PguiTextCtrl Txt_FriendName;

		public PguiTextCtrl Num_Rank;

		public PguiTextCtrl Txt_LastLogin;

		public PguiTextCtrl Txt_Comment;

		public IconCharaCtrl iconChara;

		public PguiReplaceSpriteCtrl Icon_Attribute;

		public HelperPackData helperPackData;

		public AchievementCtrl Achievement;
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_FilterOnOff = baseTr.Find("Base/FriendListAll/SortFilterBtnsAll/Btn_FilterOnOff").GetComponent<PguiButtonCtrl>();
			this.Btn_Sort = baseTr.Find("Base/FriendListAll/SortFilterBtnsAll/Btn_Sort").GetComponent<PguiButtonCtrl>();
			this.Btn_SortUpDown = baseTr.Find("Base/FriendListAll/SortFilterBtnsAll/Btn_SortUpDown").GetComponent<PguiButtonCtrl>();
			this.Btn_Search = baseTr.Find("Base/FriendSearch/Search/Btn_Search").GetComponent<PguiButtonCtrl>();
			this.Btn_Copy = baseTr.Find("Base/FriendSearch/MyID/Btn_Copy").GetComponent<PguiButtonCtrl>();
			this.Txt_SearchID = baseTr.Find("Base/FriendSearch/Search/InputField/Placeholder").GetComponent<Text>();
			this.Txt_MyID = baseTr.Find("Base/FriendSearch/MyID/Base/Txt_ID").GetComponent<PguiTextCtrl>();
			this.ScrollViewSend = baseTr.Find("Base/FriendListAll/ScrollViewSend").GetComponent<ReuseScroll>();
			this.ScrollViewRcv = baseTr.Find("Base/FriendListAll/ScrollViewRcv").GetComponent<ReuseScroll>();
			this.FollowFollowerSearchTab = baseTr.Find("Base/TabGroup").GetComponent<PguiTabGroupCtrl>();
			this.AttributeFilterTab = baseTr.Find("Base/FriendSelect/TabGroup").GetComponent<PguiTabGroupCtrl>();
			this.Num_Own = baseTr.Find("Base/FriendListAll/SortFilterBtnsAll/Num_Own").GetComponent<PguiTextCtrl>();
			this.InputField = baseTr.Find("Base/FriendSearch/Search/InputField/").GetComponent<InputField>();
			this.AllListObj = baseTr.Find("Base/FriendListAll").gameObject;
			this.SearchObj = baseTr.Find("Base/FriendSearch").gameObject;
			this.ResFriendListBar = (GameObject)AssetManager.GetAssetData("SceneFriend/GUI/Prefab/Friend_ListBar_Friend");
			this.friendBarList = new List<SelFollowCtrl.GuiFriendBar>();
			this.sortFilter = new SelFollowCtrl.SortFilter(baseTr.Find("Base/FriendListAll/SortFilterBtnsAll"));
			this.Txt_None = baseTr.Find("Base/FriendListAll/Txt_None").GetComponent<PguiTextCtrl>();
			this.Btn_Sort.gameObject.SetActive(true);
			this.Btn_SortUpDown.gameObject.SetActive(true);
			this.Btn_FilterOnOff.gameObject.SetActive(false);
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_FilterOnOff;

		public PguiButtonCtrl Btn_Sort;

		public PguiButtonCtrl Btn_SortUpDown;

		public PguiButtonCtrl Btn_Search;

		public PguiButtonCtrl Btn_Copy;

		public Text Txt_SearchID;

		public PguiTextCtrl Txt_MyID;

		public ReuseScroll ScrollViewSend;

		public ReuseScroll ScrollViewRcv;

		public PguiTabGroupCtrl FollowFollowerSearchTab;

		public PguiTabGroupCtrl AttributeFilterTab;

		public PguiTextCtrl Num_Own;

		public GameObject AllListObj;

		public GameObject SearchObj;

		public InputField InputField;

		public GameObject ResFriendListBar;

		public SelFollowCtrl.SortFilter sortFilter;

		public List<SelFollowCtrl.GuiFriendBar> friendBarList;

		public PguiTextCtrl Txt_None;
	}
}
