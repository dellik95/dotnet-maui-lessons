using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core.Internals;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core;

internal class EffectsRenderer : ITouchListener
{
	private readonly IDrawable? drawable;

	private ICanvas? currentCanvas;

	private bool shouldDrawHighlight = false;

	private bool shouldDrawRipple = false;

	private readonly HighlightEffectLayer? highlightEffectLayer;

	private readonly RippleEffectLayer? rippleEffectLayer;

	private RectF rippleBounds;

	private RectF highlightBounds;

	private ObservableCollection<RectF> rippleBoundsCollection = new ObservableCollection<RectF>();

	private ObservableCollection<RectF> highlightBoundsCollection = new ObservableCollection<RectF>();

	private Brush rippleColorBrush = new SolidColorBrush(Color.FromRgba(150, 150, 150, 75));

	private Brush highlightBrush = new SolidColorBrush(Color.FromRgba(200, 200, 200, 50));

	public Brush HighlightBrush
	{
		get
		{
			return highlightBrush;
		}
		set
		{
			highlightBrush = value;
		}
	}

	public Brush RippleColorBrush
	{
		get
		{
			return rippleColorBrush;
		}
		set
		{
			rippleColorBrush = value;
		}
	}

	public double RippleAnimationDuration { get; set; }

	public double RippleStart { get; set; }

	internal bool ShouldDrawHighlight
	{
		get
		{
			return shouldDrawHighlight;
		}
		private set
		{
			if (ShouldDrawHighlight != value)
			{
				shouldDrawHighlight = value;
				InvalidateDrawable();
			}
		}
	}

	internal bool ShouldDrawRipple
	{
		get
		{
			return shouldDrawRipple;
		}
		private set
		{
			if (shouldDrawRipple != value)
			{
				shouldDrawRipple = value;
				InvalidateDrawable();
			}
		}
	}

	internal RectF RippleBounds
	{
		get
		{
			return rippleBounds;
		}
		private set
		{
			if (rippleBounds != value)
			{
				rippleBounds = value;
				InvalidateDrawable();
			}
		}
	}

	internal RectF HighlightBounds
	{
		get
		{
			return highlightBounds;
		}
		private set
		{
			if (highlightBounds != value)
			{
				highlightBounds = value;
				InvalidateDrawable();
			}
		}
	}

	public ObservableCollection<RectF> RippleBoundsCollection
	{
		get
		{
			return rippleBoundsCollection;
		}
		set
		{
			rippleBoundsCollection = value;
		}
	}

	public ObservableCollection<RectF> HighlightBoundsCollection
	{
		get
		{
			return highlightBoundsCollection;
		}
		set
		{
			highlightBoundsCollection = value;
		}
	}

	public EffectsRenderer(View drawableView)
	{
		drawableView.AddTouchListener(this);
		if (drawableView is IDrawable drawable)
		{
			this.drawable = drawable;
			rippleEffectLayer = new RippleEffectLayer(RippleColorBrush, 1000.0, drawable, drawableView);
			highlightEffectLayer = new HighlightEffectLayer(HighlightBrush, drawable);
		}
	}

	internal void DrawEffects(ICanvas canvas)
	{
		currentCanvas = canvas;
		if (HighlightBounds.Width > 0f && HighlightBounds.Height > 0f)
		{
			DrawHighlight(HighlightBounds);
		}
		if (RippleBounds.Width > 0f && RippleBounds.Height > 0f)
		{
			DrawRipple(RippleBounds);
		}
	}

	private void DrawRipple(RectF rectF)
	{
		if (ShouldDrawRipple && rippleEffectLayer != null && currentCanvas != null)
		{
			rippleEffectLayer.DrawRipple(currentCanvas, rectF, RippleColorBrush, clipBounds: true);
		}
	}

	private void DrawHighlight(RectF rectF)
	{
		if (ShouldDrawHighlight && highlightEffectLayer != null && currentCanvas != null)
		{
			highlightEffectLayer.DrawHighlight(currentCanvas, rectF, HighlightBrush);
		}
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

	public void OnTouch(PointerEventArgs e)
	{
		if (e.Action == PointerActions.Moved)
		{
			CheckBoundsContainsPoint(e.TouchPoint, HighlightBoundsCollection, isRipple: false);
		}
		else if (e.Action == PointerActions.Pressed)
		{
			CheckBoundsContainsPoint(e.TouchPoint, RippleBoundsCollection, isRipple: true);
			if (ShouldDrawRipple && rippleEffectLayer != null)
			{
				rippleEffectLayer.StartRippleAnimation(e.TouchPoint, RippleColorBrush, 2000.0, 0f, fadeoutRipple: true);
			}
			else if (rippleBounds.Width > 0f && rippleBounds.Height > 0f)
			{
				RemoveRipple();
			}
		}
		else if (e.Action == PointerActions.Released)
		{
			if (rippleBounds.Width > 0f && rippleBounds.Height > 0f)
			{
				RemoveRipple();
			}
		}
		else if (e.Action == PointerActions.Cancelled || e.Action == PointerActions.Exited)
		{
			ShouldDrawHighlight = false;
			ShouldDrawRipple = false;
			if (rippleBounds.Width > 0f && rippleBounds.Height > 0f)
			{
				RemoveRipple();
			}
			if (highlightBounds.Width > 0f && highlightBounds.Height > 0f)
			{
				RemoveHighlight();
			}
		}
	}

	private void RemoveRipple()
	{
		rippleEffectLayer?.OnRippleAnimationFinished();
	}

	private void RemoveHighlight()
	{
		if (highlightEffectLayer != null)
		{
			HighlightBounds = new RectF(0f, 0f, 0f, 0f);
			highlightEffectLayer.UpdateHighlightBounds();
		}
	}

	private void CheckBoundsContainsPoint(Point p, ObservableCollection<RectF> bounds, bool isRipple)
	{
		foreach (RectF bound in bounds)
		{
			if (bound.Contains(p))
			{
				if (isRipple)
				{
					RippleBounds = bound;
					ShouldDrawRipple = true;
				}
				else
				{
					HighlightBounds = bound;
					ShouldDrawHighlight = true;
				}
				return;
			}
		}
		if (isRipple)
		{
			ShouldDrawRipple = false;
		}
		else
		{
			ShouldDrawHighlight = false;
		}
	}
}
