using Microsoft.Maui.Animations;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

internal class CupertinoBusyIndicatorAnimation : BusyIndicatorAnimation
{
	private readonly float itemNumber = 12f;

	private float alphaValue = 255f;

	private readonly int lineSize = 6;

	private readonly float rotateAngle = 30f;

	private readonly float minimumAlphaValue = 150f;

	private readonly float alphaStepValue = 30f;

	private readonly float strokeSize = 10f;

	private int currentLine = 0;

	private readonly float defaultAlphaValue = 1f;

	public CupertinoBusyIndicatorAnimation(IAnimationManager animationManagerValue)
		: base(animationManagerValue)
	{
		base.DefaultDuration = 300.0;
		defaultHeight = 100.0;
		defaultWidth = 100.0;
	}

	protected override void OnDrawAnimation(SfView view, ICanvas canvas)
	{
		base.OnDrawAnimation(view, canvas);
		canvas.StrokeSize = lineSize;
		canvas.StrokeLineCap = LineCap.Round;
		DrawLines(canvas, actualRect);
	}

	protected override void OnUpdateAnimation()
	{
		base.OnUpdateAnimation();
		UpdateAnimation();
		drawableView?.InvalidateDrawable();
	}

	private void DrawLines(ICanvas canvas, RectF bounds)
	{
		canvas.Translate(bounds.Center.X, bounds.Center.Y);
		canvas.Rotate((float)currentLine * rotateAngle);
		alphaValue = 1f;
		canvas.StrokeColor = base.Color;
		canvas.StrokeSize = strokeSize * (float)sizeFactor;
		for (int i = 0; (float)i < itemNumber; i++)
		{
			if (alphaValue <= minimumAlphaValue)
			{
				alphaValue += alphaStepValue;
			}
			canvas.Alpha = alphaValue;
			canvas.DrawLine(bounds.Height / 4f, 0f, bounds.Height / 2f, 0f);
			canvas.Rotate(0f - rotateAngle);
		}
		canvas.Rotate(itemNumber - (float)currentLine * rotateAngle);
		canvas.Rotate((0f - rotateAngle) / 2.5f);
		canvas.Translate(0f - bounds.Center.X, 0f - bounds.Center.Y);
		canvas.Alpha = defaultAlphaValue;
	}

	private void UpdateAnimation()
	{
		if ((float)currentLine >= itemNumber)
		{
			currentLine = 0;
		}
		else
		{
			currentLine++;
		}
	}
}
