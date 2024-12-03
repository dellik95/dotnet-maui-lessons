using Microsoft.Maui;
using Microsoft.Maui.Animations;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

internal abstract class BusyIndicatorAnimation : Animation
{
	internal SfView? drawableView;

	private double secondsSinceLastUpdate;

	private double actualDuration = 0.0;

	internal double sizeFactor = 0.5;

	internal Rect actualRect = default(Rect);

	internal double defaultHeight = 0.0;

	internal double defaultWidth = 0.0;

	private double defaultDuration = 100.0;

	private double animationduration = 0.5;

	private Color color = Microsoft.Maui.Graphics.Color.FromArgb("#FF512BD4");

	internal double DefaultDuration
	{
		get
		{
			return defaultDuration;
		}
		set
		{
			defaultDuration = value;
			SetActualDuration();
		}
	}

	internal double AnimationDuration
	{
		get
		{
			return animationduration;
		}
		set
		{
			animationduration = value;
			if (animationduration > 1.0)
			{
				animationduration = 1.0;
			}
			SetActualDuration();
		}
	}

	internal Color Color
	{
		get
		{
			return color;
		}
		set
		{
			color = value;
		}
	}

	public BusyIndicatorAnimation(IAnimationManager animationManagerValue)
	{
		animationManger = animationManagerValue;
		base.Easing = Microsoft.Maui.Easing.SinIn;
		base.Repeats = true;
		SetActualDuration();
	}

	protected virtual void OnDrawAnimation(SfView view, ICanvas canvas)
	{
	}

	protected virtual void OnUpdateAnimation()
	{
	}

	protected override void OnTick(double millisecondsSinceLastUpdate)
	{
		base.OnTick(millisecondsSinceLastUpdate);
		secondsSinceLastUpdate += millisecondsSinceLastUpdate;
		if (secondsSinceLastUpdate > actualDuration)
		{
			UpdateActualRect();
			OnUpdateAnimation();
			secondsSinceLastUpdate = 0.0;
		}
	}

	internal void UpdateActualRect()
	{
		if (drawableView != null)
		{
			double num = drawableView.Width / 2.0;
			double num2 = drawableView.Height / 2.0;
			double num3 = defaultWidth / 2.0;
			double num4 = defaultHeight / 2.0;
			num3 *= sizeFactor;
			num4 *= sizeFactor;
			actualRect.X = num - num3;
			actualRect.Y = num2 - num4;
			actualRect.Width = defaultWidth * sizeFactor;
			actualRect.Height = defaultHeight * sizeFactor;
		}
	}

	internal void DrawAnimation(SfView view, ICanvas canvas)
	{
		drawableView ??= view;
		OnDrawAnimation(view, canvas);
	}

	private void SetActualDuration()
	{
		actualDuration = AnimationDuration * DefaultDuration;
	}

	internal void RunAnimation(bool canStart)
	{
		base.HasFinished = !canStart;
		if (canStart)
		{
			Resume();
		}
		else
		{
			Pause();
		}
	}
}
