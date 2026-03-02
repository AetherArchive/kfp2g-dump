using System;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200051C RID: 1308
	[Serializable]
	public class RouletteData
	{
		// Token: 0x040027BD RID: 10173
		public int roulette_id;

		// Token: 0x040027BE RID: 10174
		public int target_gacha_id;

		// Token: 0x040027BF RID: 10175
		public int remaining_draw_count;

		// Token: 0x040027C0 RID: 10176
		public string action_id;

		// Token: 0x040027C1 RID: 10177
		public long created_at;

		// Token: 0x040027C2 RID: 10178
		public int assistant_chara_id;

		// Token: 0x040027C3 RID: 10179
		public string bg_texture_path;

		// Token: 0x040027C4 RID: 10180
		public string start_text;

		// Token: 0x040027C5 RID: 10181
		public string end_text;

		// Token: 0x040027C6 RID: 10182
		public string performance_id;

		// Token: 0x040027C7 RID: 10183
		public int roulette_model_id;

		// Token: 0x040027C8 RID: 10184
		public string texture_path;
	}
}
