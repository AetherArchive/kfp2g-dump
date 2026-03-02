using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.HttpRequest.Protocol;

// Token: 0x02000085 RID: 133
public class HomeCheckResult
{
	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06000512 RID: 1298 RVA: 0x00023343 File Offset: 0x00021543
	// (set) Token: 0x06000513 RID: 1299 RVA: 0x0002334B File Offset: 0x0002154B
	public Sealed sealedData { get; private set; } = new Sealed();

	// Token: 0x06000514 RID: 1300 RVA: 0x00023354 File Offset: 0x00021554
	public HomeCheckResult(HomeCheckResponse res)
	{
		this.usedHelperNum = res.friend_use_num;
		this.usedHelperPoint = res.friend_point;
		this.sealedData = ((res.sealed_line != null) ? res.sealed_line : new Sealed());
		this.treeHouseBadgeFlag = res.master_room_stamp_flg != 0;
		this.SetTreeHouseCharge(res.master_room_machine_list, res.server_time);
		if (res.bonuses != null)
		{
			this.loginBonusList = res.bonuses.ConvertAll<HomeCheckResult.LoginBonus>((Bonus item) => new HomeCheckResult.LoginBonus
			{
				id = item.bonus_id,
				day = item.days,
				isReceive = (item.receive == 1)
			});
		}
		if (res.roulette_data != null)
		{
			this.rouletteData = new HomeCheckResult.RouletteData
			{
				rouletteId = res.roulette_data.roulette_id,
				targetGachaId = res.roulette_data.target_gacha_id,
				remainingDrawCount = res.roulette_data.remaining_draw_count,
				actionId = res.roulette_data.action_id,
				createdAt = res.roulette_data.created_at,
				assistantCharaId = res.roulette_data.assistant_chara_id,
				bgTexturePath = res.roulette_data.bg_texture_path,
				startText = res.roulette_data.start_text,
				endText = res.roulette_data.end_text,
				performanceId = res.roulette_data.performance_id,
				rouletteModelId = res.roulette_data.roulette_model_id,
				texturePath = res.roulette_data.texture_path
			};
		}
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x000234EB File Offset: 0x000216EB
	public HomeCheckResult()
	{
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x00023509 File Offset: 0x00021709
	public bool IsTreeHouseCharge()
	{
		return this.GetTreeHouseChargeMinTime() <= 0;
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x00023517 File Offset: 0x00021717
	public bool IsTreeHouseBatteryCharge()
	{
		return this.GetTreeHouseBatteryChargeTime() <= 0;
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x00023528 File Offset: 0x00021728
	public void SetTreeHouseCharge(List<MasterRoomMachineDataModel> tim_list, long server_time)
	{
		this.OriginTreeHouseChargeTimeList = tim_list;
		this.TreeHouseChargeTimeList = this.OriginTreeHouseChargeTimeList.ConvertAll<MasterRoomMachineDataModel>((MasterRoomMachineDataModel x) => new MasterRoomMachineDataModel(x)).ToList<MasterRoomMachineDataModel>();
		this.treeHouseChargeCheck = new DateTime(PrjUtil.ConvertTimeToTicks(server_time));
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x00023584 File Offset: 0x00021784
	public int GetTreeHouseChargeMinTime()
	{
		TimeSpan timeSpan = TimeManager.Now - this.treeHouseChargeCheck;
		int num = 999;
		foreach (MasterRoomMachineDataModel masterRoomMachineDataModel in this.OriginTreeHouseChargeTimeList)
		{
			int num2 = masterRoomMachineDataModel.nextsecond - (int)timeSpan.TotalSeconds;
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num2 < num)
			{
				num = num2;
			}
		}
		return num;
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00023604 File Offset: 0x00021804
	public int GetTreeHouseBatteryChargeTime()
	{
		TimeSpan timeSpan = TimeManager.Now - this.treeHouseChargeCheck;
		int num = this.OriginTreeHouseChargeTimeList.Find((MasterRoomMachineDataModel item) => item.machineId == DataManager.DmTreeHouse.GetChargeBatteryData().id).nextsecond - (int)timeSpan.TotalSeconds;
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x00023664 File Offset: 0x00021864
	public List<MasterRoomMachineDataModel> GetTreeHouseMachineFntrTimeList()
	{
		TimeSpan timeSpan = TimeManager.Now - this.treeHouseChargeCheck;
		for (int i = 0; i < this.OriginTreeHouseChargeTimeList.Count; i++)
		{
			int num = this.OriginTreeHouseChargeTimeList[i].nextsecond - (int)timeSpan.TotalSeconds;
			if (num < 0)
			{
				num = 0;
			}
			this.TreeHouseChargeTimeList[i].nextsecond = num;
		}
		return this.TreeHouseChargeTimeList.FindAll((MasterRoomMachineDataModel x) => x.machineId != DataManager.DmTreeHouse.GetChargeBatteryData().id);
	}

	// Token: 0x04000559 RID: 1369
	public int presentBoxNum;

	// Token: 0x0400055A RID: 1370
	public int usedHelperNum;

	// Token: 0x0400055B RID: 1371
	public int usedHelperPoint;

	// Token: 0x0400055C RID: 1372
	public bool treeHouseBadgeFlag;

	// Token: 0x0400055D RID: 1373
	public List<MasterRoomMachineDataModel> OriginTreeHouseChargeTimeList;

	// Token: 0x0400055E RID: 1374
	public List<MasterRoomMachineDataModel> TreeHouseChargeTimeList;

	// Token: 0x0400055F RID: 1375
	private DateTime treeHouseChargeCheck;

	// Token: 0x04000560 RID: 1376
	public List<HomeCheckResult.LoginBonus> loginBonusList = new List<HomeCheckResult.LoginBonus>();

	// Token: 0x04000561 RID: 1377
	public HomeCheckResult.RouletteData rouletteData;

	// Token: 0x020006C6 RID: 1734
	public class LoginBonus
	{
		// Token: 0x04003073 RID: 12403
		public int id;

		// Token: 0x04003074 RID: 12404
		public int day;

		// Token: 0x04003075 RID: 12405
		public bool isReceive;
	}

	// Token: 0x020006C7 RID: 1735
	public class RouletteData
	{
		// Token: 0x04003076 RID: 12406
		public int rouletteId;

		// Token: 0x04003077 RID: 12407
		public int targetGachaId;

		// Token: 0x04003078 RID: 12408
		public int remainingDrawCount;

		// Token: 0x04003079 RID: 12409
		public string actionId;

		// Token: 0x0400307A RID: 12410
		public long createdAt;

		// Token: 0x0400307B RID: 12411
		public int assistantCharaId;

		// Token: 0x0400307C RID: 12412
		public string bgTexturePath;

		// Token: 0x0400307D RID: 12413
		public string startText;

		// Token: 0x0400307E RID: 12414
		public string endText;

		// Token: 0x0400307F RID: 12415
		public string performanceId;

		// Token: 0x04003080 RID: 12416
		public int rouletteModelId;

		// Token: 0x04003081 RID: 12417
		public string texturePath;
	}
}
