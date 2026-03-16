using System;
using System.Collections.Generic;
using SGNFW.Mst;

public class PvpStaticData
{
	public static PvpStaticData MakeDummy(int id)
	{
		PvpStaticData pvpStaticData = new PvpStaticData();
		pvpStaticData.baseData = new MstPvpData
		{
			pvpId = 1,
			winAcquireCoin = 100,
			winAcquireExp = 200,
			winAcquireKizuna = 300,
			winAcquirePoint = 400,
			loseAcquireCoin = 30,
			stagePresetId = "SD_pvpstadium_noon_a"
		};
		pvpStaticData.rankMstList = new List<MstPvpRankData>();
		pvpStaticData.winningBonusList = new List<MstPvpWinningBonusData>();
		pvpStaticData.turnBonusList = new List<MstPvpTurnBonusData>();
		pvpStaticData.oppBonusList = new List<MstPvpOppBonusData>();
		pvpStaticData.pvpDefenseList = new List<MstPvpDefenseData>();
		int num = 3;
		for (int i = 0; i < num; i++)
		{
			pvpStaticData.rankMstList.Add(new MstPvpRankData
			{
				pvpPointMin = 100 * i,
				pvpPointMax = 100 * (i + 1) - 1,
				itemId00 = 30101,
				itemNum00 = 200 * (i + 1),
				pvpRankName = "ランク" + (i + 1).ToString()
			});
			pvpStaticData.winningBonusList.Add(new MstPvpWinningBonusData
			{
				winningNum = i * 2,
				winAcquirePoint = 10
			});
			pvpStaticData.turnBonusList.Add(new MstPvpTurnBonusData
			{
				turnNum = i * 2,
				pointOdds = 10
			});
			pvpStaticData.oppBonusList.Add(new MstPvpOppBonusData
			{
				strength = i + 1,
				pointOdds = 100 + i * 50
			});
			pvpStaticData.pvpDefenseList.Add(new MstPvpDefenseData
			{
				winNum = i * 4 + 1,
				itemId00 = 30103,
				itemNum00 = 300 * i
			});
			pvpStaticData.seasonStartTime = new DateTime(PrjUtil.ConvertTicksToTime(TimeManager.SystemNow.Ticks));
			pvpStaticData.seasonEndTime = new DateTime(PrjUtil.ConvertTicksToTime(TimeManager.SystemNow.Ticks + TimeManager.Second2Tick(100600L)));
		}
		pvpStaticData.rankInfoList = pvpStaticData.rankMstList.ConvertAll<PvpRankInfo>((MstPvpRankData item) => new PvpRankInfo
		{
			pointRangeLow = item.pvpPointMin,
			rankIcon = item.pvpRankIcon,
			rankName = item.pvpRankName
		});
		for (int j = 0; j < pvpStaticData.rankInfoList.Count - 1; j++)
		{
			pvpStaticData.rankInfoList[j].nexRankInfo = pvpStaticData.rankInfoList[j + 1];
		}
		pvpStaticData.rewardItemId = 30103;
		pvpStaticData.spBattleTurnNum = 10;
		pvpStaticData.spEventId = 0;
		return pvpStaticData;
	}

	public PvpRankInfo GetPvpRankInfoByRankId(int id)
	{
		return this.rankInfoList.Find((PvpRankInfo item) => item.id == id);
	}

	public PvpRankInfo GetPvpRankInfoByPoint(int point)
	{
		for (int i = 0; i < this.rankInfoList.Count; i++)
		{
			int num = this.rankInfoList.Count - i - 1;
			if (this.rankInfoList[num].pointRangeLow <= point)
			{
				return this.rankInfoList[num];
			}
		}
		return null;
	}

	public List<PvpRankInfo> GetPvpRankInfoByPointRange(int pointLow, int pointHigh)
	{
		List<PvpRankInfo> list = new List<PvpRankInfo>();
		int num = -1;
		int num2 = -1;
		for (int i = 0; i < this.rankInfoList.Count; i++)
		{
			int num3 = this.rankInfoList.Count - i - 1;
			if (num == -1 && this.rankInfoList[num3].pointRangeLow <= pointLow)
			{
				num = num3;
			}
			if (num2 == -1 && this.rankInfoList[num3].pointRangeLow <= pointHigh)
			{
				num2 = num3;
			}
		}
		if (num != -1 && num2 != -1)
		{
			for (int j = num; j <= num2; j++)
			{
				list.Add(this.rankInfoList[j]);
			}
		}
		return list;
	}

	public PvpStaticData.Type type;

	public int seasonId;

	public MstPvpData baseData;

	public List<MstPvpRankData> rankMstList;

	public List<PvpRankInfo> rankInfoList;

	public List<MstPvpWinningBonusData> winningBonusList;

	public List<MstPvpTurnBonusData> turnBonusList;

	public List<MstPvpOppBonusData> oppBonusList;

	public List<MstPvpDefenseData> pvpDefenseList;

	public DateTime seasonStartTime;

	public DateTime seasonEndTime;

	public int pvpServerId;

	public int rewardItemId;

	public int spBattleTurnNum;

	public int spEventId;

	public enum Type
	{
		INVALID,
		NORMAL,
		SPECIAL
	}
}
