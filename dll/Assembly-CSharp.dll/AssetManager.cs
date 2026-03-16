using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SGNFW.Ab;
using SGNFW.Common;
using UnityEngine;

public class AssetManager : Singleton<AssetManager>
{
	public static bool IsFinishInitialize { get; private set; }

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

	public void SetAbChkType(AssetManager.abCheckType typ)
	{
		this.abChkTyp = typ;
	}

	public bool IsEndAbChek()
	{
		return this.abCheckList == null;
	}

	public void Initialize()
	{
		this.abCheckList = null;
		base.StartCoroutine(this.LoadInitialize());
	}

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

	public static bool IsExsistAssetData(string filePath)
	{
		if (!Manager.Initialized)
		{
			return false;
		}
		Data data = Manager.GetData(AssetManager.GetAssetDataFullPath(filePath));
		return data != null;
	}

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

	public static GameObject InstantiateAssetData(string filePath, Transform parent = null)
	{
		Object assetData = AssetManager.GetAssetData(filePath);
		if (assetData != null)
		{
			return (GameObject)Object.Instantiate(assetData, parent);
		}
		return null;
	}

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

	protected override void OnSingletonAwake()
	{
	}

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

	public AssetPathParameter GetAssetPathParameter()
	{
		if (this.AssetPathParameter == null)
		{
			this.AssetPathParameter = AssetManager.GetAssetData("Gui/Parameter/AssetPathParameter") as AssetPathParameter;
		}
		return this.AssetPathParameter;
	}

	private static readonly string PREFIX_PATH_TEX2D = "Texture2D/";

	private static readonly string PREFIX_PATH_EFFECT = "Effects/";

	public static readonly string PREFIX_PATH_SOUND = "Sounds/";

	public static readonly string PREFIX_PATH_MOVIE = "Movie/";

	public static readonly string PREFIX_PATH_CHARA_MODEL = "Charas/Model/";

	public static readonly string PREFIX_PATH_CHARA_MOTION = "Charas/MotionAssign/";

	public static readonly string PREFIX_PATH_CHARA_PARAM = "Charas/Parameter/";

	public static readonly string PREFIX_PATH_QUEST_PARAM = "Quest/Parameter/";

	public static readonly string PREFIX_PATH_GUI_PARAM = "Gui/Parameter/";

	public static readonly string PREFIX_PATH_EFFECT_SE_PARAM = "Effects/EffectSeParam";

	private static readonly string PREFIX_PATH_SCENARIO = "Scenario/";

	public static readonly string PREFIX_PATH_FACE_PACK = "Charas/FacePackData/";

	private static readonly string PREFIX_PATH_LIGHT = "Light/";

	private static readonly string PREFIX_PATH_FURNITURE = "Stage/Furniture/";

	private static readonly string PREFIX_PATH_TREEHOUSE = "Stage/Captain_furniture/";

	private static readonly string PREFIX_PATH_GUIBG = "GuiBg/";

	private static readonly string PREFIX_PATH_GUI = "Gui/";

	private static readonly string PREFIX_PATH_AUTH = "Auth/";

	private static readonly string PREFIX_PATH_AUTH_PARAM = "Auth/Parameter/";

	private static readonly string PREFIX_PATH_STAGE_MODEL = "Stage/Stage/";

	public static readonly string PREFIX_PATH_TREEHOUSE_MODEL_DATA = "TreeHouse/";

	public static readonly string ASSET_FOLDER_PATH = "Assets/Parade/Prefabs/";

	private static readonly string ASSET_FOLDER_PATH_TEX2D = "Assets/DesignBaseData/";

	public static readonly string ASSET_FOLDER_PATH_SOUND = "Assets/Parade/";

	public static readonly string ASSET_FOLDER_PATH_MOVIE = "Assets/Parade/";

	public static readonly string ASSET_FOLDER_PATH_FACE_PACK = "Assets/Parade/Prefabs/Charas/FacePackData/";

	public static readonly string ASSET_FOLDER_PATH_CAMERA_HOLDER = "Assets/Editor/CameraHolder/";

	public static readonly string ASSET_CATEGORY_SOUND = "sound";

	public static readonly string ASSET_CATEGORY_PARAMETER = "parameter";

	public static readonly string ASSET_CATEGORY_QUESTPARAM = "questparam";

	public static readonly string ASSET_CATEGORY_FACE_PACK = "facepack";

	public static readonly string ASSET_CATEGORY_MOVIE = "movie";

	public static readonly string ASSET_CATEGORY_TREE_HOUSE = "treehouse";

	public static readonly string ASSET_OP_MOVIE = "OpeningMovie_M";

	public static readonly string ASSET_INTRODUCTION_MOVIE = "NewFriends";

	public static readonly string ASSET_MOVIE_EXT = ".enc";

	public Dictionary<AssetManager.OWNER, List<string>> loadAssetListByOwner;

	private static List<string> safeList = new List<string> { "Effects" };

	private List<Data> abCheckList;

	private static int[] checkSpeed = new int[] { 99999, 3, 30, 300 };

	private AssetManager.abCheckType abChkTyp;

	private CharaModelReferencer DefaultMotionReferencer;

	private AssetPathParameter AssetPathParameter;

	public enum OWNER
	{
		Invalild,
		TestViewer,
		CharaModel,
		PguiWrapper,
		Scenario,
		HomeStage,
		HomeChara,
		Sound,
		AuthGacha,
		EffectManager,
		Battle,
		DataManagerChara,
		DataManagerQuest,
		AuthPlayer,
		CanvasBG,
		MoviePlayer,
		DataInitialize,
		PicnicStage,
		NameEntry,
		QuestSelector,
		TreeHouseStage,
		IntroductionFriends
	}

	public enum abCheckType
	{
		OFF,
		LOW,
		MIDDLE,
		HIGH
	}

	public enum DownloadType
	{
		INVALID,
		NEED_ONLY,
		FULL,
		OP_MOVIE,
		INTRODUCTION
	}
}
