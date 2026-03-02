using System;
using SGNFW.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	// Token: 0x02000237 RID: 567
	[RequireComponent(typeof(Text))]
	public class TextLocalize : MonoBehaviour
	{
		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x060023BE RID: 9150 RVA: 0x00199C72 File Offset: 0x00197E72
		// (set) Token: 0x060023BF RID: 9151 RVA: 0x00199C7A File Offset: 0x00197E7A
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

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x060023C0 RID: 9152 RVA: 0x00199C98 File Offset: 0x00197E98
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

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x060023C1 RID: 9153 RVA: 0x00199CC0 File Offset: 0x00197EC0
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

		// Token: 0x060023C2 RID: 9154 RVA: 0x00199CE6 File Offset: 0x00197EE6
		public void SetArgs(params string[] args)
		{
			this.args = args;
			this.modify = true;
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x00199CF8 File Offset: 0x00197EF8
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

		// Token: 0x060023C4 RID: 9156 RVA: 0x00199DB3 File Offset: 0x00197FB3
		private void Awake()
		{
			this.UpdateText();
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x00199DBB File Offset: 0x00197FBB
		private void Update()
		{
			if (this.modify)
			{
				this.UpdateText();
				this.modify = false;
			}
		}

		// Token: 0x04001AE8 RID: 6888
		[SerializeField]
		private string key;

		// Token: 0x04001AE9 RID: 6889
		[SerializeField]
		private string[] args;

		// Token: 0x04001AEA RID: 6890
		private TextHyphenation cachedHyphenText;

		// Token: 0x04001AEB RID: 6891
		private Text cachedText;

		// Token: 0x04001AEC RID: 6892
		private bool modify;
	}
}
