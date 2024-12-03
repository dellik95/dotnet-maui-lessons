using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

internal class SfTooltip : ContentView
{
	private readonly Grid parentView;

	private readonly TooltipDrawableView drawableView;

	private readonly ContentView contentView;

	private readonly TooltipHelper tooltipHelper;

	private bool isDisappeared = false;

	private const string durationAnimation = "Duration";

	private bool isTooltipActivate = false;

	private View? content;

	public new static readonly BindableProperty BackgroundProperty = BindableProperty.Create(nameof(Background), typeof(Brush), typeof(SfTooltip), new SolidColorBrush(Colors.Black), BindingMode.OneWay, null, OnBackgroundPropertyChanged);

	public new View? Content
	{
		get
		{
			return content;
		}
		set
		{
			if (content != value)
			{
				content = value;
				OnContentChanged();
			}
		}
	}

	public TooltipPosition Position { get; set; } = TooltipPosition.Auto;


	public int Duration { get; set; } = 2;


	public new Brush Background
	{
		get
		{
			return (Brush)GetValue(BackgroundProperty);
		}
		set
		{
			SetValue(BackgroundProperty, value);
		}
	}

	public event EventHandler<TooltipClosedEventArgs>? TooltipClosed;

	public SfTooltip()
	{
		parentView = new Grid();
		drawableView = new TooltipDrawableView(this);
		contentView = new ContentView();
		tooltipHelper = new TooltipHelper(drawableView.InvalidateDrawable);
		parentView.Add(drawableView);
		parentView.Add(contentView);
		base.Content = parentView;
	}

	public void Show(Rect containerRect, Rect targetRect, bool animated)
	{
		if (containerRect.IsEmpty || targetRect.IsEmpty || Content == null)
		{
			return;
		}
		double x = containerRect.X;
		double y = containerRect.Y;
		double width = containerRect.Width;
		double height = containerRect.Height;
		if (!(targetRect.X > x + width) && !(targetRect.Y > y + height))
		{
			tooltipHelper.Position = Position;
			tooltipHelper.Duration = Duration;
			tooltipHelper.Background = Background;
			if (isTooltipActivate)
			{
				isDisappeared = false;
				Hide(animated);
			}
			if (base.Opacity == 0.0)
			{
				base.Opacity = 1.0;
			}
			Content.VerticalOptions = LayoutOptions.Start;
			Content.HorizontalOptions = LayoutOptions.Start;
			tooltipHelper.ContentSize = Content.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins).Request;
			tooltipHelper.Show(containerRect, targetRect, animated: false);
			SetContentMargin(tooltipHelper.ContentViewMargin);
			AbsoluteLayout.SetLayoutBounds((BindableObject)this, tooltipHelper.TooltipRect);
			drawableView.InvalidateDrawable();
			isTooltipActivate = true;
			AutoHide();
		}
	}

	public void Hide(bool animated)
	{
		this.AbortAnimation("Duration");
		base.Opacity = 0.0;
		AbsoluteLayout.SetLayoutBounds((BindableObject)this, new Rect(0.0, 0.0, 1.0, 1.0));
		isTooltipActivate = false;
		this.TooltipClosed?.Invoke(this, new TooltipClosedEventArgs
		{
			IsCompleted = isDisappeared
		});
	}

	internal void Draw(ICanvas canvas, RectF dirtyRect)
	{
		Draw(canvas);
	}

	private void Draw(ICanvas canvas)
	{
		if (!(tooltipHelper.RoundedRect == Rect.Zero))
		{
			tooltipHelper.Draw(canvas);
		}
	}

	private void AutoHide()
	{
		Animation animation = new Animation();
		animation.Commit(this, "Duration", 16u, (uint)(tooltipHelper.Duration * 1000), null, Hide, () => false);
	}

	private void Hide(double value, bool isCompleted)
	{
		isDisappeared = !isCompleted;
		if (!isCompleted)
		{
			Hide(animated: false);
		}
	}

	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();
		if (Content != null)
		{
			BindableObject.SetInheritedBindingContext(Content, base.BindingContext);
		}
	}

	private static void OnBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
	}

	private void SetContentMargin(Thickness thickness)
	{
		if (contentView != null)
		{
			contentView.Margin = thickness;
		}
	}

	private void OnContentChanged()
	{
		if (Content != null)
		{
			BindableObject.SetInheritedBindingContext(Content, base.BindingContext);
		}
		contentView.Content = Content;
	}
}
