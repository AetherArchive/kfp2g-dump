using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SGNFW.Common
{
	// Token: 0x02000257 RID: 599
	public class FitGUI
	{
		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06002565 RID: 9573 RVA: 0x0019F56B File Offset: 0x0019D76B
		public static float width
		{
			get
			{
				return FitGUI.orgWidth - FitGUI.marginWidth;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06002566 RID: 9574 RVA: 0x0019F578 File Offset: 0x0019D778
		public static float height
		{
			get
			{
				return FitGUI.orgHeight - FitGUI.marginHeight;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06002567 RID: 9575 RVA: 0x0019F585 File Offset: 0x0019D785
		public static GUIStyle ButtonStyle
		{
			get
			{
				return FitGUI.MakeStyle(GUI.skin.button, FitGUI.fontSizeLarge);
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06002568 RID: 9576 RVA: 0x0019F59B File Offset: 0x0019D79B
		public static GUIStyle LabelStyle
		{
			get
			{
				return FitGUI.MakeStyle(GUI.skin.label, FitGUI.fontSize);
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06002569 RID: 9577 RVA: 0x0019F5B1 File Offset: 0x0019D7B1
		public static GUIStyle TextAreaStyle
		{
			get
			{
				return FitGUI.MakeStyle(GUI.skin.textArea, FitGUI.fontSize);
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x0600256A RID: 9578 RVA: 0x0019F5C7 File Offset: 0x0019D7C7
		public static GUIStyle SelectionGridStyle
		{
			get
			{
				return FitGUI.MakeStyle(GUI.skin.button, FitGUI.fontSize);
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x0600256B RID: 9579 RVA: 0x0019F5DD File Offset: 0x0019D7DD
		public static GUIStyle ToggleStyle
		{
			get
			{
				return FitGUI.MakeStyle(GUI.skin.toggle, FitGUI.fontSize);
			}
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x0019F5F4 File Offset: 0x0019D7F4
		public static GUIStyle BoxStyle(string colorName = "")
		{
			GUIStyle guistyle = new GUIStyle(GUI.skin.box);
			if (!(colorName == "blue"))
			{
				if (colorName == "red")
				{
					guistyle.normal.background = FitGUI.bgTexRed;
					return guistyle;
				}
				if (colorName == "gray")
				{
					guistyle.normal.background = FitGUI.bgTexGray;
					return guistyle;
				}
				if (colorName == "pink")
				{
					guistyle.normal.background = FitGUI.bgTexPink;
					return guistyle;
				}
				if (colorName == "green")
				{
					guistyle.normal.background = FitGUI.bgTexGreen;
					return guistyle;
				}
			}
			guistyle.normal.background = FitGUI.bgTexBlue;
			return guistyle;
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x0600256D RID: 9581 RVA: 0x0019F6AB File Offset: 0x0019D8AB
		public static GUIStyle HorizontalSliderStyle
		{
			get
			{
				return FitGUI.MakeStyle(GUI.skin.horizontalSlider, FitGUI.fontSize);
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x0600256E RID: 9582 RVA: 0x0019F6C1 File Offset: 0x0019D8C1
		public static GUIStyle HorizontalSliderThumbStyle
		{
			get
			{
				return FitGUI.MakeStyle(GUI.skin.horizontalSliderThumb, FitGUI.fontSize);
			}
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x0019F6D7 File Offset: 0x0019D8D7
		public static void Initialize(int sw, int sh, int fs, int fss, int fsl, int mw = 0, int mh = 0)
		{
			FitGUI.orgWidth = (float)sw;
			FitGUI.orgHeight = (float)sh;
			FitGUI.fontSize = (float)fs;
			FitGUI.fontSizeSmall = (float)fss;
			FitGUI.fontSizeLarge = (float)fsl;
			FitGUI.marginWidth = (float)mw;
			FitGUI.marginHeight = (float)mh;
			FitGUI.Initialize();
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x0019F714 File Offset: 0x0019D914
		private static void Initialize()
		{
			FitGUI.ratioWidth = (float)Screen.width / FitGUI.orgWidth;
			FitGUI.ratioHeight = (float)Screen.height / FitGUI.orgHeight;
			FitGUI.ratio = FitGUI.ratioWidth;
			if (FitGUI.ratioWidth > FitGUI.ratioHeight)
			{
				FitGUI.ratio = FitGUI.ratioHeight;
			}
			if (FitGUI.ratio < 1f)
			{
				FitGUI.ratio = 1f;
			}
			FitGUI.bgTexBlue = FitGUI.CreateColorTexture(new Color(0.4375f, 0.5703125f, 0.7421875f));
			FitGUI.bgTexRed = FitGUI.CreateColorTexture(new Color(1f, 0f, 0f));
			FitGUI.bgTexGray = FitGUI.CreateColorTexture(new Color(0.49609375f, 0.49609375f, 0.49609375f));
			FitGUI.bgTexPink = FitGUI.CreateColorTexture(new Color(0.99609375f, 0.6796875f, 0.78515625f));
			FitGUI.bgTexGreen = FitGUI.CreateColorTexture(new Color(0.15234375f, 0.69140625f, 0.30859375f));
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x0019F80F File Offset: 0x0019DA0F
		public static Texture2D CreateColorTexture(Color color)
		{
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.SetPixel(0, 0, color);
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x0019F827 File Offset: 0x0019DA27
		public static Texture2D CreateColorTexture(float r, float g, float b, float a = 1f)
		{
			return FitGUI.CreateColorTexture(new Color(r, g, b, a));
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x0019F837 File Offset: 0x0019DA37
		public static float GetLength(float l)
		{
			return FitGUI.ratio * l;
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x0019F840 File Offset: 0x0019DA40
		public static float GetWidth(float w)
		{
			return FitGUI.ratioWidth * w;
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x0019F849 File Offset: 0x0019DA49
		public static float GetHeight(float h)
		{
			return FitGUI.ratioHeight * h;
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x0019F852 File Offset: 0x0019DA52
		public static GUILayoutOption Width(float w)
		{
			return GUILayout.Width(FitGUI.GetWidth(w));
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x0019F85F File Offset: 0x0019DA5F
		public static GUILayoutOption Height(float h)
		{
			return GUILayout.Height(FitGUI.GetHeight(h));
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x0019F86C File Offset: 0x0019DA6C
		public static GUILayoutOption[] Size(float w, float h)
		{
			return new GUILayoutOption[]
			{
				FitGUI.Width(w),
				FitGUI.Height(h)
			};
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x0019F888 File Offset: 0x0019DA88
		public static GUILayoutOption[] TextSize(string text)
		{
			Vector2 vector = FitGUI.LabelStyle.CalcSize(new GUIContent(text));
			return FitGUI.Size(vector.x, vector.y);
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x0019F8B7 File Offset: 0x0019DAB7
		public static GUIStyle MakeStyle(GUIStyle org, float fontSize)
		{
			return new GUIStyle(org)
			{
				fontSize = (int)(fontSize * FitGUI.ratio),
				fontStyle = FontStyle.Bold
			};
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x0019F8D4 File Offset: 0x0019DAD4
		public static void LabelFit(string text, GUIStyle style = null)
		{
			if (style == null)
			{
				style = FitGUI.LabelStyle;
			}
			GUILayout.Label(text, style, FitGUI.TextSize(text));
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x0019F8F0 File Offset: 0x0019DAF0
		public static void LabelLarge(string text, float w = 200f)
		{
			GUIStyle guistyle = FitGUI.MakeStyle(GUI.skin.label, FitGUI.fontSizeLarge);
			FitGUI.Label(text, w, guistyle);
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x0019F91A File Offset: 0x0019DB1A
		public static void Label(string text)
		{
			GUILayout.Label(text, FitGUI.LabelStyle, Array.Empty<GUILayoutOption>());
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x0019F92C File Offset: 0x0019DB2C
		public static void Label(string text, float w)
		{
			FitGUI.Label(text, w, null);
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x0019F936 File Offset: 0x0019DB36
		public static void Label(string text, float w, GUIStyle style)
		{
			if (style == null)
			{
				style = FitGUI.LabelStyle;
			}
			GUILayout.Label(text, style, new GUILayoutOption[] { FitGUI.Width(w) });
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x0019F958 File Offset: 0x0019DB58
		public static void Label(string text, float w, float h)
		{
			FitGUI.Label(text, w, h, null);
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x0019F963 File Offset: 0x0019DB63
		public static void Label(string text, float w, float h, GUIStyle style)
		{
			if (style == null)
			{
				style = FitGUI.LabelStyle;
			}
			GUILayout.Label(text, style, new GUILayoutOption[]
			{
				FitGUI.Width(w),
				FitGUI.Height(h)
			});
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x0019F98E File Offset: 0x0019DB8E
		public static bool Toggle(bool value, string text, float w = 200f, float h = 40f, GUIStyle style = null)
		{
			if (style == null)
			{
				style = FitGUI.LabelStyle;
			}
			return GUILayout.Toggle(value, text, FitGUI.ToggleStyle, new GUILayoutOption[]
			{
				FitGUI.Width(w),
				FitGUI.Height(h)
			});
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x0019F9BF File Offset: 0x0019DBBF
		public static bool Button(string text, float w = 200f, float h = 40f, GUIStyle style = null)
		{
			if (style == null)
			{
				style = FitGUI.ButtonStyle;
			}
			return GUILayout.Button(text, style, FitGUI.Size(w, h));
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x0019F9DC File Offset: 0x0019DBDC
		public static bool ButtonSmall(string text, float w = 50f, float h = 30f)
		{
			GUIStyle guistyle = FitGUI.MakeStyle(GUI.skin.button, FitGUI.fontSizeSmall);
			guistyle.fontStyle = FontStyle.Normal;
			return FitGUI.Button(text, w, h, guistyle);
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x0019FA10 File Offset: 0x0019DC10
		public static bool ButtonLeft(string text, float w = 200f, float h = 40f)
		{
			GUIStyle buttonStyle = FitGUI.ButtonStyle;
			buttonStyle.alignment = TextAnchor.MiddleLeft;
			return FitGUI.Button(text, w, h, buttonStyle);
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x0019FA33 File Offset: 0x0019DC33
		public static bool ButtonLabel(string label, string text, float w = 200f, float h = 40f)
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			FitGUI.LabelFit(label, null);
			bool flag = FitGUI.Button(text, w, h, null);
			GUILayout.EndHorizontal();
			return flag;
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x0019FA54 File Offset: 0x0019DC54
		public static void ButtonLabel(string label, ref bool flag, float w = 150f)
		{
			string text = "OFF";
			GUIStyle guistyle = null;
			if (flag)
			{
				text = "ON";
				guistyle = FitGUI.ButtonStyle;
				guistyle.normal.background = FitGUI.bgTexRed;
				guistyle.hover.background = FitGUI.bgTexRed;
			}
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (FitGUI.Button(text, 50f, 40f, guistyle))
			{
				flag = !flag;
			}
			FitGUI.Label(label, w);
			GUILayout.EndHorizontal();
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x0019FACC File Offset: 0x0019DCCC
		public static string TextField(string text, float w = 200f, float h = 30f)
		{
			return GUILayout.TextField(text, FitGUI.TextAreaStyle, FitGUI.Size(w, h));
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x0019FAE4 File Offset: 0x0019DCE4
		public static string TextFieldLabel(string label, string text, float w = 200f, float h = 30f)
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			string text2 = label;
			if (text2 == null)
			{
				text2 = "";
			}
			FitGUI.LabelFit(text2, null);
			text2 = text;
			if (text2 == null)
			{
				text2 = "";
			}
			text = GUILayout.TextField(text2, FitGUI.TextAreaStyle, FitGUI.Size(w, h));
			GUILayout.EndHorizontal();
			return text;
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x0019FB32 File Offset: 0x0019DD32
		public static int SelectionGrid(int selected, string[] texts, int xCount)
		{
			return GUILayout.SelectionGrid(selected, texts, xCount, FitGUI.SelectionGridStyle, Array.Empty<GUILayoutOption>());
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x0019FB48 File Offset: 0x0019DD48
		public static bool[] SelecMulitList(string label, bool[] selectList, Dictionary<int, string> itemDic, float w = -1f, int xCount = -1)
		{
			GUILayout.BeginVertical(GUI.skin.box, Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			int num = 0;
			FitGUI.LabelFit(label, null);
			num++;
			if (FitGUI.ButtonSmall("AllON", 50f, 30f))
			{
				for (int i = 0; i < selectList.Length; i++)
				{
					selectList[i] = true;
				}
			}
			if (FitGUI.ButtonSmall("AllOFF", 50f, 30f))
			{
				for (int j = 0; j < selectList.Length; j++)
				{
					selectList[j] = false;
				}
			}
			num++;
			for (int k = 0; k < itemDic.Count; k++)
			{
				if (xCount > 0 && num % xCount == 0)
				{
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				}
				string dictionaryValue = FitGUI.GetDictionaryValue(itemDic, k);
				if (w < 0f)
				{
					selectList[k] = GUILayout.Toggle(selectList[k], dictionaryValue, FitGUI.ToggleStyle, Array.Empty<GUILayoutOption>());
				}
				else
				{
					selectList[k] = GUILayout.Toggle(selectList[k], dictionaryValue, FitGUI.ToggleStyle, new GUILayoutOption[] { FitGUI.Width(w) });
				}
				num++;
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			return selectList;
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x0019FC5C File Offset: 0x0019DE5C
		public static void Separator(float height = 20f, bool noBar = false)
		{
			float num = height / 2f;
			GUILayout.Space(FitGUI.GetHeight(num));
			if (!noBar)
			{
				GUILayout.Button("", new GUILayoutOption[] { FitGUI.Height(2f) });
			}
			GUILayout.Space(FitGUI.GetHeight(num));
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x0019FCA8 File Offset: 0x0019DEA8
		public static int SelectLR(string label, int index, Dictionary<int, string> itemDic, float w = 200f)
		{
			bool flag = false;
			return FitGUI.SelectLRPush(label, index, itemDic, out flag, w);
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x0019FCC4 File Offset: 0x0019DEC4
		public static int SelectLRPush(string label, int index, Dictionary<int, string> itemDic, out bool push, float w = 300f)
		{
			push = false;
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			FitGUI.LabelFit(label, null);
			if (FitGUI.Button("＜", 50f, 30f, null))
			{
				index--;
				if (index < 0)
				{
					index = itemDic.Count - 1;
				}
			}
			int num = itemDic.Keys.ToArray<int>()[index];
			string text = itemDic[num];
			push = FitGUI.Button(text, w, 40f, null);
			if (FitGUI.Button("＞", 50f, 30f, null))
			{
				index++;
				if (index >= itemDic.Count)
				{
					index = 0;
				}
			}
			GUILayout.EndHorizontal();
			return index;
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x0019FD63 File Offset: 0x0019DF63
		public static int GetDictionaryKey(Dictionary<int, string> dic, int index)
		{
			return dic.Keys.ToArray<int>()[index];
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x0019FD74 File Offset: 0x0019DF74
		public static string GetDictionaryValue(Dictionary<int, string> dic, int index)
		{
			int[] array = dic.Keys.ToArray<int>();
			return dic[array[index]];
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x0019FD96 File Offset: 0x0019DF96
		public static float HorizontalSlider(float value, float minValue, float maxValue)
		{
			return GUILayout.HorizontalSlider(value, minValue, maxValue, FitGUI.HorizontalSliderStyle, FitGUI.HorizontalSliderThumbStyle, Array.Empty<GUILayoutOption>());
		}

		// Token: 0x04001BB9 RID: 7097
		public static float orgWidth = 640f;

		// Token: 0x04001BBA RID: 7098
		public static float orgHeight = 1136f;

		// Token: 0x04001BBB RID: 7099
		public static float marginWidth = 24f;

		// Token: 0x04001BBC RID: 7100
		public static float marginHeight = 0f;

		// Token: 0x04001BBD RID: 7101
		private static float fontSize = 15f;

		// Token: 0x04001BBE RID: 7102
		private static float fontSizeSmall = 10f;

		// Token: 0x04001BBF RID: 7103
		private static float fontSizeLarge = 20f;

		// Token: 0x04001BC0 RID: 7104
		private static float ratio;

		// Token: 0x04001BC1 RID: 7105
		private static float ratioWidth;

		// Token: 0x04001BC2 RID: 7106
		private static float ratioHeight;

		// Token: 0x04001BC3 RID: 7107
		private static Texture2D bgTexBlue;

		// Token: 0x04001BC4 RID: 7108
		private static Texture2D bgTexRed;

		// Token: 0x04001BC5 RID: 7109
		private static Texture2D bgTexGray;

		// Token: 0x04001BC6 RID: 7110
		private static Texture2D bgTexPink;

		// Token: 0x04001BC7 RID: 7111
		private static Texture2D bgTexGreen;
	}
}
