using System;
using UnityEngine;
using UnityEngine.UI;

public class PguiPanel : Image
{
	protected override void Awake()
	{
		base.Awake();
		this.color = Color.clear;
	}

	public int PguiBehaviourVersion { get; set; }
}
