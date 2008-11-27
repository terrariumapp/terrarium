//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;

namespace Terrarium.Glass
{
	[Flags]
	public enum GlassBorders
	{
		None = 0,
		Left = 1,
		Top = 2,
		Right = 4,
		Bottom = 8,
		LeftAndRight = 5,
		TopAndBottom = 10,
		All = 15
	}
}
