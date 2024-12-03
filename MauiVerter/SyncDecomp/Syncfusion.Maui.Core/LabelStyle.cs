using System.ComponentModel;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core;

public class LabelStyle : BindableObject, ITextElement
{
	public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(LabelStyle), Color.FromRgba(0.0, 0.0, 0.0, 0.87));

	public static readonly BindableProperty FontSizeProperty = FontElement.FontSizeProperty;

	public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

	public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

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

	double ITextElement.FontSizeDefaultValueCreator()
	{
		return 12.0;
	}

	void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
	{
	}

	void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
	{
	}

	void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
	{
	}

	void ITextElement.OnFontChanged(Microsoft.Maui.Font oldValue, Microsoft.Maui.Font newValue)
	{
	}
}
