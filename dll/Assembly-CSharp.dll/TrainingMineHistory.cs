using System;
using System.Collections.Generic;
using Battle;

public class TrainingMineHistory
{
	public int seasonId;

	public Dictionary<DayOfWeek, TrainingMineHistory.DayOfWeekHistory> dayOfWeekDataList = new Dictionary<DayOfWeek, TrainingMineHistory.DayOfWeekHistory>();

	public class DayOfWeekHistory
	{
		public DayOfWeek dayOfWeek;

		public long point;

		public DateTime updateTime;

		public SceneBattle_DeckInfo deckInfo;
	}
}
