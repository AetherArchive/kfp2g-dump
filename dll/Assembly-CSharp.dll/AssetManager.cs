using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SGNFW.Ab;
using SGNFW.Common;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class AssetManager : Singleton<AssetManager>
{
	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x0003F151 File Offset: 0x0003D351
	// (set) Token: 0x06000AD4 RID: 2772 RVA: 0x0003F158 File Offset: 0x0003D358
	public static bool IsFinishInitialize { get; private set; }

	// Token: 0x06000AD5 RID: 2773 RVA: 0x0003F160 File Offset: 0x0003D360
	public static void AddLoadList(string path, AssetManager.OWNER owner)
	{
		if (!Manager.Initialized)
		{
			return;
		}
		if (!Singleton<AssetManager>.Instance.loadAssetListByOwner[owner].Contains(path))
		{
			Singleton<AssetManager>.Instance.loadAssetListByOwner[owner].Add(path);
		}
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x0003F198 File Offset: 0x0003D398
	public static void UnLoadByList(AssetManager.OWNER owner, string ignoreTarget = "", bool useSafeList = true)
	{
		if (!Manager.Initialized)
		{
			return;
		}
		List<string> list = new List<string>(Singleton<AssetManager>.Instance.loadAssetListByOwner[owner]);
		List<string> list2 = new List<string>();
		if (!string.IsNullOrEmpty(ignoreTarget))
		{
			list2.Add(ignoreTarget);
		}
		if (useSafeList)
		{
			list2.AddRange(AssetManager.safeList);
		}
		bool flag = false;
		foreach (string text in list)
		{
			flag = false;
			foreach (string text2 in list2)
			{
				if (text.Contains(text2))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				AssetManager.UnloadAssetData(text, owner);
			}
		}
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x0003F274 File Offset: 0x0003D474
	public void SetAbChkType(AssetManager.abCheckType typ)
	{
		this.abChkTyp = typ;
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x0003F27D File Offset: 0x0003D47D
	public bool IsEndAbChek()
	{
		return this.abCheckList == null;
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x0003F288 File Offset: 0x0003D488
	public void Initialize()
	{
		this.abCheckList = null;
		base.StartCoroutine(this.LoadInitialize());
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x0003F29E File Offset: 0x0003D49E
	public void Destroy()
	{
		this.abCheckList = null;
		Manager.Exit();
		Manager.UnloadAll(false);
		Manager.Clear();
		if (this.loadAssetListByOwner != null)
		{
			this.loadAssetListByOwner.Clear();
		}
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x0003F2CA File Offset: 0x0003D4CA
	private IEnumerator LoadInitialize()
	{
		AssetManager.IsFinishInitialize = false;
		this.loadAssetListByOwner = new Dictionary<AssetManager.OWNER, List<string>>();
		foreach (object obj in Enum.GetValues(typeof(AssetManager.OWNER)))
		{
			AssetManager.OWNER owner = (AssetManager.OWNER)obj;
			this.loadAssetListByOwner.Add(owner, new List<string>());
		}
		bool isFinish = false;
		Manager.LoadInitializeData(true, delegate
		{
			isFinish = true;
		}, delegate(string err, Exception ex)
		{
		});
		while (!isFinish)
		{
			yield return null;
		}
		Manager.AddLoadErrorListener(delegate(string str, Data data, Exception ex)
		{
		});
		Manager.AddFailedWriteListener(delegate(string str, Data data, Exception ex)
		{
		});
		Manager.AddCachesErrorListener(delegate(Exception ex)
		{
		});
		yield return null;
		this.abChkTyp = AssetManager.abCheckType.LOW;
		this.abCheckList = new List<Data>(Manager.DataList);
		AssetManager.IsFinishInitialize = true;
		yield break;
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x0003F2DC File Offset: 0x0003D4DC
	private void Update()
	{
		if (this.abCheckList != null)
		{
			if (this.abChkTyp == AssetManager.abCheckType.OFF)
			{
				this.abCheckList.Clear();
			}
			int num = 0;
			int num2 = AssetManager.checkSpeed[(int)this.abChkTyp];
			if (!Manager.IsDone)
			{
				num2 = 0;
			}
			SceneManager.SceneName currentSceneName = Singleton<SceneManager>.Instance.CurrentSceneName;
			if (currentSceneName - SceneManager.SceneName.SceneBattle <= 1 || currentSceneName == SceneManager.SceneName.SceneGacha || currentSceneName == SceneManager.SceneName.ScenePresent)
			{
				num2 = 0;
			}
			if (Singleton<SceneManager>.Instance.IsSceneChange)
			{
				num2 = 0;
			}
			while (num < num2 && this.abCheckList.Count > 0)
			{
				Manager.CheckState(this.abCheckList[0]);
				this.abCheckList.RemoveAt(0);
				num++;
			}
			if (this.abCheckList.Count <= 0)
			{
				this.abCheckList = null;
			}
		}
	}

	// Token: 0x06000ADD RID: 2781 RVA: 0x0003F394 File Offset: 0x0003D594
	public static Utility.DownloadInfo CreateDownloadInfo(AssetManager.DownloadType type)
	{
		switch (type)
		{
		case AssetManager.DownloadType.NEED_ONLY:
		{
			List<Data> list = new List<Data>();
			foreach (Data data in Manager.DataList)
			{
				if (data.category == AssetManager.ASSET_CATEGORY_FACE_PACK)
				{
					list.Add(data);
				}
				else if (data.category == AssetManager.ASSET_CATEGORY_PARAMETER)
				{
					list.Add(data);
				}
				else if (data.category == AssetManager.ASSET_CATEGORY_QUESTPARAM)
				{
					list.Add(data);
				}
				else if (data.name == "ch_1000_a_mot.prefab")
				{
					list.Add(data);
				}
				else if (data.name == "AssetPathParameter.asset")
				{
					list.Add(data);
				}
			}
			return Utility.GetDownloadInfo(list);
		}
		case AssetManager.DownloadType.FULL:
			return Utility.GetDownloadInfo(Manager.DataList.FindAll((Data itm) => itm.category != AssetManager.ASSET_CATEGORY_MOVIE));
		case AssetManager.DownloadType.OP_MOVIE:
		{
			List<Data> list2 = new List<Data>();
			Data data2 = Manager.DataList.Find((Data itm) => itm.name == (AssetManager.ASSET_OP_MOVIE + AssetManager.ASSET_MOVIE_EXT).ToLower());
			if (data2 != null)
			{
				list2.Add(data2);
			}
			return Utility.GetDownloadInfo(list2);
		}
		case AssetManager.DownloadType.INTRODUCTION:
			new List<Data>();
			return Utility.GetDownloadInfo(Manager.DataList.FindAll((Data itm) => itm.name.StartsWith(AssetManager.ASSET_INTRODUCTION_MOVIE.ToLower())));
		default:
			return new Utility.DownloadInfo();
		}
	}

	// Token: 0x06000ADE RID: 2782 RVA: 0x0003F544 File Offset: 0x0003D744
	public static void LoadAssetData(string filePath, AssetManager.OWNER owner, int priority = 0, Action<Data> onLoadComplete = null)
	{
		if (!Manager.Initialized)
		{
			return;
		}
		Data data = Manager.GetData(AssetManager.GetAssetDataFullPath(filePath));
		if (data != null)
		{
			Singleton<AssetManager>.Instance.loadAssetListByOwner[owner].Add(filePath);
			Manager.CheckState(data);
			Manager.Load(data, onLoadComplete, false);
		}
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x0003F590 File Offset: 0x0003D790
	public static List<string> LoadAssetDataByCategory(string category, AssetManager.OWNER owner, int priority = 0, Action<Data> onLoadComplete = null)
	{
		new HashSet<string>();
		if (!Manager.Initialized)
		{
			return new List<string>();
		}
		List<Data> list = new List<Data>(Manager.GetDataCategory(new string[] { category }));
		foreach (Data data in list)
		{
			Singleton<AssetManager>.Instance.loadAssetListByOwner[owner].Add(data.name);
			Manager.CheckState(data);
			Manager.Load(data, onLoadComplete, false);
		}
		return list.ConvertAll<string>((Data item) => item.name);
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x0003F650 File Offset: 0x0003D850
	public static void DownloadAssetData(string filePath, AssetManager.OWNER owner, int priority = 0)
	{
		if (!Manager.Initialized)
		{
			return;
		}
		Data data = Manager.GetData(AssetManager.GetAssetDataFullPath(filePath));
		if (data != null)
		{
			Manager.CheckState(data);
			if (!data.IsExists && data.IsNeedDownload)
			{
				Manager.Download(data, null);
			}
		}
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x0003F694 File Offset: 0x0003D894
	public static bool IsDownloadFinishAssetData(string filePath)
	{
		if (!Manager.Initialized)
		{
			return false;
		}
		Data data = Manager.GetData(AssetManager.GetAssetDataFullPath(filePath));
		if (data != null)
		{
			IEnumerable<Data> dependencyData = Manager.GetDependencyData(data);
			if (dependencyData != null)
			{
				using (IEnumerator<Data> enumerator = dependencyData.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IsExists)
						{
							return false;
						}
					}
				}
			}
			return data.IsExists;
		}
		return true;
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x0003F70C File Offset: 0x0003D90C
	public static void UnloadAssetData(string filePath, AssetManager.OWNER owner)
	{
		if (!Manager.Initialized)
		{
			return;
		}
		if (Singleton<AssetManager>.Instance == null)
		{
			return;
		}
		if (filePath == null)
		{
			using (List<string>.Enumerator enumerator = Singleton<AssetManager>.Instance.loadAssetListByOwner[owner].GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text = enumerator.Current;
					AssetManager.UnloadAssetData(text, owner);
				}
				return;
			}
		}
		Data data = Manager.GetData(AssetManager.GetAssetDataFullPath(filePath));
		if (data != null)
		{
			Manager.CheckState(data);
			if (data.IsLoaded && Singleton<AssetManager>.Instance.loadAssetListByOwner.ContainsKey(owner) && Singleton<AssetManager>.Instance.loadAssetListByOwner[owner].Contains(filePath))
			{
				Singleton<AssetManager>.Instance.loadAssetListByOwner[owner].Remove(filePath);
				Manager.Unload(data, false);
			}
		}
	}

	// Token: 0x06000AE3 RID: 2787 RVA: 0x0003F7E8 File Offset: 0x0003D9E8
	public static bool IsLoadFinishAssetData(string filePath)
	{
		if (!Manager.Initialized)
		{
			return false;
		}
		Data data = Manager.GetData(AssetManager.GetAssetDataFullPath(filePath));
		if (data != null)
		{
			IEnumerable<Data> dependencyData = Manager.GetDependencyData(data);
			if (dependencyData != null)
			{
				using (IEnumerator<Data> enumerator = dependencyData.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IsLoaded)
						{
							return false;
						}
					}
				}
			}
			return data.IsLoaded;
		}
		return true;
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x0003F860 File Offset: 0x0003DA60
	public static bool IsExsistAssetData(string filePath)
	{
		if (!Manager.Initialized)
		{
			return false;
		}
		Data data = Manager.GetData(AssetManager.GetAssetDataFullPath(filePath));
		return data != null;
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x0003F888 File Offset: 0x0003DA88
	public static bool IsNeedAssetDownload(string filePath)
	{
		if (!Manager.Initialized)
		{
			return false;
		}
		Data data = Manager.GetData(AssetManager.GetAssetDataFullPath(filePath));
		if (data != null)
		{
			Manager.CheckState(data);
			if (!data.IsExists && data.IsNeedDownload)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x0003F8C8 File Offset: 0x0003DAC8
	public static Object GetAssetData(string filePath)
	{
		Object @object = null;
		if (@object == null)
		{
			if (filePath.IndexOf(AssetManager.PREFIX_PATH_CHARA_PARAM) >= 0)
			{
				@object = Manager.LoadAsset(Manager.GetData("parameter.asset"), AssetManager.GetAssetDataFullPath(filePath));
			}
			else if (filePath.IndexOf(AssetManager.PREFIX_PATH_FACE_PACK) >= 0)
			{
				@object = Manager.LoadAsset(Manager.GetData("facepack.prefab"), AssetManager.GetAssetDataFullPath(filePath));
			}
			else if (filePath.IndexOf(AssetManager.PREFIX_PATH_QUEST_PARAM) >= 0)
			{
				@object = Manager.LoadAsset(Manager.GetData("questparam.asset"), AssetManager.GetAssetDataFullPath(filePath));
			}
			else if (filePath.StartsWith(AssetManager.PREFIX_PATH_TREEHOUSE_MODEL_DATA))
			{
				@object = Manager.LoadAsset(Manager.GetData("treehouse.asset"), AssetManager.GetAssetDataFullPath(filePath));
			}
			else
			{
				@object = Manager.LoadAsset(AssetManager.GetAssetDataFullPath(filePath));
			}
		}
		if (@object == null)
		{
			@object = Resources.Load(filePath);
		}
		return @object;
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x0003F998 File Offset: 0x0003DB98
	public static List<Object> GetAssetDataByCategory(string category)
	{
		List<Object> list = new List<Object>();
		foreach (Data data in new List<Data>(Manager.GetDataCategory(new string[] { category })))
		{
			Object[] array = Manager.LoadAllAssets<Object>(data);
			if (array != null)
			{
				list.AddRange(array);
			}
		}
		return list;
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x0003FA08 File Offset: 0x0003DC08
	public static string GetAssetDataFullPath(string filePath)
	{
		string text = "";
		if (filePath.StartsWith(AssetManager.PREFIX_PATH_CHARA_MODEL) || filePath.StartsWith(AssetManager.PREFIX_PATH_CHARA_MOTION))
		{
			text = Path.GetFileName(filePath) + ".prefab";
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_TEX2D))
		{
			text = Path.GetFileName(filePath) + ".png";
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_SOUND))
		{
			text = Path.GetFileName(filePath);
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_MOVIE))
		{
			text = Path.GetFileName(filePath) + AssetManager.ASSET_MOVIE_EXT;
		}
		else if (filePath.StartsWith(StagePresetCtrl.PackDataPath))
		{
			text = Path.GetFileName(filePath) + ".prefab";
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_STAGE_MODEL))
		{
			text = Path.GetFileName(filePath) + ".prefab";
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_EFFECT_SE_PARAM))
		{
			text = Path.GetFileName(filePath) + ".asset";
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_EFFECT))
		{
			text = Path.GetFileName(filePath) + ".prefab";
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_SCENARIO))
		{
			text = Path.GetFileName(filePath) + (filePath.Contains(".") ? "" : ".prefab");
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_CHARA_PARAM))
		{
			text = Path.GetFileName(filePath) + (filePath.Contains(".") ? "" : ".asset");
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_FACE_PACK))
		{
			text = Path.GetFileName(filePath) + (filePath.Contains(".") ? "" : ".prefab");
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_GUI_PARAM))
		{
			text = Path.GetFileName(filePath) + (filePath.Contains(".") ? "" : ".asset");
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_QUEST_PARAM))
		{
			text = Path.GetFileName(filePath) + (filePath.Contains(".") ? "" : ".asset");
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_LIGHT))
		{
			text = Path.GetFileName(filePath) + ".prefab";
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_FURNITURE) || filePath.StartsWith(AssetManager.PREFIX_PATH_TREEHOUSE))
		{
			text = Path.GetFileName(filePath) + ".prefab";
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_GUIBG))
		{
			text = Path.GetFileName(filePath) + ".prefab";
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_GUI))
		{
			text = Path.GetFileName(filePath) + ".prefab";
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_AUTH_PARAM))
		{
			text = Path.GetFileName(filePath) + ".asset";
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_AUTH))
		{
			text = Path.GetFileName(filePath) + ".prefab";
		}
		else if (filePath.StartsWith(AssetManager.PREFIX_PATH_TREEHOUSE_MODEL_DATA))
		{
			text = Path.GetFileName(filePath) + (filePath.Contains(".") ? "" : ".asset");
		}
		if (text != string.Empty)
		{
			return text.ToLower();
		}
		if (filePath.StartsWith(AssetManager.PREFIX_PATH_CHARA_PARAM) || filePath.StartsWith(AssetManager.PREFIX_PATH_QUEST_PARAM) || filePath.StartsWith(AssetManager.PREFIX_PATH_EFFECT_SE_PARAM) || filePath.StartsWith(AssetManager.PREFIX_PATH_GUI_PARAM))
		{
			return AssetManager.ASSET_FOLDER_PATH + filePath + (filePath.Contains(".") ? "" : ".asset");
		}
		if (filePath.StartsWith(AssetManager.PREFIX_PATH_TEX2D))
		{
			return AssetManager.ASSET_FOLDER_PATH_TEX2D + filePath + ".png";
		}
		if (filePath.StartsWith(AssetManager.PREFIX_PATH_SOUND))
		{
			return AssetManager.ASSET_FOLDER_PATH_SOUND + filePath + (filePath.Contains(".") ? "" : ".acb");
		}
		if (filePath.StartsWith(AssetManager.PREFIX_PATH_MOVIE))
		{
			return AssetManager.ASSET_FOLDER_PATH_MOVIE + filePath + AssetManager.ASSET_MOVIE_EXT;
		}
		if (filePath.StartsWith(AssetManager.PREFIX_PATH_SCENARIO) || filePath.StartsWith(AssetManager.PREFIX_PATH_FACE_PACK))
		{
			return AssetManager.ASSET_FOLDER_PATH + filePath + (filePath.Contains(".") ? "" : ".prefab");
		}
		if (filePath.StartsWith(AssetManager.PREFIX_PATH_AUTH_PARAM))
		{
			return AssetManager.ASSET_FOLDER_PATH + filePath + ".asset";
		}
		if (filePath.StartsWith(AssetManager.PREFIX_PATH_TREEHOUSE_MODEL_DATA))
		{
			return AssetManager.ASSET_FOLDER_PATH + filePath + ".asset";
		}
		return AssetManager.ASSET_FOLDER_PATH + filePath + ".prefab";
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x0003FEB8 File Offset: 0x0003E0B8
	public static GameObject InstantiateAssetData(string filePath, Transform parent = null)
	{
		Object assetData = AssetManager.GetAssetData(filePath);
		if (assetData != null)
		{
			return (GameObject)Object.Instantiate(assetData, parent);
		}
		return null;
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x0003FEE4 File Offset: 0x0003E0E4
	public static void ResetShader(Transform obj)
	{
		Renderer component = obj.GetComponent<Renderer>();
		if (component != null)
		{
			foreach (Material material in component.materials)
			{
				material.shader = Shader.Find(material.shader.name);
			}
		}
		foreach (object obj2 in obj)
		{
			AssetManager.ResetShader((Transform)obj2);
		}
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x0003FF78 File Offset: 0x0003E178
	protected override void OnSingletonAwake()
	{
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x0003FF7C File Offset: 0x0003E17C
	public CharaModelReferencer GetDefaultMotionReferencer()
	{
		if (this.DefaultMotionReferencer == null)
		{
			GameObject gameObject = AssetManager.InstantiateAssetData("Charas/MotionAssign/ch_1000_a_mot", null);
			this.DefaultMotionReferencer = gameObject.GetComponent<CharaModelReferencer>();
			gameObject.SetActive(false);
			gameObject.hideFlags = HideFlags.HideInHierarchy;
		}
		return this.DefaultMotionReferencer;
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x0003FFC3 File Offset: 0x0003E1C3
	public AssetPathParameter GetAssetPathParameter()
	{
		if (this.AssetPathParameter == null)
		{
			this.AssetPathParameter = AssetManager.GetAssetData("Gui/Parameter/AssetPathParameter") as AssetPathParameter;
		}
		return this.AssetPathParameter;
	}

	// Token: 0x0400086B RID: 2155
	private static readonly string PREFIX_PATH_TEX2D = "Texture2D/";

	// Token: 0x0400086C RID: 2156
	private static readonly string PREFIX_PATH_EFFECT = "Effects/";

	// Token: 0x0400086D RID: 2157
	public static readonly string PREFIX_PATH_SOUND = "Sounds/";

	// Token: 0x0400086E RID: 2158
	public static readonly string PREFIX_PATH_MOVIE = "Movie/";

	// Token: 0x0400086F RID: 2159
	public static readonly string PREFIX_PATH_CHARA_MODEL = "Charas/Model/";

	// Token: 0x04000870 RID: 2160
	public static readonly string PREFIX_PATH_CHARA_MOTION = "Charas/MotionAssign/";

	// Token: 0x04000871 RID: 2161
	public static readonly string PREFIX_PATH_CHARA_PARAM = "Charas/Parameter/";

	// Token: 0x04000872 RID: 2162
	public static readonly string PREFIX_PATH_QUEST_PARAM = "Quest/Parameter/";

	// Token: 0x04000873 RID: 2163
	public static readonly string PREFIX_PATH_GUI_PARAM = "Gui/Parameter/";

	// Token: 0x04000874 RID: 2164
	public static readonly string PREFIX_PATH_EFFECT_SE_PARAM = "Effects/EffectSeParam";

	// Token: 0x04000875 RID: 2165
	private static readonly string PREFIX_PATH_SCENARIO = "Scenario/";

	// Token: 0x04000876 RID: 2166
	public static readonly string PREFIX_PATH_FACE_PACK = "Charas/FacePackData/";

	// Token: 0x04000877 RID: 2167
	private static readonly string PREFIX_PATH_LIGHT = "Light/";

	// Token: 0x04000878 RID: 2168
	private static readonly string PREFIX_PATH_FURNITURE = "Stage/Furniture/";

	// Token: 0x04000879 RID: 2169
	private static readonly string PREFIX_PATH_TREEHOUSE = "Stage/Captain_furniture/";

	// Token: 0x0400087A RID: 2170
	private static readonly string PREFIX_PATH_GUIBG = "GuiBg/";

	// Token: 0x0400087B RID: 2171
	private static readonly string PREFIX_PATH_GUI = "Gui/";

	// Token: 0x0400087C RID: 2172
	private static readonly string PREFIX_PATH_AUTH = "Auth/";

	// Token: 0x0400087D RID: 2173
	private static readonly string PREFIX_PATH_AUTH_PARAM = "Auth/Parameter/";

	// Token: 0x0400087E RID: 2174
	private static readonly string PREFIX_PATH_STAGE_MODEL = "Stage/Stage/";

	// Token: 0x0400087F RID: 2175
	public static readonly string PREFIX_PATH_TREEHOUSE_MODEL_DATA = "TreeHouse/";

	// Token: 0x04000880 RID: 2176
	public static readonly string ASSET_FOLDER_PATH = "Assets/Parade/Prefabs/";

	// Token: 0x04000881 RID: 2177
	private static readonly string ASSET_FOLDER_PATH_TEX2D = "Assets/DesignBaseData/";

	// Token: 0x04000882 RID: 2178
	public static readonly string ASSET_FOLDER_PATH_SOUND = "Assets/Parade/";

	// Token: 0x04000883 RID: 2179
	public static readonly string ASSET_FOLDER_PATH_MOVIE = "Assets/Parade/";

	// Token: 0x04000884 RID: 2180
	public static readonly string ASSET_FOLDER_PATH_FACE_PACK = "Assets/Parade/Prefabs/Charas/FacePackData/";

	// Token: 0x04000885 RID: 2181
	public static readonly string ASSET_FOLDER_PATH_CAMERA_HOLDER = "Assets/Editor/CameraHolder/";

	// Token: 0x04000886 RID: 2182
	public static readonly string ASSET_CATEGORY_SOUND = "sound";

	// Token: 0x04000887 RID: 2183
	public static readonly string ASSET_CATEGORY_PARAMETER = "parameter";

	// Token: 0x04000888 RID: 2184
	public static readonly string ASSET_CATEGORY_QUESTPARAM = "questparam";

	// Token: 0x04000889 RID: 2185
	public static readonly string ASSET_CATEGORY_FACE_PACK = "facepack";

	// Token: 0x0400088A RID: 2186
	public static readonly string ASSET_CATEGORY_MOVIE = "movie";

	// Token: 0x0400088B RID: 2187
	public static readonly string ASSET_CATEGORY_TREE_HOUSE = "treehouse";

	// Token: 0x0400088C RID: 2188
	public static readonly string ASSET_OP_MOVIE = "OpeningMovie_M";

	// Token: 0x0400088D RID: 2189
	public static readonly string ASSET_INTRODUCTION_MOVIE = "NewFriends";

	// Token: 0x0400088E RID: 2190
	public static readonly string ASSET_MOVIE_EXT = ".enc";

	// Token: 0x04000890 RID: 2192
	public Dictionary<AssetManager.OWNER, List<string>> loadAssetListByOwner;

	// Token: 0x04000891 RID: 2193
	private static List<string> safeList = new List<string> { "Effects" };

	// Token: 0x04000892 RID: 2194
	private List<Data> abCheckList;

	// Token: 0x04000893 RID: 2195
	private static int[] checkSpeed = new int[] { 99999, 3, 30, 300 };

	// Token: 0x04000894 RID: 2196
	private AssetManager.abCheckType abChkTyp;

	// Token: 0x04000895 RID: 2197
	private CharaModelReferencer DefaultMotionReferencer;

	// Token: 0x04000896 RID: 2198
	private AssetPathParameter AssetPathParameter;

	// Token: 0x020007F8 RID: 2040
	public enum OWNER
	{
		// Token: 0x040035A8 RID: 13736
		Invalild,
		// Token: 0x040035A9 RID: 13737
		TestViewer,
		// Token: 0x040035AA RID: 13738
		CharaModel,
		// Token: 0x040035AB RID: 13739
		PguiWrapper,
		// Token: 0x040035AC RID: 13740
		Scenario,
		// Token: 0x040035AD RID: 13741
		HomeStage,
		// Token: 0x040035AE RID: 13742
		HomeChara,
		// Token: 0x040035AF RID: 13743
		Sound,
		// Token: 0x040035B0 RID: 13744
		AuthGacha,
		// Token: 0x040035B1 RID: 13745
		EffectManager,
		// Token: 0x040035B2 RID: 13746
		Battle,
		// Token: 0x040035B3 RID: 13747
		DataManagerChara,
		// Token: 0x040035B4 RID: 13748
		DataManagerQuest,
		// Token: 0x040035B5 RID: 13749
		AuthPlayer,
		// Token: 0x040035B6 RID: 13750
		CanvasBG,
		// Token: 0x040035B7 RID: 13751
		MoviePlayer,
		// Token: 0x040035B8 RID: 13752
		DataInitialize,
		// Token: 0x040035B9 RID: 13753
		PicnicStage,
		// Token: 0x040035BA RID: 13754
		NameEntry,
		// Token: 0x040035BB RID: 13755
		QuestSelector,
		// Token: 0x040035BC RID: 13756
		TreeHouseStage,
		// Token: 0x040035BD RID: 13757
		IntroductionFriends
	}

	// Token: 0x020007F9 RID: 2041
	public enum abCheckType
	{
		// Token: 0x040035BF RID: 13759
		OFF,
		// Token: 0x040035C0 RID: 13760
		LOW,
		// Token: 0x040035C1 RID: 13761
		MIDDLE,
		// Token: 0x040035C2 RID: 13762
		HIGH
	}

	// Token: 0x020007FA RID: 2042
	public enum DownloadType
	{
		// Token: 0x040035C4 RID: 13764
		INVALID,
		// Token: 0x040035C5 RID: 13765
		NEED_ONLY,
		// Token: 0x040035C6 RID: 13766
		FULL,
		// Token: 0x040035C7 RID: 13767
		OP_MOVIE,
		// Token: 0x040035C8 RID: 13768
		INTRODUCTION
	}
}
