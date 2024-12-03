using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core;

internal class TooltipDrawableView : SfDrawableView
{
	private SfTooltip tooltip;

	protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
	{
		tooltip.Draw(canvas, dirtyRect);
	}

	internal TooltipDrawableView(SfTooltip sfTooltip)
	{
		tooltip = sfTooltip;
	}
}
