using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Mst;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

// Token: 0x02000047 RID: 71
public class CharaModelHandle : MonoBehaviour
{
	// Token: 0x17000026 RID: 38
	// (get) Token: 0x0600016A RID: 362 RVA: 0x0000B69E File Offset: 0x0000989E
	// (set) Token: 0x0600016B RID: 363 RVA: 0x0000B6A6 File Offset: 0x000098A6
	public bool enabledFaceMotion { get; set; }

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x0600016C RID: 364 RVA: 0x0000B6AF File Offset: 0x000098AF
	// (set) Token: 0x0600016D RID: 365 RVA: 0x0000B6B7 File Offset: 0x000098B7
	public CharaModelHandle.EyeMotType eyeMotionType { get; set; }

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x0600016E RID: 366 RVA: 0x0000B6C0 File Offset: 0x000098C0
	// (set) Token: 0x0600016F RID: 367 RVA: 0x0000B6C8 File Offset: 0x000098C8
	public Transform headFollowObj { get; set; }

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000170 RID: 368 RVA: 0x0000B6D1 File Offset: 0x000098D1
	// (set) Token: 0x06000171 RID: 369 RVA: 0x0000B6D9 File Offset: 0x000098D9
	public Transform eyeFollowObj { get; set; }

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x06000172 RID: 370 RVA: 0x0000B6E2 File Offset: 0x000098E2
	// (set) Token: 0x06000173 RID: 371 RVA: 0x0000B6EA File Offset: 0x000098EA
	public Transform mouthFollowObj { get; set; }

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000174 RID: 372 RVA: 0x0000B6F3 File Offset: 0x000098F3
	// (set) Token: 0x06000175 RID: 373 RVA: 0x0000B6FB File Offset: 0x000098FB
	public Vector3 eyeAngleL { get; set; }

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000176 RID: 374 RVA: 0x0000B704 File Offset: 0x00009904
	// (set) Token: 0x06000177 RID: 375 RVA: 0x0000B70C File Offset: 0x0000990C
	public Vector3 eyeAngleR { get; set; }

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000178 RID: 376 RVA: 0x0000B715 File Offset: 0x00009915
	// (set) Token: 0x06000179 RID: 377 RVA: 0x0000B71D File Offset: 0x0000991D
	public float mouthAngle { get; set; }

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x0600017A RID: 378 RVA: 0x0000B726 File Offset: 0x00009926
	// (set) Token: 0x0600017B RID: 379 RVA: 0x0000B72E File Offset: 0x0000992E
	public bool haraOffset { get; set; }

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x0600017C RID: 380 RVA: 0x0000B737 File Offset: 0x00009937
	// (set) Token: 0x0600017D RID: 381 RVA: 0x0000B73F File Offset: 0x0000993F
	public CharaModelHandle.InitializeParam initializeParam { get; private set; }

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x0600017E RID: 382 RVA: 0x0000B748 File Offset: 0x00009948
	// (set) Token: 0x0600017F RID: 383 RVA: 0x0000B750 File Offset: 0x00009950
	public string modelName { get; private set; }

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000180 RID: 384 RVA: 0x0000B759 File Offset: 0x00009959
	// (set) Token: 0x06000181 RID: 385 RVA: 0x0000B761 File Offset: 0x00009961
	public string loadVoiceCueSheetName { get; private set; }

