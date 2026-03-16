using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SGNFW.Ab
{
	public class Manager : Singleton<Manager>
	{
		public static string HostUrl
		{
			get
			{
				return Manager.hostUrl;
			}
		}

		public static string DataVersion
		{
			get
			{
				return Manager.dataVersion;
			}
		}

		public static Define.Language Language
		{
			get
			{
				return Manager.language;
			}
		}

		public static bool Initialized
		{
			get
			{
				return Manager.initialized;
			}
		}

		public static string DownloadUrl
		{
			get
			{
				return string.Format("{0}{1}{2}{3}", new object[]
				{
					Manager.hostUrl.EndsWith("/") ? Manager.hostUrl : (Manager.hostUrl + "/"),
					Manager.PlatformString.EndsWith("/") ? Manager.PlatformString : (Manager.PlatformString + "/"),
					Manager.dataVersion.EndsWith("/") ? Manager.dataVersion : (Manager.dataVersion + "/"),
					Manager.LanguageString.EndsWith("/") ? Manager.LanguageString : (Manager.LanguageString + "/")
				});
			}
		}

		public static List<Data> DataList
		{
			get
			{
				return Manager.packData.dataList;
			}
		}

		public static int DataMax
		{
			get
			{
				return Manager.packData.dataList.Count;
			}
		}

		public static List<Data> StorageSaveDataList
		{
			get
			{
				return Manager.packData.dataList.FindAll((Data t) => t.save);
			}
		}

		public static string PlatformString
		{
			get
			{
				return Define.Platform.Windows.ToString();
			}
		}

		public static string LanguageString
		{
			get
			{
				return Manager.language.ToString();
			}
		}

		public static string AssetPath
		{
			get
			{
				if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
				{
					return Application.dataPath + "/../AssetBundle/";
				}
				if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
				{
					return Application.persistentDataPath + "/StreamingAssets/";
				}
				if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					return Application.temporaryCachePath + "/AssetBundle/";
				}
				if (Application.platform == RuntimePlatform.Android)
				{
					return Application.persistentDataPath + "/AssetBundle/";
				}
				return "";
			}
		}

		public static string LoadPathAtWWW
		{
			get
			{
				string assetPath = Manager.AssetPath;
				if (Application.platform == RuntimePlatform.WindowsPlayer)
				{
					return "file:///" + assetPath;
				}
				return "file://" + assetPath;
			}
		}

		public static int LoadDataNum
		{
			get
			{
				int num = 0;
				foreach (Loader loader in Manager.loaderList)
				{
					num += loader.DataNum;
				}
				return num;
			}
		}

		public static float LoadProgress
		{
			get
			{
				int num = 0;
				float num2 = 0f;
				foreach (Loader loader in Manager.loaderList)
				{
					if (!loader.IsDone)
					{
						if (loader.DataNum <= 0)
						{
							num2 += 1f;
						}
						else
						{
							num2 += loader.Progress;
						}
						num++;
					}
				}
				if (num > 1)
				{
					num2 /= (float)num;
				}
				return num2;
			}
		}

		public static bool IsDone
		{
			get
			{
				bool flag = true;
				for (int i = 0; i < Manager.loaderList.Count; i++)
				{
					if (!Manager.loaderList[i].IsDone)
					{
						flag = false;
						break;
					}
				}
				return flag;
			}
		}

		public static void Initialize(string url, string version, Define.Language lang)
		{
			Manager.hostUrl = url;
			Manager.dataVersion = version;
			Manager.language = lang;
		}

		public static void LoadInitializeData(bool redownload = false, Action completed = null, Action<string, Exception> error = null)
		{
			new Initializer
			{
				onCompleted = completed,
				onError = error
			}.Load(redownload);
			try
			{
				string text = Manager.AssetPath + "hash_file.txt";
				if (File.Exists(text))
				{
					using (StreamReader streamReader = new StreamReader(text))
					{
						string text2;
						while ((text2 = streamReader.ReadLine()) != null)
						{
							string[] array = text2.Split(':', StringSplitOptions.None);
							if (array.Length == 2)
							{
								string text3 = array[0];
								uint num;
								if (!Manager.packData.hashDic.ContainsKey(text3) && uint.TryParse(array[1], NumberStyles.HexNumber, null, out num))
								{
									Manager.packData.hashDic[text3] = num;
								}
							}
						}
						goto IL_00B6;
					}
				}
				if (!Directory.Exists(Manager.AssetPath))
				{
					Directory.CreateDirectory(Manager.AssetPath);
				}
				IL_00B6:;
			}
			catch (Exception ex)
			{
				Verbose<Verbose>.LogError(ex, null);
			}
			Manager.initialized = true;
		}

		public static Loader GetLoader()
		{
			Loader loader = null;
			for (int i = 0; i < Manager.loaderList.Count; i++)
			{
				if (!Manager.loaderList[i].IsBusy)
				{
					loader = Manager.loaderList[i];
					break;
				}
			}
			if (loader == null)
			{
				loader = Loader.Create(Singleton<Manager>.Instance.transform, Singleton<Manager>.Instance.downloadTimeout, Singleton<Manager>.Instance.downloadRetryNum);
				loader.onFinished = delegate
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (string text in Manager.packData.hashDic.Keys)
					{
						string text2 = text + ":" + Manager.packData.hashDic[text].ToString("X");
						stringBuilder.AppendLine(text2);
					}
					string assetPath = Manager.AssetPath;
					string text3 = assetPath + "hash_file.txt";
					try
					{
						if (!Directory.Exists(assetPath))
						{
							Directory.CreateDirectory(assetPath);
						}
						using (StreamWriter streamWriter = new StreamWriter(text3))
						{
							streamWriter.Write(stringBuilder.ToString());
							streamWriter.Close();
						}
					}
					catch (Exception ex)
					{
						if (Manager.onFailedWrite != null)
						{
							Exception ex2;
							Manager.onFailedWrite(text3, null, ex2);
						}
					}
					if (Manager.IsDone && Manager.onLoadFinished != null)
					{
						Manager.onLoadFinished();
					}
				};
				loader.onError = delegate(string path, Data data, Exception ex)
				{
					loader.Exit();
					if (Manager.onLoadError != null)
					{
						Manager.onLoadError(path, data, ex);
					}
				};
				loader.onFailedWrite = delegate(string path, Data data, Exception ex)
				{
					loader.Exit();
					if (Manager.onFailedWrite != null)
					{
						Manager.onFailedWrite(path, data, ex);
					}
				};
				Manager.loaderList.Add(loader);
			}
			return loader;
		}

		public static Loader Download(Data data, Action<Data> dlgt = null)
		{
			Loader loader = Manager.GetLoader();
			Manager._Load(data, dlgt, true, loader, false);
			return loader;
		}

		public static Loader Download(IEnumerable<Data> data, Action<Data> dlgt = null)
		{
			if (data == null)
			{
				return null;
			}
			Loader loader = Manager.GetLoader();
			foreach (Data data2 in data)
			{
				Manager._Load(data2, dlgt, true, loader, false);
			}
			return loader;
		}

		public static Loader Load(Data data, Action<Data> dlgt = null, bool async = false)
		{
			Loader loader = Manager.GetLoader();
			Manager._Load(data, dlgt, false, loader, async);
			return loader;
		}

		public static Loader Load(IEnumerable<Data> data, Action<Data> dlgt = null, bool async = false)
		{
			if (data == null)
			{
				return null;
			}
			Loader loader = Manager.GetLoader();
			foreach (Data data2 in data)
			{
				Manager._Load(data2, dlgt, false, loader, async);
			}
			return loader;
		}

		public static void Abort()
		{
			for (int i = 0; i < Manager.loaderList.Count; i++)
			{
				Manager.loaderList[i].Abort();
			}
		}

		public static void Abort(Loader loader)
		{
			if (loader == null)
			{
				return;
			}
			loader.Abort();
		}

		public static void Retry()
		{
			for (int i = 0; i < Manager.loaderList.Count; i++)
			{
				Manager.loaderList[i].Retry();
			}
		}

		public static void Retry(Loader loader)
		{
			if (loader == null)
			{
				return;
			}
			loader.Retry();
		}

		public static void Exit()
		{
			for (int i = 0; i < Manager.loaderList.Count; i++)
			{
				Manager.loaderList[i].Exit();
			}
		}

		public static void Exit(Loader loader)
		{
			if (loader == null)
			{
				return;
			}
			loader.Exit();
		}

		public static void AddLoadFinishedListener(Action dlgt)
		{
			Manager.onLoadFinished = (Action)Delegate.Combine(Manager.onLoadFinished, dlgt);
		}

		public static void RemoveLoadFinishedListener(Action dlgt)
		{
			Manager.onLoadFinished = (Action)Delegate.Remove(Manager.onLoadFinished, dlgt);
		}

		public static void RemoveAllLoadFinishedListener()
		{
			Manager.onLoadFinished = null;
		}

		public static void AddLoadErrorListener(Action<string, Data, Exception> dlgt)
		{
			Manager.onLoadError = (Action<string, Data, Exception>)Delegate.Combine(Manager.onLoadError, dlgt);
		}

		public static void RemoveLoadErrorListener(Action<string, Data, Exception> dlgt)
		{
			Manager.onLoadError = (Action<string, Data, Exception>)Delegate.Remove(Manager.onLoadError, dlgt);
		}

		public static void RemoveAllLoadErrorListener()
		{
			Manager.onLoadError = null;
		}

		public static void AddFailedWriteListener(Action<string, Data, Exception> dlgt)
		{
			Manager.onFailedWrite = (Action<string, Data, Exception>)Delegate.Combine(Manager.onFailedWrite, dlgt);
		}

		public static void RemoveFailedWriteListener(Action<string, Data, Exception> dlgt)
		{
			Manager.onFailedWrite = (Action<string, Data, Exception>)Delegate.Remove(Manager.onFailedWrite, dlgt);
		}

		public static void RemoveAllFailedWriteListener()
		{
			Manager.onFailedWrite = null;
		}

		public static void AddCachesErrorListener(Action<Exception> dlgt)
		{
			Manager.onCachesError = (Action<Exception>)Delegate.Combine(Manager.onCachesError, dlgt);
		}

		public static void RemoveCachesErrorListener(Action<Exception> dlgt)
		{
			Manager.onCachesError = (Action<Exception>)Delegate.Remove(Manager.onCachesError, dlgt);
		}

		public static void RemoveAllCachesErrorListener()
		{
			Manager.onCachesError = null;
		}

		public static void CheckState(List<Data> data)
		{
			foreach (Data data2 in data)
			{
				Manager.CheckState(data2);
			}
		}

		public static void CheckState(Data[] data)
		{
			for (int i = 0; i < data.Length; i++)
			{
				Manager.CheckState(data[i]);
			}
		}

		public static void CheckState(Data data)
		{
			if (!data.IsUnknown)
			{
				return;
			}
			if (data.asset != null || data.bytes != null)
			{
				data.state = Data.State.Loaded;
				return;
			}
			string text = Manager.AssetPath + data.FullPath;
			if (!File.Exists(text))
			{
				data.state = Data.State.NotExists;
				return;
			}
			if (!data.isHashCheck)
			{
				data.state = Data.State.Exists;
				return;
			}
			uint hashUint;
			if (!Manager.packData.hashDic.TryGetValue(data.name, out hashUint))
			{
				hashUint = CRC32.GetHashUint(text);
			}
			if (hashUint == data.hash)
			{
				data.state = Data.State.Exists;
				return;
			}
			Manager.RemoveCacheFile(data);
			data.state = Data.State.NotExists;
		}

		public static void SetHash(Data data)
		{
			Manager.packData.hashDic[data.name] = data.hash;
		}

		public static void Clear()
		{
			Manager.packData.Clear();
		}

		public static Data GetData(string path)
		{
			if (Manager.packData.dataDic.ContainsKey(path))
			{
				return Manager.packData.dataDic[path];
			}
			return null;
		}

		public static IEnumerable<Data> GetData(params string[] paths)
		{
			if (paths == null || paths.Length == 0)
			{
				return null;
			}
			List<Data> list = new List<Data>();
			for (int i = 0; i < paths.Length; i++)
			{
				if (Manager.packData.dataDic.ContainsKey(paths[i]))
				{
					list.Add(Manager.packData.dataDic[paths[i]]);
				}
			}
			return list;
		}

		public static IEnumerable<Data> GetDataCategory(params string[] categories)
		{
			if (categories == null || categories.Length == 0)
			{
				return null;
			}
			List<Data> list = new List<Data>();
			for (int i = 0; i < categories.Length; i++)
			{
				if (Manager.packData.categoryDic.ContainsKey(categories[i]))
				{
					list.AddRange(Manager.packData.categoryDic[categories[i]]);
				}
			}
			return list;
		}

		public static IEnumerable<Data> GetDataTag(params string[] tags)
		{
			if (tags == null || tags.Length == 0)
			{
				return null;
			}
			List<Data> list = new List<Data>();
			for (int i = 0; i < tags.Length; i++)
			{
				if (Manager.packData.tagDic.ContainsKey(tags[i]))
				{
					list.AddRange(Manager.packData.tagDic[tags[i]]);
				}
			}
			return list;
		}

		public static IEnumerable<Data> GetDataCategoryTag(string category, string tag)
		{
			return new List<Data>(Manager.GetDataTag(new string[] { tag })).FindAll((Data t) => t.category.Equals(category));
		}

		public static IEnumerable<Data> GetDependencyData(Data data)
		{
			if (data == null)
			{
				return null;
			}
			if (data.dependencies == null || data.dependencies.Length == 0)
			{
				return null;
			}
			List<Data> list = new List<Data>();
			for (int i = 0; i < data.dependencies.Length; i++)
			{
				list.Add(Manager.packData.dataList[data.dependencies[i]]);
			}
			return list;
		}

		public static Object LoadAsset(string file)
		{
			if (string.IsNullOrEmpty(file))
			{
				return null;
			}
			if (!Manager.packData.dataDic.ContainsKey(file))
			{
				return null;
			}
			return Manager.LoadAsset(Manager.packData.dataDic[file]);
		}

		public static Object LoadAsset(Data data)
		{
			return Manager.LoadAsset(data, data.name);
		}

		public static Object LoadAsset(Data data, string name)
		{
			if (data == null)
			{
				return null;
			}
			if (data.asset != null)
			{
				return data.asset.LoadAsset(name);
			}
			return null;
		}

		public static AssetBundleRequest LoadAssetAsync(string file)
		{
			if (string.IsNullOrEmpty(file))
			{
				return null;
			}
			if (!Manager.packData.dataDic.ContainsKey(file))
			{
				return null;
			}
			return Manager.LoadAssetAsync(Manager.packData.dataDic[file]);
		}

		public static AssetBundleRequest LoadAssetAsync(Data data)
		{
			return Manager.LoadAssetAsync(data, data.name);
		}

		public static AssetBundleRequest LoadAssetAsync(Data data, string name)
		{
			if (data == null)
			{
				return new AssetBundleRequestMock(null);
			}
			if (data.asset != null)
			{
				return new AssetBundleRequestUnity(data.asset.LoadAssetAsync(name));
			}
			return new AssetBundleRequestMock(null);
		}

		public static T[] LoadAllAssets<T>(string file) where T : Object
		{
			if (string.IsNullOrEmpty(file))
			{
				return null;
			}
			if (!Manager.packData.dataDic.ContainsKey(file))
			{
				return null;
			}
			return Manager.LoadAllAssets<T>(Manager.packData.dataDic[file]);
		}

		public static T[] LoadAllAssets<T>(Data data) where T : Object
		{
			if (data == null)
			{
				return null;
			}
			if (data.asset != null)
			{
				return data.asset.LoadAllAssets<T>();
			}
			return null;
		}

		public static AssetBundleRequest LoadAllAssetsAsync(string file)
		{
			if (string.IsNullOrEmpty(file))
			{
				return null;
			}
			if (!Manager.packData.dataDic.ContainsKey(file))
			{
				return null;
			}
			return Manager.LoadAllAssetsAsync(Manager.packData.dataDic[file]);
		}

		public static AssetBundleRequest LoadAllAssetsAsync(Data data)
		{
			if (data == null)
			{
				return new AssetBundleRequestMock(null);
			}
			if (data.asset != null)
			{
				return new AssetBundleRequestUnity(data.asset.LoadAllAssetsAsync());
			}
			return new AssetBundleRequestMock(null);
		}

		public static void LoadScene(string file, LoadSceneMode mode = LoadSceneMode.Single)
		{
			if (!Manager.packData.dataDic.ContainsKey(file))
			{
				return;
			}
			Manager.LoadScene(Manager.packData.dataDic[file], mode);
		}

		public static void LoadScene(Data data, LoadSceneMode mode = LoadSceneMode.Single)
		{
			Manager.LoadScene(data, data.name, mode);
		}

		public static void LoadScene(Data data, string name, LoadSceneMode mode = LoadSceneMode.Single)
		{
			if (data == null)
			{
				return;
			}
			if (data.asset == null)
			{
				return;
			}
			global::UnityEngine.SceneManagement.SceneManager.LoadScene(name.Replace(".unity", ""), mode);
		}

		public static AsyncOperation LoadSceneAsync(string file, LoadSceneMode mode = LoadSceneMode.Single)
		{
			if (!Manager.packData.dataDic.ContainsKey(file))
			{
				return null;
			}
			return Manager.LoadSceneAsync(Manager.packData.dataDic[file], mode);
		}

		public static AsyncOperation LoadSceneAsync(Data data, LoadSceneMode mode = LoadSceneMode.Single)
		{
			return Manager.LoadSceneAsync(data, data.name, mode);
		}

		public static AsyncOperation LoadSceneAsync(Data data, string name, LoadSceneMode mode = LoadSceneMode.Single)
		{
			if (data == null)
			{
				return null;
			}
			if (data.asset == null)
			{
				return null;
			}
			return global::UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name.Replace(".unity", ""), mode);
		}

		public static AsyncOperation UnloadSceneAsync(string file)
		{
			if (!Manager.packData.dataDic.ContainsKey(file))
			{
				return null;
			}
			return Manager.UnloadSceneAsync(Manager.packData.dataDic[file]);
		}

		public static AsyncOperation UnloadSceneAsync(Data data)
		{
			return Manager.UnloadSceneAsync(data, data.name);
		}

		public static AsyncOperation UnloadSceneAsync(Data data, string name)
		{
			if (data == null)
			{
				return null;
			}
			if (data.asset == null)
			{
				return null;
			}
			return global::UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(name.Replace(".unity", ""));
		}

		public static byte[] LoadBinary(string file)
		{
			if (!Manager.packData.dataDic.ContainsKey(file))
			{
				return null;
			}
			return Manager.LoadBinary(Manager.packData.dataDic[file]);
		}

		public static byte[] LoadBinary(Data data)
		{
			if (data == null)
			{
				return null;
			}
			return data.bytes;
		}

		public static bool Exists(string file)
		{
			return Manager.Exists(Manager.GetData(file));
		}

		public static bool Exists(Data data)
		{
			if (data == null)
			{
				return false;
			}
			Manager.CheckState(data);
			return data.IsExists || data.IsLoading || data.IsLoaded;
		}

		public static void RemoveCaches()
		{
			try
			{
				if (Directory.Exists(Manager.AssetPath))
				{
					Directory.Delete(Manager.AssetPath, true);
				}
			}
			catch (Exception ex)
			{
				string text = "[RemoveCaches error] ";
				Exception ex2 = ex;
				Verbose<Verbose>.LogError(text + ((ex2 != null) ? ex2.ToString() : null), null);
				if (Manager.onCachesError != null)
				{
					Manager.onCachesError(ex);
				}
			}
			for (int i = 0; i < Manager.packData.dataList.Count; i++)
			{
				Data data = Manager.packData.dataList[i];
				if (data != null && data.state != Data.State.Loaded)
				{
					data.state = Data.State.NotExists;
				}
			}
		}

		public static void RemoveCacheFile(string file)
		{
			if (!Manager.packData.dataDic.ContainsKey(file))
			{
				return;
			}
			Manager.RemoveCacheFile(Manager.packData.dataDic[file]);
		}

		public static void RemoveCacheFile(Data data)
		{
			if (data == null)
			{
				return;
			}
			string text = Manager.AssetPath + data.FullPath;
			try
			{
				if (File.Exists(text))
				{
					File.Delete(text);
				}
			}
			catch (Exception ex)
			{
				string text2 = "[RemoveCacheFile error] ";
				string text3 = text;
				string text4 = " : ";
				Exception ex2 = ex;
				Verbose<Verbose>.LogError(text2 + text3 + text4 + ((ex2 != null) ? ex2.ToString() : null), null);
				if (Manager.onCachesError != null)
				{
					Manager.onCachesError(ex);
				}
			}
			if (data.state != Data.State.Loaded)
			{
				data.state = Data.State.NotExists;
			}
		}

		public static void RemoveCacheFile(IEnumerable<Data> data)
		{
			foreach (Data data2 in data)
			{
				Manager.RemoveCacheFile(data2);
			}
		}

		public static void UnloadAll(bool unloadAllLoadedObjects = true)
		{
			for (int i = 0; i < Manager.packData.dataList.Count; i++)
			{
				Data data = Manager.packData.dataList[i];
				if (data != null)
				{
					data.refcount = 0;
					if (data.asset != null)
					{
						data.asset.Unload(unloadAllLoadedObjects);
					}
					data.asset = null;
					data.bytes = null;
					if (data.state == Data.State.Loaded)
					{
						if (File.Exists(Manager.AssetPath + data.FullPath))
						{
							data.state = Data.State.Exists;
						}
						else
						{
							data.state = Data.State.NotExists;
						}
					}
				}
			}
		}

		public static void Unload(Data data, bool unloadAllLoadedObjects = false)
		{
			Manager._Unload(Manager.GetDependencyData(data), unloadAllLoadedObjects);
			Manager._Unload(data, unloadAllLoadedObjects);
		}

		public static void Unload(IEnumerable<Data> data, bool unloadAllLoadedObjects = false)
		{
			if (data == null)
			{
				return;
			}
			foreach (Data data2 in data)
			{
				Manager.Unload(data2, unloadAllLoadedObjects);
			}
		}

		public static void Unload(string file, bool unloadAllLoadedObjects = false)
		{
			Manager.Unload(Manager.GetData(file), unloadAllLoadedObjects);
		}

		public static Exception Save(Data data, byte[] bytes)
		{
			string text = Manager.AssetPath + data.path;
			string text2 = Manager.AssetPath + data.FullPath;
			try
			{
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				using (FileStream fileStream = new FileStream(text2, FileMode.Create, FileAccess.Write))
				{
					fileStream.Write(bytes, 0, bytes.Length);
				}
			}
			catch (Exception ex)
			{
				return ex;
			}
			return null;
		}

		protected override void OnSingletonDestroy()
		{
			Manager.Exit();
			Manager.UnloadAll(false);
		}

		protected static void _Load(Data data, Action<Data> dlgt, bool dl_only, Loader loader, bool async)
		{
			if (loader == null)
			{
				return;
			}
			loader.Request(data, dlgt, dl_only, async);
		}

		protected static void _Unload(Data data, bool unloadAllLoadedObjects = false)
		{
			if (data == null)
			{
				return;
			}
			if (data.refcount <= 0)
			{
				return;
			}
			int num = data.refcount - 1;
			data.refcount = num;
			if (num > 0)
			{
				return;
			}
			if (data.asset != null)
			{
				data.asset.Unload(unloadAllLoadedObjects);
				data.asset = null;
			}
			if (data.bytes != null)
			{
				data.bytes = null;
			}
			if (data.state == Data.State.Loaded)
			{
				if (File.Exists(Manager.AssetPath + data.FullPath))
				{
					data.state = Data.State.Exists;
					return;
				}
				data.state = Data.State.NotExists;
			}
		}

		protected static void _Unload(IEnumerable<Data> data, bool unloadAllLoadedObjects = false)
		{
			if (data == null)
			{
				return;
			}
			foreach (Data data2 in data)
			{
				Manager._Unload(data2, unloadAllLoadedObjects);
			}
		}

		protected const float TIMEOUT = 4f;

		protected const int RETRY_NUM = 5;

		protected const string HASH_FILE = "hash_file.txt";

		public static Manager.Pack packData = new Manager.Pack();

		private static string hostUrl;

		private static string dataVersion;

		private static Define.Language language = Define.Language.ja;

		private static List<Loader> loaderList = new List<Loader>();

		private static Action<Exception> onCachesError;

		private static Action onLoadFinished;

		private static Action<string, Data, Exception> onLoadError;

		private static Action<string, Data, Exception> onFailedWrite;

		private static bool initialized;

		[SerializeField]
		private float downloadTimeout = 4f;

		[SerializeField]
		private int downloadRetryNum = 5;

		public class Pack
		{
			public void Clear()
			{
				this.dataList.Clear();
				this.dataDic.Clear();
				this.categoryDic.Clear();
				this.tagDic.Clear();
				this.hashDic.Clear();
			}

			public List<Data> dataList = new List<Data>();

			public Dictionary<string, Data> dataDic = new Dictionary<string, Data>();

			public Dictionary<string, List<Data>> categoryDic = new Dictionary<string, List<Data>>();

			public Dictionary<string, List<Data>> tagDic = new Dictionary<string, List<Data>>();

			public Dictionary<string, uint> hashDic = new Dictionary<string, uint>();
		}
	}
}
