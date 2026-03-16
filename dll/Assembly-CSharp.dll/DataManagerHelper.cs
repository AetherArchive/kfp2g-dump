using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;

public class DataManagerHelper
{
	public DataManagerHelper(DataManager p)
	{
		this.parentData = p;
	}

	public int SendFollowNum
	{
		get
		{
			return DataManager.DmServerMst.MstAppConfig.followLimit;
		}
	}

	public int ReceiveFollowNum
	{
		get
		{
			return DataManager.DmServerMst.MstAppConfig.followerLimit;
		}
	}

	public List<HelperPackData> GetRentalHelperList()
	{
		return this.currentRentalHelperList;
	}

	public List<HelperPackData> GetReceiveFollowHelperList()
	{
		return this.receiveFollowHelperList;
	}

	public List<HelperPackData> GetSendFollowHelperList()
	{
		return this.sendFollowHelperList;
	}

	public HelperPackData GetSearchHelperResult()
	{
		return this.searchHelper;
	}

	public DataManagerHelper.ExeStatusType GetLastExeStatusType()
	{
		return this.lastExeStatusType;
	}

	public int GetFollowLimitStatus()
	{
		return this.followLimitStatus;
	}

	public static IEnumerator RequestFollowApply(int friendId)
	{
		DataManager.DmHelper.RequestSearchHelper(friendId);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		HelperPackData searchHelperResult = DataManager.DmHelper.GetSearchHelperResult();
		if (searchHelperResult == null)
		{
			yield break;
		}
		IEnumerator ienum = DataManagerHelper.RequestFollowApply(searchHelperResult, 0, true);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	public static IEnumerator RequestFollowApply(HelperPackData helperPackData, int attr, bool dispFavorite)
	{
		if (helperPackData == null)
		{
			yield break;
		}
		if (helperPackData.isDummy)
		{
			yield break;
		}
		if (helperPackData.isSendFollow)
		{
			yield break;
		}
		if (helperPackData.friendId == DataManager.DmUserInfo.friendId)
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
		IEnumerator ienum;
		if (result != DataManagerHelper.ExeStatusType.SUCCESS)
		{
			ienum = DataManagerHelper.OpenFollowResult(result, helperPackData.userName, true);
			while (ienum.MoveNext())
			{
				yield return null;
			}
			ienum = null;
		}
		if (result != DataManagerHelper.ExeStatusType.SUCCESS)
		{
			yield break;
		}
		bool isCancel = false;
		ienum = DataManagerHelper.OpenFollowQuestion(helperPackData, attr, dispFavorite, delegate(bool action)
		{
			isCancel = action;
		});
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = null;
		if (isCancel)
		{
			yield break;
		}
		DataManager.DmHelper.RequestActionMineHelper(new List<HelperPackData> { helperPackData }, DataManagerHelper.ActionType.FOLLOW_APPLY);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		result = DataManager.DmHelper.GetLastExeStatusType();
		ienum = DataManagerHelper.OpenFollowResult(result, helperPackData.userName, false);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = null;
		yield break;
	}

	public void RequestGetFollowsList()
	{
		this.parentData.ServerRequest(FollowsListCmd.Create(), new Action<Command>(this.CbFollowsListCmd));
	}

	public void RequestSearchHelper(int friendId)
	{
		this.searchHelper = null;
		this.parentData.ServerRequest(HelperSearchCmd.Create(friendId), new Action<Command>(this.CbHelperSearchCmd));
	}

	public void RequestFollowLimit(int target_friend_id)
	{
		this.parentData.ServerRequest(HelperFollowLimitCmd.Create(target_friend_id), new Action<Command>(this.CbHelperFollowLimitCmd));
	}

	public void RequestActionMineHelper(List<HelperPackData> helperPackList, DataManagerHelper.ActionType actionType)
	{
		this.lastExeStatusType = DataManagerHelper.ExeStatusType.INVALID;
		this.requestHelperPackList = helperPackList;
		List<int> list = helperPackList.ConvertAll<int>((HelperPackData item) => item.friendId);
		this.parentData.ServerRequest(HelperMineExcecCmd.Create((int)actionType, list), new Action<Command>(this.CbHelperMineExcecCmd));
	}

	public void RequestGetRentalHelper(int questOneId, bool isChangeBtn = false)
	{
		if (this.rentalHelperNextResetTime <= TimeManager.Now)
		{
			this.rentalHelperMap.Clear();
		}
		if (this.rentalHelperMap.ContainsKey(questOneId) && !isChangeBtn)
		{
			this.currentRentalHelperList = this.rentalHelperMap[questOneId];
			return;
		}
		this.parentData.ServerRequest(HelperBattleListCmd.Create(questOneId), new Action<Command>(this.CbHelperBattleListCmd));
	}

	private void CbFollowsListCmd(Command cmd)
	{
		FollowsListResponse followsListResponse = cmd.response as FollowsListResponse;
		this.receiveFollowHelperList = new List<HelperPackData>();
		this.sendFollowHelperList = new List<HelperPackData>();
		foreach (FollowsUser followsUser in followsListResponse.followsList)
		{
			if (DataManager.DmUserInfo.friendId != followsUser.friend_id)
			{
				HelperPackData hpd = new HelperPackData(followsUser);
				if (hpd.isSendFollow)
				{
					if (followsListResponse.followsList.Find((FollowsUser item) => item.friend_id == hpd.friendId && item.is_receive_follow != 0) != null)
					{
						hpd.isReceiveFollow = true;
					}
					this.sendFollowHelperList.Add(hpd);
				}
				if (hpd.isReceiveFollow)
				{
					if (followsListResponse.followsList.Find((FollowsUser item) => item.friend_id == hpd.friendId && item.is_send_follw != 0) != null)
					{
						hpd.isSendFollow = true;
					}
					this.receiveFollowHelperList.Add(hpd);
				}
			}
		}
		this.parentData.UpdateUserAssetByAssets(followsListResponse.assets);
	}

	private void CbHelperSearchCmd(Command cmd)
	{
		HelperSearchResponse helperSearchResponse = cmd.response as HelperSearchResponse;
		if (helperSearchResponse.result_status == 0)
		{
			this.searchHelper = null;
			return;
		}
		this.searchHelper = new HelperPackData(helperSearchResponse.helper);
	}

	private void CbHelperMineExcecCmd(Command cmd)
	{
		HelperMineExcecRequest helperMineExcecRequest = cmd.request as HelperMineExcecRequest;
		HelperMineExcecResponse helperMineExcecResponse = cmd.response as HelperMineExcecResponse;
		using (List<FollowStatus>.Enumerator enumerator = helperMineExcecResponse.result_status.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				FollowStatus result = enumerator.Current;
				if (this.lastExeStatusType != DataManagerHelper.ExeStatusType.SUCCESS)
				{
					this.lastExeStatusType = (DataManagerHelper.ExeStatusType)result.status;
				}
				if (result.status == 1)
				{
					switch (helperMineExcecRequest.action_type)
					{
					case 1:
					{
						HelperPackData helperPackData = this.requestHelperPackList.Find((HelperPackData item) => item.friendId == result.friend_id);
						helperPackData.isSendFollow = true;
						helperPackData.sendFollowTime = new DateTime(PrjUtil.ConvertTimeToTicks(result.is_send_follw_datetime));
						this.sendFollowHelperList.Add(helperPackData);
						HelperPackData helperPackData2 = this.receiveFollowHelperList.Find((HelperPackData item) => item.friendId == result.friend_id);
						if (helperPackData2 != null)
						{
							helperPackData2.isSendFollow = true;
						}
						break;
					}
					case 2:
					{
						this.sendFollowHelperList.RemoveAll((HelperPackData item) => item.friendId == result.friend_id);
						HelperPackData helperPackData3 = this.receiveFollowHelperList.Find((HelperPackData item) => item.friendId == result.friend_id);
						if (helperPackData3 != null)
						{
							helperPackData3.isSendFollow = false;
						}
						break;
					}
					case 3:
					{
						this.receiveFollowHelperList.RemoveAll((HelperPackData item) => item.friendId == result.friend_id);
						HelperPackData helperPackData4 = this.sendFollowHelperList.Find((HelperPackData item) => item.friendId == result.friend_id);
						if (helperPackData4 != null)
						{
							helperPackData4.isReceiveFollow = false;
						}
						break;
					}
					}
				}
			}
		}
		this.parentData.UpdateUserAssetByAssets(helperMineExcecResponse.assets);
	}

