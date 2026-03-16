using System;
using UnityEngine;

namespace SGNFW.uGUI
{
	public class Initializer : MonoBehaviour
	{
		private void Awake()
		{
			Manager.Terminate();
			Manager.Initialize(base.transform, this.cam);
			TextManager.Terminate();
			TextManager.Initialize(this.iconArray, this.colorArray);
			Object.Destroy(this);
		}

		[SerializeField]
		private Camera cam;

		[SerializeField]
		private Icon[] iconArray;

		[SerializeField]
		private TextManager.Color[] colorArray;
	}
}
