using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	// Token: 0x02000235 RID: 565
	[RequireComponent(typeof(Text))]
	public class TextFitWidth : BaseMeshEffect
	{
		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x0600239E RID: 9118 RVA: 0x00199594 File Offset: 0x00197794
		private Text CachedText
		{
			get
			{
				if (this.cachedText != null)
				{
					return this.cachedText;
				}
				this.cachedText = base.GetComponent<Text>();
				if (this.cachedText != null)
				{
					this.cachedText.horizontalOverflow = HorizontalWrapMode.Overflow;
				}
				return this.cachedText;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x0600239F RID: 9119 RVA: 0x001995E2 File Offset: 0x001977E2
		private RectTransform CachedRect
		{
			get
			{
				if (this.cachedRect != null)
				{
					return this.cachedRect;
				}
				this.cachedRect = base.GetComponent<RectTransform>();
				return this.cachedRect;
			}
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x0019960B File Offset: 0x0019780B
		public void DoUpdate()
		{
			this.modify = true;
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x00199614 File Offset: 0x00197814
		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.IsActive())
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			vh.GetUIVertexStream(list);
			this.ModifyVertices(list);
			vh.Clear();
			vh.AddUIVertexTriangleStream(list);
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x0019964C File Offset: 0x0019784C
		public void ModifyVertices(List<UIVertex> vList)
		{
			if (!this.IsActive() || vList == null || vList.Count == 0)
			{
				return;
			}
			this.UpdateText();
			float alimentAdjustX = this.GetAlimentAdjustX();
			for (int i = 0; i < vList.Count; i++)
			{
				UIVertex uivertex = vList[i];
				Vector3 position = uivertex.position;
				position.x = position.x * this.scale_x + alimentAdjustX;
				uivertex.position = position;
				vList[i] = uivertex;
			}
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x001996C0 File Offset: 0x001978C0
		public void Refresh()
		{
			if (base.graphic != null)
			{
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x001996DB File Offset: 0x001978DB
		private void UpdateText()
		{
			if (!this.IsUpdate())
			{
				return;
			}
			this.modify = false;
			this.scale_x = this.CalcRatio();
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x001996F9 File Offset: 0x001978F9
		private bool IsUpdate()
		{
			this.modify = true;
			return this.modify;
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x00199708 File Offset: 0x00197908
		private float CalcRatio()
		{
			Text text = this.CachedText;
			float rectWidth = this.GetRectWidth(text);
			float textWidth = this.GetTextWidth(text);
			float num = 1f;
			if (textWidth > 0f && (textWidth > rectWidth || (this.extend_width && textWidth < rectWidth)))
			{
				num = rectWidth / textWidth;
			}
			return num;
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x00199750 File Offset: 0x00197950
		private float GetAlimentAdjustX()
		{
			float num = this.CalcRatio();
			if (num == 1f)
			{
				return 0f;
			}
			Text text = this.CachedText;
			float rectWidth = this.GetRectWidth(text);
			float textWidth = this.GetTextWidth(text);
			switch (this.CachedText.alignment)
			{
			case TextAnchor.UpperLeft:
			case TextAnchor.MiddleLeft:
			case TextAnchor.LowerLeft:
			{
				float num2 = rectWidth * this.CachedRect.pivot.x;
				float num3 = textWidth * this.CachedRect.pivot.x;
				return (num2 - num3 + -0.5f) * num;
			}
			case TextAnchor.UpperRight:
			case TextAnchor.MiddleRight:
			case TextAnchor.LowerRight:
			{
				float num4 = rectWidth * (1f - this.CachedRect.pivot.x);
				float num5 = textWidth * (1f - this.CachedRect.pivot.x);
				return (-num4 + num5 + -0.5f) * num;
			}
			}
			return 0f;
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x00199848 File Offset: 0x00197A48
		private float GetRectWidth(Text text)
		{
			if (text != null)
			{
				return text.rectTransform.rect.width;
			}
			return 0f;
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x00199877 File Offset: 0x00197A77
		private float GetTextWidth(Text text)
		{
			return text.preferredWidth;
		}

		// Token: 0x04001ADB RID: 6875
		[SerializeField]
		private bool extend_width;

		// Token: 0x04001ADC RID: 6876
		private Text cachedText;

		// Token: 0x04001ADD RID: 6877
		private RectTransform cachedRect;

		// Token: 0x04001ADE RID: 6878
		private bool modify = true;

		// Token: 0x04001ADF RID: 6879
		private float scale_x;
	}
}
