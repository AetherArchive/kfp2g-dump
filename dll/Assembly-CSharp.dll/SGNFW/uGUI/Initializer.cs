using System;
using UnityEngine;

namespace SGNFW.uGUI
{
	// Token: 0x0200022B RID: 555
	public class Initializer : MonoBehaviour
	{
		// Token: 0x0600232E RID: 9006 RVA: 0x00196532 File Offset: 0x00194732
		private void Awake()
		{
			Manager.Terminate();
			Manager.Initialize(base.transform, this.cam);
			TextManager.Terminate();
			TextManager.Initialize(this.iconArray, this.colorArray);
			Object.Destroy(this);
		}

		// Token: 0x04001A8B RID: 6795
		[SerializeField]
		private Camera cam;

		// Token: 0x04001A8C RID: 6796
		[SerializeField]
		private Icon[] iconArray;

		// Token: 0x04001A8D RID: 6797
		[SerializeField]
		private TextManager.Color[] colorArray;
	}
}
