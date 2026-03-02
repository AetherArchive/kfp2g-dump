using System;
using System.Collections.Generic;
using CriWare;

namespace Battle
{
	// Token: 0x02000217 RID: 535
	public class SceneBattle_FriendsCommand : SceneBattle_Action
	{
		// Token: 0x04001968 RID: 6504
		public int mark;

		// Token: 0x04001969 RID: 6505
		public List<SceneBattle_Tag> kp;

		// Token: 0x0400196A RID: 6506
		public float dspKp;

		// Token: 0x0400196B RID: 6507
		public int wildPower;

		// Token: 0x0400196C RID: 6508
		public int wildChain;

		// Token: 0x0400196D RID: 6509
		public Dictionary<SceneBattle_FriendsCommand, SceneBattle_FriendsCommand> rmvCmd;

		// Token: 0x0400196E RID: 6510
		public List<SceneBattle_FriendsCommand> chainLst;

		// Token: 0x0400196F RID: 6511
		public List<SceneBattle_FriendsCommand> joinLst;

		// Token: 0x04001970 RID: 6512
		public int tickle;

		// Token: 0x04001971 RID: 6513
		public EffectData effTickle;

		// Token: 0x04001972 RID: 6514
		public CriAtomExPlayback seTickle;
	}
}
