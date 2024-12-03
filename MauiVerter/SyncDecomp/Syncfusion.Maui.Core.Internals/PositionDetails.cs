using Microsoft.UI.Xaml;

namespace Syncfusion.Maui.Core.Internals;

internal class PositionDetails
{
	internal FrameworkElement? Relative { get; set; }

	internal float X { get; set; }

	internal float Y { get; set; }

	internal WindowOverlayHorizontalAlignment HorizontalAlignment { get; set; }

	internal WindowOverlayVerticalAlignment VerticalAlignment { get; set; }
}
