using System;
using SGNFW.Mst;

public class AdvertiseBannerData
{
	public AdvertiseBannerData(MstAdvertiseBannerData mst)
	{
		this.bannerId = mst.id;
		this.platform = mst.platform;
		this.bannerImagePath = "Texture2D/AdvertiseBanner/" + mst.bannerName;
		this.actionParamURL = mst.linkAddress;
		this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.startTime));
		this.endTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.endTime));
	}

	public int bannerId;

	public int platform;

	public string bannerImagePath;

	public string actionParamURL;

	public DateTime startTime;

	public DateTime endTime;
}
