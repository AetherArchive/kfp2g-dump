using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Mst;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class CharaModelHandle : MonoBehaviour
{
	public bool enabledFaceMotion { get; set; }

	public CharaModelHandle.EyeMotType eyeMotionType { get; set; }

	public Transform headFollowObj { get; set; }

	public Transform eyeFollowObj { get; set; }

	public Transform mouthFollowObj { get; set; }

	public Vector3 eyeAngleL { get; set; }

	public Vector3 eyeAngleR { get; set; }

	public float mouthAngle { get; set; }

	public bool haraOffset { get; set; }

	public CharaModelHandle.InitializeParam initializeParam { get; private set; }

	public string modelName { get; private set; }

	public string loadVoiceCueSheetName { get; private set; }

	public static CharaModelHandle.BlendShapeIndex MakeBlendShapeIndex(string flagName, int xyzIndex)
	{
		int num = new List<string>(CharaModelHandle.FLAG_LIST).IndexOf(flagName);
		if (num >= 0 && xyzIndex >= 0)
		{
			return CharaModelHandle.FACE_LIST[num, xyzIndex];
		}
		return CharaModelHandle.BlendShapeIndex.Max;
	}

	public float shadowSize { get; set; }

	public float shadowHeight { get; set; }

	private string skirt2long(string str)
	{
		KeyValuePair<string, string> keyValuePair = this.longSkirt.Find((KeyValuePair<string, string> itm) => itm.Key == str);
		if (!keyValuePair.Equals(default(KeyValuePair<string, string>)))
		{
			str = keyValuePair.Value;
		}
		return str;
	}

	private string skirt2short(string str)
	{
		KeyValuePair<string, string> keyValuePair = this.longSkirt.Find((KeyValuePair<string, string> itm) => itm.Value == str);
		if (!keyValuePair.Equals(default(KeyValuePair<string, string>)))
		{
			str = keyValuePair.Key;
		}
		return str;
	}

	public void Initialize(int charaId, bool isShadow = true, bool isShadowModel = false, int clothImageId = 0, bool longSkirt = false, bool isMotionSe = false, bool isDisableVoice = false, DataManagerCharaAccessory.Accessory accessory = null)
	{
		CharaModelHandle.InitializeParam initializeParam = CharaModelHandle.InitializeParam.CreaateByCharaId(charaId, clothImageId, longSkirt, isShadow);
		initializeParam.isShadowModel = isShadowModel;
		initializeParam.isEnableMotionSE = isMotionSe;
		initializeParam.isDisableVoice = isDisableVoice;
		initializeParam.accessory = accessory;
		this.Initialize(initializeParam);
	}

	public void Initialize(CharaPackData cpd, bool isShadow = true, bool isShadowModel = false, bool isMotionSe = false)
	{
		CharaModelHandle.InitializeParam initializeParam = CharaModelHandle.InitializeParam.CreaateByCharaId(cpd.id, cpd.equipClothImageId, cpd.equipLongSkirt, isShadow);
		initializeParam.isShadowModel = isShadowModel;
		initializeParam.isEnableMotionSE = isMotionSe;
		initializeParam.accessory = (cpd.dynamicData.dispAccessoryEffect ? cpd.dynamicData.accessory : null);
		this.Initialize(initializeParam);
	}

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

	private void SetEyeObj(Transform root, string flagSuffix = "")
	{
		string text = string.Concat(new string[] { "pelvis", flagSuffix, "/j_upperbody", flagSuffix, "/j_chest", flagSuffix, "/j_neck", flagSuffix, "/j_head", flagSuffix });
		this.eyeCtrlL = root.Find(text + "/j_eye_l" + flagSuffix);
		this.eyeCtrlR = root.Find(text + "/j_eye_r" + flagSuffix);
	}

	public bool IsFinishInitialize()
	{
		return this.assetModelName.IndexOf("_1015_") > 0 || this.assetModelName.IndexOf("_1016_") > 0 || this.isFinishInit;
	}

	private void OnDestroy()
	{
		this.DestoryInternal();
	}

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

	private void Update()
	{
		this.UpdateInternal();
	}

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

	public void SetShadowActive(bool act)
	{
		if (this.isFinishInit && this.shadowActive != act && this.shadow != null)
		{
			this.shadow.SetActive(act && this.modelActive);
		}
		this.shadowActive = act;
	}

	public void SetWeaponActive(bool act)
	{
		this.weaponActive = act;
	}

	public bool IsModelActive()
	{
		return this.modelActive;
	}

	public void SetPuyoTex(string tex)
	{
		if (this.puyoObj != null && string.IsNullOrEmpty(this.puyoReq) && this.puyoTex != tex)
		{
			this.puyoReq = tex;
			AssetManager.LoadAssetData(this.puyoReq, AssetManager.OWNER.CharaModel, 0, null);
		}
	}

	public void PlayAnimation(CharaMotionDefine.ActKey key, bool loop = false, float speed = 1f, float crossfade = 0f, float crossfadeL = 0f, bool turn = false)
	{
		this.PlayAnimation(key.ToString(), loop, speed, crossfade, crossfadeL, turn);
	}

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

	public bool IsCurrentAnimation(CharaMotionDefine.ActKey key)
	{
		return this.IsCurrentAnimation(key.ToString());
	}

	public bool IsCurrentAnimation(string key)
	{
		string text = (string.IsNullOrEmpty(this.motionReq.name) ? this.motionName : this.motionReq.name);
		if (!this.initializeParam.isViewer)
		{
			return this.skirt2long(text) == this.skirt2long(key);
		}
		return text == key;
	}

	public bool IsLoopAnimation()
	{
		if (!string.IsNullOrEmpty(this.motionReq.name))
		{
			return this.motionReq.loop;
		}
		return this.motionLoop;
	}

	public void SetLoopAnimation(bool sw)
	{
		this.motionLoop = sw;
	}

	public void SetAnimationSpeed(float spd)
	{
		this.motionSpeed = spd;
	}

	public void SetPosition(Vector3 pos)
	{
		this.motionPos = pos;
	}

	public void SetRotation(float rot)
	{
		this.motionRot = rot;
		while (this.motionRot < 0f)
		{
			this.motionRot += 360f;
		}
	}

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

	public void SetAuthPlayer(AuthPlayer ap)
	{
		this.authPlayer = ap;
	}

	public bool IsPlaying()
	{
		return this.isFinishInit && (this.partsDataBody.animation.enabled || !string.IsNullOrEmpty(this.motionReq.name)) && !this.motionEnd;
	}

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

	public void SetEyeColorByReferencer(float a)
	{
		Color color = this.partsDataBody.referencer.refAnimationObj.GetComponent<CharaModelReferencer>().eyeColor;
		color.a = a;
		this.SetEyeColor(color);
	}

	public Color GetEyeColor()
	{
		return this.eyeColor;
	}

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

	public Color GetCheekColor()
	{
		return this.cheekColor;
	}

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

	public float GetMouthAngle()
	{
		if (!this.isFinishInit || !(this.mouthCtrl != null))
		{
			return 0f;
		}
		return Mathf.DeltaAngle(0f, this.mouthCtrl.localEulerAngles.x);
	}

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

	public void SetLayer(string lay)
	{
		this.SetLayer(LayerMask.NameToLayer(lay));
	}

	public int GetLayer()
	{
		return this.layer;
	}

	public int GetPartsBodyLayer()
	{
		return this.partsDataBody.rootObj.layer;
	}

	public void SetAlpha(float a)
	{
		this.alpha = a;
		this.fade = 0f;
	}

	public float GetAlpha()
	{
		return this.alpha;
	}

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

	public bool IsDisp()
	{
		return this.fade > 0f || (this.fade == 0f && this.alpha > 0f);
	}

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

	public void SetNeighboringAlpha(Vector3 na)
	{
		this.NeighboringAlpha = new Vector4(na.x, na.y, na.z, this.NeighboringAlpha.w);
	}

	public void SetMatCap(float mc)
	{
		if (this.matCap >= 0f)
		{
			this.matCap = Mathf.Clamp01(mc);
		}
	}

	public float GetMatCap()
	{
		if (this.matCap < 0f)
		{
			return 0f;
		}
		return this.matCap;
	}

	public Vector3 GetHaraPos()
	{
		if (!(this.pelvis == null))
		{
			return this.pelvis.position;
		}
		return base.transform.position;
	}

	public Vector3 GetHeadPos()
	{
		if (!(this.headCtrl == null))
		{
			return this.headCtrl.position;
		}
		return this.GetHaraPos();
	}

	public Vector3 GetNodePos(string node)
	{
		Transform transform = this.childAll.Find((Transform itm) => itm.name == node);
		if (!(transform == null))
		{
			return transform.position;
		}
		return this.GetHaraPos();
	}

	public Transform GetNodeTransform(string node)
	{
		return this.childAll.Find((Transform itm) => itm.name == node);
	}

	public Vector3 GetHaraLocalPos()
	{
		if (!(this.pelvis == null))
		{
			return this.pelvis.localPosition;
		}
		return base.transform.localPosition;
	}

	public Vector3 GetCharaScale()
	{
		return this.partsDataBody.rootObj.transform.Find("root").localScale;
	}

	public void SetEnableUpdateOffscreen()
	{
		SkinnedMeshRenderer[] componentsInChildren = this.partsDataBody.rootObj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].updateWhenOffscreen = true;
		}
	}

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

	public void SetMotionTime(float time)
	{
		if (this.motionTime != time)
		{
			this.motionTimeReq = time;
		}
	}

	public void SetMotionFrame(int frame)
	{
		if ((int)(this.motionTime * 30f) != frame)
		{
			this.motionTimeReq = (float)frame / 30f;
		}
	}

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

	public void SetEffect(EffectData eff)
	{
		this.effect.Add(new KeyValuePair<EffectData, Vector3>(eff, (eff.effectObject == null) ? Vector3.zero : eff.effectObject.transform.localPosition));
	}

	public void DestroyEffect(EffectData eff)
	{
		this.effect.RemoveAll((KeyValuePair<EffectData, Vector3> itm) => itm.Key == eff);
		EffectManager.DestroyEffect(eff);
	}

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

	public static readonly string CHARA_MODEL_PATH = "Charas/Model/";

	public static readonly string OPTION_SUFFIX_EAR = "_ear";

	public static readonly string OPTION_SUFFIX_TAIL = "_tail";

	private bool haraOffOld;

	private static readonly float mouthFollowStart = 20f;

	private static readonly float mouthFollowEnd = 60f;

	private static readonly float mouthAngleMinus = -8.3f;

	private static readonly float mouthAnglePlus = 8.3f;

	private static readonly float eyeFollowStart = 10f;

	private static readonly float eyeFollowEnd = 50f;

	private static readonly Vector3 eyeAngleMinusR = new Vector3(-23.5f, -10f, -3f);

	private static readonly Vector3 eyeAnglePlusR = new Vector3(16f, 13f, 3f);

	private static readonly Vector3 eyeAngleMinusL = new Vector3(-16f, -10f, -3f);

	private static readonly Vector3 eyeAnglePlusL = new Vector3(23.5f, 13f, 3f);

	private static readonly float headFollowStart = 15f;

	private static readonly float headFollowEnd = 45f;

	private static readonly float headAngleMinus = -30f;

	private static readonly float headAnglePlus = 30f;

	private static readonly float followSpeed = 6f;

	private float mouthAngleOld;

	private Vector2 eyeAngleOldR;

	private Vector2 eyeAngleOldL;

	private Quaternion headRotationOld;

	private float mouthFollowFade;

	private float eyeFollowFade;

	private float headFollowFade;

	private UnityAction fadeOutEnd;

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

	private List<KeyValuePair<Transform, Transform>> weaponCmnOff;

	private CharaModelHandle.PartsData partsDataBody = new CharaModelHandle.PartsData();

	private CharaModelHandle.PartsData[] partsDataEar = new CharaModelHandle.PartsData[5];

	private CharaModelHandle.PartsData[] partsDataTail = new CharaModelHandle.PartsData[5];

	private int earTyp;

	private int earTypBef;

	private int earTypFrm;

	private int earTypAft;

	private int tailTyp;

	private int tailTypBef;

	private int tailTypFrm;

	private int tailTypAft;

	private List<CharaModelHandle.BlendShapeData> blendShapeList = new List<CharaModelHandle.BlendShapeData>();

	private Dictionary<string, List<GameObject>> modelList = new Dictionary<string, List<GameObject>>();

	private List<GameObject> weaponList = new List<GameObject>();

	private Transform bonWeaponA;

	private Transform bonWeaponB;

	private int weaponDispA = -1;

	private int weaponDispB = -1;

	private int weaponDispSub;

	private List<Transform> childAll;

	private List<Transform> childEar;

	private List<Transform> childTail;

	private Transform eyeBallL;

	private Transform eyeBallR;

	private Transform eyeCtrlL;

	private Transform eyeCtrlR;

	private List<Renderer> eyeBall = new List<Renderer>();

	private Color eyeColor = Color.clear;

	private Renderer cheek;

	private Color cheekColor = Color.white;

	private List<Renderer> parts = new List<Renderer>();

	private List<Color> partsColor = new List<Color>();

	private List<float> partsAnim = new List<float>();

	private List<float> partsTime = new List<float>();

	private Transform mouthCtrl;

	private Transform headCtrl;

	private Transform pelvis;

	private Transform upperBody;

	private Transform lowerBody;

	private Transform thighL;

	private Transform thighR;

	private List<Transform> wrist;

	private Transform mdlSubweapon;

	private Transform bonSubweapon;

	private Transform partsAlphaObj;

	public static readonly string[] FLAG_LIST = new string[] { "flag_weapon_a", "flag_brow_a", "flag_brow_b", "flag_brow_c", "flag_eye_a", "flag_eye_b", "flag_eye_c", "flag_mouth_a", "flag_mouth_b", "flag_mouth_c" };

	private List<Transform> flagList = new List<Transform>();

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

	private int layer;

	private float alpha;

	private float fade;

	private Vector4 NeighboringAlpha = new Vector4(0f, -1f, 0f, 1f);

	private float matCap = -1f;

	public bool camouflage;

	private float camouflageAlpha;

	private List<CullingCheck> cullingList;

	private List<Material> materials;

	private Dictionary<Transform, Quaternion> crossBone;

	private Dictionary<Transform, Quaternion> crossBoneL;

	private Dictionary<Transform, Vector3> crossRoot;

	private Dictionary<Transform, Transform> crossWeapon;

	private float crossFade;

	private float crossFadeL;

	private CharaModelHandle.MotionReq motionReq;

	private string motionName;

	private bool motionLoop;

	private float motionLength;

	private float motionTime;

	private float motionTimeReq;

	private float motionSpeed;

	private bool motionEnd;

	private Vector3 motionPos;

	private float motionRot;

	private GameObject shadow;

	private Material shadowMat;

	private GameObject shadowModel;

	private List<KeyValuePair<Transform, Transform>> shadowBone;

	private CullingCheck shadowCull;

	private Camera shadowCamera;

	private List<KeyValuePair<EffectData, Vector3>> effect = new List<KeyValuePair<EffectData, Vector3>>();

	public List<EffectData> charaEffect;

	private string charaEffectName = string.Empty;

	private CharaModelHandle.CharaEffectType charaEffectType;

	public bool isAccessoryAnchor;

	private List<CharaModelHandle.AccEff> accEff;

	private static Transform accCamChara = null;

	private static int accCamType = 0;

	private GameObject puyoObj;

	private string puyoTex;

	private string puyoReq;

	private float puyoBreath;

	private bool isFinishInit;

	private AuthPlayer authPlayer;

	private readonly List<KeyValuePair<string, string>> longSkirt = new List<KeyValuePair<string, string>>
	{
		new KeyValuePair<string, string>(CharaMotionDefine.ActKey.PVP_ENTRY.ToString(), CharaMotionDefine.ActKey.PVP_ENTRY_LONG.ToString()),
		new KeyValuePair<string, string>(CharaMotionDefine.ActKey.PVP_DEATH_ST.ToString(), CharaMotionDefine.ActKey.PVP_DEATH_ST_LONG.ToString()),
		new KeyValuePair<string, string>(CharaMotionDefine.ActKey.PVP_DEATH_LP.ToString(), CharaMotionDefine.ActKey.PVP_DEATH_LP_LONG.ToString()),
		new KeyValuePair<string, string>(CharaMotionDefine.ActKey.PVP_DEATH_EN.ToString(), CharaMotionDefine.ActKey.PVP_DEATH_EN_LONG.ToString()),
		new KeyValuePair<string, string>(CharaMotionDefine.ActKey.PVP_RESULT_SIT.ToString(), CharaMotionDefine.ActKey.PVP_RESULT_SIT_LONG.ToString()),
		new KeyValuePair<string, string>(CharaMotionDefine.ActKey.PIC_SITTING_DOWN.ToString(), CharaMotionDefine.ActKey.PIC_SITTING_DOWN_LONG.ToString())
	};

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

	private readonly List<string> crossFadeOffMotion = new List<string> { CharaMotionDefine.ActKey.GACHA_LP.ToString() };

	private readonly List<string> tailOffMotion = new List<string>
	{
		CharaMotionDefine.ActKey.MYR_SLEEP_1.ToString(),
		CharaMotionDefine.ActKey.MYR_SLEEP_2.ToString(),
		CharaMotionDefine.ActKey.MYR_SLEEP_3.ToString(),
		CharaMotionDefine.ActKey.MYR_SLEEP_4.ToString()
	};

	private IEnumerator initializeRoutine;

	private string assetModelName = string.Empty;

	private bool assetIsShadow;

	private bool modelActive = true;

	private bool shadowActive = true;

	private bool weaponActive = true;

	private HashSet<Camera> viewCamera;

	public enum EyeMotType
	{
		DISABLE,
		ENABLE_CHARA,
		ENABLE_AUTH
	}

	public enum CharaEffectType
	{
		DISABLE,
		EAR,
		SPARKLE,
		AURA,
		OBJECT
	}

	private class WeaponOffsetParam
	{
		public string weaponSuffix;

		public string offsetRootNode;

		public Vector3 offsetPos;

		public Quaternion offsetRot;
	}

	public class PartsData
	{
		public PartsData()
		{
		}

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

		public GameObject rootObj;

		public SimpleAnimation animation;

		public Dictionary<string, bool> animationKeyList = new Dictionary<string, bool>();

		public CharaModelReferencer referencer;
	}

	public class InitializeParam
	{
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

		public string bodyModelName;

		public bool longSkirt;

		public bool isShadow;

		public bool isShadowModel;

		public bool isEnableMotionSE;

		public bool isDisableVoice;

		public DataManagerCharaAccessory.Accessory accessory;

		public int layer;

		public bool isViewer;

		private static readonly List<string> ImageID2suffix = new List<string>
		{
			"a", "b", "c", "d", "e", "f", "g", "h", "i", "j",
			"k", "l", "m", "n", "o", "p", "q", "r", "s", "t",
			"u", "v", "w", "x", "y", "z"
		};
	}

	public enum PartsType
	{
		Body,
		Ear,
		Tail,
		Max
	}

	public enum BlendShapeIndex
	{
		weapon_a,
		weapon_b,
		brow_down_r,
		brow_down_l,
		brow_up_r,
		brow_up_l,
		brow_sad_r,
		brow_sad_l,
		brow_anger_r,
		brow_anger_l,
		eye_close,
		eye_smile,
		eye_anger,
		eye_sad,
		eye_wink_r,
		eye_wink_l,
		eye_special_a,
		eye_special_b,
		mouth_a,
		mouth_i,
		mouth_n,
		mouth_o,
		mouth_smile,
		mouth_sad,
		mouth_special_a,
		mouth_special_b,
		sub_weapon,
		Max
	}

	public class BlendShapeData
	{
		public SkinnedMeshRenderer mesh;

		public int idx;

		public float val;

		public string mdlkey;

		public int mdlidx;
	}

	public class MotionReq
	{
		public string name;

		public bool loop;

		public float speed;

		public float fade;

		public float fadeL;

		public bool turn;
	}

	public class AccEff
	{
		public int typ;

		public string name;

		public DataManagerCharaAccessory.DispPosition pos;

		public bool dsp;

		public EffectData eff;
	}
}
