using System;
using UnityEngine;

[Serializable]
public class ActionEffectParam
{
	public string effectName;

	public bool useCharaLight;

	public float startOffsetTime;

	public CharaDef.EffectDispType dispType;

	public CharaDef.TargetNodeName dispModelNodeName;

	public float flightTime;

	public float hitHeight;

	public Vector3 offsetPosition;

	public Vector3 offsetRotation;

	public Vector3 offsetScale;

	public string hitEffectName;

	public float hitStartOffsetTime;

	public Vector3 hitOffsetPosition;

	public Vector3 hitOffsetRotation;

	public Vector3 hitOffsetScale;

	public float effectStartTime;
}
