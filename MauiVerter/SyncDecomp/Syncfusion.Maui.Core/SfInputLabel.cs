using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

internal class SfInputLabel : Label
{
	public SfInputLabel()
	{
		base.Style = new Style(typeof(SfInputLabel));
		base.TextColor = Colors.Black;
		base.FontAutoScalingEnabled = false;
	}
}
