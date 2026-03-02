using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class CharaStaticAlphaBase : ScriptableObject
{
	// Token: 0x040003BE RID: 958
	public int plasmPoint;

	// Token: 0x040003BF RID: 959
	public int hpParamLv1;

	// Token: 0x040003C0 RID: 960
	public int hpParamLvMiddle;

	// Token: 0x040003C1 RID: 961
	public int hpLvMiddleNum;

	// Token: 0x040003C2 RID: 962
	public int hpParamLv99;

	// Token: 0x040003C3 RID: 963
	public int atkParamLv1;

	// Token: 0x040003C4 RID: 964
	public int atkParamLvMiddle;

	// Token: 0x040003C5 RID: 965
	public int atkLvMiddleNum;

	// Token: 0x040003C6 RID: 966
	public int atkParamLv99;

	// Token: 0x040003C7 RID: 967
	public int defParamLv1;

	// Token: 0x040003C8 RID: 968
	public int defParamLvMiddle;

	// Token: 0x040003C9 RID: 969
	public int defLvMiddleNum;

	// Token: 0x040003CA RID: 970
	public int defParamLv99;

	// Token: 0x040003CB RID: 971
	public int maxStockMp;

	// Token: 0x040003CC RID: 972
	public int avoidRatio;

	// Token: 0x040003CD RID: 973
	public float width;

	// Token: 0x040003CE RID: 974
	public float height;

	// Token: 0x040003CF RID: 975
	public float AbnormalEffectHeadScale;

	// Token: 0x040003D0 RID: 976
	public float AbnormalEffectHeadY;

	// Token: 0x040003D1 RID: 977
	public float AbnormalEffectHeadZ;

	// Token: 0x040003D2 RID: 978
	public float AbnormalEffectRootScale;

	// Token: 0x040003D3 RID: 979
	public float AbnormalEffectRootY;

	// Token: 0x040003D4 RID: 980
	public float AbnormalEffectRootZ;

	// Token: 0x040003D5 RID: 981
	public bool isSpecialFlagSupported;

	// Token: 0x040003D6 RID: 982
	public CharaDef.OrderCardType orderCardType00;

	// Token: 0x040003D7 RID: 983
	public CharaDef.OrderCardType orderCardType01;

	// Token: 0x040003D8 RID: 984
	public CharaDef.OrderCardType orderCardType02;

	// Token: 0x040003D9 RID: 985
	public CharaDef.OrderCardType orderCardType03;

	// Token: 0x040003DA RID: 986
	public CharaDef.OrderCardType orderCardType04;

	// Token: 0x040003DB RID: 987
	public int orderCardValue00;

	// Token: 0x040003DC RID: 988
	public int orderCardValue01;

	// Token: 0x040003DD RID: 989
	public int orderCardValue02;

	// Token: 0x040003DE RID: 990
	public int orderCardValue03;

	// Token: 0x040003DF RID: 991
	public int orderCardValue04;

	// Token: 0x040003E0 RID: 992
	public int orderCardSPValueMP;

	// Token: 0x040003E1 RID: 993
	public int orderCardSPValuePlasm;

	// Token: 0x040003E2 RID: 994
	public List<string> modelEffect;

	// Token: 0x040003E3 RID: 995
	public float cameraOffsetX;

	// Token: 0x040003E4 RID: 996
	public float cameraOffsetY;

	// Token: 0x040003E5 RID: 997
	public float cameraOffsetZ;

	// Token: 0x040003E6 RID: 998
	public float cameraRotationX;

	// Token: 0x040003E7 RID: 999
	public float cameraRotationY;

	// Token: 0x040003E8 RID: 1000
	public float cameraRotationZ;

	// Token: 0x040003E9 RID: 1001
	public int entryLpNum;

	// Token: 0x040003EA RID: 1002
	public string entryEffect;
}
