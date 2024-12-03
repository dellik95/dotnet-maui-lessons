using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.Core;

internal class SnapLayoutManager : LayoutManager
{
	private SnapLayout layout;

	internal SnapLayoutManager(SnapLayout layout)
		: base(layout)
	{
		this.layout = layout;
	}

	public override Size Measure(double widthConstraint, double heightConstraint)
	{
		return layout.LayoutMeasure(widthConstraint, heightConstraint);
	}

	public override Size ArrangeChildren(Rect bounds)
	{
		return layout.LayoutArrangeChildren(bounds);
	}
}
