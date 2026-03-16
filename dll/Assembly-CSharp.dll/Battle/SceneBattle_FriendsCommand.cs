using System;
using System.Collections.Generic;
using CriWare;

namespace Battle
{
	public class SceneBattle_FriendsCommand : SceneBattle_Action
	{
		public int mark;

		public List<SceneBattle_Tag> kp;

		public float dspKp;

		public int wildPower;

		public int wildChain;

		public Dictionary<SceneBattle_FriendsCommand, SceneBattle_FriendsCommand> rmvCmd;

		public List<SceneBattle_FriendsCommand> chainLst;

		public List<SceneBattle_FriendsCommand> joinLst;

		public int tickle;

		public EffectData effTickle;

		public CriAtomExPlayback seTickle;
	}
}
