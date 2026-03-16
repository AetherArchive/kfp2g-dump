using System;
using UnityEngine;

namespace SGNFW.Common.NativePlugin
{
	public class Clipboard
	{
		public static void SetTextToClipboard(string str)
		{
			GUIUtility.systemCopyBuffer = str;
		}
	}
}
