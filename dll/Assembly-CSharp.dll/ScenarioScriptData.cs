using System;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioScriptData : MonoBehaviour
{
	public string mTitleName;

	public bool ImpossibleSkip;

	public List<string> cueSheetList;

	public List<string> seSheetList;

	public List<string> effectSheetList;

	public List<ScenarioScriptData.CharacterData> charaDatas;

	public List<ScenarioScriptData.CharacterData> miraiDatas;

	public List<ScenarioScriptData.ScenarioRowData> rowDatas;

	[Serializable]
	public class CharacterData
	{
		public CharacterData(string model, string name)
		{
			this.model = model;
			this.name = name;
		}

		public string model;

		public string name;
	}

	[Serializable]
	public class ScenarioRowData
	{
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

		public ScenarioRowData()
		{
			this.Clear();
		}

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

		public ScenarioDefine.TYPE mType;

		public int mSerifCharaID;

		public string mSerifCharaName;

		public int arrayNum;

		public int[] ID;

		public int[] mCharaPosition;

		public ScenarioDefine.CHARA_MOVE[] mCharaMove;

		public CharaMotionDefine.ActKey[] mModelMotion;

		public ScenarioDefine.MOTION_FADE[] mMotionFade;

		public string[] mModelFaceId;

		public string[] mIdleFaceId;

		public int[] mCharaEffect;

		public bool[] mCharaFaceRot;

		public string[] mStrParams = new string[3];

		public int[] mIntParams = new int[5];

		public float[] mFloatParams = new float[3];
	}
}
