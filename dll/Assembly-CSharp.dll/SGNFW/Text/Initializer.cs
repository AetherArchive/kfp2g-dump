using System;
using UnityEngine;

namespace SGNFW.Text
{
	public class Initializer : MonoBehaviour
	{
		private void Start()
		{
			Manager.Terminate();
			Manager.Initialize(string.Join("|", this.customTag));
			if (this.initialText)
			{
				Manager.SetJson(this.initialText.text);
			}
			Object.Destroy(base.gameObject);
		}

		[SerializeField]
		private TextAsset initialText;

		[SerializeField]
		private string[] customTag;
	}
}
