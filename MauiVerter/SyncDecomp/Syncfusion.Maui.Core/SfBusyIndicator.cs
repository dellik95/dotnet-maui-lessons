using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Animations;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core.BusyIndicator;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core;

public class SfBusyIndicator : SfContentView, ITextElement
{
	private BusyIndicatorAnimation? busyIndicatorAnimation;

	private IAnimationManager? animationManager;

	private Rect TextRect = default(Rect);

	private double titleHeight = 0.0;

	private AnimationType animation;

	public static readonly BindableProperty IsRunningProperty = BindableProperty.Create(nameof(IsRunning), typeof(bool), typeof(SfBusyIndicator), false, BindingMode.OneWay, null, OnIsRunningPropertyChanged);

	public static readonly BindableProperty IndicatorColorProperty = BindableProperty.Create(nameof(IndicatorColor), typeof(Color), typeof(SfBusyIndicator), Color.FromArgb("#FF512BD4"), BindingMode.OneWay, null, OnIndicatorColorPropertyChanged);

	public static readonly BindableProperty OverlayFillProperty = BindableProperty.Create(nameof(OverlayFill), typeof(Brush), typeof(SfBusyIndicator), null, BindingMode.OneWay, null, OnOverlayFillPropertyChanged);

	public static readonly BindableProperty AnimationTypeProperty = BindableProperty.Create(nameof(AnimationType), typeof(AnimationType), typeof(SfBusyIndicator), AnimationType.CircularMaterial, BindingMode.OneWay, null, OnAnimationTypeChanged);

