using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	[ExecuteInEditMode]
	public class TextWithIcon : Text
	{
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

		public void AddTexts(string texts)
		{
			base.text += texts;
			if (this.onChange != null)
			{
				this.onChange(this);
			}
		}

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

		private float CalcWidth(Vector3[] verts)
		{
			return Vector3.Distance(verts[0], verts[1]);
		}

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

		public Action<TextWithIcon> onChange;

		public Action<TextWithIcon, Image> onUpdate;

		[SerializeField]
		private float iconScale = 1f;

		private List<TextWithIcon.Icon> iconList = new List<TextWithIcon.Icon>();

		private TextHyphenation cachedHyphenText;

		private class Icon
		{
			public Icon(Image img, Vector3 pos, float w)
			{
				this.img = img;
				this.pos = pos;
				this.w = w;
				this.recalculate = true;
			}

			public Image img;

			public Vector3 pos;

			public float w;

			public bool recalculate;
		}
	}
}
