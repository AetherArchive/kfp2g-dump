using System;
using UnityEngine;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	// Token: 0x0200022C RID: 556
	[AddComponentMenu("UI/Effects/Italic")]
	[RequireComponent(typeof(Graphic))]
	public class Italic : BaseMeshEffect
	{
		// Token: 0x06002330 RID: 9008 RVA: 0x00196570 File Offset: 0x00194770
		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.IsActive() || vh.currentVertCount <= 0)
			{
				return;
			}
			Italic.TransformItalic transformItalic;
			this.GetTransformItalic(out transformItalic);
			UIVertex uivertex = default(UIVertex);
			for (int i = 0; i < vh.currentVertCount; i++)
			{
				vh.PopulateUIVertex(ref uivertex, i);
				uivertex.position = transformItalic.transform(uivertex.position);
				vh.SetUIVertex(uivertex, i);
			}
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x001965D5 File Offset: 0x001947D5
		public void Refresh()
		{
			if (base.graphic != null)
			{
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x001965F0 File Offset: 0x001947F0
		public void GetTransformItalic(out Italic.TransformItalic transformItalic)
		{
			transformItalic.widthScale = this.widthScale;
			float num = (this.italicRight ? (-45f) : 45f);
			transformItalic.baseRotation = Quaternion.Euler(0f, 0f, num);
			float num2 = Mathf.Abs(45f);
			float num3 = 0.017453292f * num2;
			float num4 = Mathf.Sin(num3);
			float num5 = Mathf.Cos(num3);
			float num6 = this.targetAngle / 90f;
			float num7 = 90f - (90f - num2) * num6;
			float num8 = num5 / num4;
			transformItalic.dstWidth = Mathf.Tan(0.017453292f * num7) * num8;
			if (this.adjust != Italic.Adjust.None)
			{
				float num9 = this.targetAngle * (num2 / 90f);
				switch (this.adjust)
				{
				case Italic.Adjust.Type2:
					num9 = -num9;
					break;
				case Italic.Adjust.Type3:
					num9 = 90f - num9;
					break;
				case Italic.Adjust.Type4:
					num9 -= 90f;
					break;
				}
				transformItalic.adjustRotation = Quaternion.Euler(0f, 0f, num9);
				return;
			}
			transformItalic.adjustRotation = Quaternion.identity;
		}

		// Token: 0x04001A8E RID: 6798
		private const float BaseAngle = 45f;

		// Token: 0x04001A8F RID: 6799
		public Italic.Adjust adjust = Italic.Adjust.Type1;

		// Token: 0x04001A90 RID: 6800
		public bool italicRight = true;

		// Token: 0x04001A91 RID: 6801
		public float targetAngle = 60f;

		// Token: 0x04001A92 RID: 6802
		public float widthScale = 1f;

		// Token: 0x02001061 RID: 4193
		public enum Adjust
		{
			// Token: 0x04005B9B RID: 23451
			None,
			// Token: 0x04005B9C RID: 23452
			Type1,
			// Token: 0x04005B9D RID: 23453
			Type2,
			// Token: 0x04005B9E RID: 23454
			Type3,
			// Token: 0x04005B9F RID: 23455
			Type4
		}

		// Token: 0x02001062 RID: 4194
		public struct TransformItalic
		{
			// Token: 0x060052D8 RID: 21208 RVA: 0x00249558 File Offset: 0x00247758
			public Vector3 transform(Vector3 position)
			{
				position.x *= this.widthScale;
				position = this.baseRotation * position;
				position.x *= this.dstWidth;
				position = this.adjustRotation * position;
				return position;
			}

			// Token: 0x04005BA0 RID: 23456
			public Quaternion baseRotation;

			// Token: 0x04005BA1 RID: 23457
			public Quaternion adjustRotation;

			// Token: 0x04005BA2 RID: 23458
			public float dstWidth;

			// Token: 0x04005BA3 RID: 23459
			public float widthScale;
		}
	}
}
