using System;
using SGNFW.HttpRequest.Protocol;

// Token: 0x020000D0 RID: 208
[Serializable]
public class PhotoDynamicData
{
	// Token: 0x170001DF RID: 479
	// (get) Token: 0x06000950 RID: 2384 RVA: 0x0003A6B6 File Offset: 0x000388B6
	// (set) Token: 0x06000951 RID: 2385 RVA: 0x0003A6BE File Offset: 0x000388BE
	public PhotoDynamicData.PhotoOwnerType OwnerType { get; set; }

	// Token: 0x06000952 RID: 2386 RVA: 0x0003A6C8 File Offset: 0x000388C8
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

	// Token: 0x06000953 RID: 2387 RVA: 0x0003A760 File Offset: 0x00038960
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

	// Token: 0x04000792 RID: 1938
	public long dataId;

	// Token: 0x04000793 RID: 1939
	public int level;

	// Token: 0x04000794 RID: 1940
	public long exp;

	// Token: 0x04000795 RID: 1941
	public int levelRank;

	// Token: 0x04000796 RID: 1942
	public bool lockFlag;

	// Token: 0x04000797 RID: 1943
	public bool imgRevertFlag;

	// Token: 0x04000798 RID: 1944
	public bool favoriteFlag;

	// Token: 0x04000799 RID: 1945
	public DateTime insertTime;

	// Token: 0x0400079A RID: 1946
	public int photoId;

	// Token: 0x020007C5 RID: 1989
	public enum PhotoOwnerType
	{
		// Token: 0x0400349F RID: 13471
		Undefined,
		// Token: 0x040034A0 RID: 13472
		User,
		// Token: 0x040034A1 RID: 13473
		Helper
	}
}
