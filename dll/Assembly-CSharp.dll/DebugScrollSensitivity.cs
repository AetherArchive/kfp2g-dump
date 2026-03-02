using System;
using System.Collections.Generic;
using System.Diagnostics;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000035 RID: 53
public class DebugScrollSensitivity : Singleton<DebugScrollSensitivity>
{
	// Token: 0x060000C5 RID: 197 RVA: 0x0000693C File Offset: 0x00004B3C
	private void Start()
	{
		if (this.scrollInfoList == null)
		{
			this.scrollInfoList = new List<DebugScrollSensitivity.ScrollInfo>();
		}
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00006951 File Offset: 0x00004B51
	[Conditional("UNITY_EDITOR")]
	public static void ResistScroll(string name, ScrollRect scroll)
	{
		Singleton<DebugScrollSensitivity>.Instance.scrollInfoList.Add(new DebugScrollSensitivity.ScrollInfo(name, scroll));
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x0000696C File Offset: 0x00004B6C
	[Conditional("UNITY_EDITOR")]
	private void OnValidate()
	{
		foreach (DebugScrollSensitivity.ScrollInfo scrollInfo in this.scrollInfoList)
		{
			if (null != scrollInfo.scrollRect)
			{
				scrollInfo.scrollRect.scrollSensitivity = scrollInfo.sensitivity;
			}
		}
	}

	// Token: 0x04000132 RID: 306
	[SerializeField]
	private List<DebugScrollSensitivity.ScrollInfo> scrollInfoList;

	// Token: 0x0200059E RID: 1438
	[Serializable]
	public class ScrollInfo
	{
		// Token: 0x06002F04 RID: 12036 RVA: 0x001B48AD File Offset: 0x001B2AAD
		public ScrollInfo(string name, ScrollRect scroll)
		{
			this.placeName = name;
			this.scrollRect = scroll;
			this.sensitivity = scroll.scrollSensitivity;
		}

		// Token: 0x04002989 RID: 10633
		public string placeName;

		// Token: 0x0400298A RID: 10634
		public ScrollRect scrollRect;

		// Token: 0x0400298B RID: 10635
		public float sensitivity;
	}
}
