using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F4 RID: 244
public class InputFieldDuplicateCtrl : MonoBehaviour
{
	// Token: 0x17000306 RID: 774
	// (get) Token: 0x06000BBA RID: 3002 RVA: 0x0004541F File Offset: 0x0004361F
	private int inputConfirmStrLen
	{
		get
		{
			if (!string.IsNullOrEmpty(this.inputConfirmString))
			{
				return this.inputConfirmString.Length;
			}
			return 0;
		}
	}

	// Token: 0x06000BBB RID: 3003 RVA: 0x0004543B File Offset: 0x0004363B
	[Conditional("UNITY_EDITOR_WIN")]
	[Conditional("UNITY_STANDALONE_WIN")]
	private void Start()
	{
		this.inputField = base.gameObject.GetComponent<InputField>();
	}

	// Token: 0x06000BBC RID: 3004 RVA: 0x00045450 File Offset: 0x00043650
	[Conditional("UNITY_EDITOR_WIN")]
	[Conditional("UNITY_STANDALONE_WIN")]
	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && !string.IsNullOrEmpty(this.inputField.text) && this.inputConfirmString != this.inputField.text)
		{
			if (this.inputConfirmStrLen + (this.selectAnchorPos - this.inputConfirmStrLen) * 2 == this.inputField.text.Length)
			{
				string text = this.inputField.text.Substring(this.inputConfirmStrLen);
				if (this.IsEvenNumWithoutZero(text.Length))
				{
					int length = text.Length;
					if (text.Substring(0, length / 2) == text.Substring(length / 2, length / 2))
					{
						this.inputField.text = this.inputField.text.Substring(0, this.inputConfirmStrLen) + text.Substring(0, length / 2);
					}
				}
			}
			else
			{
				int num = (this.inputField.text.Length - (this.inputConfirmStrLen + (this.selectAnchorPos - this.inputConfirmStrLen) * 2)) / 2;
				int num2 = this.inputConfirmStrLen - num;
				int num3 = this.inputField.text.Length - num;
				num3 = ((this.inputField.text.Length <= num3) ? 0 : num3);
				string text2 = this.inputField.text.Remove(num3);
				int num4 = num2;
				num4 = ((text2.Length <= num4) ? 0 : num4);
				string text3 = text2.Substring(num4);
				if (this.IsEvenNumWithoutZero(text3.Length))
				{
					int length2 = text3.Length;
					if (text3.Substring(0, length2 / 2) == text3.Substring(length2 / 2, length2 / 2))
					{
						this.inputField.text = this.inputField.text.Substring(0, num2) + text3.Substring(0, length2 / 2) + this.inputField.text.Substring(this.inputField.text.Length - num, num);
					}
				}
			}
		}
		if (this.inputConfirmString != this.inputField.text)
		{
			this.inputConfirmString = this.inputField.text;
		}
		if (this.selectAnchorPos != this.inputField.selectionAnchorPosition)
		{
			this.selectAnchorPos = this.inputField.selectionAnchorPosition;
		}
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x000456B1 File Offset: 0x000438B1
	private bool IsEvenNumWithoutZero(int num)
	{
		return 1 < num && num % 2 == 0;
	}

	// Token: 0x04000929 RID: 2345
	private InputField inputField;

	// Token: 0x0400092A RID: 2346
	private string inputConfirmString;

	// Token: 0x0400092B RID: 2347
	private int selectAnchorPos;
}
