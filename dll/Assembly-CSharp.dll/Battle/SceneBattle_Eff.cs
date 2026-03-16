using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
	public class SceneBattle_Eff
	{
		public SceneBattle_Eff()
		{
			this.tag = null;
			this.tim = 0f;
			this.nam = null;
			this.fly = -1f;
			this.height = -1f;
			this.node = CharaDef.TargetNodeName.root;
			this.pos = Vector3.zero;
			this.rot = Vector3.zero;
			this.scl = Vector3.one;
			this.hittag = null;
		}

		public List<SceneBattle_Chara> tag;

		public float tim;

		public string nam;

		public bool light;

		public float fly;

		public float height;

		public CharaDef.TargetNodeName node;

		public Vector3 pos;

		public Vector3 rot;

		public Vector3 scl;

		public List<SceneBattle_Tag> hittag;
	}
}
