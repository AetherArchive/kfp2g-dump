using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003C RID: 60
public class AuthConfigParam : MonoBehaviour
{
	// Token: 0x04000144 RID: 324
	public bool disableStageDisp;

	// Token: 0x04000145 RID: 325
	public int disableStageFrame;

	// Token: 0x04000146 RID: 326
	public bool noLoadEffect;

	// Token: 0x04000147 RID: 327
	public bool noLoadSound;

	// Token: 0x04000148 RID: 328
	public bool useNormalBodyParam;

	// Token: 0x04000149 RID: 329
	public bool disableAuthFaceMotion;

	// Token: 0x0400014A RID: 330
	public string targetCharaModel = "";

	// Token: 0x0400014B RID: 331
	public List<string> targetCharaModelList = new List<string>();
}
