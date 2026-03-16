using System;
using System.Collections.Generic;
using UnityEngine;

public class CharaStaticAlphaBase : ScriptableObject
{
	public int plasmPoint;

	public int hpParamLv1;

	public int hpParamLvMiddle;

	public int hpLvMiddleNum;

	public int hpParamLv99;

	public int atkParamLv1;

	public int atkParamLvMiddle;

	public int atkLvMiddleNum;

	public int atkParamLv99;

	public int defParamLv1;

	public int defParamLvMiddle;

	public int defLvMiddleNum;

	public int defParamLv99;

	public int maxStockMp;

	public int avoidRatio;

	public float width;

	public float height;

	public float AbnormalEffectHeadScale;

	public float AbnormalEffectHeadY;

	public float AbnormalEffectHeadZ;

	public float AbnormalEffectRootScale;

	public float AbnormalEffectRootY;

	public float AbnormalEffectRootZ;

	public bool isSpecialFlagSupported;

	public CharaDef.OrderCardType orderCardType00;

	public CharaDef.OrderCardType orderCardType01;

	public CharaDef.OrderCardType orderCardType02;

	public CharaDef.OrderCardType orderCardType03;

	public CharaDef.OrderCardType orderCardType04;

	public int orderCardValue00;

	public int orderCardValue01;

	public int orderCardValue02;

	public int orderCardValue03;

	public int orderCardValue04;

	public int orderCardSPValueMP;

	public int orderCardSPValuePlasm;

	public List<string> modelEffect;

	public float cameraOffsetX;

	public float cameraOffsetY;

	public float cameraOffsetZ;

	public float cameraRotationX;

	public float cameraRotationY;

	public float cameraRotationZ;

	public int entryLpNum;

	public string entryEffect;
}
