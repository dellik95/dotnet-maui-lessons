using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core;

internal class FontIconLabel : Label
{
	internal string? AvatarContentType { get; set; }

	public FontIconLabel()
	{
		base.Style = new Style(typeof(Label));
	}
}
