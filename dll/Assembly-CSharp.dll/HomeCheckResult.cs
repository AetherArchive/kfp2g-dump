using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.HttpRequest.Protocol;

public class HomeCheckResult
{
	public Sealed sealedData { get; private set; } = new Sealed();

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

	public HomeCheckResult()
	{
	}

	public bool IsTreeHouseCharge()
	{
		return this.GetTreeHouseChargeMinTime() <= 0;
	}

	public bool IsTreeHouseBatteryCharge()
	{
		return this.GetTreeHouseBatteryChargeTime() <= 0;
	}

	public void SetTreeHouseCharge(List<MasterRoomMachineDataModel> tim_list, long server_time)
	{
		this.OriginTreeHouseChargeTimeList = tim_list;
		this.TreeHouseChargeTimeList = this.OriginTreeHouseChargeTimeList.ConvertAll<MasterRoomMachineDataModel>((MasterRoomMachineDataModel x) => new MasterRoomMachineDataModel(x)).ToList<MasterRoomMachineDataModel>();
		this.treeHouseChargeCheck = new DateTime(PrjUtil.ConvertTimeToTicks(server_time));
	}

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

	public int presentBoxNum;

	public int usedHelperNum;

	public int usedHelperPoint;

	public bool treeHouseBadgeFlag;

	public List<MasterRoomMachineDataModel> OriginTreeHouseChargeTimeList;

	public List<MasterRoomMachineDataModel> TreeHouseChargeTimeList;

	private DateTime treeHouseChargeCheck;

	public List<HomeCheckResult.LoginBonus> loginBonusList = new List<HomeCheckResult.LoginBonus>();

	public HomeCheckResult.RouletteData rouletteData;

	public class LoginBonus
	{
		public int id;

		public int day;

		public bool isReceive;
	}

	public class RouletteData
	{
		public int rouletteId;

		public int targetGachaId;

		public int remainingDrawCount;

		public string actionId;

		public long createdAt;

		public int assistantCharaId;

		public string bgTexturePath;

		public string startText;

		public string endText;

		public string performanceId;

		public int rouletteModelId;

		public string texturePath;
	}
}
