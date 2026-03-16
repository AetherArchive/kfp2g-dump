using System;
using UnityEngine;

public static class GameObjectExtensions
{
	public static void SetLayerRecursively(this GameObject self, int layer)
	{
		self.layer = layer;
		foreach (object obj in self.transform)
		{
			((Transform)obj).gameObject.SetLayerRecursively(layer);
		}
	}
}
