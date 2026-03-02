using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DMMHelper;
using SGNFW.Common;
using UnityEngine;

namespace SGNFW.Ab
{
	// Token: 0x02000286 RID: 646
	public class LoadRoutine : MonoBehaviour
	{
		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x0600272A RID: 10026 RVA: 0x001A4E74 File Offset: 0x001A3074
		// (set) Token: 0x06002729 RID: 10025 RVA: 0x001A4E6B File Offset: 0x001A306B
		public virtual float Progress { get; protected set; }

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x0600272B RID: 10027 RVA: 0x001A4E7C File Offset: 0x001A307C
		public virtual bool IsBusy
		{
			get
			{
				return this.busy;
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x0600272C RID: 10028 RVA: 0x001A4E84 File Offset: 0x001A3084
		public virtual int Size
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.dataList.Count; i++)
				{
					num += this.dataList[i].size;
				}
				return num;
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x0600272D RID: 10029 RVA: 0x001A4EC0 File Offset: 0x001A30C0
		public virtual int Duty
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.dataList.Count; i++)
				{
					int num2 = this.dataList[i].size;
					if (!this.dataList[i].isDownloadOnly)
					{
						if (this.dataList[i].IsLoaded)
						{
							num2 = 0;
						}
						else if (this.dataList[i].IsExists)
						{
							num2 /= 5;
						}
					}
					else if (this.dataList[i].IsLoaded)
					{
						num2 = 0;
					}
					else if (this.dataList[i].IsLoading)
					{
						num2 = 0;
					}
					else if (this.dataList[i].IsExists)
					{
						num2 = 0;
					}
					num += num2;
				}
				return num;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x0600272E RID: 10030 RVA: 0x001A4F88 File Offset: 0x001A3188
		public virtual float TotalProgress
		{
			get
			{
				return (float)this.loadedCounter + this.Progress;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x0600272F RID: 10031 RVA: 0x001A4F98 File Offset: 0x001A3198
		public virtual int LoadedCounter
		{
			get
			{
				return this.loadedCounter;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06002730 RID: 10032 RVA: 0x001A4FA0 File Offset: 0x001A31A0
		public virtual int DataNum
		{
			get
			{
				return this.dataList.Count;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06002731 RID: 10033 RVA: 0x001A4FAD File Offset: 0x001A31AD
		public virtual int LoadDataIndex
		{
			get
			{
				return this.loadIndex;
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06002732 RID: 10034 RVA: 0x001A4FB5 File Offset: 0x001A31B5
		public virtual bool IsAbort
		{
			get
			{
				return this.abort;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06002733 RID: 10035 RVA: 0x001A4FBD File Offset: 0x001A31BD
		public virtual bool IsReady
		{
			get
			{
				return !this.abort && this.loadIndex < this.DataNum;
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06002734 RID: 10036 RVA: 0x001A4FD7 File Offset: 0x001A31D7
		public virtual bool IsError
		{
			get
			{
				return this.error;
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06002735 RID: 10037 RVA: 0x001A4FDF File Offset: 0x001A31DF
		// (set) Token: 0x06002736 RID: 10038 RVA: 0x001A4FE7 File Offset: 0x001A31E7
		public virtual bool Pause
		{
			get
			{
				return this.pause;
			}
			set
			{
				this.pause = value;
			}
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x001A4FF0 File Offset: 0x001A31F0
		public static LoadRoutine Create(GameObject go, float timeout, int retryNum)
		{
			LoadRoutine loadRoutine = go.AddComponent<LoadRoutine>();
			loadRoutine.Setup(timeout, retryNum);
			return loadRoutine;
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x001A5000 File Offset: 0x001A3200
		public virtual void Add(Data data)
		{
			Manager.CheckState(data);
			this.dataList.Add(data);
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x001A5014 File Offset: 0x001A3214
		public virtual bool Contains(Data data)
		{
			return this.dataList.Contains(data);
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x001A5022 File Offset: 0x001A3222
		public virtual IEnumerator Run()
		{
			if (this.dataList.Count <= this.loadIndex)
			{
				yield break;
			}
			if (this.busy)
			{
				yield break;
			}
			this.busy = true;
			this.retryCounter = 0;
			while (this.loadIndex < this.dataList.Count && !this.abort)
			{
				this.retry = false;
				Data data = this.dataList[this.loadIndex];
				IEnumerator coroutine = this.Run(data);
				while (coroutine.MoveNext())
				{
					yield return null;
				}
				while (this.pause)
				{
					yield return null;
				}
				if (!this.retry)
				{
					this.retryCounter = 0;
					this.loadIndex++;
				}
				yield return null;
				coroutine = null;
			}
			this.busy = false;
			if (this.onFinished != null)
			{
				this.onFinished(this);
			}
			yield break;
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x001A5034 File Offset: 0x001A3234
		public virtual void Clear()
		{
			foreach (Data data in this.dataList)
			{
				data.onCompleted = null;
			}
			this.dataList.Clear();
			this.loadedCounter = 0;
			this.loadIndex = 0;
			this.Progress = 0f;
			this.abort = false;
			this.busy = false;
			this.error = false;
			this.pause = false;
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x001A50C8 File Offset: 0x001A32C8
		public virtual void Abort()
		{
			this.abort = true;
			this.pause = false;
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x001A50D8 File Offset: 0x001A32D8
		public virtual void Retry()
		{
			this.retryCounter = 0;
			this.error = false;
			this.pause = false;
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x001A50EF File Offset: 0x001A32EF
		public virtual void Exit()
		{
			this.Abort();
			this.pause = false;
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x001A50FE File Offset: 0x001A32FE
		protected virtual void Setup(float timeout, int retryNum)
		{
			this.downloadTimeout = timeout;
			this.downloadRetryNum = retryNum;
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x001A510E File Offset: 0x001A330E
		protected virtual IEnumerator Run(Data data)
		{
			if (data.save && (data.IsNeedDownload || data.isDownloadOnly))
			{
				IEnumerator coroutine = this.DownloadAndSave(data);
				while (coroutine.MoveNext())
				{
					yield return null;
				}
				coroutine = null;
			}
			else if (!data.save && !data.isDownloadOnly)
			{
				IEnumerator coroutine = this.DownloadAndLoad(data);
				while (coroutine.MoveNext())
				{
					yield return null;
				}
				coroutine = null;
			}
			else if (!data.IsNeedDownload && !data.isDownloadOnly)
			{
				IEnumerator coroutine = this.Load(data);
				while (coroutine.MoveNext())
				{
					yield return null;
				}
				coroutine = null;
			}
			else
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x001A5124 File Offset: 0x001A3324
		protected virtual IEnumerator DownloadAndSave(Data data)
		{
			string url = null;
			string filePath = null;
			WWW www = null;
			byte[] bytes = null;
			this.state = LoadRoutine.State.Running;
			float prevProgress = (this.Progress = (data.downloadProgress = 0f));
			this.timer = this.downloadTimeout;
			Action _dispose = delegate
			{
				if (www != null)
				{
					www.Dispose();
					www = null;
				}
			};
			Func<bool> _retry = delegate
			{
				if (!data.retry)
				{
					return false;
				}
				this.retryCounter++;
				return this.downloadRetryNum <= 0 || this.retryCounter < this.downloadRetryNum;
			};
			Action<Exception> _error = delegate(Exception e)
			{
				_dispose();
				this.state = LoadRoutine.State.None;
				data.state = Data.State.NotExists;
				DMMHelpManager.abErrorCnt++;
				if (_retry())
				{
					this.retry = true;
					return;
				}
				this.error = true;
				if (this.onFailedRead != null)
				{
					this.onFailedRead(url, data, e);
				}
			};
			Action _wwwError = delegate
			{
				Exception ex4 = new Exception(www.error, Exception.Code.Unknown);
				_error(ex4);
			};
			Action _abort = delegate
			{
				this.state = LoadRoutine.State.None;
				if (data.state == Data.State.Downloading)
				{
					data.state = Data.State.NotExists;
				}
				_dispose();
			};
			Action _success = delegate
			{
				this.loadedCounter++;
				this.Progress = (data.downloadProgress = 0f);
				if (!data.isDownloadOnly)
				{
					if (data.IsAsset)
					{
						data.asset = www.assetBundle;
					}
					else
					{
						data.bytes = bytes;
					}
					data.state = Data.State.Loaded;
				}
				else if (data.state == Data.State.Downloading)
				{
					data.state = Data.State.Exists;
				}
				if (this.onSuccess != null)
				{
					this.onSuccess(data);
				}
				_dispose();
			};
			if (data.IsExists || data.IsLoaded)
			{
				_success();
				if (this.abort)
				{
					_abort();
					yield break;
				}
				if (data.onCompleted != null)
				{
					data.onCompleted(data);
				}
			}
			else if (data.IsNeedDownload)
			{
				url = Manager.DownloadUrl + data.DownloadPath;
				filePath = Manager.AssetPath + data.FullPath;
				data.state = Data.State.Downloading;
				www = new WWW(url);
				while (www != null && !www.isDone)
				{
					this.Progress = (data.downloadProgress = www.progress);
					if (prevProgress < this.Progress)
					{
						this.timer = this.downloadTimeout;
					}
					else
					{
						this.timer -= Time.unscaledDeltaTime;
						if (this.timer <= 0f)
						{
							Exception ex = new Exception("timeout", Exception.Code.Timeout);
							_error(ex);
							yield break;
						}
					}
					prevProgress = this.Progress;
					yield return null;
					if (this.abort)
					{
						_abort();
						yield break;
					}
				}
				if (!string.IsNullOrEmpty(www.error))
				{
					_wwwError();
					yield break;
				}
				bytes = www.bytes;
				if (data.isHashCheck)
				{
					uint hashUint = CRC32.GetHashUint(bytes);
					if (data.hash != hashUint)
					{
						Manager.RemoveCacheFile(data);
						Verbose<Verbose>.LogError(string.Concat(new string[]
						{
							data.name,
							" : hash=",
							data.hash.ToString("X"),
							" / hash=",
							hashUint.ToString("X")
						}), null);
						Exception ex2 = new Exception(string.Empty, Exception.Code.Hash);
						_error(ex2);
						yield break;
					}
				}
				try
				{
					string directoryName = Path.GetDirectoryName(filePath);
					if (!Directory.Exists(directoryName))
					{
						Directory.CreateDirectory(directoryName);
					}
					using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
					{
						fileStream.Write(bytes, 0, bytes.Length);
					}
				}
				catch (Exception ex3)
				{
					if (this.onFailedWrite != null)
					{
						this.onFailedWrite(filePath, data, ex3);
					}
				}
				Manager.SetHash(data);
				_success();
				if (this.abort)
				{
					_abort();
					yield break;
				}
				if (data.onCompleted != null)
				{
					data.onCompleted(data);
				}
			}
			this.state = LoadRoutine.State.None;
			yield break;
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x001A513A File Offset: 0x001A333A
		protected virtual IEnumerator DownloadAndLoad(Data data)
		{
			string url = null;
			WWW www = null;
			this.state = LoadRoutine.State.Running;
			float prevProgress = (this.Progress = (data.downloadProgress = 0f));
			this.timer = this.downloadTimeout;
			Action _dispose = delegate
			{
				if (www != null)
				{
					www.Dispose();
					www = null;
				}
			};
			Func<bool> _retry = delegate
			{
				if (!data.retry)
				{
					return false;
				}
				this.retryCounter++;
				return this.downloadRetryNum <= 0 || this.retryCounter < this.downloadRetryNum;
			};
			Action<Exception> _error = delegate(Exception e)
			{
				_dispose();
				this.state = LoadRoutine.State.None;
				if (data.state == Data.State.Downloading)
				{
					data.state = Data.State.NotExists;
				}
				else if (data.state == Data.State.Loading)
				{
					data.state = Data.State.Exists;
				}
				if (_retry())
				{
					this.retry = true;
					return;
				}
				this.error = true;
				if (this.onFailedRead != null)
				{
					this.onFailedRead(url, data, e);
				}
			};
			Action _wwwError = delegate
			{
				Exception ex3 = new Exception(www.error, Exception.Code.Unknown);
				_error(ex3);
			};
			Action _abort = delegate
			{
				this.state = LoadRoutine.State.None;
				if (data.state == Data.State.Downloading)
				{
					data.state = Data.State.NotExists;
				}
				_dispose();
			};
			Action _success = delegate
			{
				this.loadedCounter++;
				this.Progress = (data.downloadProgress = 0f);
				if (www != null)
				{
					if (data.IsAsset)
					{
						data.asset = www.assetBundle;
					}
					else
					{
						data.bytes = www.bytes;
					}
				}
				data.state = Data.State.Loaded;
				_dispose();
				if (this.onSuccess != null)
				{
					this.onSuccess(data);
				}
			};
			url = Manager.DownloadUrl + data.DownloadPath;
			data.state = Data.State.Downloading;
			www = new WWW(url);
			while (www != null && !www.isDone)
			{
				this.Progress = (data.downloadProgress = www.progress);
				if (prevProgress < this.Progress)
				{
					this.timer = this.downloadTimeout;
				}
				else
				{
					this.timer -= Time.unscaledDeltaTime;
					if (this.timer <= 0f)
					{
						Exception ex = new Exception("timeout", Exception.Code.Timeout);
						_error(ex);
						yield break;
					}
				}
				prevProgress = this.Progress;
				yield return null;
				if (this.abort)
				{
					_abort();
					yield break;
				}
			}
			if (!string.IsNullOrEmpty(www.error))
			{
				_wwwError();
				yield break;
			}
			if (!data.IsAsset && data.isHashCheck)
			{
				uint hashUint = CRC32.GetHashUint(Manager.AssetPath + data.FullPath);
				if (data.hash != hashUint)
				{
					Manager.RemoveCacheFile(data);
					Verbose<Verbose>.LogError(string.Concat(new string[]
					{
						data.name,
						" : hash=",
						data.hash.ToString("X"),
						" / hash=",
						hashUint.ToString("X")
					}), null);
					Exception ex2 = new Exception(string.Empty, Exception.Code.Hash);
					_error(ex2);
					yield break;
				}
			}
			_success();
			if (this.abort)
			{
				_abort();
				yield break;
			}
			if (data.onCompleted != null)
			{
				data.onCompleted(data);
			}
			this.state = LoadRoutine.State.None;
			yield break;
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x001A5150 File Offset: 0x001A3350
		protected virtual IEnumerator Load(Data data)
		{
			string url = null;
			WWW www = null;
			this.state = LoadRoutine.State.Running;
			this.Progress = (data.downloadProgress = 0f);
			this.timer = this.downloadTimeout;
			Action _dispose = delegate
			{
				if (www != null)
				{
					www.Dispose();
					www = null;
				}
			};
			Func<bool> _retry = delegate
			{
				if (!data.retry)
				{
					return false;
				}
				this.retryCounter++;
				return this.downloadRetryNum <= 0 || this.retryCounter < this.downloadRetryNum;
			};
			Action<Exception, bool> _error = delegate(Exception e, bool r)
			{
				_dispose();
				this.state = LoadRoutine.State.None;
				if (data.state == Data.State.Downloading)
				{
					data.state = Data.State.NotExists;
				}
				else if (data.state == Data.State.Loading)
				{
					data.state = Data.State.Exists;
				}
				if (r && _retry())
				{
					this.retry = true;
					return;
				}
				if (this.onFailedRead != null)
				{
					this.onFailedRead(url, data, e);
				}
			};
			Action _wwwError = delegate
			{
				Exception ex = new Exception(www.error, Exception.Code.Unknown);
				_error(ex, false);
			};
			Action _abort = delegate
			{
				this.state = LoadRoutine.State.None;
				if (data.state == Data.State.Loading)
				{
					data.state = Data.State.Exists;
				}
				_dispose();
			};
			Action _success = delegate
			{
				this.loadedCounter++;
				this.Progress = 0f;
				if (www != null)
				{
					if (data.IsAsset)
					{
						data.asset = www.assetBundle;
					}
					else
					{
						data.bytes = www.bytes;
					}
				}
				data.state = Data.State.Loaded;
				_dispose();
				if (this.onSuccess != null)
				{
					this.onSuccess(data);
				}
				if (data.onCompleted != null)
				{
					data.onCompleted(data);
				}
			};
			Action<Action> _successAsync = delegate(Action onFinish)
			{
				Action action = delegate
				{
					data.state = Data.State.Loaded;
					_dispose();
					if (this.onSuccess != null)
					{
						this.onSuccess(data);
					}
					onFinish();
					if (data.onCompleted != null)
					{
						data.onCompleted(data);
					}
				};
				this.loadedCounter++;
				this.Progress = (data.downloadProgress = 0f);
				if (www != null)
				{
					if (data.IsAsset)
					{
						data.asset = www.assetBundle;
					}
					else
					{
						data.bytes = www.bytes;
					}
				}
				action();
			};
			if (data.IsLoaded)
			{
				if (data.async)
				{
					LoadRoutine.<>c__DisplayClass57_2 CS$<>8__locals2 = new LoadRoutine.<>c__DisplayClass57_2();
					CS$<>8__locals2.finish = false;
					_successAsync(delegate
					{
						CS$<>8__locals2.finish = true;
					});
					while (!CS$<>8__locals2.finish)
					{
						yield return null;
					}
					CS$<>8__locals2 = null;
				}
				else
				{
					_success();
				}
			}
			else if (data.IsExists)
			{
				url = Manager.LoadPathAtWWW + data.DownloadPath;
				data.state = Data.State.Loading;
				www = new WWW(url);
				while (!www.isDone)
				{
					this.Progress = (data.downloadProgress = www.progress);
					yield return null;
					if (this.abort)
					{
						_abort();
						yield break;
					}
				}
				if (!string.IsNullOrEmpty(www.error))
				{
					_wwwError();
					yield break;
				}
				if (data.async)
				{
					LoadRoutine.<>c__DisplayClass57_3 CS$<>8__locals3 = new LoadRoutine.<>c__DisplayClass57_3();
					CS$<>8__locals3.finish = false;
					_successAsync(delegate
					{
						CS$<>8__locals3.finish = true;
					});
					while (!CS$<>8__locals3.finish)
					{
						yield return null;
					}
					CS$<>8__locals3 = null;
				}
				else
				{
					_success();
				}
			}
			this.state = LoadRoutine.State.None;
			yield break;
		}

		// Token: 0x04001CAB RID: 7339
		public Action<string, Data, Exception> onFailedRead;

		// Token: 0x04001CAC RID: 7340
		public Action<Data> onSuccess;

		// Token: 0x04001CAD RID: 7341
		public Action<LoadRoutine> onFinished;

		// Token: 0x04001CAE RID: 7342
		public Action<string, Data, Exception> onFailedWrite;

		// Token: 0x04001CAF RID: 7343
		protected List<Data> dataList = new List<Data>();

		// Token: 0x04001CB0 RID: 7344
		protected LoadRoutine.State state;

		// Token: 0x04001CB1 RID: 7345
		protected int loadedCounter;

		// Token: 0x04001CB2 RID: 7346
		protected int loadIndex;

		// Token: 0x04001CB3 RID: 7347
		protected float timer;

		// Token: 0x04001CB4 RID: 7348
		protected int retryCounter;

		// Token: 0x04001CB5 RID: 7349
		protected bool busy;

		// Token: 0x04001CB6 RID: 7350
		protected bool abort;

		// Token: 0x04001CB7 RID: 7351
		protected bool retry;

		// Token: 0x04001CB8 RID: 7352
		protected float downloadTimeout;

		// Token: 0x04001CB9 RID: 7353
		protected int downloadRetryNum;

		// Token: 0x04001CBA RID: 7354
		protected bool error;

		// Token: 0x04001CBB RID: 7355
		protected bool pause;

		// Token: 0x020010B3 RID: 4275
		protected enum State
		{
			// Token: 0x04005CA1 RID: 23713
			None,
			// Token: 0x04005CA2 RID: 23714
			Running
		}
	}
}
