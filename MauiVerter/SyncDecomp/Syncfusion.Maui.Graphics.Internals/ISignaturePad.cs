using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Graphics.Internals;

public interface ISignaturePad : IView, IElement, ITransform
{
	double MaximumStrokeThickness { get; set; }

	double MinimumStrokeThickness { get; set; }

	Color StrokeColor { get; set; }

	new Brush Background { get; set; }

	ImageSource? ToImageSource();

	void Clear();

	bool StartInteraction();

	void EndInteraction();
}
