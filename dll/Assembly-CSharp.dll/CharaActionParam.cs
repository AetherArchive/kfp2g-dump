using System;
using UnityEngine;

[Serializable]
public class CharaActionParam
{
	public CharaDef.ActionDamageType attackType;

	public bool noLook;

	public Vector3 attackPositionOffset;

	public float quakeStartTime;

	public float quakeEndTime;

	public float quakeWidth;

	public int quakeNum;

	public CharaDef.MonitorQuakeType quakeType;

	public float voiceDelay;

	public CharaDef.ActionCameraType cameraType;

	public Vector3 skillCameraOffsetPosition;

	public Vector3 skillCameraOffsetTarget;
}
