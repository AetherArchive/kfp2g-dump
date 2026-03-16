using System;
using SGNFW.HttpRequest.Protocol;

public class TreeHouseMyset
{
	public TreeHouseMyset()
	{
	}

	public TreeHouseMyset(int id, MasterRoomMyset serverData)
	{
		if (serverData == null)
		{
			this.mysetId = id;
			this.name = "未設定";
			this.isDataEnable = false;
			return;
		}
		this.mysetId = serverData.myset_id;
		this.name = serverData.name;
		this.saveTime = new DateTime(PrjUtil.ConvertTimeToTicks(serverData.update_time));
		this.isDataEnable = true;
	}

	public int mysetId;

	public string name;

	public DateTime saveTime;

	public bool isDataEnable;
}
