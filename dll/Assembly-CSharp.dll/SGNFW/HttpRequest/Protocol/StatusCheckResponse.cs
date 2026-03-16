using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class StatusCheckResponse : Response
	{
		public int result;

		public int maintenance_login;

		public int dif_version;

		public int friend_code;

		public int not_regist_flg;

		public int dmm_data_linked_flg;

		public int reaccept_flg;

		public Maintenance maintenance;
	}
}
