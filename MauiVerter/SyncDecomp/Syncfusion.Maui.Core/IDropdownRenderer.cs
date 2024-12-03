using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

internal interface IDropdownRenderer
{
	void DrawDropDownButton(ICanvas canvas, RectF rectF);

	void DrawClearButton(ICanvas canvas, RectF rectF);

	void DrawBorder(ICanvas canvas, RectF rectF, bool isFocused, Color borderColor, Color focusedBorderColor);
}
