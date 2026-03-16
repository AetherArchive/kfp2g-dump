using System;
using System.Collections.Generic;

public class CharaPromotePreset
{
	public int PromoteNum { get; set; }

	public static CharaPromotePreset MakeDummy(int depth)
	{
		return new CharaPromotePreset
		{
			promoteOneList = new List<CharaPromoteOne>
			{
				new CharaPromoteOne
				{
					promoteUseItemId = 14001,
					promoteUseItemNum = depth,
					costGoldNum = depth * 100
				},
				new CharaPromoteOne
				{
					promoteUseItemId = 14001,
					promoteUseItemNum = depth,
					costGoldNum = depth * 100
				},
				new CharaPromoteOne
				{
					promoteUseItemId = 14001,
					promoteUseItemNum = depth,
					costGoldNum = depth * 100
				},
				new CharaPromoteOne
				{
					promoteUseItemId = 14001,
					promoteUseItemNum = depth,
					costGoldNum = depth * 100
				},
				new CharaPromoteOne
				{
					promoteUseItemId = 14001,
					promoteUseItemNum = depth,
					costGoldNum = depth * 100
				},
				new CharaPromoteOne
				{
					promoteUseItemId = 14001,
					promoteUseItemNum = depth,
					costGoldNum = depth * 100
				}
			}
		};
	}

	public List<CharaPromoteOne> promoteOneList;
}
