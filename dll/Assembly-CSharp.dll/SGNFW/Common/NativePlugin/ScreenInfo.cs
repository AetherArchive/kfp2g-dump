using System;
using UnityEngine;

namespace SGNFW.Common.NativePlugin
{
	// Token: 0x02000270 RID: 624
	public class ScreenInfo
	{
		// Token: 0x06002640 RID: 9792 RVA: 0x001A14B5 File Offset: 0x0019F6B5
		public static int GetRenderScreenWidth()
		{
			return Screen.width;
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x001A14BC File Offset: 0x0019F6BC
		public static int GetRenderScreenHeight()
		{
			return Screen.height;
		}
	}
}
