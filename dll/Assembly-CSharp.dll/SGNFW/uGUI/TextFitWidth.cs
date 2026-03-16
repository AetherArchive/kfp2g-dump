using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	[RequireComponent(typeof(Text))]
	public class TextFitWidth : BaseMeshEffect
	{
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

		public void DoUpdate()
		{
			this.modify = true;
		}

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

		public void Refresh()
		{
			if (base.graphic != null)
			{
				base.graphic.SetVerticesDirty();
			}
		}

		private void UpdateText()
		{
			if (!this.IsUpdate())
			{
				return;
			}
			this.modify = false;
			this.scale_x = this.CalcRatio();
		}

		private bool IsUpdate()
		{
			this.modify = true;
			return this.modify;
		}

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

		private float GetRectWidth(Text text)
		{
			if (text != null)
			{
				return text.rectTransform.rect.width;
			}
			return 0f;
		}

		private float GetTextWidth(Text text)
		{
			return text.preferredWidth;
		}

		[SerializeField]
		private bool extend_width;

		private Text cachedText;

		private RectTransform cachedRect;

		private bool modify = true;

		private float scale_x;
	}
}
