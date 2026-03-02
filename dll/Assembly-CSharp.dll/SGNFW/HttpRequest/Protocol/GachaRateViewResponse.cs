using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003DB RID: 987
	public class GachaRateViewResponse : Response
	{
		// Token: 0x040024CE RID: 9422
		public RarityViewResult gacha_rarity_result;

		// Token: 0x040024CF RID: 9423
		public CharaViewResult gacha_charas_result;

		// Token: 0x040024D0 RID: 9424
		public PhotoViewResult gacha_photos_result;

		// Token: 0x040024D1 RID: 9425
		public ItemViewResult gacha_item_result;

		// Token: 0x040024D2 RID: 9426
		public MasterRoomFurnitureViewResult gacha_master_room_furniture_result;
	}
}
