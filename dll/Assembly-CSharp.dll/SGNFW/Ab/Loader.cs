using System;
using System.Collections.Generic;
using UnityEngine;

namespace SGNFW.Ab
{
	public class Loader : MonoBehaviour
	{
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

		public int DataNum
		{
			get
			{
				return this.dataList.Count;
			}
		}

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

		public List<Data> DataList
		{
			get
			{
				return this.dataList;
			}
		}

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

		public void Abort()
		{
			for (int i = 0; i < this.routine.Length; i++)
			{
				this.routine[i].Abort();
			}
		}

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

		protected void _OnSuccess(Data data)
		{
		}

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

		protected void _OnFailedWrite(string path, Data data, Exception exception)
		{
			if (this.onFailedWrite != null)
			{
				this.onFailedWrite(path, data, exception);
			}
		}

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

		protected void _Clear()
		{
			for (int i = 0; i < this.routine.Length; i++)
			{
				this.routine[i].Clear();
			}
			this.dataList.Clear();
			this.state = Loader.State.None;
		}

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

		public Action onFinished;

		public Action<string, Data, Exception> onError;

		public Action<string, Data, Exception> onFailedWrite;

		protected LoadRoutine[] routine = new LoadRoutine[5];

		protected List<Data> dataList = new List<Data>();

		protected Loader.State state;

		protected enum State
		{
			None,
			Running
		}
	}
}
