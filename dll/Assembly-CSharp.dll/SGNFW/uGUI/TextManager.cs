using System;
using System.Collections.Generic;
using SGNFW.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	// Token: 0x02000238 RID: 568
	public class TextManager
	{
		// Token: 0x060023C7 RID: 9159 RVA: 0x00199DDA File Offset: 0x00197FDA
		private TextManager(Icon[] iconArray, TextManager.Color[] colorArray)
		{
			TextManager.iconArray = iconArray;
			TextManager.colorArray = colorArray;
		}

		// Token: 0x060023C8 RID: 9160 RVA: 0x00199DEE File Offset: 0x00197FEE
		public static void Initialize(Icon[] sprArray, TextManager.Color[] clrArray)
		{
			if (TextManager.instance != null)
			{
				return;
			}
			TextManager.instance = new TextManager(sprArray, clrArray);
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x00199E04 File Offset: 0x00198004
		public static void Terminate()
		{
			TextManager.instance = null;
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x00199E0C File Offset: 0x0019800C
		public static void SetText(TextWithIcon text, string key, params string[] args)
		{
			Manager.OnCustomKeyword = new Manager.OnCustomKeywordDelegate(TextManager.instance.OnCustomKeyword);
			TextManager.iconList.Clear();
			text.onChange = delegate(TextWithIcon _text)
			{
				using (List<string>.Enumerator enumerator = TextManager.iconList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string name = enumerator.Current;
						Icon icon = Array.Find<Icon>(TextManager.iconArray, (Icon t) => t.sprite.name == name);
						if (icon != null)
						{
							TextManager.CreateImage(icon, _text);
						}
					}
				}
			};
			if (text.CachedHyphenText != null)
			{
				text.CachedHyphenText.Text = Manager.GetText(key, args);
			}
			else
			{
				text.text = Manager.GetText(key, args);
			}
			text.onChange = null;
			Manager.OnCustomKeyword = null;
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x00199EA0 File Offset: 0x001980A0
		public static void SetTextRaw(TextWithIcon text, string txt)
		{
			Manager.OnCustomKeyword = new Manager.OnCustomKeywordDelegate(TextManager.instance.OnCustomKeyword);
			TextManager.iconList.Clear();
			text.onChange = delegate(TextWithIcon _text)
			{
				using (List<string>.Enumerator enumerator = TextManager.iconList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string name = enumerator.Current;
						Icon icon = Array.Find<Icon>(TextManager.iconArray, (Icon t) => t.sprite.name == name);
						if (icon != null)
						{
							TextManager.CreateImage(icon, _text);
						}
					}
				}
			};
			if (text.CachedHyphenText != null)
			{
				text.CachedHyphenText.Text = Manager.Process(txt);
			}
			else
			{
				text.text = Manager.Process(txt);
			}
			text.onChange = null;
			Manager.OnCustomKeyword = null;
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x00199F2C File Offset: 0x0019812C
		public static void AddTextRaw(TextWithIcon text, string txt)
		{
			Manager.OnCustomKeyword = new Manager.OnCustomKeywordDelegate(TextManager.instance.OnCustomKeyword);
			TextManager.iconList.Clear();
			text.onChange = delegate(TextWithIcon _text)
			{
				using (List<string>.Enumerator enumerator = TextManager.iconList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string name = enumerator.Current;
						Icon icon = Array.Find<Icon>(TextManager.iconArray, (Icon t) => t.sprite.name == name);
						if (icon != null)
						{
							TextManager.CreateImage(icon, _text);
						}
					}
				}
			};
			if (text.CachedHyphenText != null)
			{
				TextHyphenation cachedHyphenText = text.CachedHyphenText;
				cachedHyphenText.Text += Manager.Process(txt);
			}
			else
			{
				text.AddTexts(Manager.Process(txt));
			}
			text.onChange = null;
			Manager.OnCustomKeyword = null;
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x00199FC4 File Offset: 0x001981C4
		public static void SetText(TextWithRawImage text, string key, params string[] args)
		{
			Manager.OnCustomKeyword = new Manager.OnCustomKeywordDelegate(TextManager.instance.OnCustomKeyword);
			TextManager.iconList.Clear();
			text.onChange = delegate(TextWithRawImage _text)
			{
				foreach (string text2 in TextManager.iconList)
				{
					TextManager.CreateRawImage(text2, _text);
				}
			};
			if (text.CachedHyphenText != null)
			{
				text.CachedHyphenText.Text = Manager.GetText(key, args);
			}
			else
			{
				text.text = Manager.GetText(key, args);
			}
			text.onChange = null;
			Manager.OnCustomKeyword = null;
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x0019A058 File Offset: 0x00198258
		public static void SetTextRaw(TextWithRawImage text, string txt)
		{
			Manager.OnCustomKeyword = new Manager.OnCustomKeywordDelegate(TextManager.instance.OnCustomKeyword);
			TextManager.iconList.Clear();
			text.onChange = delegate(TextWithRawImage _text)
			{
				foreach (string text2 in TextManager.iconList)
				{
					TextManager.CreateRawImage(text2, _text);
				}
			};
			if (text.CachedHyphenText != null)
			{
				text.CachedHyphenText.Text = Manager.Process(txt);
			}
			else
			{
				text.text = Manager.Process(txt);
			}
			text.onChange = null;
			Manager.OnCustomKeyword = null;
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x0019A0E4 File Offset: 0x001982E4
		public static void AddTextRaw(TextWithRawImage text, string txt)
		{
			Manager.OnCustomKeyword = new Manager.OnCustomKeywordDelegate(TextManager.instance.OnCustomKeyword);
			TextManager.iconList.Clear();
			text.onChange = delegate(TextWithRawImage _text)
			{
				foreach (string text2 in TextManager.iconList)
				{
					TextManager.CreateRawImage(text2, _text);
				}
			};
			if (text.CachedHyphenText != null)
			{
				TextHyphenation cachedHyphenText = text.CachedHyphenText;
				cachedHyphenText.Text += Manager.Process(txt);
			}
			else
			{
				text.AddTexts(Manager.Process(txt));
			}
			text.onChange = null;
			Manager.OnCustomKeyword = null;
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x0019A17C File Offset: 0x0019837C
		public static bool ContainsIcon(string iconName)
		{
			return Array.Exists<Icon>(TextManager.iconArray, (Icon t) => t.sprite.name == iconName);
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x0019A1AC File Offset: 0x001983AC
		public static bool ContainsIcon(Sprite icon)
		{
			return Array.Exists<Icon>(TextManager.iconArray, (Icon t) => t.sprite == icon);
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x0019A1DC File Offset: 0x001983DC
		private string OnCustomKeyword(string body, int index, int length, string tag, string key)
		{
			if (key.StartsWith("icon="))
			{
				body = body.Remove(index, length);
				body = body.Insert(index, "<color=#00000000>[+]</color>");
				string text = key.Remove(0, "icon=".Length);
				if (!string.IsNullOrEmpty(text))
				{
					TextManager.iconList.Add(text);
				}
			}
			else if (key.StartsWith("color="))
			{
				Func<global::UnityEngine.Color, int> func = delegate(global::UnityEngine.Color _clr)
				{
					int num = 0;
					Color32 color2 = _clr;
					return num | ((int)color2.r * 16777216) | ((int)color2.g * 65536) | ((int)color2.b * 256) | (int)color2.a;
				};
				string name = key.Remove(0, "color=".Length);
				if (name == "end")
				{
					body = body.Remove(index, length);
					body = body.Insert(index, "</color>");
				}
				else
				{
					TextManager.Color color = Array.Find<TextManager.Color>(TextManager.colorArray, (TextManager.Color t) => t.key == name);
					if (color != null)
					{
						body = body.Remove(index, length);
						body = body.Insert(index, string.Format("<color=#{0:x8}>", func(color.value)));
					}
				}
			}
			return body;
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x0019A304 File Offset: 0x00198504
		private static Image CreateImage(Icon icon, TextWithIcon txt)
		{
			if (icon == null)
			{
				return null;
			}
			GameObject gameObject = new GameObject(icon.sprite.name);
			gameObject.transform.SetParent(txt.transform);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = new Vector3(icon.scale, icon.scale, 1f);
			Image image = gameObject.AddComponent<Image>();
			if (icon.material != null)
			{
				image.material = icon.material;
			}
			image.sprite = icon.sprite;
			image.enabled = false;
			if (TextManager.onCreateImage != null)
			{
				TextManager.onCreateImage(txt, image, icon);
			}
			txt.onUpdate = delegate(TextWithIcon _txt, Image _img)
			{
				if (TextManager.onUpdateImage != null)
				{
					TextManager.onUpdateImage(_txt, _img);
				}
			};
			return image;
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x0019A3E8 File Offset: 0x001985E8
		private static RawImage CreateRawImage(string name, TextWithRawImage txt)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			GameObject gameObject = new GameObject(name);
			gameObject.transform.SetParent(txt.transform);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			RawImage rawImage = gameObject.AddComponent<RawImage>();
			rawImage.uvRect = txt.UvRect;
			if (txt.Mat != null)
			{
				rawImage.material = txt.Mat;
			}
			rawImage.enabled = false;
			if (TextManager.onCreateRawImage != null)
			{
				TextManager.onCreateRawImage(txt, rawImage, name);
			}
			txt.onUpdate = delegate(TextWithRawImage _txt, RawImage _rimg)
			{
				if (TextManager.onUpdateRawImage != null)
				{
					TextManager.onUpdateRawImage(_txt, _rimg);
				}
			};
			return rawImage;
		}

		// Token: 0x04001AED RID: 6893
		public static Action<TextWithRawImage, RawImage, string> onCreateRawImage;

		// Token: 0x04001AEE RID: 6894
		public static Action<TextWithIcon, Image, Icon> onCreateImage;

		// Token: 0x04001AEF RID: 6895
		public static Action<TextWithRawImage, RawImage> onUpdateRawImage;

		// Token: 0x04001AF0 RID: 6896
		public static Action<TextWithIcon, Image> onUpdateImage;

		// Token: 0x04001AF1 RID: 6897
		private static TextManager instance;

		// Token: 0x04001AF2 RID: 6898
		private static List<string> iconList = new List<string>();

		// Token: 0x04001AF3 RID: 6899
		private static Icon[] iconArray = null;

		// Token: 0x04001AF4 RID: 6900
		private static TextManager.Color[] colorArray = null;

		// Token: 0x0200106F RID: 4207
		[Serializable]
		public class Color
		{
			// Token: 0x04005BC5 RID: 23493
			public string key;

			// Token: 0x04005BC6 RID: 23494
			public global::UnityEngine.Color value = global::UnityEngine.Color.white;
		}
	}
}
