using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SGNFW.Common;
using SGNFW.Common.Json;
using SGNFW.Login;
using UnityEngine;

namespace SGNFW.Http
{
	public abstract class Command
	{
		public bool IsAccessing
		{
			get
			{
				return this.isAccessing;
			}
		}

		public int RetryCount
		{
			get
			{
				return this.retryCount;
			}
		}

		protected string Url { get; set; }

		protected string Server { get; set; }

		protected string EncryptKey { get; set; }

		protected string UserAgent { get; set; }

		protected float TimeoutTime { get; set; }

		protected bool IsPostMethod { get; set; }

		protected bool IsDummy { get; set; }

		public Command()
		{
			if (Command.st_tm_counter == 0L)
			{
				Command.st_tm_counter = DateTime.Now.Ticks;
			}
			else
			{
				Command.st_tm_counter += 1L;
			}
			this.tm_counter = Command.st_tm_counter;
			this.retryCount = 0;
		}

		public void Execute()
		{
			this.accessTimer += Time.unscaledDeltaTime;
			switch (this.phase)
			{
			case Command.Phase.Begin:
			case Command.Phase.Check:
				this.phase = Command.Phase.Check;
				if (this.IsDummy)
				{
					goto IL_03B8;
				}
				this.isAccessing = true;
				if (Command.waitTime > 0f)
				{
					this.waitTimer = 0f;
					this.phase = Command.Phase.Wait;
					return;
				}
				break;
			case Command.Phase.Wait:
				this.phase = Command.Phase.Wait;
				this.waitTimer += Time.unscaledDeltaTime;
				if (this.waitTimer < Command.waitTime)
				{
					return;
				}
				break;
			case Command.Phase.ReqStart:
				break;
			case Command.Phase.ReqWait:
				this.phase = Command.Phase.ReqWait;
				if (!this.connection.IsBusy)
				{
					this.phase = Command.Phase.ReqFinished;
					return;
				}
				if (this.accessTimer < this.TimeoutTime)
				{
					return;
				}
				if (!this.isDisableLog)
				{
					goto IL_0376;
				}
				goto IL_0376;
			case Command.Phase.ReqFinished:
			{
				this.phase = Command.Phase.ReqFinished;
				if (this.connection.IsError)
				{
					Verbose<Verbose>.LogError("<color=#c0a0a0>[HTTP] Network Error. Retry... " + this.connection.ExceptionStatus.ToString() + "</color>", null);
					goto IL_032E;
				}
				string @string = Encoding.UTF8.GetString(this.connection.Bytes);
				if (!this.isDisableLog && this.connection.Bytes.Length > 15000)
				{
					Encoding.UTF8.GetString(this.connection.Bytes, 0, 15000);
				}
				this.connection.Dispose();
				this.connection = null;
				this.response = this.Parse(@string);
				if (this.response.error_code == null || this.response.error_code.id == 0)
				{
					goto IL_0290;
				}
				goto IL_02B3;
			}
			case Command.Phase.ResSuccess:
				goto IL_0290;
			case Command.Phase.ResError:
				goto IL_02B3;
			case Command.Phase.InternetUnavailable:
				this.phase = Command.Phase.InternetUnavailable;
				if (this.connection != null)
				{
					this.connection.Dispose();
					this.connection = null;
				}
				if (this.onUnavailable != null)
				{
					this.onUnavailable(this);
				}
				if (this.phase == Command.Phase.Exit)
				{
					goto IL_040F;
				}
				goto IL_0406;
			case Command.Phase.ClientError:
				goto IL_032E;
			case Command.Phase.Timeout:
				goto IL_0376;
			case Command.Phase.Pause:
				goto IL_0406;
			case Command.Phase.Dummy:
				goto IL_03B8;
			case Command.Phase.Exit:
				goto IL_040F;
			default:
				return;
			}
			this.phase = Command.Phase.ReqStart;
			this.connection = new Connection();
			this.connection.UserAgent = this.UserAgent;
			this.connection.EncryptKey = this.EncryptKey;
			this.connection.IsPostMethod = this.IsPostMethod;
			this.request._tm_ = this.tm_counter;
			this.request.account = LoginManager.Account;
			this.request.platform = LoginManager.Platform;
			string requestJson = this.getRequestJson(this.request);
			this.connection.Fields = requestJson;
			this.connection.Request(Path.Combine(this.Server, this.Url), null);
			bool flag = this.isDisableLog;
			this.accessTimer = 0f;
			this.phase = Command.Phase.ReqWait;
			return;
			IL_0290:
			this.phase = Command.Phase.ResSuccess;
			if (this.onSuccess != null)
			{
				this.onSuccess(this);
				goto IL_040F;
			}
			goto IL_040F;
			IL_02B3:
			this.phase = Command.Phase.ResError;
			bool flag2 = this.isDisableLog;
			if (this.onError != null)
			{
				this.onError(this);
			}
			if (this.phase == Command.Phase.Exit)
			{
				goto IL_040F;
			}
			goto IL_0406;
			IL_032E:
			this.phase = Command.Phase.ClientError;
			if (this.connection != null)
			{
				this.connection.Dispose();
				this.connection = null;
			}
			if (this.onClientError != null)
			{
				this.onClientError(this);
			}
			if (this.phase == Command.Phase.Exit)
			{
				goto IL_040F;
			}
			goto IL_0406;
			IL_0376:
			this.phase = Command.Phase.Timeout;
			if (this.connection != null)
			{
				this.connection.Dispose();
				this.connection = null;
			}
			if (this.onTimeout != null)
			{
				this.onTimeout(this);
			}
			if (this.phase == Command.Phase.Exit)
			{
				goto IL_040F;
			}
			goto IL_0406;
			IL_03B8:
			this.phase = Command.Phase.Dummy;
			string text = "";
			this.response = this.DummyParse(out text);
			if (this.response.error_code != null && this.response.error_code.id != 0)
			{
				goto IL_02B3;
			}
			if (!this.isDisableLog)
			{
				goto IL_0290;
			}
			goto IL_0290;
			IL_0406:
			this.phase = Command.Phase.Pause;
			return;
			IL_040F:
			this.phase = Command.Phase.Exit;
			this.isAccessing = false;
			if (this.onFinished != null)
			{
				this.onFinished(this);
			}
		}

