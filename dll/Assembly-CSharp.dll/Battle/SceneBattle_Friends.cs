using System;
using UnityEngine;

namespace Battle
{
	public class SceneBattle_Friends : SceneBattle_Chara
	{
		public string iconName;

		public string iconNameInf;

		public Texture2D icon;

		public Vector3 commandCameraPos;

		public Vector3 commandCameraRot;

		public Vector3 chainCameraPos;

		public Vector3 chainCameraRot;

		public CharaStaticWaitSkill waitAction;

		public bool isWaitAct;

		public SceneBattle_CardInfo card;

		public int inBack;

		public int nowBack;

		public int waitCount;

		public int entNum;

		public float entEn;

		public float entLp;

		public string entEff;

		public float dspAtr;

		public EffectData effBarrier;

		public EffectData effWait;

		public float effWaitTime;

		public PrjUtil.ParamPreset boardParam;

		public int wildPower;

		public int tfBeforeId;
	}
}
