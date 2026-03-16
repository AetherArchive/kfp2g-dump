using System;
using System.Collections.Generic;
using AEAuth3;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
	public class SceneBattle_GUI
	{
		public GameObject PlayerSkill
		{
			get
			{
				return this._playerSkill;
			}
		}

		public AEImage PlayerSkillAE
		{
			get
			{
				return this._playerSkillAE;
			}
		}

		public PguiButtonCtrl BtnPlayerSkill
		{
			get
			{
				return this._btnPlayerSkill;
			}
		}

		public PguiReplaceSpriteCtrl IconPlayerSkill
		{
			get
			{
				return this._iconPlayerSkill;
			}
		}

		public PguiTextCtrl RemPlayerSkill
		{
			get
			{
				return this._remPlayerSkill;
			}
		}

		public GameObject PlayerSkillBan
		{
			get
			{
				return this._playerSkillBan;
			}
		}

		public GameObject TacticsSkill
		{
			get
			{
				return this._tacticsSkill;
			}
		}

		public PguiAECtrl TacticsSkillAE
		{
			get
			{
				return this._tacticsSkillAE;
			}
		}

		public PguiReplaceSpriteCtrl IconTacticsSkill
		{
			get
			{
				return this._iconTacticsSkill;
			}
		}

		public GameObject DeckAll
		{
			get
			{
				return this._deckAll;
			}
		}

		public List<GameObject> Chara
		{
			get
			{
				return this._chara;
			}
		}

		public List<Transform> TouchChara
		{
			get
			{
				return this._touchChara;
			}
		}

		public List<Transform> TouchWait
		{
			get
			{
				return this._touchWait;
			}
		}

		public List<PguiAECtrl> CharaSel
		{
			get
			{
				return this._charaSel;
			}
		}

		public List<AEImage> EffArtsOk
		{
			get
			{
				return this._effArtsOk;
			}
		}

		public List<AEImage> EffArtsOkDbl
		{
			get
			{
				return this._effArtsOkDbl;
			}
		}

		public List<Transform> TouchArts
		{
			get
			{
				return this._touchArts;
			}
		}

		public AEImage DeckAllAE
		{
			get
			{
				return this._deckAllAE;
			}
		}

		public GameObject ActInfoAll
		{
			get
			{
				return this._actInfoAll;
			}
		}

		public GameObject ActGageBase
		{
			get
			{
				return this._actGageBase;
			}
		}

		public PguiImageCtrl ActGageGage
		{
			get
			{
				return this._actGageGage;
			}
		}

		public PguiImageCtrl ActGageGageAdd
		{
			get
			{
				return this._actGageGageAdd;
			}
		}

		public PguiImageCtrl ActGageGageAdd2
		{
			get
			{
				return this._actGageGageAdd2;
			}
		}

		public GameObject ActGageGageFlash
		{
			get
			{
				return this._actGageGageFlash;
			}
		}

		public PguiTextCtrl NumActBefore
		{
			get
			{
				return this._numActBefore;
			}
		}

		public PguiTextCtrl NumActAfter
		{
			get
			{
				return this._numActAfter;
			}
		}

		public Transform TouchActGage
		{
			get
			{
				return this._touchActGage;
			}
		}

		public CanvasGroup ActInfo
		{
			get
			{
				return this._actInfo;
			}
		}

		public PguiTextCtrl NumUseBefore
		{
			get
			{
				return this._numUseBefore;
			}
		}

		public PguiTextCtrl NumUseAfter
		{
			get
			{
				return this._numUseAfter;
			}
		}

		public AEImage ActGageAE
		{
			get
			{
				return this._actGageAE;
			}
		}

		public PguiTextCtrl ActGageExtra
		{
			get
			{
				return this._actGageExtra;
			}
		}

		public AEImage ActInfoBackAE
		{
			get
			{
				return this._actInfoBackAE;
			}
		}

		public AEImage ActInfoFrontAE
		{
			get
			{
				return this._actInfoFrontAE;
			}
		}

		public GameObject ActGageBan
		{
			get
			{
				return this._actGageBan;
			}
		}

		public GameObject TimeAll
		{
			get
			{
				return this._timeAll;
			}
		}

		public GameObject MenuAll
		{
			get
			{
				return this._menuAll;
			}
		}

		public PguiToggleButtonCtrl BtnAuto
		{
			get
			{
				return this._btnAuto;
			}
		}

		public GameObject BtnAutoLock
		{
			get
			{
				return this._btnAutoLock;
			}
		}

		public PguiTextCtrl BtnAutoTxt
		{
			get
			{
				return this._btnAutoTxt;
			}
		}

		public SimpleAnimation AnimAutoInfo
		{
			get
			{
				return this._animAutoInfo;
			}
		}

		public GameObject BtnAutoBan
		{
			get
			{
				return this._btnAutoBan;
			}
		}

		public GameObject BtnAutoLamp
		{
			get
			{
				return this._btnAutoLamp;
			}
		}

		public PguiToggleButtonCtrl BtnFast
		{
			get
			{
				return this._btnFast;
			}
		}

		public PguiButtonCtrl BtnMenu
		{
			get
			{
				return this._btnMenu;
			}
		}

		public PguiTextCtrl NumYaseiTotal
		{
			get
			{
				return this._numYaseiTotal;
			}
		}

		public GameObject PlasmShare
		{
			get
			{
				return this._plasmShare;
			}
		}

		public PguiAECtrl PlasmShareCmnAE
		{
			get
			{
				return this._plasmShareCmnAE;
			}
		}

		public List<AEImage> PlasmShareAE
		{
			get
			{
				return this._plasmShareAE;
			}
		}

		public PguiTextCtrl numWave
		{
			get
			{
				return this._numWave;
			}
		}

		public PguiTextCtrl numTurn
		{
			get
			{
				return this._numTurn;
			}
		}

		public GameObject SelectCard
		{
			get
			{
				return this._selectCard;
			}
		}

		public SimpleAnimation AnimSelectCard
		{
			get
			{
				return this._animSelectCard;
			}
		}

		public GameObject ActionCard
		{
			get
			{
				return this._actionCard;
			}
		}

		public SimpleAnimation AnimActionCard
		{
			get
			{
				return this._animActionCard;
			}
		}

		public GameObject AnimTurn
		{
			get
			{
				return this._animTurn;
			}
		}

		public AEImage AnimTurnAE
		{
			get
			{
				return this._animTurnAE;
			}
		}

		public PguiReplaceSpriteCtrl AnimNumTurn1
		{
			get
			{
				return this._animNumTurn1;
			}
		}

		public PguiReplaceSpriteCtrl AnimNumTurn2
		{
			get
			{
				return this._animNumTurn2;
			}
		}

		public GameObject AnimTurnAct
		{
			get
			{
				return this._animTurnAct;
			}
		}

		public AEImage AnimTurnActAE
		{
			get
			{
				return this._animTurnActAE;
			}
		}

		public PguiTextCtrl AnimNumTurnAct
		{
			get
			{
				return this._animNumTurnAct;
			}
		}

		public GameObject AnimWave
		{
			get
			{
				return this._animWave;
			}
		}

		public AEImage AnimWaveAE
		{
			get
			{
				return this._animWaveAE;
			}
		}

		public PguiReplaceSpriteCtrl AnimNumWaveC1
		{
			get
			{
				return this._animNumWaveC1;
			}
		}

		public PguiReplaceSpriteCtrl AnimNumWaveC2
		{
			get
			{
				return this._animNumWaveC2;
			}
		}

		public PguiReplaceSpriteCtrl AnimNumWaveM1
		{
			get
			{
				return this._animNumWaveM1;
			}
		}

		public PguiReplaceSpriteCtrl AnimNumWaveM2
		{
			get
			{
				return this._animNumWaveM2;
			}
		}

		public SimpleAnimation InfoPlayerSkill
		{
			get
			{
				return this._infoPlayerSkill;
			}
		}

		public PguiTextCtrl NumPlayerSkill
		{
			get
			{
				return this._numPlayerSkill;
			}
		}

		public PguiButtonCtrl BtnInfoSkill
		{
			get
			{
				return this._btnInfoSkill;
			}
		}

		public GameObject AnimPlayerSkill
		{
			get
			{
				return this._animPlayerSkill;
			}
		}

		public AEImage AnimPlayerSkillAE
		{
			get
			{
				return this._animPlayerSkillAE;
			}
		}

		public AEImage RareInAE
		{
			get
			{
				return this._rareInAE;
			}
		}

		public AEImage BossInAE
		{
			get
			{
				return this._bossInAE;
			}
		}

		public AEImage BattleStartAE
		{
			get
			{
				return this._battleStartAE;
			}
		}

		public AEImage ResultAE
		{
			get
			{
				return this._resultAE;
			}
		}

		public PguiAECtrl KemonoArtsAE
		{
			get
			{
				return this._kemonoArtsAE;
			}
		}

		public PguiAECtrl KemonoFadeAE
		{
			get
			{
				return this._kemonoFadeAE;
			}
		}

		public PguiAECtrl ChainFrontAE
		{
			get
			{
				return this._chainFrontAE;
			}
		}

		public PguiAECtrl ChainBackAE
		{
			get
			{
				return this._chainBackAE;
			}
		}

		public PguiAECtrl ChainCountAE
		{
			get
			{
				return this._chainCountAE;
			}
		}

		public PguiAECtrl BeatActAE
		{
			get
			{
				return this._beatActAE;
			}
		}

		public PguiAECtrl ActionActAE
		{
			get
			{
				return this._actionActAE;
			}
		}

		public PguiAECtrl SpecialActAE
		{
			get
			{
				return this._specialActAE;
			}
		}

		public PguiAECtrl TryActAE
		{
			get
			{
				return this._tryActAE;
			}
		}

		public float TryActMove
		{
			get
			{
				return this._tryActMove;
			}
		}

		public PguiAECtrl TryActPlasmAE
		{
			get
			{
				return this._tryActPlasmAE;
			}
		}

		public List<AEImage> StaySkill
		{
			get
			{
				return this._staySkill;
			}
		}

		public List<AEImage> ScheduledSkillListAE
		{
			get
			{
				return this._scheduledSkillListAE;
			}
		}

		public List<AEImage> EnemyScheduledSkillListAE
		{
			get
			{
				return this._enemyScheduledSkillListAE;
			}
		}

		public Transform HpGage
		{
			get
			{
				return this._hpGage;
			}
		}

		public Transform NumDamage
		{
			get
			{
				return this._numDamage;
			}
		}

		public Transform OrderCard
		{
			get
			{
				return this._orderCard;
			}
		}

		public PguiAECtrl PvpChainCountAE
		{
			get
			{
				return this._pvpChainCountAE;
			}
		}

		public PguiAECtrl PvpBeatActAE
		{
			get
			{
				return this._pvpBeatActAE;
			}
		}

		public PguiAECtrl PvpActionActAE
		{
			get
			{
				return this._pvpActionActAE;
			}
		}

		public PguiAECtrl PvpTryActAE
		{
			get
			{
				return this._pvpTryActAE;
			}
		}

		public PguiAECtrl PvpSpecialActAE
		{
			get
			{
				return this._pvpSpecialActAE;
			}
		}

		public List<AEImage> PvpStaySkill
		{
			get
			{
				return this._pvpStaySkill;
			}
		}

		public SimpleAnimation PvpCoinGet
		{
			get
			{
				return this._pvpCoinGet;
			}
		}

		public SimpleAnimation ArtsInfo
		{
			get
			{
				return this._artsInfo;
			}
		}

		public SimpleAnimation StatInfo
		{
			get
			{
				return this._statInfo;
			}
		}

		public HorizontalLayoutGroup TrainingScoreL
		{
			get
			{
				return this._trainingScoreL;
			}
		}

		public HorizontalLayoutGroup TrainingScoreR
		{
			get
			{
				return this._trainingScoreR;
			}
		}

		public AEImage TrainingMissionAE1
		{
			get
			{
				return this._trainingMissionAE1;
			}
		}

		public AEImage TrainingMissionAE2
		{
			get
			{
				return this._trainingMissionAE2;
			}
		}

		public Transform PopUpInfoCard
		{
			get
			{
				return this._popUpInfoCard;
			}
		}

		public Transform PopUpInfoAction
		{
			get
			{
				return this._popUpInfoAction;
			}
		}

		public PguiTextCtrl CardInfo
		{
			get
			{
				return this._cardInfo;
			}
		}

		public PguiTextCtrl CardInfoV
		{
			get
			{
				return this._cardInfoV;
			}
		}

		public PguiTextCtrl PvpTrainingScore
		{
			get
			{
				return this._pvpTrainingScore;
			}
		}

		public PguiTextCtrl PvpTrainingScoreV
		{
			get
			{
				return this._pvpTrainingScoreV;
			}
		}

		public PguiAECtrl HugeFadeAE
		{
			get
			{
				return this._hugeFadeAE;
			}
		}

		public PguiAECtrl CaptainCautionAE
		{
			get
			{
				return this._captainCautionAE;
			}
		}

		public AEImage TacticsPlasmScoreAE
		{
			get
			{
				return this._tacticsPlasmScoreAE;
			}
		}

		public AEImage TacticsAttackAE
		{
			get
			{
				return this._tacticsAttackAE;
			}
		}

		public AEImage TacticsAE
		{
			get
			{
				return this._tacticsAE;
			}
		}

		public AEImage TacticsPlayerAE
		{
			get
			{
				return this._tacticsPlayerAE;
			}
		}

		public AEImage TacticsVersusAE
		{
			get
			{
				return this._tacticsVersusAE;
			}
		}

		public AEImage TacticsEndScoreAE
		{
			get
			{
				return this._tacticsEndScoreAE;
			}
		}

		public PguiAECtrl TickleStartAE
		{
			get
			{
				return this._tickleStartAE;
			}
		}

		public PguiAECtrl TickleResultAE
		{
			get
			{
				return this._tickleResultAE;
			}
		}

		public GameObject PracticeSign
		{
			get
			{
				return this._practiceSign;
			}
		}

		public SceneBattle_GUI(Transform baseTr)
		{
			this._playerSkill = baseTr.Find("PlayerSkill").gameObject;
			this._playerSkillAE = this._playerSkill.transform.Find("AEImage").GetComponent<AEImage>();
			this._playerSkillAE.playLoop = false;
			this._playerSkillAE.autoPlay = true;
			this._btnPlayerSkill = this._playerSkill.transform.Find("PlayerSkill/Btn_PlayerSkill").GetComponent<PguiButtonCtrl>();
			this._iconPlayerSkill = this._btnPlayerSkill.transform.Find("BaseImage/Icon_PlayerSkill").GetComponent<PguiReplaceSpriteCtrl>();
			this._remPlayerSkill = this._btnPlayerSkill.transform.Find("BaseImage/Num_Act").GetComponent<PguiTextCtrl>();
			this._playerSkillBan = this._btnPlayerSkill.transform.Find("AEImage_MarkBan").gameObject;
			this._tacticsSkill = baseTr.Find("TacticsSkill").gameObject;
			this._tacticsSkillAE = this._tacticsSkill.transform.Find("AEImage").GetComponent<PguiAECtrl>();
			this._iconTacticsSkill = this._tacticsSkill.transform.Find("TacticsSkill/BaseImage/Icon_TactiksSkill").GetComponent<PguiReplaceSpriteCtrl>();
			this._deckAll = baseTr.Find("DeckAll").gameObject;
			this._chara = new List<GameObject>();
			this._touchChara = new List<Transform>();
			this._touchWait = new List<Transform>();
			this._charaSel = new List<PguiAECtrl>();
			this._effArtsOk = new List<AEImage>();
			this._effArtsOkDbl = new List<AEImage>();
			this._touchArts = new List<Transform>();
			for (int i = 1; i <= this.numberFriends; i++)
			{
				Transform transform = this._deckAll.transform.Find("Chara0" + i.ToString());
				this._chara.Add(transform.gameObject);
				this._touchChara.Add(transform.Find("TouchCollision"));
				this._touchWait.Add(transform.Find("Card/Btn_StaySkill"));
				this._charaSel.Add(transform.Find("Card/AEImage_CharaSelect").GetComponent<PguiAECtrl>());
				transform = this._deckAll.transform.Find("Eff_ArtsOK_Front0" + i.ToString());
				AEImage aeimage = transform.GetComponent<AEImage>();
				aeimage.playLoop = true;
				aeimage.autoPlay = true;
				aeimage.playInTime = 0f;
				aeimage.playOutTime = aeimage.duration;
				aeimage.enabled = false;
				this._effArtsOk.Add(aeimage);
				this._touchArts.Add(transform.Find("TouchCollision"));
				transform = this._deckAll.transform.Find("Eff_MPdouble_ArtsOK_Front0" + i.ToString());
				aeimage = transform.GetComponent<AEImage>();
				aeimage.playLoop = true;
				aeimage.autoPlay = true;
				aeimage.playInTime = 0f;
				aeimage.playOutTime = aeimage.duration;
				aeimage.enabled = false;
				this._effArtsOkDbl.Add(aeimage);
			}
			this._deckAllAE = this._deckAll.transform.Find("AEImage_CardCtr").GetComponent<AEImage>();
			this._deckAllAE.playLoop = false;
			this._deckAllAE.autoPlay = false;
			this._deckAllAE.playTime = 2.5f;
			this._actInfoAll = baseTr.Find("ActInfoAll").gameObject;
			this._actGageBase = this._actInfoAll.transform.Find("ActGage_Base").gameObject;
			this._actGageGage = this._actGageBase.transform.Find("ActGage_Gage").GetComponent<PguiImageCtrl>();
			this._actGageGageAdd = this._actGageBase.transform.Find("ActGage_Gage_Add").GetComponent<PguiImageCtrl>();
			this._actGageGageAdd2 = this._actGageBase.transform.Find("ActGage_Gage_Add2").GetComponent<PguiImageCtrl>();
			this._actGageGageFlash = this._actGageBase.transform.Find("AEImage_GageFlash").gameObject;
			this._numActBefore = this._actGageBase.transform.Find("Num_Act_Before").GetComponent<PguiTextCtrl>();
			this._numActAfter = this._actGageBase.transform.Find("Num_Act_After").GetComponent<PguiTextCtrl>();
			this._touchActGage = this._actGageBase.transform.Find("TouchCollision");
			this._actInfo = this._actInfoAll.transform.Find("ActInfo").GetComponent<CanvasGroup>();
			this._numUseBefore = this._actInfo.transform.Find("Num_Use_Before").GetComponent<PguiTextCtrl>();
			this._numUseAfter = this._actInfo.transform.Find("Num_Use_After").GetComponent<PguiTextCtrl>();
			this._actGageAE = this._actInfoAll.transform.Find("AEImage_ActGage").GetComponent<AEImage>();
			this._actGageAE.playLoop = false;
			this._actGageAE.autoPlay = true;
			this._actGageExtra = this._actInfoAll.transform.Find("ExtraPoint").GetComponent<PguiTextCtrl>();
			this._actInfoBackAE = this._actInfoAll.transform.Find("AEImage_ActLight_Back").GetComponent<AEImage>();
			this._actInfoBackAE.playLoop = false;
			this._actInfoBackAE.autoPlay = true;
			this._actInfoFrontAE = this._actInfoAll.transform.Find("AEImage_ActLight_Front").GetComponent<AEImage>();
			this._actInfoFrontAE.playLoop = false;
			this._actInfoFrontAE.autoPlay = true;
			this._actGageBan = this._actInfoAll.transform.Find("AEImage_MarkBan").gameObject;
			this._timeAll = baseTr.Find("TimeAll").gameObject;
			this._menuAll = baseTr.Find("MenuAll").gameObject;
			this._btnAuto = this._menuAll.transform.Find("Btn_Auto").GetComponent<PguiToggleButtonCtrl>();
			this._btnAutoLock = this._btnAuto.transform.Find("Mark_Lock").gameObject;
			this._btnAutoTxt = this._btnAuto.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>();
			this._animAutoInfo = this._timeAll.transform.Find("PopUpInfo").GetComponent<SimpleAnimation>();
			this._btnAutoBan = this._btnAuto.transform.Find("AEImage_MarkBan").gameObject;
			this._btnAutoLamp = this._btnAuto.transform.Find("AEImage_AutLamp").gameObject;
			this._btnFast = this._menuAll.transform.Find("Btn_Fast").GetComponent<PguiToggleButtonCtrl>();
			this._btnMenu = this._menuAll.transform.Find("Btn_Menu").GetComponent<PguiButtonCtrl>();
			this._numYaseiTotal = this._timeAll.transform.Find("YaseiInfo/Num_Yasei_Total").GetComponent<PguiTextCtrl>();
			this._plasmShare = baseTr.Find("AEImage_PlasmShare").gameObject;
			this._plasmShareCmnAE = this._plasmShare.transform.Find("AEImage_PlasmShare_Cmn").GetComponent<PguiAECtrl>();
			this._plasmShareAE = new List<AEImage>();
			int num = 1;
			for (;;)
			{
				Transform transform2 = this._plasmShare.transform.Find("AEImage_PlasmShare" + num.ToString("D2"));
				if (transform2 == null)
				{
					break;
				}
				this._plasmShareAE.Add(transform2.GetComponent<AEImage>());
				num++;
			}
			this._numWave = this._timeAll.transform.Find("Num_Wave").GetComponent<PguiTextCtrl>();
			this._numTurn = this._timeAll.transform.Find("Num_Turn").GetComponent<PguiTextCtrl>();
			this._selectCard = baseTr.Find("CardDone_Center").gameObject;
			this._animSelectCard = this._selectCard.GetComponent<SimpleAnimation>();
			this._animSelectCard.playAutomatically = false;
			this._actionCard = baseTr.Find("CardDone_Top").gameObject;
			this._animActionCard = this._actionCard.GetComponent<SimpleAnimation>();
			this._animActionCard.playAutomatically = false;
			this._animTurn = baseTr.Find("Anim_Turn").gameObject;
			this._animTurnAE = this._animTurn.transform.Find("AEImage").GetComponent<AEImage>();
			this._animTurnAE.playLoop = false;
			this._animTurnAE.autoPlay = true;
			this._animTurnAE.playOutTime = this._animTurnAE.duration;
			this._animNumTurn1 = this._animTurn.transform.Find("Num_Turn/Num_Turn01").GetComponent<PguiReplaceSpriteCtrl>();
			this._animNumTurn2 = this._animTurn.transform.Find("Num_Turn/Num_Turn02").GetComponent<PguiReplaceSpriteCtrl>();
			this._animTurnAct = this._animTurn.transform.Find("ActGageInfo").gameObject;
			this._animTurnActAE = this._animTurnAct.transform.Find("AEImage").GetComponent<AEImage>();
			this._animTurnActAE.playLoop = false;
			this._animTurnActAE.autoPlay = true;
			this._animTurnActAE.playOutTime = this._animTurnAE.duration;
			this._animNumTurnAct = this._animTurnAct.transform.Find("ActGageInfo/Num_ActGage").GetComponent<PguiTextCtrl>();
			this._animWave = baseTr.Find("Anim_Wave").gameObject;
			this._animWaveAE = this._animWave.transform.Find("AEImage").GetComponent<AEImage>();
			this._animWaveAE.playLoop = false;
			this._animWaveAE.autoPlay = true;
			this._animWaveAE.playOutTime = this._animWaveAE.duration;
			this._animNumWaveC1 = this._animWave.transform.Find("WaveAll/Num_Wave_Current01").GetComponent<PguiReplaceSpriteCtrl>();
			this._animNumWaveC2 = this._animWave.transform.Find("WaveAll/Num_Wave_Current02").GetComponent<PguiReplaceSpriteCtrl>();
			this._animNumWaveM1 = this._animWave.transform.Find("WaveAll/Num_Wave_Max01").GetComponent<PguiReplaceSpriteCtrl>();
			this._animNumWaveM2 = this._animWave.transform.Find("WaveAll/Num_Wave_Max02").GetComponent<PguiReplaceSpriteCtrl>();
			this._infoPlayerSkill = baseTr.Find("Info_PlayerSkill").GetComponent<SimpleAnimation>();
			this._numPlayerSkill = this._infoPlayerSkill.transform.Find("Num_Times").GetComponent<PguiTextCtrl>();
			this._btnInfoSkill = this._infoPlayerSkill.transform.Find("Btn_OK").GetComponent<PguiButtonCtrl>();
			this._animPlayerSkill = baseTr.Find("Anim_PlayerSkillCutIn").gameObject;
			this._animPlayerSkillAE = this._animPlayerSkill.transform.Find("AEImage").GetComponent<AEImage>();
			this._animPlayerSkillAE.playLoop = false;
			this._animPlayerSkillAE.autoPlay = true;
			this._animPlayerSkillAE.playOutTime = this._animPlayerSkillAE.duration;
			this._rareInAE = baseTr.Find("AEImage_RareEnemyIn").GetComponent<AEImage>();
			this._rareInAE.playLoop = false;
			this._rareInAE.autoPlay = true;
			this._rareInAE.playOutTime = this._rareInAE.duration;
			this._bossInAE = baseTr.Find("AEImage_BossIn").GetComponent<AEImage>();
			this._bossInAE.playLoop = false;
			this._bossInAE.autoPlay = true;
			this._bossInAE.playOutTime = this._bossInAE.duration;
			this._battleStartAE = baseTr.Find("AEImage_BattleStart").GetComponent<AEImage>();
			this._battleStartAE.playLoop = false;
			this._battleStartAE.autoPlay = true;
			this._battleStartAE.playOutTime = this._battleStartAE.duration;
			this._resultAE = baseTr.Find("AEImage_Result").GetComponent<AEImage>();
			this._resultAE.playLoop = false;
			this._resultAE.autoPlay = true;
			this._kemonoArtsAE = baseTr.Find("AEImage_KemonoArtsAuth").GetComponent<PguiAECtrl>();
			this._kemonoFadeAE = baseTr.Find("AEImage_KemonoArtsAuth_Fade").GetComponent<PguiAECtrl>();
			this._chainFrontAE = baseTr.Find("AEImage_Chain_Front").GetComponent<PguiAECtrl>();
			this._chainBackAE = baseTr.Find("AEImage_Chain_Back/AEImage").GetComponent<PguiAECtrl>();
			this._chainCountAE = baseTr.Find("AEImage_ChainCount").GetComponent<PguiAECtrl>();
			this._beatActAE = baseTr.Find("AEImage_OrederCutIn_R/AEImage").GetComponent<PguiAECtrl>();
			this._actionActAE = baseTr.Find("AEImage_OrederCutIn_B/AEImage").GetComponent<PguiAECtrl>();
			this._specialActAE = baseTr.Find("AEImage_OrederCutIn_S/AEImage").GetComponent<PguiAECtrl>();
			this._tryActAE = baseTr.Find("AEImage_OrederCutIn_G/AEImage").GetComponent<PguiAECtrl>();
			this._tryActAE.PlayAnimation(PguiAECtrl.AmimeType.MAX, null);
			this._tryActMove = this._tryActAE.m_AEImage.playInTime;
			this._tryActPlasmAE = this._tryActAE.transform.parent.Find("AEImage_TryPlasmUp").GetComponent<PguiAECtrl>();
			Transform transform3 = baseTr.Find("StaySkill");
			this._staySkill = new List<AEImage>();
			int num2 = 1;
			for (;;)
			{
				Transform transform4 = transform3.Find("AEImage_StaySkill0" + num2.ToString());
				if (transform4 == null)
				{
					break;
				}
				AEImage aeimage2 = transform4.GetComponent<AEImage>();
				aeimage2.autoPlay = true;
				aeimage2.playOutTime = aeimage2.duration;
				aeimage2.gameObject.AddComponent<Grayscale>();
				this._staySkill.Add(aeimage2);
				aeimage2 = transform4.Find("AEImage_StaySkill_Front").GetComponent<AEImage>();
				aeimage2.autoPlay = true;
				aeimage2.playOutTime = aeimage2.duration;
				transform4.gameObject.SetActive(false);
				num2++;
			}
			transform3 = baseTr.Find("ScheduledSkill");
			this._scheduledSkillListAE = new List<AEImage>();
			if (transform3 != null)
			{
				int num3 = 1;
				for (;;)
				{
					Transform transform5 = transform3.Find("AEImage_ScheduledSkill0" + num3.ToString());
					if (transform5 == null)
					{
						break;
					}
					AEImage component = transform5.GetComponent<AEImage>();
					component.autoPlay = true;
					component.playOutTime = component.duration;
					this._scheduledSkillListAE.Add(component);
					transform5.gameObject.SetActive(false);
					num3++;
				}
			}
			transform3 = baseTr.Find("EnemyScheduledSkill");
			this._enemyScheduledSkillListAE = new List<AEImage>();
			if (transform3 != null)
			{
				int num4 = 1;
				for (;;)
				{
					Transform transform6 = transform3.Find("AEImage_ScheduledSkill0" + num4.ToString());
					if (transform6 == null)
					{
						break;
					}
					AEImage component2 = transform6.GetComponent<AEImage>();
					component2.autoPlay = true;
					component2.playOutTime = component2.duration;
					this._enemyScheduledSkillListAE.Add(component2);
					transform6.gameObject.SetActive(false);
					num4++;
				}
			}
			transform3.gameObject.SetActive(true);
			this._hpGage = new GameObject("HpGage").AddComponent<RectTransform>().transform;
			this._hpGage.SetParent(baseTr, false);
			this._hpGage.SetSiblingIndex(0);
			this._numDamage = new GameObject("NumDamage").AddComponent<RectTransform>().transform;
			this._numDamage.SetParent(baseTr, false);
			this._orderCard = new GameObject("OrderCard").transform;
			this._orderCard.SetParent(baseTr, false);
			this._orderCard.gameObject.SetActive(false);
			this._pvpChainCountAE = baseTr.Find("AEImage_ChainCount_PvPEnemy").GetComponent<PguiAECtrl>();
			this._pvpBeatActAE = baseTr.Find("AEImage_OrederCutIn_R_PvPEnemy/AEImage").GetComponent<PguiAECtrl>();
			this._pvpActionActAE = baseTr.Find("AEImage_OrederCutIn_B_PvPEnemy/AEImage").GetComponent<PguiAECtrl>();
			this._pvpTryActAE = baseTr.Find("AEImage_OrederCutIn_G_PvPEnemy/AEImage").GetComponent<PguiAECtrl>();
			this._pvpSpecialActAE = baseTr.Find("AEImage_OrederCutIn_S_PvPEnemy/AEImage").GetComponent<PguiAECtrl>();
			transform3 = baseTr.Find("StaySkill_PvPEnemy");
			this._pvpStaySkill = new List<AEImage>();
			int num5 = 1;
			for (;;)
			{
				Transform transform7 = transform3.Find("AEImage_StaySkill0" + num5.ToString());
				if (transform7 == null)
				{
					break;
				}
				AEImage aeimage3 = transform7.GetComponent<AEImage>();
				aeimage3.autoPlay = true;
				aeimage3.playOutTime = aeimage3.duration;
				aeimage3.gameObject.AddComponent<Grayscale>();
				this._pvpStaySkill.Add(aeimage3);
				aeimage3 = transform7.Find("AEImage_StaySkill_Front").GetComponent<AEImage>();
				aeimage3.autoPlay = true;
				aeimage3.playOutTime = aeimage3.duration;
				transform7.gameObject.SetActive(false);
				num5++;
			}
			transform3.gameObject.SetActive(true);
			this._pvpCoinGet = baseTr.Find("PvP_CutInInfo_Get").GetComponent<SimpleAnimation>();
			this._artsInfo = baseTr.Find("TapInfo_KemonoMiracle").GetComponent<SimpleAnimation>();
			this._statInfo = baseTr.Find("TapInfo_BattleStatus").GetComponent<SimpleAnimation>();
			this._trainingMissionAE1 = baseTr.Find("AEImage_Training_MissionClear/Bg").GetComponent<AEImage>();
			this._trainingMissionAE1.playLoop = false;
			this._trainingMissionAE1.autoPlay = true;
			this._trainingMissionAE1.playOutTime = this._trainingMissionAE1.duration;
			this._trainingMissionAE1.IsUnscaledTime = true;
			this._trainingMissionAE2 = baseTr.Find("AEImage_Training_MissionClear/MissionClear").GetComponent<AEImage>();
			this._trainingMissionAE2.playLoop = false;
			this._trainingMissionAE2.autoPlay = true;
			this._trainingMissionAE2.playOutTime = this._trainingMissionAE2.duration;
			this._trainingMissionAE2.IsUnscaledTime = true;
			this._trainingScoreL = baseTr.Find("Training_ScoreL/ScoreAll").GetComponent<HorizontalLayoutGroup>();
			this._trainingScoreR = baseTr.Find("Training_ScoreR/ScoreAll").GetComponent<HorizontalLayoutGroup>();
			this._popUpInfoCard = baseTr.Find("Null_PopUpInfo_Card");
			this._popUpInfoAction = baseTr.Find("Null_PopUpInfo_Action");
			this._cardInfo = this._timeAll.transform.Find("CardInfo").GetComponent<PguiTextCtrl>();
			this._cardInfoV = this._timeAll.transform.Find("CardInfoV").GetComponent<PguiTextCtrl>();
			this._pvpTrainingScore = this._timeAll.transform.Find("PvpTrainingScore").GetComponent<PguiTextCtrl>();
			this._pvpTrainingScoreV = this._timeAll.transform.Find("PvpTrainingScoreV").GetComponent<PguiTextCtrl>();
			this._hugeFadeAE = baseTr.Find("AEImage_WhiteFade").GetComponent<PguiAECtrl>();
			this._captainCautionAE = baseTr.Find("AEImage_Caution").GetComponent<PguiAECtrl>();
			this._tacticsPlasmScoreAE = baseTr.Find("AEImage_Tactics_PlasmScore").GetComponent<AEImage>();
			this._tacticsAttackAE = baseTr.Find("AEImage_Tactics_Attack").GetComponent<AEImage>();
			this._tacticsAE = baseTr.Find("AEImage_Tactics/Bg").GetComponent<AEImage>();
			this._tacticsAE.playLoop = false;
			this._tacticsAE.autoPlay = true;
			this._tacticsAE.playOutTime = this._tacticsAE.duration;
			this._tacticsAE.IsUnscaledTime = true;
			this._tacticsPlayerAE = baseTr.Find("AEImage_Tactics/AEImage_OwnTactics").GetComponent<AEImage>();
			this._tacticsPlayerAE.playLoop = false;
			this._tacticsPlayerAE.autoPlay = true;
			this._tacticsPlayerAE.playOutTime = this._tacticsPlayerAE.duration;
			this._tacticsPlayerAE.IsUnscaledTime = true;
			this._tacticsVersusAE = baseTr.Find("AEImage_Tactics/AEImage_EnemyTactics").GetComponent<AEImage>();
			this._tacticsVersusAE.playLoop = false;
			this._tacticsVersusAE.autoPlay = true;
			this._tacticsVersusAE.playOutTime = this._tacticsVersusAE.duration;
			this._tacticsVersusAE.IsUnscaledTime = true;
			this._tacticsEndScoreAE = baseTr.Find("AEImage_TacticsEnd_ToalDmage").GetComponent<AEImage>();
			baseTr.Find("AEImage_Tactics").SetAsLastSibling();
			this._tickleStartAE = baseTr.Find("AEImage_Tickling_start").GetComponent<PguiAECtrl>();
			this._tickleResultAE = baseTr.Find("AEImage_Tickling_result").GetComponent<PguiAECtrl>();
			this._practiceSign = baseTr.Find("Practice").gameObject;
		}

		private readonly int numberFriends = 5;

		private GameObject _playerSkill;

		private AEImage _playerSkillAE;

		private PguiButtonCtrl _btnPlayerSkill;

		private PguiReplaceSpriteCtrl _iconPlayerSkill;

		private PguiTextCtrl _remPlayerSkill;

		private GameObject _playerSkillBan;

		private GameObject _tacticsSkill;

		private PguiAECtrl _tacticsSkillAE;

		private PguiReplaceSpriteCtrl _iconTacticsSkill;

		private GameObject _deckAll;

		private List<GameObject> _chara;

		private List<Transform> _touchChara;

		private List<Transform> _touchWait;

		private List<PguiAECtrl> _charaSel;

		private List<AEImage> _effArtsOk;

		private List<AEImage> _effArtsOkDbl;

		private List<Transform> _touchArts;

		private AEImage _deckAllAE;

		private GameObject _actInfoAll;

		private GameObject _actGageBase;

		private PguiImageCtrl _actGageGage;

		private PguiImageCtrl _actGageGageAdd;

		private PguiImageCtrl _actGageGageAdd2;

		private GameObject _actGageGageFlash;

		private PguiTextCtrl _numActBefore;

		private PguiTextCtrl _numActAfter;

		private Transform _touchActGage;

		private CanvasGroup _actInfo;

		private PguiTextCtrl _numUseBefore;

		private PguiTextCtrl _numUseAfter;

		private AEImage _actGageAE;

		private PguiTextCtrl _actGageExtra;

		private AEImage _actInfoBackAE;

		private AEImage _actInfoFrontAE;

		private GameObject _actGageBan;

		private GameObject _timeAll;

		private GameObject _menuAll;

		private PguiToggleButtonCtrl _btnAuto;

		private GameObject _btnAutoLock;

		private PguiTextCtrl _btnAutoTxt;

		private SimpleAnimation _animAutoInfo;

		private GameObject _btnAutoBan;

		private GameObject _btnAutoLamp;

		private PguiToggleButtonCtrl _btnFast;

		private PguiButtonCtrl _btnMenu;

		private PguiTextCtrl _numYaseiTotal;

		private GameObject _plasmShare;

		private PguiAECtrl _plasmShareCmnAE;

		private List<AEImage> _plasmShareAE;

		private PguiTextCtrl _numWave;

		private PguiTextCtrl _numTurn;

		private GameObject _selectCard;

		private SimpleAnimation _animSelectCard;

		private GameObject _actionCard;

		private SimpleAnimation _animActionCard;

		private GameObject _animTurn;

		private AEImage _animTurnAE;

		private PguiReplaceSpriteCtrl _animNumTurn1;

		private PguiReplaceSpriteCtrl _animNumTurn2;

		private GameObject _animTurnAct;

		private AEImage _animTurnActAE;

		private PguiTextCtrl _animNumTurnAct;

		private GameObject _animWave;

		private AEImage _animWaveAE;

		private PguiReplaceSpriteCtrl _animNumWaveC1;

		private PguiReplaceSpriteCtrl _animNumWaveC2;

		private PguiReplaceSpriteCtrl _animNumWaveM1;

		private PguiReplaceSpriteCtrl _animNumWaveM2;

		private SimpleAnimation _infoPlayerSkill;

		private PguiTextCtrl _numPlayerSkill;

		private PguiButtonCtrl _btnInfoSkill;

		private GameObject _animPlayerSkill;

		private AEImage _animPlayerSkillAE;

		private AEImage _rareInAE;

		private AEImage _bossInAE;

		private AEImage _battleStartAE;

		private AEImage _resultAE;

		private PguiAECtrl _kemonoArtsAE;

		private PguiAECtrl _kemonoFadeAE;

		private PguiAECtrl _chainFrontAE;

		private PguiAECtrl _chainBackAE;

		private PguiAECtrl _chainCountAE;

		private PguiAECtrl _beatActAE;

		private PguiAECtrl _actionActAE;

		private PguiAECtrl _specialActAE;

		private PguiAECtrl _tryActAE;

		private float _tryActMove;

		private PguiAECtrl _tryActPlasmAE;

		private List<AEImage> _staySkill;

		private List<AEImage> _scheduledSkillListAE;

		private List<AEImage> _enemyScheduledSkillListAE;

		private Transform _hpGage;

		private Transform _numDamage;

		private Transform _orderCard;

		private PguiAECtrl _pvpChainCountAE;

		private PguiAECtrl _pvpBeatActAE;

		private PguiAECtrl _pvpActionActAE;

		private PguiAECtrl _pvpTryActAE;

		private PguiAECtrl _pvpSpecialActAE;

		private List<AEImage> _pvpStaySkill;

		private SimpleAnimation _pvpCoinGet;

		private SimpleAnimation _artsInfo;

		private SimpleAnimation _statInfo;

		private HorizontalLayoutGroup _trainingScoreL;

		private HorizontalLayoutGroup _trainingScoreR;

		private AEImage _trainingMissionAE1;

		private AEImage _trainingMissionAE2;

		private Transform _popUpInfoCard;

		private Transform _popUpInfoAction;

		private PguiTextCtrl _cardInfo;

		private PguiTextCtrl _cardInfoV;

		private PguiTextCtrl _pvpTrainingScore;

		private PguiTextCtrl _pvpTrainingScoreV;

		private PguiAECtrl _hugeFadeAE;

		private PguiAECtrl _captainCautionAE;

		private AEImage _tacticsPlasmScoreAE;

		private AEImage _tacticsAttackAE;

		private AEImage _tacticsAE;

		private AEImage _tacticsPlayerAE;

		private AEImage _tacticsVersusAE;

		private AEImage _tacticsEndScoreAE;

		private PguiAECtrl _tickleStartAE;

		private PguiAECtrl _tickleResultAE;

		private GameObject _practiceSign;
	}
}