		public void Retry()
		{
			this.phase = Command.Phase.Check;
			this.retryCount++;
		}

		public void Exit()
		{
			this.phase = Command.Phase.Exit;
		}

		public void Abort()
		{
			this.isAccessing = false;
			if (this.connection != null)
			{
				this.connection.Dispose();
				this.connection = null;
			}
			this.phase = Command.Phase.Exit;
			if (this.onAbort != null)
			{
				this.onAbort(this);
			}
		}

		public void Clear()
		{
			this.accessTimer = 0f;
			this.phase = Command.Phase.Begin;
			this.isAccessing = false;
			this.waitTimer = 0f;
			this.response = null;
			this.tm_counter = 0L;
			if (this.connection != null)
			{
				this.connection.Dispose();
				this.connection = null;
			}
			this.onSuccess = null;
			this.onError = null;
			this.onFinished = null;
			this.onUnavailable = null;
			this.onClientError = null;
			this.onTimeout = null;
			this.onAbort = null;
			this.retryCount = 0;
		}

		public bool IsPause()
		{
			return this.phase == Command.Phase.Pause;
		}

		protected string getRequestJson(Request req)
		{
			return PrjJson.ToJson(req);
		}

		protected abstract Response Parse(string text);

		protected abstract string DummyTextPath();

		protected Response DummyParse(out string dummyText)
		{
			TextAsset textAsset = Resources.Load<TextAsset>(this.DummyTextPath());
			dummyText = Encoding.UTF8.GetString(textAsset.bytes);
			return this.Parse(dummyText);
		}

		public static void SetupRequest(Command cmd, Action<Command> gameCallback)
		{
			Command.SetupRequest(cmd, gameCallback, null, null);
		}

