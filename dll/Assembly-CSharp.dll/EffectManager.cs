using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Ab;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Rendering;

public class EffectManager : Singleton<EffectManager>
{
	public IEnumerator MappingSePackData()
	{
		string mapFilePath = "Effects/EffectSeParam";
		AssetManager.LoadAssetData(mapFilePath, AssetManager.OWNER.EffectManager, 0, null);
		while (!AssetManager.IsLoadFinishAssetData(mapFilePath))
		{
			yield return null;
		}
		EffectSeParameter effectSeParameter = AssetManager.GetAssetData(mapFilePath) as EffectSeParameter;
		if (effectSeParameter.packList != null)
		{
			this.sePackMap = new Dictionary<string, EffectSeParameter.PackData>();
			for (int i = 0; i < effectSeParameter.packList.Count; i++)
			{
				this.sePackMap.Add(effectSeParameter.packList[i].effectName, effectSeParameter.packList[i]);
			}
		}
		yield break;
	}

	private EffectSeParameter.PackData GetSoundPackData(string assetName)
	{
		if (this.sePackMap != null && this.sePackMap.ContainsKey(assetName))
		{
			return this.sePackMap[assetName];
		}
		return null;
	}

	public static Camera BillboardCamera
	{
		get
		{
			return Singleton<EffectManager>.Instance.billboardCamera;
		}
		set
		{
			Singleton<EffectManager>.Instance.billboardCamera = value;
		}
	}

	public static void ReqLoadEffect(string assetName, AssetManager.OWNER owner, int priority = 0, Action<Data> onLoadComplete = null)
	{
		string effectPrefabPath = EffectManager.GetEffectPrefabPath(assetName);
		if (effectPrefabPath == null)
		{
			return;
		}
		AssetManager.LoadAssetData(effectPrefabPath, owner, priority, onLoadComplete);
	}

	public static void UnloadEffect(string assetName, AssetManager.OWNER owner)
	{
		string effectPrefabPath = EffectManager.GetEffectPrefabPath(assetName);
		if (effectPrefabPath == null)
		{
			return;
		}
		AssetManager.UnloadAssetData(effectPrefabPath, owner);
	}

	public static bool IsLoadFinishEffect(string assetName)
	{
		string effectPrefabPath = EffectManager.GetEffectPrefabPath(assetName);
		return effectPrefabPath != null && AssetManager.IsLoadFinishAssetData(effectPrefabPath);
	}

	public static bool IsExsistEffect(string assetName)
	{
		string effectPrefabPath = EffectManager.GetEffectPrefabPath(assetName);
		return effectPrefabPath != null && AssetManager.IsExsistAssetData(effectPrefabPath);
	}

	public static EffectData InstantiateEffect(string assetName, Transform parent = null, int layer = 1, float scale = 1f)
	{
		string effectPrefabPath = EffectManager.GetEffectPrefabPath(assetName);
		if (effectPrefabPath == null)
		{
			return null;
		}
		GameObject gameObject = (GameObject)AssetManager.GetAssetData(effectPrefabPath);
		if (gameObject == null)
		{
			return null;
		}
		GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, parent);
		if (scale != 1f && assetName.Contains("scalable"))
		{
			gameObject2.transform.localScale = new Vector3(scale, scale, scale);
		}
		EffectData effectData = new EffectData(gameObject2, Singleton<EffectManager>.Instance.GetSoundPackData(assetName), assetName);
		effectData.SetNormalLayer(layer);
		if (gameObject2 != null)
		{
			EffectManager.ChangeGameObject(gameObject2, layer);
			Singleton<EffectManager>.Instance.effectList.Add(effectData);
		}
		return effectData;
	}

	public static void DestroyEffect(EffectData effectData)
	{
		if (Singleton<EffectManager>.Instance != null && effectData != null && effectData.effectObject != null)
		{
			Singleton<EffectManager>.Instance.effectList.Remove(effectData);
			effectData.Destory();
		}
	}

	public static void DestroyEffectAll()
	{
		foreach (EffectData effectData in Singleton<EffectManager>.Instance.effectList)
		{
			effectData.Destory();
		}
		Singleton<EffectManager>.Instance.effectList.Clear();
	}

	private static void ChangeGameObject(GameObject inObj, int layer)
	{
		foreach (Transform transform in inObj.GetComponentsInChildren<Transform>(true))
		{
			if (layer != 1)
			{
				transform.gameObject.layer = layer;
			}
			MeshRenderer component = transform.GetComponent<MeshRenderer>();
			if (component != null)
			{
				component.shadowCastingMode = ShadowCastingMode.Off;
				component.receiveShadows = false;
				component.reflectionProbeUsage = ReflectionProbeUsage.Off;
			}
		}
	}

	public static string GetEffectPrefabPath(string assetName)
	{
		string text = "Effects/";
		if (assetName.StartsWith(EffectManager.PREFIX_EFFECT + "act"))
		{
			text += "act/";
		}
		else if (assetName.StartsWith(EffectManager.PREFIX_EFFECT + "authend"))
		{
			text += "authend/";
		}
		else if (assetName.StartsWith(EffectManager.PREFIX_EFFECT + "auth"))
		{
			text += "auth/";
		}
		else if (assetName.StartsWith(EffectManager.PREFIX_EFFECT + "buff"))
		{
			text += "buff/";
		}
		else if (assetName.StartsWith(EffectManager.PREFIX_EFFECT + "info"))
		{
			text += "info/";
		}
		else if (assetName.StartsWith(EffectManager.PREFIX_EFFECT + "option"))
		{
			text += "option/";
		}
		else if (assetName.StartsWith(EffectManager.PREFIX_EFFECT + "scenario"))
		{
			text += "scenario/";
		}
		else if (assetName.StartsWith(EffectManager.PREFIX_EFFECT + "stage"))
		{
			text += "stage/";
		}
		else
		{
			if (!assetName.StartsWith(EffectManager.PREFIX_EFFECT + "wait"))
			{
				return null;
			}
			text += "wait/";
		}
		return text + assetName;
	}

	private Dictionary<string, EffectSeParameter.PackData> sePackMap;

	private List<EffectData> effectList = new List<EffectData>();

	private Camera billboardCamera;

	private const int DEFAULT_LAYER = 1;

	public static string PREFIX_EFFECT = "Ef_";
}
