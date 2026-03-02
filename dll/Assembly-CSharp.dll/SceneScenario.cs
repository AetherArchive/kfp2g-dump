using System;
using System.Collections;
using SGNFW.Common;
using UnityEngine;

// Token: 0x0200017A RID: 378
public class SceneScenario : BaseScene
{
	// Token: 0x06001836 RID: 6198 RVA: 0x0012A0EA File Offset: 0x001282EA
	public int GetStoryType()
	{
		if (this.receiveArgs != null)
		{
			return this.receiveArgs.storyType;
		}
		return 0;
	}

	// Token: 0x06001837 RID: 6199 RVA: 0x0012A101 File Offset: 0x00128301
	public int GetQuestId()
	{
		if (this.receiveArgs != null)
		{
			return this.receiveArgs.questId;
		}
		return 0;
	}

	// Token: 0x06001838 RID: 6200 RVA: 0x0012A118 File Offset: 0x00128318
	public override void OnCreateScene()
	{
	}

	// Token: 0x06001839 RID: 6201 RVA: 0x0012A11C File Offset: 0x0012831C
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

	// Token: 0x0600183A RID: 6202 RVA: 0x0012A1AC File Offset: 0x001283AC
	public override bool OnEnableSceneWait()
	{
		return this.scenarioCtrl.IsFinishLoad();
	}

	// Token: 0x0600183B RID: 6203 RVA: 0x0012A1BC File Offset: 0x001283BC
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

	// Token: 0x0600183C RID: 6204 RVA: 0x0012A2A4 File Offset: 0x001284A4
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

	// Token: 0x0600183D RID: 6205 RVA: 0x0012A2B3 File Offset: 0x001284B3
	public override void OnDisableScene()
	{
		if (this.scenarioCtrl != null)
		{
			Object.Destroy(this.scenarioCtrl.gameObject);
			this.scenarioCtrl = null;
		}
		CanvasManager.HdlCmnMenu.SetupMenu(true, "", null, "", null, null);
	}

	// Token: 0x0600183E RID: 6206 RVA: 0x0012A2F2 File Offset: 0x001284F2
	public override void OnDestroyScene()
	{
	}

	// Token: 0x040012A9 RID: 4777
	private SceneScenario.Args receiveArgs;

	// Token: 0x040012AA RID: 4778
	private ScenarioScene scenarioCtrl;

	// Token: 0x040012AB RID: 4779
	private IEnumerator storyOnly;

	// Token: 0x02000D39 RID: 3385
	public class Args
	{
		// Token: 0x04004DC0 RID: 19904
		public TutorialUtil.Sequence tutorialSequence;

		// Token: 0x04004DC1 RID: 19905
		public int questId;

		// Token: 0x04004DC2 RID: 19906
		public int storyType;

		// Token: 0x04004DC3 RID: 19907
		public string scenarioName;

		// Token: 0x04004DC4 RID: 19908
		public SceneManager.SceneName nextSceneName;

		// Token: 0x04004DC5 RID: 19909
		public object nextSceneArgs;
	}
}
