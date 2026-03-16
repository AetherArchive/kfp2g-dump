using System;
using System.Collections.Generic;

public class UserMissionGroup
{
	public UserMissionGroup(MissionType inType)
	{
		this.type = inType;
		switch (this.type)
		{
		case MissionType.DAILY:
			this.tabName = "デイリー";
			this.limitTime = new DateTime?(TimeManager.GetTerminalTimeByDay(TimeManager.Now));
			goto IL_00B5;
		case MissionType.WEEKLY:
			this.tabName = "ウィークリー";
			this.limitTime = new DateTime?(TimeManager.GetTerminalTimeByWeek(TimeManager.Now));
			goto IL_00B5;
		case MissionType.TOTAL:
			this.tabName = "トータル";
			goto IL_00B5;
		case MissionType.BEGINNER:
			this.tabName = "初心者";
			goto IL_00B5;
		}
		this.tabName = "";
		IL_00B5:
		this.infoName = "";
		this.viewDataList = new List<UserMissionOne>();
	}

	public UserMissionGroup(MissionType t, int evId, string name, string info, DateTime limit)
	{
		this.eventId = evId;
		this.type = t;
		this.tabName = name;
		this.infoName = info;
		this.limitTime = new DateTime?(limit);
	}

	public UserMissionGroup()
	{
		this.eventId = 0;
		this.type = MissionType.INVALID;
		this.viewDataList = new List<UserMissionOne>();
		this.receivedDataList = new List<UserMissionOne>();
		this.limitTime = new DateTime?(new DateTime(TimeManager.Now.Ticks));
	}

	public void SortOneDataList()
	{
		List<UserMissionOne> list = this.viewDataList.FindAll((UserMissionOne x) => x.IsSpecial);
		List<UserMissionOne> list2 = this.viewDataList.FindAll((UserMissionOne x) => !x.IsSpecial);
		List<UserMissionOne> list3 = new List<UserMissionOne>();
		List<UserMissionOne> list4 = new List<UserMissionOne>();
		List<UserMissionOne> list5 = new List<UserMissionOne>();
		foreach (UserMissionOne userMissionOne in list)
		{
			if (userMissionOne.isClear && userMissionOne.Received)
			{
				list3.Add(userMissionOne);
			}
			else if (userMissionOne.isClear && !userMissionOne.Received)
			{
				list4.Add(userMissionOne);
			}
			else if (!userMissionOne.isClear)
			{
				list5.Add(userMissionOne);
			}
		}
		List<UserMissionOne> list6 = new List<UserMissionOne>();
		List<UserMissionOne> list7 = new List<UserMissionOne>();
		List<UserMissionOne> list8 = new List<UserMissionOne>();
		foreach (UserMissionOne userMissionOne2 in list2)
		{
			if (userMissionOne2.isClear && userMissionOne2.Received)
			{
				list6.Add(userMissionOne2);
			}
			else if (userMissionOne2.isClear && !userMissionOne2.Received)
			{
				list7.Add(userMissionOne2);
			}
			else if (!userMissionOne2.isClear)
			{
				list8.Add(userMissionOne2);
			}
		}
		list3.Sort((UserMissionOne a, UserMissionOne b) => a.sortNum - b.sortNum);
		list4.Sort((UserMissionOne a, UserMissionOne b) => a.sortNum - b.sortNum);
		list5.Sort((UserMissionOne a, UserMissionOne b) => a.sortNum - b.sortNum);
		list6.Sort((UserMissionOne a, UserMissionOne b) => a.sortNum - b.sortNum);
		list7.Sort((UserMissionOne a, UserMissionOne b) => a.sortNum - b.sortNum);
		list8.Sort((UserMissionOne a, UserMissionOne b) => a.sortNum - b.sortNum);
		this.viewDataList = new List<UserMissionOne>();
		this.viewDataList.AddRange(list4);
		this.viewDataList.AddRange(list7);
		this.viewDataList.AddRange(list5);
		this.viewDataList.AddRange(list8);
		this.viewDataList.AddRange(list3);
		this.viewDataList.AddRange(list6);
		List<UserMissionOne> list9 = this.receivedDataList.FindAll((UserMissionOne x) => x.isClear && x.IsSpecial);
		list9.Sort((UserMissionOne a, UserMissionOne b) => a.sortNum - b.sortNum);
		List<UserMissionOne> list10 = this.receivedDataList.FindAll((UserMissionOne x) => x.isClear && !x.IsSpecial);
		list10.Sort((UserMissionOne a, UserMissionOne b) => a.sortNum - b.sortNum);
		this.receivedDataList = new List<UserMissionOne>();
		this.receivedDataList.AddRange(list9);
		this.receivedDataList.AddRange(list10);
	}

	public MissionType type;

	public int eventId;

	public DateTime? limitTime;

	public string tabName;

	public string infoName;

	public List<UserMissionOne> viewDataList = new List<UserMissionOne>();

	public List<UserMissionOne> receivedDataList = new List<UserMissionOne>();
}
