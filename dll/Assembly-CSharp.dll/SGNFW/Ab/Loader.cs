using System;
using System.Collections.Generic;
using UnityEngine;

namespace SGNFW.Ab
{
	// Token: 0x02000284 RID: 644
	public class Loader : MonoBehaviour
	{
		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x0600270A RID: 9994 RVA: 0x001A466C File Offset: 0x001A286C
		public float Progress
		{
			get
			{
				if (this.dataList.Count <= 0)
				{
					return 1f;
				}
				float num = 0f;
				for (int i = 0; i < this.routine.Length; i++)
				{
					num += this.routine[i].TotalProgress;
				}
				return num / (float)this.dataList.Count;
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x0600270B RID: 9995 RVA: 0x001A46C4 File Offset: 0x001A28C4
		public int DataNum
		{
			get
			{
				return this.dataList.Count;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x0600270C RID: 9996 RVA: 0x001A46D4 File Offset: 0x001A28D4
		public int LoadedNum
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.routine.Length; i++)
				{
					num += this.routine[i].LoadedCounter;
				}
				return num;
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x0600270D RID: 9997 RVA: 0x001A4708 File Offset: 0x001A2908
		public bool IsBusy
		{
			get
			{
				for (int i = 0; i < this.routine.Length; i++)
				{
					if (this.routine[i].IsBusy)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x0600270E RID: 9998 RVA: 0x001A473C File Offset: 0x001A293C
		public bool IsDone
		{
			get
			{
				for (int i = 0; i < this.routine.Length; i++)
				{
					LoadRoutine loadRoutine = this.routine[i];
					if (loadRoutine.IsBusy || loadRoutine.IsReady)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x0600270F RID: 9999 RVA: 0x001A4778 File Offset: 0x001A2978
		public bool IsReady
		{
			get
			{
				for (int i = 0; i < this.routine.Length; i++)
				{
					if (this.routine[i].IsReady)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06002710 RID: 10000 RVA: 0x001A47AC File Offset: 0x001A29AC
		public bool IsError
		{
			get
			{
				for (int i = 0; i < this.routine.Length; i++)
				{
					if (this.routine[i].IsError)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06002711 RID: 10001 RVA: 0x001A47DE File Offset: 0x001A29DE
		public List<Data> DataList
		{
			get
			{
				return this.dataList;
			}
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x001A47E8 File Offset: 0x001A29E8
		public static Loader Create(Transform root, float timeout, int retryNum)
		{
			GameObject gameObject = new GameObject("ab_loader", new Type[] { typeof(Loader) });
			if (root != null)
			{
				gameObject.transform.SetParent(root);
			}
			Loader component = gameObject.GetComponent<Loader>();
			component.Setup(timeout, retryNum);
			return component;
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x001A4838 File Offset: 0x001A2A38
		public void Request(Data data, Action<Data> dlgt = null, bool dl_only = false, bool async = false)
		{
			List<Data> datas = Manager.GetDependencyData(data) as List<Data>;
			if (datas == null || datas.Count == 0)
			{
				this._Request(data, dlgt, dl_only, true, async);
				return;
			}
			datas.Add(data);
			Action<Data> action = delegate(Data d)
			{
				datas.Remove(d);
				if (datas.Count == 0 && dlgt != null)
				{
					dlgt(data);
				}
			};
			this._Request(new List<Data>(datas), action, dl_only, true, async);
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x001A48D0 File Offset: 0x001A2AD0
		public void Request(IEnumerable<Data> data, Action<Data> dlgt = null, bool dl_only = false, bool async = false)
		{
			if (data == null)
			{
				return;
			}
			foreach (Data data2 in data)
			{
				this.Request(data2, dlgt, dl_only, async);
			}
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x001A4920 File Offset: 0x001A2B20
		public void Abort()
		{
			for (int i = 0; i < this.routine.Length; i++)
			{
				this.routine[i].Abort();
			}
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x001A4950 File Offset: 0x001A2B50
		public virtual void Retry()
		{
			for (int i = 0; i < this.routine.Length; i++)
			{
				LoadRoutine loadRoutine = this.routine[i];
				if (loadRoutine.IsBusy && !loadRoutine.IsAbort)
				{
					loadRoutine.Retry();
				}
			}
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x001A4990 File Offset: 0x001A2B90
		public virtual void Exit()
		{
			for (int i = 0; i < this.routine.Length; i++)
			{
				LoadRoutine loadRoutine = this.routine[i];
				if (loadRoutine.IsBusy && !loadRoutine.IsAbort)
				{
					loadRoutine.Exit();
				}
			}
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x001A49D0 File Offset: 0x001A2BD0
		private void Setup(float timeout, int retryNum)
		{
			for (int i = 0; i < this.routine.Length; i++)
			{
				this.routine[i] = LoadRoutine.Create(base.gameObject, timeout, retryNum);
				LoadRoutine loadRoutine = this.routine[i];
				loadRoutine.onFinished = (Action<LoadRoutine>)Delegate.Combine(loadRoutine.onFinished, new Action<LoadRoutine>(this._OnFinished));
				LoadRoutine loadRoutine2 = this.routine[i];
				loadRoutine2.onSuccess = (Action<Data>)Delegate.Combine(loadRoutine2.onSuccess, new Action<Data>(this._OnSuccess));
				LoadRoutine loadRoutine3 = this.routine[i];
				loadRoutine3.onFailedRead = (Action<string, Data, Exception>)Delegate.Combine(loadRoutine3.onFailedRead, new Action<string, Data, Exception>(this._OnFailedRead));
				LoadRoutine loadRoutine4 = this.routine[i];
				loadRoutine4.onFailedWrite = (Action<string, Data, Exception>)Delegate.Combine(loadRoutine4.onFailedWrite, new Action<string, Data, Exception>(this._OnFailedWrite));
			}
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x001A4AB0 File Offset: 0x001A2CB0
		protected void _Request(Data data, Action<Data> dlgt, bool dl_only, bool retry, bool async)
		{
			if (data == null)
			{
				return;
			}
			data.isDownloadOnly = dl_only;
			data.retry = retry;
			data.async = async;
			data.onCompleted = (Action<Data>)Delegate.Combine(data.onCompleted, dlgt);
			this.state = Loader.State.Running;
			if (!this.dataList.Contains(data))
			{
				this.dataList.Add(data);
				if (!dl_only)
				{
					data.refcount++;
				}
				LoadRoutine loadRoutine = this._GetFreeRoutine();
				if (loadRoutine != null)
				{
					loadRoutine.Add(data);
					return;
				}
			}
			else if (!dl_only)
			{
				data.refcount++;
			}
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x001A4B4C File Offset: 0x001A2D4C
		protected void _Request(IEnumerable<Data> data, Action<Data> dlgt, bool dl_only, bool retry, bool async)
		{
			if (data == null)
			{
				return;
			}
			foreach (Data data2 in data)
			{
				this._Request(data2, dlgt, dl_only, retry, async);
			}
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x001A4BA0 File Offset: 0x001A2DA0
		protected void _OnFinished(LoadRoutine rtn)
		{
			if (this.IsDone)
			{
				this._Clear();
				if (this.onFinished != null)
				{
					this.onFinished();
				}
			}
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x001A4BC3 File Offset: 0x001A2DC3
		protected void _OnSuccess(Data data)
		{
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x001A4BC8 File Offset: 0x001A2DC8
		protected void _OnFailedRead(string url, Data data, Exception exception)
		{
			for (int i = 0; i < this.routine.Length; i++)
			{
				this.routine[i].Pause = true;
			}
			if (this.onError != null)
			{
				this.onError(url, data, exception);
			}
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x001A4C0C File Offset: 0x001A2E0C
		protected void _OnFailedWrite(string path, Data data, Exception exception)
		{
			if (this.onFailedWrite != null)
			{
				this.onFailedWrite(path, data, exception);
			}
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x001A4C24 File Offset: 0x001A2E24
		protected LoadRoutine _GetFreeRoutine()
		{
			LoadRoutine loadRoutine = null;
			int num = int.MaxValue;
			for (int i = 0; i < this.routine.Length; i++)
			{
				if (num > this.routine[i].Duty)
				{
					num = this.routine[i].Duty;
					loadRoutine = this.routine[i];
				}
			}
			return loadRoutine;
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x001A4C74 File Offset: 0x001A2E74
		protected void _Clear()
		{
			for (int i = 0; i < this.routine.Length; i++)
			{
				this.routine[i].Clear();
			}
			this.dataList.Clear();
			this.state = Loader.State.None;
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x001A4CB4 File Offset: 0x001A2EB4
		private void Update()
		{
			for (int i = 0; i < this.routine.Length; i++)
			{
				LoadRoutine loadRoutine = this.routine[i];
				if (!loadRoutine.IsBusy)
				{
					if (loadRoutine.IsAbort && loadRoutine.DataNum <= 0)
					{
						loadRoutine.Clear();
					}
					if (loadRoutine.DataNum > loadRoutine.LoadDataIndex)
					{
						base.StartCoroutine(loadRoutine.Run());
					}
				}
			}
		}

		// Token: 0x04001CA1 RID: 7329
		public Action onFinished;

		// Token: 0x04001CA2 RID: 7330
		public Action<string, Data, Exception> onError;

		// Token: 0x04001CA3 RID: 7331
		public Action<string, Data, Exception> onFailedWrite;

		// Token: 0x04001CA4 RID: 7332
		protected LoadRoutine[] routine = new LoadRoutine[5];

		// Token: 0x04001CA5 RID: 7333
		protected List<Data> dataList = new List<Data>();

		// Token: 0x04001CA6 RID: 7334
		protected Loader.State state;

		// Token: 0x020010AF RID: 4271
		protected enum State
		{
			// Token: 0x04005C95 RID: 23701
			None,
			// Token: 0x04005C96 RID: 23702
			Running
		}
	}
}
