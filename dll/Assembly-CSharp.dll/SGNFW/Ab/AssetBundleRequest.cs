using System;
using UnityEngine;

namespace SGNFW.Ab
{
	public abstract class AssetBundleRequest : CustomYieldInstruction
	{
		public abstract Object[] allAssets { get; }

		public abstract Object asset { get; }

		public abstract bool isDone { get; }

		public abstract int priority { get; set; }

		public abstract float progress { get; }
	}
}
