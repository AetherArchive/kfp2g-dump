using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class GachaRateViewResponse : Response
	{
		public RarityViewResult gacha_rarity_result;

		public CharaViewResult gacha_charas_result;

		public PhotoViewResult gacha_photos_result;

		public ItemViewResult gacha_item_result;

		public MasterRoomFurnitureViewResult gacha_master_room_furniture_result;
	}
}
