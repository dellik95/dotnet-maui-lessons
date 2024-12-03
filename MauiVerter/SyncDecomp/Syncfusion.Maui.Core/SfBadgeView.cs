using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

[DesignTimeVisible(true)]
[ContentProperty("Content")]
public class SfBadgeView : SfContentView
{
	public static readonly BindableProperty ScreenReaderTextProperty = BindableProperty.Create(nameof(ScreenReaderText), typeof(string), typeof(SfBadgeView), string.Empty, BindingMode.OneWay, null, OnScreenReaderTextPropertyChanged);

	public new static readonly BindableProperty ContentProperty = BindableProperty.Create("Content", typeof(View), typeof(SfBadgeView), null, BindingMode.OneWay, null, OnContentPropertyChanged);

	public static readonly BindableProperty BadgeTextProperty = BindableProperty.Create(nameof(BadgeText), typeof(string), typeof(SfBadgeView), string.Empty, BindingMode.OneWay, null, OnBadgeTextPropertyChanged);

	public static readonly BindableProperty BadgeSettingsProperty = BindableProperty.Create(nameof(BadgeSettings), typeof(BadgeSettings), typeof(SfBadgeView), null, BindingMode.OneWay, null, OnBadgeSettingsPropertyChanged, null, null, GetBadgeSettingsDefaultValue);

	private BadgeLabelView? badgeLabelView;

	public string ScreenReaderText
	{
		get
		{
			return (string)GetValue(ScreenReaderTextProperty);
		}
		set
		{
			SetValue(ScreenReaderTextProperty, value);
		}
	}

	public string BadgeText
	{
		get
		{
			return (string)GetValue(BadgeTextProperty);
		}
		set
		{
			SetValue(BadgeTextProperty, value);
		}
	}

	public BadgeSettings? BadgeSettings
	{
		get
		{
			return (BadgeSettings)GetValue(BadgeSettingsProperty);
		}
		set
		{
			SetValue(BadgeSettingsProperty, value);
		}
	}

	internal BadgeLabelView? BadgeLabelView
	{
		get
		{
			return badgeLabelView;
		}
		set
		{
			badgeLabelView = value;
		}
	}

	public SfBadgeView()
	{
		InitializeBadgeLayer();
	}

	internal void InitializeBadgeLayer()
	{
		if (BadgeLabelView == null)
		{
			BadgeLabelView badgeLabelView2 = (BadgeLabelView = new BadgeLabelView(this));
		}
		BadgeLabelView.ScreenReaderText = ScreenReaderText;
		BadgeLabelView.Text = BadgeText;
		base.DrawingOrder = DrawingOrder.AboveContent;
		base.Content = base.Content;
		if (BadgeSettings == null)
		{
			BadgeSettings = new BadgeSettings();
		}
		else
		{
			BadgeSettings.ApplySettingstoUpdatedBadgeView();
		}
		BadgeLabelView.TextElement = BadgeSettings;
	}

	private static object? GetBadgeSettingsDefaultValue(BindableObject bindable)
	{
		BadgeSettings badgeSettings = new BadgeSettings();
		badgeSettings.BadgeView = bindable as SfBadgeView;
		return badgeSettings;
	}

	protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		if (propertyName == "ImplicitStyle")
		{
			InitializeBadgeLayer();
		}
		base.OnPropertyChanged(propertyName);
	}

	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();
		if (BadgeSettings != null)
		{
			BindableObject.SetInheritedBindingContext(BadgeSettings, base.BindingContext);
		}
	}

	protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
	{
		base.OnDraw(canvas, dirtyRect);
		BadgeSettings?.AlignBadgeText(this, BadgeSettings.Position);
		badgeLabelView?.DrawBadge(canvas);
	}

	protected override Size ArrangeContent(Rect bounds)
	{
		BadgeSettings?.AlignBadge(this, BadgeSettings.Position);
		return base.ArrangeContent(bounds);
	}

	private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfBadgeView sfBadgeView)
		{
			sfBadgeView.Content = (View)newValue;
			sfBadgeView.InitializeBadgeLayer();
		}
	}

	private static void OnScreenReaderTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfBadgeView sfBadgeView && sfBadgeView.BadgeLabelView != null)
		{
			sfBadgeView.BadgeLabelView.ScreenReaderText = (string)newValue;
		}
	}

	private static void OnBadgeTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is SfBadgeView sfBadgeView && sfBadgeView.BadgeLabelView != null)
		{
			sfBadgeView.BadgeLabelView.Text = (string)newValue;
			sfBadgeView.BadgeLabelView.CalculateBadgeBounds();
			sfBadgeView.InvalidateDrawable();
		}
	}

	private static void OnBadgeSettingsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (oldValue != null && oldValue is BadgeSettings badgeSettings)
		{
			badgeSettings.BindingContext = null;
			badgeSettings.Parent = null;
		}
		if (newValue != null && newValue is BadgeSettings badgeSettings2 && bindable is SfBadgeView sfBadgeView)
		{
			badgeSettings2.Parent = sfBadgeView;
			if (sfBadgeView.BadgeLabelView != null)
			{
				sfBadgeView.BadgeLabelView.TextElement = badgeSettings2;
			}
			BindableObject.SetInheritedBindingContext(sfBadgeView.BadgeSettings, sfBadgeView.BindingContext);
		}
	}
}
