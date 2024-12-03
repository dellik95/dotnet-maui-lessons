using Microsoft.Maui.Animations;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

internal class LinearMaterialBusyIndicatorAnimation : BusyIndicatorAnimation
{
	private double a1;

	private double a2;

	private double b1;

	private double b2 = 0.0;

	private double y;

	private Point aPoint1 = new Point(0.0, 0.0);

	private Point aPoint2 = new Point(0.0, 0.0);

	private Point bPoint1 = new Point(0.0, 0.0);

	private Point bPoint2 = new Point(0.0, 0.0);

	private int strokeSize = 12;

	private bool isExpand = true;

	private double lineLength = 5.0;

	private int stepLength = 3;

	private int stepExpotentialLength = 5;

	private int maximumLineLength = 250;

	private int minimumLineLength = 30;

	public LinearMaterialBusyIndicatorAnimation(IAnimationManager animationManagerValue)
		: base(animationManagerValue)
	{
		base.DefaultDuration = 0.0;
	}

	protected override void OnDrawAnimation(SfView view, ICanvas canvas)
	{
		base.OnDrawAnimation(view, canvas);
		if (drawableView != null)
		{
			y = drawableView.Height / 2.0;
			b2 = a2 - drawableView.Width;
			b1 = a1 - drawableView.Width;
			aPoint1.X = a1;
			aPoint2.X = a2;
			bPoint1.X = b1;
			bPoint2.X = b2;
			ref Point reference = ref aPoint1;
			ref Point reference2 = ref aPoint2;
			ref Point reference3 = ref bPoint1;
			double num2 = (bPoint2.Y = y);
			double num4 = (reference3.Y = num2);
			double num6 = (reference2.Y = num4);
			reference.Y = num6;
			canvas.StrokeColor = base.Color;
			canvas.StrokeSize = (float)strokeSize * (float)sizeFactor;
			canvas.DrawLine(aPoint1, aPoint2);
			canvas.DrawLine(bPoint1, bPoint2);
		}
	}

	protected override void OnUpdateAnimation()
	{
		base.OnUpdateAnimation();
		if (drawableView != null)
		{
			a2 += stepLength;
			a1 += stepLength;
			if (a2 > drawableView.Width * 2.0)
			{
				a2 = b2;
				a1 = b1;
			}
			SetExpandingValue();
			drawableView.InvalidateDrawable();
		}
	}

	private void SetExpandingValue()
	{
		lineLength = a2 - a1;
		if (lineLength <= (double)minimumLineLength)
		{
			isExpand = true;
		}
		else if (lineLength >= (double)maximumLineLength)
		{
			isExpand = false;
		}
		if (isExpand)
		{
			a2 += stepExpotentialLength;
		}
		else
		{
			a1 += stepExpotentialLength;
		}
	}
}
