using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DMMHelper;
using SGNFW.Common;
using UnityEngine;

namespace SGNFW.Ab
{
	public class LoadRoutine : MonoBehaviour
	{
		public virtual float Progress { get; protected set; }

		public virtual bool IsBusy
		{
			get
			{
				return this.busy;
			}
		}

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

		public virtual float TotalProgress
		{
			get
			{
				return (float)this.loadedCounter + this.Progress;
			}
		}

		public virtual int LoadedCounter
		{
			get
			{
				return this.loadedCounter;
			}
		}

		public virtual int DataNum
		{
			get
			{
				return this.dataList.Count;
			}
		}

		public virtual int LoadDataIndex
		{
			get
			{
				return this.loadIndex;
			}
		}

		public virtual bool IsAbort
		{
			get
			{
				return this.abort;
			}
		}

		public virtual bool IsReady
		{
			get
			{
				return !this.abort && this.loadIndex < this.DataNum;
			}
		}

		public virtual bool IsError
		{
			get
			{
				return this.error;
			}
		}

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

		public static LoadRoutine Create(GameObject go, float timeout, int retryNum)
		{
			LoadRoutine loadRoutine = go.AddComponent<LoadRoutine>();
			loadRoutine.Setup(timeout, retryNum);
			return loadRoutine;
		}

		public virtual void Add(Data data)
		{
			Manager.CheckState(data);
			this.dataList.Add(data);
		}

		public virtual bool Contains(Data data)
		{
			return this.dataList.Contains(data);
		}

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

		public virtual void Abort()
		{
			this.abort = true;
			this.pause = false;
		}

		public virtual void Retry()
		{
			this.retryCounter = 0;
			this.error = false;
			this.pause = false;
		}

		public virtual void Exit()
		{
			this.Abort();
			this.pause = false;
		}

		protected virtual void Setup(float timeout, int retryNum)
		{
			this.downloadTimeout = timeout;
			this.downloadRetryNum = retryNum;
		}

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

		public Action<string, Data, Exception> onFailedRead;

		public Action<Data> onSuccess;

		public Action<LoadRoutine> onFinished;

		public Action<string, Data, Exception> onFailedWrite;

		protected List<Data> dataList = new List<Data>();

		protected LoadRoutine.State state;

		protected int loadedCounter;

		protected int loadIndex;

		protected float timer;

		protected int retryCounter;

		protected bool busy;

		protected bool abort;

		protected bool retry;

		protected float downloadTimeout;

		protected int downloadRetryNum;

		protected bool error;

		protected bool pause;

		protected enum State
		{
			None,
			Running
		}
	}
}
