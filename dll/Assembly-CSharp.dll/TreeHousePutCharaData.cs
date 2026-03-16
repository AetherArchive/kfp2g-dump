using System;
using SGNFW.HttpRequest.Protocol;

public class TreeHousePutCharaData
{
	public TreeHousePutCharaData()
	{
	}

	public TreeHousePutCharaData(MasterRoomChara serverData)
	{
		this.indexId = serverData.index;
		this.charaId = serverData.chara_id;
		this.clothId = serverData.cloth_id;
		this.accessoryId = serverData.accessory_id;
	}

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

	public int indexId;

	public int charaId;

	public int clothId;

	public int accessoryId;
}
