using System;
using System.Collections.Generic;

public class QuestStaticData
{
	public Dictionary<int, QuestStaticChapter> chapterDataMap { get; set; } = new Dictionary<int, QuestStaticChapter>();

	public List<QuestStaticChapter> chapterDataList { get; set; }

	public Dictionary<int, QuestStaticMap> mapDataMap { get; set; } = new Dictionary<int, QuestStaticMap>();

	public List<QuestStaticMap> mapDataList { get; set; }

	public Dictionary<int, QuestStaticQuestGroup> groupDataMap { get; set; } = new Dictionary<int, QuestStaticQuestGroup>();

	public List<QuestStaticQuestGroup> groupDataList { get; set; }

	public Dictionary<int, QuestStaticQuestOne> oneDataMap { get; set; } = new Dictionary<int, QuestStaticQuestOne>();

	public List<QuestStaticQuestOne> oneDataList { get; set; }

	public Dictionary<int, HashSet<int>> dropItemQuestMap { get; set; } = new Dictionary<int, HashSet<int>>();

	public List<QuestStaticRule> ruleDataList { get; set; }
}
