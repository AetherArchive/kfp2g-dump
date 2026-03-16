using System;
using System.Collections.Generic;
using UnityEngine;

public class CullingCheck : MonoBehaviour
{
	private void Start()
	{
		this.camList = new List<Camera>();
	}

	private void LateUpdate()
	{
		this.camList = new List<Camera>();
	}

	private void OnWillRenderObject()
	{
		this.camList.Add(Camera.current);
	}

	public List<Camera> getCamera()
	{
		return this.camList;
	}

	private List<Camera> camList = new List<Camera>();
}