	public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(SfBusyIndicator), string.Empty, BindingMode.OneWay, null, OnTitlePropertyChanged);

	public static readonly BindableProperty DurationFactorProperty = BindableProperty.Create(nameof(DurationFactor), typeof(double), typeof(SfBusyIndicator), 0.5, BindingMode.OneWay, null, OnDurationFactorPropertyChanged);

	public static readonly BindableProperty TitlePlacementProperty = BindableProperty.Create(nameof(TitlePlacement), typeof(BusyIndicatorTitlePlacement), typeof(SfBusyIndicator), BusyIndicatorTitlePlacement.Bottom, BindingMode.OneWay, null, OnTitlePlacementPropertyChanged);

	public static readonly BindableProperty TitleSpacingProperty = BindableProperty.Create(nameof(TitleSpacing), typeof(double), typeof(SfBusyIndicator), 10.0, BindingMode.OneWay, null, OnTitleSpacingPropertyChanged);

	public static readonly BindableProperty SizeFactorProperty = BindableProperty.Create(nameof(SizeFactor), typeof(double), typeof(SfBusyIndicator), 0.5, BindingMode.OneWay, null, OnSizeFactorPropertyChanged);

	public static readonly BindableProperty FontSizeProperty = FontElement.FontSizeProperty;

	public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

	public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

	public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(BadgeSettings), Colors.Black, BindingMode.Default, null, OnITextElementPropertyChanged);

	public bool IsRunning
	{
		get
		{
			return (bool)GetValue(IsRunningProperty);
		}
		set
		{
			SetValue(IsRunningProperty, value);
		}
	}

	public Color IndicatorColor
	{
		get
		{
			return (Color)GetValue(IndicatorColorProperty);
		}
		set
		{
			SetValue(IndicatorColorProperty, value);
		}
	}

	public Brush OverlayFill
	{
		get
		{
			return (Brush)GetValue(OverlayFillProperty);
		}
		set
		{
			SetValue(OverlayFillProperty, value);
		}
	}

	public AnimationType AnimationType
	{
		get
		{
			return (AnimationType)GetValue(AnimationTypeProperty);
		}
		set
		{
			SetValue(AnimationTypeProperty, value);
		}
	}

	public string Title
	{
		get
		{
			return (string)GetValue(TitleProperty);
		}
		set
		{
			SetValue(TitleProperty, value);
		}
	}

	public double DurationFactor
	{
		get
		{
			return (double)GetValue(DurationFactorProperty);
		}
		set
		{
			SetValue(DurationFactorProperty, value);
		}
	}

	public BusyIndicatorTitlePlacement TitlePlacement
	{
		get
		{
			return (BusyIndicatorTitlePlacement)GetValue(TitlePlacementProperty);
		}
		set
		{
			SetValue(TitlePlacementProperty, value);
		}
	}

	public double TitleSpacing
	{
		get
		{
			return (double)GetValue(TitleSpacingProperty);
		}
		set
		{
			SetValue(TitleSpacingProperty, value);
		}
	}

	public double SizeFactor
	{
		get
		{
			return (double)GetValue(SizeFactorProperty);
		}
		set
		{
			SetValue(SizeFactorProperty, value);
		}
	}

	[TypeConverter(typeof(FontSizeConverter))]
	public double FontSize
	{
		get
		{
			return (double)GetValue(FontSizeProperty);
		}
		set
		{
			SetValue(FontSizeProperty, value);
		}
	}

	public FontAttributes FontAttributes
	{
		get
		{
			return (FontAttributes)GetValue(FontAttributesProperty);
		}
		set
		{
			SetValue(FontAttributesProperty, value);
		}
	}

	public string FontFamily
	{
		get
		{
			return (string)GetValue(FontFamilyProperty);
		}
		set
		{
			SetValue(FontFamilyProperty, value);
		}
	}

	public Color TextColor
	{
		get
		{
			return (Color)GetValue(TextColorProperty);
		}
		set
		{
			SetValue(TextColorProperty, value);
		}
	}

	Microsoft.Maui.Font ITextElement.Font => (Microsoft.Maui.Font)GetValue(FontElement.FontProperty);

	private static void OnIsRunningPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfBusyIndicator sfBusyIndicator && sfBusyIndicator != null && sfBusyIndicator.busyIndicatorAnimation != null)
		{
			sfBusyIndicator.busyIndicatorAnimation.RunAnimation((bool)newValue);
			sfBusyIndicator.InvalidateDrawable();
		}
	}

	private static void OnIndicatorColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfBusyIndicator sfBusyIndicator && sfBusyIndicator != null && sfBusyIndicator.busyIndicatorAnimation != null)
		{
			sfBusyIndicator.busyIndicatorAnimation.Color = (Color)newValue;
			sfBusyIndicator.InvalidateDrawable();
		}
	}

	private static void OnOverlayFillPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfBusyIndicator sfBusyIndicator && sfBusyIndicator != null && sfBusyIndicator.busyIndicatorAnimation != null)
		{
			sfBusyIndicator.InvalidateDrawable();
		}
	}

	private static void OnAnimationTypeChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfBusyIndicator sfBusyIndicator)
		{
			sfBusyIndicator?.SetAnimationType((AnimationType)newValue);
		}
	}

	private static void OnTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfBusyIndicator sfBusyIndicator)
		{
			sfBusyIndicator?.InvalidateDrawable();
		}
	}

	private static void OnDurationFactorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfBusyIndicator sfBusyIndicator && sfBusyIndicator != null && sfBusyIndicator.busyIndicatorAnimation != null)
		{
			sfBusyIndicator.busyIndicatorAnimation.AnimationDuration = (double)newValue;
		}
	}

	private static void OnTitlePlacementPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfBusyIndicator sfBusyIndicator)
		{
			sfBusyIndicator?.InvalidateDrawable();
		}
	}

	private static void OnTitleSpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfBusyIndicator sfBusyIndicator)
		{
			sfBusyIndicator?.InvalidateDrawable();
		}
	}

	private static void OnSizeFactorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfBusyIndicator sfBusyIndicator && sfBusyIndicator != null && sfBusyIndicator.busyIndicatorAnimation != null)
		{
			sfBusyIndicator.busyIndicatorAnimation.sizeFactor = (double)newValue;
		}
	}

	double ITextElement.FontSizeDefaultValueCreator()
	{
		return 12.0;
	}

	void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
	{
		InvalidateDrawable();
	}

	void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
	{
		InvalidateDrawable();
	}

	void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
	{
		InvalidateDrawable();
	}

	private static void OnITextElementPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
	{
		if (bindable is SfBusyIndicator sfBusyIndicator)
		{
			sfBusyIndicator?.InvalidateDrawable();
		}
	}

	public SfBusyIndicator()
	{
		base.DrawingOrder = DrawingOrder.AboveContent;
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		if (base.Handler != null && base.Handler.MauiContext != null)
		{
			animationManager = base.Handler.MauiContext.Services.GetRequiredService<IAnimationManager>();
			SetAnimationType(AnimationType);
		}
	}

	protected override Size ArrangeContent(Rect bounds)
	{
		if (base.Content != null)
		{
			AbsoluteLayout.SetLayoutBounds((BindableObject)base.Content, bounds);
		}
		if (animation == AnimationType.DoubleCircle && busyIndicatorAnimation is DoubleCircleBusyIndicatorAnimation doubleCircleBusyIndicatorAnimation)
		{
			doubleCircleBusyIndicatorAnimation.CalculateXYPositions(this);
		}
		return base.ArrangeContent(bounds);
	}

	protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
	{
		base.OnDraw(canvas, dirtyRect);
		if (IsRunning)
		{
			if (OverlayFill != null)
			{
				canvas.SetFillPaint(OverlayFill, dirtyRect);
				canvas.FillRectangle(dirtyRect);
			}
			if (busyIndicatorAnimation != null)
			{
				busyIndicatorAnimation.DrawAnimation(this, canvas);
				DrawTitle(canvas);
			}
		}
	}

	private void DrawTitle(ICanvas canvas)
	{
		if (!string.IsNullOrEmpty(Title) && busyIndicatorAnimation != null && TitlePlacement != BusyIndicatorTitlePlacement.None)
		{
			titleHeight = TextMeasurer.CreateTextMeasurer().MeasureText(Title, this).Height;
			TextRect.X = 0.0;
			TextRect.Width = base.Width;
			if (TitlePlacement == BusyIndicatorTitlePlacement.Bottom)
			{
				TextRect.Y = busyIndicatorAnimation.actualRect.Y + busyIndicatorAnimation.actualRect.Height + TitleSpacing;
			}
			else
			{
				TextRect.Y = busyIndicatorAnimation.actualRect.Y - (TitleSpacing + titleHeight);
			}
			TextRect.Height = titleHeight;
			canvas.DrawText(Title, TextRect, HorizontalAlignment.Center, VerticalAlignment.Top, this);
		}
	}

	private void SetAnimationType(AnimationType animationType)
	{
		if (animationManager != null)
		{
			switch (animationType)
			{
			case AnimationType.CircularMaterial:
				busyIndicatorAnimation = new CircularMaterialBusyIndicatorAnimation(animationManager);
				break;
			case AnimationType.Cupertino:
				busyIndicatorAnimation = new CupertinoBusyIndicatorAnimation(animationManager);
				break;
			case AnimationType.LinearMaterial:
				busyIndicatorAnimation = new LinearMaterialBusyIndicatorAnimation(animationManager);
				break;
			case AnimationType.SingleCircle:
				busyIndicatorAnimation = new SingleCircleBusyIndictorAnimation(animationManager);
				break;
			case AnimationType.DoubleCircle:
				busyIndicatorAnimation = new DoubleCircleBusyIndicatorAnimation(animationManager, this);
				animation = AnimationType.DoubleCircle;
				break;
			}
			if (busyIndicatorAnimation != null)
			{
				busyIndicatorAnimation.Color = IndicatorColor;
				busyIndicatorAnimation.RunAnimation(IsRunning);
				busyIndicatorAnimation.AnimationDuration = DurationFactor;
				busyIndicatorAnimation.sizeFactor = SizeFactor;
				InvalidateDrawable();
			}
		}
	}

	internal void OnFontFamilyChanged(string oldValue, string newValue)
	{
		InvalidateDrawable();
	}

	internal void OnFontSizeChanged(double oldValue, double newValue)
	{
		InvalidateDrawable();
	}

	internal double FontSizeDefaultValueCreator()
	{
		throw new NotImplementedException();
	}

	internal void OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
	{
		InvalidateDrawable();
	}

	public void OnFontChanged(Microsoft.Maui.Font oldValue, Microsoft.Maui.Font newValue)
	{
		InvalidateDrawable();
	}
}
