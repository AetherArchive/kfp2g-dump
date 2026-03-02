using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public class CullingCheck : MonoBehaviour
{
	// Token: 0x06000B97 RID: 2967 RVA: 0x000446EA File Offset: 0x000428EA
	private void Start()
	{
		this.camList = new List<Camera>();
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x000446F7 File Offset: 0x000428F7
	private void LateUpdate()
	{
		this.camList = new List<Camera>();
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x00044704 File Offset: 0x00042904
	private void OnWillRenderObject()
	{
		this.camList.Add(Camera.current);
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x00044716 File Offset: 0x00042916
	public List<Camera> getCamera()
	{
		return this.camList;
	}

	// Token: 0x04000910 RID: 2320
	private List<Camera> camList = new List<Camera>();
}
