using System;
using System.Collections.Generic;
using SGNFW.Common;

namespace SGNFW.Http
{
	// Token: 0x0200024F RID: 591
	public class Manager : Singleton<Manager>
	{
		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06002541 RID: 9537 RVA: 0x0019F167 File Offset: 0x0019D367
		public static Dictionary<string, string> ServerRoot
		{
			get
			{
				return Manager.serverRoot;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06002542 RID: 9538 RVA: 0x0019F16E File Offset: 0x0019D36E
		public static Dictionary<string, string> UserAgent
		{
			get
			{
				return Manager.userAgent;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06002544 RID: 9540 RVA: 0x0019F17D File Offset: 0x0019D37D
		// (set) Token: 0x06002543 RID: 9539 RVA: 0x0019F175 File Offset: 0x0019D375
		public static string DefaultEncryptKey { get; set; }

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06002546 RID: 9542 RVA: 0x0019F18C File Offset: 0x0019D38C
		// (set) Token: 0x06002545 RID: 9541 RVA: 0x0019F184 File Offset: 0x0019D384
		public static string AccountEncryptKey { get; set; }

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06002548 RID: 9544 RVA: 0x0019F19B File Offset: 0x0019D39B
		// (set) Token: 0x06002547 RID: 9543 RVA: 0x0019F193 File Offset: 0x0019D393
		public static string SessionID { get; set; }

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x0600254A RID: 9546 RVA: 0x0019F1B4 File Offset: 0x0019D3B4
		// (set) Token: 0x06002549 RID: 9545 RVA: 0x0019F1A2 File Offset: 0x0019D3A2
		public static string Proxy
		{
			get
			{
				return Manager.proxy;
			}
			set
			{
				Manager.proxy = value;
				Connection.Proxy = Manager.proxy;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x0600254C RID: 9548 RVA: 0x0019F1C3 File Offset: 0x0019D3C3
		// (set) Token: 0x0600254B RID: 9547 RVA: 0x0019F1BB File Offset: 0x0019D3BB
		public static bool IsForceNoSecureRequest
		{
			get
			{
				return Connection.IsForceNoSecureRequest;
			}
			set
			{
				Connection.IsForceNoSecureRequest = value;
			}
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x0019F1CA File Offset: 0x0019D3CA
		public static void SetServerRoot(string key, string value)
		{
			Manager.serverRoot[key] = value;
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x0019F1D8 File Offset: 0x0019D3D8
		public static void SetUserAgent(string key, string value)
		{
			Manager.userAgent[key] = value;
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x0019F1E8 File Offset: 0x0019D3E8
		public static void Add(Command cmd)
		{
			if (Singleton<DataManager>.Instance != null && Singleton<DataManager>.Instance.DisableServerRequestByTutorial)
			{
				return;
			}
			if (Singleton<DataManager>.Instance != null && Singleton<DataManager>.Instance.DisableServerRequestByDebug)
			{
				return;
			}
			if (Manager.cmdList.Contains(cmd))
			{
				return;
			}
			Manager.cmdList.Add(cmd);
			cmd.onFinished = (Action<Command>)Delegate.Combine(cmd.onFinished, new Action<Command>(Manager.OnFinished));
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x0019F264 File Offset: 0x0019D464
		public static void Remove(Command cmd)
		{
			Manager.cmdList.Remove(cmd);
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x0019F272 File Offset: 0x0019D472
		public static void Abort(Command cmd)
		{
			cmd.Abort();
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x0019F27C File Offset: 0x0019D47C
		public static void PauseCmdExit()
		{
			foreach (Command command in Manager.cmdList)
			{
				if (command.IsPause())
				{
					command.Exit();
				}
			}
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x0019F2D8 File Offset: 0x0019D4D8
		public static bool IsCmdProcessing()
		{
			return Manager.cmdList.Count > 0;
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x0019F2E7 File Offset: 0x0019D4E7
		protected override void OnSingletonAwake()
		{
			Verbose<Verbose>.Enabled = true;
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x0019F2EF File Offset: 0x0019D4EF
		private static void OnFinished(Command cmd)
		{
			Manager.Remove(cmd);
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x0019F2F8 File Offset: 0x0019D4F8
		private void Update()
		{
			Command[] array = Manager.cmdList.ToArray();
			int num = 0;
			if (num >= array.Length)
			{
				return;
			}
			array[num].Execute();
		}

		// Token: 0x04001BA0 RID: 7072
		private static Dictionary<string, string> serverRoot = new Dictionary<string, string>();

		// Token: 0x04001BA1 RID: 7073
		private static Dictionary<string, string> userAgent = new Dictionary<string, string>();

		// Token: 0x04001BA2 RID: 7074
		private static List<Command> cmdList = new List<Command>();

		// Token: 0x04001BA3 RID: 7075
		private static string proxy = "localhost:8080";
	}
}