	private void CbHelperFollowLimitCmd(Command cmd)
	{
		HelperFollowLimitResponse helperFollowLimitResponse = cmd.response as HelperFollowLimitResponse;
		this.followLimitStatus = helperFollowLimitResponse.status_follow_max;
	}

	private void CbHelperBattleListCmd(Command cmd)
	{
		HelperBattleListResponse helperBattleListResponse = cmd.response as HelperBattleListResponse;
		HelperBattleListRequest helperBattleListRequest = cmd.request as HelperBattleListRequest;
		helperBattleListResponse.helperList.RemoveAll((Helper item) => item == null);
		this.rentalHelperMap[helperBattleListRequest.questone_id] = helperBattleListResponse.helperList.ConvertAll<HelperPackData>((Helper item) => new HelperPackData(item));
		this.currentRentalHelperList = this.rentalHelperMap[helperBattleListRequest.questone_id];
		if (this.currentRentalHelperList.Count <= 0)
		{
			this.rentalHelperMap[helperBattleListRequest.questone_id].Add(HelperPackData.MakeTutorialDummy(1, ""));
		}
		this.rentalHelperNextResetTime = TimeManager.Now.AddMinutes(5.0);
	}

	public void ClearRentalHelperNextResetTime()
	{
		this.rentalHelperNextResetTime = TimeManager.Now;
	}

