using System;
using UnityEngine;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	[AddComponentMenu("UI/Effects/Italic")]
	[RequireComponent(typeof(Graphic))]
	public class Italic : BaseMeshEffect
	{
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

		public void Refresh()
		{
			if (base.graphic != null)
			{
				base.graphic.SetVerticesDirty();
			}
		}

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

		private const float BaseAngle = 45f;

		public Italic.Adjust adjust = Italic.Adjust.Type1;

		public bool italicRight = true;

		public float targetAngle = 60f;

		public float widthScale = 1f;

		public enum Adjust
		{
			None,
			Type1,
			Type2,
			Type3,
			Type4
		}

		public struct TransformItalic
		{
			public Vector3 transform(Vector3 position)
			{
				position.x *= this.widthScale;
				position = this.baseRotation * position;
				position.x *= this.dstWidth;
				position = this.adjustRotation * position;
				return position;
			}

			public Quaternion baseRotation;

			public Quaternion adjustRotation;

			public float dstWidth;

			public float widthScale;
		}
	}
}
