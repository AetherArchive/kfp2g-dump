using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class RouletteData
	{
		public int roulette_id;

		public int target_gacha_id;

		public int remaining_draw_count;

		public string action_id;

		public long created_at;

		public int assistant_chara_id;

		public string bg_texture_path;

		public string start_text;

		public string end_text;

		public string performance_id;

		public int roulette_model_id;

		public string texture_path;
	}
}
