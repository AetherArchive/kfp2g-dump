using System;
using System.Collections.Generic;
using System.Diagnostics;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.UI;

public class DebugScrollSensitivity : Singleton<DebugScrollSensitivity>
{
	private void Start()
	{
		if (this.scrollInfoList == null)
		{
			this.scrollInfoList = new List<DebugScrollSensitivity.ScrollInfo>();
		}
	}

	[Conditional("UNITY_EDITOR")]
	public static void ResistScroll(string name, ScrollRect scroll)
	{
		Singleton<DebugScrollSensitivity>.Instance.scrollInfoList.Add(new DebugScrollSensitivity.ScrollInfo(name, scroll));
	}

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

	[SerializeField]
	private List<DebugScrollSensitivity.ScrollInfo> scrollInfoList;

	[Serializable]
	public class ScrollInfo
	{
		public ScrollInfo(string name, ScrollRect scroll)
		{
			this.placeName = name;
			this.scrollRect = scroll;
			this.sensitivity = scroll.scrollSensitivity;
		}

		public string placeName;

		public ScrollRect scrollRect;

		public float sensitivity;
	}
}
