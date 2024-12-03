using System.Collections;
using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

public interface IDrawableLayout : IDrawable, IAbsoluteLayout, ILayout, IView, IElement, ITransform, IContainer, IList<IView>, ICollection<IView>, IEnumerable<IView>, IEnumerable, ISafeAreaView, IPadding
{
	DrawingOrder DrawingOrder { get; set; }

	void InvalidateDrawable();
}
