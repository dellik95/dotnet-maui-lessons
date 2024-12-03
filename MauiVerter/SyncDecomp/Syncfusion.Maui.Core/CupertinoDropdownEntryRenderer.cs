using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

internal class CupertinoDropdownEntryRenderer : IDropdownRenderer
{
	internal float padding = 10f;

	public void DrawBorder(ICanvas canvas, RectF rectF, bool isFocused, Color borderColor, Color focusedBorderColor)
	{
		if (!isFocused)
		{
			canvas.StrokeColor = borderColor;
			canvas.StrokeSize = 1f;
		}
		else
		{
			canvas.StrokeColor = focusedBorderColor;
			canvas.StrokeSize = 2f;
		}
		canvas.SaveState();
		canvas.DrawRoundedRectangle(rectF, 6f);
		canvas.ResetStroke();
	}

	public void DrawClearButton(ICanvas canvas, RectF rectF)
	{
		PointF point = new Point(0.0, 0.0);
		PointF point2 = new Point(0.0, 0.0);
		point.X = rectF.X + padding;
		point.Y = rectF.Y + padding;
		point2.X = rectF.X + rectF.Width - padding;
		point2.Y = rectF.Y + rectF.Height - padding;
		canvas.DrawLine(point, point2);
		point.X = rectF.X + padding;
		point.Y = rectF.Y + rectF.Height - padding;
		point2.X = rectF.X + rectF.Width - padding;
		point2.Y = rectF.Y + padding;
		canvas.DrawLine(point, point2);
	}

	public void DrawDropDownButton(ICanvas canvas, RectF rectF)
	{
		float num = rectF.Width / 2f - padding / 2f;
		rectF.X = rectF.Center.X - num / 2f;
		rectF.Y = rectF.Center.Y - num / 4f;
		rectF.Width = num;
		rectF.Height = num / 2f;
		float x = rectF.X;
		float y = rectF.Y;
		float x2 = x + rectF.Width;
		float y2 = y + rectF.Height;
		float x3 = x + rectF.Width / 2f;
		PathF pathF = new PathF();
		pathF.MoveTo(x, y);
		pathF.LineTo(x3, y2);
		pathF.LineTo(x2, y);
		canvas.DrawPath(pathF);
	}
}
