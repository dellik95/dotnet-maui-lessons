using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

internal class MaterialDropdownEntryRenderer : IDropdownRenderer
{
	internal float padding = 7f;

	internal float clearButtonPadding = 9f;

	public void DrawBorder(ICanvas canvas, RectF rectF, bool isFocused, Color borderColor, Color focusedBorderColor)
	{
		if (isFocused)
		{
			canvas.StrokeColor = focusedBorderColor;
			canvas.StrokeSize = 2f;
		}
		else
		{
			canvas.StrokeSize = 1f;
			canvas.StrokeColor = borderColor;
		}
		canvas.DrawLine(rectF.X + 4f, rectF.Y + rectF.Height - 7f, rectF.X + rectF.Width - 4f, rectF.Height - 7f);
		canvas.StrokeSize = 1f;
	}

	public void DrawClearButton(ICanvas canvas, RectF rectF)
	{
		PointF point = new Point(0.0, 0.0);
		PointF point2 = new Point(0.0, 0.0);
		point.X = rectF.X + clearButtonPadding;
		point.Y = rectF.Y + clearButtonPadding;
		point2.X = rectF.X + rectF.Width - clearButtonPadding;
		point2.Y = rectF.Y + rectF.Height - clearButtonPadding;
		canvas.DrawLine(point, point2);
		point.X = rectF.X + clearButtonPadding;
		point.Y = rectF.Y + rectF.Height - clearButtonPadding;
		point2.X = rectF.X + rectF.Width - clearButtonPadding;
		point2.Y = rectF.Y + clearButtonPadding;
		canvas.DrawLine(point, point2);
	}

	public void DrawDropDownButton(ICanvas canvas, RectF rectF)
	{
		rectF.X = rectF.Center.X - padding;
		rectF.Y = rectF.Center.Y - padding / 2f;
		rectF.Width = padding * 2f;
		rectF.Height = padding;
		float x = rectF.X;
		float y = rectF.Y;
		float x2 = x + rectF.Width;
		float y2 = y + rectF.Height;
		float x3 = x + rectF.Width / 2f;
		PathF pathF = new PathF();
		pathF.MoveTo(x, y);
		pathF.LineTo(x2, y);
		pathF.LineTo(x3, y2);
		pathF.LineTo(x, y);
		pathF.Close();
		canvas.FillPath(pathF);
	}
}
