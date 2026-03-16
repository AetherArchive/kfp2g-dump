using System;
using System.Collections;
using SGNFW.Common;
using UnityEngine;

public class SceneScenario : BaseScene
{
	public int GetStoryType()
	{
		if (this.receiveArgs != null)
		{
			return this.receiveArgs.storyType;
		}
		return 0;
	}

	public int GetQuestId()
	{
		if (this.receiveArgs != null)
		{
			return this.receiveArgs.questId;
		}
		return 0;
	}

	public override void OnCreateScene()
	{
	}

	public override void OnEnableScene(object inArgs)
	{
		this.receiveArgs = inArgs as SceneScenario.Args;
		CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
		this.scenarioCtrl = AssetManager.InstantiateAssetData("SceneScenario/ScenarioPrefab", null).GetComponent<ScenarioScene>();
		this.scenarioCtrl.scenarioName = this.receiveArgs.scenarioName;
		this.scenarioCtrl.questId = this.receiveArgs.questId;
		this.scenarioCtrl.storyType = this.receiveArgs.storyType;
		this.storyOnly = null;
	}

	public override bool OnEnableSceneWait()
	{
		return this.scenarioCtrl.IsFinishLoad();
	}

	public override void Update()
	{
		if (this.scenarioCtrl.IsFinishPlay())
		{
			if (this.receiveArgs.tutorialSequence == TutorialUtil.Sequence.INVALID)
			{
				if (this.scenarioCtrl.storyType == 1 && this.receiveArgs.nextSceneName == SceneManager.SceneName.SceneQuest && DataManager.DmQuest.QuestStaticData.oneDataMap[this.receiveArgs.questId].StoryOnly)
				{
					if (this.storyOnly == null)
					{
						(this.storyOnly = this.StoryOnry()).MoveNext();
						return;
					}
					if (this.storyOnly.MoveNext())
					{
						return;
					}
				}
				if (this.receiveArgs.nextSceneName == SceneManager.SceneName.SceneBattle)
				{
					SoundManager.Play("prd_se_scenario_to_battle", false, false);
				}
				Singleton<SceneManager>.Instance.SetNextScene(this.receiveArgs.nextSceneName, this.receiveArgs.nextSceneArgs);
				return;
			}
			TutorialUtil.RequestNextSequence(this.receiveArgs.tutorialSequence);
		}
	}

	private IEnumerator StoryOnry()
	{
		DataManager.DmQuest.RequestActionStoryOnlyQuestStart(this.receiveArgs.questId);
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		DataManager.DmQuest.RequestActionStoryOnlyQuestEnd(this.receiveArgs.questId);
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		SceneQuest.Args.JustBeforeBattle justBeforeBattle = (this.receiveArgs.nextSceneArgs as SceneQuest.Args).justBeforeBattle;
		if (justBeforeBattle.isFirstClear && DataManager.DmQuest.QuestStaticData.oneDataMap.ContainsKey(justBeforeBattle.playQuestId))
		{
			justBeforeBattle.specialInfoItemList = DataManager.DmQuest.QuestStaticData.oneDataMap[justBeforeBattle.playQuestId].RewardItemList.ConvertAll<ItemData>((QuestStaticQuestOne.RewardItem item) => new ItemData(item.itemId, item.num));
			justBeforeBattle.specialInfoItemMovePresentBox = DataManager.DmQuest.LastQuestEndResponse != null && DataManager.DmQuest.LastQuestEndResponse.MovePresentBox;
		}
		bool flag = false;
		if (justBeforeBattle.isFirstClear)
		{
			QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(justBeforeBattle.playQuestId);
			flag = true;
			foreach (QuestStaticQuestOne questStaticQuestOne in questOnePackData.questGroup.questOneList)
			{
				QuestDynamicQuestOne questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataMap.TryGetValueEx(questStaticQuestOne.questId, null);
				if (questDynamicQuestOne == null || questDynamicQuestOne.clearNum == 0)
				{
					flag = false;
					break;
				}
			}
		}
		(this.receiveArgs.nextSceneArgs as SceneQuest.Args).initialMap = flag;
		yield break;
	}

	public override void OnDisableScene()
	{
		if (this.scenarioCtrl != null)
		{
			Object.Destroy(this.scenarioCtrl.gameObject);
			this.scenarioCtrl = null;
		}
		CanvasManager.HdlCmnMenu.SetupMenu(true, "", null, "", null, null);
	}

	public override void OnDestroyScene()
	{
	}

	private SceneScenario.Args receiveArgs;

	private ScenarioScene scenarioCtrl;

	private IEnumerator storyOnly;

	public class Args
	{
		public TutorialUtil.Sequence tutorialSequence;

		public int questId;

		public int storyType;

		public string scenarioName;

		public SceneManager.SceneName nextSceneName;

		public object nextSceneArgs;
	}
}
