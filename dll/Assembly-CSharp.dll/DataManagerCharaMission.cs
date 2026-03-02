using System;
using System.Collections.Generic;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x02000076 RID: 118
public class DataManagerCharaMission
{
	// Token: 0x06000420 RID: 1056 RVA: 0x0001C5E6 File Offset: 0x0001A7E6
	public DataManagerCharaMission(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x06000421 RID: 1057 RVA: 0x0001C5F5 File Offset: 0x0001A7F5
	// (set) Token: 0x06000422 RID: 1058 RVA: 0x0001C5FD File Offset: 0x0001A7FD
	public Dictionary<int, int> LastResultItemMap { get; set; }

	// Token: 0x06000423 RID: 1059 RVA: 0x0001C606 File Offset: 0x0001A806
	public DataManagerCharaMission.StaticCharaMission GetStaticCharaMissionData(int chId)
	{
		if (this.staticCharaMissionMap.ContainsKey(chId))
		{
			return this.staticCharaMissionMap[chId];
		}
		return new DataManagerCharaMission.StaticCharaMission(chId);
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x0001C629 File Offset: 0x0001A829
	public DataManagerCharaMission.DynamicCharaMission GetDynamicCharaMissionData(int chId)
	{
		if (this.dynamicCharaMissionMap.ContainsKey(chId))
		{
			return this.dynamicCharaMissionMap[chId];
		}
		return new DataManagerCharaMission.DynamicCharaMission(chId);
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x0001C64C File Offset: 0x0001A84C
	public void RequestGetMissionList()
	{
		if (DataManager.DmChara != null)
		{
			DataManager.DmChara.CharaMissionUpdateRequired = false;
		}
		List<int> list = new List<int> { 8 };
		this.parentData.ServerRequest(MissionListCmd.Create(list), new Action<Command>(this.CbMissionListCmd));
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x0001C695 File Offset: 0x0001A895
	public void RequestActionMissionRewardOne(DataManagerCharaMission.DynamicCharaMission.MissionOne targetMission)
	{
		this.LastResultItemMap = new Dictionary<int, int>();
		this.parentData.ServerRequest(MissionBonusAcceptCmd.Create(null, new List<AcceptMission> { targetMission.MakeAcceptMission() }), new Action<Command>(this.CbMissionBonusAcceptCmd));
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x0001C6D0 File Offset: 0x0001A8D0
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

	// Token: 0x06000428 RID: 1064 RVA: 0x0001C754 File Offset: 0x0001A954
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

	// Token: 0x06000429 RID: 1065 RVA: 0x0001C894 File Offset: 0x0001AA94
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

	// Token: 0x0600042A RID: 1066 RVA: 0x0001C990 File Offset: 0x0001AB90
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

	// Token: 0x0600042B RID: 1067 RVA: 0x0001C9F4 File Offset: 0x0001ABF4
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

	// Token: 0x040004E6 RID: 1254
	private DataManager parentData;

	// Token: 0x040004E7 RID: 1255
	private Dictionary<int, DataManagerCharaMission.StaticMission> staticMissionMap;

	// Token: 0x040004E8 RID: 1256
	private Dictionary<int, DataManagerCharaMission.StaticCharaMission> staticCharaMissionMap;

	// Token: 0x040004E9 RID: 1257
	private Dictionary<int, DataManagerCharaMission.DynamicCharaMission> dynamicCharaMissionMap;

	// Token: 0x02000663 RID: 1635
	public class StaticMission
	{
		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06003147 RID: 12615 RVA: 0x001BD0C0 File Offset: 0x001BB2C0
		// (set) Token: 0x06003148 RID: 12616 RVA: 0x001BD0C8 File Offset: 0x001BB2C8
		public int MissionId { get; private set; }

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06003149 RID: 12617 RVA: 0x001BD0D1 File Offset: 0x001BB2D1
		// (set) Token: 0x0600314A RID: 12618 RVA: 0x001BD0D9 File Offset: 0x001BB2D9
		public int NeedMissionId { get; private set; }

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x0600314B RID: 12619 RVA: 0x001BD0E2 File Offset: 0x001BB2E2
		// (set) Token: 0x0600314C RID: 12620 RVA: 0x001BD0EA File Offset: 0x001BB2EA
		public int CharaId { get; private set; }

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x0600314D RID: 12621 RVA: 0x001BD0F3 File Offset: 0x001BB2F3
		// (set) Token: 0x0600314E RID: 12622 RVA: 0x001BD0FB File Offset: 0x001BB2FB
		public string MissionContents { get; private set; }

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x0600314F RID: 12623 RVA: 0x001BD104 File Offset: 0x001BB304
		// (set) Token: 0x06003150 RID: 12624 RVA: 0x001BD10C File Offset: 0x001BB30C
		public int SortNum { get; private set; }

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06003151 RID: 12625 RVA: 0x001BD115 File Offset: 0x001BB315
		// (set) Token: 0x06003152 RID: 12626 RVA: 0x001BD11D File Offset: 0x001BB31D
		public int Denominator { get; private set; }

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06003153 RID: 12627 RVA: 0x001BD126 File Offset: 0x001BB326
		// (set) Token: 0x06003154 RID: 12628 RVA: 0x001BD12E File Offset: 0x001BB32E
		public int RewardItemId { get; private set; }

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06003155 RID: 12629 RVA: 0x001BD137 File Offset: 0x001BB337
		// (set) Token: 0x06003156 RID: 12630 RVA: 0x001BD13F File Offset: 0x001BB33F
		public int RewardItemNum { get; private set; }

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06003157 RID: 12631 RVA: 0x001BD148 File Offset: 0x001BB348
		// (set) Token: 0x06003158 RID: 12632 RVA: 0x001BD150 File Offset: 0x001BB350
		public bool AlwaysDispFlg { get; private set; }

		// Token: 0x06003159 RID: 12633 RVA: 0x001BD15C File Offset: 0x001BB35C
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

	// Token: 0x02000664 RID: 1636
	public class StaticCharaMission
	{
		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x0600315A RID: 12634 RVA: 0x001BD1DE File Offset: 0x001BB3DE
		// (set) Token: 0x0600315B RID: 12635 RVA: 0x001BD1E6 File Offset: 0x001BB3E6
		public int CharaId { get; private set; }

		// Token: 0x0600315C RID: 12636 RVA: 0x001BD1EF File Offset: 0x001BB3EF
		public StaticCharaMission(int chId)
		{
			this.CharaId = chId;
			this.MissionList = new List<DataManagerCharaMission.StaticMission>();
		}

		// Token: 0x04002ED5 RID: 11989
		public List<DataManagerCharaMission.StaticMission> MissionList;
	}

	// Token: 0x02000665 RID: 1637
	public class DynamicCharaMission
	{
		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x0600315D RID: 12637 RVA: 0x001BD209 File Offset: 0x001BB409
		// (set) Token: 0x0600315E RID: 12638 RVA: 0x001BD211 File Offset: 0x001BB411
		public int CharaId { get; private set; }

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x0600315F RID: 12639 RVA: 0x001BD21A File Offset: 0x001BB41A
		// (set) Token: 0x06003160 RID: 12640 RVA: 0x001BD222 File Offset: 0x001BB422
		public Dictionary<int, DataManagerCharaMission.DynamicCharaMission.MissionOne> MissionMap { get; set; }

		// Token: 0x06003161 RID: 12641 RVA: 0x001BD22B File Offset: 0x001BB42B
		public DynamicCharaMission(int chId)
		{
			this.CharaId = chId;
			this.MissionMap = new Dictionary<int, DataManagerCharaMission.DynamicCharaMission.MissionOne>();
		}

		// Token: 0x02001111 RID: 4369
		public class MissionOne
		{
			// Token: 0x17000C20 RID: 3104
			// (get) Token: 0x0600546C RID: 21612 RVA: 0x0024DBD7 File Offset: 0x0024BDD7
			// (set) Token: 0x0600546D RID: 21613 RVA: 0x0024DBDF File Offset: 0x0024BDDF
			public int MissionId { get; private set; }

			// Token: 0x17000C21 RID: 3105
			// (get) Token: 0x0600546E RID: 21614 RVA: 0x0024DBE8 File Offset: 0x0024BDE8
			// (set) Token: 0x0600546F RID: 21615 RVA: 0x0024DBF0 File Offset: 0x0024BDF0
			private int Denominator { get; set; }

			// Token: 0x17000C22 RID: 3106
			// (get) Token: 0x06005470 RID: 21616 RVA: 0x0024DBF9 File Offset: 0x0024BDF9
			// (set) Token: 0x06005471 RID: 21617 RVA: 0x0024DC01 File Offset: 0x0024BE01
			public int Numerator { get; private set; }

			// Token: 0x17000C23 RID: 3107
			// (get) Token: 0x06005472 RID: 21618 RVA: 0x0024DC0A File Offset: 0x0024BE0A
			// (set) Token: 0x06005473 RID: 21619 RVA: 0x0024DC12 File Offset: 0x0024BE12
			public bool Received { get; set; }

			// Token: 0x17000C24 RID: 3108
			// (get) Token: 0x06005474 RID: 21620 RVA: 0x0024DC1B File Offset: 0x0024BE1B
			// (set) Token: 0x06005475 RID: 21621 RVA: 0x0024DC23 File Offset: 0x0024BE23
			public long KeyServerTime { get; private set; }

			// Token: 0x17000C25 RID: 3109
			// (get) Token: 0x06005476 RID: 21622 RVA: 0x0024DC2C File Offset: 0x0024BE2C
			public bool CanReceive
			{
				get
				{
					return !this.Received && this.Denominator <= this.Numerator;
				}
			}

			// Token: 0x17000C26 RID: 3110
			// (get) Token: 0x06005477 RID: 21623 RVA: 0x0024DC49 File Offset: 0x0024BE49
			// (set) Token: 0x06005478 RID: 21624 RVA: 0x0024DC51 File Offset: 0x0024BE51
			public int SortNum { get; private set; }

			// Token: 0x06005479 RID: 21625 RVA: 0x0024DC5C File Offset: 0x0024BE5C
			public MissionOne(Mission mission, DataManagerCharaMission.StaticMission staticMission)
			{
				this.MissionId = mission.mission_id;
				this.Numerator = mission.mission_status;
				this.Received = 1 == mission.accept_flg;
				this.KeyServerTime = mission.mission_datetime;
				this.Denominator = staticMission.Denominator;
				this.SortNum = staticMission.SortNum;
			}

			// Token: 0x0600547A RID: 21626 RVA: 0x0024DCBA File Offset: 0x0024BEBA
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
