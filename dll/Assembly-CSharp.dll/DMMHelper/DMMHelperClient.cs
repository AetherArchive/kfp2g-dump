using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace DMMHelper
{
	// Token: 0x02000572 RID: 1394
	public class DMMHelperClient : MonoBehaviour
	{
		// Token: 0x06002E1C RID: 11804 RVA: 0x001B12F5 File Offset: 0x001AF4F5
		private void OnDestroy()
		{
			this.sockInst.Dispose();
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x001B1304 File Offset: 0x001AF504
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

		// Token: 0x06002E1E RID: 11806 RVA: 0x001B137C File Offset: 0x001AF57C
		private void Awake()
		{
			this.sockInst = new UdpClient();
			this.sockInst.BeginReceive(new AsyncCallback(this.OnRecv), null);
			this.serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x001B13BC File Offset: 0x001AF5BC
		private void Update()
		{
			if (this.userFunc != null && this.bRecvResult)
			{
				this.userFunc(this.loginResult);
				this.userFunc = null;
				this.bRecvResult = false;
			}
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x001B13ED File Offset: 0x001AF5ED
		public void GetLoginResult(LoginResultRecvFunc func)
		{
			if (this.userFunc == null)
			{
				this.userFunc = func;
				this.sockInst.Send(new byte[1], 1, this.serverEP);
			}
		}

		// Token: 0x0400289B RID: 10395
		private IPEndPoint serverEP;

		// Token: 0x0400289C RID: 10396
		private UdpClient sockInst;

		// Token: 0x0400289D RID: 10397
		private LoginResultRecvFunc userFunc;

		// Token: 0x0400289E RID: 10398
		private bool bRecvResult;

		// Token: 0x0400289F RID: 10399
		private LoginResult loginResult;
	}
}
