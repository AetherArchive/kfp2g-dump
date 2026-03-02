using System;
using UnityEngine;

namespace DMM.Games.Client.Sdk
{
	// Token: 0x02000578 RID: 1400
	[Serializable]
	public class NetGameApiResult
	{
		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06002E56 RID: 11862 RVA: 0x001B1BA3 File Offset: 0x001AFDA3
		// (set) Token: 0x06002E57 RID: 11863 RVA: 0x001B1BAB File Offset: 0x001AFDAB
		public string AccessToken
		{
			get
			{
				return this.access_token;
			}
			protected set
			{
				this.access_token = value;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06002E58 RID: 11864 RVA: 0x001B1BB4 File Offset: 0x001AFDB4
		// (set) Token: 0x06002E59 RID: 11865 RVA: 0x001B1BBC File Offset: 0x001AFDBC
		public string OnetimeToken
		{
			get
			{
				return this.onetime_token;
			}
			protected set
			{
				this.onetime_token = value;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06002E5A RID: 11866 RVA: 0x001B1BC5 File Offset: 0x001AFDC5
		// (set) Token: 0x06002E5B RID: 11867 RVA: 0x001B1BCD File Offset: 0x001AFDCD
		public int CanUsePoint
		{
			get
			{
				return this.can_use_point;
			}
			protected set
			{
				this.can_use_point = value;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06002E5C RID: 11868 RVA: 0x001B1BD6 File Offset: 0x001AFDD6
		// (set) Token: 0x06002E5D RID: 11869 RVA: 0x001B1BDE File Offset: 0x001AFDDE
		public int CanUseChip
		{
			get
			{
				return this.can_use_chip;
			}
			protected set
			{
				this.can_use_chip = value;
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06002E5E RID: 11870 RVA: 0x001B1BE7 File Offset: 0x001AFDE7
		// (set) Token: 0x06002E5F RID: 11871 RVA: 0x001B1BEF File Offset: 0x001AFDEF
		public string Nickname
		{
			get
			{
				return this.nickname;
			}
			protected set
			{
				this.nickname = value;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06002E60 RID: 11872 RVA: 0x001B1BF8 File Offset: 0x001AFDF8
		// (set) Token: 0x06002E61 RID: 11873 RVA: 0x001B1C00 File Offset: 0x001AFE00
		public bool InstallStatus
		{
			get
			{
				return this.install_status;
			}
			protected set
			{
				this.install_status = value;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06002E62 RID: 11874 RVA: 0x001B1C09 File Offset: 0x001AFE09
		// (set) Token: 0x06002E63 RID: 11875 RVA: 0x001B1C11 File Offset: 0x001AFE11
		public string PaymentId
		{
			get
			{
				return this.payment_id;
			}
			protected set
			{
				this.payment_id = value;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06002E64 RID: 11876 RVA: 0x001B1C1A File Offset: 0x001AFE1A
		// (set) Token: 0x06002E65 RID: 11877 RVA: 0x001B1C22 File Offset: 0x001AFE22
		public long AcceptTime
		{
			get
			{
				return this.accept_time;
			}
			protected set
			{
				this.accept_time = value;
			}
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x001B1C2C File Offset: 0x001AFE2C
		public static NetGameApiResult Parse(string json)
		{
			NetGameApiResult netGameApiResult = null;
			try
			{
				netGameApiResult = JsonUtility.FromJson<NetGameApiResult>(json);
			}
			catch
			{
			}
			if (netGameApiResult == null)
			{
				netGameApiResult = new NetGameApiResult();
				netGameApiResult.result_code = NetGameApiSdk.ErrorCode.NetworkErrorAccessNetGameApi;
			}
			return netGameApiResult;
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x001B1C6C File Offset: 0x001AFE6C
		public string ToJson()
		{
			return JsonUtility.ToJson(this);
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x001B1C74 File Offset: 0x001AFE74
		public bool IsSuccess()
		{
			return this.result_code == NetGameApiSdk.ErrorCode.Success;
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x001B1C7F File Offset: 0x001AFE7F
		public NetGameApiSdk.ErrorCode GetErrorCode()
		{
			return this.result_code;
		}

		// Token: 0x040028BA RID: 10426
		[SerializeField]
		[HideInInspector]
		protected NetGameApiSdk.ErrorCode result_code;

		// Token: 0x040028BB RID: 10427
		[SerializeField]
		[HideInInspector]
		protected string access_token;

		// Token: 0x040028BC RID: 10428
		[SerializeField]
		[HideInInspector]
		protected string onetime_token;

		// Token: 0x040028BD RID: 10429
		[SerializeField]
		[HideInInspector]
		protected int can_use_point;

		// Token: 0x040028BE RID: 10430
		[SerializeField]
		[HideInInspector]
		protected int can_use_chip;

		// Token: 0x040028BF RID: 10431
		[SerializeField]
		[HideInInspector]
		protected string nickname;

		// Token: 0x040028C0 RID: 10432
		[SerializeField]
		[HideInInspector]
		protected bool install_status;

		// Token: 0x040028C1 RID: 10433
		[SerializeField]
		[HideInInspector]
		protected string payment_id;

		// Token: 0x040028C2 RID: 10434
		[SerializeField]
		[HideInInspector]
		protected long accept_time;
	}
}
