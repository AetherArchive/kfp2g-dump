using System;
using System.Collections.Generic;
using SGNFW.Mst;

// Token: 0x020000A0 RID: 160
public class PvpStaticData
{
	// Token: 0x060006C0 RID: 1728 RVA: 0x0002D92C File Offset: 0x0002BB2C
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

	// Token: 0x060006C1 RID: 1729 RVA: 0x0002DBA0 File Offset: 0x0002BDA0
	public PvpRankInfo GetPvpRankInfoByRankId(int id)
	{
		return this.rankInfoList.Find((PvpRankInfo item) => item.id == id);
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x0002DBD4 File Offset: 0x0002BDD4
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

	// Token: 0x060006C3 RID: 1731 RVA: 0x0002DC2C File Offset: 0x0002BE2C
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

	// Token: 0x0400062C RID: 1580
	public PvpStaticData.Type type;

	// Token: 0x0400062D RID: 1581
	public int seasonId;

	// Token: 0x0400062E RID: 1582
	public MstPvpData baseData;

	// Token: 0x0400062F RID: 1583
	public List<MstPvpRankData> rankMstList;

	// Token: 0x04000630 RID: 1584
	public List<PvpRankInfo> rankInfoList;

	// Token: 0x04000631 RID: 1585
	public List<MstPvpWinningBonusData> winningBonusList;

	// Token: 0x04000632 RID: 1586
	public List<MstPvpTurnBonusData> turnBonusList;

	// Token: 0x04000633 RID: 1587
	public List<MstPvpOppBonusData> oppBonusList;

	// Token: 0x04000634 RID: 1588
	public List<MstPvpDefenseData> pvpDefenseList;

	// Token: 0x04000635 RID: 1589
	public DateTime seasonStartTime;

	// Token: 0x04000636 RID: 1590
	public DateTime seasonEndTime;

	// Token: 0x04000637 RID: 1591
	public int pvpServerId;

	// Token: 0x04000638 RID: 1592
	public int rewardItemId;

	// Token: 0x04000639 RID: 1593
	public int spBattleTurnNum;

	// Token: 0x0400063A RID: 1594
	public int spEventId;

	// Token: 0x0200074B RID: 1867
	public enum Type
	{
		// Token: 0x040032C2 RID: 12994
		INVALID,
		// Token: 0x040032C3 RID: 12995
		NORMAL,
		// Token: 0x040032C4 RID: 12996
		SPECIAL
	}
}
