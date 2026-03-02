using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	// Token: 0x02000239 RID: 569
	[ExecuteInEditMode]
	public class TextWithIcon : Text
	{
		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x060023D6 RID: 9174 RVA: 0x0019A4CB File Offset: 0x001986CB
		// (set) Token: 0x060023D7 RID: 9175 RVA: 0x0019A4D4 File Offset: 0x001986D4
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
					foreach (TextWithIcon.Icon icon in this.iconList)
					{
						icon.recalculate = true;
					}
				}
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x060023D8 RID: 9176 RVA: 0x0019A538 File Offset: 0x00198738
		// (set) Token: 0x060023D9 RID: 9177 RVA: 0x0019A540 File Offset: 0x00198740
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

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x060023DA RID: 9178 RVA: 0x0019A574 File Offset: 0x00198774
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

		// Token: 0x060023DB RID: 9179 RVA: 0x0019A59C File Offset: 0x0019879C
		public void Clear()
		{
			foreach (TextWithIcon.Icon icon in this.iconList)
			{
				if (icon.img != null)
				{
					Object.Destroy(icon.img.gameObject);
					icon.img = null;
				}
			}
			this.iconList.Clear();
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x0019A618 File Offset: 0x00198818
		public void AddTexts(string texts)
		{
			base.text += texts;
			if (this.onChange != null)
			{
				this.onChange(this);
			}
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x0019A640 File Offset: 0x00198840
		protected override void OnPopulateMesh(VertexHelper toFill)
		{
			base.OnPopulateMesh(toFill);
			Image[] componentsInChildren = base.GetComponentsInChildren<Image>();
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
				this.iconList.Add(new TextWithIcon.Icon(componentsInChildren[i / 3], vector, num));
			}
		}

		// Token: 0x060023DE RID: 9182 RVA: 0x0019A7B0 File Offset: 0x001989B0
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

		// Token: 0x060023DF RID: 9183 RVA: 0x0019A7F6 File Offset: 0x001989F6
		private float CalcWidth(Vector3[] verts)
		{
			return Vector3.Distance(verts[0], verts[1]);
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x0019A80C File Offset: 0x00198A0C
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

		// Token: 0x060023E1 RID: 9185 RVA: 0x0019A89C File Offset: 0x00198A9C
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
					Image img = this.iconList[i].img;
					if (!(img.sprite == null))
					{
						RectTransform rectTransform = img.rectTransform;
						rectTransform.anchoredPosition = this.iconList[i].pos;
						float width = img.sprite.rect.width;
						float num = img.sprite.rect.height / width;
						float num2 = this.iconList[i].w * this.iconScale;
						float num3 = num2 * num;
						rectTransform.sizeDelta = new Vector2(num2, num3);
						if (this.onUpdate != null)
						{
							this.onUpdate(this, img);
						}
						img.enabled = true;
						this.iconList[i].recalculate = false;
					}
				}
			}
		}

		// Token: 0x04001AF5 RID: 6901
		public Action<TextWithIcon> onChange;

		// Token: 0x04001AF6 RID: 6902
		public Action<TextWithIcon, Image> onUpdate;

		// Token: 0x04001AF7 RID: 6903
		[SerializeField]
		private float iconScale = 1f;

		// Token: 0x04001AF8 RID: 6904
		private List<TextWithIcon.Icon> iconList = new List<TextWithIcon.Icon>();

		// Token: 0x04001AF9 RID: 6905
		private TextHyphenation cachedHyphenText;

		// Token: 0x02001077 RID: 4215
		private class Icon
		{
			// Token: 0x06005308 RID: 21256 RVA: 0x00249B8B File Offset: 0x00247D8B
			public Icon(Image img, Vector3 pos, float w)
			{
				this.img = img;
				this.pos = pos;
				this.w = w;
				this.recalculate = true;
			}

			// Token: 0x04005BD7 RID: 23511
			public Image img;

			// Token: 0x04005BD8 RID: 23512
			public Vector3 pos;

			// Token: 0x04005BD9 RID: 23513
			public float w;

			// Token: 0x04005BDA RID: 23514
			public bool recalculate;
		}
	}
}
