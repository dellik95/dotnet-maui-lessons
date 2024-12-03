using Microsoft.Maui.Animations;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

internal class CircularMaterialBusyIndicatorAnimation : BusyIndicatorAnimation
{
	private float materialStartAngle = 0f;

	private float materialEndAngle = -30f;

	private readonly float stepLength = 7f;

	private readonly float maximumStepLength = 30f;

	private float arcLength = 30f;

	private bool isCollapsing = false;

	private float easingValue = 0.1f;

	private bool isReset = false;

	private readonly float strokeSize = 10f;

	private readonly float maximumArcLength = 330f;

	private readonly float minimumArchLength = 120f;

	public CircularMaterialBusyIndicatorAnimation(IAnimationManager animationManagerValue)
		: base(animationManagerValue)
	{
		base.DefaultDuration = 50.0;
		defaultHeight = 75.0;
		defaultWidth = 75.0;
	}

	protected override void OnDrawAnimation(SfView view, ICanvas canvas)
	{
		base.OnDrawAnimation(view, canvas);
		canvas.StrokeColor = base.Color;
		canvas.StrokeSize = strokeSize * (float)sizeFactor;
		canvas.DrawArc(actualRect, materialStartAngle, materialEndAngle, clockwise: false, closed: false);
	}

	protected override void OnUpdateAnimation()
	{
		base.OnUpdateAnimation();
		UpdateAnimation();
		drawableView?.InvalidateDrawable();
	}

	private void CheckArcLength()
	{
		if (arcLength < minimumArchLength)
		{
			isReset = isCollapsing;
			isCollapsing = true;
		}
		else if (arcLength > maximumArcLength)
		{
			isReset = isCollapsing;
			isCollapsing = false;
		}
	}

	private void CheckEasingValue()
	{
		if (isReset != isCollapsing)
		{
			isReset = isCollapsing;
			easingValue = 0.1f;
		}
		easingValue += easingValue;
		if (easingValue > maximumStepLength)
		{
			easingValue = maximumStepLength;
		}
	}

	private void UpdateAnimation()
	{
		materialStartAngle -= stepLength;
		materialEndAngle -= stepLength;
		arcLength = materialStartAngle - materialEndAngle;
		CheckArcLength();
		CheckEasingValue();
		if (isCollapsing)
		{
			materialEndAngle -= easingValue;
		}
		else
		{
			materialStartAngle -= easingValue;
		}
	}
}