	private static IEnumerator OpenFollowQuestion(HelperPackData helperPackData, int attr, bool dispFavorite, Action<bool> cancel)
	{
		bool isWindowFinish = false;
		int owAnswer = 0;
		CanvasManager.HdlOpenWindowFollow.Setup("フォロー申請確認", "フォローしますか？", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			owAnswer = index;
			isWindowFinish = true;
			return true;
		}, null, false);
		new SelFollowCtrl.GuiFriendBar(CanvasManager.HdlOpenWindowFollow.m_UserInfoContent).Setup(helperPackData, attr, SortFilterDefine.SortType.LOGIN, true, dispFavorite);
		CanvasManager.HdlOpenWindowFollow.Open();
		while (!isWindowFinish)
		{
			yield return null;
		}
		if (owAnswer != 1)
		{
			cancel(true);
			yield break;
		}
		yield break;
	}

	private static IEnumerator OpenFollowResult(DataManagerHelper.ExeStatusType type, string targetUserName, bool isCheck)
	{
		string text = "";
		if (!isCheck && type == DataManagerHelper.ExeStatusType.SUCCESS)
		{
			text = targetUserName + PrjUtil.MakeMessage(" をフォローしました！");
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
		}
		bool isWindowFinish = false;
		string text2 = (isCheck ? "確認" : "結果");
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage(text2), text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
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

	public void UpdateLatestHelper(HelperPackData data, int idx)
	{
		this.latestHelper = data;
		this.latestAttrIdx = idx;
	}

	public HelperPackData GetLatestHelper()
	{
		return this.latestHelper;
	}

	public int GetLatestAttrIdx()
	{
		return this.latestAttrIdx;
	}

	private DataManager parentData;

	private Dictionary<int, List<HelperPackData>> rentalHelperMap = new Dictionary<int, List<HelperPackData>>();

	private List<HelperPackData> currentRentalHelperList = new List<HelperPackData>();

	private List<HelperPackData> receiveFollowHelperList = new List<HelperPackData>();

	private List<HelperPackData> sendFollowHelperList = new List<HelperPackData>();

	private HelperPackData searchHelper;

	private int followLimitStatus;

	private DateTime rentalHelperNextResetTime;

	private HelperPackData latestHelper;

	private int latestAttrIdx = -1;

	private DataManagerHelper.ExeStatusType lastExeStatusType;

	private List<HelperPackData> requestHelperPackList;

	public enum ActionType
	{
		INVALID,
		FOLLOW_APPLY,
		FOLLOW_RELEASE,
		FOLLOWER_RELEASE
	}

	public enum ExeStatusType
	{
		INVALID,
		SUCCESS,
		FAIL_UNK,
		FAIL_MY_OVER,
		FAIL_OTHER_OVER
	}
}
