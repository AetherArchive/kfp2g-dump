using System;
using UnityEngine;

namespace SGNFW.Common.NativePlugin
{
	public class ScreenInfo
	{
		public static int GetRenderScreenWidth()
		{
			return Screen.width;
		}

		public static int GetRenderScreenHeight()
		{
			return Screen.height;
		}
	}
}
