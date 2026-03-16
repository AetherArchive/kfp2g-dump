using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace DMMHelper
{
	public class DMMHelperClient : MonoBehaviour
	{
		private void OnDestroy()
		{
			this.sockInst.Dispose();
		}

		private void OnRecv(IAsyncResult result)
		{
			IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Any, 0);
			try
			{
				byte[] array = this.sockInst.EndReceive(result, ref ipendPoint);
				string @string = Encoding.UTF8.GetString(array);
				this.loginResult = JsonUtility.FromJson<LoginResult>(@string);
				this.bRecvResult = true;
				this.sockInst.BeginReceive(new AsyncCallback(this.OnRecv), null);
			}
			catch (Exception)
			{
			}
		}

		private void Awake()
		{
			this.sockInst = new UdpClient();
			this.sockInst.BeginReceive(new AsyncCallback(this.OnRecv), null);
			this.serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
		}

		private void Update()
		{
			if (this.userFunc != null && this.bRecvResult)
			{
				this.userFunc(this.loginResult);
				this.userFunc = null;
				this.bRecvResult = false;
			}
		}

		public void GetLoginResult(LoginResultRecvFunc func)
		{
			if (this.userFunc == null)
			{
				this.userFunc = func;
				this.sockInst.Send(new byte[1], 1, this.serverEP);
			}
		}

		private IPEndPoint serverEP;

		private UdpClient sockInst;

		private LoginResultRecvFunc userFunc;

		private bool bRecvResult;

		private LoginResult loginResult;
	}
}
