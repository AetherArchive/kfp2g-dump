using System;
using SGNFW.HttpRequest.Protocol;
using UnityEngine;

public class TreeHouseFurnitureMapping
{
	public TreeHouseFurnitureMapping()
	{
	}

	public TreeHouseFurnitureMapping(MasterRoomFurniture serverData)
	{
		this.placementId = serverData.index;
		this.furnitureId = serverData.furniture_id;
		this.postion = new Vector3Int(serverData.position_x, serverData.position_y, serverData.position_z);
		this.angle = serverData.angle;
		this.effectFlag = serverData.effect_flg == 1;
	}

	public MasterRoomFurniture ConvertServerData()
	{
		return new MasterRoomFurniture
		{
			index = this.placementId,
			furniture_id = this.furnitureId,
			position_x = this.postion.x,
			position_y = this.postion.y,
			position_z = this.postion.z,
			angle = this.angle,
			effect_flg = (this.effectFlag ? 1 : 0)
		};
	}

	public int placementId;

	public int furnitureId;

	public Vector3Int postion;

	public int angle;

	public bool effectFlag;
}
