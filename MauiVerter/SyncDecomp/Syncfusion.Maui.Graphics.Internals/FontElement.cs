using System.ComponentModel;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Graphics.Internals;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class FontElement
{
	public static readonly BindableProperty FontProperty = BindableProperty.Create("Font", typeof(Font), typeof(ITextElement), default(Font), BindingMode.OneWay, null, OnFontPropertyChanged);

	public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create("FontFamily", typeof(string), typeof(ITextElement), null, BindingMode.OneWay, null, OnFontFamilyChanged);

	public static readonly BindableProperty FontSizeProperty = BindableProperty.Create("FontSize", typeof(double), typeof(ITextElement), -1.0, BindingMode.OneWay, null, OnFontSizeChanged, null, null, FontSizeDefaultValueCreator);

	public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create("FontAttributes", typeof(FontAttributes), typeof(ITextElement), FontAttributes.None, BindingMode.OneWay, null, OnFontAttributesChanged);

	private static readonly BindableProperty CancelEventsProperty = BindableProperty.Create("CancelEvents", typeof(bool), typeof(FontElement), false);

	private static bool GetCancelEvents(BindableObject bindable)
	{
		return (bool)bindable.GetValue(CancelEventsProperty);
	}

	private static void SetCancelEvents(BindableObject bindable, bool value)
	{
		bindable.SetValue(CancelEventsProperty, value);
	}

	private static void OnFontPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (!GetCancelEvents(bindable))
		{
			SetCancelEvents(bindable, value: true);
			Font font = (Font)newValue;
			if (font == Font.Default)
			{
				bindable.ClearValue(FontFamilyProperty);
				bindable.ClearValue(FontSizeProperty);
				bindable.ClearValue(FontAttributesProperty);
			}
			else
			{
				bindable.SetValue(FontFamilyProperty, font.Family);
				bindable.SetValue(FontSizeProperty, font.Size);
				bindable.SetValue(FontAttributesProperty, font.GetFontAttributes());
			}
			SetCancelEvents(bindable, value: false);
		}
	}

	private static bool OnChanged(BindableObject bindable)
	{
		if (GetCancelEvents(bindable))
		{
			return false;
		}
		ITextElement textElement = (ITextElement)bindable;
		SetCancelEvents(bindable, value: true);
		bindable.SetValue(FontProperty, Font.OfSize(textElement.FontFamily, textElement.FontSize, FontWeight.Regular, FontSlant.Default, enableScaling: false).WithAttributes(textElement.FontAttributes));
		SetCancelEvents(bindable, value: false);
		return true;
	}

	private static void OnFontFamilyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (OnChanged(bindable))
		{
			((ITextElement)bindable).OnFontFamilyChanged((string)oldValue, (string)newValue);
		}
	}

	private static void OnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (OnChanged(bindable))
		{
			((ITextElement)bindable).OnFontSizeChanged((double)oldValue, (double)newValue);
		}
	}

	private static object FontSizeDefaultValueCreator(BindableObject bindable)
	{
		return ((ITextElement)bindable).FontSizeDefaultValueCreator();
	}

	private static void OnFontAttributesChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (OnChanged(bindable))
		{
			((ITextElement)bindable).OnFontAttributesChanged((FontAttributes)oldValue, (FontAttributes)newValue);
		}
	}
}
