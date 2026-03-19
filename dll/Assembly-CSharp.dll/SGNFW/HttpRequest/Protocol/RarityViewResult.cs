using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class RarityViewResult
	{
		public RarityCharas rarity_charas;

		public RarityPhotos rarity_photos;

		public RarityItems rarity_items;

		public RarityMasterRoomFurnitures rarity_master_room_furnitures;

		public RarityStickers rarity_stickers;
	}
}
