using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using UnityEngine;

// Token: 0x02000087 RID: 135
public class DataManagerIntroductionNewChara
{
	// Token: 0x0600051E RID: 1310 RVA: 0x000238E1 File Offset: 0x00021AE1
	public DataManagerIntroductionNewChara(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x0600051F RID: 1311 RVA: 0x00023906 File Offset: 0x00021B06
	// (set) Token: 0x06000520 RID: 1312 RVA: 0x0002390E File Offset: 0x00021B0E
	private List<DataManagerIntroductionNewChara.IntroductionPlayedIdData> playedIntroductionIdList { get; set; }

	// Token: 0x06000521 RID: 1313 RVA: 0x00023918 File Offset: 0x00021B18
	public void InitializeMstData(MstManager mstManager)
	{
		this.playedIntroductionIdList = new List<DataManagerIntroductionNewChara.IntroductionPlayedIdData>();
		List<MstHomeIntroductionNewCharaData> mst = mstManager.GetMst<List<MstHomeIntroductionNewCharaData>>(MstType.HOME_INTRODUCTION_NEW_CHARA_DATA);
		if (mst != null)
		{
			long num = PrjUtil.ConvertTicksToTime(TimeManager.Now.Ticks);
			foreach (MstHomeIntroductionNewCharaData mstHomeIntroductionNewCharaData in mst)
			{
				if (mstHomeIntroductionNewCharaData.endTime >= num && DataManager.DmChara.GetCharaStaticData(mstHomeIntroductionNewCharaData.charaId) != null)
				{
					this.introductionMstDataList.Add(new DataManagerIntroductionNewChara.IntroductionNewCharaData(mstHomeIntroductionNewCharaData));
				}
			}
		}
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x000239BC File Offset: 0x00021BBC
	public void UpdateUserDataByServer(PlayerInfo playerInfo)
	{
		this.playedIntroductionIdList.Clear();
		if (!string.IsNullOrEmpty(playerInfo.played_introduction_list))
		{
			foreach (DataManagerIntroductionNewChara.IntroductionPlayedIdData introductionPlayedIdData in JsonUtility.FromJson<DataManagerIntroductionNewChara.SaveDatas>(playerInfo.played_introduction_list).datas)
			{
				DataManagerIntroductionNewChara.IntroductionPlayedIdData introductionPlayedIdData2 = new DataManagerIntroductionNewChara.IntroductionPlayedIdData(introductionPlayedIdData.introductionId, introductionPlayedIdData.charaId);
				this.playedIntroductionIdList.Add(introductionPlayedIdData2);
			}
		}
		this.UpdatePlayableList();
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x00023A50 File Offset: 0x00021C50
	private void UpdatePlayableList()
	{
		this.introductionPlayableDataList.Clear();
		DateTime now = TimeManager.Now;
		using (List<DataManagerIntroductionNewChara.IntroductionNewCharaData>.Enumerator enumerator = this.introductionMstDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DataManagerIntroductionNewChara.IntroductionNewCharaData mstData = enumerator.Current;
				DateTime startTime = mstData.startTime;
				DateTime endTime = mstData.endTime;
				if (now >= startTime && now < endTime && !this.playedIntroductionIdList.Exists((DataManagerIntroductionNewChara.IntroductionPlayedIdData item) => item.introductionId == mstData.introductionId && item.charaId == mstData.charaId))
				{
					DataManagerIntroductionNewChara.IntroductionPlayableCharaData introductionPlayableCharaData = new DataManagerIntroductionNewChara.IntroductionPlayableCharaData(mstData.introductionId, mstData.charaId, mstData.bannerId, mstData.characterFlag);
					this.introductionPlayableDataList.Add(introductionPlayableCharaData);
				}
			}
		}
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x00023B44 File Offset: 0x00021D44
	public List<DataManagerIntroductionNewChara.IntroductionPlayableCharaData> GetEnableIntroductionList()
	{
		if (this.introductionPlayableDataList != null)
		{
			return this.introductionPlayableDataList;
		}
		return new List<DataManagerIntroductionNewChara.IntroductionPlayableCharaData>();
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x00023B5C File Offset: 0x00021D5C
	public List<int> GetBannerIds()
	{
		new List<int>();
		return (from item in this.introductionPlayableDataList
			where item.bannerId != 0
			select item.bannerId).ToList<int>();
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x00023BC4 File Offset: 0x00021DC4
	public string GetJsonPlayedIntroductionList()
	{
		DataManagerIntroductionNewChara.SaveDatas saveDatas = new DataManagerIntroductionNewChara.SaveDatas();
		saveDatas.datas = new List<DataManagerIntroductionNewChara.IntroductionPlayedIdData>();
		foreach (DataManagerIntroductionNewChara.IntroductionPlayedIdData introductionPlayedIdData in this.playedIntroductionIdList)
		{
			DataManagerIntroductionNewChara.IntroductionPlayedIdData introductionPlayedIdData2 = new DataManagerIntroductionNewChara.IntroductionPlayedIdData(introductionPlayedIdData.introductionId, introductionPlayedIdData.charaId);
			saveDatas.datas.Add(introductionPlayedIdData2);
		}
		foreach (DataManagerIntroductionNewChara.IntroductionPlayableCharaData introductionPlayableCharaData in this.introductionPlayableDataList)
		{
			DataManagerIntroductionNewChara.IntroductionPlayedIdData introductionPlayedIdData3 = new DataManagerIntroductionNewChara.IntroductionPlayedIdData(introductionPlayableCharaData.introductionId, introductionPlayableCharaData.charaId);
			saveDatas.datas.Add(introductionPlayedIdData3);
		}
		return JsonUtility.ToJson(saveDatas);
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x00023CA4 File Offset: 0x00021EA4
	public bool IsPlayable()
	{
		return this.GetEnableIntroductionList().Count != 0;
	}

	// Token: 0x0400056F RID: 1391
	private DataManager parentData;

	// Token: 0x04000570 RID: 1392
	private List<DataManagerIntroductionNewChara.IntroductionNewCharaData> introductionMstDataList = new List<DataManagerIntroductionNewChara.IntroductionNewCharaData>();

	// Token: 0x04000571 RID: 1393
	private List<DataManagerIntroductionNewChara.IntroductionPlayableCharaData> introductionPlayableDataList = new List<DataManagerIntroductionNewChara.IntroductionPlayableCharaData>();

	// Token: 0x020006CC RID: 1740
	public class IntroductionNewCharaData
	{
		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06003301 RID: 13057 RVA: 0x001C113D File Offset: 0x001BF33D
		// (set) Token: 0x06003302 RID: 13058 RVA: 0x001C1145 File Offset: 0x001BF345
		public int introductionId { get; private set; }

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06003303 RID: 13059 RVA: 0x001C114E File Offset: 0x001BF34E
		// (set) Token: 0x06003304 RID: 13060 RVA: 0x001C1156 File Offset: 0x001BF356
		public int charaId { get; private set; }

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06003305 RID: 13061 RVA: 0x001C115F File Offset: 0x001BF35F
		// (set) Token: 0x06003306 RID: 13062 RVA: 0x001C1167 File Offset: 0x001BF367
		public int bannerId { get; private set; }

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06003307 RID: 13063 RVA: 0x001C1170 File Offset: 0x001BF370
		// (set) Token: 0x06003308 RID: 13064 RVA: 0x001C1178 File Offset: 0x001BF378
		public DateTime startTime { get; private set; }

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06003309 RID: 13065 RVA: 0x001C1181 File Offset: 0x001BF381
		// (set) Token: 0x0600330A RID: 13066 RVA: 0x001C1189 File Offset: 0x001BF389
		public DateTime endTime { get; private set; }

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x0600330B RID: 13067 RVA: 0x001C1192 File Offset: 0x001BF392
		// (set) Token: 0x0600330C RID: 13068 RVA: 0x001C119A File Offset: 0x001BF39A
		public int characterFlag { get; private set; }

		// Token: 0x0600330D RID: 13069 RVA: 0x001C11A4 File Offset: 0x001BF3A4
		public IntroductionNewCharaData(MstHomeIntroductionNewCharaData mstData)
		{
			this.introductionId = mstData.introductionId;
			this.charaId = mstData.charaId;
			this.bannerId = mstData.bannerId;
			this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(mstData.startTime));
			this.endTime = new DateTime(PrjUtil.ConvertTimeToTicks(mstData.endTime));
			this.characterFlag = mstData.characterFlag;
		}
	}

	// Token: 0x020006CD RID: 1741
	public class IntroductionPlayableCharaData
	{
		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x0600330E RID: 13070 RVA: 0x001C1213 File Offset: 0x001BF413
		// (set) Token: 0x0600330F RID: 13071 RVA: 0x001C121B File Offset: 0x001BF41B
		public int introductionId { get; private set; }

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06003310 RID: 13072 RVA: 0x001C1224 File Offset: 0x001BF424
		// (set) Token: 0x06003311 RID: 13073 RVA: 0x001C122C File Offset: 0x001BF42C
		public int charaId { get; private set; }

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06003312 RID: 13074 RVA: 0x001C1235 File Offset: 0x001BF435
		// (set) Token: 0x06003313 RID: 13075 RVA: 0x001C123D File Offset: 0x001BF43D
		public int bannerId { get; private set; }

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06003314 RID: 13076 RVA: 0x001C1246 File Offset: 0x001BF446
		// (set) Token: 0x06003315 RID: 13077 RVA: 0x001C124E File Offset: 0x001BF44E
		public bool characterFlag { get; private set; }

		// Token: 0x06003316 RID: 13078 RVA: 0x001C1257 File Offset: 0x001BF457
		public IntroductionPlayableCharaData(int introduction, int chara, int banner, int charaFlag)
		{
			this.introductionId = introduction;
			this.charaId = chara;
			this.bannerId = banner;
			this.characterFlag = charaFlag != 0;
		}
	}

	// Token: 0x020006CE RID: 1742
	[Serializable]
	public class IntroductionPlayedIdData
	{
		// Token: 0x06003317 RID: 13079 RVA: 0x001C127F File Offset: 0x001BF47F
		public IntroductionPlayedIdData(int introduction, int chara)
		{
			this.introductionId = introduction;
			this.charaId = chara;
		}

		// Token: 0x040030A0 RID: 12448
		public int introductionId;

		// Token: 0x040030A1 RID: 12449
		public int charaId;
	}

	// Token: 0x020006CF RID: 1743
	[Serializable]
	public class SaveDatas
	{
		// Token: 0x040030A2 RID: 12450
		public List<DataManagerIntroductionNewChara.IntroductionPlayedIdData> datas;
	}
}
