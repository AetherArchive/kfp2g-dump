using System;
using UnityEngine;

namespace SGNFW.uGUI
{
	// Token: 0x02000229 RID: 553
	public class Group : MonoBehaviour
	{
		// Token: 0x0600232A RID: 9002 RVA: 0x001964FB File Offset: 0x001946FB
		private void Awake()
		{
			base.enabled = false;
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x00196504 File Offset: 0x00194704
		private void OnDestroy()
		{
			Manager.RemoveGroupUI(this);
		}

		// Token: 0x04001A85 RID: 6789
		public string label;

		// Token: 0x04001A86 RID: 6790
		public Layer layer;

		// Token: 0x04001A87 RID: 6791
		public int order = int.MinValue;
	}
}
