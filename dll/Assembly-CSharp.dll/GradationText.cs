using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F3 RID: 243
[RequireComponent(typeof(Text))]
public class GradationText : BaseMeshEffect
{
	// Token: 0x17000302 RID: 770
	// (get) Token: 0x06000BAF RID: 2991 RVA: 0x00045136 File Offset: 0x00043336
	// (set) Token: 0x06000BB0 RID: 2992 RVA: 0x0004513E File Offset: 0x0004333E
	public GradationText.Blend BlendMode
	{
		get
		{
			return this._blendMode;
		}
		set
		{
			this._blendMode = value;
		}
	}

	// Token: 0x17000303 RID: 771
	// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x00045147 File Offset: 0x00043347
	// (set) Token: 0x06000BB2 RID: 2994 RVA: 0x0004514F File Offset: 0x0004334F
	public Gradient EffectGradient
	{
		get
		{
			return this._effectGradient;
		}
		set
		{
			this._effectGradient = value;
		}
	}

	// Token: 0x17000304 RID: 772
	// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x00045158 File Offset: 0x00043358
	// (set) Token: 0x06000BB4 RID: 2996 RVA: 0x00045160 File Offset: 0x00043360
	public GradationText.Type GradientType
	{
		get
		{
			return this._gradientType;
		}
		set
		{
			this._gradientType = value;
		}
	}

	// Token: 0x17000305 RID: 773
	// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x00045169 File Offset: 0x00043369
	// (set) Token: 0x06000BB6 RID: 2998 RVA: 0x00045171 File Offset: 0x00043371
	public float Offset
	{
		get
		{
			return this._offset;
		}
		set
		{
			this._offset = value;
		}
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x0004517C File Offset: 0x0004337C
	public override void ModifyMesh(VertexHelper helper)
	{
		if (!this.IsActive() || helper.currentVertCount == 0)
		{
			return;
		}
		List<UIVertex> list = new List<UIVertex>();
		helper.GetUIVertexStream(list);
		int count = list.Count;
		GradationText.Type gradientType = this.GradientType;
		if (gradientType == GradationText.Type.Horizontal)
		{
			float num = list[0].position.x;
			float num2 = list[0].position.x;
			for (int i = count - 1; i >= 1; i--)
			{
				float x = list[i].position.x;
				if (x > num2)
				{
					num2 = x;
				}
				else if (x < num)
				{
					num = x;
				}
			}
			float num3 = 1f / (num2 - num);
			UIVertex uivertex = default(UIVertex);
			for (int j = 0; j < helper.currentVertCount; j++)
			{
				helper.PopulateUIVertex(ref uivertex, j);
				uivertex.color = this.BlendColor(uivertex.color, this.EffectGradient.Evaluate((uivertex.position.x - num) * num3 - this.Offset));
				helper.SetUIVertex(uivertex, j);
			}
			return;
		}
		if (gradientType != GradationText.Type.Vertical)
		{
			return;
		}
		float num4 = list[0].position.y;
		float num5 = list[0].position.y;
		for (int k = count - 1; k >= 1; k--)
		{
			float y = list[k].position.y;
			if (y > num5)
			{
				num5 = y;
			}
			else if (y < num4)
			{
				num4 = y;
			}
		}
		float num6 = 1f / (num5 - num4);
		UIVertex uivertex2 = default(UIVertex);
		for (int l = 0; l < helper.currentVertCount; l++)
		{
			helper.PopulateUIVertex(ref uivertex2, l);
			uivertex2.color = this.BlendColor(uivertex2.color, this.EffectGradient.Evaluate((uivertex2.position.y - num4) * num6 - this.Offset));
			helper.SetUIVertex(uivertex2, l);
		}
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x00045390 File Offset: 0x00043590
	private Color BlendColor(Color colorA, Color colorB)
	{
		GradationText.Blend blendMode = this.BlendMode;
		if (blendMode == GradationText.Blend.Add)
		{
			return colorA + colorB;
		}
		if (blendMode != GradationText.Blend.Multiply)
		{
			return colorB;
		}
		return colorA * colorB;
	}

	// Token: 0x04000925 RID: 2341
	[SerializeField]
	private GradationText.Type _gradientType;

	// Token: 0x04000926 RID: 2342
	[SerializeField]
	private GradationText.Blend _blendMode = GradationText.Blend.Multiply;

	// Token: 0x04000927 RID: 2343
	[SerializeField]
	[Range(-1f, 1f)]
	private float _offset;

	// Token: 0x04000928 RID: 2344
	[SerializeField]
	private Gradient _effectGradient = new Gradient
	{
		colorKeys = new GradientColorKey[]
		{
			new GradientColorKey(Color.black, 0f),
			new GradientColorKey(Color.white, 1f)
		}
	};

	// Token: 0x0200080B RID: 2059
	public enum Type
	{
		// Token: 0x04003607 RID: 13831
		Horizontal,
		// Token: 0x04003608 RID: 13832
		Vertical
	}

	// Token: 0x0200080C RID: 2060
	public enum Blend
	{
		// Token: 0x0400360A RID: 13834
		Override,
		// Token: 0x0400360B RID: 13835
		Add,
		// Token: 0x0400360C RID: 13836
		Multiply
	}
}
