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
	// Token: 0x02000287 RID: 647
	public class Manager : Singleton<Manager>
	{
		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06002745 RID: 10053 RVA: 0x001A5179 File Offset: 0x001A3379
		public static string HostUrl
		{
			get
			{
				return Manager.hostUrl;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06002746 RID: 10054 RVA: 0x001A5180 File Offset: 0x001A3380
		public static string DataVersion
		{
			get
			{
				return Manager.dataVersion;
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06002747 RID: 10055 RVA: 0x001A5187 File Offset: 0x001A3387
		public static Define.Language Language
		{
			get
			{
				return Manager.language;
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06002748 RID: 10056 RVA: 0x001A518E File Offset: 0x001A338E
		public static bool Initialized
		{
			get
			{
				return Manager.initialized;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06002749 RID: 10057 RVA: 0x001A5198 File Offset: 0x001A3398
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

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x0600274A RID: 10058 RVA: 0x001A525D File Offset: 0x001A345D
		public static List<Data> DataList
		{
			get
			{
				return Manager.packData.dataList;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x0600274B RID: 10059 RVA: 0x001A5269 File Offset: 0x001A3469
		public static int DataMax
		{
			get
			{
				return Manager.packData.dataList.Count;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x0600274C RID: 10060 RVA: 0x001A527A File Offset: 0x001A347A
		public static List<Data> StorageSaveDataList
		{
			get
			{
				return Manager.packData.dataList.FindAll((Data t) => t.save);
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x0600274D RID: 10061 RVA: 0x001A52AC File Offset: 0x001A34AC
		public static string PlatformString
		{
			get
			{
				return Define.Platform.Windows.ToString();
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x0600274E RID: 10062 RVA: 0x001A52C8 File Offset: 0x001A34C8
		public static string LanguageString
		{
			get
			{
				return Manager.language.ToString();
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x0600274F RID: 10063 RVA: 0x001A52DC File Offset: 0x001A34DC
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

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06002750 RID: 10064 RVA: 0x001A5360 File Offset: 0x001A3560
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

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06002751 RID: 10065 RVA: 0x001A5394 File Offset: 0x001A3594
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

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06002752 RID: 10066 RVA: 0x001A53EC File Offset: 0x001A35EC
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

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06002753 RID: 10067 RVA: 0x001A5474 File Offset: 0x001A3674
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

		// Token: 0x06002754 RID: 10068 RVA: 0x001A54AF File Offset: 0x001A36AF
		public static void Initialize(string url, string version, Define.Language lang)
		{
			Manager.hostUrl = url;
			Manager.dataVersion = version;
			Manager.language = lang;
		}

		// Token: 0x06002755 RID: 10069 RVA: 0x001A54C4 File Offset: 0x001A36C4
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

		// Token: 0x06002756 RID: 10070 RVA: 0x001A55B4 File Offset: 0x001A37B4
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

		// Token: 0x06002757 RID: 10071 RVA: 0x001A56B0 File Offset: 0x001A38B0
		public static Loader Download(Data data, Action<Data> dlgt = null)
		{
			Loader loader = Manager.GetLoader();
			Manager._Load(data, dlgt, true, loader, false);
			return loader;
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x001A56D0 File Offset: 0x001A38D0
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

		// Token: 0x06002759 RID: 10073 RVA: 0x001A5728 File Offset: 0x001A3928
		public static Loader Load(Data data, Action<Data> dlgt = null, bool async = false)
		{
			Loader loader = Manager.GetLoader();
			Manager._Load(data, dlgt, false, loader, async);
			return loader;
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x001A5748 File Offset: 0x001A3948
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

		// Token: 0x0600275B RID: 10075 RVA: 0x001A57A0 File Offset: 0x001A39A0
		public static void Abort()
		{
			for (int i = 0; i < Manager.loaderList.Count; i++)
			{
				Manager.loaderList[i].Abort();
			}
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x001A57D2 File Offset: 0x001A39D2
		public static void Abort(Loader loader)
		{
			if (loader == null)
			{
				return;
			}
			loader.Abort();
		}

		// Token: 0x0600275D RID: 10077 RVA: 0x001A57E4 File Offset: 0x001A39E4
		public static void Retry()
		{
			for (int i = 0; i < Manager.loaderList.Count; i++)
			{
				Manager.loaderList[i].Retry();
			}
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x001A5816 File Offset: 0x001A3A16
		public static void Retry(Loader loader)
		{
			if (loader == null)
			{
				return;
			}
			loader.Retry();
		}

		// Token: 0x0600275F RID: 10079 RVA: 0x001A5828 File Offset: 0x001A3A28
		public static void Exit()
		{
			for (int i = 0; i < Manager.loaderList.Count; i++)
			{
				Manager.loaderList[i].Exit();
			}
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x001A585A File Offset: 0x001A3A5A
		public static void Exit(Loader loader)
		{
			if (loader == null)
			{
				return;
			}
			loader.Exit();
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x001A586C File Offset: 0x001A3A6C
		public static void AddLoadFinishedListener(Action dlgt)
		{
			Manager.onLoadFinished = (Action)Delegate.Combine(Manager.onLoadFinished, dlgt);
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x001A5883 File Offset: 0x001A3A83
		public static void RemoveLoadFinishedListener(Action dlgt)
		{
			Manager.onLoadFinished = (Action)Delegate.Remove(Manager.onLoadFinished, dlgt);
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x001A589A File Offset: 0x001A3A9A
		public static void RemoveAllLoadFinishedListener()
		{
			Manager.onLoadFinished = null;
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x001A58A2 File Offset: 0x001A3AA2
		public static void AddLoadErrorListener(Action<string, Data, Exception> dlgt)
		{
			Manager.onLoadError = (Action<string, Data, Exception>)Delegate.Combine(Manager.onLoadError, dlgt);
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x001A58B9 File Offset: 0x001A3AB9
		public static void RemoveLoadErrorListener(Action<string, Data, Exception> dlgt)
		{
			Manager.onLoadError = (Action<string, Data, Exception>)Delegate.Remove(Manager.onLoadError, dlgt);
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x001A58D0 File Offset: 0x001A3AD0
		public static void RemoveAllLoadErrorListener()
		{
			Manager.onLoadError = null;
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x001A58D8 File Offset: 0x001A3AD8
		public static void AddFailedWriteListener(Action<string, Data, Exception> dlgt)
		{
			Manager.onFailedWrite = (Action<string, Data, Exception>)Delegate.Combine(Manager.onFailedWrite, dlgt);
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x001A58EF File Offset: 0x001A3AEF
		public static void RemoveFailedWriteListener(Action<string, Data, Exception> dlgt)
		{
			Manager.onFailedWrite = (Action<string, Data, Exception>)Delegate.Remove(Manager.onFailedWrite, dlgt);
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x001A5906 File Offset: 0x001A3B06
		public static void RemoveAllFailedWriteListener()
		{
			Manager.onFailedWrite = null;
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x001A590E File Offset: 0x001A3B0E
		public static void AddCachesErrorListener(Action<Exception> dlgt)
		{
			Manager.onCachesError = (Action<Exception>)Delegate.Combine(Manager.onCachesError, dlgt);
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x001A5925 File Offset: 0x001A3B25
		public static void RemoveCachesErrorListener(Action<Exception> dlgt)
		{
			Manager.onCachesError = (Action<Exception>)Delegate.Remove(Manager.onCachesError, dlgt);
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x001A593C File Offset: 0x001A3B3C
		public static void RemoveAllCachesErrorListener()
		{
			Manager.onCachesError = null;
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x001A5944 File Offset: 0x001A3B44
		public static void CheckState(List<Data> data)
		{
			foreach (Data data2 in data)
			{
				Manager.CheckState(data2);
			}
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x001A5990 File Offset: 0x001A3B90
		public static void CheckState(Data[] data)
		{
			for (int i = 0; i < data.Length; i++)
			{
				Manager.CheckState(data[i]);
			}
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x001A59B8 File Offset: 0x001A3BB8
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

		// Token: 0x06002770 RID: 10096 RVA: 0x001A5A5B File Offset: 0x001A3C5B
		public static void SetHash(Data data)
		{
			Manager.packData.hashDic[data.name] = data.hash;
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x001A5A78 File Offset: 0x001A3C78
		public static void Clear()
		{
			Manager.packData.Clear();
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x001A5A84 File Offset: 0x001A3C84
		public static Data GetData(string path)
		{
			if (Manager.packData.dataDic.ContainsKey(path))
			{
				return Manager.packData.dataDic[path];
			}
			return null;
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x001A5AAC File Offset: 0x001A3CAC
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

		// Token: 0x06002774 RID: 10100 RVA: 0x001A5B04 File Offset: 0x001A3D04
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

		// Token: 0x06002775 RID: 10101 RVA: 0x001A5B5C File Offset: 0x001A3D5C
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

		// Token: 0x06002776 RID: 10102 RVA: 0x001A5BB4 File Offset: 0x001A3DB4
		public static IEnumerable<Data> GetDataCategoryTag(string category, string tag)
		{
			return new List<Data>(Manager.GetDataTag(new string[] { tag })).FindAll((Data t) => t.category.Equals(category));
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x001A5BF4 File Offset: 0x001A3DF4
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

		// Token: 0x06002778 RID: 10104 RVA: 0x001A5C50 File Offset: 0x001A3E50
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

		// Token: 0x06002779 RID: 10105 RVA: 0x001A5C85 File Offset: 0x001A3E85
		public static Object LoadAsset(Data data)
		{
			return Manager.LoadAsset(data, data.name);
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x001A5C93 File Offset: 0x001A3E93
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

		// Token: 0x0600277B RID: 10107 RVA: 0x001A5CB6 File Offset: 0x001A3EB6
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

		// Token: 0x0600277C RID: 10108 RVA: 0x001A5CEB File Offset: 0x001A3EEB
		public static AssetBundleRequest LoadAssetAsync(Data data)
		{
			return Manager.LoadAssetAsync(data, data.name);
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x001A5CF9 File Offset: 0x001A3EF9
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

		// Token: 0x0600277E RID: 10110 RVA: 0x001A5D2B File Offset: 0x001A3F2B
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

		// Token: 0x0600277F RID: 10111 RVA: 0x001A5D60 File Offset: 0x001A3F60
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

		// Token: 0x06002780 RID: 10112 RVA: 0x001A5D82 File Offset: 0x001A3F82
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

		// Token: 0x06002781 RID: 10113 RVA: 0x001A5DB7 File Offset: 0x001A3FB7
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

		// Token: 0x06002782 RID: 10114 RVA: 0x001A5DE8 File Offset: 0x001A3FE8
		public static void LoadScene(string file, LoadSceneMode mode = LoadSceneMode.Single)
		{
			if (!Manager.packData.dataDic.ContainsKey(file))
			{
				return;
			}
			Manager.LoadScene(Manager.packData.dataDic[file], mode);
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x001A5E13 File Offset: 0x001A4013
		public static void LoadScene(Data data, LoadSceneMode mode = LoadSceneMode.Single)
		{
			Manager.LoadScene(data, data.name, mode);
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x001A5E22 File Offset: 0x001A4022
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

		// Token: 0x06002785 RID: 10117 RVA: 0x001A5E4D File Offset: 0x001A404D
		public static AsyncOperation LoadSceneAsync(string file, LoadSceneMode mode = LoadSceneMode.Single)
		{
			if (!Manager.packData.dataDic.ContainsKey(file))
			{
				return null;
			}
			return Manager.LoadSceneAsync(Manager.packData.dataDic[file], mode);
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x001A5E79 File Offset: 0x001A4079
		public static AsyncOperation LoadSceneAsync(Data data, LoadSceneMode mode = LoadSceneMode.Single)
		{
			return Manager.LoadSceneAsync(data, data.name, mode);
		}

		// Token: 0x06002787 RID: 10119 RVA: 0x001A5E88 File Offset: 0x001A4088
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

		// Token: 0x06002788 RID: 10120 RVA: 0x001A5EB5 File Offset: 0x001A40B5
		public static AsyncOperation UnloadSceneAsync(string file)
		{
			if (!Manager.packData.dataDic.ContainsKey(file))
			{
				return null;
			}
			return Manager.UnloadSceneAsync(Manager.packData.dataDic[file]);
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x001A5EE0 File Offset: 0x001A40E0
		public static AsyncOperation UnloadSceneAsync(Data data)
		{
			return Manager.UnloadSceneAsync(data, data.name);
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x001A5EEE File Offset: 0x001A40EE
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

		// Token: 0x0600278B RID: 10123 RVA: 0x001A5F1A File Offset: 0x001A411A
		public static byte[] LoadBinary(string file)
		{
			if (!Manager.packData.dataDic.ContainsKey(file))
			{
				return null;
			}
			return Manager.LoadBinary(Manager.packData.dataDic[file]);
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x001A5F45 File Offset: 0x001A4145
		public static byte[] LoadBinary(Data data)
		{
			if (data == null)
			{
				return null;
			}
			return data.bytes;
		}

		// Token: 0x0600278D RID: 10125 RVA: 0x001A5F52 File Offset: 0x001A4152
		public static bool Exists(string file)
		{
			return Manager.Exists(Manager.GetData(file));
		}

		// Token: 0x0600278E RID: 10126 RVA: 0x001A5F5F File Offset: 0x001A415F
		public static bool Exists(Data data)
		{
			if (data == null)
			{
				return false;
			}
			Manager.CheckState(data);
			return data.IsExists || data.IsLoading || data.IsLoaded;
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x001A5F8C File Offset: 0x001A418C
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

		// Token: 0x06002790 RID: 10128 RVA: 0x001A6034 File Offset: 0x001A4234
		public static void RemoveCacheFile(string file)
		{
			if (!Manager.packData.dataDic.ContainsKey(file))
			{
				return;
			}
			Manager.RemoveCacheFile(Manager.packData.dataDic[file]);
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x001A6060 File Offset: 0x001A4260
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

		// Token: 0x06002792 RID: 10130 RVA: 0x001A60EC File Offset: 0x001A42EC
		public static void RemoveCacheFile(IEnumerable<Data> data)
		{
			foreach (Data data2 in data)
			{
				Manager.RemoveCacheFile(data2);
			}
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x001A6134 File Offset: 0x001A4334
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

		// Token: 0x06002794 RID: 10132 RVA: 0x001A61D1 File Offset: 0x001A43D1
		public static void Unload(Data data, bool unloadAllLoadedObjects = false)
		{
			Manager._Unload(Manager.GetDependencyData(data), unloadAllLoadedObjects);
			Manager._Unload(data, unloadAllLoadedObjects);
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x001A61E8 File Offset: 0x001A43E8
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

		// Token: 0x06002796 RID: 10134 RVA: 0x001A6234 File Offset: 0x001A4434
		public static void Unload(string file, bool unloadAllLoadedObjects = false)
		{
			Manager.Unload(Manager.GetData(file), unloadAllLoadedObjects);
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x001A6244 File Offset: 0x001A4444
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

		// Token: 0x06002798 RID: 10136 RVA: 0x001A62C8 File Offset: 0x001A44C8
		protected override void OnSingletonDestroy()
		{
			Manager.Exit();
			Manager.UnloadAll(false);
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x001A62D5 File Offset: 0x001A44D5
		protected static void _Load(Data data, Action<Data> dlgt, bool dl_only, Loader loader, bool async)
		{
			if (loader == null)
			{
				return;
			}
			loader.Request(data, dlgt, dl_only, async);
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x001A62EC File Offset: 0x001A44EC
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

		// Token: 0x0600279B RID: 10139 RVA: 0x001A637C File Offset: 0x001A457C
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

		// Token: 0x04001CBD RID: 7357
		protected const float TIMEOUT = 4f;

		// Token: 0x04001CBE RID: 7358
		protected const int RETRY_NUM = 5;

		// Token: 0x04001CBF RID: 7359
		protected const string HASH_FILE = "hash_file.txt";

		// Token: 0x04001CC0 RID: 7360
		public static Manager.Pack packData = new Manager.Pack();

		// Token: 0x04001CC1 RID: 7361
		private static string hostUrl;

		// Token: 0x04001CC2 RID: 7362
		private static string dataVersion;

		// Token: 0x04001CC3 RID: 7363
		private static Define.Language language = Define.Language.ja;

		// Token: 0x04001CC4 RID: 7364
		private static List<Loader> loaderList = new List<Loader>();

		// Token: 0x04001CC5 RID: 7365
		private static Action<Exception> onCachesError;

		// Token: 0x04001CC6 RID: 7366
		private static Action onLoadFinished;

		// Token: 0x04001CC7 RID: 7367
		private static Action<string, Data, Exception> onLoadError;

		// Token: 0x04001CC8 RID: 7368
		private static Action<string, Data, Exception> onFailedWrite;

		// Token: 0x04001CC9 RID: 7369
		private static bool initialized;

		// Token: 0x04001CCA RID: 7370
		[SerializeField]
		private float downloadTimeout = 4f;

		// Token: 0x04001CCB RID: 7371
		[SerializeField]
		private int downloadRetryNum = 5;

		// Token: 0x020010BF RID: 4287
		public class Pack
		{
			// Token: 0x060053B4 RID: 21428 RVA: 0x0024B9C6 File Offset: 0x00249BC6
			public void Clear()
			{
				this.dataList.Clear();
				this.dataDic.Clear();
				this.categoryDic.Clear();
				this.tagDic.Clear();
				this.hashDic.Clear();
			}

			// Token: 0x04005CE4 RID: 23780
			public List<Data> dataList = new List<Data>();

			// Token: 0x04005CE5 RID: 23781
			public Dictionary<string, Data> dataDic = new Dictionary<string, Data>();

			// Token: 0x04005CE6 RID: 23782
			public Dictionary<string, List<Data>> categoryDic = new Dictionary<string, List<Data>>();

			// Token: 0x04005CE7 RID: 23783
			public Dictionary<string, List<Data>> tagDic = new Dictionary<string, List<Data>>();

			// Token: 0x04005CE8 RID: 23784
			public Dictionary<string, uint> hashDic = new Dictionary<string, uint>();
		}
	}
}
