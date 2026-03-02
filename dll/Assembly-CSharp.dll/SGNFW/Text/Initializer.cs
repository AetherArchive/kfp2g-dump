using System;
using UnityEngine;

namespace SGNFW.Text
{
	// Token: 0x02000242 RID: 578
	public class Initializer : MonoBehaviour
	{
		// Token: 0x0600246E RID: 9326 RVA: 0x0019C78C File Offset: 0x0019A98C
		private void Start()
		{
			Manager.Terminate();
			Manager.Initialize(string.Join("|", this.customTag));
			if (this.initialText)
			{
				Manager.SetJson(this.initialText.text);
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x04001B34 RID: 6964
		[SerializeField]
		private TextAsset initialText;

		// Token: 0x04001B35 RID: 6965
		[SerializeField]
		private string[] customTag;
	}
}
