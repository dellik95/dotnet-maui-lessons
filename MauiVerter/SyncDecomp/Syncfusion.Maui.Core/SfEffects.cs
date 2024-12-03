using System;

namespace Syncfusion.Maui.Core;

[Flags]
public enum SfEffects
{
	None = 1,
	Highlight = 2,
	Ripple = 4,
	Scale = 8,
	Selection = 0x10,
	Rotation = 0x20
}
