using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core;

internal class TooltipFont : ITextElement
{
	private Microsoft.Maui.Font font;

	private double fontSize = 14.0;

	private string fontFamily = string.Empty;

	private FontAttributes fontAttributes = FontAttributes.None;

	private Color textColor = Colors.White;

	public Microsoft.Maui.Font Font
	{
		get
		{
			return font;
		}
		set
		{
			if (font != value)
			{
				font = value;
			}
		}
	}

	public double FontSize
	{
		get
		{
			return fontSize;
		}
		set
		{
			if (fontSize != value)
			{
				fontSize = value;
			}
			Font = font.WithSize(fontSize);
		}
	}

	public string FontFamily
	{
		get
		{
			return fontFamily;
		}
		set
		{
			if (fontFamily != value)
			{
				fontFamily = value;
			}
			Font = Microsoft.Maui.Font.OfSize(fontFamily, fontSize, FontWeight.Regular, FontSlant.Default, enableScaling: false).WithAttributes(fontAttributes);
		}
	}

	public FontAttributes FontAttributes
	{
		get
		{
			return fontAttributes;
		}
		set
		{
			if (fontAttributes != value)
			{
				fontAttributes = value;
			}
			Font = font.WithAttributes(fontAttributes);
		}
	}

	public Color TextColor
	{
		get
		{
			return textColor;
		}
		set
		{
			if (textColor != value)
			{
				textColor = value;
			}
		}
	}

	internal TooltipFont(ITextElement element)
	{
		Font = element.Font;
		FontAttributes = element.FontAttributes;
		FontFamily = element.FontFamily;
		FontSize = element.FontSize;
		TextColor = element.TextColor;
	}

	double ITextElement.FontSizeDefaultValueCreator()
	{
		if (FontSize <= 0.0)
		{
			return 0.1;
		}
		return FontSize;
	}

	void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
	{
	}

	void ITextElement.OnFontChanged(Microsoft.Maui.Font oldValue, Microsoft.Maui.Font newValue)
	{
	}

	void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
	{
	}

	void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
	{
	}
}
