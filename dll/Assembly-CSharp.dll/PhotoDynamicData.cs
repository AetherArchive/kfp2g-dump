using System;
using SGNFW.HttpRequest.Protocol;

[Serializable]
public class PhotoDynamicData
{
	public PhotoDynamicData.PhotoOwnerType OwnerType { get; set; }

	public void UpdateByServer(Photo photo)
	{
		this.dataId = photo.photo_id;
		this.photoId = photo.item_id;
		this.level = photo.level;
		this.exp = PhotoDynamicData.ConvertExp(this.photoId, photo.exp);
		this.levelRank = photo.limit_over_num;
		this.lockFlag = photo.lock_flg != 0;
		this.imgRevertFlag = photo.img_flg != 0;
		this.favoriteFlag = photo.favorite_flg != 0;
		this.insertTime = new DateTime(PrjUtil.ConvertTimeToTicks(photo.insert_time));
	}

	private static long ConvertExp(int photoId, long exp)
	{
		long num = exp;
		PhotoStaticData photoStaticData = DataManager.DmPhoto.GetPhotoStaticData(photoId);
		int? num2;
		if (photoStaticData == null)
		{
			num2 = null;
		}
		else
		{
			PhotoStaticBase baseData = photoStaticData.baseData;
			num2 = ((baseData != null) ? new int?(baseData.levelTableId) : null);
		}
		int? num3 = num2;
		bool flag = num3 != null;
		int num4 = ((num3 != null) ? num3.Value : 1);
		int num5 = 1;
		while (num5 < DataManager.DmServerMst.gameLevelInfoList.Count && DataManager.DmServerMst.gameLevelInfoList[num5].photoLevelExp.ContainsKey(num4))
		{
			long num6 = num - DataManager.DmServerMst.gameLevelInfoList[num5].photoLevelExp[num4];
			if (num6 < 0L || num6 == num)
			{
				break;
			}
			num = num6;
			num5++;
		}
		return num;
	}

	public long dataId;

	public int level;

	public long exp;

	public int levelRank;

	public bool lockFlag;

	public bool imgRevertFlag;

	public bool favoriteFlag;

	public DateTime insertTime;

	public int photoId;

	public enum PhotoOwnerType
	{
		Undefined,
		User,
		Helper
	}
}
