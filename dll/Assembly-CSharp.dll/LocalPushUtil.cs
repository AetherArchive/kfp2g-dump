using System;
using System.Collections.Generic;
using SGNFW.Common.NativePlugin;
using UnityEngine;

public class LocalPushUtil
{
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

	private static void UnregisterLocalPushInternal(int id)
	{
		LocalPush.UnregisterLocalPush(id);
		LocalPushUtil.unregistReserveMap.Remove(id);
		PlayerPrefs.DeleteKey(string.Format("lp_ureg{0}", id));
	}

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

	public static void ClaerNotification()
	{
		for (int i = 1; i < 6; i++)
		{
			LocalPushUtil.UnregisterLocalPushInternal(i);
		}
	}

	private static float resolveTimeElapsed = 0f;

	private static Dictionary<int, LocalPushUtil.UnregistReserve> unregistReserveMap = new Dictionary<int, LocalPushUtil.UnregistReserve>();

	private class UnregistReserve
	{
		public int pushId;

		public long unregistTime;
	}

	public enum NotificationID
	{
		INVALID,
		STAMINA_RECOVERY,
		PVP_STAMINA_RECOVERY,
		PICNIC_ENERGY_NOTHING,
		COMEBACK1,
		COMEBACK2,
		MAX
	}
}
