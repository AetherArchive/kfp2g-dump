using System;
using UnityEngine;

namespace SGNFW.Common.NativePlugin
{
	// Token: 0x0200026A RID: 618
	public class Clipboard
	{
		// Token: 0x06002621 RID: 9761 RVA: 0x001A126F File Offset: 0x0019F46F
		public static void SetTextToClipboard(string str)
		{
			GUIUtility.systemCopyBuffer = str;
		}
	}
}
