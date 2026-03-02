using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace DMMHelper
{
	// Token: 0x02000574 RID: 1396
	public class DMMHelperServer : MonoBehaviour
	{
		// Token: 0x06002E22 RID: 11810 RVA: 0x001B141F File Offset: 0x001AF61F
		private void OnDestroy()
		{
			this.sockInst.Dispose();
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x001B142C File Offset: 0x001AF62C
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

		// Token: 0x06002E24 RID: 11812 RVA: 0x001B14A0 File Offset: 0x001AF6A0
		private void Start()
		{
			this.sockInst = new UdpClient(9999);
			this.sockInst.BeginReceive(new AsyncCallback(this.OnRecv), null);
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x001B14CB File Offset: 0x001AF6CB
		private void Update()
		{
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x001B14D0 File Offset: 0x001AF6D0
		public void SetLoginResult(LoginResult result)
		{
			this.loginResult = result;
			string text = JsonUtility.ToJson(this.loginResult);
			this.sendBytes = Encoding.UTF8.GetBytes(text);
		}

		// Token: 0x040028A3 RID: 10403
		private UdpClient sockInst;

		// Token: 0x040028A4 RID: 10404
		private byte[] sendBytes;

		// Token: 0x040028A5 RID: 10405
		private LoginResult loginResult;
	}
}
