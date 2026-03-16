using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using UnityEngine;

public class DataManagerIntroductionNewChara
{
	public DataManagerIntroductionNewChara(DataManager p)
	{
		this.parentData = p;
	}

	private List<DataManagerIntroductionNewChara.IntroductionPlayedIdData> playedIntroductionIdList { get; set; }

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

	public List<DataManagerIntroductionNewChara.IntroductionPlayableCharaData> GetEnableIntroductionList()
	{
		if (this.introductionPlayableDataList != null)
		{
			return this.introductionPlayableDataList;
		}
		return new List<DataManagerIntroductionNewChara.IntroductionPlayableCharaData>();
	}

	public List<int> GetBannerIds()
	{
		new List<int>();
		return (from item in this.introductionPlayableDataList
			where item.bannerId != 0
			select item.bannerId).ToList<int>();
	}

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

	public bool IsPlayable()
	{
		return this.GetEnableIntroductionList().Count != 0;
	}

	private DataManager parentData;

	private List<DataManagerIntroductionNewChara.IntroductionNewCharaData> introductionMstDataList = new List<DataManagerIntroductionNewChara.IntroductionNewCharaData>();

	private List<DataManagerIntroductionNewChara.IntroductionPlayableCharaData> introductionPlayableDataList = new List<DataManagerIntroductionNewChara.IntroductionPlayableCharaData>();

	public class IntroductionNewCharaData
	{
		public int introductionId { get; private set; }

		public int charaId { get; private set; }

		public int bannerId { get; private set; }

		public DateTime startTime { get; private set; }

		public DateTime endTime { get; private set; }

		public int characterFlag { get; private set; }

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

	public class IntroductionPlayableCharaData
	{
		public int introductionId { get; private set; }

		public int charaId { get; private set; }

		public int bannerId { get; private set; }

		public bool characterFlag { get; private set; }

		public IntroductionPlayableCharaData(int introduction, int chara, int banner, int charaFlag)
		{
			this.introductionId = introduction;
			this.charaId = chara;
			this.bannerId = banner;
			this.characterFlag = charaFlag != 0;
		}
	}

	[Serializable]
	public class IntroductionPlayedIdData
	{
		public IntroductionPlayedIdData(int introduction, int chara)
		{
			this.introductionId = introduction;
			this.charaId = chara;
		}

		public int introductionId;

		public int charaId;
	}

	[Serializable]
	public class SaveDatas
	{
		public List<DataManagerIntroductionNewChara.IntroductionPlayedIdData> datas;
	}
}
