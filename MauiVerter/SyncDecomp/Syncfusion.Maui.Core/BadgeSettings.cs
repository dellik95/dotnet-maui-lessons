using System.ComponentModel;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core;

public class BadgeSettings : Element, ITextElement
{
	public static readonly BindableProperty FontSizeProperty = FontElement.FontSizeProperty;

	public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

	public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

	public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(BadgeSettings), Colors.White, BindingMode.Default, null, OnTextColorPropertyChanged);

	public static readonly BindableProperty BackgroundProperty = BindableProperty.Create(nameof(Background), typeof(Brush), typeof(BadgeSettings), new SolidColorBrush(Colors.Transparent), BindingMode.Default, null, OnBackgroundPropertyChanged);

	public static readonly BindableProperty StrokeThicknessProperty = BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(BadgeSettings), 0.0, BindingMode.Default, null, OnStrokeThicknessPropertyChanged);

	public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(BadgeSettings), new CornerRadius(25.0), BindingMode.Default, null, null, OnCornerRadiusPropertyChanged);

	public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(BadgeSettings), Colors.Transparent, BindingMode.Default, null, OnStrokePropertyChanged);

	public static readonly BindableProperty PositionProperty = BindableProperty.Create(nameof(Position), typeof(BadgePosition), typeof(BadgeSettings), BadgePosition.TopRight, BindingMode.Default, null, null, OnBadgePositionPropertyChanged);

	public static readonly BindableProperty OffsetProperty = BindableProperty.Create(nameof(Offset), typeof(Point), typeof(BadgeSettings), new Point(0.0, 0.0), BindingMode.Default, null, null, OnOffsetPropertyChanged);

	public static readonly BindableProperty AnimationProperty = BindableProperty.Create(nameof(Animation), typeof(BadgeAnimation), typeof(BadgeSettings), BadgeAnimation.Scale, BindingMode.Default, null, null, OnBadgeAnimationPropertyChanged);

	public static readonly BindableProperty TypeProperty = BindableProperty.Create(nameof(Type), typeof(BadgeType), typeof(BadgeSettings), BadgeType.Primary, BindingMode.Default, null, null, OnBadgeTypePropertyChanged);

	public static readonly BindableProperty TextPaddingProperty = BindableProperty.Create(nameof(TextPadding), typeof(Thickness), typeof(BadgeSettings), GetTextPadding(), BindingMode.Default, null, null, OnTextPaddingPropertyChanged);

	public static readonly BindableProperty AutoHideProperty = BindableProperty.Create(nameof(AutoHide), typeof(bool), typeof(BadgeSettings), false, BindingMode.Default, null, null, OnAutoHidePropertyChanged);

	public static readonly BindableProperty BadgeAlignmentProperty = BindableProperty.Create(nameof(BadgeAlignment), typeof(BadgeAlignment), typeof(BadgeSettings), BadgeAlignment.Center, BindingMode.Default, null, null, OnBadgeAlignmentPropertyChanged);

	public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(BadgeIcon), typeof(BadgeSettings), BadgeIcon.None, BindingMode.Default, null, null, OnBadgeIconPropertyChanged);

	private SfBadgeView? badgeView;

	public Brush Background
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

	public double StrokeThickness
	{
		get
		{
			return (double)GetValue(StrokeThicknessProperty);
		}
		set
		{
			SetValue(StrokeThicknessProperty, value);
		}
	}

	public CornerRadius CornerRadius
	{
		get
		{
			return (CornerRadius)GetValue(CornerRadiusProperty);
		}
		set
		{
			SetValue(CornerRadiusProperty, value);
		}
	}

	public Color Stroke
	{
		get
		{
			return (Color)GetValue(StrokeProperty);
		}
		set
		{
			SetValue(StrokeProperty, value);
		}
	}

	public BadgePosition Position
	{
		get
		{
			return (BadgePosition)GetValue(PositionProperty);
		}
		set
		{
			SetValue(PositionProperty, value);
		}
	}

	public Point Offset
	{
		get
		{
			return (Point)GetValue(OffsetProperty);
		}
		set
		{
			SetValue(OffsetProperty, value);
		}
	}

	public BadgeAnimation Animation
	{
		get
		{
			return (BadgeAnimation)GetValue(AnimationProperty);
		}
		set
		{
			SetValue(AnimationProperty, value);
		}
	}

	public BadgeType Type
	{
		get
		{
			return (BadgeType)GetValue(TypeProperty);
		}
		set
		{
			SetValue(TypeProperty, value);
		}
	}

	public Thickness TextPadding
	{
		get
		{
			return (Thickness)GetValue(TextPaddingProperty);
		}
		set
		{
			SetValue(TextPaddingProperty, value);
		}
	}

	public bool AutoHide
	{
		get
		{
			return (bool)GetValue(AutoHideProperty);
		}
		set
		{
			SetValue(AutoHideProperty, value);
		}
	}

	public BadgeAlignment BadgeAlignment
	{
		get
		{
			return (BadgeAlignment)GetValue(BadgeAlignmentProperty);
		}
		set
		{
			SetValue(BadgeAlignmentProperty, value);
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

	public BadgeIcon Icon
	{
		get
		{
			return (BadgeIcon)GetValue(IconProperty);
		}
		set
		{
			SetValue(IconProperty, value);
		}
	}

	internal SfBadgeView? BadgeView
	{
		get
		{
			return badgeView;
		}
		set
		{
			badgeView = value;
			ApplySettingstoUpdatedBadgeView();
		}
	}

	double ITextElement.FontSizeDefaultValueCreator()
	{
		return 12.0;
	}

	void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
	{
		UpdateFontAttributes(newValue);
	}

	void ITextElement.OnFontChanged(Microsoft.Maui.Font oldValue, Microsoft.Maui.Font newValue)
	{
	}

	void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
	{
		UpdateFontFamily(newValue);
	}

	void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
	{
		UpdateFontSize(newValue);
	}

	internal static void UpdateOffset(BadgePosition position, SfBadgeView badgeView, Point offset)
	{
		if (badgeView != null && badgeView.BadgeLabelView != null)
		{
			if (position == BadgePosition.Top || position == BadgePosition.Left || position == BadgePosition.TopLeft)
			{
				badgeView.BadgeLabelView.Margin = new Thickness(offset.X, offset.Y, 0.0, 0.0);
			}
			else if (position == BadgePosition.Right || position == BadgePosition.TopRight)
			{
				badgeView.BadgeLabelView.Margin = new Thickness(0.0, offset.Y, 0.0 - offset.X, 0.0);
			}
			else if (position == BadgePosition.Bottom || position == BadgePosition.BottomLeft)
			{
				badgeView.BadgeLabelView.Margin = new Thickness(offset.X, 0.0, 0.0, 0.0 - offset.Y);
			}
			else if (position == BadgePosition.BottomRight)
			{
				badgeView.BadgeLabelView.Margin = new Thickness(0.0, 0.0, 0.0 - offset.X, 0.0 - offset.Y);
			}
			badgeView?.InvalidateDrawable();
		}
	}

	internal void ApplySettingstoUpdatedBadgeView()
	{
		if (BadgeView != null)
		{
			OnBackgroundPropertyChanged(this, null, Background);
			OnBadgeAnimationPropertyChanged(this, null, Animation);
			OnBadgeIconPropertyChanged(this, null, Icon);
			OnBadgePositionPropertyChanged(this, null, Position);
			OnBadgeTypePropertyChanged(this, null, Type);
			OnCornerRadiusPropertyChanged(this, null, CornerRadius);
			UpdateFontAttributes(FontAttributes);
			UpdateFontFamily(FontFamily);
			UpdateFontSize(FontSize);
			OnOffsetPropertyChanged(this, null, Offset);
			OnStrokePropertyChanged(this, null, Stroke);
			OnStrokeThicknessPropertyChanged(this, null, StrokeThickness);
			OnTextColorPropertyChanged(this, null, TextColor);
			OnTextPaddingPropertyChanged(this, null, TextPadding);
			OnAutoHidePropertyChanged(this, null, AutoHide);
			OnBadgeAlignmentPropertyChanged(this, null, BadgeAlignment);
			badgeView?.InvalidateDrawable();
		}
	}

	internal Brush GetBadgeBackground(BadgeType value)
	{
		Brush result = new SolidColorBrush(Colors.Transparent);
		switch (value)
		{
		case BadgeType.Error:
			result = new SolidColorBrush(Color.FromRgba(220, 53, 69, 255));
			break;
		case BadgeType.Dark:
			result = new SolidColorBrush(Color.FromRgba(52, 58, 64, 255));
			break;
		case BadgeType.Info:
			result = new SolidColorBrush(Color.FromRgba(23, 162, 184, 255));
			break;
		case BadgeType.Light:
			result = new SolidColorBrush(Color.FromRgba(248, 249, 250, 255));
			break;
		case BadgeType.Primary:
			result = new SolidColorBrush(Color.FromRgba(0, 123, 255, 255));
			break;
		case BadgeType.Secondary:
			result = new SolidColorBrush(Color.FromRgba(108, 117, 125, 255));
			break;
		case BadgeType.Success:
			result = new SolidColorBrush(Color.FromRgba(40, 167, 69, 255));
			break;
		case BadgeType.Warning:
			result = new SolidColorBrush(Color.FromRgba(255, 193, 7, 255));
			break;
		case BadgeType.None:
			result = Background;
			break;
		}
		return result;
	}

	internal void AlignBadgeText(SfBadgeView badgeView, BadgePosition position)
	{
		bool flag = (((IVisualElementController)badgeView).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft;
		if (badgeView.BadgeLabelView != null)
		{
			switch (position)
			{
			case BadgePosition.Left:
				badgeView.BadgeLabelView.XPosition = (flag ? badgeView.Width : 0.0);
				badgeView.BadgeLabelView.YPosition = badgeView.Height / 2.0;
				break;
			case BadgePosition.Right:
				badgeView.BadgeLabelView.XPosition = (flag ? 0.0 : badgeView.Width);
				badgeView.BadgeLabelView.YPosition = badgeView.Height / 2.0;
				break;
			case BadgePosition.Top:
				badgeView.BadgeLabelView.XPosition = badgeView.Width / 2.0;
				badgeView.BadgeLabelView.YPosition = 0.0;
				break;
			case BadgePosition.Bottom:
				badgeView.BadgeLabelView.XPosition = badgeView.Width / 2.0;
				badgeView.BadgeLabelView.YPosition = badgeView.Height;
				break;
			case BadgePosition.TopLeft:
				badgeView.BadgeLabelView.XPosition = (flag ? badgeView.Width : 0.0);
				badgeView.BadgeLabelView.YPosition = 0.0;
				break;
			case BadgePosition.TopRight:
				badgeView.BadgeLabelView.XPosition = (flag ? 0.0 : badgeView.Width);
				badgeView.BadgeLabelView.YPosition = 0.0;
				break;
			case BadgePosition.BottomLeft:
				badgeView.BadgeLabelView.XPosition = (flag ? badgeView.Width : 0.0);
				badgeView.BadgeLabelView.YPosition = badgeView.Height;
				break;
			case BadgePosition.BottomRight:
				badgeView.BadgeLabelView.XPosition = (flag ? 0.0 : badgeView.Width);
				badgeView.BadgeLabelView.YPosition = badgeView.Height;
				break;
			}
		}
	}

	internal void AlignBadge(SfBadgeView badgeView, BadgePosition position)
	{
		AlignBadgeText(badgeView, position);
		badgeView.InvalidateDrawable();
	}

	protected override void OnParentSet()
	{
		base.OnParentSet();
		if (base.Parent != null && base.Parent is SfBadgeView)
		{
			BadgeView = base.Parent as SfBadgeView;
		}
	}

	private static void OnTextColorPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
	{
		if (bindable is BadgeSettings badgeSettings && badgeSettings.BadgeView != null && badgeSettings.BadgeView.BadgeLabelView != null)
		{
			badgeSettings.BadgeView.BadgeLabelView.TextColor = (Color)newValue;
			badgeSettings.BadgeView.InvalidateDrawable();
		}
	}

	private static void OnBackgroundPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
	{
		if (bindable is BadgeSettings badgeSettings && badgeSettings.BadgeView != null && badgeSettings.BadgeView.BadgeLabelView != null)
		{
			badgeSettings.BadgeView.BadgeLabelView.BadgeBackground = (Brush)newValue;
			badgeSettings.BadgeView.InvalidateDrawable();
		}
	}

	private static void OnStrokePropertyChanged(BindableObject bindable, object? oldValue, object newValue)
	{
		if (bindable is BadgeSettings badgeSettings && badgeSettings.BadgeView != null && badgeSettings.BadgeView.BadgeLabelView != null)
		{
			badgeSettings.BadgeView.BadgeLabelView.Stroke = (Color)newValue;
			badgeSettings.BadgeView.InvalidateDrawable();
		}
	}

	private static void OnStrokeThicknessPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
	{
		if (bindable is BadgeSettings badgeSettings && badgeSettings.BadgeView != null && badgeSettings.BadgeView.BadgeLabelView != null)
		{
			badgeSettings.BadgeView.BadgeLabelView.StrokeThickness = (double)newValue;
			badgeSettings.BadgeView.InvalidateDrawable();
		}
	}

	private static void OnTextPaddingPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
	{
		if (bindable is BadgeSettings badgeSettings && badgeSettings.BadgeView != null && badgeSettings.BadgeView.BadgeLabelView != null)
		{
			badgeSettings.BadgeView.BadgeLabelView.TextPadding = (Thickness)newValue;
			badgeSettings.BadgeView.BadgeLabelView.CalculateBadgeBounds();
			badgeSettings.BadgeView.InvalidateDrawable();
		}
	}

	private static void OnOffsetPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
	{
		if (bindable is BadgeSettings badgeSettings && badgeSettings.BadgeView != null && badgeSettings.BadgeView.BadgeLabelView != null)
		{
			Point offset = (Point)newValue;
			UpdateOffset(badgeSettings.Position, badgeSettings.BadgeView, offset);
		}
	}

	private static void OnCornerRadiusPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
	{
		if (bindable is BadgeSettings badgeSettings && badgeSettings.BadgeView != null && badgeSettings.BadgeView.BadgeLabelView != null)
		{
			badgeSettings.BadgeView.BadgeLabelView.CornerRadius = (CornerRadius)newValue;
			badgeSettings.BadgeView.InvalidateDrawable();
		}
	}

	private static void OnBadgePositionPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
	{
		if (bindable is BadgeSettings badgeSettings && badgeSettings.BadgeView != null && badgeSettings.BadgeView.BadgeLabelView != null)
		{
			BadgePosition position = (BadgePosition)newValue;
			badgeSettings.BadgeView.BadgeLabelView.BadgePosition = (BadgePosition)newValue;
			badgeSettings.AlignBadge(badgeSettings.BadgeView, position);
		}
	}

	private static void OnBadgeTypePropertyChanged(BindableObject bindable, object? oldValue, object newValue)
	{
		if (bindable is BadgeSettings badgeSettings && badgeSettings.BadgeView != null && badgeSettings.BadgeView.BadgeLabelView != null && (BadgeType)newValue != BadgeType.None)
		{
			badgeSettings.BadgeView.BadgeLabelView.BadgeBackground = badgeSettings.GetBadgeBackground((BadgeType)newValue);
			badgeSettings.BadgeView.InvalidateDrawable();
		}
	}

	private static void OnBadgeAnimationPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
	{
		if (bindable is BadgeSettings badgeSettings && badgeSettings.BadgeView != null && badgeSettings.BadgeView.BadgeLabelView != null)
		{
			if ((BadgeAnimation)newValue == BadgeAnimation.Scale)
			{
				badgeSettings.BadgeView.BadgeLabelView.AnimationEnabled = true;
			}
			else
			{
				badgeSettings.BadgeView.BadgeLabelView.AnimationEnabled = false;
			}
		}
	}

	private static void OnBadgeIconPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
	{
		if (bindable is BadgeSettings badgeSettings && badgeSettings.BadgeView != null && badgeSettings.BadgeView.BadgeLabelView != null)
		{
			badgeSettings.BadgeView.BadgeLabelView.BadgeIcon = (BadgeIcon)newValue;
			badgeSettings.BadgeView.BadgeLabelView.CalculateBadgeBounds();
			badgeSettings.BadgeView.InvalidateDrawable();
		}
	}

	private static void OnAutoHidePropertyChanged(BindableObject bindable, object? oldValue, object newValue)
	{
		if (bindable is BadgeSettings badgeSettings && badgeSettings.BadgeView != null && badgeSettings.BadgeView.BadgeLabelView != null)
		{
			badgeSettings.BadgeView.BadgeLabelView.AutoHide = (bool)newValue;
		}
	}

	private static void OnBadgeAlignmentPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
	{
		if (bindable is BadgeSettings badgeSettings && badgeSettings.BadgeView != null && badgeSettings.BadgeView.BadgeLabelView != null)
		{
			BadgeAlignment alignment = (BadgeAlignment)newValue;
			badgeSettings.UpdateBadgeAlignment(badgeSettings, alignment);
		}
	}

	private static Thickness GetTextPadding()
	{
		if (DeviceInfo.Platform == DevicePlatform.WinUI)
		{
			return new Thickness(3.0);
		}
		return new Thickness(5.0);
	}

	private void UpdateFontAttributes(FontAttributes newValue)
	{
		if (BadgeView != null && BadgeView.BadgeLabelView != null)
		{
			BadgeView.BadgeLabelView.FontAttributes = newValue;
			BadgeView.InvalidateDrawable();
			BadgeView.BadgeLabelView.CalculateBadgeBounds();
		}
	}

	private void UpdateFontFamily(string newValue)
	{
		if (BadgeView != null && BadgeView.BadgeLabelView != null)
		{
			BadgeView.BadgeLabelView.FontFamily = newValue;
			BadgeView.InvalidateDrawable();
			BadgeView.BadgeLabelView.CalculateBadgeBounds();
		}
	}

	private void UpdateFontSize(double newValue)
	{
		if (BadgeView != null && BadgeView.BadgeLabelView != null)
		{
			BadgeView.BadgeLabelView.FontSize = (float)newValue;
			BadgeView.InvalidateDrawable();
			BadgeView.BadgeLabelView.CalculateBadgeBounds();
		}
	}

	internal void UpdateBadgeAlignment(BadgeSettings settings, BadgeAlignment alignment)
	{
		if (settings.BadgeView == null || settings.BadgeView.BadgeLabelView == null)
		{
			return;
		}
		double num = ((settings.BadgeView.WidthRequest >= 0.0) ? settings.BadgeView.WidthRequest : 0.0);
		double num2 = ((settings.BadgeView.HeightRequest >= 0.0) ? settings.BadgeView.HeightRequest : 0.0);
		double num3 = ((settings.BadgeView.BadgeLabelView.WidthRequest >= 0.0) ? settings.BadgeView.BadgeLabelView.WidthRequest : 0.0);
		double num4 = ((settings.BadgeView.BadgeLabelView.HeightRequest >= 0.0) ? settings.BadgeView.BadgeLabelView.HeightRequest : 0.0);
		switch (alignment)
		{
		case BadgeAlignment.Center:
		{
			View content5 = settings.BadgeView.Content;
			if (content5 != null && content5.WidthRequest < 0.0)
			{
				View content6 = settings.BadgeView.Content;
				if (content6 != null && content6.HeightRequest < 0.0)
				{
					if (settings.Position == BadgePosition.Left || settings.Position == BadgePosition.Right)
					{
						UpdateContentSize(settings.BadgeView, num3 / 2.0, 0.0);
					}
					else if (settings.Position == BadgePosition.Top || settings.Position == BadgePosition.Bottom)
					{
						UpdateContentSize(settings.BadgeView, 0.0, num4 / 2.0);
					}
					else
					{
						UpdateContentSize(settings.BadgeView, num3 / 2.0, num4 / 2.0);
					}
					return;
				}
			}
			if (settings.BadgeView.WidthRequest < 0.0 && settings.BadgeView.HeightRequest < 0.0 && settings.BadgeView.Content != null)
			{
				if (settings.Position == BadgePosition.Left || settings.Position == BadgePosition.Right)
				{
					UpdateControlSize(settings.BadgeView, settings.BadgeView.Content.WidthRequest + num3 / 2.0, settings.BadgeView.Content.HeightRequest);
				}
				else if (settings.Position == BadgePosition.Top || settings.Position == BadgePosition.Bottom)
				{
					UpdateControlSize(settings.BadgeView, settings.BadgeView.Content.WidthRequest, settings.BadgeView.Content.HeightRequest + num4 / 2.0);
				}
				else
				{
					UpdateControlSize(settings.BadgeView, settings.BadgeView.Content.WidthRequest + num3 / 2.0, settings.BadgeView.Content.HeightRequest + num4 / 2.0);
				}
			}
			return;
		}
		case BadgeAlignment.Start:
		{
			View content = settings.BadgeView.Content;
			if (content != null && content.WidthRequest < 0.0)
			{
				View content2 = settings.BadgeView.Content;
				if (content2 != null && content2.HeightRequest < 0.0)
				{
					if (settings.Position == BadgePosition.Left || settings.Position == BadgePosition.Right)
					{
						UpdateContentSize(settings.BadgeView, num3, 0.0);
					}
					else if (settings.Position == BadgePosition.Top || settings.Position == BadgePosition.Bottom)
					{
						UpdateContentSize(settings.BadgeView, 0.0, num4);
					}
					else
					{
						UpdateContentSize(settings.BadgeView, num3, num4);
					}
				}
			}
			View content3 = settings.BadgeView.Content;
			if (content3 == null || !(content3.WidthRequest > 0.0))
			{
				return;
			}
			View content4 = settings.BadgeView.Content;
			if (content4 != null && content4.HeightRequest > 0.0)
			{
				if (settings.Position == BadgePosition.Left || settings.Position == BadgePosition.Right)
				{
					UpdateControlSize(settings.BadgeView, settings.BadgeView.Content.WidthRequest + num3, settings.BadgeView.Content.HeightRequest);
				}
				else if (settings.Position == BadgePosition.Top || settings.Position == BadgePosition.Bottom)
				{
					UpdateControlSize(settings.BadgeView, settings.BadgeView.Content.WidthRequest, settings.BadgeView.Content.HeightRequest + num4);
				}
				else
				{
					UpdateControlSize(settings.BadgeView, settings.BadgeView.Content.WidthRequest + num3, settings.BadgeView.Content.HeightRequest + num4);
				}
			}
			return;
		}
		}
		if (settings.BadgeView.Content != null && num > 0.0)
		{
			settings.BadgeView.Content.WidthRequest = num;
		}
		if (settings.BadgeView.Content != null && num2 > 0.0)
		{
			settings.BadgeView.Content.HeightRequest = num2;
		}
		View content7 = settings.BadgeView.Content;
		if (content7 != null && content7.WidthRequest > 0.0)
		{
			View content8 = settings.BadgeView.Content;
			if (content8 != null && content8.HeightRequest > 0.0)
			{
				UpdateControlSize(settings.BadgeView, settings.BadgeView.Content.WidthRequest, settings.BadgeView.Content.HeightRequest);
			}
		}
	}

	private static void UpdateContentSize(SfBadgeView badgeView, double badgeWidth, double badgeHeight)
	{
		double widthRequest = badgeView.WidthRequest;
		double heightRequest = badgeView.HeightRequest;
		if (badgeView.Content != null && widthRequest - badgeWidth > 0.0)
		{
			badgeView.Content.WidthRequest = widthRequest - badgeWidth;
		}
		else if (badgeView.Content != null && badgeView.Content.WidthRequest - badgeWidth > 0.0)
		{
			badgeView.Content.WidthRequest -= badgeWidth;
		}
		if (badgeView.Content != null && heightRequest - badgeHeight > 0.0)
		{
			badgeView.Content.HeightRequest = heightRequest - badgeHeight;
		}
		else if (badgeView.Content != null && badgeView.Content.HeightRequest - badgeHeight > 0.0)
		{
			badgeView.Content.HeightRequest -= badgeHeight;
		}
		badgeView.BadgeSettings?.UpdateContentLayout(badgeWidth, badgeHeight);
	}

	private void UpdateContentLayout(double badgeWidth, double badgeHeight)
	{
		if (BadgeView?.Content != null)
		{
			if (Position == BadgePosition.Left || Position == BadgePosition.BottomLeft)
			{
				AbsoluteLayout.SetLayoutBounds((BindableObject)BadgeView.Content, new Rect(badgeWidth, 0.0, BadgeView.Content.WidthRequest, BadgeView.Content.HeightRequest));
			}
			else if (Position == BadgePosition.Top || Position == BadgePosition.TopRight)
			{
				AbsoluteLayout.SetLayoutBounds((BindableObject)BadgeView.Content, new Rect(0.0, badgeHeight, BadgeView.Content.WidthRequest, BadgeView.Content.HeightRequest));
			}
			else if (Position == BadgePosition.Right || Position == BadgePosition.Bottom || Position == BadgePosition.BottomRight)
			{
				AbsoluteLayout.SetLayoutBounds((BindableObject)BadgeView.Content, new Rect(0.0, 0.0, BadgeView.Content.WidthRequest, BadgeView.Content.HeightRequest));
			}
			else if (Position == BadgePosition.TopLeft)
			{
				AbsoluteLayout.SetLayoutBounds((BindableObject)BadgeView.Content, new Rect(badgeWidth, badgeHeight, BadgeView.Content.WidthRequest, BadgeView.Content.HeightRequest));
			}
		}
	}

	private static void UpdateControlSize(SfBadgeView badgeView, double width, double height)
	{
		double widthRequest = badgeView.WidthRequest;
		double heightRequest = badgeView.HeightRequest;
		if (widthRequest < 0.0 && width > 0.0)
		{
			badgeView.WidthRequest = width;
		}
		if (heightRequest < 0.0 && height > 0.0)
		{
			badgeView.HeightRequest = height;
		}
	}
}
