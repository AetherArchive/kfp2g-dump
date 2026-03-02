using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	// Token: 0x02000236 RID: 566
	[RequireComponent(typeof(Text))]
	[ExecuteInEditMode]
	public class TextHyphenation : UIBehaviour
	{
		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x060023AC RID: 9132 RVA: 0x001998A0 File Offset: 0x00197AA0
		// (set) Token: 0x060023AB RID: 9131 RVA: 0x0019988E File Offset: 0x00197A8E
		public float Width
		{
			get
			{
				return this.RectTrs.rect.width;
			}
			set
			{
				this.RectTrs.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x060023AE RID: 9134 RVA: 0x001998CE File Offset: 0x00197ACE
		// (set) Token: 0x060023AD RID: 9133 RVA: 0x001998C0 File Offset: 0x00197AC0
		public int Size
		{
			get
			{
				return this.TxtComp.fontSize;
			}
			set
			{
				this.TxtComp.fontSize = value;
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x060023B0 RID: 9136 RVA: 0x001998F0 File Offset: 0x00197AF0
		// (set) Token: 0x060023AF RID: 9135 RVA: 0x001998DB File Offset: 0x00197ADB
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
				this.UpdateText(this.text);
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x060023B1 RID: 9137 RVA: 0x001998F8 File Offset: 0x00197AF8
		public Text TxtComp
		{
			get
			{
				if (this.txtComp == null)
				{
					this.txtComp = base.GetComponent<Text>();
				}
				return this.txtComp;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x060023B2 RID: 9138 RVA: 0x0019991A File Offset: 0x00197B1A
		private RectTransform RectTrs
		{
			get
			{
				if (this.rectTrs == null)
				{
					this.rectTrs = base.GetComponent<RectTransform>();
				}
				return this.rectTrs;
			}
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x0019993C File Offset: 0x00197B3C
		private static bool IsFront(char c)
		{
			return Array.Exists<char>(TextHyphenation.HYP_FRONT, (char item) => item == c);
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x0019996C File Offset: 0x00197B6C
		private static bool IsBack(char c)
		{
			return Array.Exists<char>(TextHyphenation.HYP_BACK, (char item) => item == c);
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x0019999C File Offset: 0x00197B9C
		private static bool IsLatin(char c)
		{
			return Array.Exists<char>(TextHyphenation.HYP_LATIN, (char item) => item == c);
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x001999CC File Offset: 0x00197BCC
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			this.UpdateText(this.text);
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x001999E0 File Offset: 0x00197BE0
		private bool IsOneByteChar(string str)
		{
			return Encoding.UTF8.GetBytes(str).Length == str.Length;
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x001999FA File Offset: 0x00197BFA
		private void UpdateText(string str)
		{
			if (str == null)
			{
				return;
			}
			if (this.IsOneByteChar(str))
			{
				this.TxtComp.text = str;
				return;
			}
			this.TxtComp.text = this.GetFormatedText(str);
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x00199A28 File Offset: 0x00197C28
		private float GetTextWidth(string str)
		{
			if (this.txtComp.supportRichText)
			{
				str = Regex.Replace(str, TextHyphenation.RITCH_TEXT, string.Empty);
			}
			this.txtComp.text = str;
			return this.txtComp.preferredWidth;
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x00199A60 File Offset: 0x00197C60
		private string GetFormatedText(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return string.Empty;
			}
			float width = this.RectTrs.rect.width;
			string[] array = str.Split(new string[]
			{
				Environment.NewLine,
				"\n"
			}, StringSplitOptions.None);
			TextHyphenation.sb.Length = 0;
			int i = 0;
			while (i < array.Length)
			{
				float num = this.GetTextWidth(array[i]);
				if (num > width)
				{
					num = 0f;
					using (List<string>.Enumerator enumerator = this.GetWordList(array[i]).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string text = enumerator.Current;
							float textWidth = this.GetTextWidth(text);
							if (num + textWidth > width)
							{
								TextHyphenation.sb.Append(Environment.NewLine);
								num = 0f;
							}
							TextHyphenation.sb.Append(text);
							num += textWidth;
						}
						goto IL_00E8;
					}
					goto IL_00DA;
				}
				goto IL_00DA;
				IL_00E8:
				TextHyphenation.sb.Append(Environment.NewLine);
				i++;
				continue;
				IL_00DA:
				TextHyphenation.sb.Append(array[i]);
				goto IL_00E8;
			}
			return TextHyphenation.sb.ToString();
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x00199B8C File Offset: 0x00197D8C
		private List<string> GetWordList(string str)
		{
			List<string> list = new List<string>();
			char c = '\0';
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < str.Length; i++)
			{
				char c2 = str[i];
				char c3 = ((i < str.Length - 1) ? str[i + 1] : c);
				stringBuilder.Append(c2);
				if ((!TextHyphenation.IsLatin(c2) || !TextHyphenation.IsLatin(c3)) && !TextHyphenation.IsFront(c3) && !TextHyphenation.IsBack(c2))
				{
					list.Add(stringBuilder.ToString());
					stringBuilder.Length = 0;
				}
			}
			return list;
		}

		// Token: 0x04001AE0 RID: 6880
		private static readonly string RITCH_TEXT = "(\\<color=.*\\>|</color>|\\<size=.n\\>|</size>|<b>|</b>|<i>|</i>)";

		// Token: 0x04001AE1 RID: 6881
		private static readonly char[] HYP_FRONT = ",)]）」】、。ぁぃぅぇぉっゃゅょゎ ァィゥェォッャュョヮヵヶ".ToCharArray();

		// Token: 0x04001AE2 RID: 6882
		private static readonly char[] HYP_BACK = "([（「【".ToCharArray();

		// Token: 0x04001AE3 RID: 6883
		private static readonly char[] HYP_LATIN = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!\"#$%&'()[]<>{}=~|@:;/?,.-+*`".ToCharArray();

		// Token: 0x04001AE4 RID: 6884
		private static StringBuilder sb = new StringBuilder();

		// Token: 0x04001AE5 RID: 6885
		[SerializeField]
		[TextArea(3, 10)]
		private string text;

		// Token: 0x04001AE6 RID: 6886
		private RectTransform rectTrs;

		// Token: 0x04001AE7 RID: 6887
		private Text txtComp;
	}
}
