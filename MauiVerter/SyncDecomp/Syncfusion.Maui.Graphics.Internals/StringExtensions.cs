using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Graphics.Internals;

public static class StringExtensions
{
	public static Size Measure(this string text, float textSize, FontAttributes attributes = FontAttributes.None, string? fontFamily = null)
	{
		return TextMeasurer.Instance.MeasureText(text, textSize, attributes, fontFamily);
	}

	public static Size Measure(this string text, ITextElement textElement)
	{
		return TextMeasurer.Instance.MeasureText(text, textElement);
	}
}
