using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SGNFW.Common
{
	public class FitGUI
	{
		public static float width
		{
			get
			{
				return FitGUI.orgWidth - FitGUI.marginWidth;
			}
		}

		public static float height
		{
			get
			{
				return FitGUI.orgHeight - FitGUI.marginHeight;
			}
		}

		public static GUIStyle ButtonStyle
		{
			get
			{
				return FitGUI.MakeStyle(GUI.skin.button, FitGUI.fontSizeLarge);
			}
		}

		public static GUIStyle LabelStyle
		{
			get
			{
				return FitGUI.MakeStyle(GUI.skin.label, FitGUI.fontSize);
			}
		}

		public static GUIStyle TextAreaStyle
		{
			get
			{
				return FitGUI.MakeStyle(GUI.skin.textArea, FitGUI.fontSize);
			}
		}

		public static GUIStyle SelectionGridStyle
		{
			get
			{
				return FitGUI.MakeStyle(GUI.skin.button, FitGUI.fontSize);
			}
		}

		public static GUIStyle ToggleStyle
		{
			get
			{
				return FitGUI.MakeStyle(GUI.skin.toggle, FitGUI.fontSize);
			}
		}

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

		public static GUIStyle HorizontalSliderStyle
		{
			get
			{
				return FitGUI.MakeStyle(GUI.skin.horizontalSlider, FitGUI.fontSize);
			}
		}

		public static GUIStyle HorizontalSliderThumbStyle
		{
			get
			{
				return FitGUI.MakeStyle(GUI.skin.horizontalSliderThumb, FitGUI.fontSize);
			}
		}

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

		public static Texture2D CreateColorTexture(Color color)
		{
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.SetPixel(0, 0, color);
			texture2D.Apply();
			return texture2D;
		}

		public static Texture2D CreateColorTexture(float r, float g, float b, float a = 1f)
		{
			return FitGUI.CreateColorTexture(new Color(r, g, b, a));
		}

		public static float GetLength(float l)
		{
			return FitGUI.ratio * l;
		}

		public static float GetWidth(float w)
		{
			return FitGUI.ratioWidth * w;
		}

		public static float GetHeight(float h)
		{
			return FitGUI.ratioHeight * h;
		}

		public static GUILayoutOption Width(float w)
		{
			return GUILayout.Width(FitGUI.GetWidth(w));
		}

		public static GUILayoutOption Height(float h)
		{
			return GUILayout.Height(FitGUI.GetHeight(h));
		}

		public static GUILayoutOption[] Size(float w, float h)
		{
			return new GUILayoutOption[]
			{
				FitGUI.Width(w),
				FitGUI.Height(h)
			};
		}

		public static GUILayoutOption[] TextSize(string text)
		{
			Vector2 vector = FitGUI.LabelStyle.CalcSize(new GUIContent(text));
			return FitGUI.Size(vector.x, vector.y);
		}

		public static GUIStyle MakeStyle(GUIStyle org, float fontSize)
		{
			return new GUIStyle(org)
			{
				fontSize = (int)(fontSize * FitGUI.ratio),
				fontStyle = FontStyle.Bold
			};
		}

		public static void LabelFit(string text, GUIStyle style = null)
		{
			if (style == null)
			{
				style = FitGUI.LabelStyle;
			}
			GUILayout.Label(text, style, FitGUI.TextSize(text));
		}

		public static void LabelLarge(string text, float w = 200f)
		{
			GUIStyle guistyle = FitGUI.MakeStyle(GUI.skin.label, FitGUI.fontSizeLarge);
			FitGUI.Label(text, w, guistyle);
		}

		public static void Label(string text)
		{
			GUILayout.Label(text, FitGUI.LabelStyle, Array.Empty<GUILayoutOption>());
		}

		public static void Label(string text, float w)
		{
			FitGUI.Label(text, w, null);
		}

		public static void Label(string text, float w, GUIStyle style)
		{
			if (style == null)
			{
				style = FitGUI.LabelStyle;
			}
			GUILayout.Label(text, style, new GUILayoutOption[] { FitGUI.Width(w) });
		}

		public static void Label(string text, float w, float h)
		{
			FitGUI.Label(text, w, h, null);
		}

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

		public static bool Button(string text, float w = 200f, float h = 40f, GUIStyle style = null)
		{
			if (style == null)
			{
				style = FitGUI.ButtonStyle;
			}
			return GUILayout.Button(text, style, FitGUI.Size(w, h));
		}

		public static bool ButtonSmall(string text, float w = 50f, float h = 30f)
		{
			GUIStyle guistyle = FitGUI.MakeStyle(GUI.skin.button, FitGUI.fontSizeSmall);
			guistyle.fontStyle = FontStyle.Normal;
			return FitGUI.Button(text, w, h, guistyle);
		}

		public static bool ButtonLeft(string text, float w = 200f, float h = 40f)
		{
			GUIStyle buttonStyle = FitGUI.ButtonStyle;
			buttonStyle.alignment = TextAnchor.MiddleLeft;
			return FitGUI.Button(text, w, h, buttonStyle);
		}

		public static bool ButtonLabel(string label, string text, float w = 200f, float h = 40f)
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			FitGUI.LabelFit(label, null);
			bool flag = FitGUI.Button(text, w, h, null);
			GUILayout.EndHorizontal();
			return flag;
		}

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

		public static string TextField(string text, float w = 200f, float h = 30f)
		{
			return GUILayout.TextField(text, FitGUI.TextAreaStyle, FitGUI.Size(w, h));
		}

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

		public static int SelectionGrid(int selected, string[] texts, int xCount)
		{
			return GUILayout.SelectionGrid(selected, texts, xCount, FitGUI.SelectionGridStyle, Array.Empty<GUILayoutOption>());
		}

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

		public static int SelectLR(string label, int index, Dictionary<int, string> itemDic, float w = 200f)
		{
			bool flag = false;
			return FitGUI.SelectLRPush(label, index, itemDic, out flag, w);
		}

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

		public static int GetDictionaryKey(Dictionary<int, string> dic, int index)
		{
			return dic.Keys.ToArray<int>()[index];
		}

		public static string GetDictionaryValue(Dictionary<int, string> dic, int index)
		{
			int[] array = dic.Keys.ToArray<int>();
			return dic[array[index]];
		}

		public static float HorizontalSlider(float value, float minValue, float maxValue)
		{
			return GUILayout.HorizontalSlider(value, minValue, maxValue, FitGUI.HorizontalSliderStyle, FitGUI.HorizontalSliderThumbStyle, Array.Empty<GUILayoutOption>());
		}

		public static float orgWidth = 640f;

		public static float orgHeight = 1136f;

		public static float marginWidth = 24f;

		public static float marginHeight = 0f;

		private static float fontSize = 15f;

		private static float fontSizeSmall = 10f;

		private static float fontSizeLarge = 20f;

		private static float ratio;

		private static float ratioWidth;

		private static float ratioHeight;

		private static Texture2D bgTexBlue;

		private static Texture2D bgTexRed;

		private static Texture2D bgTexGray;

		private static Texture2D bgTexPink;

		private static Texture2D bgTexGreen;
	}
}
