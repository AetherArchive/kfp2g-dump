using System;
using System.Collections;

public class RestartResolver
{
	public RestartResolver.Result result { get; private set; }

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

	private RestartResolver.RestartStep restart;

	private bool pvp;

	private bool training;

	private bool questing;

	private bool error;

	private long hashId;

	private IEnumerator comm;

	private bool practice;

	private enum RestartStep
	{
		Init,
		Connect,
		Error,
		ErrResponse,
		Confirm,
		Response,
		ReConfirm,
		ReResponse,
		Retire,
		ReStart,
		Battle,
		Title,
		Finish
	}

	public class Result
	{
		public SceneManager.SceneName nextScene;

		public object nextSceneArgs;
	}
}
