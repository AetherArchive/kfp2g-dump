using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Ab;
using SGNFW.Common;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class AuthPlayer : MonoBehaviour
{
	public static AuthPlayer InstantiateAuthPlayer(Transform parent = null, bool viewer = false)
	{
		AuthPlayer component = AssetManager.InstantiateAssetData("Framework/AuthPlayer", parent).GetComponent<AuthPlayer>();
		component.viewer = viewer;
		return component;
	}

	public AuthCamera AuthCameraObj { get; set; }

	public AuthCamera AuthCameraObjSub { get; set; }

	public bool viewer { get; set; }

	public string authName
	{
		get
		{
			if (!(this.auth == null))
			{
				return this.auth.name;
			}
			return "";
		}
	}

	public AuthPlayer.PostEffectMotionCtrl isPostEffectMotionCtrl
	{
		get
		{
			return new AuthPlayer.PostEffectMotionCtrl
			{
				techColor = (this.technicolorObj != null),
				hueValue = (this.hueValueObj != null),
				radBlur = (this.radBlurObj != null),
				wiggle = (this.wiggleObj != null),
				rgbSprit = (this.rgbSpritObj != null),
				contrastVignette = (this.contrastVignetteObj != null || this.contrastVignetteRgbObj != null || this.contrastVignetteEdgeObj != null),
				doubleVision = (this.doubleVisionObj != null),
				negative = (this.negativeObj != null),
				analogTV = (this.analogTVObj != null),
				focusLine = (this.focusLineObj != null),
				speedLine = (this.speedLineObj != null),
				blur = (this.blurObj != null),
				fog = (this.fogObj != null),
				camouflage = (this.camouflageObj != null)
			};
		}
	}

	public CC_Technicolor techColor { get; private set; }

	public CC_HueSaturationValue hueValue { get; private set; }

	public CC_RadialBlur radBlur { get; private set; }

	public CC_Wiggle wiggle { get; private set; }

	public CC_RGBSplit rgbSprit { get; private set; }

	public CC_ContrastVignette contrastVignette { get; private set; }

	public CC_DoubleVision doubleVision { get; private set; }

	public CC_Negative negative { get; private set; }

	public CC_AnalogTV analogTV { get; private set; }

	public FocusLine focusLine { get; private set; }

	public SpeedLine speedLine { get; private set; }

	public Blur blur { get; private set; }

	private Coroutine InitializeCoroutine { get; set; }

	public void InitializeByGacha(AuthPlayer.GachaParam.Before param)
	{
		this.isFinishDestroy = false;
		this.camouflage = false;
		List<AnimationEvent> list = new List<AnimationEvent>();
		if (param.effectType != AuthPlayer.GachaParam.EffectType.BLUE)
		{
			list.Add(new AnimationEvent
			{
				functionName = "AuthEffectPlay",
				stringParameter = "Ef_auth_gacha_a_normal"
			});
			list.Add(new AnimationEvent
			{
				functionName = "SoundPlay",
				stringParameter = "prd_se_at_gca_po_0001_normal"
			});
		}
		if (param.effectType != AuthPlayer.GachaParam.EffectType.GOLD)
		{
			list.Add(new AnimationEvent
			{
				functionName = "AuthEffectPlay",
				stringParameter = "Ef_auth_gacha_a_rare"
			});
			list.Add(new AnimationEvent
			{
				functionName = "SoundPlay",
				stringParameter = "prd_se_at_gca_po_0001_rare"
			});
		}
		if (param.effectType != AuthPlayer.GachaParam.EffectType.RAINBOW)
		{
			list.Add(new AnimationEvent
			{
				functionName = "AuthEffectPlay",
				stringParameter = "Ef_auth_gacha_a_superrare"
			});
			list.Add(new AnimationEvent
			{
				functionName = "SoundPlay",
				stringParameter = "prd_se_at_gca_po_0001_superrare"
			});
		}
		if (param.skyType != AuthPlayer.GachaParam.SkyType.NIGHT_STAR)
		{
			list.Add(new AnimationEvent
			{
				functionName = "AuthEffectPlay",
				stringParameter = "Ef_auth_gacha_a_shootingstar"
			});
			list.Add(new AnimationEvent
			{
				functionName = "SoundPlay",
				stringParameter = "prd_se_at_gca_po_0001_shootingstar"
			});
		}
		this.AddEffectsBeforeVrsion(param, list);
		if (param.postActType != AuthPlayer.GachaParam.PostActType.NORMAL)
		{
			list.Add(new AnimationEvent
			{
				functionName = "AuthMotPlay",
				stringParameter = "CHARA_1_A__KEYTYPE_GCA_CH_40001_POST_A"
			});
		}
		if (param.postActType != AuthPlayer.GachaParam.PostActType.JUMP)
		{
			list.Add(new AnimationEvent
			{
				functionName = "AuthMotPlay",
				stringParameter = "CHARA_1_A__KEYTYPE_GCA_CH_40001_POST_B"
			});
		}
		this.InitializeByParam("AT@AT_GCA_PO_0001_", null, param.stageData, null, null, false, false, false, list);
	}

	public void InitializeByGacha(AuthPlayer.GachaParam.After param)
	{
		this.isFinishDestroy = false;
		this.camouflage = false;
		ItemDef.Kind kind = ItemDef.Id2Kind(param.itemId);
		List<AnimationEvent> list = new List<AnimationEvent>();
		if (kind != ItemDef.Kind.CHARA)
		{
			if (kind == ItemDef.Kind.PHOTO)
			{
				if (param.effectType != AuthPlayer.GachaParam.EffectType.BLUE)
				{
					list.Add(new AnimationEvent
					{
						functionName = "AuthEffectPlay",
						stringParameter = "Ef_auth_gacha_b_normal"
					});
					list.Add(new AnimationEvent
					{
						functionName = "SoundPlay",
						stringParameter = "prd_se_at_gca_ft_0001_normal"
					});
				}
				if (param.effectType != AuthPlayer.GachaParam.EffectType.GOLD)
				{
					list.Add(new AnimationEvent
					{
						functionName = "AuthEffectPlay",
						stringParameter = "Ef_auth_gacha_b_rare"
					});
					list.Add(new AnimationEvent
					{
						functionName = "SoundPlay",
						stringParameter = "prd_se_at_gca_ft_0001_rare"
					});
				}
				if (param.effectType != AuthPlayer.GachaParam.EffectType.RAINBOW)
				{
					list.Add(new AnimationEvent
					{
						functionName = "AuthEffectPlay",
						stringParameter = "Ef_auth_gacha_b_superrare"
					});
					list.Add(new AnimationEvent
					{
						functionName = "SoundPlay",
						stringParameter = "prd_se_at_gca_ft_0001_superrare"
					});
				}
				this.AddEffectsAfterVrsion(param, list);
				this.InitializeByParam("AT@AT_GCA_FT_0001_", null, param.stageData, null, null, false, false, false, list);
				return;
			}
			if (kind != ItemDef.Kind.TREEHOUSE_FURNITURE)
			{
				return;
			}
			if (param.effectType != AuthPlayer.GachaParam.EffectType.BLUE)
			{
				list.Add(new AnimationEvent
				{
					functionName = "AuthEffectPlay",
					stringParameter = "Ef_auth_interior_gacha_b_normal"
				});
				list.Add(new AnimationEvent
				{
					functionName = "SoundPlay",
					stringParameter = "prd_se_at_gca_fk_2001_normal"
				});
			}
			if (param.effectType != AuthPlayer.GachaParam.EffectType.GOLD)
			{
				list.Add(new AnimationEvent
				{
					functionName = "AuthEffectPlay",
					stringParameter = "Ef_auth_interior_gacha_b_rare"
				});
				list.Add(new AnimationEvent
				{
					functionName = "SoundPlay",
					stringParameter = "prd_se_at_gca_fk_2001_rare"
				});
			}
			if (param.effectType != AuthPlayer.GachaParam.EffectType.RAINBOW)
			{
				list.Add(new AnimationEvent
				{
					functionName = "AuthEffectPlay",
					stringParameter = "Ef_auth_interior_gacha_b_superrare"
				});
				list.Add(new AnimationEvent
				{
					functionName = "SoundPlay",
					stringParameter = "prd_se_at_gca_fk_2001_superrare"
				});
			}
			this.AddEffectsAfterVrsion(param, list);
			this.InitializeByParam("AT@AT_GCA_FK_2001_", null, param.stageData, null, null, false, false, false, list);
			return;
		}
		else
		{
			CharaModelHandle.InitializeParam initializeParam = CharaModelHandle.InitializeParam.CreaateByCharaId(param.itemId, 0, false, false);
			if (param.isPromotion)
			{
				if (param.effectType != AuthPlayer.GachaParam.EffectType.BLUE)
				{
					list.Add(new AnimationEvent
					{
						functionName = "AuthEffectPlay",
						stringParameter = "Ef_auth_gacha_b_promotion_rare"
					});
					list.Add(new AnimationEvent
					{
						functionName = "SoundPlay",
						stringParameter = "prd_se_at_gca_ch_0002_normal"
					});
				}
				if (param.effectType != AuthPlayer.GachaParam.EffectType.GOLD)
				{
					list.Add(new AnimationEvent
					{
						functionName = "AuthEffectPlay",
						stringParameter = "Ef_auth_gacha_b_promotion_superrare"
					});
					list.Add(new AnimationEvent
					{
						functionName = "SoundPlay",
						stringParameter = "prd_se_at_gca_ch_0002_rare"
					});
				}
				if (param.effectType != AuthPlayer.GachaParam.EffectType.RAINBOW)
				{
					list.Add(new AnimationEvent
					{
						functionName = "SoundPlay",
						stringParameter = "prd_se_at_gca_ch_0002_superrare"
					});
				}
				this.AddEffectsAfterVrsion(param, list);
				this.InitializeByParam("AT@AT_GCA_CH_0002_", new List<CharaModelHandle.InitializeParam> { null, null, initializeParam }, param.stageData, null, null, false, false, false, list);
				return;
			}
			if (param.effectType != AuthPlayer.GachaParam.EffectType.BLUE)
			{
				list.Add(new AnimationEvent
				{
					functionName = "AuthEffectPlay",
					stringParameter = "Ef_auth_gacha_b_normal"
				});
				list.Add(new AnimationEvent
				{
					functionName = "SoundPlay",
					stringParameter = "prd_se_at_gca_ch_0001_normal"
				});
			}
			if (param.effectType != AuthPlayer.GachaParam.EffectType.GOLD)
			{
				list.Add(new AnimationEvent
				{
					functionName = "AuthEffectPlay",
					stringParameter = "Ef_auth_gacha_b_rare"
				});
				list.Add(new AnimationEvent
				{
					functionName = "SoundPlay",
					stringParameter = "prd_se_at_gca_ch_0001_rare"
				});
			}
			if (param.effectType != AuthPlayer.GachaParam.EffectType.RAINBOW)
			{
				list.Add(new AnimationEvent
				{
					functionName = "AuthEffectPlay",
					stringParameter = "Ef_auth_gacha_b_superrare"
				});
				list.Add(new AnimationEvent
				{
					functionName = "SoundPlay",
					stringParameter = "prd_se_at_gca_ch_0001_superrare"
				});
			}
			this.AddEffectsAfterVrsion(param, list);
			this.InitializeByParam("AT@AT_GCA_CH_0001_", new List<CharaModelHandle.InitializeParam> { null, null, initializeParam }, param.stageData, null, null, false, false, false, list);
			return;
		}
	}

	public void InitializeByGacha2(AuthPlayer.GachaParam.After param)
	{
		this.isFinishDestroy = false;
		this.camouflage = false;
		int num = (int)ItemDef.Id2Kind(param.itemId);
		List<AnimationEvent> list = new List<AnimationEvent>();
		if (num == 1)
		{
			CharaModelHandle.InitializeParam initializeParam = CharaModelHandle.InitializeParam.CreaateByCharaId(param.itemId, 0, false, false);
			if (param.isPromotion)
			{
				if (param.effectType != AuthPlayer.GachaParam.EffectType.BLUE)
				{
					list.Add(new AnimationEvent
					{
						functionName = "AuthEffectPlay",
						stringParameter = "ef_auth_gacha_b_promotion_rare"
					});
					list.Add(new AnimationEvent
					{
						functionName = "SoundPlay",
						stringParameter = "prd_se_at_gca_ch_1002_normal"
					});
				}
				if (param.effectType != AuthPlayer.GachaParam.EffectType.GOLD)
				{
					list.Add(new AnimationEvent
					{
						functionName = "AuthEffectPlay",
						stringParameter = "ef_auth_gacha_b_promotion_superrare"
					});
					list.Add(new AnimationEvent
					{
						functionName = "SoundPlay",
						stringParameter = "prd_se_at_gca_ch_1002_rare"
					});
				}
				if (param.effectType != AuthPlayer.GachaParam.EffectType.RAINBOW)
				{
					list.Add(new AnimationEvent
					{
						functionName = "SoundPlay",
						stringParameter = "prd_se_at_gca_ch_1002_superrare"
					});
				}
				this.AddEffectsAfterVrsion(param, list);
				this.InitializeByParam("AT@AT_GCA_CH_1002_", new List<CharaModelHandle.InitializeParam> { null, null, initializeParam }, param.stageData, null, null, false, false, false, list);
				return;
			}
			if (param.effectType != AuthPlayer.GachaParam.EffectType.BLUE)
			{
				list.Add(new AnimationEvent
				{
					functionName = "AuthEffectPlay",
					stringParameter = "Ef_auth_gacha_b_normal"
				});
				list.Add(new AnimationEvent
				{
					functionName = "SoundPlay",
					stringParameter = "prd_se_at_gca_ch_1001_normal"
				});
			}
			if (param.effectType != AuthPlayer.GachaParam.EffectType.GOLD)
			{
				list.Add(new AnimationEvent
				{
					functionName = "AuthEffectPlay",
					stringParameter = "Ef_auth_gacha_b_rare"
				});
				list.Add(new AnimationEvent
				{
					functionName = "SoundPlay",
					stringParameter = "prd_se_at_gca_ch_1001_rare"
				});
			}
			if (param.effectType != AuthPlayer.GachaParam.EffectType.RAINBOW)
			{
				list.Add(new AnimationEvent
				{
					functionName = "AuthEffectPlay",
					stringParameter = "Ef_auth_gacha_b_superrare"
				});
				list.Add(new AnimationEvent
				{
					functionName = "SoundPlay",
					stringParameter = "prd_se_at_gca_ch_1001_superrare"
				});
			}
			this.AddEffectsAfterVrsion(param, list);
			this.InitializeByParam("AT@AT_GCA_CH_1001_", new List<CharaModelHandle.InitializeParam> { null, null, initializeParam }, param.stageData, null, null, false, false, false, list);
		}
	}

	private void AddEffectsBeforeVrsion(AuthPlayer.GachaParam.Before param, List<AnimationEvent> removeEventList)
	{
		AuthPlayer.GachaAnimationEventsBefore gachaAnimationEventsBefore = new AuthPlayer.GachaAnimationEventsBefore();
		Dictionary<AuthPlayer.GachaParam.PutType, List<ValueTuple<string, string>>> dictionary = new Dictionary<AuthPlayer.GachaParam.PutType, List<ValueTuple<string, string>>>
		{
			{
				AuthPlayer.GachaParam.PutType.MIRAI,
				gachaAnimationEventsBefore.miraiEventBefore
			},
			{
				AuthPlayer.GachaParam.PutType.FULL_MEMBERS,
				gachaAnimationEventsBefore.fullMembersEventsBefore
			},
			{
				AuthPlayer.GachaParam.PutType.NORMAL,
				gachaAnimationEventsBefore.normalEventsBefore
			},
			{
				AuthPlayer.GachaParam.PutType.LUCKY_BEAST,
				gachaAnimationEventsBefore.luckyBeastEventsBefore
			},
			{
				AuthPlayer.GachaParam.PutType.MIRAI_NANA,
				gachaAnimationEventsBefore.miraiNanaEventsBefore
			},
			{
				AuthPlayer.GachaParam.PutType.MIRAI_CARRENDER,
				gachaAnimationEventsBefore.miraiCarrenderEventsBefore
			},
			{
				AuthPlayer.GachaParam.PutType.MIRAI_NANA_CARRENDER,
				gachaAnimationEventsBefore.miraiNanaCarrenderEventsBefore
			},
			{
				AuthPlayer.GachaParam.PutType.MIRAI_KAKO,
				gachaAnimationEventsBefore.miraiKakoEventsBefore
			},
			{
				AuthPlayer.GachaParam.PutType.MIRAI_KAKO_NANA,
				gachaAnimationEventsBefore.miraiKakoNanaEventsBefore
			},
			{
				AuthPlayer.GachaParam.PutType.MIRAI_KAKO_CARRENDER,
				gachaAnimationEventsBefore.miraiKakoCarrenderEventsBefore
			}
		};
		if (dictionary.ContainsKey(param.putType))
		{
			foreach (ValueTuple<string, string> valueTuple in dictionary[param.putType])
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				removeEventList.Add(new AnimationEvent
				{
					functionName = item,
					stringParameter = item2
				});
			}
		}
	}

	private void AddEffectsAfterVrsion(AuthPlayer.GachaParam.After param, List<AnimationEvent> removeEventList)
	{
		AuthPlayer.GachaAnimationEventsAfter gachaAnimationEventsAfter = new AuthPlayer.GachaAnimationEventsAfter();
		Dictionary<AuthPlayer.GachaParam.PutType, List<string>> dictionary = new Dictionary<AuthPlayer.GachaParam.PutType, List<string>>
		{
			{
				AuthPlayer.GachaParam.PutType.MIRAI,
				gachaAnimationEventsAfter.miraiEventsAfter
			},
			{
				AuthPlayer.GachaParam.PutType.FULL_MEMBERS,
				gachaAnimationEventsAfter.fullMembersEventsAfter
			},
			{
				AuthPlayer.GachaParam.PutType.NORMAL,
				gachaAnimationEventsAfter.normalEventsAfter
			},
			{
				AuthPlayer.GachaParam.PutType.LUCKY_BEAST,
				gachaAnimationEventsAfter.luckyBeastEventsAfter
			},
			{
				AuthPlayer.GachaParam.PutType.MIRAI_NANA,
				gachaAnimationEventsAfter.miraiNanaEventsAfter
			},
			{
				AuthPlayer.GachaParam.PutType.MIRAI_CARRENDER,
				gachaAnimationEventsAfter.miraiCarrenderEventsAfter
			},
			{
				AuthPlayer.GachaParam.PutType.MIRAI_NANA_CARRENDER,
				gachaAnimationEventsAfter.miraiNanaCarrenderEventsAfter
			},
			{
				AuthPlayer.GachaParam.PutType.MIRAI_KAKO,
				gachaAnimationEventsAfter.miraiKakoEventsAfter
			},
			{
				AuthPlayer.GachaParam.PutType.MIRAI_KAKO_NANA,
				gachaAnimationEventsAfter.miraiKakoNanaEventsAfter
			},
			{
				AuthPlayer.GachaParam.PutType.MIRAI_KAKO_CARRENDER,
				gachaAnimationEventsAfter.miraiKakoCarrenderEventsAfter
			}
		};
		if (dictionary.ContainsKey(param.putType))
		{
			foreach (string text in dictionary[param.putType])
			{
				removeEventList.Add(new AnimationEvent
				{
					functionName = "AuthEffectPlay",
					stringParameter = text
				});
			}
		}
	}

	public void Initialize(string authName, List<CharaModelHandle.InitializeParam> charaModelParam = null, StagePresetCtrl stageData = null, List<AuthCharaData> charaList = null, string replaceChara = null, bool isArtsMaxAction = false, bool isQuickAuth = false, bool isModelShadow = false)
	{
		this.InitializeByParam(authName, charaModelParam, stageData, charaList, replaceChara, isArtsMaxAction, isQuickAuth, isModelShadow, null);
	}

	public void Initialize(string authName, List<CharaModelHandle> charaModelHandleList, StagePresetCtrl stageData)
	{
		List<CharaModelHandle.InitializeParam> list;
		if (charaModelHandleList == null)
		{
			list = null;
		}
		else
		{
			list = charaModelHandleList.ConvertAll<CharaModelHandle.InitializeParam>((CharaModelHandle item) => item.initializeParam);
		}
		this.InitializeByParam(authName, list, stageData, null, null, false, false, false, null);
	}

	public void InitializeByParam(string authName, List<CharaModelHandle.InitializeParam> charaInitializeParam = null, StagePresetCtrl stageData = null, List<AuthCharaData> charaList = null, string replaceChara = null, bool isArtsMaxAction = false, bool isQuickVoice = false, bool isModelShadow = false, List<AnimationEvent> removeEvent = null)
	{
		if (this.auth != null)
		{
			return;
		}
		this.isFinishInitialize = false;
		this.InitializeCoroutine = base.StartCoroutine(this.InitializeInternal(authName, charaInitializeParam, stageData, charaList, replaceChara, Vector3.zero, isArtsMaxAction, isQuickVoice, isModelShadow, removeEvent));
	}

	private IEnumerator InitializeInternal(string authName, List<CharaModelHandle.InitializeParam> charaInitializeParam, StagePresetCtrl stageData, List<AuthCharaData> charaList, string replaceChara, Vector3 offset, bool isArtsMaxAction, bool isQuickAuth, bool isModelShadow, List<AnimationEvent> removeEvent)
	{
		if (this.auth != null)
		{
			yield break;
		}
		this.isFinishInitialize = false;
		if (!this.viewer)
		{
			isModelShadow = false;
		}
		string authPath = "Auth/" + authName;
		AssetManager.LoadAssetData(authPath, AssetManager.OWNER.AuthPlayer, 0, null);
		while (!AssetManager.IsLoadFinishAssetData(authPath))
		{
			yield return null;
		}
		GameObject gameObject = AssetManager.InstantiateAssetData(authPath, base.transform);
		gameObject.name = authName;
		AuthConfigParam component = gameObject.GetComponent<AuthConfigParam>();
		this.modelChange = new Dictionary<string, string>();
		if (charaList == null)
		{
			charaList = new List<AuthCharaData>();
			if (component.targetCharaModelList.Count > 0 || component.targetCharaModel != string.Empty)
			{
				List<string> list = new List<string>(component.targetCharaModelList);
				if (list.Count == 0)
				{
					list.Add(component.targetCharaModel);
				}
				for (int j = 0; j < list.Count; j++)
				{
					int num = j + 1;
					AuthCharaData authCharaData = null;
					if (charaInitializeParam != null && list[j].StartsWith("ch_"))
					{
						CharaModelHandle.InitializeParam initializeParam = null;
						if (authName.StartsWith("AT@AT_ART_CH_"))
						{
							string cmn = list[j].Substring(0, 8);
							initializeParam = charaInitializeParam.Find((CharaModelHandle.InitializeParam itm) => itm != null && itm.bodyModelName.StartsWith("ch_") && itm.bodyModelName.Substring(0, 8) == cmn);
							if (initializeParam == null && authName.Substring(12, 5) == cmn.Substring(2, 5) && charaInitializeParam.Count > 0)
							{
								initializeParam = charaInitializeParam[0];
							}
						}
						else if (charaInitializeParam.Count > j)
						{
							initializeParam = charaInitializeParam[j];
						}
						if (initializeParam != null)
						{
							authCharaData = new AuthCharaData(num, initializeParam, isModelShadow);
						}
					}
					if (authCharaData == null)
					{
						string text = list[j];
						authCharaData = new AuthCharaData(num, text, isModelShadow);
					}
					authCharaData.Parent = new GameObject();
					authCharaData.Parent.name = "Chara_" + num.ToString() + "_A";
					authCharaData.Parent.transform.SetParent(base.transform, true);
					authCharaData.charaModelHandle.transform.SetParent(authCharaData.Parent.transform, false);
					authCharaData.charaModelHandle.transform.localScale = Vector3.one;
					authCharaData.charaModelHandle.SetAuthPlayer(this);
					charaList.Add(authCharaData);
				}
			}
		}
		gameObject.GetComponent<MotionEventHandler>().charaModelList = charaList;
		gameObject.GetComponent<MotionEventHandler>().modelChange = this.modelChange;
		this.stagePresetCtrl = stageData;
		this.stageDisableFrame = (component.disableStageDisp ? component.disableStageFrame : (-1));
		this.disableAuthFaceMotion = component.disableAuthFaceMotion;
		this.loadLightAssetNameList = new List<string>();
		this.auth = gameObject;
		this.charaList = charaList;
		this.offset = offset;
		this.time = -1f;
		this.pause = false;
		this.prePause = false;
		this.isPlayed = false;
		this.isActFinishProcess = false;
		this.effectList = new List<AuthPlayer.AuthEffectData>();
		this.transCacheList = new Dictionary<string, Transform>();
		this.efCtrlList = new HashSet<string>();
		this.charaCount = 0;
		this.authRenderSetting = this.auth.GetComponent<RenderSettingParam>();
		this.backupRenderSetting = base.gameObject.AddComponent<RenderSettingParam>();
		this.backupRenderSetting.Scene2Param();
		this.auth.GetComponent<EffectEventHandler>().authPlayer = this;
		this.AuthCameraObj = new AuthCamera(AssetManager.InstantiateAssetData("Framework/AuthCamera", base.transform).GetComponent<Camera>(), true);
		this.AuthCameraObjSub = new AuthCamera(AssetManager.InstantiateAssetData("Framework/AuthCamera", base.transform).GetComponent<Camera>(), false);
		this.CharaCamera = null;
		this.anim = this.auth.GetComponentInChildren<SimpleAnimation>();
		AnimationClip copy = Object.Instantiate<AnimationClip>(this.anim.clip);
		this.anim.AddClip(copy, "Custom");
		this.anim.ExStop(true);
		List<AnimationEvent> list2 = new List<AnimationEvent>(copy.events);
		if (!isArtsMaxAction)
		{
			list2.RemoveAll((AnimationEvent item) => item.functionName == "SoundPlay" && (item.stringParameter.EndsWith("_maxa") || item.stringParameter.EndsWith("_maxb")));
		}
		else
		{
			foreach (AnimationEvent animationEvent in list2.FindAll((AnimationEvent item) => item.functionName == "SoundPlay" && item.stringParameter.EndsWith("_maxb")))
			{
				string repStr = animationEvent.stringParameter.Replace("_maxb", "");
				list2.RemoveAll((AnimationEvent item) => item.functionName == "SoundPlay" && item.stringParameter == repStr);
			}
		}
		copy.events = list2.ToArray();
		if (isQuickAuth)
		{
			List<AnimationEvent> list3 = new List<AnimationEvent>(copy.events);
			foreach (AnimationEvent animationEvent2 in list3)
			{
				if (animationEvent2.functionName == "SoundPlay")
				{
					animationEvent2.stringParameter += "_quick";
				}
			}
			copy.events = list3.ToArray();
		}
		if (removeEvent != null)
		{
			List<AnimationEvent> list4 = new List<AnimationEvent>(copy.events);
			using (List<AnimationEvent>.Enumerator enumerator = removeEvent.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AnimationEvent ev = enumerator.Current;
					list4.RemoveAll((AnimationEvent item) => item.functionName == ev.functionName && item.stringParameter.StartsWith(ev.stringParameter));
				}
			}
			copy.events = list4.ToArray();
		}
		int k;
		for (int i = 0; i < this.charaList.Count; i = k + 1)
		{
			while (!this.charaList[i].charaModelHandle.IsFinishInitialize())
			{
				yield return null;
			}
			k = i;
		}
		for (int l = 0; l < charaList.Count; l++)
		{
			charaList[l].charaModelHandle.mouthFollowObj = this.AuthCameraObj.Trans;
		}
		if (this.auth.name.StartsWith("AT@AT_ART_"))
		{
			for (int m = 0; m < charaList.Count; m++)
			{
				charaList[m].charaModelHandle.SetScaleOne();
			}
		}
		List<int> list5 = new List<int>();
		foreach (AnimationEvent animationEvent3 in copy.events)
		{
			if (!(animationEvent3.functionName != "AuthMotPlay"))
			{
				int num2;
				float num3;
				float num4;
				float num5;
				bool flag;
				string text2 = MotionEventHandler.AnimeEventParse(animationEvent3, out num2, out num3, out num4, out num5, out flag);
				if (!list5.Contains(num2))
				{
					foreach (string text3 in this.modelChange.Keys)
					{
						if (text2.IndexOf(text3) > 0)
						{
							text2 = text2.Replace(text3, this.modelChange[text3]);
							break;
						}
					}
					list5.Add(num2);
					AuthCharaData authCharaData2 = charaList[num2];
					authCharaData2.start = num3;
					CharaModelHandle charaModelHandle = authCharaData2.charaModelHandle;
					charaModelHandle.SetModelActive(true);
					charaModelHandle.FadeOut(0f, null);
					charaModelHandle.PlayAnimationByAuth(text2, authCharaData2.start, 0f, false);
				}
			}
		}
		int num6 = 0;
		while (num6 < 2 && num6 < charaList.Count)
		{
			for (int n = 1; n <= 4; n++)
			{
				string text4 = AuthPlayer.GetOptionEffectName(this.auth.name, num6) + n.ToString();
				if (Singleton<AssetManager>.Instance != null && EffectManager.IsExsistEffect(text4))
				{
					GameObject gameObject2 = new GameObject();
					gameObject2.transform.SetParent(base.transform, true);
					gameObject2.name = "Auth_" + text4;
					AuthPlayer.AuthEffectData authEffectData = new AuthPlayer.AuthEffectData(charaList[num6].charaModelHandle.GetNodeTransform("opt" + n.ToString()), gameObject2, text4, true);
					this.effectList.Add(authEffectData);
				}
			}
			num6++;
		}
		Transform[] componentsInChildren = this.auth.transform.GetComponentsInChildren<Transform>();
		k = 0;
		while (k < componentsInChildren.Length)
		{
			Transform transform = componentsInChildren[k];
			if (transform.name.Equals("CAM_camera1___"))
			{
				this.camPosObj = transform;
				goto IL_0DF6;
			}
			if (transform.name.Equals("CAM_camera1_aim___"))
			{
				this.intrPosObj = transform;
				goto IL_0DF6;
			}
			if (transform.name.Equals("CAM_camera1_focallength___"))
			{
				this.fovObj = transform;
				goto IL_0DF6;
			}
			if (transform.name.Equals("CAM_camera1_twist___"))
			{
				this.rollObj = transform;
				goto IL_0DF6;
			}
			if (transform.name.StartsWith(AuthCharaData.AUTH_CHARA_PREFIX))
			{
				this.charaCount++;
				string[] array = transform.name.Split('_', StringSplitOptions.None);
				int num7 = int.Parse(array[1]);
				AuthCharaData charaData = this.GetCharaData(num7, array[2]);
				if (charaData != null)
				{
					if (charaData.AuthObj == null)
					{
						charaData.AuthObj = new List<Transform>();
					}
					charaData.AuthObj.Add(transform);
					goto IL_0DF6;
				}
			}
			else if (transform.name.StartsWith("Ef_"))
			{
				AuthEffectParam param = new AuthEffectParam(transform.name);
				if (param.ctrlParam != null && !param.ctrlParam.Equals("EFNONE"))
				{
					this.efCtrlList.Add(param.ctrlParam);
				}
				GameObject gameObject3 = new GameObject();
				gameObject3.transform.SetParent(base.transform, true);
				gameObject3.name = "Auth_" + transform.name;
				AuthPlayer.AuthEffectData authEffectData2 = new AuthPlayer.AuthEffectData(transform, gameObject3, param.effectName, false);
				this.effectList.Add(authEffectData2);
				if (isArtsMaxAction && AuthPlayer.MAX_ARTS_EFFECT_NAME.Find((string item) => param.effectName.StartsWith(item)) != null)
				{
					GameObject gameObject4 = new GameObject();
					gameObject4.transform.SetParent(base.transform, true);
					gameObject4.name = "Auth_" + transform.name + "_max";
					AuthPlayer.AuthEffectData authEffectData3 = new AuthPlayer.AuthEffectData(transform, gameObject4, param.effectName + "_max", false);
					this.effectList.Add(authEffectData3);
					goto IL_0DF6;
				}
				goto IL_0DF6;
			}
			else if (transform.name.StartsWith(AuthCharaData.AUTH_BLENDSHAPE_PREFIX))
			{
				string[] array2 = transform.name.Split('_', StringSplitOptions.None);
				int num8 = int.Parse(array2[2]);
				if (this.GetCharaData(num8, array2[3]) != null)
				{
					goto IL_0DF6;
				}
			}
			else
			{
				if (transform.name.Equals("FL_CC_technicolor_"))
				{
					this.technicolorObj = transform;
					goto IL_0DF6;
				}
				if (transform.name.Equals("FL_CC_huesaturationvalue_"))
				{
					this.hueValueObj = transform;
					goto IL_0DF6;
				}
				if (transform.name.Equals("FL_CC_radial_blur_"))
				{
					this.radBlurObj = transform;
					goto IL_0DF6;
				}
				if (transform.name.Equals("FL_CC_Wiggle"))
				{
					this.wiggleObj = transform;
					goto IL_0DF6;
				}
				if (transform.name.Equals("FL_CC_RGB_Sprit"))
				{
					this.rgbSpritObj = transform;
					goto IL_0DF6;
				}
				if (transform.name.Equals("FL_CC_Constant_Vignette_"))
				{
					this.contrastVignetteObj = transform;
					this.contrastVignetteEdgeObj = transform.Find("EDGE");
					this.contrastVignetteRgbObj = transform.Find("RGB");
					goto IL_0DF6;
				}
				if (transform.name.Equals("FL_CC_Double_Vision"))
				{
					this.doubleVisionObj = transform;
					goto IL_0DF6;
				}
				if (transform.name.Equals("FL_CC_Negative_"))
				{
					this.negativeObj = transform;
					goto IL_0DF6;
				}
				if (transform.name.Equals("FL_CC_AnalogTV_"))
				{
					this.analogTVObj = transform;
					goto IL_0DF6;
				}
				if (transform.name.Equals("FL_CC_FocusLine_"))
				{
					this.focusLineObj = transform;
					goto IL_0DF6;
				}
				if (transform.name.Equals("FL_CC_SpeedLine_"))
				{
					this.speedLineObj = transform;
					goto IL_0DF6;
				}
				if (transform.name.Equals("FL_CC_blur_"))
				{
					this.blurObj = transform;
					goto IL_0DF6;
				}
				if (transform.name.Equals("FL_CC_fog_"))
				{
					this.fogObj = transform;
					goto IL_0DF6;
				}
				if (transform.name.Equals("FL_CC_camouflage_"))
				{
					this.camouflageObj = transform;
					goto IL_0DF6;
				}
				goto IL_0DF6;
			}
			IL_0E0F:
			k++;
			continue;
			IL_0DF6:
			this.transCacheList.Add(transform.name, transform.transform);
			goto IL_0E0F;
		}
		this.postEffectAssetName = "Auth/Parameter/" + authName + "_posteffect";
		if (AssetManager.IsExsistAssetData(this.postEffectAssetName))
		{
			AssetManager.LoadAssetData(this.postEffectAssetName, AssetManager.OWNER.AuthPlayer, 0, null);
			while (!AssetManager.IsLoadFinishAssetData(this.postEffectAssetName))
			{
				yield return null;
			}
			if ((this.postEffectParam = AssetManager.GetAssetData(this.postEffectAssetName) as AuthPostEffectParam) == null)
			{
				this.postEffectParam = ScriptableObject.CreateInstance(typeof(AuthPostEffectParam)) as AuthPostEffectParam;
			}
		}
		else
		{
			this.postEffectAssetName = null;
			this.postEffectParam = ScriptableObject.CreateInstance(typeof(AuthPostEffectParam)) as AuthPostEffectParam;
		}
		this.techColor = this.AuthCameraObj.cam.gameObject.GetComponent<CC_Technicolor>();
		if (this.technicolorObj == null && this.postEffectParam.Technicolor.Count <= 0)
		{
			if (this.viewer)
			{
				this.techColor.enabled = false;
			}
			else
			{
				Object.Destroy(this.techColor);
				this.techColor = null;
			}
		}
		this.hueValue = this.AuthCameraObj.cam.gameObject.GetComponent<CC_HueSaturationValue>();
		if (this.hueValueObj == null && this.postEffectParam.HueSaturation.Count <= 0)
		{
			if (this.viewer)
			{
				this.hueValue.enabled = false;
			}
			else
			{
				Object.Destroy(this.hueValue);
				this.hueValue = null;
			}
		}
		this.radBlur = this.AuthCameraObj.cam.gameObject.GetComponent<CC_RadialBlur>();
		if (this.radBlurObj == null && this.postEffectParam.RadialBlur.Count <= 0)
		{
			if (this.viewer)
			{
				this.radBlur.enabled = false;
			}
			else
			{
				Object.Destroy(this.radBlur);
				this.radBlur = null;
			}
		}
		this.wiggle = this.AuthCameraObj.cam.gameObject.GetComponent<CC_Wiggle>();
		if (this.wiggleObj == null && this.postEffectParam.Wiggle.Count <= 0)
		{
			if (this.viewer)
			{
				this.wiggle.enabled = false;
			}
			else
			{
				Object.Destroy(this.wiggle);
				this.wiggle = null;
			}
		}
		this.rgbSprit = this.AuthCameraObj.cam.gameObject.GetComponent<CC_RGBSplit>();
		if (this.rgbSpritObj == null && this.postEffectParam.RGBSplit.Count <= 0)
		{
			if (this.viewer)
			{
				this.rgbSprit.enabled = false;
			}
			else
			{
				Object.Destroy(this.rgbSprit);
				this.rgbSprit = null;
			}
		}
		this.contrastVignette = this.AuthCameraObj.cam.gameObject.GetComponent<CC_ContrastVignette>();
		if (this.contrastVignetteObj == null && this.postEffectParam.ContrastVignette.Count <= 0)
		{
			if (this.viewer)
			{
				this.contrastVignette.enabled = false;
			}
			else
			{
				Object.Destroy(this.contrastVignette);
				this.contrastVignette = null;
			}
		}
		this.doubleVision = this.AuthCameraObj.cam.gameObject.GetComponent<CC_DoubleVision>();
		if (this.doubleVisionObj == null && this.postEffectParam.DoubleVision.Count <= 0)
		{
			if (this.viewer)
			{
				this.doubleVision.enabled = false;
			}
			else
			{
				Object.Destroy(this.doubleVision);
				this.doubleVision = null;
			}
		}
		this.negative = this.AuthCameraObj.cam.gameObject.GetComponent<CC_Negative>();
		if (this.negativeObj == null && this.postEffectParam.Negative.Count <= 0)
		{
			if (this.viewer)
			{
				this.negative.enabled = false;
			}
			else
			{
				Object.Destroy(this.negative);
				this.negative = null;
			}
		}
		this.analogTV = this.AuthCameraObj.cam.gameObject.GetComponent<CC_AnalogTV>();
		if (this.analogTVObj == null && this.postEffectParam.AnalogTV.Count <= 0)
		{
			if (this.viewer)
			{
				this.analogTV.enabled = false;
			}
			else
			{
				Object.Destroy(this.analogTV);
				this.analogTV = null;
			}
		}
		this.focusLine = this.AuthCameraObj.cam.gameObject.GetComponent<FocusLine>();
		if (this.focusLineObj == null && this.postEffectParam.FocusLine.Count <= 0)
		{
			if (this.viewer)
			{
				this.focusLine.enabled = false;
			}
			else
			{
				Object.Destroy(this.focusLine);
				this.focusLine = null;
			}
		}
		this.speedLine = this.AuthCameraObj.cam.gameObject.GetComponent<SpeedLine>();
		if (this.speedLineObj == null && this.postEffectParam.SpeedLine.Count <= 0)
		{
			if (this.viewer)
			{
				this.speedLine.enabled = false;
			}
			else
			{
				Object.Destroy(this.speedLine);
				this.speedLine = null;
			}
		}
		this.blur = this.AuthCameraObj.cam.gameObject.GetComponent<Blur>();
		if (this.blurObj == null && this.postEffectParam.Blur.Count <= 0)
		{
			if (this.viewer)
			{
				this.blur.enabled = false;
			}
			else
			{
				Object.Destroy(this.blur);
				this.blur = null;
			}
		}
		this.AuthCameraObj.cam.gameObject.GetComponent<MultipleGaussianBloom>().camouflageEnable = true;
		this.camouflage = false;
		Object.Destroy(this.AuthCameraObjSub.cam.gameObject.GetComponent<CC_HueSaturationValue>());
		Object.Destroy(this.AuthCameraObjSub.cam.gameObject.GetComponent<CC_RadialBlur>());
		Object.Destroy(this.AuthCameraObjSub.cam.gameObject.GetComponent<CC_Technicolor>());
		Object.Destroy(this.AuthCameraObjSub.cam.gameObject.GetComponent<CC_Wiggle>());
		Object.Destroy(this.AuthCameraObjSub.cam.gameObject.GetComponent<CC_RGBSplit>());
		Object.Destroy(this.AuthCameraObjSub.cam.gameObject.GetComponent<CC_ContrastVignette>());
		Object.Destroy(this.AuthCameraObjSub.cam.gameObject.GetComponent<CC_DoubleVision>());
		Object.Destroy(this.AuthCameraObjSub.cam.gameObject.GetComponent<CC_Negative>());
		Object.Destroy(this.AuthCameraObjSub.cam.gameObject.GetComponent<CC_AnalogTV>());
		Object.Destroy(this.AuthCameraObjSub.cam.gameObject.GetComponent<FocusLine>());
		Object.Destroy(this.AuthCameraObjSub.cam.gameObject.GetComponent<SpeedLine>());
		Object.Destroy(this.AuthCameraObjSub.cam.gameObject.GetComponent<Blur>());
		if (this.blur != null)
		{
			this.CharaCamera = new GameObject("CharaCamera", new Type[] { typeof(Camera) }).GetComponent<Camera>();
			this.CharaCamera.transform.SetParent(base.transform, false);
			this.CharaCamera.gameObject.SetActive(false);
		}
		this.authLight = new List<GameObject>();
		this.authLightParam = this.auth.GetComponent<AuthLightParam>();
		if (this.authLightParam != null)
		{
			for (int i = 0; i < this.authLightParam.authLight.Length; i = k + 1)
			{
				if (this.authLightParam.authLight[i] != "")
				{
					string assetName = "Light/auth/" + this.authLightParam.authLight[i];
					if (AssetManager.IsExsistAssetData(assetName))
					{
						AssetManager.LoadAssetData(assetName, AssetManager.OWNER.AuthPlayer, 0, null);
						while (!AssetManager.IsLoadFinishAssetData(assetName))
						{
							yield return null;
						}
						GameObject gameObject5 = AssetManager.InstantiateAssetData(assetName, this.auth.transform);
						if (gameObject5 != null)
						{
							gameObject5.SetActive(false);
							this.authLight.Add(gameObject5);
							this.loadLightAssetNameList.Add(assetName);
						}
					}
					assetName = null;
				}
				k = i;
			}
		}
		if (this.viewer)
		{
			Color color = Color.white;
			if (isModelShadow && this.authLight.Count > 0)
			{
				GameObject gameObject6 = this.authLight[0];
				Light light = gameObject6.GetComponent<Light>();
				if (light != null)
				{
					light.shadows = LightShadows.Soft;
					light.shadowStrength = 0.73f;
				}
				GameObject gameObject7 = Object.Instantiate<GameObject>(gameObject6, gameObject6.transform.parent);
				light = gameObject7.GetComponent<Light>();
				if (light != null)
				{
					light.cullingMask &= ~(1 << LayerMask.NameToLayer("AuthMain"));
					light.shadowStrength = 0.54f;
				}
				gameObject7.transform.localEulerAngles += new Vector3(0f, 30f, 0f);
				this.authLight.Add(gameObject7);
				gameObject7 = Object.Instantiate<GameObject>(gameObject6, gameObject6.transform.parent);
				light = gameObject7.GetComponent<Light>();
				if (light != null)
				{
					light.cullingMask &= ~(1 << LayerMask.NameToLayer("AuthMain"));
					light.shadowStrength = 0.35f;
				}
				gameObject7.transform.localEulerAngles += new Vector3(0f, -60f, 0f);
				this.authLight.Add(gameObject7);
				color /= 3f;
			}
			foreach (Renderer renderer in this.stagePresetCtrl.stageModelObj.GetComponentsInChildren<Renderer>(true))
			{
				if (renderer.material.HasProperty("_Color") && renderer.material.HasProperty("_ShadowTex") && renderer.material.HasProperty("_AddmapTex"))
				{
					renderer.material.SetColor("_Color", color);
				}
				if (renderer.materials != null)
				{
					foreach (Material material in renderer.materials)
					{
						if (material.HasProperty("_Color") && material.HasProperty("_ShadowTex") && material.HasProperty("_AddmapTex"))
						{
							material.SetColor("_Color", color);
						}
					}
				}
			}
		}
		if (!this.disableAuthFaceMotion)
		{
			for (int num10 = 0; num10 < this.charaList.Count; num10++)
			{
				if (this.charaList[num10].AuthObj != null)
				{
					string text5 = "";
					foreach (object obj in this.charaList[num10].AuthObj[this.objIndex].transform)
					{
						Transform transform2 = (Transform)obj;
						int num11 = transform2.name.IndexOf(" ");
						if (num11 >= 0)
						{
							text5 = transform2.name.Substring(num11);
							break;
						}
					}
					this.charaList[num10].charaModelHandle.UpdateCharaObjByAuth(this.charaList[num10].AuthObj[this.objIndex].Find("root" + text5), text5);
				}
			}
		}
		foreach (AuthPlayer.AuthEffectData efData in this.effectList)
		{
			while (!efData.IsFinishLoadEffect())
			{
				yield return null;
			}
			efData = null;
		}
		List<AuthPlayer.AuthEffectData>.Enumerator enumerator4 = default(List<AuthPlayer.AuthEffectData>.Enumerator);
		IEnumerator soundDownload = SoundManager.LoadCueSheetWithDownload(AuthPlayer.GetSoundName(this.auth.name));
		while (soundDownload.MoveNext())
		{
			yield return null;
		}
		this.isFinishInitialize = true;
		yield break;
		yield break;
	}

	private static string GetSoundName(string authName)
	{
		if (authName == null)
		{
			return null;
		}
		if (!authName.StartsWith(AuthPlayer.AUTH_NAME_PREFIX))
		{
			return null;
		}
		return "prd_se_" + authName.Substring(AuthPlayer.AUTH_NAME_PREFIX.Length).ToLower();
	}

	private static string GetOptionEffectName(string authName, int charaIndex)
	{
		if (authName == null)
		{
			return string.Empty;
		}
		if (!authName.StartsWith(AuthPlayer.AUTH_NAME_PREFIX))
		{
			return string.Empty;
		}
		return "Ef_option_" + authName.Substring(AuthPlayer.AUTH_NAME_PREFIX.Length).ToLower() + "opt" + ((charaIndex == 0) ? "" : "s");
	}

	public bool IsFinishInitialize()
	{
		return this.isFinishInitialize;
	}

	private void OnDestroy()
	{
		this.DestroyProcessing();
	}

	public void DestroyProcessing()
	{
		if (this.auth == null)
		{
			return;
		}
		if (this.InitializeCoroutine != null)
		{
			base.StopCoroutine(this.InitializeCoroutine);
			this.InitializeCoroutine = null;
		}
		this.PlayFinishProcess();
		SoundManager.UnloadCueSheet(AuthPlayer.GetSoundName(this.auth.name));
		foreach (string text in this.loadLightAssetNameList)
		{
			AssetManager.UnloadAssetData(text, AssetManager.OWNER.AuthPlayer);
		}
		foreach (GameObject gameObject in this.authLight)
		{
			Object.Destroy(gameObject);
		}
		foreach (AuthCharaData authCharaData in this.charaList)
		{
			Object.Destroy(authCharaData.Parent);
		}
		this.postEffectParam = null;
		if (!string.IsNullOrEmpty(this.postEffectAssetName))
		{
			AssetManager.UnloadAssetData(this.postEffectAssetName, AssetManager.OWNER.AuthPlayer);
		}
		Object.Destroy(this.auth);
		foreach (AuthPlayer.AuthEffectData authEffectData in this.effectList)
		{
			authEffectData.Destory();
			Object.Destroy(authEffectData.Parent);
		}
		this.backupRenderSetting.Param2Scene();
		Object.Destroy(this.backupRenderSetting);
		this.backupRenderSetting = null;
		this.effectList = null;
		this.transCacheList = null;
		this.anim = null;
		this.auth = null;
		this.authLight = null;
		this.charaList = null;
		Object.Destroy(this.AuthCameraObj.cam.gameObject);
		Object.Destroy(this.AuthCameraObjSub.cam.gameObject);
		this.AuthCameraObj = null;
		this.AuthCameraObjSub = null;
		if (this.CharaCamera != null)
		{
			Object.Destroy(this.CharaCamera.gameObject);
			this.CharaCamera = null;
		}
		this.isFinishInitialize = false;
		this.stagePresetCtrl = null;
	}

	private void Update()
	{
		this.ProjectUpdate();
		if (this.isFinishDestroy && this.auth != null && this.IsFinished())
		{
			this.PlayFinishProcess();
		}
		float num = 4f;
		if (this.fixKeepFps > 0 && TimeManager.NowFPS() - TimeManager.AveFPS() >= num)
		{
			Application.targetFrameRate = 30;
		}
	}

	private void LateUpdate()
	{
		this.ProjectLateUpdate();
	}

	private void ProjectUpdate()
	{
		if (this.auth == null)
		{
			return;
		}
		if (!this.isPlayed)
		{
			return;
		}
		if (this.pause != this.prePause)
		{
			if (this.pause)
			{
				this.anim["Custom"].speed = 0f;
			}
			else
			{
				this.anim["Custom"].speed = 1f;
			}
			this.prePause = this.pause;
		}
		float num = (this.fovObj.localScale.y - this.offset.x) * 0.01f;
		float num2 = Mathf.Atan2(0.12699999f, num) * 57.295776f * 2f;
		this.AuthCameraObj.SetFieldOfView(num2);
		this.AuthCameraObj.SetCameraPosition(this.camPosObj.position);
		float num3 = this.rollObj.transform.localScale.y - this.offset.x;
		this.AuthCameraObj.SetInterPos(this.intrPosObj.position, num3);
		foreach (AuthCharaData authCharaData in this.charaList)
		{
			if (authCharaData.AuthObj != null)
			{
				authCharaData.Parent.transform.position = authCharaData.AuthObj[this.objIndex].transform.position;
				authCharaData.Parent.transform.rotation = authCharaData.AuthObj[this.objIndex].transform.rotation;
				authCharaData.Parent.transform.localScale = authCharaData.AuthObj[this.objIndex].transform.localScale;
			}
		}
		if (!this.viewer || this.GetTime() != this.time)
		{
			int num4 = (int)(this.GetTime() * 30f);
			if (this.techColor != null && this.postEffectParam.Technicolor.Count > 0)
			{
				AuthPostEffectParam.TechnicolorParam technicolorParam = this.postEffectParam.prvTechnicolor(num4);
				if (technicolorParam == null || technicolorParam.frame < 0)
				{
					this.techColor.enabled = false;
				}
				else
				{
					this.techColor.enabled = true;
					AuthPostEffectParam.TechnicolorParam technicolorParam2 = this.postEffectParam.nxtTechnicolor(num4);
					if (technicolorParam2 == null)
					{
						technicolorParam2 = technicolorParam;
					}
					int num5 = Mathf.Abs(technicolorParam2.frame) - technicolorParam.frame;
					float num6 = ((num5 > 0) ? ((float)(num4 - technicolorParam.frame) / (float)num5) : 0f);
					this.techColor.exposure = Mathf.Lerp(technicolorParam.exposure, technicolorParam2.exposure, num6);
					this.techColor.balance = Vector3.Lerp(technicolorParam.balance, technicolorParam2.balance, num6);
					this.techColor.amount = Mathf.Lerp(technicolorParam.amount, technicolorParam2.amount, num6);
				}
			}
			else if (this.techColor != null && this.technicolorObj != null)
			{
				this.techColor.exposure = this.technicolorObj.localPosition.y * 100f;
				this.techColor.amount = this.technicolorObj.localPosition.z * 100f;
				this.techColor.balance = new Vector3(this.technicolorObj.localScale.x, this.technicolorObj.localScale.y, this.technicolorObj.localScale.z);
			}
			if (this.hueValue != null && this.postEffectParam.HueSaturation.Count > 0)
			{
				AuthPostEffectParam.HueSaturationParam hueSaturationParam = this.postEffectParam.prvHueSaturation(num4);
				if (hueSaturationParam == null || hueSaturationParam.frame < 0)
				{
					this.hueValue.enabled = false;
				}
				else
				{
					this.hueValue.enabled = true;
					AuthPostEffectParam.HueSaturationParam hueSaturationParam2 = this.postEffectParam.nxtHueSaturation(num4);
					if (hueSaturationParam2 == null)
					{
						hueSaturationParam2 = hueSaturationParam;
					}
					int num7 = Mathf.Abs(hueSaturationParam2.frame) - hueSaturationParam.frame;
					float num8 = ((num7 > 0) ? ((float)(num4 - hueSaturationParam.frame) / (float)num7) : 0f);
					this.hueValue.hue = Mathf.Lerp(hueSaturationParam.hue, hueSaturationParam2.hue, num8);
					this.hueValue.saturation = Mathf.Lerp(hueSaturationParam.saturation, hueSaturationParam2.saturation, num8);
					this.hueValue.value = Mathf.Lerp(hueSaturationParam.value, hueSaturationParam2.value, num8);
				}
			}
			else if (this.hueValue != null && this.hueValueObj != null)
			{
				this.hueValue.hue = this.hueValueObj.localScale.x;
				this.hueValue.saturation = this.hueValueObj.localScale.y;
				this.hueValue.value = this.hueValueObj.localScale.z;
			}
			if (this.radBlur != null && this.postEffectParam.RadialBlur.Count > 0)
			{
				AuthPostEffectParam.RadialBlurParam radialBlurParam = this.postEffectParam.prvRadialBlur(num4);
				if (radialBlurParam == null || radialBlurParam.frame < 0)
				{
					this.radBlur.enabled = false;
				}
				else
				{
					this.radBlur.enabled = true;
					AuthPostEffectParam.RadialBlurParam radialBlurParam2 = this.postEffectParam.nxtRadialBlur(num4);
					if (radialBlurParam2 == null)
					{
						radialBlurParam2 = radialBlurParam;
					}
					int num9 = Mathf.Abs(radialBlurParam2.frame) - radialBlurParam.frame;
					float num10 = ((num9 > 0) ? ((float)(num4 - radialBlurParam.frame) / (float)num9) : 0f);
					this.radBlur.amount = Mathf.Lerp(radialBlurParam.amount, radialBlurParam2.amount, num10);
					this.radBlur.center = Vector2.Lerp(radialBlurParam.center, radialBlurParam2.center, num10);
				}
			}
			else if (this.radBlur != null && this.radBlurObj != null)
			{
				this.radBlur.amount = this.radBlurObj.localScale.y;
				this.radBlur.center = new Vector2(this.radBlurObj.localScale.x, this.radBlurObj.localScale.z);
			}
			if (this.wiggle != null && this.postEffectParam.Wiggle.Count > 0)
			{
				AuthPostEffectParam.WiggleParam wiggleParam = this.postEffectParam.prvWiggle(num4);
				if (wiggleParam == null || wiggleParam.frame < 0)
				{
					this.wiggle.enabled = false;
				}
				else
				{
					this.wiggle.enabled = true;
					AuthPostEffectParam.WiggleParam wiggleParam2 = this.postEffectParam.nxtWiggle(num4);
					if (wiggleParam2 == null)
					{
						wiggleParam2 = wiggleParam;
					}
					int num11 = Mathf.Abs(wiggleParam2.frame) - wiggleParam.frame;
					float num12 = ((num11 > 0) ? ((float)(num4 - wiggleParam.frame) / (float)num11) : 0f);
					this.wiggle.scale = Mathf.Lerp(wiggleParam.scale, wiggleParam2.scale, num12);
					this.wiggle.speed = Mathf.Lerp(wiggleParam.speed, wiggleParam2.speed, num12);
				}
			}
			else if (this.wiggle != null && this.wiggleObj != null)
			{
				this.wiggle.scale = this.wiggleObj.localPosition.x * 100f;
				this.wiggle.speed = this.wiggleObj.localPosition.y * 100f;
			}
			if (this.rgbSprit != null && this.postEffectParam.RGBSplit.Count > 0)
			{
				AuthPostEffectParam.RGBSplitParam rgbsplitParam = this.postEffectParam.prvRGBSplit(num4);
				if (rgbsplitParam == null || rgbsplitParam.frame < 0)
				{
					this.rgbSprit.enabled = false;
				}
				else
				{
					this.rgbSprit.enabled = true;
					AuthPostEffectParam.RGBSplitParam rgbsplitParam2 = this.postEffectParam.nxtRGBSplit(num4);
					if (rgbsplitParam2 == null)
					{
						rgbsplitParam2 = rgbsplitParam;
					}
					int num13 = Mathf.Abs(rgbsplitParam2.frame) - rgbsplitParam.frame;
					float num14 = ((num13 > 0) ? ((float)(num4 - rgbsplitParam.frame) / (float)num13) : 0f);
					this.rgbSprit.amount = Mathf.Lerp(rgbsplitParam.amount, rgbsplitParam2.amount, num14);
					this.rgbSprit.angle = Mathf.Lerp(rgbsplitParam.angle, rgbsplitParam2.angle, num14);
				}
			}
			else if (this.rgbSprit != null && this.rgbSpritObj != null)
			{
				this.rgbSprit.amount = this.rgbSpritObj.localPosition.x * 100f;
				this.rgbSprit.angle = this.rgbSpritObj.localPosition.y * 100f;
			}
			if (this.contrastVignette != null && this.postEffectParam.ContrastVignette.Count > 0)
			{
				AuthPostEffectParam.ContrastVignetteParam contrastVignetteParam = this.postEffectParam.prvContrastVignette(num4);
				if (contrastVignetteParam == null || contrastVignetteParam.frame < 0)
				{
					this.contrastVignette.enabled = false;
				}
				else
				{
					this.contrastVignette.enabled = true;
					AuthPostEffectParam.ContrastVignetteParam contrastVignetteParam2 = this.postEffectParam.nxtContrastVignette(num4);
					if (contrastVignetteParam2 == null)
					{
						contrastVignetteParam2 = contrastVignetteParam;
					}
					int num15 = Mathf.Abs(contrastVignetteParam2.frame) - contrastVignetteParam.frame;
					float num16 = ((num15 > 0) ? ((float)(num4 - contrastVignetteParam.frame) / (float)num15) : 0f);
					this.contrastVignette.center = Vector2.Lerp(contrastVignetteParam.center, contrastVignetteParam2.center, num16);
					this.contrastVignette.sharpness = Mathf.Lerp(contrastVignetteParam.sharpness, contrastVignetteParam2.sharpness, num16);
					this.contrastVignette.darkness = Mathf.Lerp(contrastVignetteParam.darkness, contrastVignetteParam2.darkness, num16);
					this.contrastVignette.contrast = Mathf.Lerp(contrastVignetteParam.contrast, contrastVignetteParam2.contrast, num16);
					this.contrastVignette.redCoeff = Mathf.Lerp(contrastVignetteParam.redCoeff, contrastVignetteParam2.redCoeff, num16);
					this.contrastVignette.greenCoeff = Mathf.Lerp(contrastVignetteParam.greenCoeff, contrastVignetteParam2.greenCoeff, num16);
					this.contrastVignette.blueCoeff = Mathf.Lerp(contrastVignetteParam.blueCoeff, contrastVignetteParam2.blueCoeff, num16);
					this.contrastVignette.edge = Mathf.Lerp(contrastVignetteParam.edge, contrastVignetteParam2.edge, num16);
					this.contrastVignette.redAmbient = Mathf.Lerp(contrastVignetteParam.redAmbient, contrastVignetteParam2.redAmbient, num16);
					this.contrastVignette.greenAmbient = Mathf.Lerp(contrastVignetteParam.greenAmbient, contrastVignetteParam2.greenAmbient, num16);
					this.contrastVignette.blueAmbient = Mathf.Lerp(contrastVignetteParam.blueAmbient, contrastVignetteParam2.blueAmbient, num16);
				}
			}
			else if (this.contrastVignette != null && this.contrastVignetteObj != null)
			{
				this.contrastVignette.center.x = -this.contrastVignetteObj.localPosition.x * 100f;
				this.contrastVignette.center.y = this.contrastVignetteObj.localPosition.y * 100f;
				this.contrastVignette.sharpness = this.contrastVignetteObj.localPosition.z * 100f;
				this.contrastVignette.darkness = this.contrastVignetteObj.localScale.x;
				this.contrastVignette.contrast = this.contrastVignetteObj.localScale.y;
				this.contrastVignette.edge = this.contrastVignetteObj.localScale.z;
				if (this.contrastVignetteRgbObj)
				{
					this.contrastVignette.redCoeff = -this.contrastVignetteRgbObj.localPosition.x * 100f;
					this.contrastVignette.greenCoeff = this.contrastVignetteRgbObj.localPosition.y * 100f;
					this.contrastVignette.blueCoeff = this.contrastVignetteRgbObj.localPosition.z * 100f;
				}
				if (this.contrastVignetteEdgeObj)
				{
					this.contrastVignette.redAmbient = this.contrastVignetteEdgeObj.localScale.x;
					this.contrastVignette.greenAmbient = this.contrastVignetteEdgeObj.localScale.y;
					this.contrastVignette.blueAmbient = this.contrastVignetteEdgeObj.localScale.z;
				}
			}
			if (this.doubleVision != null && this.postEffectParam.DoubleVision.Count > 0)
			{
				AuthPostEffectParam.DoubleVisionParam doubleVisionParam = this.postEffectParam.prvDoubleVision(num4);
				if (doubleVisionParam == null || doubleVisionParam.frame < 0)
				{
					this.doubleVision.enabled = false;
				}
				else
				{
					this.doubleVision.enabled = true;
					AuthPostEffectParam.DoubleVisionParam doubleVisionParam2 = this.postEffectParam.nxtDoubleVision(num4);
					if (doubleVisionParam2 == null)
					{
						doubleVisionParam2 = doubleVisionParam;
					}
					int num17 = Mathf.Abs(doubleVisionParam2.frame) - doubleVisionParam.frame;
					float num18 = ((num17 > 0) ? ((float)(num4 - doubleVisionParam.frame) / (float)num17) : 0f);
					this.doubleVision.displace = Vector2.Lerp(doubleVisionParam.displace, doubleVisionParam2.displace, num18);
					this.doubleVision.amount = Mathf.Lerp(doubleVisionParam.amount, doubleVisionParam2.amount, num18);
				}
			}
			else if (this.doubleVision != null && this.doubleVisionObj != null)
			{
				this.doubleVision.displace.x = this.doubleVisionObj.localPosition.x * 100f;
				this.doubleVision.displace.y = this.doubleVisionObj.localPosition.y * 100f;
				this.doubleVision.amount = this.doubleVisionObj.localPosition.z * 100f;
			}
			if (this.negative != null && this.postEffectParam.Negative.Count > 0)
			{
				AuthPostEffectParam.NegativeParam negativeParam = this.postEffectParam.prvNegative(num4);
				if (negativeParam == null || negativeParam.frame < 0)
				{
					this.negative.enabled = false;
				}
				else
				{
					this.negative.enabled = true;
					AuthPostEffectParam.NegativeParam negativeParam2 = this.postEffectParam.nxtNegative(num4);
					if (negativeParam2 == null)
					{
						negativeParam2 = negativeParam;
					}
					int num19 = Mathf.Abs(negativeParam2.frame) - negativeParam.frame;
					float num20 = ((num19 > 0) ? ((float)(num4 - negativeParam.frame) / (float)num19) : 0f);
					this.negative.amount = Mathf.Lerp(negativeParam.amount, negativeParam2.amount, num20);
				}
			}
			else if (this.negative != null && this.negativeObj != null)
			{
				this.negative.amount = -this.negativeObj.localPosition.x * 100f;
			}
			if (this.analogTV != null && this.postEffectParam.AnalogTV.Count > 0)
			{
				AuthPostEffectParam.AnalogTVParam analogTVParam = this.postEffectParam.prvAnalogTV(num4);
				if (analogTVParam == null || analogTVParam.frame < 0)
				{
					this.analogTV.enabled = false;
				}
				else
				{
					this.analogTV.enabled = true;
					AuthPostEffectParam.AnalogTVParam analogTVParam2 = this.postEffectParam.nxtAnalogTV(num4);
					if (analogTVParam2 == null)
					{
						analogTVParam2 = analogTVParam;
					}
					int num21 = Mathf.Abs(analogTVParam2.frame) - analogTVParam.frame;
					float num22 = ((num21 > 0) ? ((float)(num4 - analogTVParam.frame) / (float)num21) : 0f);
					this.analogTV.noiseIntensity = Mathf.Lerp(analogTVParam.noiseIntensity, analogTVParam2.noiseIntensity, num22);
					this.analogTV.scanlinesIntensity = Mathf.Lerp(analogTVParam.scanlinesIntensity, analogTVParam2.scanlinesIntensity, num22);
				}
			}
			else if (this.analogTV != null && this.analogTVObj != null)
			{
				this.analogTV.noiseIntensity = this.analogTVObj.localPosition.x * 100f;
				this.analogTV.scanlinesIntensity = this.analogTVObj.localPosition.y * 100f;
			}
			if (this.focusLine != null && this.postEffectParam.FocusLine.Count > 0)
			{
				AuthPostEffectParam.FocusLineParam focusLineParam = this.postEffectParam.prvFocusLine(num4);
				if (focusLineParam == null || focusLineParam.frame < 0)
				{
					this.focusLine.enabled = false;
				}
				else
				{
					this.focusLine.enabled = true;
					AuthPostEffectParam.FocusLineParam focusLineParam2 = this.postEffectParam.nxtFocusLine(num4);
					if (focusLineParam2 == null)
					{
						focusLineParam2 = focusLineParam;
					}
					int num23 = Mathf.Abs(focusLineParam2.frame) - focusLineParam.frame;
					float num24 = ((num23 > 0) ? ((float)(num4 - focusLineParam.frame) / (float)num23) : 0f);
					this.focusLine.focusPos = Vector2.Lerp(focusLineParam.focusPos, focusLineParam2.focusPos, num24);
					this.focusLine.lineColor = Color.Lerp(focusLineParam.lineColor, focusLineParam2.lineColor, num24);
				}
			}
			else if (this.focusLine != null && this.focusLineObj != null)
			{
				if (this.focusLineObj.localPosition.z != 0f)
				{
					this.focusLine.enabled = true;
					this.focusLine.focusPos.x = -this.focusLineObj.localPosition.x * 100f;
					this.focusLine.focusPos.y = this.focusLineObj.localPosition.y * 100f;
					this.focusLine.lineColor.r = this.focusLineObj.localScale.x / 255f;
					this.focusLine.lineColor.g = this.focusLineObj.localScale.y / 255f;
					this.focusLine.lineColor.b = this.focusLineObj.localScale.z / 255f;
					this.focusLine.lineColor.a = this.focusLineObj.localPosition.z * 100f / 255f;
				}
				else
				{
					this.focusLine.enabled = false;
				}
			}
			if (this.speedLine != null && this.postEffectParam.SpeedLine.Count > 0)
			{
				AuthPostEffectParam.SpeedLineParam speedLineParam = this.postEffectParam.prvSpeedLine(num4);
				if (speedLineParam == null || speedLineParam.frame < 0)
				{
					this.speedLine.enabled = false;
				}
				else
				{
					this.speedLine.enabled = true;
					AuthPostEffectParam.SpeedLineParam speedLineParam2 = this.postEffectParam.nxtSpeedLine(num4);
					if (speedLineParam2 == null)
					{
						speedLineParam2 = speedLineParam;
					}
					int num25 = Mathf.Abs(speedLineParam2.frame) - speedLineParam.frame;
					float num26 = ((num25 > 0) ? ((float)(num4 - speedLineParam.frame) / (float)num25) : 0f);
					this.speedLine.lineAngle = Mathf.Lerp(speedLineParam.lineAngle, speedLineParam2.lineAngle, num26);
					this.speedLine.lineSpeed = Mathf.Lerp(speedLineParam.lineSpeed, speedLineParam2.lineSpeed, num26);
					this.speedLine.lineColor = Color.Lerp(speedLineParam.lineColor, speedLineParam2.lineColor, num26);
				}
			}
			else if (this.speedLine != null && this.speedLineObj != null)
			{
				if (this.speedLineObj.localPosition.z != 0f)
				{
					this.speedLine.enabled = true;
					this.speedLine.lineAngle = -this.speedLineObj.localPosition.x * 100f;
					this.speedLine.lineSpeed = this.speedLineObj.localPosition.y * 100f;
					this.speedLine.lineColor.r = this.speedLineObj.localScale.x / 255f;
					this.speedLine.lineColor.g = this.speedLineObj.localScale.y / 255f;
					this.speedLine.lineColor.b = this.speedLineObj.localScale.z / 255f;
					this.speedLine.lineColor.a = this.speedLineObj.localPosition.z * 100f / 255f;
				}
				else
				{
					this.speedLine.enabled = false;
				}
			}
			if (this.blur != null && this.postEffectParam.Blur.Count > 0)
			{
				AuthPostEffectParam.BlurParam blurParam = this.postEffectParam.prvBlur(num4);
				if (blurParam == null || blurParam.frame < 0)
				{
					this.blur.enabled = false;
				}
				else
				{
					this.blur.enabled = true;
					AuthPostEffectParam.BlurParam blurParam2 = this.postEffectParam.nxtBlur(num4);
					if (blurParam2 == null)
					{
						blurParam2 = blurParam;
					}
					int num27 = Mathf.Abs(blurParam2.frame) - blurParam.frame;
					float num28 = ((num27 > 0) ? ((float)(num4 - blurParam.frame) / (float)num27) : 0f);
					this.blur.iterations = Mathf.RoundToInt(Mathf.Lerp((float)blurParam.iterations, (float)blurParam2.iterations, num28));
					this.blur.blurSpread = Mathf.Lerp(blurParam.blurSpread, blurParam2.blurSpread, num28);
					this.blur.amount = Mathf.Lerp(blurParam.amount, blurParam2.amount, num28);
				}
			}
			else if (this.blur != null && this.blurObj != null)
			{
				this.blur.iterations = Mathf.RoundToInt(this.blurObj.localScale.x);
				this.blur.blurSpread = this.blurObj.localScale.y;
				this.blur.amount = this.blurObj.localScale.z;
				this.blur.enabled = this.blur.iterations > 0;
			}
			if (this.postEffectParam.Fog.Count > 0)
			{
				AuthPostEffectParam.FogParam fogParam = this.postEffectParam.prvFog(num4);
				if (fogParam == null || fogParam.frame < 0)
				{
					RenderSettings.fog = false;
				}
				else
				{
					RenderSettings.fog = true;
					RenderSettings.fogMode = FogMode.Linear;
					AuthPostEffectParam.FogParam fogParam2 = this.postEffectParam.nxtFog(num4);
					if (fogParam2 == null)
					{
						fogParam2 = fogParam;
					}
					int num29 = Mathf.Abs(fogParam2.frame) - fogParam.frame;
					float num30 = ((num29 > 0) ? ((float)(num4 - fogParam.frame) / (float)num29) : 0f);
					RenderSettings.fogColor = Color.Lerp(fogParam.color, fogParam2.color, num30);
					RenderSettings.fogStartDistance = Mathf.Lerp(fogParam.start, fogParam2.start, num30);
					RenderSettings.fogEndDistance = Mathf.Lerp(fogParam.end, fogParam2.end, num30);
				}
			}
			else if (this.fogObj != null)
			{
				RenderSettings.fogMode = FogMode.Linear;
				RenderSettings.fogColor = new Color(this.fogObj.localScale.x / 255f, this.fogObj.localScale.y / 255f, this.fogObj.localScale.z / 255f, this.fogObj.localPosition.z * 100f / 255f);
				RenderSettings.fogStartDistance = -this.fogObj.localPosition.x * 100f;
				RenderSettings.fogEndDistance = this.fogObj.localPosition.y * 100f;
				RenderSettings.fog = RenderSettings.fogStartDistance > 0f;
			}
			if (this.postEffectParam.Camouflage.Count > 0)
			{
				AuthPostEffectParam.CamouflageParam camouflageParam = this.postEffectParam.prvCamouflage(num4);
				this.camouflage = camouflageParam != null && camouflageParam.frame >= 0;
			}
			else if (this.camouflageObj != null)
			{
				this.camouflage = this.camouflageObj.localScale.x < 0.5f;
			}
			if (this.charaList.Count > 0)
			{
				this.charaList[0].charaModelHandle.camouflage = this.camouflage;
			}
		}
		if (this.CharaCamera != null && this.blur != null)
		{
			if (this.CharaCamera.gameObject.activeSelf ^ this.blur.enabled)
			{
				this.CharaCamera.gameObject.SetActive(this.blur.enabled);
				int num31 = LayerMask.NameToLayer(this.blur.enabled ? "AuthChara" : "AuthMain");
				foreach (AuthCharaData authCharaData2 in this.charaList)
				{
					authCharaData2.charaModelHandle.SetLayer(num31);
				}
			}
			if (this.CharaCamera.gameObject.activeSelf)
			{
				this.CharaCamera.CopyFrom(this.AuthCameraObj.cam);
				this.CharaCamera.depth += 1f;
				this.CharaCamera.clearFlags = CameraClearFlags.Nothing;
				this.CharaCamera.cullingMask = 1 << LayerMask.NameToLayer("AuthChara");
				this.CharaCamera.transform.position = this.AuthCameraObj.Trans.position;
				this.CharaCamera.transform.rotation = this.AuthCameraObj.Trans.rotation;
				this.CharaCamera.transform.localScale = this.AuthCameraObj.Trans.localScale;
			}
		}
		if (this.stagePresetCtrl != null)
		{
			if (this.stageDisableFrame >= 0)
			{
				bool flag = (float)this.stageDisableFrame >= this.anim["Custom"].time * 30f;
				this.stagePresetCtrl.gameObject.SetActive(flag);
				foreach (GameObject gameObject in this.authLight)
				{
					gameObject.SetActive(!flag);
				}
				this.stagePresetCtrl.lightByStage.enabled = flag;
				this.stagePresetCtrl.lightByPlayer.enabled = flag;
				this.stagePresetCtrl.lightByEnemy.enabled = flag;
			}
			else
			{
				bool flag2 = this.authLight.Count == 0;
				foreach (GameObject gameObject2 in this.authLight)
				{
					gameObject2.SetActive(!flag2);
				}
				this.stagePresetCtrl.lightByStage.enabled = flag2;
				this.stagePresetCtrl.lightByPlayer.enabled = flag2;
				this.stagePresetCtrl.lightByEnemy.enabled = flag2;
			}
		}
		else
		{
			foreach (GameObject gameObject3 in this.authLight)
			{
				gameObject3.SetActive(true);
			}
		}
		this.time = this.GetTime();
	}

	private void ProjectLateUpdate()
	{
		if (this.auth == null)
		{
			return;
		}
		if (!this.isPlayed)
		{
			return;
		}
		foreach (AuthPlayer.AuthEffectData authEffectData in this.effectList)
		{
			if (authEffectData.Played && !(authEffectData.AuthObj == null))
			{
				authEffectData.Parent.transform.position = authEffectData.AuthObj.position;
				authEffectData.Parent.transform.rotation = authEffectData.AuthObj.rotation;
			}
		}
	}

	private AuthCharaData GetCharaData(int index, string kind)
	{
		AuthCharaData authCharaData = null;
		foreach (AuthCharaData authCharaData2 in this.charaList)
		{
			if (authCharaData2.Index == index)
			{
				authCharaData = authCharaData2;
				break;
			}
		}
		return authCharaData;
	}

	public AuthPlayer.AuthEffectData GetEffectParent(string name)
	{
		return this.effectList.Find((AuthPlayer.AuthEffectData item) => item.AuthObj.name == name);
	}

	public void PlayAuth(bool OldPlay = false)
	{
		if (this.auth == null)
		{
			return;
		}
		if (null == this.anim)
		{
			return;
		}
		this.fixKeepFps = Application.targetFrameRate;
		this.auth.GetComponent<MotionEventHandler>().animeState = (OldPlay ? null : this.anim["Custom"]);
		this.anim.ExPlayAnimation("Custom", null);
		this.isPlayed = true;
		this.isActFinishProcess = false;
		this.AuthCameraObj.SetActive(true);
		this.AuthCameraObjSub.SetActive(true);
		if (this.CharaCamera != null)
		{
			this.CharaCamera.gameObject.SetActive(false);
		}
		this.backupEffectBillboardCamera = EffectManager.BillboardCamera;
		EffectManager.BillboardCamera = this.AuthCameraObj.cam;
		foreach (GameObject gameObject in this.authLight)
		{
			gameObject.SetActive(true);
		}
		if (this.stagePresetCtrl != null)
		{
			this.backupPlayerLightMask = this.stagePresetCtrl.lightByPlayer.cullingMask;
			this.stagePresetCtrl.lightByPlayer.cullingMask |= 1 << LayerMask.NameToLayer("AuthMain");
			this.stagePresetCtrl.lightByPlayer.cullingMask |= 1 << LayerMask.NameToLayer("AuthMainAlpha");
			this.stagePresetCtrl.lightByPlayer.cullingMask |= 1 << LayerMask.NameToLayer("AuthChara");
			this.stagePresetCtrl.lightByPlayer.cullingMask |= 1 << LayerMask.NameToLayer("Camouflage");
			Transform transform = this.stagePresetCtrl.stageModelObj.transform.Find("AuthHideGroup");
			if (transform != null)
			{
				transform.gameObject.SetActive(false);
			}
			this.stagePresetCtrl.StageAuthHideManager.gameObject.SetActive(false);
		}
		this.ProjectUpdate();
		this.authRenderSetting.Param2Scene();
	}

	public void PlayFinishProcess()
	{
		if (this.isPlayed && !this.isActFinishProcess)
		{
			Application.targetFrameRate = this.fixKeepFps;
			this.fixKeepFps = 0;
			this.isActFinishProcess = true;
			this.AuthCameraObj.SetActive(false);
			this.AuthCameraObjSub.SetActive(false);
			if (this.CharaCamera != null)
			{
				this.CharaCamera.gameObject.SetActive(false);
			}
			foreach (AuthPlayer.AuthEffectData authEffectData in this.effectList)
			{
				if (authEffectData.effectData != null)
				{
					if (null != authEffectData.effectData.effectObject && authEffectData.effectData.effectObject.activeInHierarchy)
					{
						authEffectData.effectData.SetNormalizedTime(0f);
					}
					for (int i = 0; i < authEffectData.effectData.particleList.Count; i++)
					{
						authEffectData.effectData.particleList[i].Stop();
					}
					GameObject effectObject = authEffectData.effectData.effectObject;
					if (effectObject != null)
					{
						effectObject.SetActive(false);
					}
					authEffectData.Played = false;
				}
			}
			if (Singleton<EffectManager>.Instance != null)
			{
				EffectManager.BillboardCamera = this.backupEffectBillboardCamera;
			}
			foreach (GameObject gameObject in this.authLight)
			{
				gameObject.SetActive(false);
			}
			if (this.stagePresetCtrl != null)
			{
				this.stagePresetCtrl.lightByStage.enabled = true;
				this.stagePresetCtrl.lightByPlayer.enabled = true;
				this.stagePresetCtrl.lightByEnemy.enabled = true;
				this.stagePresetCtrl.gameObject.SetActive(true);
				this.stagePresetCtrl.lightByPlayer.cullingMask = this.backupPlayerLightMask;
				GameObject stageModelObj = this.stagePresetCtrl.stageModelObj;
				Transform transform = ((stageModelObj != null) ? stageModelObj.transform.Find("AuthHideGroup") : null);
				if (transform != null)
				{
					transform.gameObject.SetActive(true);
				}
				StagePutObjManager stageAuthHideManager = this.stagePresetCtrl.StageAuthHideManager;
				if (stageAuthHideManager != null)
				{
					stageAuthHideManager.gameObject.SetActive(true);
				}
			}
			this.backupRenderSetting.Param2Scene();
			this.anim.ExStop(false);
		}
	}

	public void StopSound()
	{
		if (this.auth != null)
		{
			SoundManager.Stop(AuthPlayer.GetSoundName(this.auth.name));
		}
	}

	public void Pause(bool pause)
	{
		this.pause = pause;
	}

	public void ForceFinish()
	{
		this.anim.ExStop(true);
		this.PlayFinishProcess();
	}

	public bool IsPlaying()
	{
		return this.auth != null && this.anim != null && this.anim.ExIsPlaying() && !this.pause;
	}

	public bool IsFinished()
	{
		return this.isPlayed && (this.anim == null || !this.anim.ExIsPlaying());
	}

	public GameObject GetCacheObj(string name)
	{
		if (!this.transCacheList.ContainsKey(name))
		{
			return null;
		}
		return this.transCacheList[name].gameObject;
	}

	public float GetLength()
	{
		if (!(this.anim == null))
		{
			return this.anim["Custom"].length;
		}
		return 0f;
	}

	public float GetTime()
	{
		if (!(this.anim == null))
		{
			return this.anim["Custom"].time;
		}
		return 0f;
	}

	public string GetFrameInfo()
	{
		if (this.anim == null)
		{
			return "0/0";
		}
		SimpleAnimation.State state = this.anim["Custom"];
		return ((int)(state.time * 30f)).ToString() + "/" + ((int)(state.length * 30f)).ToString();
	}

	public int GetCharaCount()
	{
		return this.charaCount;
	}

	public void PlayEffectByEventHandler(string effectName)
	{
		AuthPlayer.AuthEffectData authEffectData = this.effectList.Find((AuthPlayer.AuthEffectData item) => item.effectName == effectName);
		authEffectData.effectData.effectObject.SetActive(true);
		authEffectData.effectData.SetNormalizedTime(0f);
		for (int i = 0; i < authEffectData.effectData.particleList.Count; i++)
		{
			authEffectData.effectData.particleList[i].Play();
		}
		authEffectData.Played = true;
		if (AuthPlayer.MAX_ARTS_EFFECT_NAME.Find((string item) => effectName.StartsWith(item)) != null)
		{
			AuthPlayer.AuthEffectData authEffectData2 = this.effectList.Find((AuthPlayer.AuthEffectData item) => item.effectName == effectName + "_max");
			if (authEffectData2 != null)
			{
				authEffectData2.effectData.effectObject.SetActive(true);
				authEffectData2.effectData.SetNormalizedTime(0f);
				for (int j = 0; j < authEffectData2.effectData.particleList.Count; j++)
				{
					authEffectData2.effectData.particleList[j].Play();
				}
				authEffectData2.Played = true;
			}
		}
		foreach (AuthPlayer.AuthEffectData authEffectData3 in this.effectList.FindAll((AuthPlayer.AuthEffectData item) => item.isOption))
		{
			authEffectData3.effectData.effectObject.SetActive(true);
			authEffectData3.effectData.SetNormalizedTime(0f);
			for (int k = 0; k < authEffectData3.effectData.particleList.Count; k++)
			{
				authEffectData3.effectData.particleList[k].Play();
			}
			authEffectData3.Played = true;
		}
	}

	private const float FILM_APERTURE_H = 45.15556f;

	private const float FILM_APERTURE_V = 25.4f;

	private const float FILM_APERTURE_V_HALF = 0.12699999f;

	private const float RADIAN_TO_DEGREE = 57.295776f;

	private static readonly List<string> MAX_ARTS_EFFECT_NAME = new List<string> { "Ef_auth_pow_attack", "Ef_auth_pow_buff" };

	private Camera CharaCamera;

	private GameObject auth;

	private SimpleAnimation anim;

	public List<AuthCharaData> charaList;

	public Dictionary<string, string> modelChange;

	public List<GameObject> authLight;

	private AuthLightParam authLightParam;

	private List<AuthPlayer.AuthEffectData> effectList;

	public HashSet<string> efCtrlList;

	private List<string> loadLightAssetNameList;

	private RenderSettingParam authRenderSetting;

	private RenderSettingParam backupRenderSetting;

	private int fixKeepFps;

	private Dictionary<string, Transform> transCacheList;

	private Transform camPosObj;

	private Transform intrPosObj;

	private Transform fovObj;

	private Transform rollObj;

	private Transform technicolorObj;

	private Transform hueValueObj;

	private Transform radBlurObj;

	private Transform wiggleObj;

	private Transform rgbSpritObj;

	private Transform contrastVignetteObj;

	private Transform contrastVignetteRgbObj;

	private Transform contrastVignetteEdgeObj;

	private Transform doubleVisionObj;

	private Transform negativeObj;

	private Transform analogTVObj;

	private Transform focusLineObj;

	private Transform speedLineObj;

	private Transform blurObj;

	private Transform fogObj;

	private Transform camouflageObj;

	private string postEffectAssetName;

	public AuthPostEffectParam postEffectParam;

	private Vector3 offset;

	private float time;

	private bool pause;

	private bool prePause;

	private bool isFinishInitialize;

	private bool isPlayed;

	private bool isActFinishProcess;

	private int charaCount;

	private StagePresetCtrl stagePresetCtrl;

	private int stageDisableFrame = -1;

	private bool disableAuthFaceMotion;

	private int backupPlayerLightMask;

	private Camera backupEffectBillboardCamera;

	private const string CUSTOM_CLIP_NAME = "Custom";

	public bool isFinishDestroy = true;

	public bool camouflage;

	private static readonly string AUTH_NAME_PREFIX = "AT@";

	public int objIndex;

	public class PostEffectMotionCtrl
	{
		public bool techColor;

		public bool hueValue;

		public bool radBlur;

		public bool wiggle;

		public bool rgbSprit;

		public bool contrastVignette;

		public bool doubleVision;

		public bool negative;

		public bool analogTV;

		public bool focusLine;

		public bool speedLine;

		public bool blur;

		public bool fog;

		public bool camouflage;
	}

	public class AuthEffectData
	{
		public Transform AuthObj { get; set; }

		public GameObject Parent { get; set; }

		public bool Played { get; set; }

		public string effectName { get; set; }

		public EffectData effectData { get; set; }

		public bool isOption { get; set; }

		public AuthEffectData(Transform authObj, GameObject parent, string effectName, bool isOpt)
		{
			AuthPlayer.AuthEffectData <>4__this = this;
			this.AuthObj = authObj;
			this.Parent = parent;
			this.Played = false;
			this.isOption = isOpt;
			this.effectName = effectName;
			EffectManager.ReqLoadEffect(this.effectName, AssetManager.OWNER.EffectManager, 0, delegate(Data data)
			{
				<>4__this.effectData = EffectManager.InstantiateEffect(effectName, <>4__this.Parent.transform, 1, 1f);
				int num = LayerMask.NameToLayer("AuthMain");
				int num2 = LayerMask.NameToLayer("AuthSub");
				int num3 = LayerMask.NameToLayer("Camouflage");
				Transform[] componentsInChildren = <>4__this.effectData.effectObject.GetComponentsInChildren<Transform>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i].name.EndsWith("_refraction"))
					{
						componentsInChildren[i].gameObject.layer = num3;
						Renderer[] components = componentsInChildren[i].GetComponents<Renderer>();
						for (int j = 0; j < components.Length; j++)
						{
							foreach (Material material in components[j].materials)
							{
								material.SetFloat("_Camouflage", 0.9f);
								material.SetFloat("_Alpha", 1f);
							}
						}
					}
					else if (componentsInChildren[i].name.IndexOf("second") == 0)
					{
						componentsInChildren[i].gameObject.layer = num2;
					}
					else
					{
						componentsInChildren[i].gameObject.layer = num;
					}
				}
				<>4__this.effectData.SetActive(false);
			});
		}

		public bool IsFinishLoadEffect()
		{
			return this.effectData != null;
		}

		public void Destory()
		{
			if (this.IsFinishLoadEffect())
			{
				EffectManager.UnloadEffect(this.effectName, AssetManager.OWNER.EffectManager);
				EffectManager.DestroyEffect(this.effectData);
			}
		}
	}

	[Serializable]
	public class GachaParam
	{
		public enum SkyType
		{
			INVALID,
			NORMAL,
			NIGHT,
			NIGHT_STAR
		}

		public enum PutType
		{
			INVALID,
			NORMAL,
			LUCKY_BEAST,
			MIRAI,
			MIRAI_KAKO,
			MIRAI_NANA,
			MIRAI_CARRENDER,
			MIRAI_KAKO_NANA,
			MIRAI_KAKO_CARRENDER,
			MIRAI_NANA_CARRENDER,
			FULL_MEMBERS
		}

		public enum PostActType
		{
			INVALID,
			NORMAL,
			JUMP
		}

		public enum EffectType
		{
			INVALID,
			BLUE,
			GOLD,
			RAINBOW
		}

		public class Before
		{
			public AuthPlayer.GachaParam.SkyType skyType;

			public AuthPlayer.GachaParam.PutType putType;

			public AuthPlayer.GachaParam.PostActType postActType;

			public AuthPlayer.GachaParam.EffectType effectType;

			public StagePresetCtrl stageData;
		}

		public class After
		{
			public AuthPlayer.GachaParam.PutType putType;

			public AuthPlayer.GachaParam.EffectType effectType;

			public bool isPromotion;

			public int itemId;

			public StagePresetCtrl stageData;
		}
	}

	public class EffectConstants
	{
		public const string AuthEffectPlay = "AuthEffectPlay";

		public const string SoundPlay = "SoundPlay";

		public const string EfAuthGachaALuckyBeast = "Ef_auth_gacha_a_luckybeast";

		public const string PrdSeAtGcaPo0001LuckyBeast = "prd_se_at_gca_po_0001_luckybeast";

		public const string EfAuthGachaAKako = "Ef_auth_gacha_a_kako";

		public const string EfAuthGachaACarrender = "Ef_auth_gacha_a_carrender";

		public const string EfAuthGachaANana = "Ef_auth_gacha_a_nana";

		public const string EfAuthGachaAMirai = "Ef_auth_gacha_a_mirai";

		public const string EfAuthGachaBKako = "Ef_auth_gacha_b_kako";

		public const string EfAuthGachaBCarrender = "Ef_auth_gacha_b_carrender";

		public const string EfAuthGachaBNana = "Ef_auth_gacha_b_nana";

		public const string EfAuthGachaBMirai = "Ef_auth_gacha_b_mirai";
	}

	public class GachaAnimationEventsBefore
	{
		public List<ValueTuple<string, string>> miraiEventBefore = new List<ValueTuple<string, string>>
		{
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_luckybeast"),
			new ValueTuple<string, string>("SoundPlay", "prd_se_at_gca_po_0001_luckybeast"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_kako"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_carrender"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_nana")
		};

		public List<ValueTuple<string, string>> fullMembersEventsBefore = new List<ValueTuple<string, string>>
		{
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_luckybeast"),
			new ValueTuple<string, string>("SoundPlay", "prd_se_at_gca_po_0001_luckybeast")
		};

		public List<ValueTuple<string, string>> normalEventsBefore = new List<ValueTuple<string, string>>
		{
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_luckybeast"),
			new ValueTuple<string, string>("SoundPlay", "prd_se_at_gca_po_0001_luckybeast"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_kako"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_carrender"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_nana"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_mirai")
		};

		public List<ValueTuple<string, string>> luckyBeastEventsBefore = new List<ValueTuple<string, string>>
		{
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_mirai"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_kako"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_carrender"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_nana")
		};

		public List<ValueTuple<string, string>> miraiNanaEventsBefore = new List<ValueTuple<string, string>>
		{
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_luckybeast"),
			new ValueTuple<string, string>("SoundPlay", "prd_se_at_gca_po_0001_luckybeast"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_kako"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_carrender")
		};

		public List<ValueTuple<string, string>> miraiCarrenderEventsBefore = new List<ValueTuple<string, string>>
		{
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_luckybeast"),
			new ValueTuple<string, string>("SoundPlay", "prd_se_at_gca_po_0001_luckybeast"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_kako"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_nana")
		};

		public List<ValueTuple<string, string>> miraiNanaCarrenderEventsBefore = new List<ValueTuple<string, string>>
		{
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_luckybeast"),
			new ValueTuple<string, string>("SoundPlay", "prd_se_at_gca_po_0001_luckybeast"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_kako")
		};

		public List<ValueTuple<string, string>> miraiKakoEventsBefore = new List<ValueTuple<string, string>>
		{
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_luckybeast"),
			new ValueTuple<string, string>("SoundPlay", "prd_se_at_gca_po_0001_luckybeast"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_carrender"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_nana")
		};

		public List<ValueTuple<string, string>> miraiKakoNanaEventsBefore = new List<ValueTuple<string, string>>
		{
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_luckybeast"),
			new ValueTuple<string, string>("SoundPlay", "prd_se_at_gca_po_0001_luckybeast"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_carrender")
		};

		public List<ValueTuple<string, string>> miraiKakoCarrenderEventsBefore = new List<ValueTuple<string, string>>
		{
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_luckybeast"),
			new ValueTuple<string, string>("SoundPlay", "prd_se_at_gca_po_0001_luckybeast"),
			new ValueTuple<string, string>("AuthEffectPlay", "Ef_auth_gacha_a_nana")
		};
	}

	public class GachaAnimationEventsAfter
	{
		public List<string> miraiEventsAfter = new List<string> { "Ef_auth_gacha_b_kako", "Ef_auth_gacha_b_carrender", "Ef_auth_gacha_b_nana" };

		public List<string> fullMembersEventsAfter = new List<string>();

		public List<string> normalEventsAfter = new List<string> { "Ef_auth_gacha_b_kako", "Ef_auth_gacha_b_carrender", "Ef_auth_gacha_b_nana", "Ef_auth_gacha_b_mirai" };

		public List<string> luckyBeastEventsAfter = new List<string> { "Ef_auth_gacha_b_mirai", "Ef_auth_gacha_b_kako", "Ef_auth_gacha_b_carrender", "Ef_auth_gacha_b_nana" };

		public List<string> miraiNanaEventsAfter = new List<string> { "Ef_auth_gacha_b_kako", "Ef_auth_gacha_b_carrender" };

		public List<string> miraiCarrenderEventsAfter = new List<string> { "Ef_auth_gacha_b_kako", "Ef_auth_gacha_b_nana" };

		public List<string> miraiNanaCarrenderEventsAfter = new List<string> { "Ef_auth_gacha_b_kako" };

		public List<string> miraiKakoEventsAfter = new List<string> { "Ef_auth_gacha_b_carrender", "Ef_auth_gacha_b_nana" };

		public List<string> miraiKakoNanaEventsAfter = new List<string> { "Ef_auth_gacha_b_carrender" };

		public List<string> miraiKakoCarrenderEventsAfter = new List<string> { "Ef_auth_gacha_b_nana" };
	}
}
