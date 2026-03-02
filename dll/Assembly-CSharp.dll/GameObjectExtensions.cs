using System;
using UnityEngine;

// Token: 0x020000FA RID: 250
public static class GameObjectExtensions
{
	// Token: 0x06000C17 RID: 3095 RVA: 0x00047EF0 File Offset: 0x000460F0
	public static void SetLayerRecursively(this GameObject self, int layer)
	{
		self.layer = layer;
		foreach (object obj in self.transform)
		{
			((Transform)obj).gameObject.SetLayerRecursively(layer);
		}
	}
}
