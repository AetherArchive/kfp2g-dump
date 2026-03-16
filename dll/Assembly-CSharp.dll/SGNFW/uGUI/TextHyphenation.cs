using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	[RequireComponent(typeof(Text))]
	[ExecuteInEditMode]
	public class TextHyphenation : UIBehaviour
	{
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

		private static bool IsFront(char c)
		{
			return Array.Exists<char>(TextHyphenation.HYP_FRONT, (char item) => item == c);
		}

		private static bool IsBack(char c)
		{
			return Array.Exists<char>(TextHyphenation.HYP_BACK, (char item) => item == c);
		}

		private static bool IsLatin(char c)
		{
			return Array.Exists<char>(TextHyphenation.HYP_LATIN, (char item) => item == c);
		}

		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			this.UpdateText(this.text);
		}

		private bool IsOneByteChar(string str)
		{
			return Encoding.UTF8.GetBytes(str).Length == str.Length;
		}

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

		private float GetTextWidth(string str)
		{
			if (this.txtComp.supportRichText)
			{
				str = Regex.Replace(str, TextHyphenation.RITCH_TEXT, string.Empty);
			}
			this.txtComp.text = str;
			return this.txtComp.preferredWidth;
		}

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

		private static readonly string RITCH_TEXT = "(\\<color=.*\\>|</color>|\\<size=.n\\>|</size>|<b>|</b>|<i>|</i>)";

		private static readonly char[] HYP_FRONT = ",)]）」】、。ぁぃぅぇぉっゃゅょゎ ァィゥェォッャュョヮヵヶ".ToCharArray();

		private static readonly char[] HYP_BACK = "([（「【".ToCharArray();

		private static readonly char[] HYP_LATIN = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!\"#$%&'()[]<>{}=~|@:;/?,.-+*`".ToCharArray();

		private static StringBuilder sb = new StringBuilder();

		[SerializeField]
		[TextArea(3, 10)]
		private string text;

		private RectTransform rectTrs;

		private Text txtComp;
	}
}
