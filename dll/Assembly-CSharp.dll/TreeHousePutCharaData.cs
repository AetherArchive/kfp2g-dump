using System;
using SGNFW.HttpRequest.Protocol;

// Token: 0x020000B8 RID: 184
public class TreeHousePutCharaData
{
	// Token: 0x060007FD RID: 2045 RVA: 0x00035D5E File Offset: 0x00033F5E
	public TreeHousePutCharaData()
	{
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x00035D66 File Offset: 0x00033F66
	public TreeHousePutCharaData(MasterRoomChara serverData)
	{
		this.indexId = serverData.index;
		this.charaId = serverData.chara_id;
		this.clothId = serverData.cloth_id;
		this.accessoryId = serverData.accessory_id;
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x00035D9E File Offset: 0x00033F9E
	public MasterRoomChara ConvertServerData()
	{
		return new MasterRoomChara
		{
			index = this.indexId,
			chara_id = this.charaId,
			cloth_id = this.clothId,
			accessory_id = this.accessoryId
		};
	}

	// Token: 0x040006F3 RID: 1779
	public int indexId;

	// Token: 0x040006F4 RID: 1780
	public int charaId;

	// Token: 0x040006F5 RID: 1781
	public int clothId;

	// Token: 0x040006F6 RID: 1782
	public int accessoryId;
}
