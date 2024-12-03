using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.Core;

internal abstract class SnapLayout : Layout
{
	protected override ILayoutManager CreateLayoutManager()
	{
		return new SnapLayoutManager(this);
	}

	internal abstract Size LayoutMeasure(double widthConstraint, double heightConstraint);

	internal abstract Size LayoutArrangeChildren(Rect bounds);
}