	// Token: 0x06000182 RID: 386 RVA: 0x0000B76C File Offset: 0x0000996C
	public static CharaModelHandle.BlendShapeIndex MakeBlendShapeIndex(string flagName, int xyzIndex)
	{
		int num = new List<string>(CharaModelHandle.FLAG_LIST).IndexOf(flagName);
		if (num >= 0 && xyzIndex >= 0)
		{
			return CharaModelHandle.FACE_LIST[num, xyzIndex];
		}
		return CharaModelHandle.BlendShapeIndex.Max;
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000183 RID: 387 RVA: 0x0000B7A1 File Offset: 0x000099A1
	// (set) Token: 0x06000184 RID: 388 RVA: 0x0000B7A9 File Offset: 0x000099A9
	public float shadowSize { get; set; }

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000185 RID: 389 RVA: 0x0000B7B2 File Offset: 0x000099B2
	// (set) Token: 0x06000186 RID: 390 RVA: 0x0000B7BA File Offset: 0x000099BA
	public float shadowHeight { get; set; }

	// Token: 0x06000187 RID: 391 RVA: 0x0000B7C4 File Offset: 0x000099C4
	private string skirt2long(string str)
	{
		KeyValuePair<string, string> keyValuePair = this.longSkirt.Find((KeyValuePair<string, string> itm) => itm.Key == str);
		if (!keyValuePair.Equals(default(KeyValuePair<string, string>)))
		{
			str = keyValuePair.Value;
		}
		return str;
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0000B828 File Offset: 0x00009A28
	private string skirt2short(string str)
	{
		KeyValuePair<string, string> keyValuePair = this.longSkirt.Find((KeyValuePair<string, string> itm) => itm.Value == str);
		if (!keyValuePair.Equals(default(KeyValuePair<string, string>)))
		{
			str = keyValuePair.Key;
		}
		return str;
	}

	// Token: 0x06000189 RID: 393 RVA: 0x0000B88C File Offset: 0x00009A8C
	public void Initialize(int charaId, bool isShadow = true, bool isShadowModel = false, int clothImageId = 0, bool longSkirt = false, bool isMotionSe = false, bool isDisableVoice = false, DataManagerCharaAccessory.Accessory accessory = null)
	{
		CharaModelHandle.InitializeParam initializeParam = CharaModelHandle.InitializeParam.CreaateByCharaId(charaId, clothImageId, longSkirt, isShadow);
		initializeParam.isShadowModel = isShadowModel;
		initializeParam.isEnableMotionSE = isMotionSe;
		initializeParam.isDisableVoice = isDisableVoice;
		initializeParam.accessory = accessory;
		this.Initialize(initializeParam);
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0000B8CC File Offset: 0x00009ACC
	public void Initialize(CharaPackData cpd, bool isShadow = true, bool isShadowModel = false, bool isMotionSe = false)
	{
		CharaModelHandle.InitializeParam initializeParam = CharaModelHandle.InitializeParam.CreaateByCharaId(cpd.id, cpd.equipClothImageId, cpd.equipLongSkirt, isShadow);
		initializeParam.isShadowModel = isShadowModel;
		initializeParam.isEnableMotionSE = isMotionSe;
		initializeParam.accessory = (cpd.dynamicData.dispAccessoryEffect ? cpd.dynamicData.accessory : null);
		this.Initialize(initializeParam);
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0000B92C File Offset: 0x00009B2C
	public void Initialize(CharaModelHandle.InitializeParam initParam)
	{
		this.DestoryInternal();
		this.initializeParam = initParam;
		this.isFinishInit = false;
		this.enabledFaceMotion = true;
		this.eyeMotionType = CharaModelHandle.EyeMotType.ENABLE_CHARA;
		this.headFollowObj = null;
		this.eyeFollowObj = null;
		this.mouthFollowObj = null;
		this.eyeAngleL = (this.eyeAngleR = Vector3.zero);
		this.mouthAngle = 0f;
		this.headRotationOld = Quaternion.identity;
		this.eyeAngleOldL = (this.eyeAngleOldR = Vector2.zero);
		this.mouthAngleOld = 0f;
		this.headFollowFade = (this.eyeFollowFade = (this.mouthFollowFade = 0f));
		this.layer = initParam.layer;
		this.eyeColor = Color.clear;
		this.cheekColor = Color.white;
		this.partsColor = new List<Color>();
		this.partsAnim = new List<float>();
		this.partsTime = new List<float>();
		this.eyeBall = new List<Renderer>();
		this.eyeBallL = (this.eyeBallR = (this.eyeCtrlL = (this.eyeCtrlR = null)));
		this.cheek = null;
		this.parts = new List<Renderer>();
		this.mouthCtrl = null;
		this.headCtrl = null;
		this.mdlSubweapon = null;
		this.bonSubweapon = null;
		this.flagList = new List<Transform>();
		this.blendShapeList = new List<CharaModelHandle.BlendShapeData>();
		this.modelList = new Dictionary<string, List<GameObject>>();
		this.weaponList = new List<GameObject>();
		this.bonWeaponA = null;
		this.bonWeaponB = null;
		this.motionReq = new CharaModelHandle.MotionReq();
		this.motionReq.name = "";
		this.motionName = "";
		this.motionLoop = false;
		this.motionLength = (this.motionTime = (this.motionSpeed = 0f));
		this.motionTimeReq = -1f;
		this.motionEnd = false;
		this.motionPos.y = -1f;
		this.motionRot = -1f;
		this.haraOffset = false;
		this.haraOffOld = false;
		this.cullingList = new List<CullingCheck>();
		this.materials = new List<Material>();
		this.alpha = 1f;
		this.NeighboringAlpha = new Vector4(0f, -1f, 0f, 1f);
		this.matCap = -1f;
		this.camouflage = false;
		this.camouflageAlpha = 0f;
		this.pelvis = null;
		this.upperBody = (this.lowerBody = null);
		this.thighL = (this.thighR = null);
		this.wrist = new List<Transform>();
		this.crossBone = new Dictionary<Transform, Quaternion>();
		this.crossBoneL = new Dictionary<Transform, Quaternion>();
		this.crossRoot = new Dictionary<Transform, Vector3>();
		this.crossWeapon = new Dictionary<Transform, Transform>();
		this.crossFade = (this.crossFadeL = 0f);
		this.shadow = null;
		this.shadowMat = null;
		this.shadowSize = 1f;
		this.shadowHeight = 0.01f;
		this.shadowModel = null;
		this.shadowBone = null;
		this.shadowCull = null;
		this.shadowCamera = null;
		this.effect = new List<KeyValuePair<EffectData, Vector3>>();
		this.weaponCmnOff = new List<KeyValuePair<Transform, Transform>>();
		this.loadVoiceCueSheetName = "";
		this.authPlayer = null;
		this.puyoObj = null;
		this.puyoTex = null;
		this.puyoReq = null;
		this.puyoBreath = 0f;
		this.initializeRoutine = this.InitializeInternal(this.initializeParam.bodyModelName, this.initializeParam.isShadow, this.initializeParam.isShadowModel, this.initializeParam.isDisableVoice, this.initializeParam.accessory);
		this.initializeRoutine.MoveNext();
	}

	// Token: 0x0600018C RID: 396 RVA: 0x0000BCDC File Offset: 0x00009EDC
	private static List<string> MakeOptionModelName(string bodyModelName, string optionSuffix)
	{
		List<string> list = new List<string>();
		string text = bodyModelName.Substring(0, bodyModelName.LastIndexOf("_") + 1);
		string text2 = bodyModelName + optionSuffix;
		if (AssetManager.IsExsistAssetData(AssetManager.PREFIX_PATH_CHARA_MODEL + text2))
		{
			list.Add(text2);
		}
		if (list.Count <= 0)
		{
			text2 = text + "a" + optionSuffix;
			if (AssetManager.IsExsistAssetData(AssetManager.PREFIX_PATH_CHARA_MODEL + text2))
			{
				list.Add(text2);
			}
		}
		if (list.Count > 0)
		{
			list.Add("");
			text2 = text + "x" + optionSuffix;
			list.Add(AssetManager.IsExsistAssetData(AssetManager.PREFIX_PATH_CHARA_MODEL + text2) ? text2 : "");
			text2 = text + "y" + optionSuffix;
			list.Add(AssetManager.IsExsistAssetData(AssetManager.PREFIX_PATH_CHARA_MODEL + text2) ? text2 : "");
			text2 = text + "z" + optionSuffix;
			list.Add(AssetManager.IsExsistAssetData(AssetManager.PREFIX_PATH_CHARA_MODEL + text2) ? text2 : "");
		}
		return list;
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0000BDF3 File Offset: 0x00009FF3
	private IEnumerator InitializeInternal(string bodyModelName, bool isShadow, bool isShadowModel, bool isDisableVoice, DataManagerCharaAccessory.Accessory accessory)
	{
		string luckyPath = null;
		bool isNeedShadowUp = false;
		if (bodyModelName.IndexOf("_1004_") > 0 || bodyModelName.IndexOf("_1005_") > 0 || bodyModelName.IndexOf("_1025_") > 0 || bodyModelName.IndexOf("_1032_") > 0)
		{
			int num = bodyModelName.Length - 1;
			int num2 = (int)(bodyModelName[num] - 'a');
			if (num2 == 1)
			{
				luckyPath = "Effects/info/Ef_info_stump";
			}
			else if (num2 == 2)
			{
				luckyPath = "Effects/info/Ef_info_rock";
			}
			else if (num2 == 3)
			{
				luckyPath = "Effects/info/Ef_info_cardboard";
			}
			bodyModelName = bodyModelName.Substring(0, num) + "a";
		}
		else if (bodyModelName.IndexOf("_1013_") > 0)
		{
			int num3 = bodyModelName.Length - 1;
			if (bodyModelName[num3] - 'a' == '\u0001')
			{
				luckyPath = "Effects/info/Ef_info_stand_kitty";
			}
			bodyModelName = bodyModelName.Substring(0, num3) + "a";
		}
		else if (bodyModelName.IndexOf("_1014_") > 0)
		{
			int num4 = bodyModelName.Length - 1;
			if (bodyModelName[num4] - 'a' == '\u0001')
			{
				luckyPath = "Effects/info/Ef_info_stand_mimmy";
			}
			bodyModelName = bodyModelName.Substring(0, num4) + "a";
		}
		else if (bodyModelName.IndexOf("_1034_") > 0)
		{
			int num5 = bodyModelName.Length - 1;
			if (bodyModelName[num5] - 'a' == '\u0001')
			{
				luckyPath = "Effects/info/Ef_info_stand_wood";
			}
			bodyModelName = bodyModelName.Substring(0, num5) + "a";
		}
		else if (bodyModelName.IndexOf("_1038_") > 0)
		{
			int num6 = bodyModelName.Length - 1;
			if (bodyModelName[num6] - 'a' == '\u0001')
			{
				luckyPath = "Effects/info/Ef_info_stand_wood";
			}
			bodyModelName = bodyModelName.Substring(0, num6) + "a";
			isNeedShadowUp = true;
		}
		else if (bodyModelName.IndexOf("_1060_") > 0)
		{
			int num7 = bodyModelName.Length - 1;
			if (bodyModelName[num7] - 'a' == '\u0001')
			{
				luckyPath = "Effects/info/Ef_info_stand_wood";
			}
			bodyModelName = bodyModelName.Substring(0, num7) + "a";
		}
		if (DataManager.DmChara != null)
		{
			MstCharaEffectData charaEffectData = DataManager.DmChara.GetCharaEffectData(bodyModelName);
			if (charaEffectData != null)
			{
				this.charaEffectType = (CharaModelHandle.CharaEffectType)charaEffectData.effectType;
				if (this.charaEffectType == CharaModelHandle.CharaEffectType.AURA)
				{
					int num8 = bodyModelName.IndexOf(charaEffectData.effectName);
					char c3 = bodyModelName[num8 + 5];
					if (c3 >= '0' && c3 <= '5')
					{
						this.charaEffect = new List<EffectData>();
					}
				}
				else
				{
					this.charaEffect = new List<EffectData>();
				}
				this.charaEffectName = charaEffectData.effectName;
			}
		}
		this.modelName = bodyModelName;
		this.assetModelName = bodyModelName;
		this.assetIsShadow = isShadow;
		string bodyModelPath = CharaModelHandle.CHARA_MODEL_PATH + bodyModelName;
		if (this.assetModelName.IndexOf("_1015_") > 0 || this.assetModelName.IndexOf("_1016_") > 0)
		{
			AssetManager.LoadAssetData(bodyModelPath, AssetManager.OWNER.CharaModel, 0, null);
			yield return null;
			while (!AssetManager.IsLoadFinishAssetData(bodyModelPath))
			{
				yield return null;
			}
			this.puyoObj = AssetManager.InstantiateAssetData(bodyModelPath, base.transform);
			this.puyoObj.transform.localPosition = Vector3.zero;
			this.puyoObj.transform.eulerAngles = new Vector3(0f, 180f, 0f);
			this.puyoObj.transform.localScale = Vector3.one;
			this.puyoObj.SetActive(true);
			yield break;
		}
		List<string> list = CharaModelHandle.MakeOptionModelName(bodyModelName, CharaModelHandle.OPTION_SUFFIX_EAR);
		List<string> earModelPath = new List<string>();
		foreach (string text in list)
		{
			earModelPath.Add(string.IsNullOrEmpty(text) ? "" : (CharaModelHandle.CHARA_MODEL_PATH + text));
		}
		List<string> list2 = CharaModelHandle.MakeOptionModelName(bodyModelName, CharaModelHandle.OPTION_SUFFIX_TAIL);
		List<string> tailModelPath = new List<string>();
		foreach (string text2 in list2)
		{
			tailModelPath.Add(string.IsNullOrEmpty(text2) ? "" : (CharaModelHandle.CHARA_MODEL_PATH + text2));
		}
		string shadowPath = "Effects/info/Ef_info_shadow";
		AssetManager.LoadAssetData(bodyModelPath, AssetManager.OWNER.CharaModel, 0, null);
		foreach (string text3 in earModelPath)
		{
			if (!string.IsNullOrEmpty(text3))
			{
				AssetManager.LoadAssetData(text3, AssetManager.OWNER.CharaModel, 0, null);
			}
		}
		foreach (string text4 in tailModelPath)
		{
			if (!string.IsNullOrEmpty(text4))
			{
				AssetManager.LoadAssetData(text4, AssetManager.OWNER.CharaModel, 0, null);
			}
		}
		if (isShadow)
		{
			AssetManager.LoadAssetData(shadowPath, AssetManager.OWNER.CharaModel, 0, null);
		}
		if (!string.IsNullOrEmpty(luckyPath))
		{
			AssetManager.LoadAssetData(luckyPath, AssetManager.OWNER.CharaModel, 0, null);
		}
		if (this.charaEffect != null)
		{
			EffectManager.ReqLoadEffect(this.charaEffectName, AssetManager.OWNER.CharaModel, 0, null);
		}
		this.accEff = new List<CharaModelHandle.AccEff>();
		if (accessory != null)
		{
			foreach (DataManagerCharaAccessory.DispData dispData in accessory.AccessoryData.DispDataList)
			{
				CharaModelHandle.AccEff accEff = null;
				if (!string.IsNullOrEmpty(dispData.Name))
				{
					accEff = new CharaModelHandle.AccEff
					{
						typ = this.accEff.Count,
						name = dispData.Name,
						pos = dispData.Position,
						dsp = false,
						eff = null
					};
					EffectManager.ReqLoadEffect(accEff.name, AssetManager.OWNER.CharaModel, 0, null);
				}
				this.accEff.Add(accEff);
			}
			this.isAccessoryAnchor = accessory.AccessoryData.IsAnchor;
		}
		while (this.accEff.Count < 4)
		{
			this.accEff.Add(null);
		}
		yield return null;
		while (!AssetManager.IsLoadFinishAssetData(bodyModelPath))
		{
			yield return null;
		}
		for (;;)
		{
			bool flag = true;
			foreach (string text5 in earModelPath)
			{
				if (!string.IsNullOrEmpty(text5))
				{
					flag &= AssetManager.IsLoadFinishAssetData(text5);
				}
			}
			if (flag)
			{
				break;
			}
			yield return null;
		}
		for (;;)
		{
			bool flag2 = true;
			foreach (string text6 in tailModelPath)
			{
				if (!string.IsNullOrEmpty(text6))
				{
					flag2 &= AssetManager.IsLoadFinishAssetData(text6);
				}
			}
			if (flag2)
			{
				break;
			}
			yield return null;
		}
		while (isShadow)
		{
			if (AssetManager.IsLoadFinishAssetData(shadowPath))
			{
				break;
			}
			yield return null;
		}
		while (!string.IsNullOrEmpty(luckyPath))
		{
			if (AssetManager.IsLoadFinishAssetData(luckyPath))
			{
				break;
			}
			yield return null;
		}
		while (this.charaEffect != null && !EffectManager.IsLoadFinishEffect(this.charaEffectName))
		{
			yield return null;
		}
		foreach (CharaModelHandle.AccEff ae in this.accEff)
		{
			while (ae != null && !EffectManager.IsLoadFinishEffect(ae.name))
			{
				yield return null;
			}
			ae = null;
		}
		List<CharaModelHandle.AccEff>.Enumerator enumerator3 = default(List<CharaModelHandle.AccEff>.Enumerator);
		this.partsDataBody = new CharaModelHandle.PartsData(AssetManager.InstantiateAssetData(bodyModelPath, base.transform), true, bodyModelName);
		this.partsDataBody.rootObj.SetActive(false);
		this.partsDataBody.rootObj.name = bodyModelName;
		for (int i = 0; i < this.partsDataEar.Length; i++)
		{
			this.partsDataEar[i] = null;
		}
		for (int j = 0; j < this.partsDataTail.Length; j++)
		{
			this.partsDataTail[j] = null;
		}
		if (isDisableVoice)
		{
			this.loadVoiceCueSheetName = "";
		}
		else
		{
			string text7 = bodyModelName;
			if (this.partsDataBody.referencer.ussVariantCopyPrefabVoice && !string.IsNullOrEmpty(this.partsDataBody.referencer.variantCopyPrefabName))
			{
				text7 = this.partsDataBody.referencer.variantCopyPrefabName;
			}
			string[] array = text7.Split('_', StringSplitOptions.None);
			if (array.Length > 1)
			{
				int num9 = 0;
				if (int.TryParse(array[1], out num9))
				{
					this.loadVoiceCueSheetName = "";
					if (num9 >= 10000)
					{
						num9 /= 10;
						this.loadVoiceCueSheetName = "0";
					}
					this.loadVoiceCueSheetName = "cv_" + num9.ToString("0000") + this.loadVoiceCueSheetName;
					if (SoundManager.IsExsistCueSheetAssetData(this.loadVoiceCueSheetName))
					{
						IEnumerator soundDownload = SoundManager.LoadCueSheetWithDownload(this.loadVoiceCueSheetName);
						while (soundDownload.MoveNext())
						{
							yield return null;
						}
						soundDownload = null;
					}
				}
			}
		}
		this.childAll = new List<Transform>(this.partsDataBody.rootObj.transform.GetComponentsInChildren<Transform>(true));
		this.childEar = new List<Transform>();
		this.childTail = new List<Transform>();
		string earNode = this.partsDataBody.referencer.refAnimationObj.GetComponent<CharaModelReferencer>().earNodeName;
		if (string.IsNullOrEmpty(earNode))
		{
			earNode = "j_head";
		}
		int num10 = 0;
		Predicate<Transform> <>9__3;
		while (num10 < earModelPath.Count && num10 < this.partsDataEar.Length)
		{
			if (!string.IsNullOrEmpty(earModelPath[num10]))
			{
				CharaModelHandle.PartsData[] array2 = this.partsDataEar;
				int num11 = num10;
				string text8 = earModelPath[num10];
				List<Transform> list3 = this.childAll;
				Predicate<Transform> predicate;
				if ((predicate = <>9__3) == null)
				{
					predicate = (<>9__3 = (Transform item) => item.name == earNode);
				}
				array2[num11] = new CharaModelHandle.PartsData(AssetManager.InstantiateAssetData(text8, list3.Find(predicate)), false, null);
			}
			num10++;
		}
		if (this.partsDataEar[0] != null)
		{
			this.childEar = new List<Transform>(this.partsDataEar[0].rootObj.transform.GetComponentsInChildren<Transform>(true));
		}
		string tailNode = this.partsDataBody.referencer.refAnimationObj.GetComponent<CharaModelReferencer>().tailNodeName;
		if (string.IsNullOrEmpty(tailNode))
		{
			tailNode = "j_lowerbody";
		}
		int num12 = 0;
		Predicate<Transform> <>9__4;
		while (num12 < tailModelPath.Count && num12 < this.partsDataTail.Length)
		{
			if (!string.IsNullOrEmpty(tailModelPath[num12]))
			{
				CharaModelHandle.PartsData[] array3 = this.partsDataTail;
				int num13 = num12;
				string text9 = tailModelPath[num12];
				List<Transform> list4 = this.childAll;
				Predicate<Transform> predicate2;
				if ((predicate2 = <>9__4) == null)
				{
					predicate2 = (<>9__4 = (Transform item) => item.name == tailNode);
				}
				array3[num13] = new CharaModelHandle.PartsData(AssetManager.InstantiateAssetData(text9, list4.Find(predicate2)), false, null);
			}
			num12++;
		}
		if (this.partsDataTail[0] != null)
		{
			this.childTail = new List<Transform>(this.partsDataTail[0].rootObj.transform.GetComponentsInChildren<Transform>(true));
		}
		if (isShadow)
		{
			this.shadow = AssetManager.InstantiateAssetData(shadowPath, base.transform);
			this.shadow.SetActive(false);
			this.shadowMat = this.shadow.transform.GetComponentInChildren<MeshRenderer>(true).material;
			foreach (Renderer renderer in this.shadow.GetComponentsInChildren<Renderer>(true))
			{
				renderer.shadowCastingMode = ShadowCastingMode.Off;
				renderer.receiveShadows = false;
			}
		}
		this.weaponCmnOff = new List<KeyValuePair<Transform, Transform>>();
		using (List<CharaModelHandle.WeaponOffsetParam>.Enumerator enumerator4 = CharaModelHandle.WeaponOffsetList.GetEnumerator())
		{
			while (enumerator4.MoveNext())
			{
				CharaModelHandle.WeaponOffsetParam wop = enumerator4.Current;
				Transform transform = this.childAll.Find((Transform item) => item.name.StartsWith("md_weapon") && item.name.EndsWith(wop.weaponSuffix));
				if (!(transform == null))
				{
					transform = this.childAll.Find((Transform item) => item.name.StartsWith(wop.offsetRootNode));
					if (!(transform == null))
					{
						Transform transform2 = new GameObject(wop.offsetRootNode + "_sp").transform;
						transform2.SetParent(transform, true);
						transform2.localPosition = wop.offsetPos;
						transform2.localRotation = wop.offsetRot;
						this.weaponCmnOff.Add(new KeyValuePair<Transform, Transform>(transform, transform2));
					}
				}
			}
		}
		if (!string.IsNullOrEmpty(luckyPath))
		{
			this.shadowModel = AssetManager.InstantiateAssetData(luckyPath, base.transform);
			this.shadowModel.SetActive(false);
			if (isNeedShadowUp)
			{
				this.shadowModel.transform.localPosition = new Vector3(0f, 0.8f, 0f);
			}
			Renderer componentInChildren = this.shadowModel.GetComponentInChildren<Renderer>(true);
			if (componentInChildren != null)
			{
				this.shadowCull = componentInChildren.gameObject.AddComponent<CullingCheck>();
				componentInChildren.shadowCastingMode = ShadowCastingMode.Off;
				componentInChildren.receiveShadows = false;
			}
			AssetManager.UnloadAssetData(luckyPath, AssetManager.OWNER.CharaModel);
		}
		else if (isShadowModel)
		{
			this.shadowModel = Object.Instantiate<GameObject>(this.partsDataBody.rootObj);
			CharaModelReferencer[] componentsInChildren = this.shadowModel.GetComponentsInChildren<CharaModelReferencer>(true);
			for (int k = 0; k < componentsInChildren.Length; k++)
			{
				Object.Destroy(componentsInChildren[k]);
			}
			SimpleAnimation[] componentsInChildren2 = this.shadowModel.GetComponentsInChildren<SimpleAnimation>(true);
			for (int k = 0; k < componentsInChildren2.Length; k++)
			{
				Object.Destroy(componentsInChildren2[k]);
			}
			Animator[] componentsInChildren3 = this.shadowModel.GetComponentsInChildren<Animator>(true);
			for (int k = 0; k < componentsInChildren3.Length; k++)
			{
				Object.Destroy(componentsInChildren3[k]);
			}
			Osage[] componentsInChildren4 = this.shadowModel.GetComponentsInChildren<Osage>(true);
			for (int k = 0; k < componentsInChildren4.Length; k++)
			{
				Object.Destroy(componentsInChildren4[k]);
			}
			this.shadowModel.transform.SetParent(base.transform, false);
			this.shadowModel.name = bodyModelName + "(Shadow)";
			List<GameObject> list5 = new List<GameObject>();
			foreach (Renderer renderer2 in this.shadowModel.GetComponentsInChildren<Renderer>(true))
			{
				if (renderer2.name == "model" || renderer2.name == "body" || renderer2.name == "md_body" || renderer2.name == "md_face")
				{
					renderer2.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
					renderer2.receiveShadows = false;
				}
				else
				{
					list5.Add(renderer2.gameObject);
				}
			}
			this.shadowBone = new List<KeyValuePair<Transform, Transform>>();
			List<Transform> list6 = new List<Transform>(this.partsDataBody.rootObj.transform.GetComponentsInChildren<Transform>(true));
			Transform[] array5 = this.shadowModel.GetComponentsInChildren<Transform>(true);
			for (int k = 0; k < array5.Length; k++)
			{
				Transform o = array5[k];
				if (o.name.StartsWith("flag_"))
				{
					list5.Add(o.gameObject);
				}
				else if (o.name.StartsWith("eye_"))
				{
					list5.Add(o.gameObject);
				}
				else if (o.name == "pelvis" || o.name.StartsWith("j_"))
				{
					Transform transform3 = list6.Find((Transform itm) => itm.name == o.name);
					if (transform3 == null)
					{
						list5.Add(o.gameObject);
					}
					else
					{
						this.shadowBone.Add(new KeyValuePair<Transform, Transform>(o, transform3));
					}
				}
			}
			foreach (GameObject gameObject in list5)
			{
				Object.Destroy(gameObject);
			}
		}
		yield return null;
		Transform transform4 = null;
		List<SkinnedMeshRenderer> list7 = new List<SkinnedMeshRenderer>();
		foreach (Transform transform5 in this.childAll)
		{
			if (transform5.name == "md_eye_l")
			{
				this.eyeBallL = transform5;
				this.eyeBall.Add(transform5.GetComponent<Renderer>());
			}
			else if (transform5.name == "md_eye_r")
			{
				this.eyeBallR = transform5;
				this.eyeBall.Add(transform5.GetComponent<Renderer>());
			}
			else if (transform5.name == "j_eye_l")
			{
				this.eyeCtrlL = transform5;
			}
			else if (transform5.name == "j_eye_r")
			{
				this.eyeCtrlR = transform5;
			}
			else if (transform5.name == "md_cheek")
			{
				this.cheek = transform5.GetComponent<Renderer>();
			}
			else if (transform5.name.StartsWith("parts"))
			{
				this.parts.Add(transform5.GetComponent<Renderer>());
			}
			else if (transform5.name == "root")
			{
				transform4 = transform5;
			}
			else if (transform5.name == "j_mouth")
			{
				this.mouthCtrl = transform5;
			}
			else if (transform5.name == "j_head")
			{
				this.headCtrl = transform5;
			}
			else if (transform5.name == "pelvis")
			{
				this.pelvis = transform5;
			}
			else if (transform5.name == "j_upperbody")
			{
				this.upperBody = transform5;
			}
			else if (transform5.name == "j_lowerbody")
			{
				this.lowerBody = transform5;
			}
			else if (transform5.name == "md_subweapon")
			{
				this.mdlSubweapon = transform5;
			}
			else if (transform5.name == "subweapon")
			{
				this.bonSubweapon = transform5;
			}
			else if (transform5.name == "j_weapon_a")
			{
				this.bonWeaponA = transform5;
			}
			else if (transform5.name == "j_weapon_b")
			{
				this.bonWeaponB = transform5;
			}
			else if (transform5.name == "j_thigh_l")
			{
				this.thighL = transform5;
			}
			else if (transform5.name == "j_thigh_r")
			{
				this.thighR = transform5;
			}
			else if (transform5.name.StartsWith("j_index_a"))
			{
				this.wrist.Add(transform5);
			}
			SkinnedMeshRenderer component = transform5.GetComponent<SkinnedMeshRenderer>();
			if (component != null)
			{
				list7.Add(component);
			}
		}
		if (this.bonSubweapon == null)
		{
			this.bonSubweapon = this.childEar.Find((Transform itm) => itm.name == "subweapon");
		}
		if (this.bonSubweapon == null)
		{
			this.bonSubweapon = this.childTail.Find((Transform itm) => itm.name == "subweapon");
		}
		this.parts.Sort((Renderer a, Renderer b) => a.name.CompareTo(b.name));
		for (int l = 0; l < this.parts.Count; l++)
		{
			this.partsColor.Add(Color.clear);
			this.partsAnim.Add(0f);
			this.partsTime.Add(0f);
		}
		if (transform4 != null)
		{
			this.SetFlagList(transform4, "");
			if (this.pelvis == null)
			{
				this.pelvis = transform4;
			}
			for (int m = 0; m < 27; m++)
			{
				CharaModelHandle.BlendShapeIndex blendShapeIndex = (CharaModelHandle.BlendShapeIndex)m;
				string text10 = blendShapeIndex.ToString();
				bool flag3 = text10.StartsWith("weapon");
				CharaModelHandle.BlendShapeData blendShapeData = new CharaModelHandle.BlendShapeData();
				blendShapeData.mesh = null;
				blendShapeData.idx = -1;
				blendShapeData.val = (flag3 ? 1f : 0f);
				foreach (SkinnedMeshRenderer skinnedMeshRenderer in list7)
				{
					int num14 = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("BS_" + text10 + "." + text10);
					if (num14 < 0)
					{
						num14 = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex("BS_" + text10.Substring(0, text10.IndexOf("_")) + "." + text10);
					}
					if (num14 >= 0)
					{
						blendShapeData.mesh = skinnedMeshRenderer;
						blendShapeData.idx = num14;
						break;
					}
				}
				blendShapeData.mdlkey = text10.Substring(0, text10.IndexOf("_") + 1);
				blendShapeData.mdlidx = -1;
				if (!flag3)
				{
					string mdstr = "md_" + text10;
					Transform transform6 = this.childAll.Find((Transform itm) => itm.name == mdstr);
					if (transform6 != null)
					{
						if (!this.modelList.ContainsKey(blendShapeData.mdlkey))
						{
							this.modelList.Add(blendShapeData.mdlkey, new List<GameObject>());
						}
						blendShapeData.mdlidx = this.modelList[blendShapeData.mdlkey].Count;
						this.modelList[blendShapeData.mdlkey].Add(transform6.gameObject);
					}
				}
				this.blendShapeList.Add(blendShapeData);
			}
			foreach (string text11 in new List<string>(this.modelList.Keys))
			{
				string text12 = "md_" + text11;
				foreach (GameObject gameObject2 in this.modelList[text11])
				{
					gameObject2.SetActive(false);
				}
				List<GameObject> list8 = new List<GameObject>();
				foreach (Transform transform7 in this.childAll)
				{
					if (transform7.name.StartsWith(text12) && !this.modelList[text11].Contains(transform7.gameObject))
					{
						list8.Add(transform7.gameObject);
					}
				}
				this.modelList[text11].Add(null);
				this.modelList[text11].AddRange(list8);
			}
			char c2;
			char c;
			for (c = 'a'; c <= 'h'; c = c2 + '\u0001')
			{
				Transform transform8 = this.childAll.Find((Transform itm) => itm.name.StartsWith("md_weapon_" + c.ToString()));
				this.weaponList.Add((transform8 != null) ? transform8.gameObject : null);
				c2 = c;
			}
			Transform[] componentsInChildren5 = transform4.GetComponentsInChildren<Transform>();
			if (transform4 != null)
			{
				this.crossRoot.Add(transform4, transform4.localPosition);
			}
			if (this.pelvis != null && this.pelvis != transform4)
			{
				this.crossRoot.Add(this.pelvis, this.pelvis.localPosition);
			}
			if (this.pelvis != null)
			{
				this.crossBone.Add(this.pelvis, this.pelvis.localRotation);
			}
			foreach (Transform transform9 in componentsInChildren5)
			{
				if (transform9.name.StartsWith("j_") && !transform9.name.StartsWith("j_head") && !transform9.name.StartsWith("j_eye") && !transform9.name.StartsWith("j_mouth") && !transform9.name.StartsWith("j_osg") && !transform9.name.StartsWith("j_ear") && !transform9.name.StartsWith("j_tail") && (!transform9.name.StartsWith("j_weapon") || !transform9.name.EndsWith("_sp")))
				{
					if (transform9.name.StartsWith("j_lowerbody") || transform9.name.StartsWith("j_thigh") || transform9.name.StartsWith("j_leg") || transform9.name.StartsWith("j_foot") || transform9.name.StartsWith("j_toe"))
					{
						this.crossBoneL.Add(transform9, transform9.localRotation);
					}
					else
					{
						this.crossBone.Add(transform9, transform9.localRotation);
					}
					if (transform9.name.StartsWith("j_weapon"))
					{
						this.crossWeapon.Add(transform9, null);
					}
				}
			}
		}
		foreach (Renderer renderer3 in this.partsDataBody.rootObj.GetComponentsInChildren<Renderer>(true))
		{
			if (renderer3.materials.Length != 0)
			{
				this.cullingList.Add(renderer3.gameObject.AddComponent<CullingCheck>());
				this.materials.AddRange(renderer3.materials);
				if (renderer3.name == "model" && renderer3.material.HasProperty("_Matcap"))
				{
					this.matCap = 0f;
				}
			}
			renderer3.shadowCastingMode = ShadowCastingMode.Off;
			renderer3.receiveShadows = false;
		}
		this.isFinishInit = true;
		this.partsDataBody.rootObj.SetActive(this.modelActive);
		if (this.shadowModel != null)
		{
			this.shadowModel.SetActive(this.modelActive);
		}
		if (this.shadow != null)
		{
			this.shadow.SetActive(this.modelActive && this.shadowActive);
		}
		this.SetEyeColor(this.eyeColor);
		this.SetCheekColor(this.cheekColor);
		yield break;
		yield break;
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0000BE28 File Offset: 0x0000A028
	private void SetFlagList(Transform root, string flagSuffix = "")
	{
		this.flagList.Clear();
		foreach (string text in CharaModelHandle.FLAG_LIST)
		{
			Transform transform = root.Find(text + flagSuffix);
			if (transform == null)
			{
				transform = new GameObject(text + flagSuffix).transform;
				transform.SetParent(root, false);
			}
			transform.localScale = Vector3.one * 20f;
			transform.gameObject.SetActive(false);
			this.flagList.Add(transform);
		}
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000BEB8 File Offset: 0x0000A0B8
	private void SetEyeObj(Transform root, string flagSuffix = "")
	{
		string text = string.Concat(new string[] { "pelvis", flagSuffix, "/j_upperbody", flagSuffix, "/j_chest", flagSuffix, "/j_neck", flagSuffix, "/j_head", flagSuffix });
		this.eyeCtrlL = root.Find(text + "/j_eye_l" + flagSuffix);
		this.eyeCtrlR = root.Find(text + "/j_eye_r" + flagSuffix);
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0000BF3F File Offset: 0x0000A13F
	public bool IsFinishInitialize()
	{
		return this.assetModelName.IndexOf("_1015_") > 0 || this.assetModelName.IndexOf("_1016_") > 0 || this.isFinishInit;
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000BF6F File Offset: 0x0000A16F
	private void OnDestroy()
	{
		this.DestoryInternal();
	}

	// Token: 0x06000192 RID: 402 RVA: 0x0000BF78 File Offset: 0x0000A178
	public void DestoryInternal()
	{
		if (this.initializeRoutine != null)
		{
			this.initializeRoutine = null;
		}
		this.isFinishInit = false;
		this.childAll = null;
		this.eyeBallL = (this.eyeBallR = null);
		this.eyeBall = null;
		this.eyeCtrlL = (this.eyeCtrlR = null);
		this.cheek = null;
		this.parts = null;
		this.mouthCtrl = (this.headCtrl = null);
		this.pelvis = null;
		this.upperBody = (this.lowerBody = null);
		this.thighL = (this.thighR = null);
		this.wrist = null;
		this.mdlSubweapon = (this.bonSubweapon = null);
		this.blendShapeList = null;
		this.modelList = null;
		this.weaponList = null;
		this.bonWeaponA = (this.bonWeaponB = null);
		this.cullingList = null;
		this.materials = null;
		this.headFollowObj = (this.eyeFollowObj = (this.mouthFollowObj = null));
		this.flagList = null;
		this.crossBone = null;
		this.crossBoneL = null;
		this.crossRoot = null;
		this.crossWeapon = null;
		this.authPlayer = null;
		if (this.puyoObj != null)
		{
			this.puyoObj.GetComponentsInChildren<MeshRenderer>(true)[0].material.SetTexture("_MainTex", null);
			if (!string.IsNullOrEmpty(this.puyoReq))
			{
				AssetManager.UnloadAssetData(this.puyoReq, AssetManager.OWNER.CharaModel);
				this.puyoReq = null;
			}
			if (!string.IsNullOrEmpty(this.puyoTex))
			{
				AssetManager.UnloadAssetData(this.puyoTex, AssetManager.OWNER.CharaModel);
				this.puyoTex = null;
			}
			Object.Destroy(this.puyoObj);
			this.puyoObj = null;
			return;
		}
		foreach (KeyValuePair<EffectData, Vector3> keyValuePair in this.effect)
		{
			EffectManager.DestroyEffect(keyValuePair.Key);
		}
		this.effect = new List<KeyValuePair<EffectData, Vector3>>();
		this.weaponCmnOff = new List<KeyValuePair<Transform, Transform>>();
		if (this.charaEffect != null)
		{
			foreach (EffectData effectData in this.charaEffect)
			{
				EffectManager.DestroyEffect(effectData);
			}
			EffectManager.UnloadEffect(this.charaEffectName, AssetManager.OWNER.CharaModel);
			this.charaEffect = null;
		}
		if (this.accEff != null)
		{
			foreach (CharaModelHandle.AccEff accEff in this.accEff)
			{
				if (accEff != null)
				{
					if (accEff.eff != null)
					{
						EffectManager.DestroyEffect(accEff.eff);
					}
					EffectManager.UnloadEffect(accEff.name, AssetManager.OWNER.CharaModel);
				}
			}
			this.accEff = null;
		}
		if (CharaModelHandle.accCamChara == base.transform)
		{
			CharaModelHandle.accCamChara = null;
		}
		if (this.shadowModel != null)
		{
			this.shadowCull = null;
			this.shadowBone = null;
			Object.Destroy(this.shadowModel);
			this.shadowModel = null;
		}
		for (int i = 0; i < this.partsDataEar.Length; i++)
		{
			if (this.partsDataEar[i] != null)
			{
				Object.Destroy(this.partsDataEar[i].rootObj);
				this.partsDataEar[i].rootObj = null;
				this.partsDataEar[i] = null;
			}
		}
		for (int j = 0; j < this.partsDataTail.Length; j++)
		{
			if (this.partsDataTail[j] != null)
			{
				Object.Destroy(this.partsDataTail[j].rootObj);
				this.partsDataTail[j].rootObj = null;
				this.partsDataTail[j] = null;
			}
		}
		if (this.partsDataBody != null)
		{
			Object.Destroy(this.partsDataBody.rootObj);
			this.partsDataBody.rootObj = null;
			this.partsDataBody = null;
		}
		if (this.shadow != null)
		{
			this.shadowMat = null;
			Object.Destroy(this.shadow);
			this.shadow = null;
		}
		if (!string.IsNullOrEmpty(this.loadVoiceCueSheetName))
		{
			SoundManager.UnloadCueSheet(this.loadVoiceCueSheetName);
			this.loadVoiceCueSheetName = "";
		}
		string text = this.assetModelName;
		this.assetModelName = string.Empty;
		if (!string.IsNullOrEmpty(text))
		{
			string text2 = CharaModelHandle.CHARA_MODEL_PATH + text;
			List<string> list = CharaModelHandle.MakeOptionModelName(text, CharaModelHandle.OPTION_SUFFIX_EAR);
			List<string> list2 = new List<string>();
			foreach (string text3 in list)
			{
				list2.Add(string.IsNullOrEmpty(text3) ? "" : (CharaModelHandle.CHARA_MODEL_PATH + text3));
			}
			List<string> list3 = CharaModelHandle.MakeOptionModelName(text, CharaModelHandle.OPTION_SUFFIX_TAIL);
			List<string> list4 = new List<string>();
			foreach (string text4 in list3)
			{
				list4.Add(string.IsNullOrEmpty(text4) ? "" : (CharaModelHandle.CHARA_MODEL_PATH + text4));
			}
			string text5 = "Effects/info/Ef_info_shadow";
			AssetManager.UnloadAssetData(text2, AssetManager.OWNER.CharaModel);
			foreach (string text6 in list2)
			{
				if (!string.IsNullOrEmpty(text6))
				{
					AssetManager.UnloadAssetData(text6, AssetManager.OWNER.CharaModel);
				}
			}
			foreach (string text7 in list4)
			{
				if (!string.IsNullOrEmpty(text7))
				{
					AssetManager.UnloadAssetData(text7, AssetManager.OWNER.CharaModel);
				}
			}
			if (this.assetIsShadow)
			{
				AssetManager.UnloadAssetData(text5, AssetManager.OWNER.CharaModel);
			}
			PrjUtil.ReleaseMemory(PrjUtil.UnloadUnused / 10);
		}
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0000C564 File Offset: 0x0000A764
	private void Update()
	{
		this.UpdateInternal();
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0000C56C File Offset: 0x0000A76C
	public void UpdateInternal()
	{
		if (this.initializeRoutine != null && !this.initializeRoutine.MoveNext())
		{
			this.initializeRoutine = null;
		}
		if (this.puyoObj != null)
		{
			if (!string.IsNullOrEmpty(this.puyoReq) && AssetManager.IsLoadFinishAssetData(this.puyoReq))
			{
				this.puyoObj.GetComponentsInChildren<MeshRenderer>(true)[0].material.SetTexture("_MainTex", AssetManager.GetAssetData(this.puyoReq) as Texture2D);
				this.puyoTex = this.puyoReq;
				this.puyoReq = null;
			}
			if (this.modelActive && this.fade != 0f)
			{
				this.alpha += Time.deltaTime / this.fade;
			}
			this.setPuyo();
			return;
		}
		if (this.isFinishInit)
		{
			float num;
			if (this.motionTimeReq >= 0f)
			{
				num = (this.motionTime = this.motionTimeReq);
				this.motionTimeReq = -1f;
			}
			else if (this.motionSpeed < 0f)
			{
				this.motionTime += TimeManager.DeltaTime;
				if ((num = this.motionTime + this.motionSpeed) >= 0f)
				{
					this.motionTime = (this.motionLoop ? num : (-this.motionSpeed));
				}
				else
				{
					num = 0f;
				}
			}
			else
			{
				this.motionTime += TimeManager.DeltaTime * this.motionSpeed;
				if ((num = this.motionTime - this.motionLength) >= 0f)
				{
					this.motionTime = (this.motionLoop ? num : this.motionLength);
				}
				else
				{
					num = 0f;
				}
			}
			if (!string.IsNullOrEmpty(this.motionReq.name))
			{
				this.motionEnd = false;
				this.motionName = this.motionReq.name;
				this.motionLoop = this.motionReq.loop;
				this.motionSpeed = this.motionReq.speed;
				this.motionTime = num;
				if (this.crossFade < this.motionReq.fade)
				{
					this.crossFade = this.motionReq.fade;
				}
				else if (this.motionReq.fade < 0f)
				{
					this.crossFade = 0f;
				}
				if (this.crossFadeL < this.motionReq.fadeL)
				{
					this.crossFadeL = this.motionReq.fadeL;
				}
				else if (this.motionReq.fadeL < 0f)
				{
					this.crossFadeL = 0f;
				}
				if (this.crossFadeOffMotion.Contains(this.motionName))
				{
					this.crossFade = (this.crossFadeL = 0f);
				}
				if (this.motionReq.turn)
				{
					if (this.pelvis != null && this.crossBone.ContainsKey(this.pelvis))
					{
						this.pelvis.localEulerAngles = new Vector3(this.pelvis.localEulerAngles.x, 0f, this.pelvis.localEulerAngles.z);
						this.crossBone[this.pelvis] = this.pelvis.localRotation;
					}
					if (this.upperBody != null && this.crossBone.ContainsKey(this.upperBody))
					{
						this.upperBody.localEulerAngles = new Vector3(this.upperBody.localEulerAngles.x, 0f, this.upperBody.localEulerAngles.z);
						this.crossBone[this.upperBody] = this.upperBody.localRotation;
					}
					if (this.lowerBody != null && this.crossBoneL.ContainsKey(this.lowerBody))
					{
						this.lowerBody.localEulerAngles = new Vector3(this.lowerBody.localEulerAngles.x, 0f, this.lowerBody.localEulerAngles.z);
						this.crossBoneL[this.lowerBody] = this.lowerBody.localRotation;
					}
					this.partsDataBody.referencer.ResetOsage();
					for (int i = 0; i < this.partsDataEar.Length; i++)
					{
						CharaModelHandle.PartsData partsData = this.partsDataEar[i];
						if (partsData != null && !(partsData.rootObj == null) && !partsData.animation.enabled)
						{
							partsData.referencer.ResetOsage();
						}
					}
					for (int j = 0; j < this.partsDataTail.Length; j++)
					{
						CharaModelHandle.PartsData partsData = this.partsDataTail[j];
						if (partsData != null && !(partsData.rootObj == null) && !partsData.animation.enabled)
						{
							partsData.referencer.ResetOsage();
						}
					}
				}
				if (this.headFollowFade < this.motionReq.fade)
				{
					this.headFollowFade = this.motionReq.fade;
				}
				CharaModelReferencer.RefAnime refAnime = this.partsDataBody.referencer.refAnimationObj.GetComponent<CharaModelReferencer>().prefabAnimeList.Find((CharaModelReferencer.RefAnime item) => item.name == this.motionName);
				CharaModelReferencer.RefAnime refAnime2 = Singleton<AssetManager>.Instance.GetDefaultMotionReferencer().prefabAnimeList.Find((CharaModelReferencer.RefAnime item) => item.name == this.motionName);
				num = 0f;
				if (this.partsDataBody.animation.enabled = this.partsDataBody.animationKeyList.ContainsKey(this.motionName))
				{
					this.partsDataBody.animation.ExPauseAnimation(this.motionName);
					SimpleAnimation.State state = this.partsDataBody.animation.GetState(this.motionName);
					this.motionLength = ((state == null || state.clip == null) ? 1f : state.clip.length);
					num = ((this.motionSpeed < 0f) ? (this.motionTime * this.motionLength / -this.motionSpeed) : this.motionTime);
					if (state != null)
					{
						state.time = num;
					}
				}
				if (this.initializeParam.isEnableMotionSE && Singleton<SoundManager>.Instance != null)
				{
					if (refAnime != null && refAnime.seName != string.Empty)
					{
						SoundManager.Play(refAnime.seName, false, false);
					}
					else if (refAnime2 != null && refAnime2.seName != string.Empty)
					{
						SoundManager.Play(refAnime2.seName, false, false);
					}
				}
				this.earTyp = 0;
				this.earTypBef = 0;
				this.earTypFrm = 0;
				this.earTypAft = 0;
				if (refAnime != null)
				{
					this.earTyp = (this.earTypBef = refAnime.earBefore);
					this.earTypFrm = refAnime.earFrame;
					this.earTypAft = refAnime.earAfter;
				}
				if (refAnime2 != null && this.earTypBef == 0 && this.earTypFrm == 0 && this.earTypAft == 0)
				{
					this.earTyp = (this.earTypBef = refAnime2.earBefore);
					this.earTypFrm = refAnime2.earFrame;
					this.earTypAft = refAnime2.earAfter;
				}
				for (int k = 0; k < this.partsDataEar.Length; k++)
				{
					CharaModelHandle.PartsData partsData = this.partsDataEar[k];
					if (partsData != null && !(partsData.rootObj == null))
					{
						partsData.rootObj.SetActive(k == this.earTyp);
						if (partsData.rootObj.activeSelf && (partsData.animation.enabled = partsData.animationKeyList.ContainsKey(this.motionName)))
						{
							partsData.animation.ExPauseAnimation(this.motionName);
							SimpleAnimation.State state2 = partsData.animation.GetState(this.motionName);
							if (state2 != null)
							{
								state2.time = num;
							}
						}
					}
				}
				this.tailTyp = 0;
				this.tailTypBef = 0;
				this.tailTypFrm = 0;
				this.tailTypAft = 0;
				if (refAnime != null)
				{
					this.tailTyp = (this.tailTypBef = refAnime.tailBefore);
					this.tailTypFrm = refAnime.tailFrame;
					this.tailTypAft = refAnime.tailAfter;
				}
				if (refAnime2 != null && this.tailTypBef == 0 && this.tailTypFrm == 0 && this.tailTypAft == 0)
				{
					this.tailTyp = (this.tailTypBef = refAnime2.tailBefore);
					this.tailTypFrm = refAnime2.tailFrame;
					this.tailTypAft = refAnime2.tailAfter;
				}
				if (this.tailOffMotion.Contains(this.motionName))
				{
					this.tailTyp = -1;
				}
				for (int l = 0; l < this.partsDataTail.Length; l++)
				{
					CharaModelHandle.PartsData partsData = this.partsDataTail[l];
					if (partsData != null && !(partsData.rootObj == null))
					{
						partsData.rootObj.SetActive(l == this.tailTyp);
						if (partsData.rootObj.activeSelf && (partsData.animation.enabled = partsData.animationKeyList.ContainsKey(this.motionName)))
						{
							partsData.animation.ExPauseAnimation(this.motionName);
							SimpleAnimation.State state3 = partsData.animation.GetState(this.motionName);
							if (state3 != null)
							{
								state3.time = num;
							}
						}
					}
				}
				this.motionReq.name = "";
				if (this.motionPos.y >= 0f)
				{
					base.transform.localPosition = this.motionPos;
					this.motionPos.y = -1f;
					this.haraOffOld = true;
				}
				if (this.motionRot >= 0f)
				{
					base.transform.localEulerAngles = new Vector3(0f, this.motionRot, 0f);
					this.motionRot = -1f;
				}
			}
			else if (this.motionLength > 0f)
			{
				if (!this.motionLoop)
				{
					this.motionEnd = ((this.motionSpeed < 0f) ? (this.motionTime > -this.motionSpeed - 0.03334f) : (this.motionTime > this.motionLength - 0.03334f * this.motionSpeed));
				}
				num = ((this.motionSpeed < 0f) ? (this.motionTime * this.motionLength / -this.motionSpeed) : this.motionTime);
				if (this.partsDataBody.animation.enabled)
				{
					SimpleAnimation.State state4 = this.partsDataBody.animation.GetState(this.motionName);
					if (state4 != null)
					{
						state4.time = num;
					}
				}
				int num2 = (int)(((this.authPlayer == null) ? num : this.authPlayer.GetTime()) * 30f);
				int num3 = ((this.earTypFrm <= 0 || num2 < this.earTypFrm) ? this.earTypBef : this.earTypAft);
				CharaModelHandle.PartsData partsData;
				if (this.earTyp != num3)
				{
					this.earTyp = num3;
					for (int m = 0; m < this.partsDataEar.Length; m++)
					{
						partsData = this.partsDataEar[m];
						if (partsData != null && !(partsData.rootObj == null))
						{
							partsData.rootObj.SetActive(m == this.earTyp);
							if (partsData.rootObj.activeSelf && (partsData.animation.enabled = partsData.animationKeyList.ContainsKey(this.motionName)))
							{
								partsData.animation.ExPauseAnimation(this.motionName);
							}
						}
					}
				}
				partsData = ((this.earTyp < 0 || this.earTyp >= this.partsDataEar.Length) ? null : this.partsDataEar[this.earTyp]);
				if (partsData != null && partsData.rootObj != null && partsData.animation.enabled)
				{
					SimpleAnimation.State state5 = partsData.animation.GetState(this.motionName);
					if (state5 != null)
					{
						state5.time = num;
					}
				}
				int num4 = ((this.tailTypFrm <= 0 || num2 < this.tailTypFrm) ? this.tailTypBef : this.tailTypAft);
				if (this.tailOffMotion.Contains(this.motionName))
				{
					num4 = -1;
				}
				if (this.tailTyp != num4)
				{
					this.tailTyp = num4;
					for (int n = 0; n < this.partsDataTail.Length; n++)
					{
						partsData = this.partsDataTail[n];
						if (partsData != null && !(partsData.rootObj == null))
						{
							partsData.rootObj.SetActive(n == this.tailTyp);
							if (partsData.rootObj.activeSelf && (partsData.animation.enabled = partsData.animationKeyList.ContainsKey(this.motionName)))
							{
								partsData.animation.ExPauseAnimation(this.motionName);
							}
						}
					}
				}
				partsData = ((this.tailTyp < 0 || this.tailTyp >= this.partsDataTail.Length) ? null : this.partsDataTail[this.tailTyp]);
				if (partsData != null && partsData.rootObj != null && partsData.animation.enabled)
				{
					SimpleAnimation.State state6 = partsData.animation.GetState(this.motionName);
					if (state6 != null)
					{
						state6.time = num;
					}
				}
			}
			else if (!string.IsNullOrEmpty(this.motionName))
			{
				num = 0f;
				if (this.partsDataBody.animation.enabled)
				{
					if (!this.partsDataBody.animation.ExIsPlaying())
					{
						if (this.motionLoop)
						{
							this.partsDataBody.animation.ExPlayAnimation(this.motionName, null);
						}
						else
						{
							this.motionEnd = true;
						}
					}
					SimpleAnimation.State state7 = this.partsDataBody.animation.GetState(this.motionName);
					if (state7 != null)
					{
						num = state7.time;
					}
				}
				int num5 = (int)(((this.authPlayer == null) ? num : this.authPlayer.GetTime()) * 30f);
				int num6 = ((this.earTypFrm <= 0 || num5 < this.earTypFrm) ? this.earTypBef : this.earTypAft);
				CharaModelHandle.PartsData partsData;
				if (this.earTyp != num6)
				{
					this.earTyp = num6;
					for (int num7 = 0; num7 < this.partsDataEar.Length; num7++)
					{
						partsData = this.partsDataEar[num7];
						if (partsData != null && !(partsData.rootObj == null))
						{
							partsData.rootObj.SetActive(num7 == this.earTyp);
							if (partsData.rootObj.activeSelf && (partsData.animation.enabled = partsData.animationKeyList.ContainsKey(this.motionName)))
							{
								partsData.animation.ExPauseAnimation(this.motionName);
							}
						}
					}
				}
				partsData = ((this.earTyp < 0 || this.earTyp >= this.partsDataEar.Length) ? null : this.partsDataEar[this.earTyp]);
				if (partsData != null && partsData.rootObj != null && partsData.animation.enabled)
				{
					if (!partsData.animation.ExIsPlaying() && this.motionLoop)
					{
						partsData.animation.ExPlayAnimation(this.motionName, null);
					}
					SimpleAnimation.State state8 = partsData.animation.GetState(this.motionName);
					if (state8 != null)
					{
						state8.time = num;
					}
				}
				int num8 = ((this.tailTypFrm <= 0 || num5 < this.tailTypFrm) ? this.tailTypBef : this.tailTypAft);
				if (this.tailOffMotion.Contains(this.motionName))
				{
					num8 = -1;
				}
				if (this.tailTyp != num8)
				{
					this.tailTyp = num8;
					for (int num9 = 0; num9 < this.partsDataTail.Length; num9++)
					{
						partsData = this.partsDataTail[num9];
						if (partsData != null && !(partsData.rootObj == null))
						{
							partsData.rootObj.SetActive(num9 == this.tailTyp);
							if (partsData.rootObj.activeSelf && (partsData.animation.enabled = partsData.animationKeyList.ContainsKey(this.motionName)))
							{
								partsData.animation.ExPauseAnimation(this.motionName);
							}
						}
					}
				}
				partsData = ((this.tailTyp < 0 || this.tailTyp >= this.partsDataTail.Length) ? null : this.partsDataTail[this.tailTyp]);
				if (partsData != null && partsData.rootObj != null && partsData.animation.enabled)
				{
					if (!partsData.animation.ExIsPlaying() && this.motionLoop)
					{
						partsData.animation.ExPlayAnimation(this.motionName, null);
					}
					SimpleAnimation.State state9 = partsData.animation.GetState(this.motionName);
					if (state9 != null)
					{
						state9.time = num;
					}
				}
			}
			this.partsDataBody.referencer.SetRootPos(this.rootResetMotion.Contains(this.motionName));
			if (this.modelActive && this.fade != 0f)
			{
				this.alpha += Time.deltaTime / this.fade;
			}
			float num10 = Time.deltaTime * 5f;
			if (this.NeighboringAlpha.y >= 0f)
			{
				Vector3 haraPos = this.GetHaraPos();
				haraPos.x -= this.NeighboringAlpha.x;
				haraPos.z -= this.NeighboringAlpha.z;
				float num11 = Mathf.Sqrt(haraPos.x * haraPos.x + haraPos.z * haraPos.z);
				if (num11 <= 1f || (num11 < 1.25f && this.NeighboringAlpha.w < 1f))
				{
					num10 = -num10;
				}
			}
			this.NeighboringAlpha.w = Mathf.Clamp01(this.NeighboringAlpha.w + num10);
			this.SetEnv();
			this.NeighboringAlpha.y = -1f;
			this.partsDataBody.referencer.UpdateOsage();
			for (int num12 = 0; num12 < this.partsDataEar.Length; num12++)
			{
				CharaModelHandle.PartsData partsData = this.partsDataEar[num12];
				if (partsData != null && !(partsData.rootObj == null) && !partsData.animation.enabled)
				{
					partsData.referencer.UpdateOsage();
				}
			}
			for (int num13 = 0; num13 < this.partsDataTail.Length; num13++)
			{
				CharaModelHandle.PartsData partsData = this.partsDataTail[num13];
				if (partsData != null && !(partsData.rootObj == null) && !partsData.animation.enabled)
				{
					partsData.referencer.UpdateOsage();
				}
			}
		}
	}

	// Token: 0x06000195 RID: 405 RVA: 0x0000D794 File Offset: 0x0000B994
	private void SetEnv()
	{
		int num = this.layer;
		string text = LayerMask.LayerToName(this.layer);
		if (this.alpha < 0f)
		{
			this.alpha = 0f;
		}
		else if (this.alpha > 1f)
		{
			this.alpha = 1f;
		}
		float num2 = this.alpha * this.NeighboringAlpha.w;
		if (num2 <= 0f)
		{
			num = LayerMask.NameToLayer(text = "Ignore Raycast");
		}
		else if (num2 < 1f)
		{
			if (this.matCap < 0f)
			{
				if (FieldAlpha.layerList.Contains(text))
				{
					num = LayerMask.NameToLayer(text + "Alpha");
				}
				else
				{
					num = LayerMask.NameToLayer("FieldPlayerAlpha");
				}
			}
			this.camouflageAlpha = 0f;
		}
		else if (this.camouflage)
		{
			num = LayerMask.NameToLayer("Camouflage");
			if ((this.camouflageAlpha += TimeManager.DeltaTime) > 0.9f)
			{
				this.camouflageAlpha = 0.9f;
			}
		}
		else if ((this.camouflageAlpha -= TimeManager.DeltaTime) > 0f)
		{
			num = LayerMask.NameToLayer("Camouflage");
			if (this.camouflageAlpha > 0.77f)
			{
				this.camouflageAlpha = 0.77f;
			}
		}
		else
		{
			this.camouflageAlpha = 0f;
		}
		if (this.partsDataBody.rootObj.layer != num)
		{
			this.partsDataBody.rootObj.SetLayerRecursively(num);
			if (num == LayerMask.NameToLayer(text = "Ignore Raycast"))
			{
				UnityAction unityAction = this.fadeOutEnd;
				if (unityAction != null)
				{
					unityAction();
				}
				this.fadeOutEnd = null;
			}
		}
		if (this.shadow != null && this.shadow.layer != this.layer)
		{
			this.shadow.SetLayerRecursively(this.layer);
		}
		foreach (KeyValuePair<EffectData, Vector3> keyValuePair in this.effect)
		{
			if (keyValuePair.Key.effectObject != null && keyValuePair.Key.effectObject.layer != this.layer && this.partsDataBody.rootObj.layer != LayerMask.NameToLayer(text = "Ignore Raycast"))
			{
				keyValuePair.Key.effectObject.SetLayerRecursively(this.layer);
			}
		}
		if (this.charaEffectType == CharaModelHandle.CharaEffectType.OBJECT)
		{
			if (this.charaEffect == null)
			{
				goto IL_0451;
			}
			using (List<EffectData>.Enumerator enumerator2 = this.charaEffect.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					EffectData effectData = enumerator2.Current;
					if (effectData.effectObject != null && base.transform.parent != null && effectData.effectObject.layer != this.layer && base.transform.parent.name != "RenderTexture Base" && base.transform.parent.name != "FieldSceneBattleResult" && base.transform.parent.name != "FieldSceneBattle(Clone)" && (!(base.transform.parent.name == "Chara") || !(base.transform.parent.parent != null) || !(base.transform.parent.parent.name == "FieldSceneTreeHouse(Clone)")) && (!(base.transform.parent.name == "Model") || !(base.transform.root.name == "ScenarioPrefab(Clone)")))
					{
						effectData.effectObject.SetLayerRecursively(this.layer);
					}
				}
				goto IL_0451;
			}
		}
		if (this.charaEffectType != CharaModelHandle.CharaEffectType.DISABLE)
		{
			foreach (EffectData effectData2 in this.charaEffect)
			{
				if (effectData2.effectObject != null && effectData2.effectObject.layer != this.layer)
				{
					effectData2.effectObject.SetLayerRecursively(this.layer);
				}
			}
		}
		IL_0451:
		if (this.accEff != null)
		{
			foreach (CharaModelHandle.AccEff accEff in this.accEff)
			{
				if (accEff != null && accEff.eff != null && accEff.eff.effectObject != null && accEff.eff.effectObject.layer != this.layer)
				{
					accEff.eff.effectObject.SetLayerRecursively(this.layer);
				}
			}
		}
		if (this.accEff != null && this.isAccessoryAnchor)
		{
			foreach (CharaModelHandle.AccEff accEff2 in this.accEff)
			{
				if (accEff2 != null && accEff2.eff != null && accEff2.eff.effectObject != null && base.transform.parent != null && accEff2.eff.effectObject.layer != this.layer && base.transform.parent.name != "RenderTexture Base" && base.transform.parent.name != "FieldSceneBattleResult" && base.transform.parent.name != "FieldSceneBattle(Clone)" && (!(base.transform.parent.name == "Chara") || !(base.transform.parent.parent != null) || !(base.transform.parent.parent.name == "FieldSceneTreeHouse(Clone)")) && (!(base.transform.parent.name == "Model") || !(base.transform.root.name == "ScenarioPrefab(Clone)")))
				{
					accEff2.eff.effectObject.SetLayerRecursively(this.layer);
				}
			}
		}
		if (this.shadowModel != null)
		{
			int num3 = ((this.shadowBone == null) ? num : LayerMask.NameToLayer((text == "FieldEnemy" || text == "FieldPlayer" || text == "AuthMain") ? (text + "Shadow") : text));
			if (this.shadowModel.layer != num3)
			{
				this.shadowModel.SetLayerRecursively(num3);
			}
			if (this.shadowCull != null)
			{
				List<Camera> camera = this.shadowCull.getCamera();
				if (camera != null && camera.Count > 0 && camera[0] != null)
				{
					this.shadowCamera = camera[0];
				}
				Material[] array = this.shadowCull.GetComponent<Renderer>().materials;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetColor("_Color", new Color(1f, 1f, 1f, num2));
				}
			}
		}
		foreach (Material material in this.materials)
		{
			material.SetFloat("_Alpha", num2);
			material.SetFloat("_Camouflage", this.camouflageAlpha);
		}
		if (this.matCap >= 0f)
		{
			foreach (Material material2 in this.materials)
			{
				material2.SetFloat("_MatcapValue", this.matCap);
			}
		}
	}

	// Token: 0x06000196 RID: 406 RVA: 0x0000E074 File Offset: 0x0000C274
	private void setPuyo()
	{
		int num = this.layer;
		string text = LayerMask.LayerToName(this.layer);
		if (this.alpha <= 0f)
		{
			this.alpha = 0f;
			num = LayerMask.NameToLayer("Ignore Raycast");
		}
		else if (this.alpha < 1f)
		{
			if (FieldAlpha.layerList.Contains(text))
			{
				num = LayerMask.NameToLayer(text + "Alpha");
			}
			else
			{
				num = LayerMask.NameToLayer("FieldPlayerAlpha");
			}
		}
		else
		{
			this.alpha = 1f;
		}
		if (this.puyoObj.layer != num)
		{
			this.puyoObj.SetLayerRecursively(num);
		}
		this.puyoObj.GetComponentsInChildren<MeshRenderer>(true)[0].material.SetFloat("_Alpha", this.alpha);
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000E140 File Offset: 0x0000C340
	private void SetFaceData(CharaModelHandle.BlendShapeIndex bsi, float val)
	{
		if (val > 10f)
		{
			return;
		}
		float num = Mathf.Clamp01(val);
		int num2 = 0;
		while (num2 < this.partsColor.Count && num2 < CharaModelHandle.COLOR_LIST.GetLength(0))
		{
			for (int i = 0; i < CharaModelHandle.COLOR_LIST.GetLength(1); i++)
			{
				if (bsi == CharaModelHandle.COLOR_LIST[num2, i])
				{
					Color color = this.partsColor[num2];
					if (i == 0)
					{
						color.r = num;
					}
					else if (i == 1)
					{
						color.g = num;
					}
					else if (i == 2)
					{
						color.b = num;
					}
					else if (i == 3)
					{
						color.a = num;
					}
					else
					{
						this.partsAnim[num2] = num;
					}
					this.partsColor[num2] = color;
				}
			}
			num2++;
		}
		if (bsi == CharaModelHandle.BlendShapeIndex.weapon_a)
		{
			this.weaponDispA = (int)(val + 0.5f) - 1;
		}
		else if (bsi == CharaModelHandle.BlendShapeIndex.weapon_b)
		{
			this.weaponDispB = (int)(val + 0.5f) - 1;
		}
		else if (bsi == CharaModelHandle.BlendShapeIndex.sub_weapon)
		{
			this.weaponDispSub = (int)(val + 0.5f);
		}
		else if (bsi >= (CharaModelHandle.BlendShapeIndex)this.blendShapeList.Count)
		{
			return;
		}
		CharaModelHandle.BlendShapeData blendShapeData = this.blendShapeList[(int)bsi];
		if (blendShapeData.mesh != null && blendShapeData.idx >= 0)
		{
			blendShapeData.mesh.SetBlendShapeWeight(blendShapeData.idx, num * 100f);
		}
		if (this.modelList.ContainsKey(blendShapeData.mdlkey) && blendShapeData.mdlidx >= 0)
		{
			this.modelList[blendShapeData.mdlkey][blendShapeData.mdlidx].SetActive(val > 0.5f);
		}
	}

	// Token: 0x06000198 RID: 408 RVA: 0x0000E2E4 File Offset: 0x0000C4E4
	private void SetEyeAngle(Transform ball, Vector3 minus, Vector3 plus, Vector2 old)
	{
		float z = ball.localEulerAngles.z;
		Vector3 vector = new Vector3(this.eyeFollowObj.position.x, ball.position.y, this.eyeFollowObj.position.z);
		ball.LookAt(vector);
		float num = this.SetAngle(ball.localEulerAngles.x, CharaModelHandle.eyeFollowStart, CharaModelHandle.eyeFollowEnd, minus.x, plus.x);
		ball.localEulerAngles = new Vector3(num, 0f, 0f);
		float num2 = Vector3.Magnitude(this.eyeFollowObj.position - ball.position);
		vector = ball.TransformPoint(0f, 0f, num2);
		vector.y = this.eyeFollowObj.position.y;
		ball.LookAt(vector);
		float num3 = this.SetAngle(ball.localEulerAngles.y, CharaModelHandle.eyeFollowStart, CharaModelHandle.eyeFollowEnd, minus.y, plus.y);
		ball.localEulerAngles = new Vector3(num, num3, z);
	}

	// Token: 0x06000199 RID: 409 RVA: 0x0000E3F8 File Offset: 0x0000C5F8
	private float SetAngle(float ang, float start, float end, float minus, float plus)
	{
		float num = Mathf.DeltaAngle(0f, ang);
		float num2 = end - start;
		if (num < 0f)
		{
			if ((num += start) > 0f)
			{
				num = 0f;
			}
			else if (num < -num2)
			{
				num = -num2;
			}
		}
		if (num > 0f)
		{
			if ((num -= start) < 0f)
			{
				num = 0f;
			}
			else if (num > num2)
			{
				num = num2;
			}
		}
		num /= num2;
		if (num < 0f)
		{
			num *= -minus;
		}
		else
		{
			num *= plus;
		}
		return num;
	}

	// Token: 0x0600019A RID: 410 RVA: 0x0000E478 File Offset: 0x0000C678
	private void LateUpdate()
	{
		this.viewCamera = null;
		if (this.puyoObj != null)
		{
			if ((this.puyoBreath += 180f * TimeManager.DeltaTime) >= 360f)
			{
				this.puyoBreath -= 360f;
			}
			this.puyoObj.transform.localScale = new Vector3(1f, 1f + Mathf.Sin(0.017453292f * this.puyoBreath) * 0.005f, 1f);
			this.puyoObj.transform.eulerAngles = new Vector3(0f, 180f, 0f);
			return;
		}
		if (this.isFinishInit)
		{
			bool flag = false;
			if (this.haraOffset)
			{
				Vector3 position = base.transform.position;
				if (this.pelvis != null)
				{
					position.x += position.x - this.pelvis.position.x;
					position.z += position.z - this.pelvis.position.z;
				}
				base.transform.position = position;
				this.haraOffset = false;
				this.haraOffOld = true;
				flag = true;
			}
			else if (this.haraOffOld)
			{
				this.haraOffOld = false;
				flag = true;
			}
			if (this.headCtrl != null)
			{
				if (this.headFollowObj != null)
				{
					float z = this.headCtrl.localEulerAngles.z;
					Vector3 vector = new Vector3(this.headFollowObj.position.x, this.headCtrl.position.y, this.headFollowObj.position.z);
					this.headCtrl.LookAt(vector);
					float num = this.SetAngle(this.headCtrl.localEulerAngles.x, CharaModelHandle.headFollowStart, CharaModelHandle.headFollowEnd, CharaModelHandle.headAngleMinus, CharaModelHandle.headAnglePlus);
					this.headCtrl.localEulerAngles = new Vector3(num, 0f, 0f);
					float num2 = Vector3.Magnitude(this.headFollowObj.position - this.headCtrl.position);
					vector = this.headCtrl.TransformPoint(0f, 0f, num2);
					vector.y = this.headFollowObj.position.y;
					this.headCtrl.LookAt(vector);
					float num3 = this.SetAngle(this.headCtrl.localEulerAngles.y, CharaModelHandle.headFollowStart, CharaModelHandle.headFollowEnd, CharaModelHandle.headAngleMinus, CharaModelHandle.headAnglePlus);
					this.headCtrl.localEulerAngles = new Vector3(num, num3, z);
					this.headFollowFade = 0.2f;
				}
				float num4 = this.headFollowFade;
				if (num4 > 0f)
				{
					num4 = Mathf.Clamp01((this.headFollowFade -= TimeManager.DeltaTime) / num4);
				}
				else
				{
					num4 = 0f;
				}
				this.headRotationOld = (this.headCtrl.localRotation = Quaternion.Lerp(this.headCtrl.localRotation, this.headRotationOld, num4));
			}
			if (this.eyeFollowObj != null)
			{
				if (this.eyeBallL != null)
				{
					this.SetEyeAngle(this.eyeBallL, CharaModelHandle.eyeAngleMinusL, CharaModelHandle.eyeAnglePlusL, this.eyeAngleOldL);
				}
				if (this.eyeBallR != null)
				{
					this.SetEyeAngle(this.eyeBallR, CharaModelHandle.eyeAngleMinusR, CharaModelHandle.eyeAnglePlusR, this.eyeAngleOldR);
				}
				this.eyeFollowFade = CharaModelHandle.followSpeed;
			}
			else if (this.eyeMotionType == CharaModelHandle.EyeMotType.ENABLE_CHARA && this.enabledFaceMotion)
			{
				if (this.eyeBallL != null && this.eyeCtrlL != null)
				{
					this.eyeBallL.rotation = this.eyeCtrlL.rotation;
				}
				if (this.eyeBallR != null && this.eyeCtrlR != null)
				{
					this.eyeBallR.rotation = this.eyeCtrlR.rotation;
				}
			}
			else if (this.eyeMotionType == CharaModelHandle.EyeMotType.ENABLE_AUTH)
			{
				if (this.eyeBallL != null && this.eyeCtrlL != null)
				{
					this.eyeBallL.localEulerAngles = this.eyeCtrlL.localEulerAngles;
				}
				if (this.eyeBallR != null && this.eyeCtrlR != null)
				{
					this.eyeBallR.localEulerAngles = this.eyeCtrlR.localEulerAngles;
				}
			}
			else
			{
				if (this.eyeBallL != null)
				{
					this.eyeBallL.localEulerAngles = this.eyeAngleL;
				}
				if (this.eyeBallR != null)
				{
					this.eyeBallR.localEulerAngles = this.eyeAngleR;
				}
			}
			if (this.eyeBallL)
			{
				if (this.eyeFollowFade > 1f)
				{
					Vector3 localEulerAngles = this.eyeBallL.localEulerAngles;
					localEulerAngles.x = this.eyeAngleOldL.x + Mathf.DeltaAngle(this.eyeAngleOldL.x, localEulerAngles.x) / this.eyeFollowFade;
					localEulerAngles.y = this.eyeAngleOldL.y + Mathf.DeltaAngle(this.eyeAngleOldL.y, localEulerAngles.y) / this.eyeFollowFade;
					this.eyeBallL.localEulerAngles = localEulerAngles;
				}
				this.eyeAngleOldL = this.eyeBallL.localEulerAngles;
			}
			if (this.eyeBallR)
			{
				if (this.eyeFollowFade > 1f)
				{
					Vector3 localEulerAngles2 = this.eyeBallR.localEulerAngles;
					localEulerAngles2.x = this.eyeAngleOldR.x + Mathf.DeltaAngle(this.eyeAngleOldR.x, localEulerAngles2.x) / this.eyeFollowFade;
					localEulerAngles2.y = this.eyeAngleOldR.y + Mathf.DeltaAngle(this.eyeAngleOldR.y, localEulerAngles2.y) / this.eyeFollowFade;
					this.eyeBallR.localEulerAngles = localEulerAngles2;
				}
				this.eyeAngleOldR = this.eyeBallR.localEulerAngles;
			}
			this.eyeFollowFade -= 1f;
			this.weaponDispA = (this.weaponDispB = -1);
			this.weaponDispSub = 0;
			for (int i = 0; i < this.blendShapeList.Count; i++)
			{
				this.SetFaceData((CharaModelHandle.BlendShapeIndex)i, this.blendShapeList[i].val);
			}
			int num5 = 0;
			while (num5 < CharaModelHandle.FACE_LIST.GetLength(0) && num5 < this.flagList.Count && (this.enabledFaceMotion || num5 <= 0))
			{
				Transform transform = this.flagList[num5];
				this.SetFaceData(CharaModelHandle.FACE_LIST[num5, 0], transform.localScale.x - 1f);
				this.SetFaceData(CharaModelHandle.FACE_LIST[num5, 1], transform.localScale.y - 1f);
				this.SetFaceData(CharaModelHandle.FACE_LIST[num5, 2], transform.localScale.z - 1f);
				num5++;
			}
			using (Dictionary<string, List<GameObject>>.ValueCollection.Enumerator enumerator = this.modelList.Values.GetEnumerator())
			{
				IL_07C0:
				while (enumerator.MoveNext())
				{
					List<GameObject> list = enumerator.Current;
					int j = 0;
					bool flag2 = false;
					while (j < list.Count)
					{
						GameObject gameObject = list[j++];
						if (gameObject == null)
						{
							IL_07B5:
							while (j < list.Count)
							{
								list[j++].SetActive(!flag2);
							}
							goto IL_07C0;
						}
						flag2 |= gameObject.activeSelf;
					}
					goto IL_07B5;
				}
			}
			for (int k = 0; k < this.weaponList.Count; k++)
			{
				if (this.weaponList[k] != null)
				{
					this.weaponList[k].SetActive(this.weaponActive && (k == this.weaponDispA || k == this.weaponDispB));
				}
			}
			for (int l = 0; l < this.parts.Count; l++)
			{
				Color color = this.partsColor[l];
				if (this.partsAnim[l] > 0f)
				{
					List<float> list2 = this.partsTime;
					int m = l;
					if ((list2[m] += TimeManager.DeltaTime / this.partsAnim[l]) > 1f)
					{
						list2 = this.partsTime;
						m = l;
						list2[m] -= 2f;
					}
					color.a = Mathf.Abs(this.partsTime[l]);
				}
				Material[] array = this.parts[l].materials;
				for (int m = 0; m < array.Length; m++)
				{
					array[m].SetColor("_PartsColor", color);
				}
			}
			if (this.mouthCtrl != null)
			{
				float num6 = this.mouthAngle;
				if (this.mouthFollowObj)
				{
					this.mouthCtrl.LookAt(new Vector3(this.mouthFollowObj.position.x, this.mouthCtrl.position.y, this.mouthFollowObj.position.z));
					num6 = this.SetAngle(this.mouthCtrl.localEulerAngles.x, CharaModelHandle.mouthFollowStart, CharaModelHandle.mouthFollowEnd, CharaModelHandle.mouthAngleMinus, CharaModelHandle.mouthAnglePlus);
					this.mouthFollowFade = CharaModelHandle.followSpeed;
				}
				if (this.mouthFollowFade > 1f)
				{
					num6 = this.mouthAngleOld + Mathf.DeltaAngle(this.mouthAngleOld, num6) / this.mouthFollowFade;
					this.mouthFollowFade -= 1f;
				}
				this.mouthCtrl.localEulerAngles = new Vector3(this.mouthAngleOld = num6, 0f, 0f);
			}
			if (this.partsDataBody.animationKeyList.ContainsKey(this.motionName) && this.partsDataBody.animationKeyList[this.motionName])
			{
				foreach (KeyValuePair<Transform, Transform> keyValuePair in this.weaponCmnOff)
				{
					keyValuePair.Key.position = keyValuePair.Value.position;
					keyValuePair.Key.rotation = keyValuePair.Value.rotation;
				}
			}
			Dictionary<Transform, Vector3> dictionary = new Dictionary<Transform, Vector3>();
			foreach (Transform transform2 in this.wrist)
			{
				dictionary.Add(transform2, transform2.position);
			}
			List<Transform> list3 = new List<Transform>(this.crossWeapon.Keys);
			foreach (Transform transform3 in list3)
			{
				float num7 = 999999f;
				foreach (Transform transform4 in this.wrist)
				{
					float num8 = Vector3.Magnitude(transform3.position - transform4.position);
					if (num7 > num8)
					{
						num7 = num8;
						this.crossWeapon[transform3] = transform4;
					}
				}
			}
			float num9 = this.crossFadeL;
			if (num9 > 0f)
			{
				num9 = Mathf.Clamp01((this.crossFadeL -= TimeManager.DeltaTime) / num9);
			}
			else
			{
				num9 = 0f;
			}
			list3 = new List<Transform>(this.crossRoot.Keys);
			if (flag)
			{
				foreach (Transform transform5 in list3)
				{
					this.crossRoot[transform5] = transform5.localPosition;
				}
			}
			foreach (Transform transform6 in list3)
			{
				this.crossRoot[transform6] = (transform6.localPosition = Vector3.Lerp(transform6.localPosition, this.crossRoot[transform6], num9));
			}
			list3 = new List<Transform>(this.crossBoneL.Keys);
			foreach (Transform transform7 in list3)
			{
				this.crossBoneL[transform7] = (transform7.localRotation = Quaternion.Lerp(transform7.localRotation, this.crossBoneL[transform7], num9));
			}
			num9 = this.crossFade;
			if (num9 > 0f)
			{
				num9 = Mathf.Clamp01((this.crossFade -= TimeManager.DeltaTime) / num9);
			}
			else
			{
				num9 = 0f;
			}
			list3 = new List<Transform>(this.crossBone.Keys);
			foreach (Transform transform8 in list3)
			{
				this.crossBone[transform8] = (transform8.localRotation = Quaternion.Lerp(transform8.localRotation, this.crossBone[transform8], num9));
			}
			list3 = new List<Transform>(this.crossWeapon.Keys);
			foreach (Transform transform9 in list3)
			{
				if (!(this.crossWeapon[transform9] == null))
				{
					Vector3 vector2 = this.crossWeapon[transform9].position - dictionary[this.crossWeapon[transform9]];
					transform9.position += vector2;
				}
			}
			Quaternion rotation = this.partsDataBody.rootObj.transform.rotation;
			this.partsDataBody.rootObj.transform.rotation = Quaternion.identity;
			this.partsDataBody.referencer.LateUpdateOsage(this.thighL, this.thighR);
			for (int n = 0; n < this.partsDataEar.Length; n++)
			{
				CharaModelHandle.PartsData partsData = this.partsDataEar[n];
				if (partsData != null && !(partsData.rootObj == null) && !partsData.animation.enabled)
				{
					partsData.referencer.LateUpdateOsage(this.thighL, this.thighR);
				}
			}
			for (int num10 = 0; num10 < this.partsDataTail.Length; num10++)
			{
				CharaModelHandle.PartsData partsData2 = this.partsDataTail[num10];
				if (partsData2 != null && !(partsData2.rootObj == null) && !partsData2.animation.enabled)
				{
					partsData2.referencer.LateUpdateOsage(this.thighL, this.thighR);
				}
			}
			this.partsDataBody.rootObj.transform.rotation = rotation;
			if (this.mdlSubweapon != null)
			{
				Transform transform10 = null;
				if (this.weaponDispSub == 0)
				{
					transform10 = this.bonSubweapon;
				}
				else if (this.weaponDispSub == 1)
				{
					transform10 = this.bonWeaponA;
				}
				else if (this.weaponDispSub == 2)
				{
					transform10 = this.bonWeaponB;
				}
				if (transform10 != null)
				{
					this.mdlSubweapon.position = transform10.position;
					this.mdlSubweapon.rotation = transform10.rotation;
					this.mdlSubweapon.localScale = transform10.localScale;
				}
				this.mdlSubweapon.gameObject.SetActive(transform10 != null);
			}
			Vector3 vector3 = ((this.pelvis == null) ? base.transform.position : new Vector3(this.pelvis.position.x, base.transform.position.y, this.pelvis.position.z));
			if (this.shadow != null)
			{
				float num11 = this.GetHaraPos().y - this.shadowHeight;
				if (num11 > 1f)
				{
					num11 = (6f - num11) / 5f;
				}
				else
				{
					num11 = (1f + num11) / 2f;
				}
				num11 = Mathf.Clamp01(num11);
				this.shadow.transform.position = new Vector3(vector3.x, this.shadowHeight, vector3.z);
				this.shadow.transform.localScale = Vector3.one * this.shadowSize * (num11 + 1f);
				this.shadowMat.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, this.alpha * num11 * 0.5f));
			}
			if (this.shadowBone != null)
			{
				foreach (KeyValuePair<Transform, Transform> keyValuePair2 in this.shadowBone)
				{
					keyValuePair2.Key.position = keyValuePair2.Value.position;
					keyValuePair2.Key.rotation = keyValuePair2.Value.rotation;
					keyValuePair2.Key.localScale = keyValuePair2.Value.localScale;
				}
				this.shadowModel.transform.localScale = new Vector3(this.alpha, this.alpha, this.alpha);
			}
			if (this.shadowModel != null && this.shadowCamera != null)
			{
				this.shadowModel.transform.LookAt(this.shadowCamera.transform);
				this.shadowModel.transform.eulerAngles = new Vector3(0f, this.shadowModel.transform.eulerAngles.y, 0f);
			}
			List<KeyValuePair<EffectData, Vector3>> list4 = new List<KeyValuePair<EffectData, Vector3>>();
			foreach (KeyValuePair<EffectData, Vector3> keyValuePair3 in this.effect)
			{
				if (keyValuePair3.Key.IsFinishByAnimation())
				{
					list4.Add(keyValuePair3);
				}
				if (keyValuePair3.Key.effectObject != null)
				{
					Vector3 value = keyValuePair3.Value;
					if (value.y < -100f)
					{
						value.y = ((this.pelvis == null) ? 1f : this.pelvis.position.y);
					}
					keyValuePair3.Key.effectObject.transform.position = vector3 + value;
					int num12 = LayerMask.NameToLayer("Ignore Raycast");
					if (this.partsDataBody.rootObj.layer == num12 && keyValuePair3.Key.effectObject.layer != num12)
					{
						keyValuePair3.Key.effectObject.SetLayerRecursively(num12);
					}
				}
			}
			foreach (KeyValuePair<EffectData, Vector3> keyValuePair4 in list4)
			{
				this.effect.Remove(keyValuePair4);
				EffectManager.DestroyEffect(keyValuePair4.Key);
			}
			switch (this.charaEffectType)
			{
			case CharaModelHandle.CharaEffectType.EAR:
				this.UpdateEarEffect();
				break;
			case CharaModelHandle.CharaEffectType.SPARKLE:
				this.UpdateSparkleEffect();
				break;
			case CharaModelHandle.CharaEffectType.AURA:
				this.UpdateAuraEffect();
				break;
			case CharaModelHandle.CharaEffectType.OBJECT:
				this.UpdateObjectEffect();
				break;
			}
			if (this.accEff != null)
			{
				foreach (CharaModelHandle.AccEff accEff in this.accEff)
				{
					if (accEff != null)
					{
						if (this.modelActive && this.alpha > 0.5f && accEff.dsp && (accEff.pos != DataManagerCharaAccessory.DispPosition.Camera || (CharaModelHandle.accCamChara == base.transform && CharaModelHandle.accCamType == accEff.typ)))
						{
							if (accEff.eff == null)
							{
								accEff.eff = EffectManager.InstantiateEffect(accEff.name, base.transform, this.layer, 1f);
								accEff.eff.PlayEffect(false);
							}
							if (accEff.eff.effectObject != null)
							{
								if (accEff.pos == DataManagerCharaAccessory.DispPosition.Camera)
								{
									accEff.eff.effectObject.transform.position = ((EffectManager.BillboardCamera == null) ? Vector3.zero : EffectManager.BillboardCamera.transform.TransformPoint(0f, 0f, 0.5f));
									accEff.eff.effectObject.transform.rotation = ((EffectManager.BillboardCamera == null) ? Quaternion.identity : EffectManager.BillboardCamera.transform.rotation);
								}
								else
								{
									Transform transform11 = null;
									if (accEff.pos == DataManagerCharaAccessory.DispPosition.Body)
									{
										transform11 = this.pelvis;
									}
									else if (accEff.pos == DataManagerCharaAccessory.DispPosition.Head)
									{
										transform11 = this.headCtrl;
									}
									else if (accEff.pos == DataManagerCharaAccessory.DispPosition.LeftHand)
									{
										transform11 = this.childAll.Find((Transform itm) => itm.name == "j_wrist_l");
									}
									else if (accEff.pos == DataManagerCharaAccessory.DispPosition.RightHand)
									{
										transform11 = this.childAll.Find((Transform itm) => itm.name == "j_wrist_r");
									}
									if (transform11 == null)
									{
										transform11 = base.transform;
									}
									accEff.eff.effectObject.transform.position = transform11.position;
									if (this.isAccessoryAnchor)
									{
										accEff.eff.effectObject.transform.eulerAngles = transform11.eulerAngles + new Vector3(0f, 0f, 90f);
									}
								}
							}
						}
						else if (accEff.eff != null)
						{
							EffectManager.DestroyEffect(accEff.eff);
							accEff.eff = null;
						}
					}
				}
			}
			if (this.partsAlphaObj != null)
			{
				if (this.partsAlphaObj.localScale.x > 1f)
				{
					this.SetEyeColorByReferencer(this.partsAlphaObj.localScale.x - 1f);
				}
				if (this.partsAlphaObj.localScale.y > 1f)
				{
					Color white = Color.white;
					white.a = this.partsAlphaObj.localScale.y - 1f;
					this.SetCheekColor(white);
				}
			}
		}
	}

	// Token: 0x0600019B RID: 411 RVA: 0x0000FD00 File Offset: 0x0000DF00
	public void SetModelActive(bool act)
	{
		if (this.puyoObj != null)
		{
			if (this.modelActive != act)
			{
				this.puyoObj.SetActive(act);
			}
		}
		else if (this.isFinishInit && this.modelActive != act)
		{
			this.partsDataBody.rootObj.SetActive(act);
			if (this.shadowModel != null)
			{
				this.shadowModel.SetActive(act);
			}
			if (this.shadow != null)
			{
				this.shadow.SetActive(act && this.shadowActive);
			}
			if (act)
			{
				this.PlayAnimation(this.motionName, this.motionLoop, this.motionSpeed, 0f, 0f, false);
			}
		}
		this.modelActive = act;
	}

	// Token: 0x0600019C RID: 412 RVA: 0x0000FDC6 File Offset: 0x0000DFC6
	public void SetShadowActive(bool act)
	{
		if (this.isFinishInit && this.shadowActive != act && this.shadow != null)
		{
			this.shadow.SetActive(act && this.modelActive);
		}
		this.shadowActive = act;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0000FE05 File Offset: 0x0000E005
	public void SetWeaponActive(bool act)
	{
		this.weaponActive = act;
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000FE0E File Offset: 0x0000E00E
	public bool IsModelActive()
	{
		return this.modelActive;
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0000FE16 File Offset: 0x0000E016
	public void SetPuyoTex(string tex)
	{
		if (this.puyoObj != null && string.IsNullOrEmpty(this.puyoReq) && this.puyoTex != tex)
		{
			this.puyoReq = tex;
			AssetManager.LoadAssetData(this.puyoReq, AssetManager.OWNER.CharaModel, 0, null);
		}
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0000FE56 File Offset: 0x0000E056
	public void PlayAnimation(CharaMotionDefine.ActKey key, bool loop = false, float speed = 1f, float crossfade = 0f, float crossfadeL = 0f, bool turn = false)
	{
		this.PlayAnimation(key.ToString(), loop, speed, crossfade, crossfadeL, turn);
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x0000FE74 File Offset: 0x0000E074
	public void PlayAnimation(string key, bool loop = false, float speed = 1f, float crossfade = 0f, float crossfadeL = 0f, bool turn = false)
	{
		this.motionReq.name = (this.initializeParam.isViewer ? key : (this.initializeParam.longSkirt ? this.skirt2long(key) : this.skirt2short(key)));
		this.motionReq.loop = loop;
		this.motionReq.speed = speed;
		this.motionReq.fade = crossfade;
		this.motionReq.fadeL = crossfadeL;
		this.motionReq.turn = turn;
		this.motionEnd = false;
		this.motionPos.y = -1f;
		this.motionRot = -1f;
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x0000FF1A File Offset: 0x0000E11A
	public bool IsCurrentAnimation(CharaMotionDefine.ActKey key)
	{
		return this.IsCurrentAnimation(key.ToString());
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x0000FF30 File Offset: 0x0000E130
	public bool IsCurrentAnimation(string key)
	{
		string text = (string.IsNullOrEmpty(this.motionReq.name) ? this.motionName : this.motionReq.name);
		if (!this.initializeParam.isViewer)
		{
			return this.skirt2long(text) == this.skirt2long(key);
		}
		return text == key;
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0000FF8B File Offset: 0x0000E18B
	public bool IsLoopAnimation()
	{
		if (!string.IsNullOrEmpty(this.motionReq.name))
		{
			return this.motionReq.loop;
		}
		return this.motionLoop;
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x0000FFB1 File Offset: 0x0000E1B1
	public void SetLoopAnimation(bool sw)
	{
		this.motionLoop = sw;
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0000FFBA File Offset: 0x0000E1BA
	public void SetAnimationSpeed(float spd)
	{
		this.motionSpeed = spd;
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x0000FFC3 File Offset: 0x0000E1C3
	public void SetPosition(Vector3 pos)
	{
		this.motionPos = pos;
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0000FFCC File Offset: 0x0000E1CC
	public void SetRotation(float rot)
	{
		this.motionRot = rot;
		while (this.motionRot < 0f)
		{
			this.motionRot += 360f;
		}
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0000FFF8 File Offset: 0x0000E1F8
	public float GetAnimationTime(string key = null)
	{
		if (!this.isFinishInit)
		{
			return 0f;
		}
		if (this.initializeParam.isViewer)
		{
			if (!string.IsNullOrEmpty(key) && key != this.motionName)
			{
				return 0f;
			}
		}
		else if (!string.IsNullOrEmpty(key) && this.skirt2long(key) != this.skirt2long(this.motionName))
		{
			return 0f;
		}
		SimpleAnimation.State state = this.partsDataBody.animation.GetState(this.motionName);
		if (state == null || state.clip == null || state.clip.length < 0.0001f)
		{
			return 1f;
		}
		return Mathf.Clamp01(state.time / state.clip.length);
	}

	// Token: 0x060001AA RID: 426 RVA: 0x000100BC File Offset: 0x0000E2BC
	public void SetAnimationTime(float tim)
	{
		if (!this.isFinishInit)
		{
			return;
		}
		CharaModelHandle.PartsData partsData = this.partsDataBody;
		if (partsData != null)
		{
			SimpleAnimation.State state = partsData.animation.GetState(this.motionName);
			if (state != null)
			{
				state.time = tim;
			}
		}
		for (int i = 0; i < this.partsDataEar.Length; i++)
		{
			partsData = this.partsDataEar[i];
			if (partsData != null && !(partsData.rootObj == null) && partsData.animation.enabled)
			{
				SimpleAnimation.State state2 = partsData.animation.GetState(this.motionName);
				if (state2 != null)
				{
					state2.time = tim;
				}
			}
		}
		for (int j = 0; j < this.partsDataTail.Length; j++)
		{
			partsData = this.partsDataTail[j];
			if (partsData != null && !(partsData.rootObj == null) && partsData.animation.enabled)
			{
				SimpleAnimation.State state3 = partsData.animation.GetState(this.motionName);
				if (state3 != null)
				{
					state3.time = tim;
				}
			}
		}
	}

	// Token: 0x060001AB RID: 427 RVA: 0x000101AC File Offset: 0x0000E3AC
	public float GetAnimationLength(string key = null)
	{
		if (!this.isFinishInit)
		{
			return 0f;
		}
		if (this.initializeParam.isViewer)
		{
			if (!string.IsNullOrEmpty(key) && key != this.motionName)
			{
				return 0f;
			}
		}
		else if (!string.IsNullOrEmpty(key) && this.skirt2long(key) != this.skirt2long(this.motionName))
		{
			return 0f;
		}
		SimpleAnimation.State state = this.partsDataBody.animation.GetState(this.motionName);
		if (state != null && !(state.clip == null))
		{
			return state.clip.length;
		}
		return 0f;
	}

	// Token: 0x060001AC RID: 428 RVA: 0x00010254 File Offset: 0x0000E454
	public CharaMotionDefine.ActKey GetCurrentAnimation()
	{
		CharaMotionDefine.ActKey actKey = CharaMotionDefine.ActKey.INVALID;
		string text = (string.IsNullOrEmpty(this.motionReq.name) ? this.motionName : this.motionReq.name);
		if (!this.initializeParam.isViewer)
		{
			text = this.skirt2short(text);
		}
		try
		{
			actKey = (CharaMotionDefine.ActKey)Enum.Parse(typeof(CharaMotionDefine.ActKey), text);
		}
		catch
		{
			actKey = CharaMotionDefine.ActKey.INVALID;
		}
		return actKey;
	}

	// Token: 0x060001AD RID: 429 RVA: 0x000102D0 File Offset: 0x0000E4D0
	public bool IsSameAnimation(CharaMotionDefine.ActKey a, CharaMotionDefine.ActKey b)
	{
		bool flag = true;
		SimpleAnimation.State state = this.partsDataBody.animation.GetState(a.ToString());
		SimpleAnimation.State state2 = this.partsDataBody.animation.GetState(b.ToString());
		if (state != state2)
		{
			if (state == null || state2 == null)
			{
				flag = false;
			}
			else if (state.clip != state2.clip)
			{
				if (state.clip == null || state2.clip == null)
				{
					flag = false;
				}
				else if (state.clip.name != state2.clip.name)
				{
					flag = false;
				}
			}
		}
		return flag;
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0001037C File Offset: 0x0000E57C
	public void PlayAnimationByAuth(string key, float startTime, float speed, bool loop)
	{
		this.motionReq.name = "";
		this.motionName = key;
		this.motionLoop = loop;
		this.motionEnd = false;
		this.motionPos.y = -1f;
		this.motionRot = -1f;
		this.motionLength = (this.motionTime = (this.motionSpeed = 0f));
		this.motionTimeReq = -1f;
		this.crossFade = (this.crossFadeL = 0f);
		this.headFollowFade = (this.eyeFollowFade = (this.mouthFollowFade = 0f));
		if (this.isFinishInit)
		{
			CharaModelReferencer.RefAnime refAnime = this.partsDataBody.referencer.refAnimationObj.GetComponent<CharaModelReferencer>().prefabAnimeList.Find((CharaModelReferencer.RefAnime item) => item.name == this.motionName);
			if (this.partsDataBody.animation.enabled = this.partsDataBody.animationKeyList.ContainsKey(this.motionName))
			{
				this.partsDataBody.animation.ExPlayAnimation(this.motionName, startTime, speed);
			}
			this.partsDataBody.referencer.SetRootPos(this.rootResetMotion.Contains(this.motionName));
			this.earTyp = 0;
			this.earTypBef = 0;
			this.earTypFrm = 0;
			this.earTypAft = 0;
			if (refAnime != null)
			{
				this.earTyp = (this.earTypBef = refAnime.earBefore);
				this.earTypFrm = refAnime.earFrame;
				this.earTypAft = refAnime.earAfter;
			}
			for (int i = 0; i < this.partsDataEar.Length; i++)
			{
				CharaModelHandle.PartsData partsData = this.partsDataEar[i];
				if (partsData != null)
				{
					if (partsData.rootObj != null)
					{
						partsData.rootObj.SetActive(i == this.earTyp);
					}
					if (partsData.animation.enabled = partsData.animationKeyList.ContainsKey(this.motionName))
					{
						partsData.animation.ExPlayAnimation(this.motionName, startTime, speed);
					}
				}
			}
			this.tailTyp = 0;
			this.tailTypBef = 0;
			this.tailTypFrm = 0;
			this.tailTypAft = 0;
			if (refAnime != null)
			{
				this.tailTyp = (this.tailTypBef = refAnime.tailBefore);
				this.tailTypFrm = refAnime.tailFrame;
				this.tailTypAft = refAnime.tailAfter;
			}
			for (int j = 0; j < this.partsDataTail.Length; j++)
			{
				CharaModelHandle.PartsData partsData2 = this.partsDataTail[j];
				if (partsData2 != null)
				{
					if (partsData2.rootObj != null)
					{
						partsData2.rootObj.SetActive(j == this.tailTyp);
					}
					if (partsData2.animation.enabled = partsData2.animationKeyList.ContainsKey(this.motionName))
					{
						partsData2.animation.ExPlayAnimation(this.motionName, startTime, speed);
					}
				}
			}
		}
	}

	// Token: 0x060001AF RID: 431 RVA: 0x0001065A File Offset: 0x0000E85A
	public void SetAuthPlayer(AuthPlayer ap)
	{
		this.authPlayer = ap;
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00010663 File Offset: 0x0000E863
	public bool IsPlaying()
	{
		return this.isFinishInit && (this.partsDataBody.animation.enabled || !string.IsNullOrEmpty(this.motionReq.name)) && !this.motionEnd;
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x0001069C File Offset: 0x0000E89C
	public List<SimpleAnimation> GetEnableAnimationList()
	{
		List<SimpleAnimation> list = new List<SimpleAnimation>();
		if (this.isFinishInit)
		{
			list.Add(this.partsDataBody.animation);
			CharaModelHandle.PartsData partsData = ((this.earTyp < 0 || this.earTyp >= this.partsDataEar.Length) ? null : this.partsDataEar[this.earTyp]);
			list.Add((partsData == null) ? null : partsData.animation);
			partsData = ((this.tailTyp < 0 || this.tailTyp >= this.partsDataTail.Length) ? null : this.partsDataTail[this.tailTyp]);
			list.Add((partsData == null) ? null : partsData.animation);
		}
		return list;
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x00010744 File Offset: 0x0000E944
	public void SetEyeColor(Color color)
	{
		this.eyeColor = color;
		if (this.isFinishInit)
		{
			foreach (Renderer renderer in this.eyeBall)
			{
				Material[] array = renderer.materials;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetColor("_EyeColor", this.eyeColor);
				}
			}
		}
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x000107C8 File Offset: 0x0000E9C8
	public void SetEyeColorByReferencer(float a)
	{
		Color color = this.partsDataBody.referencer.refAnimationObj.GetComponent<CharaModelReferencer>().eyeColor;
		color.a = a;
		this.SetEyeColor(color);
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x000107FF File Offset: 0x0000E9FF
	public Color GetEyeColor()
	{
		return this.eyeColor;
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x00010808 File Offset: 0x0000EA08
	public void SetCheekColor(Color color)
	{
		this.cheekColor = color;
		if (this.isFinishInit && this.cheek != null)
		{
			Material[] array = this.cheek.materials;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetColor("_BaseColor", this.cheekColor);
			}
		}
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x0001085F File Offset: 0x0000EA5F
	public Color GetCheekColor()
	{
		return this.cheekColor;
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x00010868 File Offset: 0x0000EA68
	public void SetFaceParam(List<float> param)
	{
		if (this.isFinishInit)
		{
			for (int i = 0; i < this.blendShapeList.Count; i++)
			{
				if (i < param.Count)
				{
					this.blendShapeList[i].val = param[i];
				}
			}
		}
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x000108B4 File Offset: 0x0000EAB4
	public List<float> GetFaceParam()
	{
		List<float> list = new List<float>();
		if (this.isFinishInit)
		{
			for (int i = 0; i < this.blendShapeList.Count; i++)
			{
				list.Add(this.blendShapeList[i].val);
			}
		}
		return list;
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x000108FD File Offset: 0x0000EAFD
	public float GetMouthAngle()
	{
		if (!this.isFinishInit || !(this.mouthCtrl != null))
		{
			return 0f;
		}
		return Mathf.DeltaAngle(0f, this.mouthCtrl.localEulerAngles.x);
	}

	// Token: 0x060001BA RID: 442 RVA: 0x00010938 File Offset: 0x0000EB38
	public void SetFacePackData(FacePackData packData, Transform mouthFollowObj = null, Transform eyeFollowObj = null)
	{
		if (packData == null)
		{
			return;
		}
		this.SetFaceParam(new List<float>(packData.blendShapeParam));
		this.mouthFollowObj = (packData.isMouseFollow ? mouthFollowObj : null);
		this.eyeFollowObj = (packData.isEyeFollow ? eyeFollowObj : null);
		this.headFollowObj = (packData.isEyeFollow ? eyeFollowObj : null);
		this.SetCheekColor(packData.cheekColor);
	}

	// Token: 0x060001BB RID: 443 RVA: 0x000109A2 File Offset: 0x0000EBA2
	public void SetLayer(int lay)
	{
		this.layer = lay;
		if (this.puyoObj != null)
		{
			this.setPuyo();
			return;
		}
		if (this.isFinishInit)
		{
			this.SetEnv();
		}
	}

	// Token: 0x060001BC RID: 444 RVA: 0x000109CE File Offset: 0x0000EBCE
	public void SetLayer(string lay)
	{
		this.SetLayer(LayerMask.NameToLayer(lay));
	}

	// Token: 0x060001BD RID: 445 RVA: 0x000109DC File Offset: 0x0000EBDC
	public int GetLayer()
	{
		return this.layer;
	}

	// Token: 0x060001BE RID: 446 RVA: 0x000109E4 File Offset: 0x0000EBE4
	public int GetPartsBodyLayer()
	{
		return this.partsDataBody.rootObj.layer;
	}

	// Token: 0x060001BF RID: 447 RVA: 0x000109F6 File Offset: 0x0000EBF6
	public void SetAlpha(float a)
	{
		this.alpha = a;
		this.fade = 0f;
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x00010A0A File Offset: 0x0000EC0A
	public float GetAlpha()
	{
		return this.alpha;
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x00010A14 File Offset: 0x0000EC14
	public void FadeIn(float time)
	{
		this.fade = time;
		if (time < 0.0001f)
		{
			this.SetAlpha(1f);
		}
		if (this.puyoObj != null)
		{
			this.setPuyo();
			return;
		}
		if (this.isFinishInit)
		{
			this.SetEnv();
		}
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x00010A60 File Offset: 0x0000EC60
	public void FadeOut(float time, UnityAction action = null)
	{
		this.fade = time;
		if (time < 0.0001f)
		{
			this.SetAlpha(0f);
		}
		else
		{
			this.fade = -this.fade;
		}
		if (this.puyoObj != null)
		{
			this.setPuyo();
		}
		else if (this.isFinishInit)
		{
			this.SetEnv();
		}
		if (action != null)
		{
			this.fadeOutEnd = action;
		}
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x00010AC6 File Offset: 0x0000ECC6
	public bool IsDisp()
	{
		return this.fade > 0f || (this.fade == 0f && this.alpha > 0f);
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x00010AF4 File Offset: 0x0000ECF4
	public HashSet<Camera> GetViewCamera()
	{
		if (this.viewCamera == null)
		{
			this.viewCamera = new HashSet<Camera>();
			foreach (CullingCheck cullingCheck in this.cullingList)
			{
				foreach (Camera camera in cullingCheck.getCamera())
				{
					this.viewCamera.Add(camera);
				}
			}
		}
		return this.viewCamera;
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x00010BA0 File Offset: 0x0000EDA0
	public void SetNeighboringAlpha(Vector3 na)
	{
		this.NeighboringAlpha = new Vector4(na.x, na.y, na.z, this.NeighboringAlpha.w);
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x00010BCA File Offset: 0x0000EDCA
	public void SetMatCap(float mc)
	{
		if (this.matCap >= 0f)
		{
			this.matCap = Mathf.Clamp01(mc);
		}
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x00010BE5 File Offset: 0x0000EDE5
	public float GetMatCap()
	{
		if (this.matCap < 0f)
		{
			return 0f;
		}
		return this.matCap;
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x00010C00 File Offset: 0x0000EE00
	public Vector3 GetHaraPos()
	{
		if (!(this.pelvis == null))
		{
			return this.pelvis.position;
		}
		return base.transform.position;
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x00010C27 File Offset: 0x0000EE27
	public Vector3 GetHeadPos()
	{
		if (!(this.headCtrl == null))
		{
			return this.headCtrl.position;
		}
		return this.GetHaraPos();
	}

	// Token: 0x060001CA RID: 458 RVA: 0x00010C4C File Offset: 0x0000EE4C
	public Vector3 GetNodePos(string node)
	{
		Transform transform = this.childAll.Find((Transform itm) => itm.name == node);
		if (!(transform == null))
		{
			return transform.position;
		}
		return this.GetHaraPos();
	}

	// Token: 0x060001CB RID: 459 RVA: 0x00010C94 File Offset: 0x0000EE94
	public Transform GetNodeTransform(string node)
	{
		return this.childAll.Find((Transform itm) => itm.name == node);
	}

	// Token: 0x060001CC RID: 460 RVA: 0x00010CC5 File Offset: 0x0000EEC5
	public Vector3 GetHaraLocalPos()
	{
		if (!(this.pelvis == null))
		{
			return this.pelvis.localPosition;
		}
		return base.transform.localPosition;
	}

	// Token: 0x060001CD RID: 461 RVA: 0x00010CEC File Offset: 0x0000EEEC
	public Vector3 GetCharaScale()
	{
		return this.partsDataBody.rootObj.transform.Find("root").localScale;
	}

	// Token: 0x060001CE RID: 462 RVA: 0x00010D10 File Offset: 0x0000EF10
	public void SetEnableUpdateOffscreen()
	{
		SkinnedMeshRenderer[] componentsInChildren = this.partsDataBody.rootObj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].updateWhenOffscreen = true;
		}
	}

	// Token: 0x060001CF RID: 463 RVA: 0x00010D48 File Offset: 0x0000EF48
	public void SetScaleOne()
	{
		if (this.isFinishInit)
		{
			this.partsDataBody.referencer.setScaleOne();
			foreach (CharaModelHandle.PartsData partsData in this.partsDataEar)
			{
				if (partsData != null && partsData.rootObj != null)
				{
					partsData.referencer.setScaleOne();
				}
			}
			foreach (CharaModelHandle.PartsData partsData2 in this.partsDataTail)
			{
				if (partsData2 != null && partsData2.rootObj != null)
				{
					partsData2.referencer.setScaleOne();
				}
			}
		}
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x00010DD8 File Offset: 0x0000EFD8
	public float GetMotionTime(bool disp = false)
	{
		if (!string.IsNullOrEmpty(this.motionName))
		{
			float num = (disp ? 30f : 1f);
			if (this.motionLength > 0f)
			{
				if (!disp)
				{
					return this.motionTime * num;
				}
				return (float)((int)(this.motionTime * num));
			}
			else
			{
				SimpleAnimation.State state = this.partsDataBody.animation[this.motionName];
				if (state != null)
				{
					float time = state.time;
					if (!disp)
					{
						return time * num;
					}
					return (float)((int)(time * num));
				}
			}
		}
		return 0f;
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x00010E59 File Offset: 0x0000F059
	public void SetMotionTime(float time)
	{
		if (this.motionTime != time)
		{
			this.motionTimeReq = time;
		}
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x00010E6B File Offset: 0x0000F06B
	public void SetMotionFrame(int frame)
	{
		if ((int)(this.motionTime * 30f) != frame)
		{
			this.motionTimeReq = (float)frame / 30f;
		}
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x00010E8C File Offset: 0x0000F08C
	public float GetMotionLength(bool disp = false)
	{
		if (!string.IsNullOrEmpty(this.motionName))
		{
			float num = (disp ? 30f : 1f);
			if (this.motionLength > 0f)
			{
				if (!disp)
				{
					return this.motionLength * num;
				}
				return (float)((int)(this.motionLength * num));
			}
			else
			{
				SimpleAnimation.State state = this.partsDataBody.animation[this.motionName];
				if (state != null)
				{
					float length = state.length;
					if (!disp)
					{
						return length * num;
					}
					return (float)((int)(length * num));
				}
			}
		}
		return 0f;
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x00010F0D File Offset: 0x0000F10D
	public void UpdateCharaObjByAuth(Transform charaObj, string suffix)
	{
		if (charaObj != null)
		{
			this.SetFlagList(charaObj, suffix);
			this.eyeMotionType = CharaModelHandle.EyeMotType.ENABLE_AUTH;
			this.SetEyeObj(charaObj, suffix);
			this.partsAlphaObj = charaObj.Find("flag_alpha_a" + suffix);
		}
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x00010F46 File Offset: 0x0000F146
	public void SetEffect(EffectData eff)
	{
		this.effect.Add(new KeyValuePair<EffectData, Vector3>(eff, (eff.effectObject == null) ? Vector3.zero : eff.effectObject.transform.localPosition));
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x00010F80 File Offset: 0x0000F180
	public void DestroyEffect(EffectData eff)
	{
		this.effect.RemoveAll((KeyValuePair<EffectData, Vector3> itm) => itm.Key == eff);
		EffectManager.DestroyEffect(eff);
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x00010FC0 File Offset: 0x0000F1C0
	public void DispAccessory(int typ, bool sw, bool cf = false)
	{
		CharaModelHandle.AccEff accEff = this.accEff.Find((CharaModelHandle.AccEff itm) => itm != null && itm.typ == typ);
		if (accEff != null)
		{
			accEff.dsp = sw;
			if (accEff.pos == DataManagerCharaAccessory.DispPosition.Camera)
			{
				if (sw)
				{
					if (CharaModelHandle.accCamChara == null || cf)
					{
						CharaModelHandle.accCamChara = base.transform;
						CharaModelHandle.accCamType = accEff.typ;
						return;
					}
				}
				else if (CharaModelHandle.accCamChara == base.transform && CharaModelHandle.accCamType == accEff.typ)
				{
					CharaModelHandle.accCamChara = null;
				}
			}
		}
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x00011054 File Offset: 0x0000F254
	private void UpdateEarEffect()
	{
		if (this.charaEffect == null)
		{
			return;
		}
		if (this.modelActive && this.alpha > 0.5f)
		{
			while (this.charaEffect.Count < 3)
			{
				EffectData effectData = EffectManager.InstantiateEffect(this.charaEffectName, base.transform, this.layer, 1f);
				effectData.PlayEffect(false);
				this.charaEffect.Add(effectData);
			}
			Transform transform = this.childEar.Find((Transform itm) => itm.name == "j_osg_ear_b_r");
			if (transform != null && this.charaEffect[0].effectObject != null)
			{
				this.charaEffect[0].effectObject.transform.position = transform.position;
			}
			transform = this.childEar.Find((Transform itm) => itm.name == "j_osg_ear_b_l");
			if (transform != null && this.charaEffect[1].effectObject != null)
			{
				this.charaEffect[1].effectObject.transform.position = transform.position;
			}
			transform = this.childTail.Find((Transform itm) => itm.name == "j_osg_tail_c");
			if (transform != null && this.charaEffect[2].effectObject != null)
			{
				this.charaEffect[2].effectObject.transform.position = transform.position;
				return;
			}
		}
		else if (this.charaEffect.Count > 0)
		{
			foreach (EffectData effectData2 in this.charaEffect)
			{
				EffectManager.DestroyEffect(effectData2);
			}
			this.charaEffect = new List<EffectData>();
		}
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x00011270 File Offset: 0x0000F470
	private void UpdateSparkleEffect()
	{
		if (this.charaEffect == null)
		{
			return;
		}
		if (this.modelActive && this.alpha > 0.5f)
		{
			while (this.charaEffect.Count < 1)
			{
				EffectData effectData = EffectManager.InstantiateEffect(this.charaEffectName, base.transform, this.layer, 1f);
				effectData.PlayEffect(false);
				this.charaEffect.Add(effectData);
			}
			Transform transform = this.childAll.Find((Transform itm) => itm.name == "j_chest");
			if (transform != null && this.charaEffect[0].effectObject != null)
			{
				this.charaEffect[0].effectObject.transform.position = transform.position;
				return;
			}
		}
		else if (this.charaEffect.Count > 0)
		{
			foreach (EffectData effectData2 in this.charaEffect)
			{
				EffectManager.DestroyEffect(effectData2);
			}
			this.charaEffect = new List<EffectData>();
		}
	}

	// Token: 0x060001DA RID: 474 RVA: 0x000113B0 File Offset: 0x0000F5B0
	private void UpdateAuraEffect()
	{
		if (this.charaEffect == null)
		{
			return;
		}
		if (this.modelActive && this.alpha > 0.5f)
		{
			while (this.charaEffect.Count < 1)
			{
				EffectData effectData = EffectManager.InstantiateEffect(this.charaEffectName, base.transform, this.layer, 1f);
				effectData.PlayEffect(false);
				this.charaEffect.Add(effectData);
			}
			if (this.pelvis != null && this.charaEffect[0].effectObject != null)
			{
				this.charaEffect[0].effectObject.transform.position = this.pelvis.position;
				return;
			}
		}
		else if (this.charaEffect.Count > 0)
		{
			foreach (EffectData effectData2 in this.charaEffect)
			{
				EffectManager.DestroyEffect(effectData2);
			}
			this.charaEffect = new List<EffectData>();
		}
	}

	// Token: 0x060001DB RID: 475 RVA: 0x000114D0 File Offset: 0x0000F6D0
	private void UpdateObjectEffect()
	{
		if (this.charaEffect == null)
		{
			return;
		}
		if (this.modelActive && this.alpha > 0.5f)
		{
			Transform transform = this.childAll.Find((Transform itm) => itm.name == "j_chest");
			while (this.charaEffect.Count < 1)
			{
				EffectData effectData = EffectManager.InstantiateEffect(this.charaEffectName, base.transform, this.layer, 1f);
				effectData.PlayEffect(false);
				this.charaEffect.Add(effectData);
			}
			if (transform != null && this.charaEffect[0].effectObject != null)
			{
				this.charaEffect[0].effectObject.transform.position = transform.position;
				this.charaEffect[0].effectObject.transform.eulerAngles = transform.eulerAngles + new Vector3(0f, 0f, 90f);
				return;
			}
		}
		else if (this.charaEffect.Count > 0)
		{
			foreach (EffectData effectData2 in this.charaEffect)
			{
				EffectManager.DestroyEffect(effectData2);
			}
			this.charaEffect = new List<EffectData>();
		}
	}

	// Token: 0x040001D1 RID: 465
	public static readonly string CHARA_MODEL_PATH = "Charas/Model/";

	// Token: 0x040001D2 RID: 466
	public static readonly string OPTION_SUFFIX_EAR = "_ear";

	// Token: 0x040001D3 RID: 467
	public static readonly string OPTION_SUFFIX_TAIL = "_tail";

	// Token: 0x040001DD RID: 477
	private bool haraOffOld;

	// Token: 0x040001DE RID: 478
	private static readonly float mouthFollowStart = 20f;

	// Token: 0x040001DF RID: 479
	private static readonly float mouthFollowEnd = 60f;

	// Token: 0x040001E0 RID: 480
	private static readonly float mouthAngleMinus = -8.3f;

	// Token: 0x040001E1 RID: 481
	private static readonly float mouthAnglePlus = 8.3f;

	// Token: 0x040001E2 RID: 482
	private static readonly float eyeFollowStart = 10f;

	// Token: 0x040001E3 RID: 483
	private static readonly float eyeFollowEnd = 50f;

	// Token: 0x040001E4 RID: 484
	private static readonly Vector3 eyeAngleMinusR = new Vector3(-23.5f, -10f, -3f);

	// Token: 0x040001E5 RID: 485
	private static readonly Vector3 eyeAnglePlusR = new Vector3(16f, 13f, 3f);

	// Token: 0x040001E6 RID: 486
	private static readonly Vector3 eyeAngleMinusL = new Vector3(-16f, -10f, -3f);

	// Token: 0x040001E7 RID: 487
	private static readonly Vector3 eyeAnglePlusL = new Vector3(23.5f, 13f, 3f);

	// Token: 0x040001E8 RID: 488
	private static readonly float headFollowStart = 15f;

	// Token: 0x040001E9 RID: 489
	private static readonly float headFollowEnd = 45f;

	// Token: 0x040001EA RID: 490
	private static readonly float headAngleMinus = -30f;

	// Token: 0x040001EB RID: 491
	private static readonly float headAnglePlus = 30f;

	// Token: 0x040001EC RID: 492
	private static readonly float followSpeed = 6f;

	// Token: 0x040001ED RID: 493
	private float mouthAngleOld;

	// Token: 0x040001EE RID: 494
	private Vector2 eyeAngleOldR;

	// Token: 0x040001EF RID: 495
	private Vector2 eyeAngleOldL;

	// Token: 0x040001F0 RID: 496
	private Quaternion headRotationOld;

	// Token: 0x040001F1 RID: 497
	private float mouthFollowFade;

	// Token: 0x040001F2 RID: 498
	private float eyeFollowFade;

	// Token: 0x040001F3 RID: 499
	private float headFollowFade;

	// Token: 0x040001F4 RID: 500
	private UnityAction fadeOutEnd;

	// Token: 0x040001F5 RID: 501
	private static readonly List<CharaModelHandle.WeaponOffsetParam> WeaponOffsetList = new List<CharaModelHandle.WeaponOffsetParam>
	{
		new CharaModelHandle.WeaponOffsetParam
		{
			weaponSuffix = "a_orb",
			offsetRootNode = "j_weapon_a",
			offsetPos = new Vector3(0.00861f, -0.155f, 0.01122f),
			offsetRot = Quaternion.Euler(55f, 7.5f, 0f)
		},
		new CharaModelHandle.WeaponOffsetParam
		{
			weaponSuffix = "b_orb",
			offsetRootNode = "j_weapon_b",
			offsetPos = new Vector3(0.00861f, 0.155f, 0.01122f),
			offsetRot = Quaternion.Euler(-55f, 7.5f, 0f)
		}
	};

	// Token: 0x040001F6 RID: 502
	private List<KeyValuePair<Transform, Transform>> weaponCmnOff;

	// Token: 0x040001FA RID: 506
	private CharaModelHandle.PartsData partsDataBody = new CharaModelHandle.PartsData();

	// Token: 0x040001FB RID: 507
	private CharaModelHandle.PartsData[] partsDataEar = new CharaModelHandle.PartsData[5];

	// Token: 0x040001FC RID: 508
	private CharaModelHandle.PartsData[] partsDataTail = new CharaModelHandle.PartsData[5];

	// Token: 0x040001FD RID: 509
	private int earTyp;

	// Token: 0x040001FE RID: 510
	private int earTypBef;

	// Token: 0x040001FF RID: 511
	private int earTypFrm;

	// Token: 0x04000200 RID: 512
	private int earTypAft;

	// Token: 0x04000201 RID: 513
	private int tailTyp;

	// Token: 0x04000202 RID: 514
	private int tailTypBef;

	// Token: 0x04000203 RID: 515
	private int tailTypFrm;

	// Token: 0x04000204 RID: 516
	private int tailTypAft;

	// Token: 0x04000205 RID: 517
	private List<CharaModelHandle.BlendShapeData> blendShapeList = new List<CharaModelHandle.BlendShapeData>();

	// Token: 0x04000206 RID: 518
	private Dictionary<string, List<GameObject>> modelList = new Dictionary<string, List<GameObject>>();

	// Token: 0x04000207 RID: 519
	private List<GameObject> weaponList = new List<GameObject>();

	// Token: 0x04000208 RID: 520
	private Transform bonWeaponA;

	// Token: 0x04000209 RID: 521
	private Transform bonWeaponB;

	// Token: 0x0400020A RID: 522
	private int weaponDispA = -1;

	// Token: 0x0400020B RID: 523
	private int weaponDispB = -1;

	// Token: 0x0400020C RID: 524
	private int weaponDispSub;

	// Token: 0x0400020D RID: 525
	private List<Transform> childAll;

	// Token: 0x0400020E RID: 526
	private List<Transform> childEar;

	// Token: 0x0400020F RID: 527
	private List<Transform> childTail;

	// Token: 0x04000210 RID: 528
	private Transform eyeBallL;

	// Token: 0x04000211 RID: 529
	private Transform eyeBallR;

	// Token: 0x04000212 RID: 530
	private Transform eyeCtrlL;

	// Token: 0x04000213 RID: 531
	private Transform eyeCtrlR;

	// Token: 0x04000214 RID: 532
	private List<Renderer> eyeBall = new List<Renderer>();

	// Token: 0x04000215 RID: 533
	private Color eyeColor = Color.clear;

	// Token: 0x04000216 RID: 534
	private Renderer cheek;

	// Token: 0x04000217 RID: 535
	private Color cheekColor = Color.white;

	// Token: 0x04000218 RID: 536
	private List<Renderer> parts = new List<Renderer>();

	// Token: 0x04000219 RID: 537
	private List<Color> partsColor = new List<Color>();

	// Token: 0x0400021A RID: 538
	private List<float> partsAnim = new List<float>();

	// Token: 0x0400021B RID: 539
	private List<float> partsTime = new List<float>();

	// Token: 0x0400021C RID: 540
	private Transform mouthCtrl;

	// Token: 0x0400021D RID: 541
	private Transform headCtrl;

	// Token: 0x0400021E RID: 542
	private Transform pelvis;

	// Token: 0x0400021F RID: 543
	private Transform upperBody;

	// Token: 0x04000220 RID: 544
	private Transform lowerBody;

	// Token: 0x04000221 RID: 545
	private Transform thighL;

	// Token: 0x04000222 RID: 546
	private Transform thighR;

	// Token: 0x04000223 RID: 547
	private List<Transform> wrist;

	// Token: 0x04000224 RID: 548
	private Transform mdlSubweapon;

	// Token: 0x04000225 RID: 549
	private Transform bonSubweapon;

	// Token: 0x04000226 RID: 550
	private Transform partsAlphaObj;

	// Token: 0x04000227 RID: 551
	public static readonly string[] FLAG_LIST = new string[] { "flag_weapon_a", "flag_brow_a", "flag_brow_b", "flag_brow_c", "flag_eye_a", "flag_eye_b", "flag_eye_c", "flag_mouth_a", "flag_mouth_b", "flag_mouth_c" };

	// Token: 0x04000228 RID: 552
	private List<Transform> flagList = new List<Transform>();

	// Token: 0x04000229 RID: 553
	private static readonly CharaModelHandle.BlendShapeIndex[,] FACE_LIST = new CharaModelHandle.BlendShapeIndex[,]
	{
		{
			CharaModelHandle.BlendShapeIndex.weapon_a,
			CharaModelHandle.BlendShapeIndex.weapon_b,
			CharaModelHandle.BlendShapeIndex.sub_weapon
		},
		{
			CharaModelHandle.BlendShapeIndex.brow_down_r,
			CharaModelHandle.BlendShapeIndex.brow_down_l,
			CharaModelHandle.BlendShapeIndex.brow_up_r
		},
		{
			CharaModelHandle.BlendShapeIndex.brow_up_l,
			CharaModelHandle.BlendShapeIndex.brow_sad_r,
			CharaModelHandle.BlendShapeIndex.brow_sad_l
		},
		{
			CharaModelHandle.BlendShapeIndex.brow_anger_r,
			CharaModelHandle.BlendShapeIndex.brow_anger_l,
			CharaModelHandle.BlendShapeIndex.Max
		},
		{
			CharaModelHandle.BlendShapeIndex.eye_close,
			CharaModelHandle.BlendShapeIndex.eye_smile,
			CharaModelHandle.BlendShapeIndex.eye_anger
		},
		{
			CharaModelHandle.BlendShapeIndex.eye_sad,
			CharaModelHandle.BlendShapeIndex.eye_wink_r,
			CharaModelHandle.BlendShapeIndex.eye_wink_l
		},
		{
			CharaModelHandle.BlendShapeIndex.eye_special_a,
			CharaModelHandle.BlendShapeIndex.eye_special_b,
			CharaModelHandle.BlendShapeIndex.Max
		},
		{
			CharaModelHandle.BlendShapeIndex.mouth_a,
			CharaModelHandle.BlendShapeIndex.mouth_i,
			CharaModelHandle.BlendShapeIndex.mouth_n
		},
		{
			CharaModelHandle.BlendShapeIndex.mouth_o,
			CharaModelHandle.BlendShapeIndex.mouth_smile,
			CharaModelHandle.BlendShapeIndex.mouth_sad
		},
		{
			CharaModelHandle.BlendShapeIndex.mouth_special_a,
			CharaModelHandle.BlendShapeIndex.mouth_special_b,
			CharaModelHandle.BlendShapeIndex.Max
		}
	};

	// Token: 0x0400022A RID: 554
	public static readonly CharaModelHandle.BlendShapeIndex[,] COLOR_LIST = new CharaModelHandle.BlendShapeIndex[,]
	{
		{
			CharaModelHandle.BlendShapeIndex.brow_down_r,
			CharaModelHandle.BlendShapeIndex.brow_down_l,
			CharaModelHandle.BlendShapeIndex.brow_up_r,
			CharaModelHandle.BlendShapeIndex.brow_up_l,
			CharaModelHandle.BlendShapeIndex.brow_sad_r
		},
		{
			CharaModelHandle.BlendShapeIndex.eye_close,
			CharaModelHandle.BlendShapeIndex.eye_smile,
			CharaModelHandle.BlendShapeIndex.eye_anger,
			CharaModelHandle.BlendShapeIndex.eye_sad,
			CharaModelHandle.BlendShapeIndex.eye_wink_r
		},
		{
			CharaModelHandle.BlendShapeIndex.mouth_a,
			CharaModelHandle.BlendShapeIndex.mouth_i,
			CharaModelHandle.BlendShapeIndex.mouth_n,
			CharaModelHandle.BlendShapeIndex.mouth_o,
			CharaModelHandle.BlendShapeIndex.mouth_smile
		}
	};

	// Token: 0x0400022B RID: 555
	private int layer;

	// Token: 0x0400022C RID: 556
	private float alpha;

	// Token: 0x0400022D RID: 557
	private float fade;

	// Token: 0x0400022E RID: 558
	private Vector4 NeighboringAlpha = new Vector4(0f, -1f, 0f, 1f);

	// Token: 0x0400022F RID: 559
	private float matCap = -1f;

	// Token: 0x04000230 RID: 560
	public bool camouflage;

	// Token: 0x04000231 RID: 561
	private float camouflageAlpha;

	// Token: 0x04000232 RID: 562
	private List<CullingCheck> cullingList;

	// Token: 0x04000233 RID: 563
	private List<Material> materials;

	// Token: 0x04000234 RID: 564
	private Dictionary<Transform, Quaternion> crossBone;

	// Token: 0x04000235 RID: 565
	private Dictionary<Transform, Quaternion> crossBoneL;

	// Token: 0x04000236 RID: 566
	private Dictionary<Transform, Vector3> crossRoot;

	// Token: 0x04000237 RID: 567
	private Dictionary<Transform, Transform> crossWeapon;

	// Token: 0x04000238 RID: 568
	private float crossFade;

	// Token: 0x04000239 RID: 569
	private float crossFadeL;

	// Token: 0x0400023A RID: 570
	private CharaModelHandle.MotionReq motionReq;

	// Token: 0x0400023B RID: 571
	private string motionName;

	// Token: 0x0400023C RID: 572
	private bool motionLoop;

	// Token: 0x0400023D RID: 573
	private float motionLength;

	// Token: 0x0400023E RID: 574
	private float motionTime;

	// Token: 0x0400023F RID: 575
	private float motionTimeReq;

	// Token: 0x04000240 RID: 576
	private float motionSpeed;

	// Token: 0x04000241 RID: 577
	private bool motionEnd;

	// Token: 0x04000242 RID: 578
	private Vector3 motionPos;

	// Token: 0x04000243 RID: 579
	private float motionRot;

	// Token: 0x04000244 RID: 580
	private GameObject shadow;

	// Token: 0x04000245 RID: 581
	private Material shadowMat;

	// Token: 0x04000248 RID: 584
	private GameObject shadowModel;

	// Token: 0x04000249 RID: 585
	private List<KeyValuePair<Transform, Transform>> shadowBone;

	// Token: 0x0400024A RID: 586
	private CullingCheck shadowCull;

	// Token: 0x0400024B RID: 587
	private Camera shadowCamera;

	// Token: 0x0400024C RID: 588
	private List<KeyValuePair<EffectData, Vector3>> effect = new List<KeyValuePair<EffectData, Vector3>>();

	// Token: 0x0400024D RID: 589
	public List<EffectData> charaEffect;

	// Token: 0x0400024E RID: 590
	private string charaEffectName = string.Empty;

	// Token: 0x0400024F RID: 591
	private CharaModelHandle.CharaEffectType charaEffectType;

	// Token: 0x04000250 RID: 592
	public bool isAccessoryAnchor;

	// Token: 0x04000251 RID: 593
	private List<CharaModelHandle.AccEff> accEff;

	// Token: 0x04000252 RID: 594
	private static Transform accCamChara = null;

	// Token: 0x04000253 RID: 595
	private static int accCamType = 0;

	// Token: 0x04000254 RID: 596
	private GameObject puyoObj;

	// Token: 0x04000255 RID: 597
	private string puyoTex;

	// Token: 0x04000256 RID: 598
	private string puyoReq;

	// Token: 0x04000257 RID: 599
	private float puyoBreath;

	// Token: 0x04000258 RID: 600
	private bool isFinishInit;

	// Token: 0x04000259 RID: 601
	private AuthPlayer authPlayer;

	// Token: 0x0400025A RID: 602
	private readonly List<KeyValuePair<string, string>> longSkirt = new List<KeyValuePair<string, string>>
	{
		new KeyValuePair<string, string>(CharaMotionDefine.ActKey.PVP_ENTRY.ToString(), CharaMotionDefine.ActKey.PVP_ENTRY_LONG.ToString()),
		new KeyValuePair<string, string>(CharaMotionDefine.ActKey.PVP_DEATH_ST.ToString(), CharaMotionDefine.ActKey.PVP_DEATH_ST_LONG.ToString()),
		new KeyValuePair<string, string>(CharaMotionDefine.ActKey.PVP_DEATH_LP.ToString(), CharaMotionDefine.ActKey.PVP_DEATH_LP_LONG.ToString()),
		new KeyValuePair<string, string>(CharaMotionDefine.ActKey.PVP_DEATH_EN.ToString(), CharaMotionDefine.ActKey.PVP_DEATH_EN_LONG.ToString()),
		new KeyValuePair<string, string>(CharaMotionDefine.ActKey.PVP_RESULT_SIT.ToString(), CharaMotionDefine.ActKey.PVP_RESULT_SIT_LONG.ToString()),
		new KeyValuePair<string, string>(CharaMotionDefine.ActKey.PIC_SITTING_DOWN.ToString(), CharaMotionDefine.ActKey.PIC_SITTING_DOWN_LONG.ToString())
	};

	// Token: 0x0400025B RID: 603
	private readonly List<string> rootResetMotion = new List<string>
	{
		CharaMotionDefine.ActKey.H_ITEM_BED_LP.ToString(),
		CharaMotionDefine.ActKey.H_ACT_TREE_SLP_LP.ToString(),
		CharaMotionDefine.ActKey.H_ITEM_CHR_LP.ToString(),
		CharaMotionDefine.ActKey.PVP_DEATH_ST.ToString(),
		CharaMotionDefine.ActKey.PVP_DEATH_ST_LONG.ToString(),
		CharaMotionDefine.ActKey.PVP_DEATH_LP.ToString(),
		CharaMotionDefine.ActKey.PVP_DEATH_LP_LONG.ToString(),
		CharaMotionDefine.ActKey.PVP_DEATH_EN.ToString(),
		CharaMotionDefine.ActKey.PVP_DEATH_EN_LONG.ToString(),
		CharaMotionDefine.ActKey.PVP_RESULT_LIE.ToString(),
		CharaMotionDefine.ActKey.PVP_RESULT_SIT.ToString(),
		CharaMotionDefine.ActKey.PVP_RESULT_SIT_LONG.ToString(),
		CharaMotionDefine.ActKey.PIC_SITTING_DOWN.ToString(),
		CharaMotionDefine.ActKey.PIC_SITTING_DOWN_LONG.ToString(),
		CharaMotionDefine.ActKey.MYR_CHAT_1MOT.ToString(),
		CharaMotionDefine.ActKey.MYR_CHAT_2MOT.ToString(),
		CharaMotionDefine.ActKey.MYR_CHAT_3MOT.ToString(),
		CharaMotionDefine.ActKey.MYR_CHAT_4MOT.ToString(),
		CharaMotionDefine.ActKey.MYR_TEA_1MOT.ToString(),
		CharaMotionDefine.ActKey.MYR_TEA_2MOT.ToString(),
		CharaMotionDefine.ActKey.MYR_TEA_3MOT.ToString(),
		CharaMotionDefine.ActKey.MYR_TEA_4MOT.ToString(),
		CharaMotionDefine.ActKey.MYR_PLAY_1MOT.ToString(),
		CharaMotionDefine.ActKey.MYR_PLAY_2MOT.ToString(),
		CharaMotionDefine.ActKey.MYR_PLAY_3MOT.ToString(),
		CharaMotionDefine.ActKey.MYR_PLAY_4MOT.ToString(),
		CharaMotionDefine.ActKey.MYR_TALK_1MOT_NOHAND.ToString(),
		CharaMotionDefine.ActKey.MYR_TALK_2MOT_NOHAND.ToString(),
		CharaMotionDefine.ActKey.MYR_TALK_3MOT_NOHAND.ToString(),
		CharaMotionDefine.ActKey.MYR_TALK_4MOT_NOHAND.ToString()
	};

	// Token: 0x0400025C RID: 604
	private readonly List<string> crossFadeOffMotion = new List<string> { CharaMotionDefine.ActKey.GACHA_LP.ToString() };

	// Token: 0x0400025D RID: 605
	private readonly List<string> tailOffMotion = new List<string>
	{
		CharaMotionDefine.ActKey.MYR_SLEEP_1.ToString(),
		CharaMotionDefine.ActKey.MYR_SLEEP_2.ToString(),
		CharaMotionDefine.ActKey.MYR_SLEEP_3.ToString(),
		CharaMotionDefine.ActKey.MYR_SLEEP_4.ToString()
	};

	// Token: 0x0400025E RID: 606
	private IEnumerator initializeRoutine;

	// Token: 0x0400025F RID: 607
	private string assetModelName = string.Empty;

	// Token: 0x04000260 RID: 608
	private bool assetIsShadow;

	// Token: 0x04000261 RID: 609
	private bool modelActive = true;

	// Token: 0x04000262 RID: 610
	private bool shadowActive = true;

	// Token: 0x04000263 RID: 611
	private bool weaponActive = true;

	// Token: 0x04000264 RID: 612
	private HashSet<Camera> viewCamera;

	// Token: 0x020005DD RID: 1501
	public enum EyeMotType
	{
		// Token: 0x04002A47 RID: 10823
		DISABLE,
		// Token: 0x04002A48 RID: 10824
		ENABLE_CHARA,
		// Token: 0x04002A49 RID: 10825
		ENABLE_AUTH
	}

	// Token: 0x020005DE RID: 1502
	public enum CharaEffectType
	{
		// Token: 0x04002A4B RID: 10827
		DISABLE,
		// Token: 0x04002A4C RID: 10828
		EAR,
		// Token: 0x04002A4D RID: 10829
		SPARKLE,
		// Token: 0x04002A4E RID: 10830
		AURA,
		// Token: 0x04002A4F RID: 10831
		OBJECT
	}

	// Token: 0x020005DF RID: 1503
	private class WeaponOffsetParam
	{
		// Token: 0x04002A50 RID: 10832
		public string weaponSuffix;

		// Token: 0x04002A51 RID: 10833
		public string offsetRootNode;

		// Token: 0x04002A52 RID: 10834
		public Vector3 offsetPos;

		// Token: 0x04002A53 RID: 10835
		public Quaternion offsetRot;
	}

	// Token: 0x020005E0 RID: 1504
	public class PartsData
	{
		// Token: 0x06002F87 RID: 12167 RVA: 0x001B714F File Offset: 0x001B534F
		public PartsData()
		{
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x001B7164 File Offset: 0x001B5364
		public PartsData(GameObject rootObj, bool body = false, string modelName = null)
		{
			if (rootObj == null && !string.IsNullOrEmpty(modelName))
			{
				Verbose<PrjLog>.LogError(modelName + "の読み込みに失敗しました。担当者に連絡をお願いいたします。", null);
			}
			this.rootObj = rootObj;
			this.animation = rootObj.transform.Find("root").GetComponent<SimpleAnimation>();
			if (this.animation == null)
			{
				this.animation = rootObj.GetComponent<SimpleAnimation>();
			}
			this.referencer = rootObj.GetComponent<CharaModelReferencer>();
			this.referencer.JoinReferenceParam();
			if (body)
			{
				CharaModelReferencer.JoinReferenceAnimParam(this.animation, Singleton<AssetManager>.Instance.GetDefaultMotionReferencer());
			}
			this.animation.ExInit();
			foreach (SimpleAnimation.State state in this.animation.GetStates())
			{
				if (state.clip != null)
				{
					this.animationKeyList.Add(state.name, state.clip.name.StartsWith("1000"));
				}
			}
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x001B728C File Offset: 0x001B548C
		public void ReloadAnimationKey()
		{
			this.animationKeyList.Clear();
			foreach (SimpleAnimation.State state in this.animation.GetStates())
			{
				if (state.clip != null)
				{
					this.animationKeyList.Add(state.name, state.clip.name.StartsWith("1000"));
				}
			}
		}

		// Token: 0x04002A54 RID: 10836
		public GameObject rootObj;

		// Token: 0x04002A55 RID: 10837
		public SimpleAnimation animation;

		// Token: 0x04002A56 RID: 10838
		public Dictionary<string, bool> animationKeyList = new Dictionary<string, bool>();

		// Token: 0x04002A57 RID: 10839
		public CharaModelReferencer referencer;
	}

	// Token: 0x020005E1 RID: 1505
	public class InitializeParam
	{
		// Token: 0x06002F8A RID: 12170 RVA: 0x001B7318 File Offset: 0x001B5518
		public InitializeParam(string bodyModelName, bool longSkirt, bool isShadow)
		{
			this.bodyModelName = bodyModelName;
			this.longSkirt = longSkirt;
			this.isShadow = isShadow;
			this.isShadowModel = false;
			this.isDisableVoice = false;
			this.accessory = null;
			this.layer = LayerMask.NameToLayer("FieldPlayer");
			this.isViewer = false;
		}

		// Token: 0x06002F8B RID: 12171 RVA: 0x001B736C File Offset: 0x001B556C
		public InitializeParam(string bodyModelName, bool longSkirt, bool isShadow, bool isModelShadow, int layer)
		{
			this.bodyModelName = bodyModelName;
			this.longSkirt = longSkirt;
			this.isShadow = isShadow;
			this.isShadowModel = isModelShadow;
			this.isDisableVoice = false;
			this.accessory = null;
			this.layer = layer;
			this.isViewer = false;
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x001B73BC File Offset: 0x001B55BC
		public static CharaModelHandle.InitializeParam CreaateByCharaId(int charaId, int clothImageId, bool longSkirt, bool isShadow)
		{
			ItemDef.Kind kind = ItemDef.Id2Kind(charaId);
			int num = DataManagerChara.CharaId2ModelId(charaId);
			if (kind <= ItemDef.Kind.NPC)
			{
				if (kind == ItemDef.Kind.CHARA || kind == ItemDef.Kind.NPC)
				{
					string text = "";
					while (clothImageId >= 0)
					{
						text = CharaModelHandle.InitializeParam.ImageID2suffix[clothImageId % CharaModelHandle.InitializeParam.ImageID2suffix.Count] + text;
						clothImageId = clothImageId / CharaModelHandle.InitializeParam.ImageID2suffix.Count - 1;
					}
					text = "ch_" + num.ToString("0000") + "_" + text;
					return new CharaModelHandle.InitializeParam(text, longSkirt, isShadow);
				}
			}
			else
			{
				if (kind == ItemDef.Kind.ENEMY_CHARA)
				{
					return new CharaModelHandle.InitializeParam("ch_" + num.ToString("00000") + "_a", longSkirt, isShadow);
				}
				if (kind == ItemDef.Kind.PERFORMANCE_ONLY_OBJECT)
				{
					return new CharaModelHandle.InitializeParam("ch_" + num.ToString("00000") + "_a", longSkirt, isShadow);
				}
			}
			Verbose<PrjLog>.LogError("Error : CreaateByCharaId:" + kind.ToString(), null);
			return new CharaModelHandle.InitializeParam("ch_0001_a", longSkirt, isShadow);
		}

		// Token: 0x04002A58 RID: 10840
		public string bodyModelName;

		// Token: 0x04002A59 RID: 10841
		public bool longSkirt;

		// Token: 0x04002A5A RID: 10842
		public bool isShadow;

		// Token: 0x04002A5B RID: 10843
		public bool isShadowModel;

		// Token: 0x04002A5C RID: 10844
		public bool isEnableMotionSE;

		// Token: 0x04002A5D RID: 10845
		public bool isDisableVoice;

		// Token: 0x04002A5E RID: 10846
		public DataManagerCharaAccessory.Accessory accessory;

		// Token: 0x04002A5F RID: 10847
		public int layer;

		// Token: 0x04002A60 RID: 10848
		public bool isViewer;

		// Token: 0x04002A61 RID: 10849
		private static readonly List<string> ImageID2suffix = new List<string>
		{
			"a", "b", "c", "d", "e", "f", "g", "h", "i", "j",
			"k", "l", "m", "n", "o", "p", "q", "r", "s", "t",
			"u", "v", "w", "x", "y", "z"
		};
	}

	// Token: 0x020005E2 RID: 1506
	public enum PartsType
	{
		// Token: 0x04002A63 RID: 10851
		Body,
		// Token: 0x04002A64 RID: 10852
		Ear,
		// Token: 0x04002A65 RID: 10853
		Tail,
		// Token: 0x04002A66 RID: 10854
		Max
	}

	// Token: 0x020005E3 RID: 1507
	public enum BlendShapeIndex
	{
		// Token: 0x04002A68 RID: 10856
		weapon_a,
		// Token: 0x04002A69 RID: 10857
		weapon_b,
		// Token: 0x04002A6A RID: 10858
		brow_down_r,
		// Token: 0x04002A6B RID: 10859
		brow_down_l,
		// Token: 0x04002A6C RID: 10860
		brow_up_r,
		// Token: 0x04002A6D RID: 10861
		brow_up_l,
		// Token: 0x04002A6E RID: 10862
		brow_sad_r,
		// Token: 0x04002A6F RID: 10863
		brow_sad_l,
		// Token: 0x04002A70 RID: 10864
		brow_anger_r,
		// Token: 0x04002A71 RID: 10865
		brow_anger_l,
		// Token: 0x04002A72 RID: 10866
		eye_close,
		// Token: 0x04002A73 RID: 10867
		eye_smile,
		// Token: 0x04002A74 RID: 10868
		eye_anger,
		// Token: 0x04002A75 RID: 10869
		eye_sad,
		// Token: 0x04002A76 RID: 10870
		eye_wink_r,
		// Token: 0x04002A77 RID: 10871
		eye_wink_l,
		// Token: 0x04002A78 RID: 10872
		eye_special_a,
		// Token: 0x04002A79 RID: 10873
		eye_special_b,
		// Token: 0x04002A7A RID: 10874
		mouth_a,
		// Token: 0x04002A7B RID: 10875
		mouth_i,
		// Token: 0x04002A7C RID: 10876
		mouth_n,
		// Token: 0x04002A7D RID: 10877
		mouth_o,
		// Token: 0x04002A7E RID: 10878
		mouth_smile,
		// Token: 0x04002A7F RID: 10879
		mouth_sad,
		// Token: 0x04002A80 RID: 10880
		mouth_special_a,
		// Token: 0x04002A81 RID: 10881
		mouth_special_b,
		// Token: 0x04002A82 RID: 10882
		sub_weapon,
		// Token: 0x04002A83 RID: 10883
		Max
	}

	// Token: 0x020005E4 RID: 1508
	public class BlendShapeData
	{
		// Token: 0x04002A84 RID: 10884
		public SkinnedMeshRenderer mesh;

		// Token: 0x04002A85 RID: 10885
		public int idx;

		// Token: 0x04002A86 RID: 10886
		public float val;

		// Token: 0x04002A87 RID: 10887
		public string mdlkey;

		// Token: 0x04002A88 RID: 10888
		public int mdlidx;
	}

	// Token: 0x020005E5 RID: 1509
	public class MotionReq
	{
		// Token: 0x04002A89 RID: 10889
		public string name;

		// Token: 0x04002A8A RID: 10890
		public bool loop;

		// Token: 0x04002A8B RID: 10891
		public float speed;

		// Token: 0x04002A8C RID: 10892
		public float fade;

		// Token: 0x04002A8D RID: 10893
		public float fadeL;

		// Token: 0x04002A8E RID: 10894
		public bool turn;
	}

	// Token: 0x020005E6 RID: 1510
	public class AccEff
	{
		// Token: 0x04002A8F RID: 10895
		public int typ;

		// Token: 0x04002A90 RID: 10896
		public string name;

		// Token: 0x04002A91 RID: 10897
		public DataManagerCharaAccessory.DispPosition pos;

		// Token: 0x04002A92 RID: 10898
		public bool dsp;

		// Token: 0x04002A93 RID: 10899
		public EffectData eff;
	}
}
