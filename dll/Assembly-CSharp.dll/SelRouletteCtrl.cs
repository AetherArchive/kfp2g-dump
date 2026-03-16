using System;
using System.Collections;
using System.Collections.Generic;
using CriWare;
using SGNFW.Common;
using UnityEngine;

public class SelRouletteCtrl : MonoBehaviour
{
	private void Start()
	{
		Singleton<DataManager>.Instance.InitializeByEditor(null);
	}

	private void Update()
	{
	}

	public static IEnumerator ExeRoulette(Transform parentTr)
	{
		SelLoginBonus.rcvDate = TimeManager.Now;
		SelRouletteCtrl.allSkip = false;
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		if (homeCheckResult == null || homeCheckResult.rouletteData == null || string.IsNullOrEmpty(homeCheckResult.rouletteData.actionId))
		{
			yield break;
		}
		IEnumerator roulette = SelRouletteCtrl.ExeRoulettePerformance(parentTr, homeCheckResult.rouletteData);
		while (roulette.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private static IEnumerator ExeRoulettePerformance(Transform parentTr, HomeCheckResult.RouletteData rouletteData)
	{
		SelRouletteCtrl.<>c__DisplayClass15_0 CS$<>8__locals1 = new SelRouletteCtrl.<>c__DisplayClass15_0();
		SelRouletteCtrl.guiData = null;
		CS$<>8__locals1.isRequestSkip = false;
		if (SelRouletteCtrl.guiData == null)
		{
			SelRouletteCtrl.guiData = new SelRouletteCtrl.GuiRoulette(AssetManager.InstantiateAssetData("SceneLoginBonus/GUI/Prefab/GUI_Roulette", parentTr).transform);
			SelRouletteCtrl.guiData.baseObj.transform.SetAsFirstSibling();
			SelRouletteCtrl.guiData.Btn_Skip.AddOnClickListener(delegate(PguiButtonCtrl btn)
			{
				SelRouletteCtrl.allSkip = true;
			}, PguiButtonCtrl.SoundType.DEFAULT);
			SelRouletteCtrl.guiData.Btn_Skip.gameObject.SetActive(false);
			SelRouletteCtrl.guiData.cutInParent.SetActive(false);
			SelRouletteCtrl.guiData.FrontBG.gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
			{
				if (SelRouletteCtrl.guiData.Touch.activeSelf)
				{
					CS$<>8__locals1.isRequestSkip = true;
				}
			}, null, null, null, null);
			SelRouletteCtrl.guiData.FrontBG.SetRawImage(rouletteData.bgTexturePath, true, false, null);
			SelRouletteCtrl.guiData.Touch.SetActive(false);
		}
		SelRouletteCtrl.fadeGui = AssetManager.InstantiateAssetData("Cmn/GUI/Prefab/GUI_LeafFade", parentTr).GetComponent<PguiAECtrl>();
		GameObject uiPerformance = AssetManager.InstantiateAssetData("SceneLoginBonus/GUI/Prefab/RouletteResultPerformance_" + rouletteData.actionId.ToString(), SelRouletteCtrl.guiData.performanceParent.transform);
		if (uiPerformance != null)
		{
			uiPerformance.SetActive(false);
		}
		SelRouletteCtrl.guiData.CharSerif.gameObject.SetActive(false);
		int charaId = rouletteData.assistantCharaId;
		CS$<>8__locals1.renderTextureChara = AssetManager.InstantiateAssetData("RenderTextureChara/Prefab/RenderTextureCharaCtrl", SelRouletteCtrl.guiData.baseObj.transform.Find("RenderChara").transform).GetComponent<RenderTextureChara>();
		CS$<>8__locals1.renderTextureChara.postion = new Vector2(-2480f, -150f);
		CS$<>8__locals1.renderTextureChara.fieldOfView = 15f;
		CS$<>8__locals1.renderTextureChara.Setup(charaId, 1, CharaMotionDefine.ActKey.SCENARIO_STAND_BY, 0, false, true, null, false, null, 0f, null, false, false, false);
		CS$<>8__locals1.loadEnd = false;
		CS$<>8__locals1.renderTextureRoulette = AssetManager.InstantiateAssetData("RenderTextureChara/Prefab/RenderTextureCharaCtrl", SelRouletteCtrl.guiData.baseObj.transform.Find("RenderChara").transform).GetComponent<RenderTextureChara>();
		CS$<>8__locals1.renderTextureRoulette.postion = new Vector2(170f, 0f);
		CS$<>8__locals1.renderTextureRoulette.fieldOfView = 15f;
		CS$<>8__locals1.renderTextureRoulette.SetCameraPosition(new Vector3(0f, 1.05f, -10f));
		int num = ((rouletteData.rouletteModelId <= 0) ? SelRouletteCtrl.DEFAULT_ROULETTE_MODEL_ID : rouletteData.rouletteModelId);
		CS$<>8__locals1.renderTextureRoulette.Setup(num, 2, CharaMotionDefine.ActKey.INVALID, 0, false, true, delegate
		{
			CS$<>8__locals1.loadEnd = true;
		}, false, null, 0f, null, false, false, false);
		if (rouletteData.performanceId.Contains(SelRouletteCtrl.PERFORMANCE_LIST.RESTART.ToString()))
		{
			SelRouletteCtrl.guiData.cutInBG.SetRawImage(AssetManager.IsExsistAssetData(rouletteData.texturePath) ? rouletteData.texturePath : SelRouletteCtrl.DEFAULT_CUTIN_TEX_PATH, true, false, null);
		}
		while (!CS$<>8__locals1.loadEnd)
		{
			yield return null;
		}
		SelRouletteCtrl.<>c__DisplayClass15_1 CS$<>8__locals2 = new SelRouletteCtrl.<>c__DisplayClass15_1();
		CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
		CS$<>8__locals2.animEnd = false;
		SoundManager.Play("prd_se_login_bonus_leaf", false, false);
		SelRouletteCtrl.fadeGui.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
		{
			CS$<>8__locals2.CS$<>8__locals1.renderTextureRoulette.SetCameraPosition(new Vector3(0f, 1.05f, 10f));
			PguiAECtrl pguiAECtrl = SelRouletteCtrl.fadeGui;
			PguiAECtrl.AmimeType amimeType = PguiAECtrl.AmimeType.END;
			PguiAECtrl.FinishCallback finishCallback;
			if ((finishCallback = CS$<>8__locals2.<>9__7) == null)
			{
				finishCallback = (CS$<>8__locals2.<>9__7 = delegate
				{
					CS$<>8__locals2.animEnd = true;
				});
			}
			pguiAECtrl.PlayAnimation(amimeType, finishCallback);
		});
		while (!CS$<>8__locals2.animEnd)
		{
			yield return null;
		}
		CS$<>8__locals2 = null;
		CS$<>8__locals1.charaAnimEnd = false;
		CS$<>8__locals1.renderTextureChara.postion = new Vector2(-390f, -150f);
		CS$<>8__locals1.renderTextureChara.OnValidate();
		CS$<>8__locals1.charaAnimEnd = false;
		SoundManager.Play("prd_se_roulette_charain", false, false);
		CS$<>8__locals1.renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.LOGIN_BONUS_ENTRY, false, 0f, delegate
		{
			CS$<>8__locals1.charaAnimEnd = true;
			CS$<>8__locals1.renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
		});
		float timeout = 0f;
		SelRouletteCtrl.guiData.Touch.SetActive(true);
		while (!CS$<>8__locals1.charaAnimEnd && timeout < 5f)
		{
			timeout += Time.deltaTime;
			yield return null;
		}
		CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(charaId);
		SelRouletteCtrl.guiData.Txt_CharaName.text = charaStaticData.GetName();
		SelRouletteCtrl.guiData.Txt_Serif.text = rouletteData.startText;
		SelRouletteCtrl.guiData.CharSerif.gameObject.SetActive(!string.IsNullOrEmpty(rouletteData.startText));
		CS$<>8__locals1.isRequestSkip = false;
		while (!CS$<>8__locals1.isRequestSkip)
		{
			yield return null;
		}
		SelRouletteCtrl.guiData.Touch.SetActive(false);
		SelRouletteCtrl.guiData.Btn_Skip.gameObject.SetActive(true);
		CS$<>8__locals1.charaAnimEnd = false;
		List<string> list = new List<string>();
		string motion_key = "";
		List<string> restartKeypaier = (restartKeypaier = SelRouletteCtrl.RESTART_KEYPAIR[0]);
		if (rouletteData.performanceId.Contains(SelRouletteCtrl.PERFORMANCE_LIST.RESTART.ToString()))
		{
			restartKeypaier = SelRouletteCtrl.RESTART_KEYPAIR[Random.Range(0, SelRouletteCtrl.RESTART_KEYPAIR.Count)];
			motion_key = restartKeypaier[0];
		}
		else
		{
			SelRouletteCtrl.ACTION_ID_TO_MOTION_KEY.TryGetValue(rouletteData.actionId, out list);
			motion_key = list[Random.Range(0, list.Count)];
		}
		if (!string.IsNullOrEmpty(motion_key))
		{
			CriAtomExPlayback roulettePlaySe = SoundManager.Play("prd_se_roulette_startstop", false, false);
			CS$<>8__locals1.renderTextureRoulette.SetAnimation(SelRouletteCtrl.ROULETTE_MOTION_KEY + motion_key, false, 0f, delegate
			{
				CS$<>8__locals1.charaAnimEnd = true;
			});
			while (!CS$<>8__locals1.charaAnimEnd && !SelRouletteCtrl.allSkip)
			{
				yield return null;
			}
			if (SelRouletteCtrl.allSkip)
			{
				CS$<>8__locals1.renderTextureRoulette.SetAnimation(SelRouletteCtrl.ROULETTE_MOTION_KEY + motion_key, false, 100f, null);
			}
			roulettePlaySe.Stop();
			if (rouletteData.performanceId.Contains(SelRouletteCtrl.PERFORMANCE_LIST.RESTART.ToString()))
			{
				CS$<>8__locals1.charaAnimEnd = false;
				SelRouletteCtrl.guiData.cutInParent.SetActive(true);
				CriAtomExPlayback caep = SoundManager.Play("prd_se_roulette_cutin", false, false);
				float time = 0f;
				while (time < 0.2f && !SelRouletteCtrl.allSkip)
				{
					time += Time.deltaTime;
					yield return null;
				}
				roulettePlaySe = SoundManager.Play("prd_se_roulette_startstop2", false, false);
				motion_key = restartKeypaier[1];
				CS$<>8__locals1.renderTextureRoulette.SetAnimation(SelRouletteCtrl.ROULETTE_MOTION_KEY + motion_key, false, 0f, delegate
				{
					CS$<>8__locals1.charaAnimEnd = true;
				});
				time = 0f;
				while (time < 0.5f && !SelRouletteCtrl.allSkip)
				{
					time += Time.deltaTime;
					yield return null;
				}
				CS$<>8__locals1.renderTextureChara.SetFacePack(FacePackData.Id2PackData("FACE_WHAT_1_D"));
				CS$<>8__locals1.renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SURPRISE, false, 0f, delegate
				{
					CS$<>8__locals1.renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
				});
				while (!CS$<>8__locals1.charaAnimEnd && !SelRouletteCtrl.allSkip)
				{
					yield return null;
				}
				if (SelRouletteCtrl.allSkip)
				{
					CS$<>8__locals1.renderTextureRoulette.SetAnimation(SelRouletteCtrl.ROULETTE_MOTION_KEY + motion_key, false, 100f, null);
					caep.Stop();
					roulettePlaySe.Stop();
				}
				caep = default(CriAtomExPlayback);
			}
			SelRouletteCtrl.guiData.Btn_Skip.gameObject.SetActive(false);
			SoundManager.Play((rouletteData.actionId == SelRouletteCtrl.LUCKY_ACTION_ID) ? "prd_se_roulette_result_02" : "prd_se_roulette_result_01", false, false);
			if (uiPerformance != null)
			{
				uiPerformance.SetActive(true);
			}
			SelRouletteCtrl.guiData.cutInParent.SetActive(false);
			CS$<>8__locals1.charaAnimEnd = false;
			SelRouletteCtrl.guiData.Txt_Serif.text = rouletteData.endText;
			SelRouletteCtrl.guiData.CharSerif.gameObject.SetActive(!string.IsNullOrEmpty(rouletteData.endText));
			CS$<>8__locals1.renderTextureChara.SetFacePack(FacePackData.Id2PackData("FACE_GOOD_4_B"));
			CS$<>8__locals1.renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.JOY, false, 0f, delegate
			{
				CS$<>8__locals1.charaAnimEnd = true;
				SelRouletteCtrl.guiData.Touch.SetActive(true);
				CS$<>8__locals1.renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
			});
			CS$<>8__locals1.isRequestSkip = false;
			while (!CS$<>8__locals1.isRequestSkip || !CS$<>8__locals1.charaAnimEnd)
			{
				yield return null;
			}
			SelRouletteCtrl.Uninit();
			yield break;
		}
		SelRouletteCtrl.Uninit();
		yield break;
	}

	private static void Uninit()
	{
		if (SelRouletteCtrl.guiData != null)
		{
			SelRouletteCtrl.guiData.Touch.SetActive(false);
			Object.Destroy(SelRouletteCtrl.guiData.baseObj);
		}
		if (SelRouletteCtrl.fadeGui != null)
		{
			Object.Destroy(SelRouletteCtrl.fadeGui.gameObject);
		}
	}

	private static bool allSkip;

	private static SelRouletteCtrl.GuiRoulette guiData;

	private static PguiAECtrl fadeGui;

	public static DateTime rcvDate = DateTime.MinValue;

	private static readonly string ROULETTE_MOTION_KEY = "RLT_CH_40401_PTN_";

	private static readonly string DEFAULT_CUTIN_TEX_PATH = "Texture2D/Roulette/cutin_bg_default";

	private static readonly Dictionary<string, List<string>> ACTION_ID_TO_MOTION_KEY = new Dictionary<string, List<string>>
	{
		{
			"1",
			new List<string> { "A", "B", "C", "D" }
		},
		{
			"2",
			new List<string> { "E", "F", "G" }
		},
		{
			"3",
			new List<string> { "H", "I" }
		},
		{
			"4",
			new List<string> { "J" }
		}
	};

	private static readonly List<List<string>> RESTART_KEYPAIR = new List<List<string>>
	{
		new List<string> { "B", "K" },
		new List<string> { "C", "L" }
	};

	private static readonly string LUCKY_ACTION_ID = "4";

	private static readonly int DEFAULT_ROULETTE_MODEL_ID = 40401;

	public class GuiRoulette
	{
		public GuiRoulette(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Skip = baseTr.Find("Btn_Skip").GetComponent<PguiButtonCtrl>();
			this.FrontBG = baseTr.Find("FrontBG").GetComponent<PguiRawImageCtrl>();
			this.Touch = baseTr.Find("Txt_Touch").gameObject;
			this.performanceParent = baseTr.Find("PerformanceParent").gameObject;
			this.CharSerif = baseTr.Find("CharSerif").GetComponent<PguiImageCtrl>();
			this.Txt_Serif = baseTr.Find("CharSerif/Txt_Serif").GetComponent<PguiTextCtrl>();
			this.Txt_CharaName = baseTr.Find("CharSerif/NameBase/Txt_CharaName").GetComponent<PguiTextCtrl>();
			this.cutInParent = baseTr.Find("CutIn").gameObject;
			this.cutInBG = baseTr.Find("CutIn/CutInMask/CutInBg").GetComponent<PguiRawImageCtrl>();
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_Skip;

		public PguiRawImageCtrl FrontBG;

		public PguiRawImageCtrl cutInBG;

		public GameObject Touch;

		public GameObject performanceParent;

		public PguiImageCtrl CharSerif;

		public PguiTextCtrl Txt_Serif;

		public PguiTextCtrl Txt_CharaName;

		public GameObject cutInParent;
	}

	private enum PERFORMANCE_LIST
	{
		RESTART
	}
}
