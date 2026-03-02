using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000113 RID: 275
public class ScenarioScriptData : MonoBehaviour
{
	// Token: 0x04000AEA RID: 2794
	public string mTitleName;

	// Token: 0x04000AEB RID: 2795
	public bool ImpossibleSkip;

	// Token: 0x04000AEC RID: 2796
	public List<string> cueSheetList;

	// Token: 0x04000AED RID: 2797
	public List<string> seSheetList;

	// Token: 0x04000AEE RID: 2798
	public List<string> effectSheetList;

	// Token: 0x04000AEF RID: 2799
	public List<ScenarioScriptData.CharacterData> charaDatas;

	// Token: 0x04000AF0 RID: 2800
	public List<ScenarioScriptData.CharacterData> miraiDatas;

	// Token: 0x04000AF1 RID: 2801
	public List<ScenarioScriptData.ScenarioRowData> rowDatas;

	// Token: 0x02000878 RID: 2168
	[Serializable]
	public class CharacterData
	{
		// Token: 0x060038BD RID: 14525 RVA: 0x001CBC6C File Offset: 0x001C9E6C
		public CharacterData(string model, string name)
		{
			this.model = model;
			this.name = name;
		}

		// Token: 0x04003917 RID: 14615
		public string model;

		// Token: 0x04003918 RID: 14616
		public string name;
	}

	// Token: 0x02000879 RID: 2169
	[Serializable]
	public class ScenarioRowData
	{
		// Token: 0x060038BE RID: 14526 RVA: 0x001CBC84 File Offset: 0x001C9E84
		public void Clear()
		{
			this.mType = ScenarioDefine.TYPE.SERIF;
			this.mSerifCharaID = -1;
			this.mSerifCharaName = "";
			this.arrayNum = 0;
			this.ID = new int[0];
			this.mCharaPosition = new int[0];
			this.mCharaMove = new ScenarioDefine.CHARA_MOVE[0];
			this.mModelMotion = new CharaMotionDefine.ActKey[0];
			this.mMotionFade = new ScenarioDefine.MOTION_FADE[0];
			this.mModelFaceId = new string[0];
			this.mIdleFaceId = new string[0];
			this.mCharaEffect = new int[0];
			this.mCharaFaceRot = new bool[0];
			this.mStrParams = new string[] { "こんにちわ", "", "" };
			this.mIntParams = new int[5];
			this.mFloatParams = new float[3];
		}

		// Token: 0x060038BF RID: 14527 RVA: 0x001CBD59 File Offset: 0x001C9F59
		public ScenarioRowData()
		{
			this.Clear();
		}

		// Token: 0x060038C0 RID: 14528 RVA: 0x001CBD8C File Offset: 0x001C9F8C
		public ScenarioRowData(ScenarioScriptData.ScenarioRowData inData)
		{
			this.mType = inData.mType;
			this.mSerifCharaID = inData.mSerifCharaID;
			this.mSerifCharaName = inData.mSerifCharaName;
			this.arrayNum = inData.arrayNum;
			this.ID = new int[this.arrayNum];
			this.mCharaPosition = new int[this.arrayNum];
			this.mCharaMove = new ScenarioDefine.CHARA_MOVE[this.arrayNum];
			this.mModelMotion = new CharaMotionDefine.ActKey[this.arrayNum];
			this.mMotionFade = new ScenarioDefine.MOTION_FADE[this.arrayNum];
			this.mModelFaceId = new string[this.arrayNum];
			this.mIdleFaceId = new string[this.arrayNum];
			this.mCharaEffect = new int[this.arrayNum];
			this.mCharaFaceRot = new bool[this.arrayNum];
			for (int i = 0; i < this.arrayNum; i++)
			{
				this.ID[i] = inData.ID[i];
				this.mCharaPosition[i] = inData.mCharaPosition[i];
				this.mCharaMove[i] = inData.mCharaMove[i];
				this.mModelMotion[i] = inData.mModelMotion[i];
				this.mMotionFade[i] = inData.mMotionFade[i];
				this.mModelFaceId[i] = inData.mModelFaceId[i];
				this.mIdleFaceId[i] = inData.mIdleFaceId[i];
				this.mCharaEffect[i] = inData.mCharaEffect[i];
				this.mCharaFaceRot[i] = inData.mCharaFaceRot[i];
			}
			for (int j = 0; j < this.mStrParams.Length; j++)
			{
				this.mStrParams[j] = inData.mStrParams[j];
			}
			for (int k = 0; k < this.mIntParams.Length; k++)
			{
				this.mIntParams[k] = inData.mIntParams[k];
			}
			for (int l = 0; l < this.mFloatParams.Length; l++)
			{
				this.mFloatParams[l] = inData.mFloatParams[l];
			}
		}

		// Token: 0x04003919 RID: 14617
		public ScenarioDefine.TYPE mType;

		// Token: 0x0400391A RID: 14618
		public int mSerifCharaID;

		// Token: 0x0400391B RID: 14619
		public string mSerifCharaName;

		// Token: 0x0400391C RID: 14620
		public int arrayNum;

		// Token: 0x0400391D RID: 14621
		public int[] ID;

		// Token: 0x0400391E RID: 14622
		public int[] mCharaPosition;

		// Token: 0x0400391F RID: 14623
		public ScenarioDefine.CHARA_MOVE[] mCharaMove;

		// Token: 0x04003920 RID: 14624
		public CharaMotionDefine.ActKey[] mModelMotion;

		// Token: 0x04003921 RID: 14625
		public ScenarioDefine.MOTION_FADE[] mMotionFade;

		// Token: 0x04003922 RID: 14626
		public string[] mModelFaceId;

		// Token: 0x04003923 RID: 14627
		public string[] mIdleFaceId;

		// Token: 0x04003924 RID: 14628
		public int[] mCharaEffect;

		// Token: 0x04003925 RID: 14629
		public bool[] mCharaFaceRot;

		// Token: 0x04003926 RID: 14630
		public string[] mStrParams = new string[3];

		// Token: 0x04003927 RID: 14631
		public int[] mIntParams = new int[5];

		// Token: 0x04003928 RID: 14632
		public float[] mFloatParams = new float[3];
	}
}
