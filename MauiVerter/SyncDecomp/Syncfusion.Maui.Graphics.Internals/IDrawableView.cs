using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Graphics.Internals;

public interface IDrawableView : IView, IElement, ITransform, IDrawable
{
	void InvalidateDrawable();
}
