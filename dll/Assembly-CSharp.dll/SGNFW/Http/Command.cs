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
	// Token: 0x0200024C RID: 588
	public abstract class Command
	{
		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x060024F5 RID: 9461 RVA: 0x0019DCB8 File Offset: 0x0019BEB8
		public bool IsAccessing
		{
			get
			{
				return this.isAccessing;
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x060024F6 RID: 9462 RVA: 0x0019DCC0 File Offset: 0x0019BEC0
		public int RetryCount
		{
			get
			{
				return this.retryCount;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x060024F8 RID: 9464 RVA: 0x0019DCD1 File Offset: 0x0019BED1
		// (set) Token: 0x060024F7 RID: 9463 RVA: 0x0019DCC8 File Offset: 0x0019BEC8
		protected string Url { get; set; }

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x060024FA RID: 9466 RVA: 0x0019DCE2 File Offset: 0x0019BEE2
		// (set) Token: 0x060024F9 RID: 9465 RVA: 0x0019DCD9 File Offset: 0x0019BED9
		protected string Server { get; set; }

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x060024FC RID: 9468 RVA: 0x0019DCF3 File Offset: 0x0019BEF3
		// (set) Token: 0x060024FB RID: 9467 RVA: 0x0019DCEA File Offset: 0x0019BEEA
		protected string EncryptKey { get; set; }

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x060024FE RID: 9470 RVA: 0x0019DD04 File Offset: 0x0019BF04
		// (set) Token: 0x060024FD RID: 9469 RVA: 0x0019DCFB File Offset: 0x0019BEFB
		protected string UserAgent { get; set; }

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06002500 RID: 9472 RVA: 0x0019DD15 File Offset: 0x0019BF15
		// (set) Token: 0x060024FF RID: 9471 RVA: 0x0019DD0C File Offset: 0x0019BF0C
		protected float TimeoutTime { get; set; }

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06002502 RID: 9474 RVA: 0x0019DD26 File Offset: 0x0019BF26
		// (set) Token: 0x06002501 RID: 9473 RVA: 0x0019DD1D File Offset: 0x0019BF1D
		protected bool IsPostMethod { get; set; }

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06002504 RID: 9476 RVA: 0x0019DD37 File Offset: 0x0019BF37
		// (set) Token: 0x06002503 RID: 9475 RVA: 0x0019DD2E File Offset: 0x0019BF2E
		protected bool IsDummy { get; set; }

		// Token: 0x06002505 RID: 9477 RVA: 0x0019DD40 File Offset: 0x0019BF40
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

		// Token: 0x06002506 RID: 9478 RVA: 0x0019DD94 File Offset: 0x0019BF94
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

		// Token: 0x06002507 RID: 9479 RVA: 0x0019E1D3 File Offset: 0x0019C3D3
		public void Retry()
		{
			this.phase = Command.Phase.Check;
			this.retryCount++;
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x0019E1EA File Offset: 0x0019C3EA
		public void Exit()
		{
			this.phase = Command.Phase.Exit;
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x0019E1F4 File Offset: 0x0019C3F4
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

		// Token: 0x0600250A RID: 9482 RVA: 0x0019E234 File Offset: 0x0019C434
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

		// Token: 0x0600250B RID: 9483 RVA: 0x0019E2C6 File Offset: 0x0019C4C6
		public bool IsPause()
		{
			return this.phase == Command.Phase.Pause;
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x0019E2D2 File Offset: 0x0019C4D2
		protected string getRequestJson(Request req)
		{
			return PrjJson.ToJson(req);
		}

		// Token: 0x0600250D RID: 9485
		protected abstract Response Parse(string text);

		// Token: 0x0600250E RID: 9486
		protected abstract string DummyTextPath();

		// Token: 0x0600250F RID: 9487 RVA: 0x0019E2DC File Offset: 0x0019C4DC
		protected Response DummyParse(out string dummyText)
		{
			TextAsset textAsset = Resources.Load<TextAsset>(this.DummyTextPath());
			dummyText = Encoding.UTF8.GetString(textAsset.bytes);
			return this.Parse(dummyText);
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x0019E30F File Offset: 0x0019C50F
		public static void SetupRequest(Command cmd, Action<Command> gameCallback)
		{
			Command.SetupRequest(cmd, gameCallback, null, null);
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x0019E31C File Offset: 0x0019C51C
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

		// Token: 0x06002512 RID: 9490 RVA: 0x0019E458 File Offset: 0x0019C658
		public static bool IsStaticErrorCode(int errorCode)
		{
			return Command.STATIC_ERROR_CODE_LIST.Exists((ErrorCode item) => item.id == errorCode);
		}

		// Token: 0x04001B66 RID: 7014
		protected const float DEFAULT_TIMEOUT_TIMER = 15f;

		// Token: 0x04001B67 RID: 7015
		public static float waitTime;

		// Token: 0x04001B68 RID: 7016
		private static long st_tm_counter = 0L;

		// Token: 0x04001B69 RID: 7017
		public Action<Command> onSuccess;

		// Token: 0x04001B6A RID: 7018
		public Action<Command> onError;

		// Token: 0x04001B6B RID: 7019
		public Action<Command> onFinished;

		// Token: 0x04001B6C RID: 7020
		public Action<Command> onUnavailable;

		// Token: 0x04001B6D RID: 7021
		public Action<Command> onClientError;

		// Token: 0x04001B6E RID: 7022
		public Action<Command> onTimeout;

		// Token: 0x04001B6F RID: 7023
		public Action<Command> onAbort;

		// Token: 0x04001B70 RID: 7024
		public bool isDisableLog = true;

		// Token: 0x04001B71 RID: 7025
		public Dictionary<string, object> optionMap;

		// Token: 0x04001B72 RID: 7026
		private float accessTimer;

		// Token: 0x04001B73 RID: 7027
		private Command.Phase phase;

		// Token: 0x04001B74 RID: 7028
		private bool isAccessing;

		// Token: 0x04001B75 RID: 7029
		private float waitTimer;

		// Token: 0x04001B76 RID: 7030
		private Connection connection;

		// Token: 0x04001B77 RID: 7031
		private long tm_counter;

		// Token: 0x04001B78 RID: 7032
		private int retryCount;

		// Token: 0x04001B79 RID: 7033
		public Response response;

		// Token: 0x04001B7A RID: 7034
		public Request request;

		// Token: 0x04001B82 RID: 7042
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

		// Token: 0x02001095 RID: 4245
		private enum Phase
		{
			// Token: 0x04005C26 RID: 23590
			Begin,
			// Token: 0x04005C27 RID: 23591
			Check,
			// Token: 0x04005C28 RID: 23592
			Wait,
			// Token: 0x04005C29 RID: 23593
			ReqStart,
			// Token: 0x04005C2A RID: 23594
			ReqWait,
			// Token: 0x04005C2B RID: 23595
			ReqFinished,
			// Token: 0x04005C2C RID: 23596
			ResSuccess,
			// Token: 0x04005C2D RID: 23597
			ResError,
			// Token: 0x04005C2E RID: 23598
			InternetUnavailable,
			// Token: 0x04005C2F RID: 23599
			ClientError,
			// Token: 0x04005C30 RID: 23600
			Timeout,
			// Token: 0x04005C31 RID: 23601
			Pause,
			// Token: 0x04005C32 RID: 23602
			Dummy,
			// Token: 0x04005C33 RID: 23603
			Exit
		}

		// Token: 0x02001096 RID: 4246
		private enum STATIC_ERROR_CODE_IDX
		{
			// Token: 0x04005C35 RID: 23605
			TIME_OUT,
			// Token: 0x04005C36 RID: 23606
			UNAVAILABLE,
			// Token: 0x04005C37 RID: 23607
			CLIENT_ERROR
		}
	}
}
