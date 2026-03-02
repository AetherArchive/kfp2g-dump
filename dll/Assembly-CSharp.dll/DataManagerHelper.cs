using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;

// Token: 0x0200007D RID: 125
public class DataManagerHelper
{
	// Token: 0x0600049C RID: 1180 RVA: 0x00021A8C File Offset: 0x0001FC8C
	public DataManagerHelper(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x0600049D RID: 1181 RVA: 0x00021AD9 File Offset: 0x0001FCD9
	public int SendFollowNum
	{
		get
		{
			return DataManager.DmServerMst.MstAppConfig.followLimit;
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x0600049E RID: 1182 RVA: 0x00021AEA File Offset: 0x0001FCEA
	public int ReceiveFollowNum
	{
		get
		{
			return DataManager.DmServerMst.MstAppConfig.followerLimit;
		}
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x00021AFB File Offset: 0x0001FCFB
	public List<HelperPackData> GetRentalHelperList()
	{
		return this.currentRentalHelperList;
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x00021B03 File Offset: 0x0001FD03
	public List<HelperPackData> GetReceiveFollowHelperList()
	{
		return this.receiveFollowHelperList;
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x00021B0B File Offset: 0x0001FD0B
	public List<HelperPackData> GetSendFollowHelperList()
	{
		return this.sendFollowHelperList;
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x00021B13 File Offset: 0x0001FD13
	public HelperPackData GetSearchHelperResult()
	{
		return this.searchHelper;
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x00021B1B File Offset: 0x0001FD1B
	public DataManagerHelper.ExeStatusType GetLastExeStatusType()
	{
		return this.lastExeStatusType;
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x00021B23 File Offset: 0x0001FD23
	public int GetFollowLimitStatus()
	{
		return this.followLimitStatus;
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x00021B2B File Offset: 0x0001FD2B
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

	// Token: 0x060004A6 RID: 1190 RVA: 0x00021B3A File Offset: 0x0001FD3A
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

	// Token: 0x060004A7 RID: 1191 RVA: 0x00021B57 File Offset: 0x0001FD57
	public void RequestGetFollowsList()
	{
		this.parentData.ServerRequest(FollowsListCmd.Create(), new Action<Command>(this.CbFollowsListCmd));
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x00021B75 File Offset: 0x0001FD75
	public void RequestSearchHelper(int friendId)
	{
		this.searchHelper = null;
		this.parentData.ServerRequest(HelperSearchCmd.Create(friendId), new Action<Command>(this.CbHelperSearchCmd));
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x00021B9B File Offset: 0x0001FD9B
	public void RequestFollowLimit(int target_friend_id)
	{
		this.parentData.ServerRequest(HelperFollowLimitCmd.Create(target_friend_id), new Action<Command>(this.CbHelperFollowLimitCmd));
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x00021BBC File Offset: 0x0001FDBC
	public void RequestActionMineHelper(List<HelperPackData> helperPackList, DataManagerHelper.ActionType actionType)
	{
		this.lastExeStatusType = DataManagerHelper.ExeStatusType.INVALID;
		this.requestHelperPackList = helperPackList;
		List<int> list = helperPackList.ConvertAll<int>((HelperPackData item) => item.friendId);
		this.parentData.ServerRequest(HelperMineExcecCmd.Create((int)actionType, list), new Action<Command>(this.CbHelperMineExcecCmd));
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x00021C1C File Offset: 0x0001FE1C
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

	// Token: 0x060004AC RID: 1196 RVA: 0x00021C88 File Offset: 0x0001FE88
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

	// Token: 0x060004AD RID: 1197 RVA: 0x00021DBC File Offset: 0x0001FFBC
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

	// Token: 0x060004AE RID: 1198 RVA: 0x00021DF8 File Offset: 0x0001FFF8
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

	// Token: 0x060004AF RID: 1199 RVA: 0x00021FC8 File Offset: 0x000201C8
	private void CbHelperFollowLimitCmd(Command cmd)
	{
		HelperFollowLimitResponse helperFollowLimitResponse = cmd.response as HelperFollowLimitResponse;
		this.followLimitStatus = helperFollowLimitResponse.status_follow_max;
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x00021FF0 File Offset: 0x000201F0
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

	// Token: 0x060004B1 RID: 1201 RVA: 0x000220DD File Offset: 0x000202DD
	public void ClearRentalHelperNextResetTime()
	{
		this.rentalHelperNextResetTime = TimeManager.Now;
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x000220EA File Offset: 0x000202EA
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

	// Token: 0x060004B3 RID: 1203 RVA: 0x0002210E File Offset: 0x0002030E
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

	// Token: 0x060004B4 RID: 1204 RVA: 0x0002212B File Offset: 0x0002032B
	public void UpdateLatestHelper(HelperPackData data, int idx)
	{
		this.latestHelper = data;
		this.latestAttrIdx = idx;
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x0002213B File Offset: 0x0002033B
	public HelperPackData GetLatestHelper()
	{
		return this.latestHelper;
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x00022143 File Offset: 0x00020343
	public int GetLatestAttrIdx()
	{
		return this.latestAttrIdx;
	}

	// Token: 0x04000519 RID: 1305
	private DataManager parentData;

	// Token: 0x0400051A RID: 1306
	private Dictionary<int, List<HelperPackData>> rentalHelperMap = new Dictionary<int, List<HelperPackData>>();

	// Token: 0x0400051B RID: 1307
	private List<HelperPackData> currentRentalHelperList = new List<HelperPackData>();

	// Token: 0x0400051C RID: 1308
	private List<HelperPackData> receiveFollowHelperList = new List<HelperPackData>();

	// Token: 0x0400051D RID: 1309
	private List<HelperPackData> sendFollowHelperList = new List<HelperPackData>();

	// Token: 0x0400051E RID: 1310
	private HelperPackData searchHelper;

	// Token: 0x0400051F RID: 1311
	private int followLimitStatus;

	// Token: 0x04000520 RID: 1312
	private DateTime rentalHelperNextResetTime;

	// Token: 0x04000521 RID: 1313
	private HelperPackData latestHelper;

	// Token: 0x04000522 RID: 1314
	private int latestAttrIdx = -1;

	// Token: 0x04000523 RID: 1315
	private DataManagerHelper.ExeStatusType lastExeStatusType;

	// Token: 0x04000524 RID: 1316
	private List<HelperPackData> requestHelperPackList;

	// Token: 0x020006B1 RID: 1713
	public enum ActionType
	{
		// Token: 0x0400301D RID: 12317
		INVALID,
		// Token: 0x0400301E RID: 12318
		FOLLOW_APPLY,
		// Token: 0x0400301F RID: 12319
		FOLLOW_RELEASE,
		// Token: 0x04003020 RID: 12320
		FOLLOWER_RELEASE
	}

	// Token: 0x020006B2 RID: 1714
	public enum ExeStatusType
	{
		// Token: 0x04003022 RID: 12322
		INVALID,
		// Token: 0x04003023 RID: 12323
		SUCCESS,
		// Token: 0x04003024 RID: 12324
		FAIL_UNK,
		// Token: 0x04003025 RID: 12325
		FAIL_MY_OVER,
		// Token: 0x04003026 RID: 12326
		FAIL_OTHER_OVER
	}
}
