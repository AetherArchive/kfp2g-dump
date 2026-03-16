using System;
using System.Collections.Generic;
using UnityEngine;

public class AuthConfigParam : MonoBehaviour
{
	public bool disableStageDisp;

	public int disableStageFrame;

	public bool noLoadEffect;

	public bool noLoadSound;

	public bool useNormalBodyParam;

	public bool disableAuthFaceMotion;

	public string targetCharaModel = "";

	public List<string> targetCharaModelList = new List<string>();
}
