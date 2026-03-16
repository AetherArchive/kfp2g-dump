using System;
using System.Collections.Generic;
using SGNFW.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	public class TextManager
	{
		private TextManager(Icon[] iconArray, TextManager.Color[] colorArray)
		{
			TextManager.iconArray = iconArray;
			TextManager.colorArray = colorArray;
		}

		public static void Initialize(Icon[] sprArray, TextManager.Color[] clrArray)
		{
			if (TextManager.instance != null)
			{
				return;
			}
			TextManager.instance = new TextManager(sprArray, clrArray);
		}

		public static void Terminate()
		{
			TextManager.instance = null;
		}

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

		public static bool ContainsIcon(string iconName)
		{
			return Array.Exists<Icon>(TextManager.iconArray, (Icon t) => t.sprite.name == iconName);
		}

		public static bool ContainsIcon(Sprite icon)
		{
			return Array.Exists<Icon>(TextManager.iconArray, (Icon t) => t.sprite == icon);
		}

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

		public static Action<TextWithRawImage, RawImage, string> onCreateRawImage;

		public static Action<TextWithIcon, Image, Icon> onCreateImage;

		public static Action<TextWithRawImage, RawImage> onUpdateRawImage;

		public static Action<TextWithIcon, Image> onUpdateImage;

		private static TextManager instance;

		private static List<string> iconList = new List<string>();

		private static Icon[] iconArray = null;

		private static TextManager.Color[] colorArray = null;

		[Serializable]
		public class Color
		{
			public string key;

			public global::UnityEngine.Color value = global::UnityEngine.Color.white;
		}
	}
}
