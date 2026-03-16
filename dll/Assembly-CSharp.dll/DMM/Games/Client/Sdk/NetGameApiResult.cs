using System;
using UnityEngine;

namespace DMM.Games.Client.Sdk
{
	[Serializable]
	public class NetGameApiResult
	{
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

		public string ToJson()
		{
			return JsonUtility.ToJson(this);
		}

		public bool IsSuccess()
		{
			return this.result_code == NetGameApiSdk.ErrorCode.Success;
		}

		public NetGameApiSdk.ErrorCode GetErrorCode()
		{
			return this.result_code;
		}

		[SerializeField]
		[HideInInspector]
		protected NetGameApiSdk.ErrorCode result_code;

		[SerializeField]
		[HideInInspector]
		protected string access_token;

		[SerializeField]
		[HideInInspector]
		protected string onetime_token;

		[SerializeField]
		[HideInInspector]
		protected int can_use_point;

		[SerializeField]
		[HideInInspector]
		protected int can_use_chip;

		[SerializeField]
		[HideInInspector]
		protected string nickname;

		[SerializeField]
		[HideInInspector]
		protected bool install_status;

		[SerializeField]
		[HideInInspector]
		protected string payment_id;

		[SerializeField]
		[HideInInspector]
		protected long accept_time;
	}
}
