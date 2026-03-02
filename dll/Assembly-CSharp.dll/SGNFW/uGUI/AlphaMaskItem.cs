using System;
using UnityEngine;

namespace SGNFW.uGUI
{
	// Token: 0x02000227 RID: 551
	public class AlphaMaskItem : MonoBehaviour
	{
		// Token: 0x0600231B RID: 8987 RVA: 0x0019620C File Offset: 0x0019440C
		public void SetAlphaMask(AlphaMask alphaMask)
		{
			this.alphaMask = alphaMask;
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x00196215 File Offset: 0x00194415
		public void Apply()
		{
			if (this.alphaMask == null)
			{
				return;
			}
			this.alphaMask.Apply(base.gameObject);
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x00196237 File Offset: 0x00194437
		private void Start()
		{
			if (this.alphaMask == null)
			{
				this.alphaMask = base.transform.GetComponentInParent<AlphaMask>();
			}
			this.Apply();
		}

		// Token: 0x04001A7D RID: 6781
		[SerializeField]
		private AlphaMask alphaMask;
	}
}
