using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000037 RID: 55
public class DebugWebViewOpen : MonoBehaviour
{
	// Token: 0x060000CA RID: 202 RVA: 0x000069E8 File Offset: 0x00004BE8
	public void OnClickButton()
	{
		this.webViewWindowCtrl.Open(this.inputFiledYear.text);
	}

	// Token: 0x04000133 RID: 307
	public InputField inputFiledYear;

	// Token: 0x04000134 RID: 308
	public WebViewWindowCtrl webViewWindowCtrl;
}
