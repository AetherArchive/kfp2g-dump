using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003B RID: 59
public class AuthCharaData
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x060000D6 RID: 214 RVA: 0x00006C8C File Offset: 0x00004E8C
	// (set) Token: 0x060000D7 RID: 215 RVA: 0x00006C94 File Offset: 0x00004E94
	public int Index { get; set; }

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x060000D8 RID: 216 RVA: 0x00006C9D File Offset: 0x00004E9D
	// (set) Token: 0x060000D9 RID: 217 RVA: 0x00006CA5 File Offset: 0x00004EA5
	public CharaModelHandle charaModelHandle { get; set; }

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x060000DA RID: 218 RVA: 0x00006CAE File Offset: 0x00004EAE
	// (set) Token: 0x060000DB RID: 219 RVA: 0x00006CB6 File Offset: 0x00004EB6
	public GameObject Parent { get; set; }

	// Token: 0x060000DC RID: 220 RVA: 0x00006CC0 File Offset: 0x00004EC0
	public AuthCharaData(int index, string charaModelName, bool isModelShadow)
	{
		this.Index = index;
		this.charaModelHandle = new GameObject
		{
			name = "AUTH_CHARA_DATA"
		}.AddComponent<CharaModelHandle>();
		this.charaModelHandle.Initialize(new CharaModelHandle.InitializeParam(charaModelName, false, false, isModelShadow, LayerMask.NameToLayer("AuthMain")));
		this.charaModelHandle.SetModelActive(false);
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00006D24 File Offset: 0x00004F24
	public AuthCharaData(int index, CharaModelHandle.InitializeParam cmd, bool isModelShadow)
	{
		this.Index = index;
		this.charaModelHandle = new GameObject
		{
			name = "AUTH_CHARA_DATA"
		}.AddComponent<CharaModelHandle>();
		this.charaModelHandle.Initialize(new CharaModelHandle.InitializeParam(cmd.bodyModelName, false, false, isModelShadow, LayerMask.NameToLayer("AuthMain")));
		this.charaModelHandle.SetModelActive(false);
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00006D8A File Offset: 0x00004F8A
	public void Destory()
	{
		this.charaModelHandle.DestoryInternal();
	}

	// Token: 0x0400013A RID: 314
	public static readonly string AUTH_CHARA_PREFIX = "CHARA_";

	// Token: 0x0400013B RID: 315
	public static readonly string AUTH_BLENDSHAPE_PREFIX = "Bs_";

	// Token: 0x0400013F RID: 319
	public List<Transform> AuthObj;

	// Token: 0x04000140 RID: 320
	public float offset;

	// Token: 0x04000141 RID: 321
	public float start;

	// Token: 0x04000142 RID: 322
	public float speed;

	// Token: 0x04000143 RID: 323
	public float end;
}
