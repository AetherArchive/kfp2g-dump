using System;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class DebugModelTestCtrl : MonoBehaviour
{
	// Token: 0x060000B4 RID: 180 RVA: 0x000062E6 File Offset: 0x000044E6
	private void Start()
	{
		if (this.charaAnimeCtrl != null)
		{
			this.charaAnimeCtrl.ExPlayAnimation("IDLING_LP_DHS_M_NOM", null);
		}
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00006307 File Offset: 0x00004507
	private void Update()
	{
	}

	// Token: 0x04000125 RID: 293
	public SimpleAnimation charaAnimeCtrl;
}
