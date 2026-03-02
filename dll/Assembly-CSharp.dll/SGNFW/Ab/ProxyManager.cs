using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

namespace SGNFW.Ab
{
	// Token: 0x02000288 RID: 648
	public class ProxyManager : Singleton<ProxyManager>
	{
		// Token: 0x0600279E RID: 10142 RVA: 0x001A6400 File Offset: 0x001A4600
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

		// Token: 0x0600279F RID: 10143 RVA: 0x001A64B4 File Offset: 0x001A46B4
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

		// Token: 0x060027A0 RID: 10144 RVA: 0x001A656C File Offset: 0x001A476C
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

		// Token: 0x060027A1 RID: 10145 RVA: 0x001A65DC File Offset: 0x001A47DC
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

		// Token: 0x060027A2 RID: 10146 RVA: 0x001A6680 File Offset: 0x001A4880
		protected override void OnSingletonDestroy()
		{
			Manager.Unload(ProxyManager.unloadList, false);
			ProxyManager.unloadList.Clear();
			Object.Destroy(ProxyManager.loader);
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x001A66A4 File Offset: 0x001A48A4
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

		// Token: 0x060027A4 RID: 10148 RVA: 0x001A6704 File Offset: 0x001A4904
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

		// Token: 0x060027A5 RID: 10149 RVA: 0x001A6728 File Offset: 0x001A4928
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

		// Token: 0x060027A6 RID: 10150 RVA: 0x001A67DC File Offset: 0x001A49DC
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

		// Token: 0x060027A7 RID: 10151 RVA: 0x001A6873 File Offset: 0x001A4A73
		protected static IEnumerator Purge(Object obj)
		{
			yield return null;
			Object.DestroyImmediate(obj, true);
			yield break;
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x001A6882 File Offset: 0x001A4A82
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

		// Token: 0x04001CCC RID: 7372
		protected const float TIMEOUT = 4f;

		// Token: 0x04001CCD RID: 7373
		protected const int RETRY_NUM = 5;

		// Token: 0x04001CCE RID: 7374
		protected const int UNLOAD_EXEC_DATA_NUM = 32;

		// Token: 0x04001CCF RID: 7375
		protected const int UNLOAD_DATA_NUM = 16;

		// Token: 0x04001CD0 RID: 7376
		public static Action onFinished;

		// Token: 0x04001CD1 RID: 7377
		public static Action<string, Data, Exception> onError;

		// Token: 0x04001CD2 RID: 7378
		public static Action<string, Data, Exception> onFailedWrite;

		// Token: 0x04001CD3 RID: 7379
		public static Func<bool> purgePolicy;

		// Token: 0x04001CD4 RID: 7380
		protected static Dictionary<string, LoadProxy.Wrap> bank = new Dictionary<string, LoadProxy.Wrap>();

		// Token: 0x04001CD5 RID: 7381
		protected static Loader loader;

		// Token: 0x04001CD6 RID: 7382
		protected static List<string> purgeList = new List<string>();

		// Token: 0x04001CD7 RID: 7383
		protected static List<Data> unloadList = new List<Data>();

		// Token: 0x04001CD8 RID: 7384
		[SerializeField]
		private float downloadTimeout = 4f;

		// Token: 0x04001CD9 RID: 7385
		[SerializeField]
		private int downloadRetryNum = 5;

		// Token: 0x04001CDA RID: 7386
		[SerializeField]
		private int unloadExecDataNum = 32;

		// Token: 0x04001CDB RID: 7387
		[SerializeField]
		private int unloadDataNum = 16;
	}
}
