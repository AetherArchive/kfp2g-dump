using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

namespace SGNFW.Ab
{
	public class ProxyManager : Singleton<ProxyManager>
	{
		public static void Load(string file, LoadProxy proxy, bool retry = true, bool async = false)
		{
			if (!proxy.gameObject.activeInHierarchy)
			{
				Singleton<ProxyManager>.Instance.StartCoroutine(ProxyManager.LoadWait(file, proxy, retry));
				return;
			}
			LoadProxy.Wrap wrap = null;
			if (!ProxyManager.bank.TryGetValue(file, out wrap))
			{
				wrap = new LoadProxy.Wrap();
				ProxyManager.bank.Add(file, wrap);
			}
			proxy.Insert(wrap);
			if (wrap.obj == null)
			{
				ProxyManager.LoadAsync(wrap, file, retry, async);
				if (ProxyManager.purgePolicy != null && ProxyManager.purgePolicy())
				{
					ProxyManager.Purge(false);
					return;
				}
			}
			else if (wrap.proxy.onLoaded != null)
			{
				wrap.proxy.onLoaded(wrap.obj, wrap.proxy);
			}
		}

		public static void Purge(bool force = false)
		{
			ProxyManager.purgeList.Clear();
			foreach (KeyValuePair<string, LoadProxy.Wrap> keyValuePair in ProxyManager.bank)
			{
				if (ProxyManager.Purge(keyValuePair, force))
				{
					ProxyManager.purgeList.Add(keyValuePair.Key);
				}
			}
			foreach (string text in ProxyManager.purgeList)
			{
				ProxyManager.bank.Remove(text);
			}
		}

		public static void ClearLoadedCallback()
		{
			foreach (KeyValuePair<string, LoadProxy.Wrap> keyValuePair in ProxyManager.bank)
			{
				if (!(keyValuePair.Value.proxy == null))
				{
					keyValuePair.Value.proxy.ClearLoadedCallback();
				}
			}
		}

		protected override void OnSingletonAwake()
		{
			ProxyManager.loader = Loader.Create(base.transform, this.downloadTimeout, this.downloadRetryNum);
			ProxyManager.loader.onFinished = delegate
			{
				if (ProxyManager.onFinished != null)
				{
					ProxyManager.onFinished();
				}
			};
			ProxyManager.loader.onError = delegate(string url, Data data, Exception ex)
			{
				ProxyManager.loader.Retry();
				ProxyManager.LoadError(null, data);
				if (ProxyManager.onError != null)
				{
					ProxyManager.onError(url, data, ex);
				}
			};
			ProxyManager.loader.onFailedWrite = delegate(string path, Data data, Exception ex)
			{
				ProxyManager.loader.Exit();
				if (ProxyManager.onFailedWrite != null)
				{
					ProxyManager.onFailedWrite(path, data, ex);
				}
			};
		}

		protected override void OnSingletonDestroy()
		{
			Manager.Unload(ProxyManager.unloadList, false);
			ProxyManager.unloadList.Clear();
			Object.Destroy(ProxyManager.loader);
		}

		protected static void LoadComplete(LoadProxy.Wrap info, Data data)
		{
			Object @object = Manager.LoadAsset(data);
			if (info == null)
			{
				ProxyManager.bank.TryGetValue(data.name, out info);
			}
			if (info != null)
			{
				info.obj = @object;
				if (info.proxy != null)
				{
					info.proxy.Complete(@object);
				}
				info.state = LoadProxy.State.None;
			}
			ProxyManager.unloadList.Add(data);
		}

		protected static void LoadError(LoadProxy.Wrap info, Data data)
		{
			if (info == null)
			{
				ProxyManager.bank.TryGetValue(data.name, out info);
			}
			if (info != null)
			{
				info.state = LoadProxy.State.None;
			}
		}

		protected static void LoadAsync(LoadProxy.Wrap info, string file, bool retry, bool async)
		{
			if (info.state != LoadProxy.State.None)
			{
				return;
			}
			Action<Data> action = delegate(Data _data)
			{
				if (_data.IsLoaded)
				{
					ProxyManager.LoadComplete(info, _data);
					return;
				}
				ProxyManager.LoadError(info, _data);
			};
			if (ProxyManager.unloadList.Count >= Singleton<ProxyManager>.Instance.unloadExecDataNum && ProxyManager.unloadList.Count >= Singleton<ProxyManager>.Instance.unloadDataNum)
			{
				Manager.Unload(ProxyManager.unloadList.GetRange(0, Singleton<ProxyManager>.Instance.unloadDataNum), false);
				ProxyManager.unloadList.RemoveRange(0, Singleton<ProxyManager>.Instance.unloadDataNum);
			}
			Data data = Manager.GetData(file);
			ProxyManager.loader.Request(data, action, false, async);
			info.state = LoadProxy.State.Running;
		}

		protected static bool Purge(KeyValuePair<string, LoadProxy.Wrap> kv, bool force = false)
		{
			if (kv.Value.obj != null && (force || kv.Value.proxy == null))
			{
				Data data = Manager.GetData(kv.Key);
				if (data != null && data.refcount <= 0)
				{
					Singleton<ProxyManager>.Instance.StartCoroutine(ProxyManager.Purge(kv.Value.obj));
					kv.Value.obj = null;
					kv.Value.state = LoadProxy.State.None;
					kv.Value.proxy = null;
					return true;
				}
			}
			return false;
		}

		protected static IEnumerator Purge(Object obj)
		{
			yield return null;
			Object.DestroyImmediate(obj, true);
			yield break;
		}

		protected static IEnumerator LoadWait(string file, LoadProxy proxy, bool retry)
		{
			while (proxy != null && !proxy.gameObject.activeInHierarchy)
			{
				yield return null;
			}
			if (proxy == null)
			{
				yield break;
			}
			ProxyManager.Load(file, proxy, retry, false);
			yield break;
		}

		protected const float TIMEOUT = 4f;

		protected const int RETRY_NUM = 5;

		protected const int UNLOAD_EXEC_DATA_NUM = 32;

		protected const int UNLOAD_DATA_NUM = 16;

		public static Action onFinished;

		public static Action<string, Data, Exception> onError;

		public static Action<string, Data, Exception> onFailedWrite;

		public static Func<bool> purgePolicy;

		protected static Dictionary<string, LoadProxy.Wrap> bank = new Dictionary<string, LoadProxy.Wrap>();

		protected static Loader loader;

		protected static List<string> purgeList = new List<string>();

		protected static List<Data> unloadList = new List<Data>();

		[SerializeField]
		private float downloadTimeout = 4f;

		[SerializeField]
		private int downloadRetryNum = 5;

		[SerializeField]
		private int unloadExecDataNum = 32;

		[SerializeField]
		private int unloadDataNum = 16;
	}
}
