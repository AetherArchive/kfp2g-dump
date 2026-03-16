using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class Picnic
	{
		public Assets assets;

		public List<PicnicChara> charalist;

		public int energy;

		public List<PicnicPlay> playlist;
	}
}
