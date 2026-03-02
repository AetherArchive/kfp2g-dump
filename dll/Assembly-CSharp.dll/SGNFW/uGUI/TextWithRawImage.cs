using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	// Token: 0x0200023A RID: 570
	[ExecuteInEditMode]
	public class TextWithRawImage : Text
	{
		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x060023E3 RID: 9187 RVA: 0x0019A9DD File Offset: 0x00198BDD
		// (set) Token: 0x060023E4 RID: 9188 RVA: 0x0019A9E8 File Offset: 0x00198BE8
		public float IconScale
		{
			get
			{
				return this.iconScale;
			}
			set
			{
				if (this.iconScale != value)
				{
					this.iconScale = value;
					foreach (TextWithRawImage.Icon icon in this.iconList)
					{
						icon.recalculate = true;
					}
				}
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x060023E5 RID: 9189 RVA: 0x0019AA4C File Offset: 0x00198C4C
		// (set) Token: 0x060023E6 RID: 9190 RVA: 0x0019AA54 File Offset: 0x00198C54
		public Rect UvRect
		{
			get
			{
				return this.uvRect;
			}
			set
			{
				if (this.uvRect != value)
				{
					this.uvRect = value;
					foreach (TextWithRawImage.Icon icon in this.iconList)
					{
						icon.recalculate = true;
					}
				}
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x060023E7 RID: 9191 RVA: 0x0019AABC File Offset: 0x00198CBC
		// (set) Token: 0x060023E8 RID: 9192 RVA: 0x0019AAC4 File Offset: 0x00198CC4
		public Material Mat
		{
			get
			{
				return this.mat;
			}
			set
			{
				if (this.mat != value)
				{
					this.mat = value;
					foreach (TextWithRawImage.Icon icon in this.iconList)
					{
						icon.recalculate = true;
					}
				}
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x060023E9 RID: 9193 RVA: 0x0019AB2C File Offset: 0x00198D2C
		// (set) Token: 0x060023EA RID: 9194 RVA: 0x0019AB34 File Offset: 0x00198D34
		public override string text
		{
			get
			{
				return base.text;
			}
			set
			{
				if (base.text != value)
				{
					base.text = value;
					this.Clear();
					if (this.onChange != null)
					{
						this.onChange(this);
					}
				}
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x060023EB RID: 9195 RVA: 0x0019AB68 File Offset: 0x00198D68
		public TextHyphenation CachedHyphenText
		{
			get
			{
				TextHyphenation textHyphenation;
				if ((textHyphenation = this.cachedHyphenText) == null)
				{
					textHyphenation = (this.cachedHyphenText = base.GetComponent<TextHyphenation>());
				}
				return textHyphenation;
			}
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x0019AB90 File Offset: 0x00198D90
		public void Clear()
		{
			foreach (TextWithRawImage.Icon icon in this.iconList)
			{
				if (icon.rimg != null)
				{
					Object.Destroy(icon.rimg.gameObject);
					icon.rimg = null;
				}
			}
			this.iconList.Clear();
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x0019AC0C File Offset: 0x00198E0C
		public void AddTexts(string texts)
		{
			base.text += texts;
			if (this.onChange != null)
			{
				this.onChange(this);
			}
		}

		// Token: 0x060023EE RID: 9198 RVA: 0x0019AC34 File Offset: 0x00198E34
		protected override void OnPopulateMesh(VertexHelper toFill)
		{
			base.OnPopulateMesh(toFill);
			RawImage[] componentsInChildren = base.GetComponentsInChildren<RawImage>();
			if (componentsInChildren == null || componentsInChildren.Length == 0)
			{
				return;
			}
			int[] indexes = this.GetIndexes(this);
			if (indexes == null || indexes.Length == 0 || indexes.Length % 3 != 0)
			{
				return;
			}
			Italic component = base.GetComponent<Italic>();
			Italic.TransformItalic transformItalic = default(Italic.TransformItalic);
			if (component != null)
			{
				component.GetTransformItalic(out transformItalic);
			}
			List<UIVertex> list = new List<UIVertex>();
			toFill.GetUIVertexStream(list);
			Vector3[] array = new Vector3[6];
			for (int i = 0; i < indexes.Length; i += 3)
			{
				Vector3 vector = Vector3.zero;
				float num = 0f;
				for (int j = 0; j < 3; j++)
				{
					int num2 = indexes[i + j] * 6;
					int num3 = num2 + 6;
					if (num2 < list.Count)
					{
						for (int k = num2; k < num3; k++)
						{
							if (null == component)
							{
								array[k - num2] = list[k].position;
							}
							else
							{
								array[k - num2] = transformItalic.transform(list[k].position);
							}
						}
						Vector3 vector2 = this.CalcCenter(array);
						vector += vector2;
						num += this.CalcWidth(array);
					}
				}
				vector /= 3f;
				this.iconList.Add(new TextWithRawImage.Icon(componentsInChildren[i / 3], vector, num));
			}
		}

		// Token: 0x060023EF RID: 9199 RVA: 0x0019ADA4 File Offset: 0x00198FA4
		private Vector3 CalcCenter(Vector3[] verts)
		{
			Vector3 vector = Vector3.zero;
			if (verts == null || verts.Length == 0)
			{
				return vector;
			}
			foreach (Vector3 vector2 in verts)
			{
				vector += vector2;
			}
			return vector / (float)verts.Length;
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x0019ADEA File Offset: 0x00198FEA
		private float CalcWidth(Vector3[] verts)
		{
			return Vector3.Distance(verts[0], verts[1]);
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x0019AE00 File Offset: 0x00199000
		private int[] GetIndexes(Text text)
		{
			List<int> list = new List<int>();
			foreach (object obj in Regex.Matches(text.text, "\\[\\+\\]"))
			{
				Match match = (Match)obj;
				list.Add(match.Index);
				list.Add(match.Index + 1);
				list.Add(match.Index + 2);
			}
			return list.ToArray();
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x0019AE90 File Offset: 0x00199090
		private void Update()
		{
			if (this.iconList == null || this.iconList.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < this.iconList.Count; i++)
			{
				if (this.iconList[i].recalculate)
				{
					RawImage rimg = this.iconList[i].rimg;
					if (!(rimg.texture == null))
					{
						RectTransform rectTransform = rimg.rectTransform;
						rectTransform.anchoredPosition = this.iconList[i].pos;
						float num = (float)rimg.texture.width * this.uvRect.width;
						float num2 = (float)rimg.texture.height * this.uvRect.height / num;
						float num3 = this.iconList[i].w * this.iconScale;
						float num4 = num3 * num2;
						rectTransform.sizeDelta = new Vector2(num3, num4);
						rimg.uvRect = this.uvRect;
						rimg.enabled = true;
						this.iconList[i].recalculate = false;
					}
				}
			}
		}

		// Token: 0x04001AFA RID: 6906
		public Action<TextWithRawImage> onChange;

		// Token: 0x04001AFB RID: 6907
		public Action<TextWithRawImage, RawImage> onUpdate;

		// Token: 0x04001AFC RID: 6908
		[SerializeField]
		private float iconScale = 1f;

		// Token: 0x04001AFD RID: 6909
		[SerializeField]
		private Rect uvRect = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04001AFE RID: 6910
		[SerializeField]
		private Material mat;

		// Token: 0x04001AFF RID: 6911
		private List<TextWithRawImage.Icon> iconList = new List<TextWithRawImage.Icon>();

		// Token: 0x04001B00 RID: 6912
		private TextHyphenation cachedHyphenText;

		// Token: 0x02001078 RID: 4216
		private class Icon
		{
			// Token: 0x06005309 RID: 21257 RVA: 0x00249BAF File Offset: 0x00247DAF
			public Icon(RawImage rimg, Vector3 pos, float w)
			{
				this.rimg = rimg;
				this.pos = pos;
				this.w = w;
				this.recalculate = true;
			}

			// Token: 0x04005BDB RID: 23515
			public RawImage rimg;

			// Token: 0x04005BDC RID: 23516
			public Vector3 pos;

			// Token: 0x04005BDD RID: 23517
			public float w;

			// Token: 0x04005BDE RID: 23518
			public bool recalculate;
		}
	}
}
