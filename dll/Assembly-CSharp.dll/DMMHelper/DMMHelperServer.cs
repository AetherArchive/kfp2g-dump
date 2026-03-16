using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace DMMHelper
{
	public class DMMHelperServer : MonoBehaviour
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
				this.sockInst.EndReceive(result, ref ipendPoint);
				this.sockInst.Send(this.sendBytes, this.sendBytes.Length, ipendPoint);
				this.sockInst.BeginReceive(new AsyncCallback(this.OnRecv), null);
			}
			catch (Exception)
			{
			}
		}

		private void Start()
		{
			this.sockInst = new UdpClient(9999);
			this.sockInst.BeginReceive(new AsyncCallback(this.OnRecv), null);
		}

		private void Update()
		{
		}

		public void SetLoginResult(LoginResult result)
		{
			this.loginResult = result;
			string text = JsonUtility.ToJson(this.loginResult);
			this.sendBytes = Encoding.UTF8.GetBytes(text);
		}

		private UdpClient sockInst;

		private byte[] sendBytes;

		private LoginResult loginResult;
	}
}
