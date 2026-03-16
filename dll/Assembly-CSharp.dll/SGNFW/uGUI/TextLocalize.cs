using System;
using SGNFW.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	[RequireComponent(typeof(Text))]
	public class TextLocalize : MonoBehaviour
	{
		public string Key
		{
			get
			{
				return this.key;
			}
			set
			{
				if (this.key != value)
				{
					this.key = value;
					this.modify = true;
				}
			}
		}

		private Text CachedText
		{
			get
			{
				Text text;
				if ((text = this.cachedText) == null)
				{
					text = (this.cachedText = base.GetComponent<Text>());
				}
				return text;
			}
		}

		private TextHyphenation CachedHyphenationText
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

		public void SetArgs(params string[] args)
		{
			this.args = args;
			this.modify = true;
		}

		private void UpdateText()
		{
			if (Manager.Instance == null || !Manager.IsSetup)
			{
				return;
			}
			if (this.CachedText is TextWithIcon)
			{
				TextManager.SetText((TextWithIcon)this.CachedText, this.key, this.args);
				return;
			}
			if (this.CachedText is TextWithRawImage)
			{
				TextManager.SetText((TextWithRawImage)this.CachedText, this.key, this.args);
				return;
			}
			object[] array;
			if (this.CachedHyphenationText != null)
			{
				TextHyphenation cachedHyphenationText = this.CachedHyphenationText;
				string text = this.key;
				array = this.args;
				cachedHyphenationText.Text = Manager.GetText(text, array);
				return;
			}
			Text text2 = this.CachedText;
			string text3 = this.key;
			array = this.args;
			text2.text = Manager.GetText(text3, array);
		}

		private void Awake()
		{
			this.UpdateText();
		}

		private void Update()
		{
			if (this.modify)
			{
				this.UpdateText();
				this.modify = false;
			}
		}

		[SerializeField]
		private string key;

		[SerializeField]
		private string[] args;

		private TextHyphenation cachedHyphenText;

		private Text cachedText;

		private bool modify;
	}
}
