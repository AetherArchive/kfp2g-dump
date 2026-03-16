using System;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioParty : ScriptableObject
{
	public ScenarioParty(List<ScenarioParty.Friends> friendsList)
	{
		this.friends = friendsList.ToArray();
	}

	public ScenarioParty.Friends[] friends = new ScenarioParty.Friends[1];

	[Serializable]
	public class Photo
	{
		public int id;

		public int level;

		public int limit;
	}

	[Serializable]
	public class Friends
	{
		public int id;

		public int clothItem;

		public int rank;

		public int level;

		public int kizunaLevel;

		public int yasei;

		public int miracleLevel;

		public bool miracleMax;

		public int dropItemId;

		public ScenarioParty.Photo[] photo = new ScenarioParty.Photo[4];

		public bool nanairoAbilityReleaseFlag;
	}
}
