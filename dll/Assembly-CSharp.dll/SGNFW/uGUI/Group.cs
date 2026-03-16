using System;
using UnityEngine;

namespace SGNFW.uGUI
{
	public class Group : MonoBehaviour
	{
		private void Awake()
		{
			base.enabled = false;
		}

		private void OnDestroy()
		{
			Manager.RemoveGroupUI(this);
		}

		public string label;

		public Layer layer;

		public int order = int.MinValue;
	}
}
