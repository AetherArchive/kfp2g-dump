using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;

[Serializable]
public class QuestDynamicQuestOne
{
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

	public int questOneId;

	public List<int> evalList = new List<int>();

	public int playNum;

	public int clearNum;

	public int todayClearNum;

	public DateTime? firstClearTime;

	public DateTime? lastClearTime;

	public int todayRecoveryNum;

	public bool openFlagByItem;

	public int skipCount;

	public int skipRecoveryCount;
}
