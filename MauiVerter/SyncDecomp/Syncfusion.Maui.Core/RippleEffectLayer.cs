using System;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core;

internal class RippleEffectLayer
{
	private const float RippleTransparencyFactor = 0.12f;

	private float rippleDiameter;

	private readonly string rippleAnimatorName = "RippleAnimator";

	private readonly string fadeOutName = "RippleFadeOut";

	private Point touchPoint;

	private double animationAreaLength;

	private float alphaValue;

	private bool fadeOutRipple;

	private Brush rippleColor = new SolidColorBrush(Colors.Black);

	private double rippleAnimationDuration;

	private readonly float minAnimationDuration = 1f;

	private bool removeRippleAnimation;

	private readonly IDrawable drawable;

	private readonly IAnimatable animation;

	internal double Width { get; set; }

	internal double Height { get; set; }

	internal bool CanRemoveRippleAnimation
	{
		get
		{
			return removeRippleAnimation;
		}
		set
		{
			removeRippleAnimation = value;
		}
	}

	private float RippleFadeInOutAnimationDuration => (float)(((rippleAnimationDuration < (double)minAnimationDuration) ? ((double)minAnimationDuration) : rippleAnimationDuration) / 4.0);

	public RippleEffectLayer(Brush rippleColor, double rippleDuration, IDrawable drawable, IAnimatable animate)
	{
		this.rippleColor = rippleColor;
		rippleAnimationDuration = rippleDuration;
		this.drawable = drawable;
		animation = animate;
		alphaValue = 0.12f;
	}

	internal void DrawRipple(ICanvas canvas, RectF dirtyRect)
	{
		if (rippleColor != null)
		{
			canvas.Alpha = alphaValue;
			DrawRipple(canvas, dirtyRect, rippleColor);
		}
	}

	internal void DrawRipple(ICanvas canvas, RectF dirtyRect, Brush color, bool clipBounds = false)
	{
		if (rippleColor != null)
		{
			canvas.SetFillPaint(color, dirtyRect);
			if (clipBounds)
			{
				ExpandRippleEllipse(canvas, dirtyRect);
			}
			else
			{
				ExpandRippleEllipse(canvas);
			}
		}
	}

