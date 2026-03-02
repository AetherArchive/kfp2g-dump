using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000192 RID: 402
public class BirthdayWindowCtrl : MonoBehaviour
{
	// Token: 0x02000E85 RID: 3717
	public class GUI
	{
		// Token: 0x06004CEF RID: 19695 RVA: 0x0022FC1C File Offset: 0x0022DE1C
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.openWindow = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.inputFiledYear = baseTr.Find("Base/Window/Input_Base/InputField_L").GetComponent<InputField>();
			this.inputFiledMonth = baseTr.Find("Base/Window/Input_Base/InputField_R").GetComponent<InputField>();
		}

		// Token: 0x04005368 RID: 21352
		public GameObject baseObj;

		// Token: 0x04005369 RID: 21353
		public PguiOpenWindowCtrl openWindow;

		// Token: 0x0400536A RID: 21354
		public InputField inputFiledYear;

		// Token: 0x0400536B RID: 21355
		public InputField inputFiledMonth;
	}
}
