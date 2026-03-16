using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldDuplicateCtrl : MonoBehaviour
{
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

	[Conditional("UNITY_EDITOR_WIN")]
	[Conditional("UNITY_STANDALONE_WIN")]
	private void Start()
	{
		this.inputField = base.gameObject.GetComponent<InputField>();
	}

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

	private bool IsEvenNumWithoutZero(int num)
	{
		return 1 < num && num % 2 == 0;
	}

	private InputField inputField;

	private string inputConfirmString;

	private int selectAnchorPos;
}
