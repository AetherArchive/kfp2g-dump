using System;
using SGNFW.Mst;

// Token: 0x020000A5 RID: 165
public class AdvertiseBannerData
{
	// Token: 0x0600074E RID: 1870 RVA: 0x00032518 File Offset: 0x00030718
	public AdvertiseBannerData(MstAdvertiseBannerData mst)
	{
		this.bannerId = mst.id;
		this.platform = mst.platform;
		this.bannerImagePath = "Texture2D/AdvertiseBanner/" + mst.bannerName;
		this.actionParamURL = mst.linkAddress;
		this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.startTime));
		this.endTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.endTime));
	}

	// Token: 0x04000665 RID: 1637
	public int bannerId;

	// Token: 0x04000666 RID: 1638
	public int platform;

	// Token: 0x04000667 RID: 1639
	public string bannerImagePath;

	// Token: 0x04000668 RID: 1640
	public string actionParamURL;

	// Token: 0x04000669 RID: 1641
	public DateTime startTime;

	// Token: 0x0400066A RID: 1642
	public DateTime endTime;
}
