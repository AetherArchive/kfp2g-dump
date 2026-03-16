using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugWebViewOpen : MonoBehaviour
{
	public void OnClickButton()
	{
		this.webViewWindowCtrl.Open(this.inputFiledYear.text);
	}

	public InputField inputFiledYear;

	public WebViewWindowCtrl webViewWindowCtrl;
}
