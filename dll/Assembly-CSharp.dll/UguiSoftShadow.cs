using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UguiSoftShadow : Shadow
{
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

	private void ModifyVertices(List<UIVertex> verts)
	{
		int num = verts.Count * 5;
		if (verts.Capacity < num)
		{
			verts.Capacity = num;
		}
		int num2 = 0;
		int num3 = verts.Count;
		for (int i = 0; i < this._OutLineResolution; i++)
		{
			int num4 = 0;
			while ((float)num4 < this._BlurResolution)
			{
				float num5 = this._BlurResolution - (float)(num4 / 1);
				int num6 = Mathf.RoundToInt(255f - 255f / num5);
				float num7 = Mathf.Sin(6.2831855f * (float)i / (float)this._OutLineResolution) * this._OutlineWidth * (float)num4 + base.effectDistance.x;
				float num8 = Mathf.Cos(6.2831855f * (float)i / (float)this._OutLineResolution) * this._OutlineWidth * (float)num4 + base.effectDistance.y;
				base.ApplyShadowZeroAlloc(verts, base.effectColor * new Color(1f, 1f, 1f, (float)num6 * this.BlurContrast / 255f), num2, verts.Count, num7, num8);
				num2 = num3;
				num3 = verts.Count;
				num4++;
			}
		}
	}

	public float _OutlineWidth = 8f;

	[SerializeField]
	[Range(0f, 12f)]
	public int _OutLineResolution = 12;

	[SerializeField]
	[Range(0f, 6f)]
	public float _BlurResolution = 4f;

	[SerializeField]
	[Range(0f, 1f)]
	public float BlurContrast = 0.3f;
}
