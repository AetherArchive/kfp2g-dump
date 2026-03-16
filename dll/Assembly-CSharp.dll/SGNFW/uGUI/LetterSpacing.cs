using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	public class LetterSpacing : BaseMeshEffect
	{
		protected LetterSpacing()
		{
		}

		public float spacing
		{
			get
			{
				return this.m_spacing;
			}
			set
			{
				if (this.m_spacing == value)
				{
					return;
				}
				this.m_spacing = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		public void ModifyVertices(List<UIVertex> verts)
		{
			if (!this.IsActive())
			{
				return;
			}
			Text component = base.GetComponent<Text>();
			if (component == null)
			{
				return;
			}
			string[] array = component.text.Split('\n', StringSplitOptions.None);
			float num = this.spacing * (float)component.fontSize / 100f;
			float num2 = 0f;
			int num3 = 0;
			switch (component.alignment)
			{
			case TextAnchor.UpperLeft:
			case TextAnchor.MiddleLeft:
			case TextAnchor.LowerLeft:
				num2 = 0f;
				break;
			case TextAnchor.UpperCenter:
			case TextAnchor.MiddleCenter:
			case TextAnchor.LowerCenter:
				num2 = 0.5f;
				break;
			case TextAnchor.UpperRight:
			case TextAnchor.MiddleRight:
			case TextAnchor.LowerRight:
				num2 = 1f;
				break;
			}
			foreach (string text in array)
			{
				float num4 = (float)(text.Length - 1) * num * num2;
				for (int j = 0; j < text.Length; j++)
				{
					int num5 = num3 * 6;
					int num6 = num3 * 6 + 1;
					int num7 = num3 * 6 + 2;
					int num8 = num3 * 6 + 3;
					int num9 = num3 * 6 + 4;
					int num10 = num3 * 6 + 5;
					if (num8 > verts.Count - 1)
					{
						return;
					}
					UIVertex uivertex = verts[num5];
					UIVertex uivertex2 = verts[num6];
					UIVertex uivertex3 = verts[num7];
					UIVertex uivertex4 = verts[num8];
					UIVertex uivertex5 = verts[num9];
					UIVertex uivertex6 = verts[num10];
					Vector3 vector = Vector3.right * (num * (float)j - num4);
					uivertex.position += vector;
					uivertex2.position += vector;
					uivertex3.position += vector;
					uivertex4.position += vector;
					uivertex5.position += vector;
					uivertex6.position += vector;
					verts[num5] = uivertex;
					verts[num6] = uivertex2;
					verts[num7] = uivertex3;
					verts[num8] = uivertex4;
					verts[num9] = uivertex5;
					verts[num10] = uivertex6;
					num3++;
				}
				num3++;
			}
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

		[SerializeField]
		private float m_spacing;
	}
}
