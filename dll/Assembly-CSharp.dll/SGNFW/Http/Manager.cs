using System;
using System.Collections.Generic;
using SGNFW.Common;

namespace SGNFW.Http
{
	public class Manager : Singleton<Manager>
	{
		public static Dictionary<string, string> ServerRoot
		{
			get
			{
				return Manager.serverRoot;
			}
		}

		public static Dictionary<string, string> UserAgent
		{
			get
			{
				return Manager.userAgent;
			}
		}

		public static string DefaultEncryptKey { get; set; }

		public static string AccountEncryptKey { get; set; }

		public static string SessionID { get; set; }

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

		public static void SetServerRoot(string key, string value)
		{
			Manager.serverRoot[key] = value;
		}

		public static void SetUserAgent(string key, string value)
		{
			Manager.userAgent[key] = value;
		}

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

		public static void Remove(Command cmd)
		{
			Manager.cmdList.Remove(cmd);
		}

		public static void Abort(Command cmd)
		{
			cmd.Abort();
		}

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

		public static bool IsCmdProcessing()
		{
			return Manager.cmdList.Count > 0;
		}

		protected override void OnSingletonAwake()
		{
			Verbose<Verbose>.Enabled = true;
		}

		private static void OnFinished(Command cmd)
		{
			Manager.Remove(cmd);
		}

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

		private static Dictionary<string, string> serverRoot = new Dictionary<string, string>();

		private static Dictionary<string, string> userAgent = new Dictionary<string, string>();

		private static List<Command> cmdList = new List<Command>();

		private static string proxy = "localhost:8080";
	}
}
