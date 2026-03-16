using System;
using UnityEngine;

[Serializable]
public class CharaMotionParam
{
	public float advanceTime;

	public float heightDifference;

	public float motionStartTime;

	public string charaEffectName;

	public bool useCharaLight;

	public bool jointMove;

	public float charaEffectStartOffsetTime;

	public Vector3 charaEffectOffsetPosition;

	public Vector3 charaEffectOffsetRotation;

	public Vector3 charaEffectOffsetScale;

	public float retreatTime;

	public float nextActionTime;

	public CharaMotionDefine.ActKey motionActKey;
}
