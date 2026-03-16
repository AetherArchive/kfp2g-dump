using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomGetDataResponse : Response
	{
		public Assets assets;

		public List<MasterRoomFurniture> furniture_list;

		public List<MasterRoomChara> chara_list;

		public List<MasterRoomReceiveStamplog> receive_stamp_log_list;

		public long receive_stamp_num;

		public List<MasterRoomMyset> myset_list;

		public List<MasterRoomFollow> follow_list;

		public List<MasterRoomPassing> passing_list;

		public List<MasterRoomRanking> ranking_list;

		public List<MasterRoomStampLog> stamp_log_list;

		public MasterRoomPublicInfo public_info;

		public List<MasterRoomStampPoint> receive_stamp_list;

		public List<MasterRoomMachineDataModel> master_room_machine_list;
	}
}
