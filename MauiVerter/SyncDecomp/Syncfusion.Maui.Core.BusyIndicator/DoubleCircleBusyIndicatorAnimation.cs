using Microsoft.Maui.Animations;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.BusyIndicator;

internal class DoubleCircleBusyIndicatorAnimation : BusyIndicatorAnimation
{
	private readonly float itemNumber = 4f;

	private float startAngle = 0f;

	private float endAngle = -75f;

	private float count = 15f;

	private readonly float innerStroke = 6f;

	private readonly float outerStroke = 8f;

	private double centerX1;

	private double centerY1;

	private double innerCenterX2;

	private double innerCenterY2;

	private double outerCenterX2;

	private double outerCenterY2;

	private double innerX2;

	private double innerY2;

	private double outerX2;

	private double outerY2;

	public DoubleCircleBusyIndicatorAnimation(IAnimationManager animationManagerValue, SfView view)
		: base(animationManagerValue)
	{
		defaultHeight = 75.0;
		defaultWidth = 75.0;
		CalculateXYPositions(view);
	}

	protected override void OnDrawAnimation(SfView view, ICanvas canvas)
	{
		base.OnDrawAnimation(view, canvas);
		canvas.StrokeColor = base.Color;
		for (int i = 0; (float)i < itemNumber; i++)
		{
			innerCenterX2 = innerX2;
			innerCenterY2 = innerY2;
			outerCenterX2 = outerX2;
			outerCenterY2 = outerY2;
			innerCenterX2 *= sizeFactor;
			innerCenterY2 *= sizeFactor;
			actualRect.X = centerX1 - innerCenterX2;
			actualRect.Y = centerY1 - innerCenterY2;
			actualRect.Width = defaultWidth * sizeFactor * 2.0 / 3.0;
			actualRect.Height = defaultHeight * sizeFactor * 2.0 / 3.0;
			canvas.StrokeSize = innerStroke * (float)sizeFactor;
			canvas.DrawArc(actualRect, 0f - (startAngle + count), 0f - (endAngle + count), clockwise: false, closed: false);
			outerCenterX2 *= sizeFactor;
			outerCenterY2 *= sizeFactor;
			actualRect.X = centerX1 - outerCenterY2;
			actualRect.Y = centerY1 - outerCenterY2;
			actualRect.Width = defaultWidth * sizeFactor;
			actualRect.Height = defaultHeight * sizeFactor;
			canvas.StrokeSize = outerStroke * (float)sizeFactor;
			canvas.DrawArc(actualRect, startAngle + count, endAngle + count, clockwise: true, closed: false);
			startAngle = endAngle - 15f;
			endAngle = startAngle - 75f;
		}
	}

	protected override void OnUpdateAnimation()
	{
		base.OnUpdateAnimation();
		count += 15f;
		if (count > 360f)
		{
			count = 15f;
		}
		drawableView?.InvalidateDrawable();
	}

	internal void CalculateXYPositions(SfView view)
	{
		if (view != null)
		{
			centerX1 = view.Width / 2.0;
			centerY1 = view.Height / 2.0;
			innerX2 = defaultWidth / 3.0;
			innerY2 = defaultHeight / 3.0;
			outerX2 = defaultWidth / 2.0;
			outerY2 = defaultHeight / 2.0;
		}
	}
}
