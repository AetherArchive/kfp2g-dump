using System;
using SGNFW.HttpRequest.Protocol;
using UnityEngine;

// Token: 0x020000B9 RID: 185
public class TreeHouseFurnitureMapping
{
	// Token: 0x06000800 RID: 2048 RVA: 0x00035DD5 File Offset: 0x00033FD5
	public TreeHouseFurnitureMapping()
	{
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x00035DE0 File Offset: 0x00033FE0
	public TreeHouseFurnitureMapping(MasterRoomFurniture serverData)
	{
		this.placementId = serverData.index;
		this.furnitureId = serverData.furniture_id;
		this.postion = new Vector3Int(serverData.position_x, serverData.position_y, serverData.position_z);
		this.angle = serverData.angle;
		this.effectFlag = serverData.effect_flg == 1;
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x00035E44 File Offset: 0x00034044
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

	// Token: 0x040006F7 RID: 1783
	public int placementId;

	// Token: 0x040006F8 RID: 1784
	public int furnitureId;

	// Token: 0x040006F9 RID: 1785
	public Vector3Int postion;

	// Token: 0x040006FA RID: 1786
	public int angle;

	// Token: 0x040006FB RID: 1787
	public bool effectFlag;
}
