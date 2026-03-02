using System;
using System.Collections.Generic;
using SGNFW.Common.NativePlugin;
using UnityEngine;

// Token: 0x020000F6 RID: 246
public class LocalPushUtil
{
	// Token: 0x06000BC3 RID: 3011 RVA: 0x000456D0 File Offset: 0x000438D0
	public static void ResolveUnregistReserve()
	{
		LocalPushUtil.resolveTimeElapsed += Time.unscaledDeltaTime;
		if (LocalPushUtil.resolveTimeElapsed >= 1f)
		{
			LocalPushUtil.resolveTimeElapsed = 0f;
			DateTime now = TimeManager.Now;
			foreach (LocalPushUtil.UnregistReserve unregistReserve in LocalPushUtil.unregistReserveMap.Values)
			{
				if (unregistReserve.unregistTime > 0L && now.Ticks >= unregistReserve.unregistTime)
				{
					LocalPush.UnregisterLocalPush(unregistReserve.pushId);
					unregistReserve.unregistTime = 0L;
				}
			}
		}
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x00045778 File Offset: 0x00043978
	private static void RegisterLocalPushInternal(int id, string massage, DateTime setTime)
	{
		LocalPush.RegisterLocalPush(id, massage, setTime, null);
		LocalPushUtil.unregistReserveMap[id] = new LocalPushUtil.UnregistReserve
		{
			pushId = id,
			unregistTime = setTime.Ticks - TimeManager.Second2Tick(10L)
		};
		PlayerPrefs.SetString(string.Format("lp_ureg{0}", id), LocalPushUtil.unregistReserveMap[id].unregistTime.ToString());
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x000457E5 File Offset: 0x000439E5
	private static void UnregisterLocalPushInternal(int id)
	{
		LocalPush.UnregisterLocalPush(id);
		LocalPushUtil.unregistReserveMap.Remove(id);
		PlayerPrefs.DeleteKey(string.Format("lp_ureg{0}", id));
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x00045810 File Offset: 0x00043A10
	public static void RestoreUnregistReserveMap()
	{
		DateTime now = TimeManager.Now;
		LocalPushUtil.unregistReserveMap = new Dictionary<int, LocalPushUtil.UnregistReserve>();
		for (int i = 1; i < 6; i++)
		{
			long num = long.Parse(PlayerPrefs.GetString(string.Format("lp_ureg{0}", i), "0"));
			if (num > now.Ticks)
			{
				LocalPushUtil.unregistReserveMap[i] = new LocalPushUtil.UnregistReserve
				{
					pushId = i,
					unregistTime = num
				};
			}
		}
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x00045884 File Offset: 0x00043A84
	public static void ResolveNotification(LocalPushUtil.NotificationID id)
	{
		if (DataManager.DmUserInfo.optionData == null)
		{
			return;
		}
		DateTime now = TimeManager.Now;
		switch (id)
		{
		case LocalPushUtil.NotificationID.STAMINA_RECOVERY:
		{
			if (!DataManager.DmUserInfo.optionData.PushNotifyStaminaMax)
			{
				LocalPushUtil.UnregisterLocalPushInternal((int)id);
				return;
			}
			DateTime dateTime = new DateTime(now.Ticks + DataManager.DmUserInfo.staminaInfo.GetInfoByNow(now).allRecoveryTime.Ticks);
			if (dateTime == now)
			{
				LocalPushUtil.UnregisterLocalPushInternal((int)id);
				return;
			}
			LocalPushUtil.RegisterLocalPushInternal((int)id, "スタミナが満タンになりました！", dateTime);
			return;
		}
		case LocalPushUtil.NotificationID.PVP_STAMINA_RECOVERY:
		{
			if (!DataManager.DmUserInfo.optionData.PushNotifyPvpStaminaMax)
			{
				LocalPushUtil.UnregisterLocalPushInternal((int)id);
				return;
			}
			int seasonIdByNow = DataManager.DmPvp.GetSeasonIdByNow(now, PvpStaticData.Type.NORMAL);
			PvpPackData pvpPackDataBySeasonID = DataManager.DmPvp.GetPvpPackDataBySeasonID(seasonIdByNow);
			if (pvpPackDataBySeasonID == null || pvpPackDataBySeasonID.staticData.seasonStartTime > now)
			{
				LocalPushUtil.UnregisterLocalPushInternal((int)id);
				return;
			}
			DateTime dateTime2 = new DateTime(now.Ticks + pvpPackDataBySeasonID.dynamicData.userInfo.pvpStaminaInfo.GetInfoByNow(now).allRecoveryTime.Ticks);
			if (dateTime2 == now || dateTime2 > pvpPackDataBySeasonID.staticData.seasonEndTime)
			{
				LocalPushUtil.UnregisterLocalPushInternal((int)id);
				return;
			}
			LocalPushUtil.RegisterLocalPushInternal((int)id, "挑戦回数が満タンになりました！", dateTime2);
			return;
		}
		case LocalPushUtil.NotificationID.PICNIC_ENERGY_NOTHING:
		{
			if (!DataManager.DmUserInfo.optionData.PushNotifyGenkiZero)
			{
				LocalPushUtil.UnregisterLocalPushInternal((int)id);
				return;
			}
			if (!DataManager.DmPicnic.IsEnablePicnicData)
			{
				return;
			}
			if (!DataManager.DmPicnic.PicnicDynamicData.CharaDataList.Exists((DataManagerPicnic.CharaData item) => item.CharaId != 0))
			{
				LocalPushUtil.UnregisterLocalPushInternal((int)id);
				return;
			}
			TimeSpan timeSpan = TimeManager.Now - DataManager.DmPicnic.PicnicDynamicData.LastUpdateTime;
			int num;
			if ((num = DataManager.DmPicnic.PicnicDynamicData.Energy - (int)timeSpan.TotalSeconds) < 0)
			{
				num = 0;
			}
			DateTime dateTime3 = new DateTime(now.Ticks + TimeManager.Second2Tick((long)num));
			if (dateTime3 == now)
			{
				LocalPushUtil.UnregisterLocalPushInternal((int)id);
				return;
			}
			LocalPushUtil.RegisterLocalPushInternal((int)id, "ピクニックの元気が無くなりました", dateTime3);
			return;
		}
		case LocalPushUtil.NotificationID.COMEBACK1:
		case LocalPushUtil.NotificationID.COMEBACK2:
			if (DataManager.DmUserInfo.optionData.PushNotifyInfomation)
			{
				DateTime dateTime4 = new DateTime(now.Year, now.Month, now.Day);
				dateTime4 = dateTime4.AddDays((double)((id == LocalPushUtil.NotificationID.COMEBACK1) ? 3 : 7));
				dateTime4 = dateTime4.AddHours(12.0);
				LocalPushUtil.RegisterLocalPushInternal((int)id, "ドール＜隊長さーん！みなさん、一緒に遊びたくて\nうずうずしてますよー！", dateTime4);
				return;
			}
			LocalPushUtil.UnregisterLocalPushInternal((int)id);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x00045B10 File Offset: 0x00043D10
	public static void ClaerNotification()
	{
		for (int i = 1; i < 6; i++)
		{
			LocalPushUtil.UnregisterLocalPushInternal(i);
		}
	}

	// Token: 0x0400092C RID: 2348
	private static float resolveTimeElapsed = 0f;

	// Token: 0x0400092D RID: 2349
	private static Dictionary<int, LocalPushUtil.UnregistReserve> unregistReserveMap = new Dictionary<int, LocalPushUtil.UnregistReserve>();

	// Token: 0x0200080D RID: 2061
	private class UnregistReserve
	{
		// Token: 0x0400360D RID: 13837
		public int pushId;

		// Token: 0x0400360E RID: 13838
		public long unregistTime;
	}

	// Token: 0x0200080E RID: 2062
	public enum NotificationID
	{
		// Token: 0x04003610 RID: 13840
		INVALID,
		// Token: 0x04003611 RID: 13841
		STAMINA_RECOVERY,
		// Token: 0x04003612 RID: 13842
		PVP_STAMINA_RECOVERY,
		// Token: 0x04003613 RID: 13843
		PICNIC_ENERGY_NOTHING,
		// Token: 0x04003614 RID: 13844
		COMEBACK1,
		// Token: 0x04003615 RID: 13845
		COMEBACK2,
		// Token: 0x04003616 RID: 13846
		MAX
	}
}
