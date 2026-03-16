using System;

public class EnemyDynamicData
{
	public int id { get; private set; }

	public int level { get; private set; }

	public int hpratio { get; private set; }

	public EnemyDynamicData(int id, int level, int hpratio)
	{
		this.id = id;
		this.level = level;
		this.hpratio = hpratio;
	}
}