	internal void StartRippleAnimation(Point point, Brush rippleColor, double rippleAnimationDuration, float initialRippleFactor, bool fadeoutRipple, bool canRepeat = false)
	{
		if (DeviceInfo.Platform == DevicePlatform.WinUI)
		{
			IVisualElementController obj = drawable as IVisualElementController;
			if (obj != null && (obj.EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
			{
				touchPoint = new Point(Width - point.X, point.Y);
				goto IL_0070;
			}
		}
		touchPoint = point;
		goto IL_0070;
		IL_0070:
		this.rippleColor = rippleColor;
		this.rippleAnimationDuration = rippleAnimationDuration;
		fadeOutRipple = fadeoutRipple;
		alphaValue = 0.12f;
		double start = GetRippleRadiusFromFactor(initialRippleFactor);
		animationAreaLength = GetFinalRadius(point);
		Animation animation = new Animation(OnRippleAnimationUpdate, start, animationAreaLength);
		animation.Commit(this.animation, rippleAnimatorName, 16u, (uint)rippleAnimationDuration, Easing.Linear, OnRippleFinished, () => canRepeat);
		if (fadeoutRipple)
		{
			Animation animation2 = new Animation(OnFadeAnimationUpdate, 0.0, alphaValue);
			animation2.Commit(this.animation, fadeOutName, 16u, (uint)RippleFadeInOutAnimationDuration, Easing.Linear, null, () => canRepeat);
			animation.WithConcurrent(animation2);
		}
	}

	internal void OnFadeAnimationUpdate(double value)
	{
		alphaValue = (float)value;
		InvalidateDrawable();
	}

	internal void OnRippleAnimationUpdate(double value)
	{
		rippleDiameter = (float)value;
		InvalidateDrawable();
	}

	internal void OnRippleAnimationFinished()
	{
		animation.AbortAnimation(rippleAnimatorName);
		rippleDiameter = 0f;
		InvalidateDrawable();
	}

	private void InvalidateDrawable()
	{
		if (drawable is IDrawableLayout drawableLayout)
		{
			drawableLayout.InvalidateDrawable();
		}
		else if (drawable is IDrawableView drawableView)
		{
			drawableView.InvalidateDrawable();
		}
	}

	private IElement? GetParent()
	{
		if (drawable is IDrawableLayout result)
		{
			return result;
		}
		if (drawable is IDrawableView result2)
		{
			return result2;
		}
		return null;
	}

	private float GetRippleRadiusFromFactor(float initialRippleFactor)
	{
		if (Width > 0.0 && Height > 0.0)
		{
			return (float)(Math.Min(Width, Height) / 2.0 * (double)initialRippleFactor);
		}
		if (GetParent() is View view && view.Width > 0.0 && view.Height > 0.0)
		{
			float val = (float)view.Width;
			float val2 = (float)view.Height;
			return Math.Min(val, val2) / 2f * initialRippleFactor;
		}
		return 0f;
	}

	private float GetFinalRadius(Point pivot)
	{
		if (Width > 0.0 && Height > 0.0)
		{
			float num = (float)((pivot.X > Width / 2.0) ? pivot.X : (Width - pivot.X));
			float num2 = (float)((pivot.Y > Height / 2.0) ? pivot.Y : (Height - pivot.Y));
			return (float)Math.Sqrt(num * num + num2 * num2);
		}
		if (GetParent() is View view && view.Width > 0.0 && view.Height > 0.0)
		{
			float num3 = (float)view.Width;
			float num4 = (float)view.Height;
			float num5 = (float)((pivot.X > (double)(num3 / 2f)) ? pivot.X : ((double)num3 - pivot.X));
			float num6 = (float)((pivot.Y > (double)(num4 / 2f)) ? pivot.Y : ((double)num4 - pivot.Y));
			return (float)Math.Sqrt(num5 * num5 + num6 * num6);
		}
		return (float)Math.Sqrt(pivot.X * pivot.X + pivot.Y * pivot.Y);
	}

	private void ExpandRippleEllipse(ICanvas canvas)
	{
		canvas.FillCircle((float)touchPoint.X, (float)touchPoint.Y, rippleDiameter);
	}

	private void ExpandRippleEllipse(ICanvas canvas, Rect rect)
	{
		canvas.SaveState();
		canvas.ClipRectangle(rect);
		canvas.FillCircle((float)touchPoint.X, (float)touchPoint.Y, rippleDiameter);
		canvas.RestoreState();
		canvas.ResetState();
	}

	private void OnRippleFinished(double value, bool isCompleted)
	{
		if (CanRemoveRippleAnimation)
		{
			animation.AbortAnimation(rippleAnimatorName);
			rippleDiameter = 0f;
			InvalidateDrawable();
		}
		if ((CanRemoveRippleAnimation || !animation.AnimationIsRunning(rippleAnimatorName)) && GetParent() != null && (drawable as View) is SfEffectsView && GetParent() as View is SfEffectsView sfEffectsView && (sfEffectsView.TouchUpEffects == SfEffects.None || sfEffectsView.AutoResetEffects.GetAllItems().Contains(AutoResetEffects.Ripple) || sfEffectsView.TouchUpEffects == SfEffects.Ripple || sfEffectsView.TouchUpEffects.GetAllItems().Contains(SfEffects.Ripple) || sfEffectsView.TouchUpEffects.GetAllItems().Contains(SfEffects.None)) && (sfEffectsView.LongPressEffects.GetAllItems().Contains(SfEffects.None) || !sfEffectsView.LongPressHandled || sfEffectsView.LongPressEffects.GetAllItems().Contains(SfEffects.Ripple)))
		{
			sfEffectsView?.InvokeAnimationCompletedEvent();
		}
	}
}
