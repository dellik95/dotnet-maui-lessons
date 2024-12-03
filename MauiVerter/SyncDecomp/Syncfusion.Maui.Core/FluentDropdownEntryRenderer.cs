using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core;

internal class FluentDropdownEntryRenderer : IDropdownRenderer
{
	internal float padding = 12f;

	public void DrawBorder(ICanvas canvas, RectF rectF, bool isFocused, Color borderColor, Color focusedBorderColor)
	{
		canvas.StrokeColor = Color.FromRgba(240, 240, 240, 255);
		canvas.StrokeSize = 2f;
		canvas.DrawRoundedRectangle(rectF, 5f);
		canvas.ResetStroke();
		if (isFocused)
		{
			canvas.StrokeColor = focusedBorderColor;
			canvas.DrawLine(rectF.X + 2f, rectF.Y + rectF.Height - 1.5f, rectF.X + rectF.Width - 2f, rectF.Height - 1.5f);
		}
		else
		{
			canvas.StrokeColor = borderColor;
			canvas.DrawLine(rectF.X + 2f, rectF.Y + rectF.Height - 1f, rectF.X + rectF.Width - 2f, rectF.Height - 1f);
		}
		canvas.StrokeSize = 0.6f;
		canvas.DrawLine(rectF.X + 3f, rectF.Y + rectF.Height - 0.6f, rectF.X + rectF.Width - 3f, rectF.Height - 0.6f);
		canvas.DrawLine(rectF.X + 4f, rectF.Y + rectF.Height, rectF.X + rectF.Width - 4f, rectF.Height);
		canvas.StrokeSize = 1f;
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
