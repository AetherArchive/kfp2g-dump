using System;
using UnityEngine;

namespace SGNFW.uGUI
{
	public class ScreenAdjuster : MonoBehaviour
	{
		private void Awake()
		{
			if (this.canvas == null)
			{
				this.canvas = base.GetComponentInParent<Canvas>();
			}
			if (this.canvas == null)
			{
				Object.Destroy(this);
				return;
			}
			this.rtrs = this.canvas.GetComponent<RectTransform>();
		}

		private void Start()
		{
			float num = this.baseWidth / this.baseHeight;
			float num2 = this.rtrs.rect.width / this.rtrs.rect.height;
			float num3;
			if (num < num2)
			{
				num3 = this.rtrs.rect.width / this.baseWidth;
			}
			else
			{
				num3 = this.rtrs.rect.height / this.baseHeight;
			}
			Vector3 localScale = base.transform.localScale;
			localScale = new Vector3(localScale.x * num3, localScale.y * num3, localScale.z);
			base.transform.localScale = localScale;
		}

		private void Update()
		{
		}

		private void OnDestroy()
		{
		}

		[SerializeField]
		private Canvas canvas;

		[SerializeField]
		private float baseWidth = 640f;

		[SerializeField]
		private float baseHeight = 1136f;

		private RectTransform rtrs;
	}
}
