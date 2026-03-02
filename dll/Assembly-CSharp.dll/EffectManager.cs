using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Ab;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020000C6 RID: 198
public class EffectManager : Singleton<EffectManager>
{
	// Token: 0x060008EA RID: 2282 RVA: 0x00038BDD File Offset: 0x00036DDD
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

	// Token: 0x060008EB RID: 2283 RVA: 0x00038BEC File Offset: 0x00036DEC
	private EffectSeParameter.PackData GetSoundPackData(string assetName)
	{
		if (this.sePackMap != null && this.sePackMap.ContainsKey(assetName))
		{
			return this.sePackMap[assetName];
		}
		return null;
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x060008EC RID: 2284 RVA: 0x00038C12 File Offset: 0x00036E12
	// (set) Token: 0x060008ED RID: 2285 RVA: 0x00038C1E File Offset: 0x00036E1E
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

	// Token: 0x060008EE RID: 2286 RVA: 0x00038C2C File Offset: 0x00036E2C
	public static void ReqLoadEffect(string assetName, AssetManager.OWNER owner, int priority = 0, Action<Data> onLoadComplete = null)
	{
		string effectPrefabPath = EffectManager.GetEffectPrefabPath(assetName);
		if (effectPrefabPath == null)
		{
			return;
		}
		AssetManager.LoadAssetData(effectPrefabPath, owner, priority, onLoadComplete);
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x00038C50 File Offset: 0x00036E50
	public static void UnloadEffect(string assetName, AssetManager.OWNER owner)
	{
		string effectPrefabPath = EffectManager.GetEffectPrefabPath(assetName);
		if (effectPrefabPath == null)
		{
			return;
		}
		AssetManager.UnloadAssetData(effectPrefabPath, owner);
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x00038C70 File Offset: 0x00036E70
	public static bool IsLoadFinishEffect(string assetName)
	{
		string effectPrefabPath = EffectManager.GetEffectPrefabPath(assetName);
		return effectPrefabPath != null && AssetManager.IsLoadFinishAssetData(effectPrefabPath);
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x00038C90 File Offset: 0x00036E90
	public static bool IsExsistEffect(string assetName)
	{
		string effectPrefabPath = EffectManager.GetEffectPrefabPath(assetName);
		return effectPrefabPath != null && AssetManager.IsExsistAssetData(effectPrefabPath);
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x00038CB0 File Offset: 0x00036EB0
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

	// Token: 0x060008F3 RID: 2291 RVA: 0x00038D4B File Offset: 0x00036F4B
	public static void DestroyEffect(EffectData effectData)
	{
		if (Singleton<EffectManager>.Instance != null && effectData != null && effectData.effectObject != null)
		{
			Singleton<EffectManager>.Instance.effectList.Remove(effectData);
			effectData.Destory();
		}
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x00038D84 File Offset: 0x00036F84
	public static void DestroyEffectAll()
	{
		foreach (EffectData effectData in Singleton<EffectManager>.Instance.effectList)
		{
			effectData.Destory();
		}
		Singleton<EffectManager>.Instance.effectList.Clear();
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x00038DE8 File Offset: 0x00036FE8
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

	// Token: 0x060008F6 RID: 2294 RVA: 0x00038E44 File Offset: 0x00037044
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

	// Token: 0x04000759 RID: 1881
	private Dictionary<string, EffectSeParameter.PackData> sePackMap;

	// Token: 0x0400075A RID: 1882
	private List<EffectData> effectList = new List<EffectData>();

	// Token: 0x0400075B RID: 1883
	private Camera billboardCamera;

	// Token: 0x0400075C RID: 1884
	private const int DEFAULT_LAYER = 1;

	// Token: 0x0400075D RID: 1885
	public static string PREFIX_EFFECT = "Ef_";
}
