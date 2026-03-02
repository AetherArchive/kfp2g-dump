using System;
using UnityEngine;

namespace SGNFW.Common
{
	// Token: 0x02000255 RID: 597
	public static class CameraExtensions
	{
		// Token: 0x06002561 RID: 9569 RVA: 0x0019F4D4 File Offset: 0x0019D6D4
		public static void SetCullingMask(this Camera self, string layerName, bool enable)
		{
			int num = LayerMask.NameToLayer(layerName);
			if (num < 0 || 31 < num)
			{
				return;
			}
			int num2 = 1 << num;
			self.cullingMask = (self.cullingMask & ~num2) | (enable ? num2 : 0);
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x0019F510 File Offset: 0x0019D710
		public static bool IsCullingMaskEnable(this Camera self, string layerName)
		{
			int num = LayerMask.NameToLayer(layerName);
			if (num < 0 || 31 < num)
			{
				return false;
			}
			int num2 = 1 << num;
			return (self.cullingMask & num2) != 0;
		}
	}
}
