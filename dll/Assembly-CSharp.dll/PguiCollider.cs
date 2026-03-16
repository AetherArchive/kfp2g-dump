using System;
using UnityEngine;
using UnityEngine.UI;

public class PguiCollider : Graphic
{
	protected override void Awake()
	{
		this.color = Color.clear;
		if (base.gameObject.GetComponent<CanvasRenderer>() == null)
		{
			base.gameObject.AddComponent<CanvasRenderer>();
		}
	}
}
