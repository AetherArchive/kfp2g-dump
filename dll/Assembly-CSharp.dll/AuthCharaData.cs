using System;
using System.Collections.Generic;
using UnityEngine;

public class AuthCharaData
{
	public int Index { get; set; }

	public CharaModelHandle charaModelHandle { get; set; }

	public GameObject Parent { get; set; }

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

	public void Destory()
	{
		this.charaModelHandle.DestoryInternal();
	}

	public static readonly string AUTH_CHARA_PREFIX = "CHARA_";

	public static readonly string AUTH_BLENDSHAPE_PREFIX = "Bs_";

	public List<Transform> AuthObj;

	public float offset;

	public float start;

	public float speed;

	public float end;
}
