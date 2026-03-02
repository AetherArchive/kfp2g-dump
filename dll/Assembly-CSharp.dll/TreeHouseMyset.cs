using System;
using SGNFW.HttpRequest.Protocol;

// Token: 0x020000B6 RID: 182
public class TreeHouseMyset
{
	// Token: 0x060007F9 RID: 2041 RVA: 0x00035C7A File Offset: 0x00033E7A
	public TreeHouseMyset()
	{
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x00035C84 File Offset: 0x00033E84
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

	// Token: 0x040006ED RID: 1773
	public int mysetId;

	// Token: 0x040006EE RID: 1774
	public string name;

	// Token: 0x040006EF RID: 1775
	public DateTime saveTime;

	// Token: 0x040006F0 RID: 1776
	public bool isDataEnable;
}
