using System;
using UnityEngine;

namespace SGNFW.Common
{
	public static class CameraExtensions
	{
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
