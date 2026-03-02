using System;
using UnityEngine;

namespace SGNFW.uGUI
{
	// Token: 0x02000232 RID: 562
	public class ScreenAdjuster : MonoBehaviour
	{
		// Token: 0x06002378 RID: 9080 RVA: 0x001987E4 File Offset: 0x001969E4
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

		// Token: 0x06002379 RID: 9081 RVA: 0x00198834 File Offset: 0x00196A34
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

		// Token: 0x0600237A RID: 9082 RVA: 0x001988EC File Offset: 0x00196AEC
		private void Update()
		{
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x001988EE File Offset: 0x00196AEE
		private void OnDestroy()
		{
		}

		// Token: 0x04001AC9 RID: 6857
		[SerializeField]
		private Canvas canvas;

		// Token: 0x04001ACA RID: 6858
		[SerializeField]
		private float baseWidth = 640f;

		// Token: 0x04001ACB RID: 6859
		[SerializeField]
		private float baseHeight = 1136f;

		// Token: 0x04001ACC RID: 6860
		private RectTransform rtrs;
	}
}
