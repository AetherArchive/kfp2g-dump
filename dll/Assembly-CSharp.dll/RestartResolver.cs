using System;
using System.Collections;

// Token: 0x02000126 RID: 294
public class RestartResolver
{
	// Token: 0x1700033D RID: 829
	// (get) Token: 0x06000F02 RID: 3842 RVA: 0x000B55A7 File Offset: 0x000B37A7
	// (set) Token: 0x06000F03 RID: 3843 RVA: 0x000B55AF File Offset: 0x000B37AF
	public RestartResolver.Result result { get; private set; }

	// Token: 0x06000F04 RID: 3844 RVA: 0x000B55B8 File Offset: 0x000B37B8
	public bool ResolveAction()
	{
		if (this.restart == RestartResolver.RestartStep.Init)
		{
			DataManager.DmQuest.RequestActionBattleRestartCheck(this.hashId = SceneBattle.GetRestart(out this.pvp, out this.training, out this.practice));
			this.restart = RestartResolver.RestartStep.Connect;
		}
		else if (this.restart == RestartResolver.RestartStep.Connect)
		{
			if (!DataManager.IsServerRequesting())
			{
				this.questing = DataManager.DmQuest.LastQuestRestartResponse != null && DataManager.DmQuest.LastQuestRestartResponse.questing != 0;
				this.error = DataManager.DmQuest.LastQuestRestartResponse != null && DataManager.DmQuest.LastQuestRestartResponse.error_type != 0;
				if (this.error)
				{
					this.restart = RestartResolver.RestartStep.Error;
				}
				else if (this.questing)
				{
					this.restart = ((this.hashId == 0L) ? RestartResolver.RestartStep.Error : RestartResolver.RestartStep.Confirm);
				}
				else if (this.hashId == 0L)
				{
					this.restart = (SceneBattle.IsRestart() ? RestartResolver.RestartStep.Confirm : RestartResolver.RestartStep.Title);
				}
				else
				{
					SceneBattle.DeleteRestart();
					this.restart = RestartResolver.RestartStep.Title;
				}
				if (this.restart == RestartResolver.RestartStep.Confirm && DataManager.DmUserInfo.tutorialSequence != TutorialUtil.Sequence.INVALID && DataManager.DmUserInfo.tutorialSequence != TutorialUtil.Sequence.END)
				{
					this.restart = RestartResolver.RestartStep.Error;
				}
				if (this.restart != RestartResolver.RestartStep.Title)
				{
					CanvasManager.SwitchLayerHdlLoadAndTipsCtrl(SceneManager.CanvasType.SYSTEM);
					CanvasManager.HdlLoadAndTipsCtrl.Close(true);
					CanvasManager.RestartFade();
				}
			}
		}
		else if (this.restart == RestartResolver.RestartStep.Error)
		{
			if (CanvasManager.HdlOpenWindowBasic.FinishedClose())
			{
				string text = "データの整合が取れないため";
				text += "\n挑戦中のバトルを中断します";
				text += "\n※挑戦時に消費したものは返還されません";
				text = text + "\nID." + DataManager.DmUserInfo.friendId.ToString();
				text = text + "\n(" + ((DataManager.DmQuest.LastQuestRestartResponse == null) ? "-1" : (this.error ? DataManager.DmQuest.LastQuestRestartResponse.error_type.ToString() : (this.questing ? "-2" : "-3"))) + ")";
				CanvasManager.HdlOpenWindowBasic.Setup("バトル再開エラー", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.RestartWindow), null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				this.restart = RestartResolver.RestartStep.ErrResponse;
			}
		}
		else if (this.restart != RestartResolver.RestartStep.ErrResponse)
		{
			if (this.restart == RestartResolver.RestartStep.Confirm)
			{
				if (CanvasManager.HdlOpenWindowBasic.FinishedClose())
				{
					string text2 = "挑戦中の";
					if (!this.questing && !this.error && !this.practice)
					{
						text2 += "デバッグ";
					}
					text2 += "バトルがあります";
					text2 += "\nバトルを再開しますか？";
					text2 = text2 + "\nID." + DataManager.DmUserInfo.friendId.ToString();
					CanvasManager.HdlOpenWindowBasic.Setup("バトル再開確認", text2, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, new PguiOpenWindowCtrl.Callback(this.RestartWindow), null, false);
					CanvasManager.HdlOpenWindowBasic.Open();
					this.restart = RestartResolver.RestartStep.Response;
				}
			}
			else if (this.restart != RestartResolver.RestartStep.Response)
			{
				if (this.restart == RestartResolver.RestartStep.ReConfirm)
				{
					if (CanvasManager.HdlOpenWindowBasic.FinishedClose())
					{
						string text3 = "挑戦中のバトルを中断してタイトル画面に戻ります";
						text3 += "\n※バトルを中断すると、進行状況が保存されず";
						text3 += "\n挑戦時に消費したものも返還されません";
						text3 += "\n本当によろしいですか？";
						CanvasManager.HdlOpenWindowBasic.Setup("バトル再開確認", text3, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, new PguiOpenWindowCtrl.Callback(this.RestartWindow), null, false);
						CanvasManager.HdlOpenWindowBasic.Open();
						this.restart = RestartResolver.RestartStep.ReResponse;
					}
				}
				else if (this.restart != RestartResolver.RestartStep.ReResponse)
				{
					if (this.restart == RestartResolver.RestartStep.Retire)
					{
						SceneBattle.DeleteRestart();
						this.comm = this.CommRestart(true);
						this.restart = RestartResolver.RestartStep.Title;
					}
					else if (this.restart == RestartResolver.RestartStep.ReStart)
					{
						this.comm = this.CommRestart(false);
						this.restart = RestartResolver.RestartStep.Battle;
					}
					else if (CanvasManager.HdlOpenWindowBasic.FinishedClose() && (this.comm == null || !this.comm.MoveNext()))
					{
						if (this.restart == RestartResolver.RestartStep.Finish)
						{
							return true;
						}
						this.result = new RestartResolver.Result();
						this.result.nextScene = ((this.restart == RestartResolver.RestartStep.Battle) ? SceneManager.SceneName.SceneBattle : SceneManager.SceneName.None);
						this.result.nextSceneArgs = null;
						this.restart = RestartResolver.RestartStep.Finish;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06000F05 RID: 3845 RVA: 0x000B5A05 File Offset: 0x000B3C05
	private IEnumerator CommRestart(bool retire)
	{
		if (!this.questing && !this.error)
		{
			yield break;
		}
		if (retire)
		{
			DataManager.DmQuest.RequestActionBattleRestart(0);
		}
		else
		{
			if (this.pvp)
			{
				DataManager.DmPvp.RequestGetPvpInfo(false, 0);
				while (DataManager.IsServerRequesting())
				{
					yield return null;
				}
				DataManager.DmDeck.RequestActionGetPvpDeck();
				while (DataManager.IsServerRequesting())
				{
					yield return null;
				}
			}
			if (this.training)
			{
				DataManager.DmTraining.RequestGetTrainingInfo();
				while (DataManager.IsServerRequesting())
				{
					yield return null;
				}
			}
			DataManager.DmQuest.RequestActionBattleRestart(1);
		}
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x000B5A1C File Offset: 0x000B3C1C
	private bool RestartWindow(int index)
	{
		if (this.restart == RestartResolver.RestartStep.Response)
		{
			this.restart = ((index == 1) ? RestartResolver.RestartStep.ReStart : RestartResolver.RestartStep.ReConfirm);
		}
		else if (this.restart == RestartResolver.RestartStep.ReResponse)
		{
			this.restart = ((index == 1) ? RestartResolver.RestartStep.Retire : RestartResolver.RestartStep.Confirm);
		}
		else if (this.restart == RestartResolver.RestartStep.ErrResponse)
		{
			this.restart = RestartResolver.RestartStep.Retire;
		}
		return true;
	}

	// Token: 0x04000D9A RID: 3482
	private RestartResolver.RestartStep restart;

	// Token: 0x04000D9B RID: 3483
	private bool pvp;

	// Token: 0x04000D9C RID: 3484
	private bool training;

	// Token: 0x04000D9D RID: 3485
	private bool questing;

	// Token: 0x04000D9E RID: 3486
	private bool error;

	// Token: 0x04000D9F RID: 3487
	private long hashId;

	// Token: 0x04000DA0 RID: 3488
	private IEnumerator comm;

	// Token: 0x04000DA1 RID: 3489
	private bool practice;

	// Token: 0x0200096A RID: 2410
	private enum RestartStep
	{
		// Token: 0x04003CDE RID: 15582
		Init,
		// Token: 0x04003CDF RID: 15583
		Connect,
		// Token: 0x04003CE0 RID: 15584
		Error,
		// Token: 0x04003CE1 RID: 15585
		ErrResponse,
		// Token: 0x04003CE2 RID: 15586
		Confirm,
		// Token: 0x04003CE3 RID: 15587
		Response,
		// Token: 0x04003CE4 RID: 15588
		ReConfirm,
		// Token: 0x04003CE5 RID: 15589
		ReResponse,
		// Token: 0x04003CE6 RID: 15590
		Retire,
		// Token: 0x04003CE7 RID: 15591
		ReStart,
		// Token: 0x04003CE8 RID: 15592
		Battle,
		// Token: 0x04003CE9 RID: 15593
		Title,
		// Token: 0x04003CEA RID: 15594
		Finish
	}

	// Token: 0x0200096B RID: 2411
	public class Result
	{
		// Token: 0x04003CEB RID: 15595
		public SceneManager.SceneName nextScene;

		// Token: 0x04003CEC RID: 15596
		public object nextSceneArgs;
	}
}
