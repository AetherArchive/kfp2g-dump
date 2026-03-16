using System;
using System.Collections.Generic;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class DataManagerCharaMission
{
	public DataManagerCharaMission(DataManager p)
	{
		this.parentData = p;
	}

	public Dictionary<int, int> LastResultItemMap { get; set; }

	public DataManagerCharaMission.StaticCharaMission GetStaticCharaMissionData(int chId)
	{
		if (this.staticCharaMissionMap.ContainsKey(chId))
		{
			return this.staticCharaMissionMap[chId];
		}
		return new DataManagerCharaMission.StaticCharaMission(chId);
	}

	public DataManagerCharaMission.DynamicCharaMission GetDynamicCharaMissionData(int chId)
	{
		if (this.dynamicCharaMissionMap.ContainsKey(chId))
		{
			return this.dynamicCharaMissionMap[chId];
		}
		return new DataManagerCharaMission.DynamicCharaMission(chId);
	}

	public void RequestGetMissionList()
	{
		if (DataManager.DmChara != null)
		{
			DataManager.DmChara.CharaMissionUpdateRequired = false;
		}
		List<int> list = new List<int> { 8 };
		this.parentData.ServerRequest(MissionListCmd.Create(list), new Action<Command>(this.CbMissionListCmd));
	}

	public void RequestActionMissionRewardOne(DataManagerCharaMission.DynamicCharaMission.MissionOne targetMission)
	{
		this.LastResultItemMap = new Dictionary<int, int>();
		this.parentData.ServerRequest(MissionBonusAcceptCmd.Create(null, new List<AcceptMission> { targetMission.MakeAcceptMission() }), new Action<Command>(this.CbMissionBonusAcceptCmd));
	}

	private void CbMissionListCmd(Command cmd)
	{
		MissionListResponse missionListResponse = cmd.response as MissionListResponse;
		this.dynamicCharaMissionMap = new Dictionary<int, DataManagerCharaMission.DynamicCharaMission>();
		if (missionListResponse.missions == null)
		{
			return;
		}
		foreach (Mission mission in missionListResponse.missions)
		{
			this.UpdateMission(mission);
		}
		this.parentData.UpdateUserAssetByAssets(missionListResponse.assets);
	}

	private void CbMissionBonusAcceptCmd(Command cmd)
	{
		MissionBonusAcceptResponse missionBonusAcceptResponse = cmd.response as MissionBonusAcceptResponse;
		MissionBonusAcceptRequest missionBonusAcceptRequest = cmd.request as MissionBonusAcceptRequest;
		this.LastResultItemMap = new Dictionary<int, int>();
		foreach (AcceptMission acceptMission in missionBonusAcceptRequest.accept_mission_list)
		{
			this.dynamicCharaMissionMap[this.staticMissionMap[acceptMission.mission_id].CharaId].MissionMap[acceptMission.mission_id].Received = true;
		}
		foreach (Item item in missionBonusAcceptResponse.assets.update_item_list)
		{
			if (!this.LastResultItemMap.ContainsKey(item.item_id))
			{
				int item_id = item.item_id;
				int num = item.item_num - DataManager.DmItem.GetUserItemData(item_id).num;
				this.LastResultItemMap.Add(item_id, num);
			}
		}
		this.parentData.UpdateUserAssetByAssets(missionBonusAcceptResponse.assets);
	}

	public void InitializeMstData(MstManager mstManager)
	{
		List<MstCharaMissionData> mst = mstManager.GetMst<List<MstCharaMissionData>>(MstType.CHARA_MISSION_DATA);
		this.staticMissionMap = new Dictionary<int, DataManagerCharaMission.StaticMission>();
		this.staticCharaMissionMap = new Dictionary<int, DataManagerCharaMission.StaticCharaMission>();
		this.dynamicCharaMissionMap = new Dictionary<int, DataManagerCharaMission.DynamicCharaMission>();
		foreach (MstCharaMissionData mstCharaMissionData in mst)
		{
			if (!this.staticMissionMap.ContainsKey(mstCharaMissionData.missionId))
			{
				this.staticMissionMap[mstCharaMissionData.missionId] = new DataManagerCharaMission.StaticMission(mstCharaMissionData);
			}
			if (!this.staticCharaMissionMap.ContainsKey(mstCharaMissionData.charaId))
			{
				this.staticCharaMissionMap[mstCharaMissionData.charaId] = new DataManagerCharaMission.StaticCharaMission(mstCharaMissionData.charaId);
			}
			this.staticCharaMissionMap[mstCharaMissionData.charaId].MissionList.Add(this.staticMissionMap[mstCharaMissionData.missionId]);
		}
	}

	public void UpdateUserDataByServer(List<Mission> srvMissionList)
	{
		foreach (Mission mission in srvMissionList)
		{
			if (this.staticMissionMap.ContainsKey(mission.mission_id))
			{
				this.UpdateMission(mission);
			}
		}
	}

	private void UpdateMission(Mission srvMission)
	{
		if (!this.staticMissionMap.ContainsKey(srvMission.mission_id))
		{
			return;
		}
		DataManagerCharaMission.StaticMission staticMission = this.staticMissionMap[srvMission.mission_id];
		if (!this.dynamicCharaMissionMap.ContainsKey(staticMission.CharaId))
		{
			this.dynamicCharaMissionMap[staticMission.CharaId] = new DataManagerCharaMission.DynamicCharaMission(staticMission.CharaId);
		}
		this.dynamicCharaMissionMap[staticMission.CharaId].MissionMap[srvMission.mission_id] = new DataManagerCharaMission.DynamicCharaMission.MissionOne(srvMission, staticMission);
	}

	private DataManager parentData;

	private Dictionary<int, DataManagerCharaMission.StaticMission> staticMissionMap;

	private Dictionary<int, DataManagerCharaMission.StaticCharaMission> staticCharaMissionMap;

	private Dictionary<int, DataManagerCharaMission.DynamicCharaMission> dynamicCharaMissionMap;

	public class StaticMission
	{
		public int MissionId { get; private set; }

		public int NeedMissionId { get; private set; }

		public int CharaId { get; private set; }

		public string MissionContents { get; private set; }

		public int SortNum { get; private set; }

		public int Denominator { get; private set; }

		public int RewardItemId { get; private set; }

		public int RewardItemNum { get; private set; }

		public bool AlwaysDispFlg { get; private set; }

		public StaticMission(MstCharaMissionData mstMission)
		{
			this.MissionId = mstMission.missionId;
			this.NeedMissionId = mstMission.needMissionId;
			this.CharaId = mstMission.charaId;
			this.MissionContents = mstMission.missionContents;
			this.SortNum = mstMission.sortNum;
			this.Denominator = mstMission.denom;
			this.RewardItemId = mstMission.rewardItemId;
			this.RewardItemNum = mstMission.rewardItemNum;
			this.AlwaysDispFlg = 1 == mstMission.alwaysDispFlg;
		}
	}

	public class StaticCharaMission
	{
		public int CharaId { get; private set; }

		public StaticCharaMission(int chId)
		{
			this.CharaId = chId;
			this.MissionList = new List<DataManagerCharaMission.StaticMission>();
		}

		public List<DataManagerCharaMission.StaticMission> MissionList;
	}

	public class DynamicCharaMission
	{
		public int CharaId { get; private set; }

		public Dictionary<int, DataManagerCharaMission.DynamicCharaMission.MissionOne> MissionMap { get; set; }

		public DynamicCharaMission(int chId)
		{
			this.CharaId = chId;
			this.MissionMap = new Dictionary<int, DataManagerCharaMission.DynamicCharaMission.MissionOne>();
		}

		public class MissionOne
		{
			public int MissionId { get; private set; }

			private int Denominator { get; set; }

			public int Numerator { get; private set; }

			public bool Received { get; set; }

			public long KeyServerTime { get; private set; }

			public bool CanReceive
			{
				get
				{
					return !this.Received && this.Denominator <= this.Numerator;
				}
			}

			public int SortNum { get; private set; }

			public MissionOne(Mission mission, DataManagerCharaMission.StaticMission staticMission)
			{
				this.MissionId = mission.mission_id;
				this.Numerator = mission.mission_status;
				this.Received = 1 == mission.accept_flg;
				this.KeyServerTime = mission.mission_datetime;
				this.Denominator = staticMission.Denominator;
				this.SortNum = staticMission.SortNum;
			}

			public AcceptMission MakeAcceptMission()
			{
				return new AcceptMission
				{
					mission_id = this.MissionId,
					mission_datetime = this.KeyServerTime
				};
			}
		}
	}
}
