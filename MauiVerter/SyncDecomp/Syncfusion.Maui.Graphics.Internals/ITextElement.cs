using System.ComponentModel;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Graphics.Internals;

[EditorBrowsable(EditorBrowsableState.Never)]
public interface ITextElement
{
	FontAttributes FontAttributes { get; }

	string FontFamily { get; }

	[TypeConverter(typeof(FontSizeConverter))]
	double FontSize { get; }

	Microsoft.Maui.Font Font { get; }

	Color TextColor { get; set; }

	void OnFontFamilyChanged(string oldValue, string newValue);

	void OnFontSizeChanged(double oldValue, double newValue);

	double FontSizeDefaultValueCreator();

	void OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue);

	void OnFontChanged(Microsoft.Maui.Font oldValue, Microsoft.Maui.Font newValue);
}
