using System;

namespace SGNFW.Http
{
	[Flags]
	public enum ActionTypeMask
	{
		INVALID = 0,
		THROUGH = 1,
		MAINTE_WEB = 2,
		REFRESH = 4,
		RETRY = 8,
		STORE = 16,
		TITLE = 32,
		HOME = 64,
		SHUTDOWN = 128
	}
}