		public static void SetupRequest(Command cmd, Action<Command> gameCallback, Action<Command> errorCallback, Action<Command, ErrorCode> retryCallBack)
		{
			cmd.onSuccess = (Action<Command>)Delegate.Combine(cmd.onSuccess, new Action<Command>(delegate(Command _cmd)
			{
				Command.waitTime = (float)_cmd.response.client_wait;
				if (gameCallback != null)
				{
					gameCallback(_cmd);
				}
			}));
			cmd.onError = (Action<Command>)Delegate.Combine(cmd.onError, new Action<Command>(delegate(Command _cmd)
			{
				if (errorCallback != null)
				{
					errorCallback(_cmd);
				}
			}));
			cmd.onFinished = (Action<Command>)Delegate.Combine(cmd.onFinished, new Action<Command>(delegate(Command _cmd)
			{
			}));
			cmd.onTimeout = (Action<Command>)Delegate.Combine(cmd.onTimeout, new Action<Command>(delegate(Command _cmd)
			{
				if (retryCallBack != null)
				{
					retryCallBack(_cmd, Command.STATIC_ERROR_CODE_LIST[0]);
				}
			}));
			cmd.onUnavailable = (Action<Command>)Delegate.Combine(cmd.onUnavailable, new Action<Command>(delegate(Command _cmd)
			{
				if (retryCallBack != null)
				{
					retryCallBack(_cmd, Command.STATIC_ERROR_CODE_LIST[1]);
				}
			}));
			cmd.onClientError = (Action<Command>)Delegate.Combine(cmd.onClientError, new Action<Command>(delegate(Command _cmd)
			{
				if (retryCallBack != null)
				{
					retryCallBack(_cmd, Command.STATIC_ERROR_CODE_LIST[2]);
				}
			}));
			cmd.onAbort = (Action<Command>)Delegate.Combine(cmd.onAbort, new Action<Command>(delegate(Command _cmd)
			{
			}));
		}

		public static bool IsStaticErrorCode(int errorCode)
		{
			return Command.STATIC_ERROR_CODE_LIST.Exists((ErrorCode item) => item.id == errorCode);
		}

		protected const float DEFAULT_TIMEOUT_TIMER = 15f;

		public static float waitTime;

		private static long st_tm_counter = 0L;

		public Action<Command> onSuccess;

		public Action<Command> onError;

		public Action<Command> onFinished;

		public Action<Command> onUnavailable;

		public Action<Command> onClientError;

		public Action<Command> onTimeout;

		public Action<Command> onAbort;

		public bool isDisableLog = true;

		public Dictionary<string, object> optionMap;

		private float accessTimer;

		private Command.Phase phase;

		private bool isAccessing;

		private float waitTimer;

		private Connection connection;

		private long tm_counter;

		private int retryCount;

		public Response response;

		public Request request;

		private static readonly List<ErrorCode> STATIC_ERROR_CODE_LIST = new List<ErrorCode>
		{
			new ErrorCode
			{
				id = 60001,
				msg = "通信エラーです\n通信状況をご確認ください\nリトライしますか？",
				tit = "リトライ",
				typ = 0
			},
			new ErrorCode
			{
				id = 60002,
				msg = "通信エラーです\n通信状況をご確認ください\nリトライしますか？",
				tit = "リトライ",
				typ = 0
			},
			new ErrorCode
			{
				id = 60003,
				msg = "通信エラーです\n通信状況をご確認ください\nリトライしますか？",
				tit = "リトライ",
				typ = 0
			}
		};

		private enum Phase
		{
			Begin,
			Check,
			Wait,
			ReqStart,
			ReqWait,
			ReqFinished,
			ResSuccess,
			ResError,
			InternetUnavailable,
			ClientError,
			Timeout,
			Pause,
			Dummy,
			Exit
		}

		private enum STATIC_ERROR_CODE_IDX
		{
			TIME_OUT,
			UNAVAILABLE,
			CLIENT_ERROR
		}
	}
}
