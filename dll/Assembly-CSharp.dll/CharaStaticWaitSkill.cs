using System;
using System.Collections.Generic;
using UnityEngine;

public class CharaStaticWaitSkill : ScriptableObject
{
	public string skillName;

	[Multiline(6)]
	public string skillEffect;

	public bool inBack = true;

	public bool atttackJoin;

	public int activationRate;

	public int activationNum;

	public string charaEffectName;

	public bool noJoint;

	public float charaEffectStartOffsetTime;

	public Vector3 charaEffectOffsetPosition;

	public Vector3 charaEffectOffsetRotation;

	public Vector3 charaEffectOffsetScale;

	public List<CharaBuffParam> buffList;

	public bool setEffectLayerToLoop;
}
