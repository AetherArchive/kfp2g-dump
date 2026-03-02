using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;

// Token: 0x020000D8 RID: 216
[Serializable]
public class QuestDynamicQuestOne
{
	// Token: 0x17000206 RID: 518
	// (get) Token: 0x060009B5 RID: 2485 RVA: 0x0003B1DF File Offset: 0x000393DF
	public QuestOneStatus status
	{
		get
		{
			if (this.clearNum <= 0)
			{
				return QuestOneStatus.NEW;
			}
			if (this.evalList.IndexOf(0) >= 0)
			{
				return QuestOneStatus.CLEAR;
			}
			return QuestOneStatus.COMPLETE;
		}
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x0003B214 File Offset: 0x00039414
	public void UpdateByServer(Quest serverData)
	{
		this.questOneId = serverData.quest_id;
		this.clearNum = serverData.clear_num;
		this.playNum = serverData.play_num;
		this.todayClearNum = serverData.today_clear_num;
		this.firstClearTime = ((serverData.first_clear_time != 0L) ? new DateTime?(new DateTime(PrjUtil.ConvertTimeToTicks(serverData.first_clear_time))) : null);
		this.lastClearTime = ((serverData.last_clear_time != 0L) ? new DateTime?(new DateTime(PrjUtil.ConvertTimeToTicks(serverData.last_clear_time))) : null);
		this.evalList = new List<int>();
		for (int i = 0; i < 3; i++)
		{
			this.evalList.Add(serverData.eval & (1 << i));
		}
		this.todayRecoveryNum = serverData.today_recovery_num;
		this.openFlagByItem = serverData.open_key_flag != 0;
		this.skipCount = serverData.skip_count;
		this.skipRecoveryCount = serverData.skip_recovery_count;
	}

	// Token: 0x040007D2 RID: 2002
	public int questOneId;

	// Token: 0x040007D3 RID: 2003
	public List<int> evalList = new List<int>();

	// Token: 0x040007D4 RID: 2004
	public int playNum;

	// Token: 0x040007D5 RID: 2005
	public int clearNum;

	// Token: 0x040007D6 RID: 2006
	public int todayClearNum;

	// Token: 0x040007D7 RID: 2007
	public DateTime? firstClearTime;

	// Token: 0x040007D8 RID: 2008
	public DateTime? lastClearTime;

	// Token: 0x040007D9 RID: 2009
	public int todayRecoveryNum;

	// Token: 0x040007DA RID: 2010
	public bool openFlagByItem;

	// Token: 0x040007DB RID: 2011
	public int skipCount;

	// Token: 0x040007DC RID: 2012
	public int skipRecoveryCount;
}
