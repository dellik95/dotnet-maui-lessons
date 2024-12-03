using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Graphics.Internals;

public interface ITextMeasurer
{
	Size MeasureText(string text, float textSize, FontAttributes attributes = FontAttributes.None, string? fontFamily = null);

	Size MeasureText(string text, ITextElement textElement);
}
