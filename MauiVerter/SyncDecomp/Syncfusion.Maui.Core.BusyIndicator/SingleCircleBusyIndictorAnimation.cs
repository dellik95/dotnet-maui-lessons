using Microsoft.Maui.Animations;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.BusyIndicator;

internal class SingleCircleBusyIndictorAnimation : BusyIndicatorAnimation
{
	private readonly float itemNumber = 8f;

	private readonly float lineSize = 12f;

	private float startAngle = 0f;

	private float endAngle = -40f;

	private float count = 10f;

	public SingleCircleBusyIndictorAnimation(IAnimationManager animationManagerValue)
		: base(animationManagerValue)
	{
		defaultWidth = 75.0;
		defaultHeight = 75.0;
	}

	protected override void OnDrawAnimation(SfView view, ICanvas canvas)
	{
		base.OnDrawAnimation(view, canvas);
		canvas.StrokeSize = lineSize * (float)sizeFactor;
		canvas.StrokeColor = base.Color;
		for (int i = 0; (float)i < itemNumber; i++)
		{
			canvas.DrawArc(actualRect, startAngle - count, endAngle - count, clockwise: true, closed: false);
			startAngle = endAngle - 5f;
			endAngle = startAngle - 40f;
		}
	}

	protected override void OnUpdateAnimation()
	{
		base.OnUpdateAnimation();
		count += 1f;
		if (count > 360f)
		{
			count = 1f;
		}
		drawableView?.InvalidateDrawable();
	}
}
